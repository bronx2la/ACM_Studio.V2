using System;

namespace Core.DataModels
{
    public class GenevaFlows
    {
        public string Portfolio { get; set; }
        public DateTime PortStartDate { get; set; }
        public decimal FlowAmount { get; set; }
    }
}