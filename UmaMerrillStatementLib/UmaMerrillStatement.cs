using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using Aristotle.Excel;
using Core.DataModels;
using Core.DataModels.UmaMerrill;
using Core.Enums;
using Dapper;
using DapperDatabaseAccess;

namespace UmaMerrillStatementLib
{
    public class UmaMerrillStatement
    {
        private IEnumerable<UmaMerrillNewAssetsDetailDataModel> _newAssetsDetail { get; set; }
        private IEnumerable<UmaMerrillDataModel> _merrillData { get; set; }
        
        private readonly DateTime              _startDate;
        private readonly DateTime              _endDate;
        private readonly string                _connectionString;
        private readonly TaskMetaDataDataModel _metaData;
        private          AristotleExcel        _xl;
        private string _workbookFile { get; set; }

        public UmaMerrillStatement(string startDate, string endDate, string connectionString, TaskMetaDataDataModel metaData)
        {
            _startDate        = DateTime.ParseExact(startDate, "yyyyMMdd", null);
            _endDate          = DateTime.ParseExact(endDate, "yyyyMMdd", null);
            _connectionString = connectionString;
            _metaData         = metaData;

            _merrillData = InitMerrillData();
            _newAssetsDetail = BuildNewAssetsDetail();
        }
        
        #region Build datasets
        

        private IEnumerable<UmaMerrillNewAssetsSummaryDataModel> BuildNewAssetsSummary(IEnumerable<UmaMerrillNewAssetsDetailDataModel> naDetail)
        {
            const LkuRateType.RateType rateType = LkuRateType.RateType.NewAssets;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;
            var naSumm = new List<UmaMerrillNewAssetsSummaryDataModel>();

            var naSummWorking = _newAssetsDetail
               .GroupBy(g => new
                {
                    g.RC,
                    g.Office,
                    g.FAName
                })
               .Select(group => new
                {
                    RegionalDirectorKey = group.Key.RC,
                    OfficeStateCoe = group.Key.
                    Office = group.Key.Office,
                });

            return naSumm;
        }
        
        
        #endregion

        public void ProduceReport()
        {
            //NewAssets Detail
            //NewAssets Summary
            //Ongoing Detail
            //Ongoing Summary
            //PseudoFlows
            //PseudoFlowsSummary
            
            _workbookFile = $@"{_metaData.outboundFolder}\{Core.FileNameHelpers.FormatOutboundFileName(_metaData.outboundFile)}";
            _xl = new AristotleExcel(_workbookFile);
            _xl.AddWorksheet(_newAssetsDetail, "SMA_NewAssets_Summary", SetNewAssetsSummaryColumnFormatProperties());
            _xl.AddWorksheet(_newAssetsDetail, "SMA_NewAssets_Detail", SetNewAssetsDetailColumnFormatProperties());

            _xl.SaveWorkbook();
        }

        private IEnumerable<AristotleExcelStyle> SetNewAssetsDetailColumnFormatProperties()
        {
            var _list = new List<AristotleExcelStyle>();
            var _columnStyle = new AristotleExcelStyle()
            {
                Column      = 2,
                Format      = "YYYY-mm-dd",
                ColumnWidth = 18,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                Column      = 7,
                Format      = "#,##0.00",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                Column      = 8,
                Format      = "#0.000000",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                Column      = 9,
                Format      = "#,##0.00",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                Column      = 11,
                Format      = "YYYY-mm-dd",
                ColumnWidth = 18,
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
                Column      = 5,
                Format      = "#,##0.00",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                Column      = 6,
                Format      = "#0.000000",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                Column      = 7,
                Format      = "#,##0.00",
                ColumnWidth = 25,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);
            
            _columnStyle = new AristotleExcelStyle()
            {
                Column      = 8,
                Format      = "YYYY-mm-dd",
                ColumnWidth = 18,
                FontColor   = Color.Black
            };
            _list.Add(_columnStyle);

            return _list;
        }
        
        #region Initial data load

        private IEnumerable<UmaMerrillDataModel> InitMerrillData()
        {
            var dal = new DapperDatabaseAccess<UmaMerrillDataModel>(_connectionString);

            var parms = new DynamicParameters();
            parms.Add("@StartDate", _startDate, DbType.Date, ParameterDirection.Input);
            parms.Add("@EndDate", _endDate, DbType.Date, ParameterDirection.Input);
            return dal.SqlServerFetch("dbo.prc_Uma_Merrill_Data_Fetch", parms);
        }

        
        #endregion
        
        #region Lookup Data

        private RegionalDirectorRateInfoDataModel RegionalDirectorRateInfo(string rd, int rateType, int commissionType)
        {
            var dal   = new DapperDatabaseAccess<RegionalDirectorRateInfoDataModel>(_connectionString);
            var parms = new DynamicParameters();
            parms.Add("@RegionalDirectorKey", rd);
            parms.Add("@RateTypeIid", rateType);
            parms.Add("@CommissionTypeIid", commissionType);
            return dal.SqlServerFetch("dbo.prc_RegionalDirector_RateInfo_Fetch", parms).FirstOrDefault();
        }

        private IEnumerable<UmaMerrillNewAssetsDetailDataModel> BuildNewAssetsDetail()
        {
            const LkuRateType.RateType             rateType       = LkuRateType.RateType.NewAssets;
            const LkuCommissionType.CommissionType commissionType = LkuCommissionType.CommissionType.UMA;
            var                                    naItems        = new List<UmaMerrillNewAssetsDetailDataModel>();
            
            foreach (UmaMerrillDataModel item in _merrillData)
            {
                if ( item.AsOfDate >= _startDate && item.AsOfDate <= _endDate )
                {
                    RegionalDirectorRateInfoDataModel rdRateInfo =
                        RegionalDirectorRateInfo(item.RegionalDirector, (int) rateType, (int) commissionType);
                    if (rdRateInfo != null)
                    {
                        rdRateInfo.Rate = item.RegionalDirector.IndexOf(',') > 0
                                              ? rdRateInfo.Rate / 2
                                              : rdRateInfo.Rate;

                        var na = new UmaMerrillNewAssetsDetailDataModel()
                        {
                            UniqueId           = item.UniqueID,
                            Inception          = item.StrategyEnrollmentDate,
                            Office             = item.Office,
                            OfficeState        = item.OfficeStateCode,
                            RC                 = item.RegionalDirector,
                            MarketValue        = item.TotalAssets,
                            Rate               = rdRateInfo.Rate,
                            Commission         = item.TotalAssets * rdRateInfo.Rate,
                            IsNewAsset         = "TRUE",
                            AristotleStartDate = item.StrategyEnrollmentDate,
                            IsGrayStone        = null,
                            IsXfer             = null
                        };
                        naItems.Add(na);
                    }
                }
            }

            return naItems;
        }
        
        #endregion
    }
}