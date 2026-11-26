using System.Collections.Generic;

namespace TrainingShippingSystem.Models
{
    public class AuditLogPagedResult
    {
        public List<AuditLog> Records { get; set; }

        public int TotalRecords { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }
    }
}