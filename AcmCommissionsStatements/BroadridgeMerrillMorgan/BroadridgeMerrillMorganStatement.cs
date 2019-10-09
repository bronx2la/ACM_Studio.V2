using System;
using System.Collections.Generic;
using System.Linq;
using Aristotle.Excel;
using Core.DataModels;
using Core.DataModels.Broadridge;
using Core.Enums;
using Dapper;
using DapperDatabaseAccess;

namespace AcmCommissionsStatements.BroadridgeMerrillMorgan
{
    public partial class BroadridgeMerrillMorganStatement : CommissionStatementBase
    {
        private IEnumerable<BroadridgeAssets> _broadridgeNewAssets;

        private IEnumerable<BroadridgeNewAssetsDetailDataModel> naDetail;
        private IEnumerable<BroadridgeOgDetailDataModel> ogDetail;
        private IEnumerable<BroadridgeOgSummaryDataModel> ogSummary;
        private IEnumerable<BroadridgeFlows> pfDetail;
        private IEnumerable<BroadridgeFlowsSummaryDataModel> pfSummary;
        
        public BroadridgeMerrillMorganStatement(string endDate, string connectionString, TaskMetaDataDataModel metaData) : base(endDate, connectionString, metaData)
        {
            InitializeNewAssetsData(endDate);
        }
        
        protected void InitializeNewAssetsData(string endDate)
        {
            var dal = new DapperDatabaseAccess<BroadridgeAssets>(_connectionString);
            
            var parms = new DynamicParameters();
            parms.Add("@ReportingDate", endDate);

            _broadridgeNewAssets = dal.SqlServerFetch("dbo.prc_BroadridgeAssets_MorganMerrill_Fetch", parms);
        }
        
        public void ProduceReport()
        {
            _workbookFile = $@"{_metaData.outboundFolder}\{Core.FileNameHelpers.FormatOutboundFileName(_metaData.outboundFile)}";
            _xl           = new AristotleExcel(_workbookFile);
//
            naDetail = BuildNewAssetsDetail();
            pfDetail = BuildPseudoFlowDetail();
//            ogDetail = BuildUmaOngoingDetail();
//            ogSummary = BuildOngoingSummary();
//            pfSummary = BuildPseudoFlowSummary();
            
            _xl.AddWorksheet(BuildNewAssetsSummary(), "NewAssets_Summary", ExcelColumnProperties("BroadridgeUmaMerrillMorgan.NewAssetsSummary"));
//            _xl.AddWorksheet(ogSummary, "Ongoing_Summary", ExcelColumnProperties("BroadridgeUma.OngoingSummary"));
//            _xl.AddWorksheet(pfSummary, "Pseudoflow_Summary", ExcelColumnProperties("BroadridgeUma.PseudoflowSummary"));
            _xl.AddWorksheet(naDetail, "NewAssets.Detail", ExcelColumnProperties("BroadridgeUmaMerrillMorgan.NewAssetsDetail"));
//            _xl.AddWorksheet(ogDetail, "Ongoing_Detail", ExcelColumnProperties("BroadridgeUma.OngoingDetail"));
            _xl.AddWorksheet(pfDetail, "Pseudoflow_Detail", ExcelColumnProperties("BroadridgeUmaMerrillMorgan.PseudoflowDetail"));
            _xl.SaveWorkbook();
        }


        #region PseudoFlows

//        private IEnumerable<BroadridgeFlowsSummaryDataModel> BuildPseudoFlowSummary()
//        {
//            const LkuRateType.RateType rateType = LkuRateType.RateType.Ongoing;
//            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;
//
//            var pfSumm = new List<BroadridgeFlowsSummaryDataModel>();
//            
//            var pfSummWorking = pfDetail
//                .GroupBy(c => new
//                {
//                    c.Territory,
//                    c.FirmName,
//                    c.OfficeCity,
//                    c.PersonLastName
//                })
//                .Select(group => new
//                {
//                    FirmName = group.Key.FirmName,
//                    Territory = group.Key.Territory,
//                    OfficeCity = group.Key.OfficeCity,
//                    PersonLastName = group.Key.PersonLastName,
//                    SumFlowAmount = group.Sum(c => c.FlowAmount),
//                    Rate = group.Min(c => c.Rate),
//                    Commission = group.Sum(c => c.FlowAmount) * group.Min(c => c.Rate),
//                });
//            
//            foreach (var item in pfSummWorking)
//            {
//                var summary = new BroadridgeFlowsSummaryDataModel()
//                {
//                    FirmName = item.FirmName,
//                    Territory = item.Territory,
////                    Totals = string.Empty,
//                    OfficeCity = item.OfficeCity,
//                    PersonLastName = item.PersonLastName,
//                    SumFlowAmount = item.SumFlowAmount > 0 ? item.SumFlowAmount : 0.0m,
//                    Rate        = item.Rate,
//                    SumCommission  = item.SumFlowAmount > 0 ? (item.SumFlowAmount * item.Rate) : 0.0m
//                };
//                
//                pfSumm.Add(summary);
//            }
//            
//            // Total Lines
//            var pfSummTotals = pfSumm
//                .GroupBy(g => new
//                {
//                    g.Territory,
//                    g.FirmName,
//                    g.OfficeCity,
//                    g.PersonLastName
//                })
//                .Select(grp => new
//                {
//                    RM             = grp.Key.Territory,
//                    OfficeCity     = grp.Key.OfficeCity,
//                    PersonLastName = grp.Key.PersonLastName,
//                    FirmName       = grp.Key.FirmName,
//                    InFlows        = grp.Sum(group => group.SumFlowAmount),
//                    Rate           = grp.Min(group => group.Rate),
//                    Commission     = grp.Sum(group => group.SumCommission)
//                });
//            
//            foreach (var item in pfSummTotals)
//            {
//                var summary = new BroadridgeFlowsSummaryDataModel()
//                {
//                    Territory = item.RM,
//                    FirmName = item.FirmName,
//                    Totals = "zz-Totals",
//                    OfficeCity = item.OfficeCity,
//                    PersonLastName = item.PersonLastName,
//                    SumFlowAmount = item.InFlows > 0 ? item.InFlows : 0.0m,
//                    Rate           = item.Rate,
//                    SumCommission  = item.InFlows > 0 ? item.Commission : 0.0m
//                };
//                
//                pfSumm.Add(summary);
//            }
//
//            return pfSumm.OrderBy(c => c.Territory).ThenBy(c => c.FirmName).ThenBy(c => c.OfficeCity).ThenBy(c => c.PersonLastName);
//
//        }
        
        private IEnumerable<BroadridgeFlows> BuildPseudoFlowDetail()
        {
            const LkuRateType.RateType rateType = LkuRateType.RateType.Ongoing;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;

            var pfItems = new List<BroadridgeFlows>();

            foreach (var asset in _broadridgeNewAssets)
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
                        TheSystem = asset.TheSystem,
                        FirmName = asset.FirmName,
                        FirmId = asset.FirmId,
                        FirmCRDNumber = asset.FirmCRDNumber,
                        Portfolio = asset.HoldingId,
                        HoldingExternalAccountNumber = asset.HoldingExternalAccountNumber,
                        HoldingName = asset.HoldingName,
                        HoldingStartDate = asset.HoldingStartdate,
                        HoldingCreateDate = asset.HoldingCreatedate,
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
                        PersonCRDNumber = asset.PersonCRDNumber,
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
                        SystemFAName = "?",
                        SystemOfficeAddress = "?",
                        SystemOfficeState = "?",
                        SystemQEAssets = asset.MostRecentMonthAssetBalance,
                        AssetCheck = 0.0m,
                        SystemRDCredit = asset.Territory,
                        AccountTANumber = "?",
                        ExternalAccountNumber = asset.HoldingExternalAccountNumber,
                        IsWhat = true,
                        AccountId = asset.HoldingId
                    };

                    pf.Period1Flow = pf.Period1PercentChange >= 0.1m;
                    pf.Period2Flow = pf.Period2PercentChange >= 0.1m;
                    pf.Period3Flow = pf.Period3PercentChange >= 0.1m;
                    pf.HasFlow = pf.Period1Flow || pf.Period2Flow || pf.Period3Flow;
                    
                    pf.SumFlows = 0.0m;
                    pf.SumCommission = pf.Period1Commission + pf.Period2Commission + pf.Period3Commission;

                    pf.IsSame = pf.Territory == pf.SalesCredit;
                    pfItems.Add(pf);
                }
            }

            return pfItems;
        }

        private decimal GetFlowAmount(BroadridgeAssets asset)
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
        
        #endregion

        #region New Assets
        
        private IEnumerable<BroadridgeNewAssetsDetailDataModel> BuildNewAssetsDetail()
        {
            var naItems = new List<BroadridgeNewAssetsDetailDataModel>();

            foreach (var item in _broadridgeNewAssets)
            {
                var rdRateInfo = RegionalDirectorRateInfo(item.Territory, (int)LkuRateType.RateType.NewAssets, (int)LkuCommissionType.CommissionType.UMA);
                var _tradeAmount = GetNewAssetAmountFromAssetsList(item); 
                try
                {
                    rdRateInfo.Rate = item.Territory.IndexOf(',') > 0 ? rdRateInfo.Rate / 2 : rdRateInfo.Rate;
                }
                catch (Exception e)
                {
                    rdRateInfo = new RegionalDirectorRateInfoDataModel();
                    rdRateInfo.Rate = 0.0m;
                }
                
                var rd = _regionalDirector.FirstOrDefault(r => r.LastName == item.Territory);
                var rateInfo = GetRateInfo(rd == null ? "House" : rd.RegionalDirectorKey, item.ProductName, false);
                var theRate = rateInfo?.NewAssetRate ?? 0.0m;
                
                var na = new BroadridgeNewAssetsDetailDataModel()
                {
                    TheSystem                 = item.TheSystem,
                    FirmName                  = item.FirmName,
                    FirmId                    = item.FirmId,
                    FirmCrdNumber             = item.FirmCRDNumber,
                    HoldingId                 = item.HoldingId,
                    HoldingExterAccountNumber = item.HoldingExternalAccountNumber,
                    HoldingName               = item.HoldingName,
                    HoldingStartDate          = Convert.ToDateTime(item.HoldingStartdate),
                    HoldingCreateDate         = Convert.ToDateTime(item.HoldingStartdate),
                    MostRecentMonthAssetBalance = item.MostRecentMonthAssetBalance,
                    Month1AgoAssetBalance = item.Month1AgoAssetBalance,
                    Month2AgoAssetBalance = item.Month2AgoAssetBalance,
                    Month3AgoAssetBalance = item.Month3AgoAssetBalance,
                    NewAssetValue = _tradeAmount,
                    Commission = _tradeAmount * theRate,
                    Rate = theRate,   
                    Territory              = item.Territory.Length.Equals(0) ? "Not Found" : item.Territory,
                    SalesRunTerritory = item.Territory,
                    IsTerritorySame = true,
                    ProductName = item.ProductName,
                    ProductType = item.ProductType,
                    Channel = item.Channel,
                    Region = item.Region,
                    PersonCRDNumber = item.PersonCRDNumber,
                    PersonFirstName = item.PersonFirstName,
                    PersonLastName = item.PersonLastName,
                    OfficeAddressLine1 = item.OfficeAddressLine1,
                    OfficeAddressLine2 = item.OfficeAddressLine2,
                    OfficeCity = item.OfficeCity,
                    OfficeRegionRefCode    = item.OfficeRegionRefCode,
                    OfficePostalCode = item.OfficePostalCode,
                    PersonBrokerTeamFlag = item.PersonBrokerTeamFlag,
                    Month4AgoAssetBalance = item.Month4AgoAssetBalance,
                    Month5AgoAssetBalance = item.Month5AgoAssetBalance,
                    Month6AgoAssetBalance = item.Month6AgoAssetBalance,
                    Month7AgoAssetBalance = item.Month7AgoAssetBalance,
                    Month8AgoAssetBalance = item.Month8AgoAssetBalance,
                    Month9AgoAssetBalance = item.Month9AgoAssetBalance,
                    Month10AgoAssetBalance = item.Month10AgoAssetBalance,
                    Month11AgoAssetBalance = item.Month11AgoAssetBalance,
                    Month12AgoAssetBalance = item.Month12AgoAssetBalance,
                    HoldingAddressLine1 = item.HoldingAddressLine1,
                    SystemFAName = "?",
                    SystemOfficeAddress = "?",
                    SystemOfficeState = item.OfficeRegionRefCode,
                    SystemQEAssets = item.MostRecentMonthAssetBalance,
                    AssetCheck = 0.0m,
                    SystemRDCredit = item.Territory,
                    RDCheck = "?",
                    AccountTANumber = item.HoldingId,
                    ExternalAccountNumber = item.HoldingExternalAccountNumber,
                    AccountId = item.HoldingId
                };
                
                if(_tradeAmount > 0)
                    naItems.Add(na);
            }

            return naItems;
        }
        
        private IEnumerable<BroadridgeNewAssetsSummaryDataModel> BuildNewAssetsSummary()
        {
            const LkuRateType.RateType             rateType       = LkuRateType.RateType.NewAssets;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;
            var naSumm = new List<BroadridgeNewAssetsSummaryDataModel>();

            var naSummWorking = naDetail
                .GroupBy(c => new
                {
                    c.Territory,
                    c.TheSystem,
                    c.OfficeAddressLine1,
                    c.SystemOfficeState,
                    c.ProductName,
                    c.SystemFAName
                })
                .Select(group => new
                {
                    Territory = group.Key.Territory,
                    System = group.Key.TheSystem,
                    OfficeAddress = group.Key.OfficeAddressLine1,
                    OfficeState = group.Key.SystemOfficeState,
                    ProductName = group.Key.ProductName,
                    FAName = group.Key.SystemFAName,
                    NewAssetValue = group.Sum(c => c.NewAssetValue),
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

        private decimal GetNewAssetAmountFromAssetsList(BroadridgeAssets item)
        {
            var result = 0.0m;
            
            if (item.Month3AgoAssetBalance != 0.0m) return result;
            
            if (item.Month2AgoAssetBalance > 0.0m)
                result = item.Month2AgoAssetBalance;
            else
            {
                result = item.Month1AgoAssetBalance > 0.0m ? item.Month1AgoAssetBalance : item.MostRecentMonthAssetBalance;
            }

            return result;
        }
        
        #endregion

        #region Ongoing

//        private IEnumerable<BroadridgeOgDetailDataModel> BuildUmaOngoingDetail()
//        {
//            const LkuRateType.RateType rateType = LkuRateType.RateType.Ongoing;
//            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;
//            
//            var ogItems = new List<BroadridgeOgDetailDataModel>();
//            
//            foreach (var asset in _broadridgeNewAssets)
//            {
////                RegionalDirectorRateInfoDataModel rdRateInfo = RegionalDirectorRateInfo(asset.Territory, (int)rateType, (int)commissionType);
////                if (rdRateInfo != null)
////                {
////                    rdRateInfo.Rate = asset.Territory.IndexOf(',') > 0
////                        ? rdRateInfo.Rate / 2
////                        : rdRateInfo.Rate;
//                    var territory = asset.Territory == "" ? "House" : asset.Territory;
//                    var rd = _regionalDirector.FirstOrDefault(r => r.LastName == territory);
//                    var rateInfo = GetRateInfo(rd.RegionalDirectorKey, asset.ProductName, false);
//                    var theRate = rateInfo?.OngoingRate ?? 0.0m;
//
//                    var og = new BroadridgeOgDetailDataModel()
//                    {
//                        BroadridgeOngoing = 1,
//                        Portfolio         = asset.HoldingId,
//                        PortShortName     = asset.HoldingName,
//                        RM                = asset.Territory,
//                        LastName          = asset.PersonLastName,
//                        ConsultantFirm    = asset.FirmName,
//                        Strategy          = asset.ProductType,
//                        PortStartDate     = asset.HoldingStartdate,
//                        AUM               = asset.Month3AgoAssetBalance != 0.0m ? asset.MostRecentMonthAssetBalance : 0.0m,
//                        InFlows           = GetFlowAmount(asset.HoldingId),
//                        SeasonedValue     = asset.MostRecentMonthAssetBalance - GetFlowAmount(asset.HoldingId),
//                        AnnualRate        = theRate,
//                        Rate              = theRate / 4,
//                        Commission        = (asset.MostRecentMonthAssetBalance - 0.0m) * (theRate / 4),
//                        IsOngoing        = asset.Month3AgoAssetBalance != 0.0m
//                    };
//                    ogItems.Add(og);
////                }
//            }
//
//            return ogItems.OrderBy(c => c.RM).ThenBy(c => c.ConsultantFirm).Where(c => c.IsOngoing);
//        }
//
//        private decimal GetFlowAmount(string portfolio)
//        {
//            var fa = pfDetail.FirstOrDefault(a => a.Portfolio == portfolio).FlowAmount;
//            return fa;
//        }
//        
//        
//        private IEnumerable<BroadridgeOgSummaryDataModel> BuildOngoingSummary()
//        {
//            const LkuRateType.RateType rateType = LkuRateType.RateType.Ongoing;
//            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;
//            var ogSumm = new List<BroadridgeOgSummaryDataModel>();
//            
//            var ogSummWorking = ogDetail
//               .GroupBy(c => new
//                {
//                    c.RM,
//                    c.ConsultantFirm
//                })
//                .Select(group => new
//                                {
//                                    RM = group.Key.RM,
//                                    ConsultantName = group.Key.ConsultantFirm,
//                                    AUM = group.Sum(c => c.AUM),
//                                    InFlows = group.Sum(c => c.InFlows),
//                                    SeasonedValue = group.Sum(c => c.SeasonedValue),
//                                    Rate = group.Min(c => c.Rate),
//                                    Commission = group.Sum(c => c.Commission)
//                                });
//
//            foreach (var item in ogSummWorking)
//            {
//                var summary = new BroadridgeOgSummaryDataModel()
//                {
//                    RM             = item.RM,
//                    ConsultantName = item.ConsultantName,
//                    AUM            = item.AUM,
//                    InFlows        = item.InFlows,
//                    SeasonedValue  = item.SeasonedValue,
//                    Rate           = item.Rate,
//                    Commission     = item.Commission
//                };
//                
//                ogSumm.Add(summary);
//            }
//            
//            // Total Lines
//            var ogSummTotals = ogSumm
//                              .GroupBy(g => new
//                               {
//                                   g.RM
//                               })
//                              .Select(grp => new
//                               {
//                                   RM             = grp.Key.RM,
//                                   ConsultantName = "Totals",
//                                   AUM            = grp.Sum(group => group.AUM),
//                                   InFlows        = grp.Sum(group => group.InFlows),
//                                   SeasonedValue  = grp.Sum(group => group.SeasonedValue),
//                                   Rate           = grp.Min(group => group.Rate),
//                                   Commission     = grp.Sum(group => group.Commission)
//                               });
//
//            foreach (var item in ogSummTotals)
//            {
//                var summary = new BroadridgeOgSummaryDataModel()
//                {
//                    RM             = item.RM,
//                    ConsultantName = item.ConsultantName,
//                    AUM            = item.AUM,
//                    InFlows        = item.InFlows,
//                    SeasonedValue  = item.SeasonedValue,
//                    Rate           = item.Rate,
//                    Commission     = item.Commission
//                };
//                
//                ogSumm.Add(summary);
//            }
//
//            return ogSumm.OrderBy(c => c.RM).ThenBy(c => c.ConsultantName);
//        }

        #endregion
    }
}