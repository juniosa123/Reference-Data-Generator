namespace wedoIT.CATS.Tools.ReferenceDataGenerator.Model
{
    public class ReferenceDataEvents
    {
        public ReferenceDataEvents()
        {
            AggregateVersion = 1;
            IdentityTenant = "00000000000000000000000000000000";
            IdentityUser = "00000000000000000000000000000000";
            EventClass = "wedoIT.CATS.ReferenceData.Domain.Events.MaintenanceReferenceDataCreatedEvent, wedoIT.CATS.ReferenceData.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            EventType = "MaintenanceReferenceDataCreatedEvent";
            EventTime = 1617069364175;
        }
        public string AggregateIdentifier { get; set; }
        public int AggregateVersion { get; set; }
        public string IdentityTenant { get; set; }
        public string IdentityUser { get; set; }
        public string EventClass { get; set; }
        public string EventType { get; set; }
        public string EventData { get; set; }
        public long EventTime { get; set; }
    }
}
