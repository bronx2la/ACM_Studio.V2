using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Aristotle.Excel;
using CommissionsStatementGenerator;
using Core.DataModels;
using Core.Enums;
using Dapper;
using DapperDatabaseAccess;


namespace AcmCommissionsStatements
{
    public class GenevaSmaStatement : CommissionStatementBase
    {
        private IEnumerable<GenevaSmaAssetsDataModel> _genevaSales { get; set; }
        private IEnumerable<GenevaFlows> _genevaFlows { get; set; }
        private IEnumerable<GenevaSmaAumDataModel> _aumItems { get; set; }
        

    public GenevaSmaStatement(string startDate, string endDate, string connectionString, TaskMetaDataDataModel metaData) : base(startDate, endDate, connectionString, metaData)
        {
            _genevaSales = InitNewAssetsItems();
            _genevaFlows = InitNewAssetsFlows();
            _aumItems    = InitAumItems();
        }

        protected void InitializeSalesData(string endDate)
        {
            var dal = new DapperDatabaseAccess<GenevaSmaAssetsDataModel>(_connectionString);

            var parms = new DynamicParameters();
            parms.Add("@ReportingDate", endDate);

            _genevaSales = dal.SqlServerFetch("dbo.prc_GenevaSmaAssets_DetailRows", parms).Distinct();
        }

        protected void InitializeAumData(string endDate)
        {
            var dal = new DapperDatabaseAccess<GenevaSmaAumDataModel>(_connectionString);

            var parms = new DynamicParameters();
            parms.Add("@ReportingDate", endDate);

            _aumItems = dal.SqlServerFetch("dbo.prc_GenevaAum", parms);
        }


        public void ProduceReport()
        {
            //NewAssets Detail
            IEnumerable<GenevaSmaNewAssetsDataModel> naDetail = BuildNewAssetsData();
            //NewAssets Summary
            IEnumerable<GenevaNewAssetsSummaryDataModel> naSummary = BuildNewAssetsSummaryData(naDetail);
//            //Ongoing Detail
            IEnumerable<GenevaSmaOngoingDetailDataModel> ogDetail = BuildOngoingDetail();
//            //Ongoing Summary
            IEnumerable<GenevaSmaOngoingSummaryDataModel> ogSummary = BuildOngoingSummary();

            IEnumerable<TopLevelSummaryDataModel> tlSummary = BuildTopLevelSummary(naSummary, ogDetail);
            
            
            _workbookFile = $@"{_metaData.outboundFolder}\{Core.FileNameHelpers.FormatOutboundFileName(_metaData.outboundFile)}";
            _xl           = new AristotleExcel(_workbookFile);
            _xl.AddWorksheet(tlSummary, "Top_Level_Summary", ExcelColumnProperties("GevevaSma.TopLevelSummary"));
            _xl.AddWorksheet(naSummary, "SMA_NewAssets_Summary", ExcelColumnProperties("GevevaSma.NewAssetsSummary"));
            _xl.AddWorksheet(ogSummary, "SMA_Ongoing_Summary", ExcelColumnProperties("GevevaSma.OngoingSummary"));
            _xl.AddWorksheet(naDetail, "SMA_NewAssets_Detail", ExcelColumnProperties("GevevaSma.NewAssetsDetail"));
            _xl.AddWorksheet(ogDetail, "SMA_Ongoing_Detail", ExcelColumnProperties("GevevaSma.OngoingDetail"));

            _xl.SaveWorkbook();
        }

        private GenevaSmaAumDataModel FindAumItemByPortfolioCode(string portfolioCode)
        {
            try
            {
                var result = _aumItems.Where(r => _aumItems.Any(p => r.PortfolioCode.Trim().Equals(portfolioCode))).FirstOrDefault();
                
                if(result != null)
                    return result;
                else
                {
                    return new GenevaSmaAumDataModel()
                    {
                        PortfolioCode = "UNK Like a Mug"
                    };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private GenevaFlows FindFlowAmount(string portfolioCode)
        {
            if (_genevaFlows != null)
            {
                IEnumerable<GenevaFlows> flow =
                    from item in _genevaFlows
                    where item.Portfolio == portfolioCode
                    select item;

                return flow.FirstOrDefault();
            }

            return new GenevaFlows();
        }

        private IEnumerable<GenevaSmaNewAssetsDataModel> BuildNewAssetsData()
        {
            const LkuRateType.RateType rateType = LkuRateType.RateType.NewAssets;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.SMA;
            RegionalDirectorRateInfoDataModel rdRateInfo;
            
            try
            {
                var naItems = new List<GenevaSmaNewAssetsDataModel>();

                foreach (GenevaSmaAssetsDataModel item in _genevaSales)
                {
                    bool calcCommission = ((item.Amount > 0) && (item.TranDesc == "NewCash" || item.TranDesc == "NewAsset"));
                    bool isStartBeforePeriod = ((item.PortStartDate < item.TradeDate) &&
                                                (item.PortStartDate > new DateTime(1900, 1, 1)));

                    var rateInfo = GetRateInfo(item.InternalMarketingPerson, item.Strategy);
                    var theRate = rateInfo?.NewAssetRate ?? 0.0m;

                    var na = new GenevaSmaNewAssetsDataModel()
                    {
                        InternalMarketingPerson       = item.InternalMarketingPerson,
                        Portfolio                     = item.Portfolio,
                        PortShortName                 = item.PortShortName,
                        ConsultantFirm                = item.ConsultantFirm,
                        ConsultantName                = FindAumItemByPortfolioCode(item.Portfolio.Trim()).ConsultantName,
                        Strategy                      = item.Strategy,
                        PortStartDate                 = item.PortStartDate,
                        TradeDate                     = item.TradeDate,
                        TranType                      = item.TranType,
                        InvCode                       = item.InvCode,
                        Quantity                      = item.Quantity,
                        Price                         = item.Price,
                        Amount                        = item.Amount,
                        AmountRemoved                 = null,
                        CommissionAmount              = calcCommission ? (item.Amount * theRate) : 0.0m,
                        CommissionRate                = theRate,
                        SalesTeam                     = item.SalesTeam,
                        IsStartBeforePeriod           = isStartBeforePeriod ? "TRUE" : "FALSE"
                    };
                    naItems.Add(na);
                }
                return naItems.OrderBy(c=>c.InternalMarketingPerson).ThenBy(c=>c.ConsultantFirm).ThenBy(c=>c.Portfolio);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
                    g.ConsultantFirm,
                    g.ConsultantName,
                    g.Strategy
                })
                .Select(group => new
                {
                    RD = group.Key.InternalMarketingPerson,
                    Firm = group.Key.ConsultantFirm,
                    Name = group.Key.ConsultantName,
                    Strategy = group.Key.Strategy,
                    Quantity = group.Sum(c => c.Quantity),
                    Amount = group.Sum(c => c.Amount),
                    Rate = group.Min(c => c.CommissionRate),
                    CommissionAmount = group.Sum(c => c.Amount) * group.Min(c => c.CommissionRate)
                });

            foreach (var item in naSummWorking)
            {
                var summary = new GenevaNewAssetsSummaryDataModel()
                {
                    RowLabels = item.RD,
                    ConsultantFirm = item.Firm,
                    ConsultantName = item.Name,
                    Strategy = item.Strategy,
                    Amount = item.Amount,
                    CommissionRate = item.Rate,
                    CommissionAmount = item.CommissionAmount
                };

                naSumm.Add(summary);
            }

            //Totals lines
            var naSummTotals = naSumm
                               .GroupBy(g => new
                                {
                                    g.RowLabels
                                })
                               .Select(group => new
                                {
                                    RD               = group.Key.RowLabels,
                                    IsGood           = "-----",
                                    Firm             = "-----",
                                    Name             = "-----",
                                    Strategy         = "-----",
                                    Amount           = group.Sum(c => c.Amount),
                                    Rate             = group.Min(c => c.CommissionRate),
                                    CommissionAmount = group.Sum(c => c.CommissionAmount)
                                });

            foreach (var sa in naSummTotals)
            {
                var summary = new GenevaNewAssetsSummaryDataModel()
                {
                    RowLabels = sa.RD,
                    ConsultantFirm      = "z--Totals",
                    Amount       = sa.Amount,
                    Strategy = "",
                    CommissionRate                = sa.Rate,
                    CommissionAmount          = sa.CommissionAmount,
                };

                naSumm.Add(summary);
            }

            return naSumm.OrderBy(c=>c.RowLabels).ThenBy(c=>c.ConsultantFirm);
        }

        private IEnumerable<GenevaSmaOngoingDetailDataModel> BuildOngoingDetail()
        {
            const LkuRateType.RateType rateType = LkuRateType.RateType.Ongoing;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.SMA;

            var ogItems = new List<GenevaSmaOngoingDetailDataModel>();

            foreach (GenevaSmaAumDataModel item in _aumItems.Where(o=>o.Inception < _startDate))
            {
                var flow = FindFlowAmount(item.PortfolioCode);
                var flowValue = flow?.FlowAmount ?? 0.0m;

                var rateInfo = GetRateInfo(item.IntMktPerson, item.Goal);
                var theRate = rateInfo?.OngoingRate ?? 0.0m;
                
                var og = new GenevaSmaOngoingDetailDataModel()
                {
                    RegionalDirector = item.IntMktPerson,
                    IntMktPerson = item.IntMktPerson,
                    Portfolio      = item.PortfolioCode,
                    PortShortName  = item.PortfolioName,
                    Inception  = item.Inception,
                    Goal       = item.Goal,
                    ConsultantFirm = item.ConsultantFirm,
                    ConsultantName = item.ConsultantName,
                    ReportEndDate  = _endDate,
                    Total            = item.Total,
                    InFlows        = flowValue,
                    SeasonedValue  = item.Total - flowValue,
                    Commission     = (item.Total - flowValue) * (theRate / 4),
                    Rate           = theRate / 4,
                    AnnualRate     = theRate
                };

                ogItems.Add(og);
            }

            return ogItems;
        }

        private IEnumerable<GenevaSmaOngoingSummaryDataModel> BuildOngoingSummary()
        {
            const LkuRateType.RateType             rateType       = LkuRateType.RateType.Ongoing;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.SMA;
            var naSumm = new List<GenevaSmaOngoingSummaryDataModel>();

            IEnumerable<GenevaSmaOngoingDetailDataModel> details = BuildOngoingDetail();
            var naSummWorking = details
                               .GroupBy(g => new
                                {
                                    g.RegionalDirector,
                                    g.ConsultantFirm,
                                    g.ConsultantName,
                                    g.Goal
                                })
                               .Select(group => new
                                {
                                    RD               = group.Key.RegionalDirector,
                                    Firm             = group.Key.ConsultantFirm,
                                    Name             = group.Key.ConsultantName,
                                    Goal             = group.Key.Goal,
                                    Total            = group.Sum(c => c.Total),
                                    Inflows          = group.Sum(c => c.InFlows),
                                    SeasonedValue    = group.Sum(c => c.SeasonedValue),
                                    Rate             = group.Min(c => c.Rate),
                                    CommissionAmount = group.Sum(c => c.SeasonedValue) * group.Min(c => c.Rate)
                                });

            foreach (var item in naSummWorking) 
            {
                var summary = new GenevaSmaOngoingSummaryDataModel()
                {
                    RowLabels = item.RD,
                    ConsultantFirm   = item.Firm,
                    ConsultantName   = item.Name,
                    Goal = item.Goal,
                    Total             = item.Total,
                    Inflows          = item.Inflows,
                    SeasonedValue    = item.SeasonedValue,
                    Commission       = item.CommissionAmount,
                    Rate             = item.Rate,
                    AnnualRate       = item.Rate * 4
                };
                naSumm.Add(summary);
            }

            //Totals lines
            var naSummTotals = naSumm
                              .GroupBy(g => new
                               {
                                   g.RowLabels
                               })
                              .Select(group => new
                               {
                                   RD             = @group.Key.RowLabels,
                                   ConsultantFirm = "-----",
                                   ConsultantName = "-----",
                                   Goal = "-----",
                                   Total            = @group.Sum(c=>c.Total),
                                   InFlows        = @group.Sum(c=>c.Inflows),
                                   SeasonedValue  = @group.Sum(c=>c.SeasonedValue),
                                   Rate           = @group.Max(c=>c.Rate),
                                   Commission     = @group.Sum(c=>c.SeasonedValue) * group.Max(c=>c.Rate) 
                               });

            naSumm.AddRange(naSummTotals.Select(sa => new GenevaSmaOngoingSummaryDataModel()
            {
                RowLabels = sa.RD,
                ConsultantFirm   = "z--Totals",
                ConsultantName   = "-----",
                Goal = "-----",
                Total              = sa.Total,
                Inflows          = sa.InFlows,
                SeasonedValue    = sa.SeasonedValue,
                Commission       = sa.Commission
            }));

            return naSumm.OrderBy(c=>c.RowLabels).ThenBy(c=>c.ConsultantFirm);
        }

        private IEnumerable<TopLevelSummaryDataModel> BuildTopLevelSummary(IEnumerable<GenevaNewAssetsSummaryDataModel> naSummary, IEnumerable<GenevaSmaOngoingDetailDataModel> ogDetail)
        {
            var nasumm = naSummary
                .Where(f => f.ConsultantFirm == "z--Totals")
                .GroupBy(g => new
                {
                    g.RowLabels, g.ConsultantFirm
                })
                .Select(group => new
                {
                    RD = group.Key.RowLabels,
                    Amount = group.Sum(c => c.Amount),
                    Rate = 0.0m,
                    CommissionAmount = group.Sum(c => c.CommissionAmount),
                }).Distinct();

            var topLevelSum = nasumm.Select(item => new TopLevelSummaryDataModel() {RegionalDirector = item.RD, Heading = "New Assets", Amount = item.Amount, Commission = item.CommissionAmount, AverageRate = 0.0m}).ToList().Distinct();

            var ogsumm = ogDetail
                .GroupBy(k => new
                {
                    k.RegionalDirector//, k.ConsultantFirm
                })
                .Select(group => new
                {
                    RD = group.Key.RegionalDirector,
                    Amount = group.Sum(c => c.SeasonedValue),
                    Rate = 0.0,
                    CommissionAmount = group.Sum(c => c.SeasonedValue) * group.Average(c => c.Rate)
                }).Distinct();
                
            var topLevelSummaryDataModels = topLevelSum.ToList();
            
            topLevelSum = ogsumm.Select(item => new TopLevelSummaryDataModel() {RegionalDirector = item.RD, Heading = "Ongoing", Amount = item.Amount, Commission = item.CommissionAmount, AverageRate = 0.0m});
            topLevelSummaryDataModels.AddRange(topLevelSum);

            var rdList = topLevelSummaryDataModels.Select(c => c.RegionalDirector).Distinct().ToList();

            foreach (var rd in rdList)
            {
                var tls = new TopLevelSummaryDataModel()
                {
                    RegionalDirector = rd,
                    Heading = "Totals",
                    Amount = topLevelSummaryDataModels.Where(a => a.RegionalDirector == rd).Sum(a => a.Amount),
                    Commission = topLevelSummaryDataModels.Where(a => a.RegionalDirector == rd).Sum(a => a.Commission),
                };
                tls.AverageRate = tls.Amount != 0 ? tls.Commission / tls.Amount : 0.0m;
                topLevelSummaryDataModels.Add(tls);
            }
            
            return topLevelSummaryDataModels.OrderBy(c=>c.RegionalDirector).ThenBy(c=>c.Heading);
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

            return dal.SqlServerFetch("dbo.prc_GenevaSmaAssets_DetailRows", parms).Distinct();
        }

        private IEnumerable<GenevaFlows> InitNewAssetsFlows()
        {
            var dal = new DapperDatabaseAccess<GenevaFlows>(_connectionString);

            var parms = new DynamicParameters();
            parms.Add("@ReportEndDate", _endDate, DbType.Date, ParameterDirection.Input);

            return dal.SqlServerFetch("dbo.prc_GenevaFlows", parms);
        }

        private IEnumerable<GenevaSmaAumDataModel> InitAumItems()
        {
            try
            {
                var dal = new DapperDatabaseAccess<GenevaSmaAumDataModel>(_connectionString);

                var parms = new DynamicParameters();
                parms.Add("@EndDate", _endDate, DbType.Date, ParameterDirection.Input);

                return dal.SqlServerFetch("dbo.prc_GenevaAum_DetailRows", parms);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion
    }
}