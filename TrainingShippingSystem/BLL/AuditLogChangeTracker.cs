using System;
using System.Collections.Generic;
using System.Globalization;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.BLL
{
    public static class AuditLogChangeTracker
    {
        // =====================================================
        // UPDATE: compare old and new values
        // =====================================================

        // Compare Client changes
        public static List<AuditLogDetail> Compare(
            Client oldData,
            Client newData)
        {
            var changes = new List<AuditLogDetail>();

            AddIfChanged(changes, "Name", oldData.Name, newData.Name);
            AddIfChanged(changes, "Email", oldData.Email, newData.Email);
            AddIfChanged(changes, "Phone", oldData.Phone, newData.Phone);
            AddIfChanged(changes, "Address", oldData.Address, newData.Address);

            return changes;
        }

        // Compare Voyage changes
        public static List<AuditLogDetail> Compare(
            Voyage oldData,
            Voyage newData)
        {
            var changes = new List<AuditLogDetail>();

            AddIfChanged(changes, "VoyageNumber", oldData.VoyageNumber, newData.VoyageNumber);
            AddIfChanged(changes, "VesselName", oldData.VesselName, newData.VesselName);
            AddIfChanged(changes, "ETA", oldData.ETA, newData.ETA);
            AddIfChanged(changes, "ETD", oldData.ETD, newData.ETD);

            return changes;
        }

        // Compare Bill changes
        public static List<AuditLogDetail> Compare(
            Bill oldData,
            Bill newData)
        {
            var changes = new List<AuditLogDetail>();

            AddIfChanged(changes, "BillNumber", oldData.BillNumber, newData.BillNumber);
            AddIfChanged(changes, "ClientID", oldData.ClientID, newData.ClientID);
            AddIfChanged(changes, "VoyageID", oldData.VoyageID, newData.VoyageID);
            AddIfChanged(changes, "GrossWeight", oldData.GrossWeight, newData.GrossWeight);
            AddIfChanged(changes, "NetWeight", oldData.NetWeight, newData.NetWeight);

            return changes;
        }

        // Compare Container changes
        public static List<AuditLogDetail> Compare(
            Container oldData,
            Container newData)
        {
            var changes = new List<AuditLogDetail>();

            AddIfChanged(changes, "ContainerNumber", oldData.ContainerNumber, newData.ContainerNumber);
            AddIfChanged(changes, "ContainerType", oldData.ContainerType, newData.ContainerType);
            AddIfChanged(changes, "BillID", oldData.BillID, newData.BillID);

            return changes;
        }

        // =====================================================
        // CREATE: values of the new record (stored as NewValue)
        // DELETE: values of the deleted record (stored as OldValue)
        // =====================================================

        public static List<AuditLogDetail> ForCreate(Client data)
        {
            return Snapshot(data, true);
        }

        public static List<AuditLogDetail> ForDelete(Client data)
        {
            return Snapshot(data, false);
        }

        public static List<AuditLogDetail> ForCreate(Voyage data)
        {
            return Snapshot(data, true);
        }

        public static List<AuditLogDetail> ForDelete(Voyage data)
        {
            return Snapshot(data, false);
        }

        public static List<AuditLogDetail> ForCreate(Bill data)
        {
            return Snapshot(data, true);
        }

        public static List<AuditLogDetail> ForDelete(Bill data)
        {
            return Snapshot(data, false);
        }

        public static List<AuditLogDetail> ForCreate(Container data)
        {
            return Snapshot(data, true);
        }

        public static List<AuditLogDetail> ForDelete(Container data)
        {
            return Snapshot(data, false);
        }

        private static List<AuditLogDetail> Snapshot(Client data, bool isCreate)
        {
            var details = new List<AuditLogDetail>();

            if (data == null)
                return details;

            AddSnapshot(details, isCreate, "Name", data.Name);
            AddSnapshot(details, isCreate, "Email", data.Email);
            AddSnapshot(details, isCreate, "Phone", data.Phone);
            AddSnapshot(details, isCreate, "Address", data.Address);

            return details;
        }

        private static List<AuditLogDetail> Snapshot(Voyage data, bool isCreate)
        {
            var details = new List<AuditLogDetail>();

            if (data == null)
                return details;

            AddSnapshot(details, isCreate, "VoyageNumber", data.VoyageNumber);
            AddSnapshot(details, isCreate, "VesselName", data.VesselName);
            AddSnapshot(details, isCreate, "ETA", data.ETA);
            AddSnapshot(details, isCreate, "ETD", data.ETD);

            return details;
        }

        private static List<AuditLogDetail> Snapshot(Bill data, bool isCreate)
        {
            var details = new List<AuditLogDetail>();

            if (data == null)
                return details;

            AddSnapshot(details, isCreate, "BillNumber", data.BillNumber);
            AddSnapshot(details, isCreate, "ClientID", data.ClientID);
            AddSnapshot(details, isCreate, "VoyageID", data.VoyageID);
            AddSnapshot(details, isCreate, "GrossWeight", data.GrossWeight);
            AddSnapshot(details, isCreate, "NetWeight", data.NetWeight);

            return details;
        }

        private static List<AuditLogDetail> Snapshot(Container data, bool isCreate)
        {
            var details = new List<AuditLogDetail>();

            if (data == null)
                return details;

            AddSnapshot(details, isCreate, "ContainerNumber", data.ContainerNumber);
            AddSnapshot(details, isCreate, "ContainerType", data.ContainerType);
            AddSnapshot(details, isCreate, "BillID", data.BillID);

            return details;
        }

        // =====================================================
        // HELPERS
        // =====================================================

        // Add a detail only when the value has changed
        private static void AddIfChanged(
            List<AuditLogDetail> changes,
            string fieldName,
            object oldValue,
            object newValue)
        {
            if (object.Equals(oldValue, newValue))
            {
                return;
            }

            changes.Add(new AuditLogDetail
            {
                FieldName = fieldName,
                OldValue = FormatValue(oldValue, fieldName),
                NewValue = FormatValue(newValue, fieldName)
            });
        }

        // Add one field of a created / deleted record (empty values are skipped)
        private static void AddSnapshot(
            List<AuditLogDetail> details,
            bool isCreate,
            string fieldName,
            object value)
        {
            string text = FormatValue(value, fieldName);

            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            details.Add(new AuditLogDetail
            {
                FieldName = fieldName,
                OldValue = isCreate ? null : text,
                NewValue = isCreate ? text : null
            });
        }

        // Format values for audit details
        private static string FormatValue(
            object value,
            string fieldName)
        {
            if (value == null)
            {
                return null;
            }

            if (value is DateTime)
            {
                DateTime dateValue = (DateTime)value;

                if (fieldName == "ETA" || fieldName == "ETD")
                {
                    return dateValue.ToString(
                        "yyyy-MM-dd hh:mm:ss tt",
                        CultureInfo.InvariantCulture);
                }

                return dateValue.ToString(
                    "yyyy-MM-dd HH:mm:ss",
                    CultureInfo.InvariantCulture);
            }

            return Convert.ToString(
                value,
                CultureInfo.InvariantCulture);
        }
    }
}