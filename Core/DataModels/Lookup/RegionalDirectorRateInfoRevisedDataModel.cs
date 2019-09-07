namespace CommissionsStatementGenerator
{
    public class RegionalDirectorRateInfoRevisedDataModel
    {
        public int RegionalDirectorIid { get; set; }
        public string RegionalDirectorKey { get; set; }
        public bool IsGeneva { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SeasoningMonths { get; set; }
        public string Strategy { get; set; }
        public decimal NewAssetRate { get; set; }
        public decimal OngoingRate { get; set; }
    }
}