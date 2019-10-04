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
    public class BroadridgeMerrillMorganStatement : CommissionStatementBase
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

            naDetail = BuildNewAssetsDetail();
            pfDetail = BuildPseudoFlowDetail();
            ogDetail = BuildUmaOngoingDetail();
            ogSummary = BuildOngoingSummary();
            pfSummary = BuildPseudoFlowSummary();
            
            _xl.AddWorksheet(BuildNewAssetsSummary(), "NewAssets_Summary", ExcelColumnProperties("BroadridgeUma.NewAssetsSummary"));
            _xl.AddWorksheet(ogSummary, "Ongoing_Summary", ExcelColumnProperties("BroadridgeUma.OngoingSummary"));
            _xl.AddWorksheet(pfSummary, "Pseudoflow_Summary", ExcelColumnProperties("BroadridgeUma.PseudoflowSummary"));
            _xl.AddWorksheet(naDetail, "NewAssets_Detail", ExcelColumnProperties("BroadridgeUma.NewAssetsDetail"));
            _xl.AddWorksheet(ogDetail, "Ongoing_Detail", ExcelColumnProperties("BroadridgeUma.OngoingDetail"));
            _xl.AddWorksheet(pfDetail, "Pseudoflow_Detail", ExcelColumnProperties("BroadridgeUma.PseudoflowDetail"));
            _xl.SaveWorkbook();
        }


        #region PseudoFlows

        private IEnumerable<BroadridgeFlowsSummaryDataModel> BuildPseudoFlowSummary()
        {
            const LkuRateType.RateType rateType = LkuRateType.RateType.Ongoing;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;

            var pfSumm = new List<BroadridgeFlowsSummaryDataModel>();
            
            var pfSummWorking = pfDetail
                .GroupBy(c => new
                {
                    c.Territory,
                    c.FirmName,
                    c.OfficeCity,
                    c.PersonLastName
                })
                .Select(group => new
                {
                    FirmName = group.Key.FirmName,
                    Territory = group.Key.Territory,
                    OfficeCity = group.Key.OfficeCity,
                    PersonLastName = group.Key.PersonLastName,
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
                    OfficeCity = item.OfficeCity,
                    PersonLastName = item.PersonLastName,
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
                    g.Territory,
                    g.FirmName,
                    g.OfficeCity,
                    g.PersonLastName
                })
                .Select(grp => new
                {
                    RM             = grp.Key.Territory,
                    OfficeCity     = grp.Key.OfficeCity,
                    PersonLastName = grp.Key.PersonLastName,
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
                    OfficeCity = item.OfficeCity,
                    PersonLastName = item.PersonLastName,
                    SumFlowAmount = item.InFlows > 0 ? item.InFlows : 0.0m,
                    Rate           = item.Rate,
                    SumCommission  = item.InFlows > 0 ? item.Commission : 0.0m
                };
                
                pfSumm.Add(summary);
            }

            return pfSumm.OrderBy(c => c.Territory).ThenBy(c => c.FirmName);

        }
        
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
                        FirmName = asset.FirmName,
                        FirmId = asset.FirmId,
                        FirmCRDNumber = asset.FirmCRDNumber,
                        Portfolio = asset.HoldingId,
                        PortStartDate = asset.HoldingStartdate,
                        HoldingExternalAccountNumber = asset.HoldingExternalAccountNumber,
                        HoldingName = asset.HoldingName,
                        ProductName = asset.ProductName,
                        Territory = asset.Territory,
                        PersonFirstName = asset.PersonFirstName,
                        PersonLastName = asset.PersonLastName,
                        OfficeCity = asset.OfficeCity,
                        MostRecentMonthAssetBalance = asset.MostRecentMonthAssetBalance,
                        Month1AgoAssetBalance = asset.Month1AgoAssetBalance,
                        Month2AgoAssetBalance = asset.Month2AgoAssetBalance,
                        Month3AgoAssetBalance = asset.Month3AgoAssetBalance,
                        Diff1 = IsTenPercentDiff(asset.MostRecentMonthAssetBalance, asset.Month1AgoAssetBalance) ?
                            asset.MostRecentMonthAssetBalance - asset.Month1AgoAssetBalance : 
                            0.0m,
                        Diff2 = IsTenPercentDiff(asset.Month1AgoAssetBalance, asset.Month2AgoAssetBalance) ?
                            asset.Month1AgoAssetBalance - asset.Month2AgoAssetBalance : 
                            0.0m,
                        Diff3 = IsTenPercentDiff(asset.Month2AgoAssetBalance, asset.Month3AgoAssetBalance) ?
                            asset.Month2AgoAssetBalance - asset.Month3AgoAssetBalance : 
                            0.0m,
                        Rate = theRate,
                        Commission = fa > 0 ? (asset.Month3AgoAssetBalance > 0.0m ? fa * theRate : 0.0m) : 0.0m
                    };

                    pf.FlowAmount = (pf.Diff1 + pf.Diff2 + pf.Diff3) > 0.0m? (pf.Diff1 + pf.Diff2 + pf.Diff3) : 0.0m;
                    pf.PercentDiff1 = GetPercentDiff(pf.MostRecentMonthAssetBalance, pf.Month1AgoAssetBalance);
                    pf.PercentDiff2 = GetPercentDiff(pf.Month1AgoAssetBalance, pf.Month2AgoAssetBalance);
                    pf.PercentDiff3 = GetPercentDiff(pf.Month2AgoAssetBalance, pf.Month3AgoAssetBalance);
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
                    TheSystem                 = item.System,
                    HoldingExterAccountNumber = item.HoldingExternalAccountNumber,
                    HoldingName = item.HoldingName,
                    HoldingCreateDate      = Convert.ToDateTime(item.HoldingStartdate),
                    FirmName               = item.FirmName,
                    FirmId = item.FirmId,
                    FirmCrdNumber = item.FirmCRDNumber,
                    ProductName = item.ProductName,
                    PersonFirstName = item.PersonFirstName,
                    PersonLastName = item.PersonLastName,
                    OfficeCity = item.OfficeCity,
                    OfficeAddressLine1 = item.OfficeAddressLine1,
                    OfficeRegionRefCode    = item.OfficeRegionRefCode,
                    Territory              = item.Territory.Length.Equals(0) ? "Not Found" : item.Territory,
                    MarketValue            = _tradeAmount,
                    Rate                   = theRate,
                    Commission             = _tradeAmount * theRate,
                    IsNewAsset             = item.IsNewAsset,
                    IsGrayStone            = null,
                    IsTransfer             = null
                };
                
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
                                    c.TheSystem,
                                    c.Territory,
                                    c.OfficeRegionRefCode,
                                    c.OfficeCity,
                                    c.OfficeAddressLine1,
                                    c.FirmName,
                                    c.PersonLastName,
                                    c.IsNewAsset
                                })
                               .Select(group => new
                                {
                                    System              = group.Key.TheSystem,
                                    Territory           = group.Key.Territory,
                                    FirmName            = group.Key.FirmName,
                                    PersonLastName      = group.Key.PersonLastName,
                                    OfficeRegionRefCode = group.Key.OfficeRegionRefCode,
                                    OfficeCity          = group.Key.OfficeCity,
                                    OfficeAddressLine1  = group.Key.OfficeAddressLine1,
                                    MarketValue         = group.Sum(c => c.MarketValue),
                                    Rate                = group.Min(c => c.Rate),
                                    Commission          = group.Sum(c => c.Commission),
                                    IsNewAsset          = group.Key.IsNewAsset
                                })
                               .Where(c => c.IsNewAsset.Equals(true));

            foreach (var item in naSummWorking)
            {
                var summary = new BroadridgeNewAssetsSummaryDataModel()
                {
                    TheSystem = item.System,
                    Territory = item.Territory,
                    FirmName = item.FirmName,
                    PersonLastName = item.PersonLastName,
                    OfficeAddress = item.OfficeAddressLine1,
                    OfficeCity = item.OfficeCity,
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
                                   g.TheSystem,
                                   g.Territory
                               })
                              .Select(group => new
                               {
                                   System              = group.Key.TheSystem,
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
                    TheSystem = item.System,
                    Territory = item.Territory,
                    FirmName = "z--Totals",
                    MarketValue = item.MarketValue,
                    Rate = item.Rate,
                    Commission = item.Commission
                };
                naSumm.Add(summary);
            }

            return naSumm.OrderBy(c => c.TheSystem).ThenBy(c => c.Territory).ThenBy(c => c.FirmName);
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

        private IEnumerable<BroadridgeOgDetailDataModel> BuildUmaOngoingDetail()
        {
            const LkuRateType.RateType rateType = LkuRateType.RateType.Ongoing;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;
            
            var ogItems = new List<BroadridgeOgDetailDataModel>();
            
            foreach (var asset in _broadridgeNewAssets)
            {
//                RegionalDirectorRateInfoDataModel rdRateInfo = RegionalDirectorRateInfo(asset.Territory, (int)rateType, (int)commissionType);
//                if (rdRateInfo != null)
//                {
//                    rdRateInfo.Rate = asset.Territory.IndexOf(',') > 0
//                        ? rdRateInfo.Rate / 2
//                        : rdRateInfo.Rate;
                    var territory = asset.Territory == "" ? "House" : asset.Territory;
                    var rd = _regionalDirector.FirstOrDefault(r => r.LastName == territory);
                    var rateInfo = GetRateInfo(rd.RegionalDirectorKey, asset.ProductName, false);
                    var theRate = rateInfo?.OngoingRate ?? 0.0m;

                    var og = new BroadridgeOgDetailDataModel()
                    {
                        BroadridgeOngoing = 1,
                        Portfolio         = asset.HoldingId,
                        PortShortName     = asset.HoldingName,
                        RM                = asset.Territory,
                        LastName          = asset.PersonLastName,
                        ConsultantFirm    = asset.FirmName,
                        Strategy          = asset.ProductType,
                        PortStartDate     = asset.HoldingStartdate,
                        AUM               = asset.Month3AgoAssetBalance != 0.0m ? asset.MostRecentMonthAssetBalance : 0.0m,
                        InFlows           = GetFlowAmount(asset.HoldingId),
                        SeasonedValue     = asset.MostRecentMonthAssetBalance - GetFlowAmount(asset.HoldingId),
                        AnnualRate        = theRate,
                        Rate              = theRate / 4,
                        Commission        = (asset.MostRecentMonthAssetBalance - 0.0m) * (theRate / 4),
                        IsOngoing        = asset.Month3AgoAssetBalance != 0.0m
                    };
                    ogItems.Add(og);
//                }
            }

            return ogItems.OrderBy(c => c.RM).ThenBy(c => c.ConsultantFirm).Where(c => c.IsOngoing);
        }

        private decimal GetFlowAmount(string portfolio)
        {
            var fa = pfDetail.FirstOrDefault(a => a.Portfolio == portfolio).FlowAmount;
            return fa;
        }
        
        
        private IEnumerable<BroadridgeOgSummaryDataModel> BuildOngoingSummary()
        {
            const LkuRateType.RateType rateType = LkuRateType.RateType.Ongoing;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;
            var ogSumm = new List<BroadridgeOgSummaryDataModel>();
            
            var ogSummWorking = ogDetail
               .GroupBy(c => new
                {
                    c.RM,
                    c.ConsultantFirm
                })
                .Select(group => new
                                {
                                    RM = group.Key.RM,
                                    ConsultantName = group.Key.ConsultantFirm,
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

        #endregion
    }
}