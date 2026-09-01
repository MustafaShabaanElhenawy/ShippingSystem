using System;

namespace TrainingShippingSystem.Models
{
    public class Bill
    {
        public int ID { get; set; }

        public string BillNumber { get; set; }

        public int ClientID { get; set; }

        public int VoyageID { get; set; }

        public decimal GrossWeight { get; set; }

        public decimal NetWeight { get; set; }
    }
}