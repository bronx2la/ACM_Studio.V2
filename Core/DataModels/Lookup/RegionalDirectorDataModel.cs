using System;

namespace CommissionsStatementGenerator
{
    public class RegionalDirectorDataModel
    {
        public int RegionalDirectorIid { get; set; }
        public string RegionalDirectorKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SeasoningMonths { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}