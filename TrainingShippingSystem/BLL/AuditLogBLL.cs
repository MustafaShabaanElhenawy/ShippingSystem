using System;
using System.Collections.Generic;
using TrainingShippingSystem.DAL;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.BLL
{
    public class AuditLogBLL
    {
        private readonly AuditLogDAL auditLogDAL;

        public AuditLogBLL()
        {
            auditLogDAL = new AuditLogDAL();
        }

        // INSERT AUDIT LOG
        public int AddAuditLog(AuditLog auditLog)
        {
            if (auditLog == null)
            {
                throw new ArgumentException(
                    "Audit log data is required.");
            }

            if (string.IsNullOrWhiteSpace(auditLog.ModuleName))
            {
                throw new ArgumentException(
                    "Module name is required.");
            }

            if (auditLog.ModuleName.Length > 50)
            {
                throw new ArgumentException(
                    "Module name cannot exceed 50 characters.");
            }

            if (string.IsNullOrWhiteSpace(auditLog.ActionType))
            {
                throw new ArgumentException(
                    "Action type is required.");
            }

            string action = auditLog.ActionType.Trim().ToUpperInvariant();

            if (action != "LOGIN" &&
                action != "CREATE" &&
                action != "UPDATE" &&
                action != "DELETE")
            {
                throw new ArgumentException(
                    "Invalid action type.");
            }

            if (auditLog.UserName != null &&
                auditLog.UserName.Length > 100)
            {
                throw new ArgumentException(
                    "Username cannot exceed 100 characters.");
            }

            if (auditLog.RecordID.HasValue &&
                auditLog.RecordID.Value <= 0)
            {
                throw new ArgumentException(
                    "Record ID must be greater than zero.");
            }

            auditLog.ModuleName = auditLog.ModuleName.Trim();
            auditLog.ActionType = action;

            return auditLogDAL.InsertAuditLog(auditLog);
        }


        // INSERT AUDIT LOG DETAIL
        public void AddAuditLogDetail(AuditLogDetail detail)
        {
            if (detail == null)
            {
                throw new ArgumentException(
                    "Audit log detail is required.");
            }

            if (detail.AuditLogID <= 0)
            {
                throw new ArgumentException(
                    "Invalid Audit Log ID.");
            }

            if (string.IsNullOrWhiteSpace(detail.FieldName))
            {
                throw new ArgumentException(
                    "Field name is required.");
            }

            detail.FieldName = detail.FieldName.Trim();

            auditLogDAL.InsertAuditLogDetail(detail);
        }


        // INSERT AUDIT LOG WITH DETAILS


        public int LogAction(
    AuditLog auditLog,
    List<AuditLogDetail> details)
        {
            if (auditLog == null)
                throw new ArgumentException("Audit log data is required.");

            // Reuse the existing validation
            if (string.IsNullOrWhiteSpace(auditLog.ModuleName))
                throw new ArgumentException("Module name is required.");

            if (string.IsNullOrWhiteSpace(auditLog.ActionType))
                throw new ArgumentException("Action type is required.");

            string action = auditLog.ActionType.Trim().ToUpperInvariant();

            if (action != "LOGIN" &&
                action != "CREATE" &&
                action != "UPDATE" &&
                action != "DELETE")
            {
                throw new ArgumentException("Invalid action type.");
            }

            if (auditLog.ModuleName.Trim().Length > 50)
                throw new ArgumentException("Module name is too long.");

            if (auditLog.UserName != null &&
                auditLog.UserName.Length > 100)
                throw new ArgumentException("User name is too long.");

            if (auditLog.RecordID.HasValue &&
                auditLog.RecordID.Value <= 0)
                throw new ArgumentException("Record ID must be positive.");

            if (string.IsNullOrWhiteSpace(auditLog.Description))
                throw new ArgumentException("Description is required.");

            if (auditLog.Description.Length > 1000)
                throw new ArgumentException("Description is too long.");

            auditLog.ModuleName = auditLog.ModuleName.Trim();
            auditLog.ActionType = action;

            if (details != null)
            {
                foreach (AuditLogDetail detail in details)
                {
                    if (detail == null)
                        throw new ArgumentException("Audit detail cannot be null.");

                    if (string.IsNullOrWhiteSpace(detail.FieldName))
                        throw new ArgumentException("Field name is required.");

                    if (detail.FieldName.Trim().Length > 100)
                        throw new ArgumentException("Field name is too long.");

                    detail.FieldName = detail.FieldName.Trim();
                }
            }

            return auditLogDAL.LogAction(auditLog, details);
        }



        public AuditLog GetAuditLogByID(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Audit log ID must be positive.");

            return auditLogDAL.GetAuditLogByID(id);
        }

        public List<AuditLogDetail> GetAuditLogDetails(int auditLogID)
        {
            if (auditLogID <= 0)
                throw new ArgumentException("Audit log ID must be positive.");

            return auditLogDAL.GetAuditLogDetails(auditLogID);
        }

        public List<string> GetAuditUsers()
        {
            return auditLogDAL.GetAuditUsers();
        }

        public List<string> GetAuditModules()
        {
            return auditLogDAL.GetAuditModules();
        }


        // GET AUDIT LOGS WITH FILTERS
        public AuditLogPagedResult GetAuditLogs(
            AuditLogFilter filter)
        {
            if (filter == null)
            {
                filter = new AuditLogFilter();
            }

            if (filter.PageNumber < 1)
            {
                throw new ArgumentException(
                    "Page number must be greater than zero.");
            }

            if (filter.PageSize < 1 || filter.PageSize > 100)
            {
                throw new ArgumentException(
                    "Page size must be between 1 and 100.");
            }

            if (filter.FromDate.HasValue &&
                filter.ToDate.HasValue &&
                filter.FromDate.Value.Date >
                filter.ToDate.Value.Date)
            {
                throw new ArgumentException(
                    "From Date cannot be greater than To Date.");
            }

            if (!string.IsNullOrWhiteSpace(filter.ActionType))
            {
                string action =
                    filter.ActionType.Trim().ToUpperInvariant();

                if (action != "LOGIN" &&
                    action != "CREATE" &&
                    action != "UPDATE" &&
                    action != "DELETE")
                {
                    throw new ArgumentException(
                        "Invalid action type.");
                }

                filter.ActionType = action;
            }

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                filter.Search = filter.Search.Trim();
            }

            if (!string.IsNullOrWhiteSpace(filter.UserName))
            {
                filter.UserName = filter.UserName.Trim();
            }

            if (!string.IsNullOrWhiteSpace(filter.ModuleName))
            {
                filter.ModuleName = filter.ModuleName.Trim();
            }

            return auditLogDAL.GetAuditLogs(filter);
        }
    }
}




