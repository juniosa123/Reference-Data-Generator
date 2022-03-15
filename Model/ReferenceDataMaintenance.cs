namespace wedoIT.CATS.Tools.ReferenceDataGenerator.Model
{
    public class ReferenceDataMaintenance
    {
        public ReferenceDataMaintenance()
        {
            AggregateVersion = 1;
            ValidFrom = "2008-01-01";
            ValidTo = "null";
        }
        public string RecordId { get; set; }
        public string AggregateIdentifier { get; set; }
        public int AggregateVersion { get; set; }
        public string ReferenceDataType { get; set; }
        public string Code { get; set; }
        public string ParameterData { get; set; }
        public string Localization { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
    }
}
