using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.DAL
{
    public class ContainerDAL
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["TrainingShippingDBConnection"].ConnectionString;


        // GET ALL
        public List<Container> GetContainers()
        {
            List<Container> containers = new List<Container>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("GetContainers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            containers.Add(new Container
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                ContainerNumber =
                                    reader["ContainerNumber"].ToString(),
                                ContainerType =
                                    reader["ContainerType"].ToString(),
                                BillID =
                                    Convert.ToInt32(reader["BillID"])
                            });
                        }
                    }
                }
            }

            return containers;
        }


        // GET BY ID
        public Container GetContainerByID(int id)
        {
            Container container = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("GetContainerByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ID", id);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            container = new Container
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                ContainerNumber =
                                    reader["ContainerNumber"].ToString(),
                                ContainerType =
                                    reader["ContainerType"].ToString(),
                                BillID =
                                    Convert.ToInt32(reader["BillID"])
                            };
                        }
                    }
                }
            }

            return container;
        }


        // INSERT
        public int InsertContainer(Container container)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("InsertContainer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue(
                        "@ContainerNumber",
                        container.ContainerNumber);

                    command.Parameters.AddWithValue(
                        "@ContainerType",
                        container.ContainerType);

                    command.Parameters.AddWithValue(
                        "@BillID",
                        container.BillID);

                    connection.Open();

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }


        // UPDATE
        public bool UpdateContainer(Container container)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("UpdateContainer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue(
                        "@ID",
                        container.ID);

                    command.Parameters.AddWithValue(
                        "@ContainerNumber",
                        container.ContainerNumber);

                    command.Parameters.AddWithValue(
                        "@ContainerType",
                        container.ContainerType);

                    command.Parameters.AddWithValue(
                        "@BillID",
                        container.BillID);

                    connection.Open();

                    int rowsAffected =
                        Convert.ToInt32(command.ExecuteScalar());

                    return rowsAffected > 0;
                }
            }
        }


        // DELETE
        public bool DeleteContainer(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("DeleteContainer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ID", id);

                    connection.Open();

                    int rowsAffected =
                        Convert.ToInt32(command.ExecuteScalar());

                    return rowsAffected > 0;
                }
            }
        }


        // SEARCH
        public List<Container> SearchContainers(string search)
        {
            List<Container> containers = new List<Container>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("SearchContainers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Search", search);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            containers.Add(new Container
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                ContainerNumber =
                                    reader["ContainerNumber"].ToString(),
                                ContainerType =
                                    reader["ContainerType"].ToString(),
                                BillID =
                                    Convert.ToInt32(reader["BillID"])
                            });
                        }
                    }
                }
            }

            return containers;
        }
    }
}