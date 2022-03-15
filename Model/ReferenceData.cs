using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wedoIT.CATS.Tools.ReferenceDataGenerator.Model
{
    public class ReferenceData
    {
        public ReferenceData()
        {
            Aggregates = new ReferenceDataAggregates();
            Events = new ReferenceDataEvents();
            Maintenances = new ReferenceDataMaintenance();
            ListCurrent = new List<ReferenceDataCurrents>();
        }
        public ReferenceDataAggregates Aggregates { get; set; }
        public ReferenceDataEvents Events { get; set; }
        public ReferenceDataMaintenance Maintenances { get; set; }
        public List<ReferenceDataCurrents> ListCurrent { get; set; }

    }
}
