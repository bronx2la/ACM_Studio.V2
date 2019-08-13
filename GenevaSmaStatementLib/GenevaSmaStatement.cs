using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using Aristotle.Excel;
using Core.DataModels;
using Core.Enums;
using Dapper;
using DapperDatabaseAccess;

namespace GenevaSmaStatementLib
{
    public class GenevaSmaStatementOld
    {
        private IEnumerable<GenevaSmaAssetsDataModel> NewAssetsItems { get; set; }
        private IEnumerable<GenevaSmaAumDataModel> AumItems { get; set; }
        
        private IEnumerable<GenevaSmaFlowsDataModel> FlowsItems { get; set; }

        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        private readonly string _connectionString;
        private readonly TaskMetaDataDataModel _metaData;
        private AristotleExcel _xl;
        
        private string                      _workbookFile { get; set; }
        
        public GenevaSmaStatementOld(string startDate, string endDate, string connectionString, TaskMetaDataDataModel metaData)
        {
            _startDate = DateTime.ParseExact(startDate, "yyyyMMdd", null);
            _endDate = DateTime.ParseExact(endDate, "yyyyMMdd", null);
            _connectionString = connectionString;
            _metaData = metaData;

            NewAssetsItems = InitNewAssetsItems();
            AumItems = InitAumItems();
        }

        public void ProduceReport()
        {
            //NewAssets Detail
            IEnumerable<GenevaSmaNewAssetsDataModel> naDetail = BuildNewAssetsData();
            //NewAssets Summary
            IEnumerable<GenevaNewAssetsSummaryDataModel> naSummary = BuildNewAssetsSummaryData(naDetail);
            //Ongoing Detail
            IEnumerable<GenevaSmaOngoingDetailDataModel> ogDetail = BuildOngoingDetail();
            //Ongoing Summary
            IEnumerable<GenevaSmaOngoingSummaryDataModel> ogSummary = BuildOngoingSummary();
            
            _workbookFile = $@"{_metaData.outboundFolder}\{Core.FileNameHelpers.FormatOutboundFileName(_metaData.outboundFile)}";
            _xl = new AristotleExcel(_workbookFile);
            _xl.AddWorksheet(naSummary, "SMA_NewAssets_Summary", SetNewAssetsSummaryColumnFormatProperties());
            _xl.AddWorksheet(ogDetail, "SMA_Ongoing_Summary", SetNewAssetsSummaryColumnFormatProperties());
            _xl.AddWorksheet(naDetail, "SMA_NewAssets_Detail", SetNewAssetsDetailColumnFormatProperties());
            _xl.AddWorksheet(ogDetail, "SMA_Ongoing_Detail", SetOngoingDetailColumnFormatProperties());


            _xl.SaveWorkbook();
        }
        
        private IEnumerable<AristotleExcelStyle> SetNewAssetsDetailColumnFormatProperties()
        {
            var _list = new List<AristotleExcelStyle>();
            var _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber      = 4,
                Format      = "#,##0.00",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber = 5,
                Format      = "YYYY-mm-dd",
                ColumnWidth = 18,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber = 13,
                Format      = "#,##0.00",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber = 14,
                Format      = "#,##0.00",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber = 15,
                Format      = "#0.000000",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber = 16,
                Format      = "#,##0.00",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            return _list;
        }

        private IEnumerable<AristotleExcelStyle> SetOngoingDetailColumnFormatProperties()
        {
            var _list = new List<AristotleExcelStyle>();
            var _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber = 8,
                Format      = "YYYY-MM-DD",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber      = 9,
                Format      = "#,##0.00",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber      = 10,
                Format      = "#,##0.00",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);

            _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber      = 11,
                Format      = "#,##0.00",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber      = 12,
                Format      = "#0.000000",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber      = 13,
                Format      = "#0.000000",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber      = 14,
                Format      = "#,##0.00",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            return _list;
        }

        private IEnumerable<AristotleExcelStyle> SetNewAssetsSummaryColumnFormatProperties()
        {
            var _list = new List<AristotleExcelStyle>();
            var _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber      = 4,
                Format      = "#,##0.00",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber      = 5,
                Format      = "#0.000000",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber      = 6,
                Format      = "#,##0.00",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            return _list;
        }

        private IEnumerable<AristotleExcelStyle> SetOngoingSummaryColumnFormatProperties()
        {
            var _list = new List<AristotleExcelStyle>();
            var _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber      = 2,
                Format      = "#,##0.00",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber      = 3,
                Format      = "#,##0.00",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber      = 4,
                Format      = "#,##0.00",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber      = 5,
                Format      = "#0.000000",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                ColumnNumber      = 6,
                Format      = "#,##0.00",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            return _list;
        }


        private GenevaSmaAumDataModel FindAumItemByPortfolioCode(string portfolioCode)
        {
            IEnumerable<GenevaSmaAumDataModel> aum =
                from item in AumItems
                where item.PortfolioCode == portfolioCode
                select item;
            return aum.FirstOrDefault();
        }

        private GenevaSmaFlowsDataModel FindFlowAmount( IEnumerable<GenevaSmaFlowsDataModel> flowItems, string portfolioCode)
        {
            if (flowItems != null)
            {
                IEnumerable<GenevaSmaFlowsDataModel> flow =
                    from item in flowItems
                    where item.Portfolio == portfolioCode
                    select item;
                
                return flow.FirstOrDefault();
            }
            else
            {
                return new GenevaSmaFlowsDataModel();
            }

        }
        
        private IEnumerable<GenevaSmaNewAssetsDataModel> BuildNewAssetsData()
        {
            const LkuRateType.RateType rateType = LkuRateType.RateType.NewAssets;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.SMA;
            var naItems = new List<GenevaSmaNewAssetsDataModel>();

            foreach (GenevaSmaAssetsDataModel item in NewAssetsItems)
            {
                RegionalDirectorRateInfoDataModel rdRateInfo = RegionalDirectorRateInfo(item.InternalMarketingPerson, (int)rateType, (int)commissionType);

                if(rdRateInfo != null)
                {
                    rdRateInfo.Rate = item.InternalMarketingPerson.IndexOf(',') > 0
                                          ? rdRateInfo.Rate / 2
                                          : rdRateInfo.Rate;

                    bool calcCommission = ((item.Amount > 0) && (item.TranDesc == "NewCash" || item.TranDesc == "NewAsset"));
                    bool isStartBeforePeriod = ((item.PortStartDate < item.TradeDate) &&
                                                (item.PortStartDate > new DateTime(1900, 1, 1)));


                    var na = new GenevaSmaNewAssetsDataModel()
                    {
                        InternalMarketingPerson       = item.InternalMarketingPerson,
                        Portfolio                     = item.Portfolio,
                        PortShortName                 = item.PortShortName,
                        PortStartDate                 = item.PortStartDate,
                        TradeDate                     = item.TradeDate,
                        TranType                      = item.TranType,
                        Strategy                      = item.Strategy,
                        ConsultantFirm                = item.ConsultantFirm,
                        ConsultantName                = FindAumItemByPortfolioCode(item.Portfolio).ConsultantName,
                        Ticker                        = item.InvCode,
                        Security                      = item.Security,
                        Quantity                      = item.Quantity,
                        Price                         = item.Price,
                        CommissionRate                = rdRateInfo.Rate,
                        CommissionAmount              = calcCommission ? (item.Amount * rdRateInfo.Rate) : 0.0m,
                        IsStartBeforePeriod           = isStartBeforePeriod ? "TRUE" : "FALSE",
                        SumTradeAmountByDate          = null,
                        SumTradeAmountByPortfolioCode = null,
                        IsValid                       = calcCommission ? "TRUE" : "FALSE"
                    };
                    naItems.Add(na);
                }
            }
            return naItems;
        }

        IEnumerable<GenevaNewAssetsSummaryDataModel> BuildNewAssetsSummaryData(IEnumerable<GenevaSmaNewAssetsDataModel> naDetail)
        {
            const LkuRateType.RateType rateType = LkuRateType.RateType.NewAssets;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.SMA;
            var naSumm = new List<GenevaNewAssetsSummaryDataModel>();

            var naSummWorking = naDetail
                               .GroupBy(g => new
                                {
                                    g.InternalMarketingPerson,
                                    g.IsValid,
                                    g.ConsultantFirm,
                                    g.ConsultantName
                                })
                               .Select(group => new
                                {
                                    RD                   = group.Key.InternalMarketingPerson,
                                    IsGood               = group.Key.IsValid,
                                    Firm                 = group.Key.ConsultantFirm,
                                    Name                 = group.Key.ConsultantName,
                                    Amount               = group.Sum(c => c.SumTradeAmountByDate),
                                    Rate                 = group.Min(c => c.CommissionRate),
                                    CommissionAmount     = group.Sum(c => c.SumTradeAmountByDate) * group.Min(c => c.CommissionRate)
                                });

            foreach (var item in naSummWorking)
            {
                var summary = new GenevaNewAssetsSummaryDataModel()
                {
                    RegionalDirectorKey = item.RD,
                    IsTransfer = item.IsGood,
                    ConsultantFirm = item.Firm,
                    ConsultantName = item.Name,
                    PayableAmount = item.Amount ?? 0.0m,
                    Rate = item.Rate,
                    Commission = item.CommissionAmount ?? 0.0m
                };
                
                naSumm.Add(summary);
            }

            return naSumm;
        }
        
        private IEnumerable<GenevaSmaOngoingDetailDataModel> BuildOngoingDetail()
        {
            const LkuRateType.RateType rateType = LkuRateType.RateType.Ongoing;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.SMA;

            IEnumerable<GenevaSmaFlowsDataModel> flowItems = BuildFlowsDetail();
            var ogItems = new List<GenevaSmaOngoingDetailDataModel>();

            foreach (GenevaSmaAumDataModel item in AumItems)
            {
                RegionalDirectorRateInfoDataModel rdRateInfo = RegionalDirectorRateInfo(item.RM, (int)rateType, (int)commissionType);

                if (rdRateInfo != null)
                {
                    rdRateInfo.Rate = item.RM.IndexOf(',') > 0
                                          ? rdRateInfo.Rate / 2
                                          : rdRateInfo.Rate;

                    var flow = FindFlowAmount(item.PortfolioCode);
                    var flowValue = flow.Inflows == null ? 0.0m : flow.Inflows;  
                    
                    var og = new GenevaSmaOngoingDetailDataModel()
                    {
                        SmaOngoing     = item.Id,
                        Portfolio      = item.PortfolioCode,
                        PortShortName  = item.PortfolioName,
                        RM             = item.RM,
                        ConsultantFirm = item.ConsultantFirm,
                        ConsultantName = item.ConsultantName,
                        Strategy       = item.Goal,
                        PortStartDate  = item.Inception,
                        AUM            = item.Total,
                        InFlows        = flowValue,
                        SeasonedValue  = item.Total - flowValue,
                        AnnualRate     = rdRateInfo.Rate / 4,
                        Rate           = rdRateInfo.Rate,
                        Commission     = (item.Total - flowValue) * (rdRateInfo.Rate / 4)
                    };

                    ogItems.Add(og);
                }
            }

            return ogItems;
        }

        private IEnumerable<GenevaSmaOngoingSummaryDataModel> BuildOngoingSummary()
        {
            var ogSummItems = new List<GenevaSmaOngoingSummaryDataModel>();

            return ogSummItems;
        }
        
        private IEnumerable<GenevaSmaFlowsDataModel> BuildFlowsDetail()
        {
            var dal = new DapperDatabaseAccess<GenevaSmaFlowsDataModel>(_connectionString);

            var parms = new DynamicParameters();
            parms.Add("@StartDate", _startDate, DbType.Date, ParameterDirection.Input);
            parms.Add("@EndDate", _endDate, DbType.Date, ParameterDirection.Input);
            
            return dal.SqlServerFetch("dbo.prc_GenevaSma_Ongoing_Flows", parms);
        }
        
        #region Initial data load
        private IEnumerable<GenevaSmaAssetsDataModel> InitNewAssetsItems()
        {
            var dal = new DapperDatabaseAccess<GenevaSmaAssetsDataModel>(_connectionString);

            var parms = new DynamicParameters();
            parms.Add("@StartDate", _startDate, DbType.Date, ParameterDirection.Input);
            parms.Add("@EndDate", _endDate, DbType.Date, ParameterDirection.Input);
            
            return dal.SqlServerFetch("dbo.prc_GenevaSmaAssets_DetailRows", parms);
        }

        private IEnumerable<GenevaSmaAumDataModel> InitAumItems()
        {
            var dal = new DapperDatabaseAccess<GenevaSmaAumDataModel>(_connectionString);

            var parms = new DynamicParameters();
            parms.Add("@StartDate", _startDate, DbType.Date, ParameterDirection.Input);
            parms.Add("@EndDate", _endDate, DbType.Date, ParameterDirection.Input);
            
            return dal.SqlServerFetch("dbo.prc_GenevaAum_DetailRows", parms);
        }
        #endregion

        #region Lookup Data

        private RegionalDirectorRateInfoDataModel RegionalDirectorRateInfo(string rd, int rateType, int commissionType)
        {
            var dal = new DapperDatabaseAccess<RegionalDirectorRateInfoDataModel>(_connectionString);
            var parms = new DynamicParameters();
            parms.Add("@RegionalDirectorKey", rd);
            parms.Add("@RateTypeIid", rateType);
            parms.Add("@CommissionTypeIid", commissionType);
            return dal.SqlServerFetch("dbo.prc_RegionalDirector_RateInfo_Fetch", parms).FirstOrDefault();
        }

        #endregion
    }
}