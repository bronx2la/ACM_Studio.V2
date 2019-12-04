namespace Core.DataModels
{
    public class GenevaNewAssetsSummaryDataModel
    {
        public string RowLabels { get; set; }
        public string ConsultantFirm { get; set; }
        public string ConsultantName { get; set; }
        public string Strategy { get; set; }
        public decimal Amount { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal? CommissionRate { get; set; }
        
    }
}