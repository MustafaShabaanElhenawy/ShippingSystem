namespace TrainingShippingSystem.Models
{
    public class AuditLogDetail
    {
        public int AuditLogDetailID { get; set; }

        public int AuditLogID { get; set; }

        public string FieldName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
}