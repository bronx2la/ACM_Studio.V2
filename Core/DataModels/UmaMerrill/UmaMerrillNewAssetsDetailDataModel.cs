using System;

namespace Core.DataModels.UmaMerrill
{
    public class UmaMerrillNewAssetsDetailDataModel
    {
        public string UniqueId { get; set; }
        public DateTime Inception { get; set; }
        public string FAName { get; set; }
        public string Office { get; set; }
        public string OfficeState { get; set; }
        public string RC { get; set; }
        public decimal MarketValue { get; set; }
        public decimal Rate { get; set; }
        public decimal Commission { get; set; }
        public string IsNewAsset { get; set; }
        public DateTime AristotleStartDate { get; set; }
        public string IsGrayStone { get; set; }
        public string IsXfer { get; set; }
    }
}