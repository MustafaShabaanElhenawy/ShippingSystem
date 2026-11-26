using System;

namespace TrainingShippingSystem.Models
{
    public class AuditLog
    {
        public int AuditLogID { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ModuleName { get; set; }

        public string ActionType { get; set; }

        public int? RecordID { get; set; }

        public string Description { get; set; }
    }
}