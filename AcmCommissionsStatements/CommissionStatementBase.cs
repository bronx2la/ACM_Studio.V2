using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Aristotle.Excel;
using CommissionsStatementGenerator;
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
        
        protected IEnumerable<RegionalDirectorRateInfoRevisedDataModel> _rateInfo { get; set; }
        protected IEnumerable<RegionalDirectorDataModel> _regionalDirector { get; set; }

        protected AristotleExcel _xl;
        protected string         _workbookFile { get; set; }



        protected CommissionStatementBase(string startDate, string endDate, string connectionString, TaskMetaDataDataModel metaData)
        {
            _startDate        = DateTime.ParseExact(startDate, "yyyyMMdd", null);
            _endDate          = DateTime.ParseExact(endDate, "yyyyMMdd", null);
            _connectionString = connectionString;
            _metaData         = metaData;
            _rateInfo = InitRateInfo();
            _regionalDirector = InitRegionalDirectors();
        }
        
        protected CommissionStatementBase(string endDate, string connectionString, TaskMetaDataDataModel metaData)
        {
            _endDate          = DateTime.ParseExact(endDate, "yyyyMMdd", null);
            _connectionString = connectionString;
            _metaData         = metaData;
            _rateInfo = InitRateInfo();
            _rateInfo = InitRateInfo();
            _regionalDirector = InitRegionalDirectors();
        }

        protected IEnumerable<RegionalDirectorDataModel> InitRegionalDirectors()
        {
            try
            {
                var dal = new DapperDatabaseAccess<RegionalDirectorDataModel>(_connectionString);
                return dal.SqlServerFetch("dbo.prc_RegionalDirector_Fetch_All", null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        protected IEnumerable<RegionalDirectorRateInfoRevisedDataModel> InitRateInfo()
        {
            try
            {
                var dal = new DapperDatabaseAccess<RegionalDirectorRateInfoRevisedDataModel>(_connectionString);
                return dal.SqlServerFetch("dbo.prc_RegionalDirector_RateInfo_Revised_Fetch_All", null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected RegionalDirectorRateInfoRevisedDataModel GetRateInfo(string rdkey, string strategy, bool isGeneva = true)
        {
            try
            {
                var rd = _regionalDirector.FirstOrDefault(p => p.RegionalDirectorKey == rdkey);
                if (rd != null)
                {
                    return _rateInfo.FirstOrDefault(p =>p.RegionalDirectorIid == rd.RegionalDirectorIid && p.Strategy == strategy && p.IsGeneva == isGeneva);
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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

        protected RegionalDirectorRateInfoRevisedDataModel RegionalDirectorRateInfoRevised(string rdkey, bool isNewAsset, bool isGeneva, string strategy)
        {

            return null;
        }
        
        protected IEnumerable<RegionalDirectorRateInfoRevisedDataModel> LoadRegionalDirectorRateInfoRevised()
        {
            var dal   = new DapperDatabaseAccess<RegionalDirectorRateInfoRevisedDataModel>(_connectionString);
            var result = dal.SqlServerFetch("dbo.prc_RegionalDirector_RateInfo_Revised_Fetch_All", null);

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