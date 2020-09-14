using System;

namespace AcmDataModels
{
    public class Class1
    {
        public int RegionalDirectorIid { get; set; }
        public string RegionalDirectorKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public short SeasoningMonths { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int RegionalDirectorRateRevisedIid { get; set; }
        public bool IsGeneva { get; set; }
        public string Strategy { get; set; }
        public decimal NewAssetRate { get; set; }
        public decimal OngoingRate { get; set; }
    }
}