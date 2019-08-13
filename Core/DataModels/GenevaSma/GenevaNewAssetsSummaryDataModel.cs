namespace Core.DataModels
{
    public class GenevaNewAssetsSummaryDataModel
    {
        public string RegionalDirectorKey { get; set; }
        public string ConsultantFirm { get; set; }
        public string ConsultantName { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal Commission { get; set; }
        public decimal Rate { get; set; }
        public string IsValid { get; set; }
    }
}