namespace Core.DataModels
{
    public class RegionalDirectorRateInfoDataModel
    {
        public int RegionalDirectorIID { get; set; }
        public string RegionalDirectorKey { get; set; }
        public string RegionalDirectorName { get; set; }
        public decimal Rate { get; set; }
        public int SeasoningMonths { get; set; }
    }
}