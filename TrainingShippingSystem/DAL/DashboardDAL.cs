using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.DAL
{
    public class DashboardDAL
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["TrainingShippingDBConnection"].ConnectionString;

        public DashboardStatistics GetDashboardStatistics()
        {
            DashboardStatistics statistics = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(
                    "GetDashboardStatistics", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            statistics = new DashboardStatistics
                            {
                                TotalClients = reader["TotalClients"] != DBNull.Value
                                    ? Convert.ToInt32(reader["TotalClients"])
                                    : 0,

                                TotalVoyages = reader["TotalVoyages"] != DBNull.Value
                                    ? Convert.ToInt32(reader["TotalVoyages"])
                                    : 0,

                                TotalBills = reader["TotalBills"] != DBNull.Value
                                    ? Convert.ToInt32(reader["TotalBills"])
                                    : 0,

                                TotalContainers = reader["TotalContainers"] != DBNull.Value
                                    ? Convert.ToInt32(reader["TotalContainers"])
                                    : 0
                            };
                        }
                    }
                }
            }

            return statistics;
        }
    }
}