using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Aristotle.Excel;
using Core.DataModels;
using Core.Enums;
using Dapper;
using DapperDatabaseAccess;


namespace AcmCommissionsStatements
{
    public class GenevaSmaStatement : CommissionStatementBase
    {
        private IEnumerable<GenevaSmaAssetsDataModel> _genevaSales { get; set; }
        private IEnumerable<GenevaFlows>              _genevaFlows { get; set; }
        private IEnumerable<GenevaSmaAumDataModel>    _aumItems    { get; set; }
        private IEnumerable<GenevaSmaFlowsDataModel>  FlowsItems   { get; set; }

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

            _genevaSales = dal.SqlServerFetch("dbo.prc_GenevaSmaAssets_DetailRows", parms);
        }

        protected void InitializeAumData(string endDate)
        {
            var dal = new DapperDatabaseAccess<GenevaSmaAumDataModel>(_connectionString);

            var parms = new DynamicParameters();
            parms.Add("@ReportingDate", endDate);

            _aumItems = dal.SqlServerFetch("dbo.prc_GenevaAum", parms);
//            _aumItems = dal.SqlServerFetch("dbo.prc_GenevaAum_DetailRows", parms);
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
            _xl           = new AristotleExcel(_workbookFile);
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

                foreach (GenevaSmaAssetsDataModel item in _genevaSales.Distinct())
                {
                    rdRateInfo = RegionalDirectorRateInfo(item.InternalMarketingPerson, (int)rateType, (int)commissionType);

                    if(rdRateInfo != null)
                    {
                        rdRateInfo.Rate = item.InternalMarketingPerson.IndexOf(',') > 0
                                              ? rdRateInfo.Rate / 2
                                              : rdRateInfo.Rate;

//                        bool calcCommission = item.Amount > 0;
                        bool calcCommission = ((item.Amount > 0) && (item.TranDesc == "NewCash" || item.TranDesc == "NewAsset"));
                        bool isStartBeforePeriod = ((item.PortStartDate < item.TradeDate) &&
                                                    (item.PortStartDate > new DateTime(1900, 1, 1)));

//                        var cn = FindAumItemByPortfolioCode(item.Portfolio.Trim()).ConsultantName;

                        var na = new GenevaSmaNewAssetsDataModel()
                        {
                            RegionalDirector              = item.InternalMarketingPerson,
                            Portfolio                     = item.Portfolio,
                            PortShortName                 = item.PortShortName,
                            ConsultantFirm                = item.ConsultantFirm,
                            ConsultantName                = FindAumItemByPortfolioCode(item.Portfolio.Trim()).ConsultantName,
                            Strategy                      = item.Strategy,
                            InteralMarketingPerson        = item.InternalMarketingPerson,
                            PortStartDate                 = item.PortStartDate,
                            TradeDate                     = item.TradeDate,
                            TranType                      = item.TranType,
                            Trandesc                      = item.TranDesc,
                            InvCode                       = item.InvCode,
                            Quantity                      = item.Quantity,
                            Price                         = item.Price,
                            Amount                        = item.Amount,
                            CommissionAmount              = calcCommission ? (item.Amount * rdRateInfo.Rate) : 0.0m,
                            CommissionRate                = rdRateInfo.Rate,
                            SalesTeam                     = item.SalesTeam,
                            IsStartBeforePeriod           = isStartBeforePeriod ? "TRUE" : "FALSE"
                        };
                        naItems.Add(na);
                    }
                }
                return naItems.OrderBy(c=>c.RegionalDirector).ThenBy(c=>c.ConsultantFirm).ThenBy(c=>c.Portfolio);
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
                    g.RegionalDirector,
                    g.ConsultantFirm,
                    g.ConsultantName
                })
                .Select(group => new
                {
                    RD = group.Key.RegionalDirector,
                    Firm = group.Key.ConsultantFirm,
                    Name = group.Key.ConsultantName,
                    Quantity = group.Sum(c => c.Quantity),
                    Amount = group.Sum(c => c.Amount),
                    Rate = group.Min(c => c.CommissionRate),
                    CommissionAmount = group.Sum(c => c.Amount) * group.Min(c => c.CommissionRate)
                });

            foreach (var item in naSummWorking)
            {
                var summary = new GenevaNewAssetsSummaryDataModel()
                {
                    RegionalDirectorKey = item.RD,
                    ConsultantFirm = item.Firm,
                    ConsultantName = item.Name,
                    PayableAmount = item.Amount,
                    Rate = item.Rate,
                    Commission = item.CommissionAmount
                };

                naSumm.Add(summary);
            }

            //Totals lines
            var naSummTotals = naSumm
                               .GroupBy(g => new
                                {
                                    g.RegionalDirectorKey
                                })
                               .Select(group => new
                                {
                                    RD               = group.Key.RegionalDirectorKey,
                                    IsGood           = "-----",
                                    Firm             = "-----",
                                    Name             = "-----",
                                    Amount           = group.Sum(c => c.PayableAmount),
                                    Rate             = group.Min(c => c.Rate),
                                    CommissionAmount = group.Sum(c => c.Commission)
                                });

            foreach (var sa in naSummTotals)
            {
                var summary = new GenevaNewAssetsSummaryDataModel()
                {
                    RegionalDirectorKey = sa.RD,
                    ConsultantFirm      = "z--Totals",
                    PayableAmount       = sa.Amount,
                    Rate                = sa.Rate,
                    Commission          = sa.CommissionAmount,
                };

                naSumm.Add(summary);
            }

            return naSumm.OrderBy(c=>c.RegionalDirectorKey).ThenBy(c=>c.ConsultantFirm);
        }

        private IEnumerable<GenevaSmaOngoingDetailDataModel> BuildOngoingDetail()
        {
            const LkuRateType.RateType rateType = LkuRateType.RateType.Ongoing;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.SMA;

            var ogItems = new List<GenevaSmaOngoingDetailDataModel>();

            foreach (GenevaSmaAumDataModel item in _aumItems)
            {
                RegionalDirectorRateInfoDataModel rdRateInfo = RegionalDirectorRateInfo(item.IntMktPerson, (int)rateType, (int)commissionType);

                if (rdRateInfo != null)
                {
                    rdRateInfo.Rate = item.IntMktPerson.IndexOf(',') > 0
                                          ? rdRateInfo.Rate / 2
                                          : rdRateInfo.Rate;

                    GenevaFlows flow = FindFlowAmount(item.PortfolioCode);
                    decimal flowValue = flow?.FlowAmount ?? 0.0m;

                    var og = new GenevaSmaOngoingDetailDataModel()
                    {
                        RM             = item.IntMktPerson,
                        Portfolio      = item.PortfolioCode,
                        PortShortName  = item.PortfolioName,
                        PortStartDate  = item.Inception,
                        Strategy       = item.Goal,
                        ConsultantFirm = item.ConsultantFirm,
                        ConsultantName = item.ConsultantName,
                        ReportEndDate  = _endDate,
                        AUM            = item.Total,
                        InFlows        = flowValue,
                        SeasonedValue  = item.Total - flowValue,
                        Commission     = (item.Total - flowValue) * (rdRateInfo.Rate / 4),
                        Rate           = rdRateInfo.Rate / 4,
                        AnnualRate     = rdRateInfo.Rate
                    };

                    ogItems.Add(og);
                }
                else
                {
                    Console.WriteLine(item.IntMktPerson);
                    Console.WriteLine(item.Inception);
                    Console.WriteLine("--------");
                }
            }

            return ogItems;
//            return ogItems.Where(c=>c.SeasonedValue >= 0);
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
                                    g.RM,
                                    g.ConsultantFirm,
                                    g.ConsultantName
                                })
                               .Select(group => new
                                {
                                    RD               = group.Key.RM,
                                    Firm             = group.Key.ConsultantFirm,
                                    Name             = group.Key.ConsultantName,
                                    AUM              = group.Sum(c => c.AUM),
                                    Inflows          = group.Sum(c => c.InFlows),
                                    SeasonedValue    = group.Sum(c => c.SeasonedValue),
                                    Rate             = group.Min(c => c.Rate),
                                    CommissionAmount = group.Sum(c => c.SeasonedValue) * group.Min(c => c.Rate)
                                });

            foreach (var item in naSummWorking)
            {
                var summary = new GenevaSmaOngoingSummaryDataModel()
                {
                    RegionalDirector = item.RD,
                    ConsultantFirm   = item.Firm,
                    ConsultantName   = item.Name,
                    AUM              = item.AUM,
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
                                   g.RegionalDirector
                               })
                              .Select(group => new
                               {
                                   RD             = @group.Key.RegionalDirector,
                                   ConsultantFirm = "-----",
                                   ConsultantName = "-----",
                                   AUM            = @group.Sum(c=>c.AUM),
                                   InFlows        = @group.Sum(c=>c.Inflows),
                                   SeasonedValue  = @group.Sum(c=>c.SeasonedValue),
                                   Rate           = @group.Min(c=>c.Rate),
                                   Commission     = @group.Sum(c=>c.SeasonedValue) * @group.Min(c=>c.Rate)
                               });

            naSumm.AddRange(naSummTotals.Select(sa => new GenevaSmaOngoingSummaryDataModel()
            {
                RegionalDirector = sa.RD,
                ConsultantFirm   = "z--Totals",
                ConsultantName   = "-----",
                AUM              = sa.AUM,
                Inflows          = sa.InFlows,
                SeasonedValue    = sa.SeasonedValue,
                Commission       = sa.Commission
            }));

            return naSumm.OrderBy(c=>c.RegionalDirector).ThenBy(c=>c.ConsultantFirm);
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
//                parms.Add("@StartDate", _startDate, DbType.Date, ParameterDirection.Input);
                parms.Add("@EndDate", _endDate, DbType.Date, ParameterDirection.Input);
//                parms.Add("@ReportEndDate", _endDate, DbType.Date, ParameterDirection.Input);

                    var x = dal.SqlServerFetch("dbo.prc_GenevaAum_DetailRows", parms);
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