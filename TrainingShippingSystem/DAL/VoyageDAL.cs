using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.DAL
{
    public class VoyageDAL
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["TrainingShippingDBConnection"].ConnectionString;


        // GET ALL
        public List<Voyage> GetVoyages()
        {
            List<Voyage> voyages = new List<Voyage>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetVoyages", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            voyages.Add(new Voyage
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                VoyageNumber = reader["VoyageNumber"].ToString(),
                                VesselName = reader["VesselName"].ToString(),
                                ETA = Convert.ToDateTime(reader["ETA"]),
                                ETD = Convert.ToDateTime(reader["ETD"])
                            });
                        }
                    }
                }
            }

            return voyages;
        }


        // GET BY ID
        public Voyage GetVoyageByID(int id)
        {
            Voyage voyage = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetVoyageByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ID", id);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            voyage = new Voyage
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                VoyageNumber = reader["VoyageNumber"].ToString(),
                                VesselName = reader["VesselName"].ToString(),
                                ETA = Convert.ToDateTime(reader["ETA"]),
                                ETD = Convert.ToDateTime(reader["ETD"])
                            };
                        }
                    }
                }
            }

            return voyage;
        }


        // INSERT
        public int InsertVoyage(Voyage voyage)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("InsertVoyage", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@VoyageNumber", voyage.VoyageNumber);
                    command.Parameters.AddWithValue("@VesselName", voyage.VesselName);
                    command.Parameters.AddWithValue("@ETA", voyage.ETA);
                    command.Parameters.AddWithValue("@ETD", voyage.ETD);

                    connection.Open();

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }


        // UPDATE
        public bool UpdateVoyage(Voyage voyage)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("UpdateVoyage", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ID", voyage.ID);
                    command.Parameters.AddWithValue("@VoyageNumber", voyage.VoyageNumber);
                    command.Parameters.AddWithValue("@VesselName", voyage.VesselName);
                    command.Parameters.AddWithValue("@ETA", voyage.ETA);
                    command.Parameters.AddWithValue("@ETD", voyage.ETD);

                    connection.Open();

                    int rowsAffected = Convert.ToInt32(command.ExecuteScalar());

                    return rowsAffected > 0;
                }
            }
        }


        // DELETE
        public bool DeleteVoyage(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("DeleteVoyage", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ID", id);

                    connection.Open();

                    int rowsAffected = Convert.ToInt32(command.ExecuteScalar());

                    return rowsAffected > 0;
                }
            }
        }


        // SEARCH
        public List<Voyage> SearchVoyages(string search)
        {
            List<Voyage> voyages = new List<Voyage>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SearchVoyages", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Search", search);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            voyages.Add(new Voyage
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                VoyageNumber = reader["VoyageNumber"].ToString(),
                                VesselName = reader["VesselName"].ToString(),
                                ETA = Convert.ToDateTime(reader["ETA"]),
                                ETD = Convert.ToDateTime(reader["ETD"])
                            });
                        }
                    }
                }
            }

            return voyages;
        }
    }
}