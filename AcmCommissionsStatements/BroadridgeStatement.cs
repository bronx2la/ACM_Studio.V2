using System;
using System.Collections.Generic;
using System.Linq;
using Aristotle.Excel;
using CommissionsStatementGenerator;
using Core;
using Core.DataModels;
using Core.DataModels.Broadridge;
using Core.DataModels.Broadridge.Core.DataModels.Broadridge;
using Core.Enums;
using Dapper;
using DapperDatabaseAccess;
using OfficeOpenXml.FormulaParsing.Utilities;

namespace AcmCommissionsStatements
{
        public class BroadridgeStatement : CommissionStatementBase
    {
        private IEnumerable<BroadridgeSales> _broadridgeSales;
        private IEnumerable<BroadridgeUmaAssets> _broadridgeAssets;
        private bool _isUma;

        private IEnumerable<BroadridgeOtherNewAssetsDataModel> naDetail;
        private IEnumerable<BroadridgeNewAssetsSummaryDataModel> naSummary;
        private IEnumerable<BroadridgeOtherOgDetailDataModel> ogDetail;
        private IEnumerable<BroadridgeOgSummaryDataModel> ogSummary;
        
        public BroadridgeStatement(string startDate, string endDate, string connectionString, TaskMetaDataDataModel metaData, bool isUma) : base(startDate, endDate, connectionString, metaData)
        {
            _isUma = isUma;
            InitializeSalesData(endDate);
            InitializeAssetsData(endDate);
        }

        protected void InitializeSalesData(string endDate)
        {
            var dal = new DapperDatabaseAccess<BroadridgeSales>(_connectionString);
            
            var parms = new DynamicParameters();
            parms.Add("@ReportingDate", endDate);
            parms.Add("@IsUma", _isUma);

            _broadridgeSales = dal.SqlServerFetch("dbo.prc_BroadridgeSales_Fetch", parms);
        }

        protected void InitializeAssetsData(string endDate)
        {
            var dal = new DapperDatabaseAccess<BroadridgeUmaAssets>(_connectionString);
            
            var parms = new DynamicParameters();
            parms.Add("@ReportingDate", endDate);

            _broadridgeAssets = dal.SqlServerFetch("dbo.prc_BroadridgeAssets_Fetch", parms);
        }

        public void ProduceNonMerrillMorganReport()
        {
            _workbookFile = $@"{_metaData.outboundFolder}\{Core.FileNameHelpers.FormatOutboundFileName(_metaData.outboundFile)}";
            _xl           = new AristotleExcel(_workbookFile);

            naDetail = BuildUmaNewAssetsDetail();
            naSummary = BuildUmaNewAssetsSummary();
            ogDetail = BuildUmaOngoingDetail();
            ogSummary = BuildOngoingSummary();
            
            _xl.AddWorksheet(naSummary, "NewAssets_Summary", ExcelColumnProperties("BroadridgeUma.NewAssetsSummary"));
            _xl.AddWorksheet(ogSummary, "Ongoing_Summary", ExcelColumnProperties("BroadridgeUma.OngoingSummary"));
            _xl.AddWorksheet(naDetail, "NewAssets_Detail", ExcelColumnProperties("BroadridgeUma.NewAssetsDetail"));
            _xl.AddWorksheet(ogDetail, "Ongoing_Detail", ExcelColumnProperties("BroadridgeUma.OngoingDetail"));
            _xl.SaveWorkbook();
        }

        private IEnumerable<BroadridgeOtherNewAssetsDataModel> BuildUmaNewAssetsDetail()
        {
            const LkuRateType.RateType rateType= LkuRateType.RateType.NewAssets;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;
            var naItems = new List<BroadridgeOtherNewAssetsDataModel>();

            var newTypes = new List<NewAssetTradeType>();
            var n = new NewAssetTradeType("Purchase");
            newTypes.Add(n);
            
            n = new NewAssetTradeType("Transfer In");
            newTypes.Add(n);
            
            var _sales = _broadridgeSales.Where(c => newTypes.Select(r => r.TradeType).Contains(c.TransactionCodeOverrideDescription));
            foreach (var item in _sales)
            {
                if (item.Territory != null)
                {
                    RegionalDirectorRateInfoDataModel rdRateInfo = RegionalDirectorRateInfo(item.Territory, (int)rateType, (int)commissionType);

                    if (rdRateInfo != null)
                    {
                        rdRateInfo.Rate = item.Territory.IndexOf(',') > 0
                            ? rdRateInfo.Rate / 2
                            : rdRateInfo.Rate;
                    }
                    
                    var rd = _regionalDirector.FirstOrDefault(r => r.LastName == item.Territory);
                    var rateInfo = GetRateInfo(rd == null ? "House" : rd.RegionalDirectorKey, item.ProductName, false);
                    var theRate = rateInfo?.NewAssetRate ?? 0.0m;

                    var na = new BroadridgeOtherNewAssetsDataModel()
                    {
                        TradeId              = item.TradeId,
                        TransactionCodeOverrideDescription = item.TransactionCodeOverrideDescription,
                        TradeDate = item.TradeDate,
                        SettledDate = item.SettledDate,
                        SuperSheetDate = item.SuperSheetDate,
                        TradeAmount = item.TradeAmount,
                        Commission = item.TradeAmount * theRate,
                        Rate = theRate,
                        System = item.System,
                        DealerNum = item.DealerNum,
                        DealerBranchCode = item.DealerBranchBranchCode,
                        RepCode = item.RepCode,
                        FirmId                 = item.FirmId,
                        FirmName               = item.FirmName,
                        OfficeAddressLine1     = item.OfficeAddressLine1,
                        OfficeCity             = item.OfficeCity, 
                        OfficeRegionRefCode    = item.OfficeRegionRefCode,
                        PersonFirstName        = item.PersonFirstName,
                        PersonLastName         = item.PersonLastName,
                        LineOfBusiness = item.LineOfBusiness,
                        Channel = item.Channel,
                        Region = item.Region,
                        Territory              = item.Territory,
                        ProductNasdaqSymbol = item.ProductNasdaqSymbol,
                        ProductName            = item.ProductName, 
                        AccountTANumber = item.AccountTANumber,
                        AccountId = item.RepCode,
                        ExternalAccountNumber = item.FirmId,
                        HoldingId = item.HoldingId,
                        HoldingName = item.HoldingName,
                        HoldingExternalAccountNumber = item.HoldingExternalAccountNumber
                    };
                    
                    naItems.Add(na);
                }    
            }

            var y = 0;
            return naItems.OrderBy(c => c.System).ThenBy(c => c.Territory)
                          .ThenBy(c => c.FirmName);
        }

        
        private IEnumerable<BroadridgeNewAssetsSummaryDataModel> BuildUmaNewAssetsSummary()
        {
            const LkuRateType.RateType             rateType       = LkuRateType.RateType.NewAssets;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;
            var naSumm = new List<BroadridgeNewAssetsSummaryDataModel>();

            var naSummWorking = naDetail
                .GroupBy(c => new
                {
                    c.Territory,
                    c.System,
                    c.OfficeAddressLine1,
                    c.OfficeRegionRefCode,
                    c.ProductName,
                    c.FirmName,
                    c.PersonLastName
                })
                .Select(group => new
                {
                    Territory = group.Key.Territory,
                    System = group.Key.System,
                    OfficeAddress = group.Key.OfficeAddressLine1,
                    OfficeState = group.Key.OfficeRegionRefCode,
                    ProductName = group.Key.ProductName,
                    FAName = group.Key.FirmName,
                    PersonLastName = group.Key.PersonLastName,
                    NewAssetValue = group.Sum(c => c.TradeAmount),
                    Commission = group.Sum(c => c.Commission),
                    Rate = group.Min(c => c.Rate)
                });

            foreach (var item in naSummWorking)
            {
                var summary = new BroadridgeNewAssetsSummaryDataModel()
                {
                    TheSystem = item.System,
                    Territory = item.Territory,
                    OfficeAddress = item.OfficeAddress,
                    OfficeState = item.OfficeState,
                    ProductName = item.ProductName,
                    FAName = item.FAName,
                    PersonLastName = item.PersonLastName,
                    NewAssetValue = item.NewAssetValue,
                    Commission = item.Commission,
                    Rate = item.Rate
                };
                naSumm.Add(summary);
            }
            
            // Total Lines
            var naSummTotals = naSumm
                .GroupBy(g => new
                {
                    g.TheSystem,
                    g.Territory
                })
                .Select(group => new
                {
                    System              = group.Key.TheSystem,
                    Territory           = group.Key.Territory,
                    FirmName            = "-----",
                    OfficeRegionRefCode = "-----",
                    MarketValue         = group.Sum(c => c.NewAssetValue),
                    Rate                = group.Min(c=>c.Rate),
                    Commission          = group.Sum(c => c.Commission)
                });

            foreach (var item in naSummTotals)
            {
                var summary = new BroadridgeNewAssetsSummaryDataModel()
                {
                    TheSystem = item.System,
                    Territory = item.Territory,
                    ProductName = "z--Totals",
                    NewAssetValue = item.MarketValue,
                    Rate = item.Rate,
                    Commission = item.Commission
                };
                naSumm.Add(summary);
            }

            return naSumm.OrderBy(c => c.TheSystem).ThenBy(c => c.Territory).ThenBy(c => c.ProductName);
        }

        private IEnumerable<BroadridgeFlows> BuildPseudoFlowDetail()
        {
            const LkuRateType.RateType rateType = LkuRateType.RateType.Ongoing;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;

            var pfItems = new List<BroadridgeFlows>();

            foreach (var asset in _broadridgeAssets)
            {
                RegionalDirectorRateInfoDataModel rdRateInfo = RegionalDirectorRateInfo(asset.Territory, (int)rateType, (int)commissionType);
                if (rdRateInfo != null)
                {
                    rdRateInfo.Rate = asset.Territory.IndexOf(',') > 0
                        ? rdRateInfo.Rate / 2
                        : rdRateInfo.Rate;

                    var rd = _regionalDirector.FirstOrDefault(r => r.LastName == asset.Territory);
                    var rateInfo = GetRateInfo(rd == null ? "House" : rd.RegionalDirectorKey, asset.ProductName, false);
                    var theRate = rateInfo?.NewAssetRate ?? 0.0m;

                    var fa = GetFlowAmount(asset);
                    
                    var pf = new BroadridgeFlows()
                    {
                        TheSystem = asset.System,
                        FirmName = asset.FirmName,
                        FirmId = asset.FirmId,
                        FirmCRDNumber = asset.FirmCRDNumber,
                        Portfolio = asset.HoldingId,
                        HoldingExternalAccountNumber = asset.HoldingExternalAccountNumber,
                        HoldingName = asset.HoldingName,
                        HoldingStartDate = asset.HoldingStartDate,
                        HoldingCreateDate = asset.HoldingCreateDate,
                        Month3AgoAssetBalance = asset.Month3AgoAssetBalance,
                        Month2AgoAssetBalance = asset.Month2AgoAssetBalance,
                        Period1Delta = asset.Month2AgoAssetBalance - asset.Month3AgoAssetBalance,
                        Period1Threshold = asset.Month3AgoAssetBalance * 0.1m,
                        Period1PercentChange = GetPercentDiff(asset.Month2AgoAssetBalance, asset.Month3AgoAssetBalance),
                        Period1Commission = GetPercentDiff(asset.Month2AgoAssetBalance, asset.Month3AgoAssetBalance) > 0.1m ?
                            (asset.Month2AgoAssetBalance - asset.Month3AgoAssetBalance) * theRate :
                            0.0m,
                        Month1AgoAssetBalance = asset.Month1AgoAssetBalance,
                        
                        Period2Delta = asset.Month1AgoAssetBalance - asset.Month2AgoAssetBalance,
                        Period2Threshold = asset.Month2AgoAssetBalance * 0.1m,
                        Period2PercentChange = GetPercentDiff(asset.Month1AgoAssetBalance, asset.Month2AgoAssetBalance),
                        Period2Commission = GetPercentDiff(asset.Month1AgoAssetBalance, asset.Month2AgoAssetBalance) > 0.1m ?
                            (asset.Month1AgoAssetBalance - asset.Month2AgoAssetBalance) * theRate :
                            0.0m,
                        
                        MostRecentMonthAssetBalance = asset.MostRecentMonthAssetBalance,
                        Period3Delta = asset.MostRecentMonthAssetBalance - asset.Month1AgoAssetBalance,
                        Period3Threshold = asset.Month1AgoAssetBalance * 0.1m,
                        Period3PercentChange = GetPercentDiff(asset.MostRecentMonthAssetBalance, asset.Month1AgoAssetBalance),
                        Period3Commission = GetPercentDiff(asset.MostRecentMonthAssetBalance, asset.Month1AgoAssetBalance) > 0.1m ?
                            (asset.MostRecentMonthAssetBalance - asset.Month1AgoAssetBalance) * theRate :
                            0.0m,
                        Rate = theRate,
                        
                        ProductName = asset.ProductName,
                        ProductType = asset.ProductType,
                        Channel = asset.Channel,
                        Region = asset.Channel,
                        Territory = asset.Territory,
                        SalesCredit = asset.Territory,
                        PersonCRDNumber = null,
                        PersonFirstName = asset.PersonFirstName,
                        PersonLastName = asset.PersonLastName,
                        PersonId = asset.PersonId,
                        OfficeAddressLine1 = asset.OfficeAddressLine1,
                        OfficeAddressLine2 = asset.OfficeAddressLine2,
                        OfficeCity = asset.OfficeCity,
                        OfficeRegionRefCode = asset.OfficeRegionRefCode,
                        OfficePostalCode = asset.OfficePostalCode,
                        PersonBrokerTeamFlag = asset.PersonBrokerTeamFlag,
                        
                        Month4AgoAssetBalance = asset.Month4AgoAssetBalance,
                        Month5AgoAssetBalance = asset.Month5AgoAssetBalance,
                        Month6AgoAssetBalance = asset.Month6AgoAssetBalance,
                        Month7AgoAssetBalance = asset.Month7AgoAssetBalance,
                        Month8AgoAssetBalance = asset.Month8AgoAssetBalance,
                        Month9AgoAssetBalance = asset.Month9AgoAssetBalance,
                        Month10AgoAssetBalance = asset.Month10AgoAssetBalance,
                        Month11AgoAssetBalance = asset.Month11AgoAssetBalance,
                        Month12AgoAssetBalance = asset.Month11AgoAssetBalance,
                        
                        HoldingAddressLine1 = asset.HoldingAddressLine1,
                        SystemFAName = $"{asset.PersonFirstName} {asset.PersonLastName}",
                        SystemOfficeAddress = asset.FirmName,
                        SystemOfficeState = asset.OfficeRegionRefCode,
                        SystemQEAssets = asset.MostRecentMonthAssetBalance,
                        AssetCheck = 0.0m,
                        SystemRDCredit = asset.Territory,
                        AccountTANumber = asset.HoldingExternalAccountNumber,
                        ExternalAccountNumber = asset.HoldingExternalAccountNumber,
                        IsWhat = true,
                        AccountId = asset.HoldingId
                    };

                    pf.Period1Flow = pf.Period1PercentChange >= 0.1m;
                    pf.Period2Flow = pf.Period2PercentChange >= 0.1m;
                    pf.Period3Flow = pf.Period3PercentChange >= 0.1m;
                    pf.HasFlow = (pf.Period1Flow || pf.Period2Flow || pf.Period3Flow) && pf.Month3AgoAssetBalance > 0.0m;

                    pf.SumFlows = 0.0m; //pf.Period1Flow + pf.Period2Flow + pf.Period3Flow;
                    pf.SumFlows = pf.Period1Flow ? pf.Period1Delta : 0.0m;
                    pf.SumFlows = pf.Period2Flow ? pf.SumFlows + pf.Period2Delta : 0.0m;
                    pf.SumFlows = pf.Period3Flow ? pf.SumFlows + pf.Period3Delta : 0.0m;
                    pf.SumFlows = pf.Month3AgoAssetBalance > 0.0m ? pf.SumFlows : 0.0m;
                    
                    
                    pf.SumCommission = 0.0m; 
                    pf.SumCommission = pf.Period1Commission + pf.Period2Commission + pf.Period3Commission;

                    pf.IsSame = pf.Territory == pf.SalesCredit;
                    pfItems.Add(pf);
                }
            }

            return pfItems;
        }

        
        
        private IEnumerable<BroadridgeOtherOgDetailDataModel> BuildUmaOngoingDetail()
        {
            const LkuRateType.RateType             rateType       = LkuRateType.RateType.Ongoing;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;
            
            var ogItems = new List<BroadridgeOtherOgDetailDataModel>();
            
            foreach (var asset in _broadridgeAssets)
            {
                var territory = asset.Territory == "" ? "House" : asset.Territory;
                var rd = _regionalDirector.FirstOrDefault(r => r.LastName == territory);

                if (rd == null)
                {
                    rd = new RegionalDirectorDataModel()
                    {
                        RegionalDirectorIid = 5,
                        RegionalDirectorKey = "house",
                        FirstName = "House",
                        LastName = "House",
                        SeasoningMonths = 3,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        ModifiedBy = "dbo"
                    };
                }
                
                var rateInfo = GetRateInfo(rd.RegionalDirectorKey, asset.ProductName, false);
                var theRate = rateInfo?.OngoingRate ?? 0.0m;

                var og = new BroadridgeOtherOgDetailDataModel()
                {
                    TheSystem = asset.System,
                        FirmName = asset.FirmName,
                        FirmId = asset.FirmId,
                        FirmCRDNumber = asset.FirmCRDNumber,
                        HoldingId = asset.HoldingId,
                        HoldingExternalAccountNumber = asset.HoldingExternalAccountNumber,
                        HoldingName = asset.HoldingName,
                        HoldingStartDate = asset.HoldingStartDate,
                        HoldingCreateDate = asset.HoldingCreateDate,
                        MostRecentMonthAssetBalance = asset.MostRecentMonthAssetBalance,
                        Month1AgoAssetBalance = asset.Month1AgoAssetBalance,
                        Month2AgoAssetBalance = asset.Month2AgoAssetBalance,
                        Month3AgoAssetBalance = asset.Month3AgoAssetBalance,
                        Month4AgoAssetBalance = asset.Month4AgoAssetBalance,
                        Month5AgoAssetBalance = asset.Month5AgoAssetBalance,
                        Month6AgoAssetBalance = asset.Month6AgoAssetBalance,
                        Month7AgoAssetBalance = asset.Month7AgoAssetBalance,
                        Month8AgoAssetBalance = asset.Month8AgoAssetBalance,
                        Month9AgoAssetBalance = asset.Month9AgoAssetBalance,
                        Month10AgoAssetBalance = asset.Month10AgoAssetBalance,
                        Month11AgoAssetBalance = asset.Month11AgoAssetBalance,
                        Month12AgoAssetBalance = asset.Month11AgoAssetBalance,
                        SumFlows = GetFlowAmount(asset),
                        PayableAmount = asset.MostRecentMonthAssetBalance - GetSalesTransactions(asset.HoldingId),
                        Commission = (asset.MostRecentMonthAssetBalance - GetSalesTransactions(asset.HoldingId) ) * (theRate / 4),
                        QuarterlyRate = theRate / 4,
                        AnnualRate = theRate,
                        IsSeasoned = asset.HoldingCreateDate < GetDateDiffMonths(GetFirstOfMonth(_endDate), -3),
                        ProductName = asset.ProductName,
                        ProductType = asset.ProductType,
                        Channel = asset.Channel,
                        Region = asset.Region,
                        Territory = asset.Territory,
                        SalesCredit = asset.Territory,
                        IsSame = true,
                        PersonCRDNumber = null,
                        PersonFirstName = asset.PersonFirstName,
                        PersonLastName = asset.PersonLastName,
                        PersonId = asset.PersonId,
                        OfficeAddressLine1 = asset.OfficeAddressLine1,
                        OfficeAddressLine2 = asset.OfficeAddressLine2,
                        OfficeCity = asset.OfficeCity,
                        OfficeRegionRefCode = asset.OfficeRegionRefCode,
                        OfficePostalCode = asset.OfficePostalCode,
                        PersonBrokerTeamFlag = asset.PersonBrokerTeamFlag,
                        HoldingAddressLine1 = asset.HoldingAddressLine1,
                        SystemFAName = "?",
                        SystemOfficeAddress = "?",
                        SystemOfficeState = asset.OfficeRegionRefCode,
                        SystemQuarterEndAssets = asset.MostRecentMonthAssetBalance,
                        AssetCheck = 0.0m,
                        SystemRDCredit = asset.Territory,
                        AccountTANumber = asset.HoldingExternalAccountNumber,
                        ExternalAccountNumber = asset.HoldingExternalAccountNumber,
                        IsAccountSame = true,
                        AccountId = asset.HoldingId
                };
                ogItems.Add(og);
                
            }

            return ogItems.OrderBy(c => c.Territory).ThenBy(c => c.FirmName).Where(c => c.HoldingStartDate < _endDate);
        }
        
        private IEnumerable<BroadridgeOgSummaryDataModel> BuildOngoingSummary()
        {
            const LkuRateType.RateType rateType = LkuRateType.RateType.Ongoing;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;
            var ogSumm = new List<BroadridgeOgSummaryDataModel>();
            
            var ogSummWorking = ogDetail
               .GroupBy(c => new
                {
                    c.Territory,
                    c.TheSystem,
                    c.SystemOfficeAddress,
                    c.SystemOfficeState,
                    c.PersonFirstName,
                    c.PersonLastName
                })
                .Select(group => new
                                {
                                    RM = group.Key.Territory,
                                    TheSystem = group.Key.TheSystem,
                                    SystemOfficeAddress = group.Key.SystemOfficeAddress,
                                    SystemOfficeState = group.Key.SystemOfficeAddress,
                                    SystemFAName = $"{group.Key.PersonFirstName} {group.Key.PersonLastName}",
                                    SystemQuarterEndAssets = group.Sum(c => c.SystemQuarterEndAssets),
                                    Flows = group.Sum(c => c.SumFlows),
                                    PayableAmout = group.Sum(c => c.PayableAmount),
                                    Commission = group.Sum(c => c.Commission),
                                    QuarterlyRate = group.Min(c => c.QuarterlyRate),
                                    AnnualRate = group.Min(c => c.AnnualRate),
                                });

            foreach (var item in ogSummWorking)
            {
                var summary = new BroadridgeOgSummaryDataModel()
                {
                    RM             = item.RM,
                    TheSystem = item.TheSystem,
                    SystemOfficeAddress = string.Empty,
                    State = string.Empty,
                    SystemFAName = item.SystemFAName,
                    SystemQuarterEndAssets = item.SystemQuarterEndAssets,
                    Flows        = item.Flows,
                    PayableAmount  = item.PayableAmout,
                    QuarterlyRate = item.QuarterlyRate,
                    AnnualRate = item.AnnualRate,
                    Commission     = item.Commission
                };
                
                ogSumm.Add(summary);
            }
            
            // Total Lines
            var ogSummTotals = ogSumm
                              .GroupBy(g => new
                               {
                                   g.RM, g.TheSystem
                               })
                              .Select(grp => new
                               {
                                   RM             = grp.Key.RM,
                                   TheSystem      = grp.Key.TheSystem,
                                   SystemOfficeAddress = "Totals",
                                   SystemQuarterEndAssets = grp.Min(group => group.SystemQuarterEndAssets),
                                   Flows          = grp.Sum(group => group.Flows),
                                   PaymentAmount  = grp.Sum(group => group.PayableAmount),
                                   Commission     = grp.Sum(group => group.Commission),
                                   QuarterlyRate  = grp.Min(group => group.QuarterlyRate),
                                   AnnualRate     = grp.Min(group => group.AnnualRate)
                               });

            foreach (var item in ogSummTotals)
            {
                var summary = new BroadridgeOgSummaryDataModel()
                {
                    RM             = item.RM,
                    TheSystem = item.TheSystem,
                    SystemOfficeAddress = string.Empty,
                    State = string.Empty,
                    SystemQuarterEndAssets = item.SystemQuarterEndAssets,
                    Flows  = item.Flows,
                    PayableAmount = item.PaymentAmount,
                    Commission    = item.Commission,
                    QuarterlyRate = item.QuarterlyRate,
                    AnnualRate = item.AnnualRate
                };
                
                ogSumm.Add(summary);
            }

            return ogSumm.OrderBy(c => c.RM).ThenBy(c => c.TheSystem);
        }
        
        private decimal GetFlowAmount(BroadridgeUmaAssets asset)
        {
            var result = 0.0m;
            var diff1 = 0.0m;
            var diff2 = 0.0m;
            var diff3 = 0.0m;

            //Most Recent to 1 Mon ago
            if(IsTenPercentDiff(asset.MostRecentMonthAssetBalance, asset.Month1AgoAssetBalance))
                diff1 = asset.MostRecentMonthAssetBalance - asset.Month1AgoAssetBalance;
            
            //1 Mon ago to 2 Mon Ago
            if(IsTenPercentDiff(asset.Month1AgoAssetBalance, asset.Month2AgoAssetBalance))
                diff2 = asset.Month1AgoAssetBalance - asset.Month2AgoAssetBalance;
            
            //2 Mon ago to 3 Mon Ago
            if(IsTenPercentDiff(asset.Month2AgoAssetBalance, asset.Month3AgoAssetBalance))
                diff3 = asset.Month2AgoAssetBalance - asset.Month3AgoAssetBalance;

            result = diff1 + diff2 + diff3;

            return result;
        }
        
        private decimal GetPercentDiff(decimal amount1, decimal amount2)
        {
            var pd = amount1 == 0.0m ? 0.0m : (amount1 - amount2) / amount1;
            return pd;
        }
        
        private bool IsTenPercentDiff(decimal amount1, decimal amount2)
        {
            if (amount1 == 0.0m) return false;
            var pd = Math.Abs((amount1 - amount2) / amount1);
            return Math.Abs(pd) >= .10m;
        }

        private decimal GetSalesTransactions(string holdingId)
        {
            var result = 0.0m;
            
            var sales = _broadridgeSales.Where(c => c.HoldingId == holdingId);

            foreach (var item in sales)
            {
                result = item.TransactionCodeOverrideDescription.Equals("Purchase") ||
                         item.TransactionCodeOverrideDescription.Equals("Transfer In")
                    ? result += item.TradeAmount
                    : result = result + (item.TradeAmount + -1);
            }

            return result;
        }
    }
}