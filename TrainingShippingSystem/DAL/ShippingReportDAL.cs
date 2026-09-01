using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.DAL
{
    public class ShippingReportDAL
    {
        private readonly string connectionString;

        public ShippingReportDAL()
        {
            connectionString =
                ConfigurationManager
                .ConnectionStrings["TrainingShippingDBConnection"]
                .ConnectionString;
        }

        public List<ShippingReport> GetShippingSummary()
        {
            List<ShippingReport> reports =
                new List<ShippingReport>();

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                string query =
                    "SELECT * FROM vw_BillShippingSummary ORDER BY BillID DESC";

                SqlCommand command =
                    new SqlCommand(query, connection);

                connection.Open();

                SqlDataReader reader =
                    command.ExecuteReader();

                while (reader.Read())
                {
                    ShippingReport report =
                        new ShippingReport();

                    report.BillID =
                        (int)reader["BillID"];

                    report.BillNumber =
                        reader["BillNumber"].ToString();

                    report.ClientID =
                        (int)reader["ClientID"];

                    report.ClientName =
                        reader["ClientName"].ToString();

                    report.VoyageID =
                        (int)reader["VoyageID"];

                    report.VoyageNumber =
                        reader["VoyageNumber"].ToString();

                    report.VesselName =
                        reader["VesselName"].ToString();

                    report.ETA =
                        (System.DateTime)reader["ETA"];

                    report.ETD =
                        (System.DateTime)reader["ETD"];

                    report.GrossWeight =
                        (decimal)reader["GrossWeight"];

                    report.NetWeight =
                        (decimal)reader["NetWeight"];

                    report.ContainerCount =
                        (int)reader["ContainerCount"];

                    reports.Add(report);
                }
            }

            return reports;
        }
    }
}