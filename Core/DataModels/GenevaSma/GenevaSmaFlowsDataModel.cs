using System;

namespace Core.DataModels
{
    public class GenevaSmaFlowsDataModel
    {
        public string Portfolio { get; set; }
        public string PortShortName { get; set; }
        public string RM { get; set; }
        public string ConsultantFirm { get; set; }
        public string Strategy { get; set; }
        public DateTime PortStartDate { get; set; }
        public decimal AUM { get; set; }
        public decimal Inflows { get; set; }
    }
}