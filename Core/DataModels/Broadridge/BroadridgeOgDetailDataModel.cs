using System;

namespace Core.DataModels.Broadridge
{
    public class BroadridgeOgDetailDataModel
    {
        public int      BroadridgeOngoing   { get; set; }
        public string   Portfolio           { get; set; }
        public string   PortShortName       { get; set; }
        public string   ProductName         { get; set; }
        public string   RM                  { get; set; }
        public string Region { get; set; }
        public string   LastName            { get; set; }
        public string   ConsultantFirm      { get; set; }
        public string   Strategy            { get; set; }
        public DateTime PortStartDate       { get; set; }
        public decimal  AUM                 { get; set; }
        public decimal  InFlows             { get; set; }
        public decimal  SeasonedValue       { get; set; }
        public decimal  AnnualRate          { get; set; }
        public decimal  Rate                { get; set; }
        public decimal  Commission          { get; set; }
        public bool IsOngoing { get; set; }
    }
}