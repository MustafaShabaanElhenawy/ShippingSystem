using System;

namespace TrainingShippingSystem.Models
{
    public class Voyage
    {
        public int ID { get; set; }

        public string VoyageNumber { get; set; }

        public string VesselName { get; set; }

        public DateTime ETA { get; set; }

        public DateTime ETD { get; set; }
    }
}