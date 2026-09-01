using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.DAL
{
    public class ClientDAL
    {
        private readonly string connectionString;

        public ClientDAL()
        {
            connectionString =
                ConfigurationManager
                .ConnectionStrings["TrainingShippingDBConnection"]
                .ConnectionString;
        }

        // GET ALL CLIENTS
        public List<Client> GetAll()
        {
            List<Client> clients = new List<Client>();

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("GetClients", connection))
                {
                    command.CommandType =
                        CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader =
                        command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Client client = new Client();

                            client.ID = Convert.ToInt32(reader["ID"]);
                            client.Name = reader["Name"].ToString();
                            client.Email = reader["Email"].ToString();
                            client.Phone = reader["Phone"].ToString();
                            client.Address = reader["Address"].ToString();

                            clients.Add(client);
                        }
                    }
                }
            }

            return clients;
        }


        // GET CLIENT BY ID
        public Client GetByID(int id)
        {
            Client client = null;

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("GetClientByID", connection))
                {
                    command.CommandType =
                        CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ID", id);

                    connection.Open();

                    using (SqlDataReader reader =
                        command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            client = new Client();

                            client.ID =
                                Convert.ToInt32(reader["ID"]);

                            client.Name =
                                reader["Name"].ToString();

                            client.Email =
                                reader["Email"].ToString();

                            client.Phone =
                                reader["Phone"].ToString();

                            client.Address =
                                reader["Address"].ToString();
                        }
                    }
                }
            }

            return client;
        }


        // INSERT CLIENT
        public int Insert(Client client)
        {
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("InsertClient", connection))
                {
                    command.CommandType =
                        CommandType.StoredProcedure;

                    command.Parameters.AddWithValue(
                        "@Name",
                        client.Name);

                    command.Parameters.AddWithValue(
                        "@Email",
                        client.Email);

                    command.Parameters.AddWithValue(
                        "@Phone",
                        (object)client.Phone ?? DBNull.Value);

                    command.Parameters.AddWithValue(
                        "@Address",
                        (object)client.Address ?? DBNull.Value);

                    connection.Open();

                    int newID =
                        Convert.ToInt32(command.ExecuteScalar());

                    return newID;
                }
            }
        }


        // UPDATE CLIENT
        public bool Update(Client client)
        {
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("UpdateClient", connection))
                {
                    command.CommandType =
                        CommandType.StoredProcedure;

                    command.Parameters.AddWithValue(
                        "@ID",
                        client.ID);

                    command.Parameters.AddWithValue(
                        "@Name",
                        client.Name);

                    command.Parameters.AddWithValue(
                        "@Email",
                        client.Email);

                    command.Parameters.AddWithValue(
                        "@Phone",
                        (object)client.Phone ?? DBNull.Value);

                    command.Parameters.AddWithValue(
                        "@Address",
                        (object)client.Address ?? DBNull.Value);

                    connection.Open();

                    int rowsAffected =
                        Convert.ToInt32(command.ExecuteScalar());

                    return rowsAffected > 0;
                }
            }
        }


        // DELETE CLIENT
        public bool Delete(int id)
        {
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("DeleteClient", connection))
                {
                    command.CommandType =
                        CommandType.StoredProcedure;

                    command.Parameters.AddWithValue(
                        "@ID",
                        id);

                    connection.Open();

                    int rowsAffected =
                        Convert.ToInt32(command.ExecuteScalar());

                    return rowsAffected > 0;
                }
            }
        }


        // SEARCH CLIENTS
        public List<Client> Search(string search)
        {
            List<Client> clients = new List<Client>();

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("SearchClients", connection))
                {
                    command.CommandType =
                        CommandType.StoredProcedure;

                    command.Parameters.AddWithValue(
                        "@Search",
                        search);

                    connection.Open();

                    using (SqlDataReader reader =
                        command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Client client = new Client();

                            client.ID =
                                Convert.ToInt32(reader["ID"]);

                            client.Name =
                                reader["Name"].ToString();

                            client.Email =
                                reader["Email"].ToString();

                            client.Phone =
                                reader["Phone"].ToString();

                            client.Address =
                                reader["Address"].ToString();

                            clients.Add(client);
                        }
                    }
                }
            }

            return clients;
        }
    }
}