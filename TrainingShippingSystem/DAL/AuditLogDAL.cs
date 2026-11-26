using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.DAL
{
    public class AuditLogDAL
    {
        private readonly string connectionString;

        public AuditLogDAL()
        {
            connectionString =
                ConfigurationManager
                .ConnectionStrings["TrainingShippingDBConnection"]
                .ConnectionString;
        }

        // INSERT AUDIT LOG
        public int InsertAuditLog(AuditLog auditLog)
        {
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            using (SqlCommand command =
                new SqlCommand("InsertAuditLog", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                AddAuditLogParameters(command, auditLog);

                SqlParameter outputParameter =
                    command.Parameters.Add(
                        "@AuditLogID", SqlDbType.Int);

                outputParameter.Direction =
                    ParameterDirection.Output;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToInt32(outputParameter.Value);
            }
        }

        // INSERT AUDIT LOG DETAIL
        public void InsertAuditLogDetail(AuditLogDetail detail)
        {
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            using (SqlCommand command =
                new SqlCommand("InsertAuditLogDetail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                AddDetailParameters(command, detail);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // INSERT LOG AND DETAILS IN ONE TRANSACTION
        public int LogAction(
            AuditLog auditLog,
            List<AuditLogDetail> details)
        {
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction =
                    connection.BeginTransaction())
                {
                    try
                    {
                        int auditLogID;

                        using (SqlCommand command =
                            new SqlCommand(
                                "InsertAuditLog",
                                connection,
                                transaction))
                        {
                            command.CommandType =
                                CommandType.StoredProcedure;

                            AddAuditLogParameters(command, auditLog);

                            SqlParameter outputParameter =
                                command.Parameters.Add(
                                    "@AuditLogID", SqlDbType.Int);

                            outputParameter.Direction =
                                ParameterDirection.Output;

                            command.ExecuteNonQuery();

                            auditLogID =
                                Convert.ToInt32(outputParameter.Value);
                        }

                        if (details != null)
                        {
                            foreach (AuditLogDetail detail in details)
                            {
                                if (detail == null)
                                    throw new ArgumentException(
                                        "Audit detail cannot be null.");

                                detail.AuditLogID = auditLogID;

                                using (SqlCommand command =
                                    new SqlCommand(
                                        "InsertAuditLogDetail",
                                        connection,
                                        transaction))
                                {
                                    command.CommandType =
                                        CommandType.StoredProcedure;

                                    AddDetailParameters(command, detail);

                                    command.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();

                        return auditLogID;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // GET AUDIT LOGS WITH SEARCH, FILTERS AND PAGINATION
        public AuditLogPagedResult GetAuditLogs(
            AuditLogFilter filter)
        {
            List<AuditLog> auditLogs = new List<AuditLog>();
            int totalRecords = 0;

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            using (SqlCommand command =
                new SqlCommand("GetAuditLogs", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(
                    "@Search", SqlDbType.NVarChar, 200).Value =
                    (object)filter.Search ?? DBNull.Value;

                command.Parameters.Add(
                    "@UserName", SqlDbType.NVarChar, 100).Value =
                    (object)filter.UserName ?? DBNull.Value;

                command.Parameters.Add(
                    "@ModuleName", SqlDbType.NVarChar, 50).Value =
                    (object)filter.ModuleName ?? DBNull.Value;

                command.Parameters.Add(
                    "@ActionType", SqlDbType.NVarChar, 20).Value =
                    (object)filter.ActionType ?? DBNull.Value;

                command.Parameters.Add(
                    "@FromDate", SqlDbType.Date).Value =
                    (object)filter.FromDate ?? DBNull.Value;

                command.Parameters.Add(
                    "@ToDate", SqlDbType.Date).Value =
                    (object)filter.ToDate ?? DBNull.Value;

                command.Parameters.Add(
                    "@PageNumber", SqlDbType.Int).Value =
                    filter.PageNumber;

                command.Parameters.Add(
                    "@PageSize", SqlDbType.Int).Value =
                    filter.PageSize;

                SqlParameter totalParameter =
                    command.Parameters.Add(
                        "@TotalRecords", SqlDbType.Int);

                totalParameter.Direction = ParameterDirection.Output;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        auditLogs.Add(ReadAuditLog(reader));
                    }
                }

                totalRecords =
                    Convert.ToInt32(totalParameter.Value);
            }

            return new AuditLogPagedResult
            {
                Records = auditLogs,
                TotalRecords = totalRecords,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling(
                    (double)totalRecords / filter.PageSize)
            };
        }

        // GET ONE AUDIT LOG BY ID
        public AuditLog GetAuditLogByID(int id)
        {
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            using (SqlCommand command =
                new SqlCommand("GetAuditLogByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(
                    "@AuditLogID", SqlDbType.Int).Value = id;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        return ReadAuditLog(reader);
                }
            }

            return null;
        }

        // GET DETAILS FOR ONE AUDIT LOG
        public List<AuditLogDetail> GetAuditLogDetails(int auditLogID)
        {
            List<AuditLogDetail> details =
                new List<AuditLogDetail>();

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            using (SqlCommand command =
                new SqlCommand("GetAuditLogDetails", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(
                    "@AuditLogID", SqlDbType.Int).Value = auditLogID;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AuditLogDetail detail = new AuditLogDetail
                        {
                            AuditLogDetailID =
                                Convert.ToInt32(reader["AuditLogDetailID"]),

                            AuditLogID =
                                Convert.ToInt32(reader["AuditLogID"]),

                            FieldName = reader["FieldName"].ToString(),

                            OldValue = reader["OldValue"] == DBNull.Value
                                ? null
                                : reader["OldValue"].ToString(),

                            NewValue = reader["NewValue"] == DBNull.Value
                                ? null
                                : reader["NewValue"].ToString()
                        };

                        details.Add(detail);
                    }
                }
            }

            return details;
        }

        // GET DISTINCT USER NAMES
        public List<string> GetAuditUsers()
        {
            List<string> users = new List<string>();

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            using (SqlCommand command =
                new SqlCommand("GetAuditUsers", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["UserName"] != DBNull.Value)
                            users.Add(reader["UserName"].ToString());
                    }
                }
            }

            return users;
        }

        // GET DISTINCT MODULE NAMES
        public List<string> GetAuditModules()
        {
            List<string> modules = new List<string>();

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            using (SqlCommand command =
                new SqlCommand("GetAuditModules", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["ModuleName"] != DBNull.Value)
                            modules.Add(reader["ModuleName"].ToString());
                    }
                }
            }

            return modules;
        }

        // COMMON PARAMETERS FOR AUDIT LOG INSERT
        private void AddAuditLogParameters(
            SqlCommand command,
            AuditLog auditLog)
        {
            command.Parameters.Add(
                "@UserName", SqlDbType.NVarChar, 100).Value =
                (object)auditLog.UserName ?? DBNull.Value;

            command.Parameters.Add(
                "@ModuleName", SqlDbType.NVarChar, 50).Value =
                auditLog.ModuleName;

            command.Parameters.Add(
                "@ActionType", SqlDbType.NVarChar, 20).Value =
                auditLog.ActionType;

            command.Parameters.Add(
                "@RecordID", SqlDbType.Int).Value =
                (object)auditLog.RecordID ?? DBNull.Value;

            command.Parameters.Add(
                "@Description", SqlDbType.NVarChar, 1000).Value =
                auditLog.Description;
        }

        // COMMON PARAMETERS FOR DETAIL INSERT
        private void AddDetailParameters(
            SqlCommand command,
            AuditLogDetail detail)
        {
            command.Parameters.Add(
                "@AuditLogID", SqlDbType.Int).Value =
                detail.AuditLogID;

            command.Parameters.Add(
                "@FieldName", SqlDbType.NVarChar, 100).Value =
                detail.FieldName;

            command.Parameters.Add(
                "@OldValue", SqlDbType.NVarChar, -1).Value =
                (object)detail.OldValue ?? DBNull.Value;

            command.Parameters.Add(
                "@NewValue", SqlDbType.NVarChar, -1).Value =
                (object)detail.NewValue ?? DBNull.Value;
        }

        // MAP SQL DATA READER TO AUDIT LOG MODEL
        private AuditLog ReadAuditLog(SqlDataReader reader)
        {
            return new AuditLog
            {
                AuditLogID = Convert.ToInt32(reader["AuditLogID"]),

                UserName = reader["UserName"] == DBNull.Value
                    ? null
                    : reader["UserName"].ToString(),

                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),

                ModuleName = reader["ModuleName"].ToString(),

                ActionType = reader["ActionType"].ToString(),

                RecordID = reader["RecordID"] == DBNull.Value
                    ? (int?)null
                    : Convert.ToInt32(reader["RecordID"]),

                Description = reader["Description"] == DBNull.Value
                    ? null
                    : reader["Description"].ToString()
            };
        }
    }
}