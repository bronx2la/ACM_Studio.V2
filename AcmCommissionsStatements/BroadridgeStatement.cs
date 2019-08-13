using System;
using System.Collections.Generic;
using System.Linq;
using Aristotle.Excel;
using Core.DataModels;
using Core.DataModels.Broadridge;
using Core.Enums;
using Dapper;
using DapperDatabaseAccess;

namespace AcmCommissionsStatements
{
        public class BroadridgeStatement : CommissionStatementBase
    {
        private IEnumerable<BroadridgeSales> _broadridgeSales;
        private IEnumerable<BroadridgeUmaAssets> _broadridgeAssets;
        private IEnumerable<BroadridgeFlows> _broadridgeFlows;
        private bool _isUma;

        private IEnumerable<BroadridgeNewAssetsDetailDataModel> naDetail;
        private IEnumerable<BroadridgeNewAssetsSummaryDataModel> naSummary;
        private IEnumerable<BroadridgeOgDetailDataModel> ogDetail;
        private IEnumerable<BroadridgeOgSummaryDataModel> ogSummary;
        private IEnumerable<BroadridgeFlows> pfDetail;
        private IEnumerable<BroadridgeFlowsSummaryDataModel> pfSummary;
        
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

        public void ProduceReport()
        {
            _workbookFile = $@"{_metaData.outboundFolder}\{Core.FileNameHelpers.FormatOutboundFileName(_metaData.outboundFile)}";
            _xl           = new AristotleExcel(_workbookFile);

            naDetail = BuildNewAssetsDetail();
            naSummary = BuildNewAssetsSummary();
            ogDetail = BuildUmaOngoingDetail();
            ogSummary = BuildOngoingSummary();
            pfDetail = BuildPseudoFlowDetail();
            pfSummary = BuildPseudoFlowSummary();
            
            _xl.AddWorksheet(naSummary, "NewAssets_Summary", ExcelColumnProperties("BroadridgeUma.NewAssetsSummary"));
            _xl.AddWorksheet(ogSummary, "Ongoing_Summary", ExcelColumnProperties("BroadridgeUma.OngoingSummary"));
            _xl.AddWorksheet(pfSummary, "Pseudoflow_Summary", ExcelColumnProperties("BroadridgeUma.PseudoflowSummary"));
            _xl.AddWorksheet(naDetail, "NewAssets_Detail", ExcelColumnProperties("BroadridgeUma.NewAssetsDetail"));
            _xl.AddWorksheet(ogDetail, "Ongoing_Detail", ExcelColumnProperties("BroadridgeUma.OngoingDetail"));
            _xl.AddWorksheet(pfDetail, "Pseudoflow_Detail", ExcelColumnProperties("BroadridgeUma.PseudoflowDetail"));
            _xl.SaveWorkbook();
        }

        private IEnumerable<BroadridgeNewAssetsDetailDataModel> BuildNewAssetsDetail()
        {
            const LkuRateType.RateType rateType= LkuRateType.RateType.NewAssets;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;
            var naItems = new List<BroadridgeNewAssetsDetailDataModel>();

            foreach (var item in _broadridgeSales)
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

                    var na = new BroadridgeNewAssetsDetailDataModel()
                    {
                        System                 = item.System,
                        HoldingCreateDate      = item.TradeDate,
                        FirmId                 = item.FirmId,
                        FirmName               = item.FirmName,
                        PersonFirstName        = item.PersonFirstName,
                        PersonLastName         = item.PersonLastName,
                        ProductName            = item.ProductName, 
                        OfficeAddressLine1     = item.OfficeAddressLine1,
                        OfficeCity             = item.OfficeCity, 
                        OfficeRegionRefCode    = item.OfficeRegionRefCode,
                        Territory              = item.Territory,
                        MarketValue            = item.TradeAmount,
                        Rate                   = rdRateInfo.Rate,
                        Commission             = item.TradeAmount * rdRateInfo.Rate,
                        IsNewAsset             = (item.TradeDate > _startDate) ? true : false,
                        IsGrayStone            = null,
                        IsTransfer             = null
                    };
                    
                    naItems.Add(na);
                }    
                
            }

            var y = 0;
            return naItems.OrderBy(c => c.System).ThenBy(c => c.Territory)
                          .ThenBy(c => c.FirmName);
        }

        private IEnumerable<BroadridgeNewAssetsSummaryDataModel> BuildNewAssetsSummary()
        {
            const LkuRateType.RateType             rateType       = LkuRateType.RateType.NewAssets;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;
            var naSumm = new List<BroadridgeNewAssetsSummaryDataModel>();

            var naSummWorking = naDetail
                .GroupBy(c => new
                {
                    c.System,
                    c.Territory,
                    c.FirmName,
                    c.OfficeRegionRefCode,
                    c.IsNewAsset
                })
                .Select(group => new
                {
                    System = group.Key.System,
                    Territory = group.Key.Territory,
                    FirmName = group.Key.FirmName,
                    OfficeRegionRefCode = group.Key.OfficeRegionRefCode,
                    MarketValue = group.Sum(c => c.MarketValue),
                    Rate = group.Min(c => c.Rate),
                    Commission = group.Sum(c => c.Commission),
                    IsNewAsset = group.Key.IsNewAsset
                });

            foreach (var item in naSummWorking)
            {
                var summary = new BroadridgeNewAssetsSummaryDataModel()
                {
                    System = item.System,
                    Territory = item.Territory,
                    FirmName = item.FirmName,
                    OfficeRegionRefCode = item.OfficeRegionRefCode,
                    MarketValue = item.MarketValue,
                    Rate = item.Rate,
                    Commission = item.Commission
                };
                naSumm.Add(summary);
            }
            
            // Total Lines
            var naSummTotals = naSumm
                              .GroupBy(g => new
                               {
                                   g.System,
                                   g.Territory
                               })
                              .Select(group => new
                               {
                                   System              = group.Key.System,
                                   Territory           = group.Key.Territory,
                                   FirmName            = "-----",
                                   OfficeRegionRefCode = "-----",
                                   MarketValue         = group.Sum(c => c.MarketValue),
                                   Rate                = group.Min(c=>c.Rate),
                                   Commission          = group.Sum(c => c.Commission)
                               });

            foreach (var item in naSummTotals)
            {
                var summary = new BroadridgeNewAssetsSummaryDataModel()
                {
                    System = item.System,
                    Territory = item.Territory,
                    FirmName = "z--Totals",
                    MarketValue = item.MarketValue,
                    Rate = item.Rate,
                    Commission = item.Commission
                };
                naSumm.Add(summary);
            }

            return naSumm.OrderBy(c => c.System).ThenBy(c => c.Territory).ThenBy(c => c.FirmName);
        }

        private IEnumerable<BroadridgeOgDetailDataModel> BuildUmaOngoingDetail()
        {
            const LkuRateType.RateType             rateType       = LkuRateType.RateType.Ongoing;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;
            
            var ogItems = new List<BroadridgeOgDetailDataModel>();
//            IEnumerable<BroadridgeFlows> broadridgeFlows = FindUmaFlowAmount();
            
            foreach (var asset in _broadridgeAssets)
            {
                RegionalDirectorRateInfoDataModel rdRateInfo = RegionalDirectorRateInfo(asset.Territory, (int)rateType, (int)commissionType);
                if (rdRateInfo != null)
                {
                    rdRateInfo.Rate = asset.Territory.IndexOf(',') > 0
                        ? rdRateInfo.Rate / 2
                        : rdRateInfo.Rate;
                }
                else rdRateInfo = new RegionalDirectorRateInfoDataModel();

//                BroadridgeFlows flowItem = broadridgeFlows.FirstOrDefault(a => a.Portfolio == asset.HoldingId);
                    
                var og = new BroadridgeOgDetailDataModel()
                {
                    BroadridgeOngoing = 1,
                    
                    Portfolio         = asset.HoldingId,
                    PortShortName     = asset.HoldingName,
                    RM                = asset.Territory,
                    Region            = asset.Region,
                    Territory         = asset.Territory,
                    FirstName         = asset.PersonFirstName,
                    LastName          = asset.PersonLastName,
                    ConsultantName    = $"{asset.PersonLastName}, {asset.PersonFirstName}",
                    ConsultantFirm    = asset.FirmName,
                    Strategy          = asset.ProductType,
                    PortStartDate     = asset.HoldingCreateDate,
                    AUM               = asset.Month3AgoAssetBalance > 0.0m ? asset.MostRecentMonthAssetBalance : 0.0m,
                    InFlows           = 0.0m,
                    SeasonedValue     = asset.MostRecentMonthAssetBalance - 0.0m,
                    AnnualRate        = rdRateInfo.Rate,
                    Rate              = rdRateInfo.Rate / 4,
                    Commission        = (asset.MostRecentMonthAssetBalance - 0.0m) * (rdRateInfo.Rate / 4)
                };
                ogItems.Add(og);
                
            }

            return ogItems.OrderBy(c => c.RM).ThenBy(c => c.ConsultantName).Where(c => c.PortStartDate < _endDate);
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

                    var fa = GetFlowAmount(asset);
                    
                    var pf = new BroadridgeFlows()
                    {
                        FirmName = asset.FirmName,
                        FirmId = asset.FirmId,
                        FirmCRDNumber = asset.FirmCRDNumber,
                        Portfolio = asset.HoldingId,
                        PortStartDate = asset.HoldingCreateDate,
                        HoldingExternalAccountNumber = asset.HoldingExternalAccountNumber,
                        HoldingName = asset.HoldingName,
                        ProductName = asset.ProductName,
                        Territory = asset.Territory,
                        PersonFirstName = asset.PersonFirstName,
                        PersonLastName = asset.PersonLastName,
                        OfficeCity = asset.OfficeCity,
                        FlowAmount = fa > 0.0m ? fa : 0.0m,
                        Rate = rdRateInfo.Rate,
                        Commission = fa * rdRateInfo.Rate
                    };
                    
                    pfItems.Add(pf);
                }
            }

            return pfItems;
        }
        
        private IEnumerable<BroadridgeFlowsSummaryDataModel> BuildPseudoFlowSummary()
        {
            const LkuRateType.RateType rateType = LkuRateType.RateType.Ongoing;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;

            var pfSumm = new List<BroadridgeFlowsSummaryDataModel>();
            
            var pfSummWorking = pfDetail
                .GroupBy(c => new
                {
                    c.FirmName,
                    c.Territory
                })
                .Select(group => new
                {
                    FirmName = group.Key.FirmName,
                    Territory = group.Key.Territory,
                    SumFlowAmount = group.Sum(c => c.FlowAmount),
                    Rate = group.Min(c => c.Rate),
                    Commission = group.Sum(c => c.FlowAmount) * group.Min(c => c.Rate),
                });
            
            foreach (var item in pfSummWorking)
            {
                var summary = new BroadridgeFlowsSummaryDataModel()
                {
                    FirmName = item.FirmName,
                    Territory = item.Territory,
                    SumFlowAmount = item.SumFlowAmount > 0 ? item.SumFlowAmount : 0.0m,
                    Rate        = item.Rate,
                    SumCommission  = item.SumFlowAmount > 0 ? (item.SumFlowAmount * item.Rate) : 0.0m
                };
                
                pfSumm.Add(summary);
            }
            
            // Total Lines
            var pfSummTotals = pfSumm
                .GroupBy(g => new
                {
                    g.Territory
                })
                .Select(grp => new
                {
                    RM             = grp.Key.Territory,
                    ConsultantName = "Totals",
                    InFlows        = grp.Sum(group => group.SumFlowAmount),
                    Rate           = grp.Min(group => group.Rate),
                    Commission     = grp.Sum(group => group.SumCommission)
                });
            
            foreach (var item in pfSummTotals)
            {
                var summary = new BroadridgeFlowsSummaryDataModel()
                {
                    Territory = item.RM,
                    FirmName = item.ConsultantName,
                    SumFlowAmount = item.InFlows > 0 ? item.InFlows : 0.0m,
                    Rate           = item.Rate,
                    SumCommission  = item.InFlows > 0 ? item.Commission : 0.0m
                };
                
                pfSumm.Add(summary);
            }

            return pfSumm.OrderBy(c => c.Territory).ThenBy(c => c.FirmName);
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
        
        private bool IsTenPercentDiff(decimal amount1, decimal amount2)
        {
            if (amount1 == 0.0m) return false;
            var pd = Math.Abs((amount1 - amount2) / amount1);
            return Math.Abs(pd) >= .10m;
        }

        
        
        private IEnumerable<BroadridgeOgDetailDataModel> BuildMutualFundsOngoingDetail()
        {
            const LkuRateType.RateType             rateType       = LkuRateType.RateType.Ongoing;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.MutualFunds;
            
            var ogItems   = new List<BroadridgeOgDetailDataModel>();
            IEnumerable<BroadridgeFlows> broadridgeFlows = FindUmaFlowAmount();

            foreach (var asset in _broadridgeAssets)
            {
                RegionalDirectorRateInfoDataModel rdRateInfo = RegionalDirectorRateInfo(asset.Territory, (int)rateType, (int)commissionType);
                if (rdRateInfo != null)
                {
                    rdRateInfo.Rate = asset.Territory.IndexOf(',') > 0
                                          ? rdRateInfo.Rate / 2
                                          : rdRateInfo.Rate;

//                    BroadridgeFlows flow = broadridgeFlows.FirstOrDefault(a => a.Portfolio == asset.HoldingId);
//                    decimal flowValue = flow?.FlowAmount ?? 0.0m;
//                    flowValue = flowValue < 0.0m ? 0.0m : flowValue;

                    var og = new BroadridgeOgDetailDataModel()
                    {
//                        BroadridgeOngoing = 1,
//                        Portfolio = asset.HoldingExternalAccountNumber,
//                        PortShortName = asset.HoldingName,
//                        RM = asset.Territory,
//                        ConsultantFirm = asset.Firm,
//                        ConsultantName = asset.FirmName,
//                        Strategy = asset.ProductType,
//                        PortStartDate = asset.HoldingStartdate,
//                        AUM = asset.MostRecentMonthAssetBalance,
//                        InFlows = flowValue,
//                        SeasonedValue = asset.MostRecentMonthAssetBalance - flowValue,
//                        AnnualRate = rdRateInfo.Rate,
//                        Rate = rdRateInfo.Rate / 4,
//                        Commission = (asset.MostRecentMonthAssetBalance - flowValue) * (rdRateInfo.Rate / 4)
                    };
                    ogItems.Add(og);
                }
            }

            return ogItems.Where(c => c.SeasonedValue >= 0).OrderBy(c=>c.RM).ThenBy(c=>c.ConsultantFirm);
        }


        private IEnumerable<BroadridgeFlows> FindUmaFlowAmount()
        {
            var flow1 = 0.0m;
            var flow2 = 0.0m;
            var flow3 = 0.0m;
            var broadridgeFlows = new List<BroadridgeFlows>();
            
            foreach (var item in _broadridgeAssets)
            {
//                flow3 = (item.Month2AgoAssetBalance - item.Month3AgoAssetBalance);
//                flow2 = (item.Month1AgoAssetBalance - item.Month2AgoAssetBalance);
//                flow1 = (item.MostRecentMonthAssetBalance - item.Month1AgoAssetBalance);
//
//                var broadridgeFlow = new BroadridgeFlows()
//                {
//                    Portfolio = item.HoldingId,
//                    PortStartDate = item.HoldingStartdate,
//                    FlowAmount = flow1 + flow2 + flow3
//                };
                
//                broadridgeFlows.Add(broadridgeFlow);
            }

            return broadridgeFlows;
        }
        
        private IEnumerable<BroadridgeFlows> FindMutualFundFlowAmount(string holdingId)
        {
            var flow1           = 0.0m;
            var flow2           = 0.0m;
            var flow3           = 0.0m;
            var broadridgeFlows = new List<BroadridgeFlows>();
            
            foreach (var item in _broadridgeAssets)
            {
//                flow3 = (item.Month2AgoAssetBalance - item.Month3AgoAssetBalance);
//                flow2 = (item.Month1AgoAssetBalance - item.Month2AgoAssetBalance);
//                flow1 = (item.MostRecentMonthAssetBalance - item.Month1AgoAssetBalance);
//
//                var broadridgeFlow = new BroadridgeFlows()
//                {
//                    Portfolio     = item.HoldingId,
//                    PortStartDate = item.HoldingStartdate,
//                    FlowAmount    = flow1 + flow2 + flow3
//                };
//                
//                broadridgeFlows.Add(broadridgeFlow);
            }

            return broadridgeFlows;
        }

        private IEnumerable<BroadridgeOgSummaryDataModel> BuildOngoingSummary()
        {
            const LkuRateType.RateType             rateType       = LkuRateType.RateType.Ongoing;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;
            var ogSumm = new List<BroadridgeOgSummaryDataModel>();
            
            var ogSummWorking = ogDetail
               .GroupBy(c => new
                {
                    c.RM,
                    c.ConsultantName
                })
                .Select(group => new
                                {
                                    RM = group.Key.RM,
                                    ConsultantName = group.Key.ConsultantName,
                                    AUM = group.Sum(c => c.AUM),
                                    InFlows = group.Sum(c => c.InFlows),
                                    SeasonedValue = group.Sum(c => c.SeasonedValue),
                                    Rate = group.Min(c => c.Rate),
                                    Commission = group.Sum(c => c.Commission)
                                });

            foreach (var item in ogSummWorking)
            {
                var summary = new BroadridgeOgSummaryDataModel()
                {
                    RM             = item.RM,
                    ConsultantName = item.ConsultantName,
                    AUM            = item.AUM,
                    InFlows        = item.InFlows,
                    SeasonedValue  = item.SeasonedValue,
                    Rate           = item.Rate,
                    Commission     = item.Commission
                };
                
                ogSumm.Add(summary);
            }
            
            // Total Lines
            var ogSummTotals = ogSumm
                              .GroupBy(g => new
                               {
                                   g.RM
                               })
                              .Select(grp => new
                               {
                                   RM             = grp.Key.RM,
                                   ConsultantName = "Totals",
                                   AUM            = grp.Sum(group => group.AUM),
                                   InFlows        = grp.Sum(group => group.InFlows),
                                   SeasonedValue  = grp.Sum(group => group.SeasonedValue),
                                   Rate           = grp.Min(group => group.Rate),
                                   Commission     = grp.Sum(group => group.Commission)
                               });

            foreach (var item in ogSummTotals)
            {
                var summary = new BroadridgeOgSummaryDataModel()
                {
                    RM             = item.RM,
                    ConsultantName = item.ConsultantName,
                    AUM            = item.AUM,
                    InFlows        = item.InFlows,
                    SeasonedValue  = item.SeasonedValue,
                    Rate           = item.Rate,
                    Commission     = item.Commission
                };
                
                ogSumm.Add(summary);
            }

            return ogSumm.OrderBy(c => c.RM).ThenBy(c => c.ConsultantName);
        }
        
    }
}