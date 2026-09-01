using System;

namespace TrainingShippingSystem.Models
{
    public class ShippingReport
    {
        public int BillID { get; set; }

        public string BillNumber { get; set; }

        public int ClientID { get; set; }

        public string ClientName { get; set; }

        public int VoyageID { get; set; }

        public string VoyageNumber { get; set; }

        public string VesselName { get; set; }

        public DateTime ETA { get; set; }

        public DateTime ETD { get; set; }

        public decimal GrossWeight { get; set; }

        public decimal NetWeight { get; set; }

        public int ContainerCount { get; set; }
    }
}