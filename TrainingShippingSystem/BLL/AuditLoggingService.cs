using System;
using System.Collections.Generic;
using System.Diagnostics;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.BLL
{
    public class AuditLoggingService
    {
        private readonly AuditLogBLL auditLogBLL;

        public AuditLoggingService()
        {
            auditLogBLL = new AuditLogBLL();
        }


        // GENERAL AUDIT LOGGING
        // The real operation (create / update / delete) has already succeeded when
        // this is called, so a failure here must not turn it into an error for
        // the user. The failure is traced instead of being silently ignored.
        // Returns the new audit log ID, or 0 when logging failed.
        public int Log(
            string userName,
            string moduleName,
            string actionType,
            int? recordID,
            string description,
            List<AuditLogDetail> details = null)
        {
            try
            {
                AuditLog auditLog = new AuditLog();

                auditLog.UserName = userName;
                auditLog.ModuleName = moduleName;
                auditLog.ActionType = actionType;
                auditLog.RecordID = recordID;
                auditLog.Description = description;

                return auditLogBLL.LogAction(
                    auditLog,
                    details);
            }
            catch (Exception ex)
            {
                Trace.TraceError(
                    "Audit logging failed ({0} {1} #{2}): {3}",
                    moduleName,
                    actionType,
                    recordID,
                    ex);

                return 0;
            }
        }


        // LOG LOGIN
        public int LogLogin(string userName)
        {
            return Log(
                userName,
                "Auth",
                "LOGIN",
                null,
                "User logged in: " + userName);
        }


        // LOG CREATE (details = the values of the new record)
        public int LogCreate(
            string userName,
            string moduleName,
            int recordID,
            string description,
            List<AuditLogDetail> details = null)
        {
            return Log(
                userName,
                moduleName,
                "CREATE",
                recordID,
                description,
                details);
        }


        // LOG UPDATE
        public int LogUpdate(
            string userName,
            string moduleName,
            int recordID,
            string description,
            List<AuditLogDetail> details)
        {
            return Log(
                userName,
                moduleName,
                "UPDATE",
                recordID,
                description,
                details);
        }


        // LOG DELETE (details = the values of the deleted record)
        public int LogDelete(
            string userName,
            string moduleName,
            int recordID,
            string description,
            List<AuditLogDetail> details = null)
        {
            return Log(
                userName,
                moduleName,
                "DELETE",
                recordID,
                description,
                details);
        }
    }
}