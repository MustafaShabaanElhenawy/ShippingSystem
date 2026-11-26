using System;

namespace TrainingShippingSystem.Models
{
    public class AuditLogFilter
    {
        public string Search { get; set; }

        public string UserName { get; set; }

        public string ModuleName { get; set; }

        public string ActionType { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}