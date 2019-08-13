using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Aristotle.Excel;
using Core.DataModels;
using Dapper;
using DapperDatabaseAccess;

namespace AcmCommissionsStatements
{
    public class CommissionStatementBase
    {
        protected readonly DateTime              _startDate;
        protected readonly DateTime              _endDate;
        protected readonly string                _connectionString;
        protected readonly TaskMetaDataDataModel _metaData;

        protected AristotleExcel _xl;
        protected string         _workbookFile { get; set; }



        protected CommissionStatementBase(string startDate, string endDate, string connectionString, TaskMetaDataDataModel metaData)
        {
            _startDate        = DateTime.ParseExact(startDate, "yyyyMMdd", null);
            _endDate          = DateTime.ParseExact(endDate, "yyyyMMdd", null);
            _connectionString = connectionString;
            _metaData         = metaData;
        }
        
        protected CommissionStatementBase(string endDate, string connectionString, TaskMetaDataDataModel metaData)
        {
            _endDate          = DateTime.ParseExact(endDate, "yyyyMMdd", null);
            _connectionString = connectionString;
            _metaData         = metaData;
        }

        protected RegionalDirectorRateInfoDataModel RegionalDirectorRateInfo(string rd, int rateType, int commissionType)
        {
            var dal   = new DapperDatabaseAccess<RegionalDirectorRateInfoDataModel>(_connectionString);
            var parms = new DynamicParameters();
            parms.Add("@RegionalDirectorKey", rd);
            parms.Add("@RateTypeIid", rateType);
            parms.Add("@CommissionTypeIid", commissionType);
            var result = dal.SqlServerFetch("dbo.prc_RegionalDirector_RateInfo_Fetch", parms).FirstOrDefault() ?? new RegionalDirectorRateInfoDataModel();

            return result;
        }

        protected IEnumerable<AristotleExcelStyle> ExcelColumnProperties(string reportName)
        {
            var dal   = new DapperDatabaseAccess<AristotleExcelStyle>(_connectionString);
            var parms = new DynamicParameters();
            parms.Add("@ReportName", reportName);
            return dal.SqlServerFetch("dbo.prc_ColumnStyles_Fetch", parms);
        }
    }
}