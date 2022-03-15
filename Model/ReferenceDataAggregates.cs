namespace wedoIT.CATS.Tools.ReferenceDataGenerator.Model
{
    public class ReferenceDataAggregates
    {
        public ReferenceDataAggregates()
        {
            AggregateClass = "wedoIT.CATS.ReferenceData.Domain.Aggregates.ReferenceDataAggregate";
            AggregateType = "ReferenceData";
            TenantIdentifier = "00000000000000000000000000000000";
            AggregateExpires = "null";
        }
        public string AggregateIdentifier { get; set; }
        public string AggregateClass { get; set; }
        public string AggregateType { get; set; }
        public string TenantIdentifier { get; set; }
        public string AggregateExpires { get; set; }
    }
}
