using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.DAL
{
    public class BillDAL
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["TrainingShippingDBConnection"].ConnectionString;


        // GET ALL
        public List<Bill> GetBills()
        {
            List<Bill> bills = new List<Bill>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetBills", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bills.Add(new Bill
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                BillNumber = reader["BillNumber"].ToString(),
                                ClientID = Convert.ToInt32(reader["ClientID"]),
                                VoyageID = Convert.ToInt32(reader["VoyageID"]),
                                GrossWeight = Convert.ToDecimal(reader["GrossWeight"]),
                                NetWeight = Convert.ToDecimal(reader["NetWeight"])
                            });
                        }
                    }
                }
            }

            return bills;
        }


        // GET BY ID
        public Bill GetBillByID(int id)
        {
            Bill bill = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetBillByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ID", id);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bill = new Bill
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                BillNumber = reader["BillNumber"].ToString(),
                                ClientID = Convert.ToInt32(reader["ClientID"]),
                                VoyageID = Convert.ToInt32(reader["VoyageID"]),
                                GrossWeight = Convert.ToDecimal(reader["GrossWeight"]),
                                NetWeight = Convert.ToDecimal(reader["NetWeight"])
                            };
                        }
                    }
                }
            }

            return bill;
        }


        // INSERT
        public int InsertBill(Bill bill)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("InsertBill", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@BillNumber", bill.BillNumber);
                    command.Parameters.AddWithValue("@ClientID", bill.ClientID);
                    command.Parameters.AddWithValue("@VoyageID", bill.VoyageID);
                    command.Parameters.AddWithValue("@GrossWeight", bill.GrossWeight);
                    command.Parameters.AddWithValue("@NetWeight", bill.NetWeight);

                    connection.Open();

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }


        // UPDATE
        public bool UpdateBill(Bill bill)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("UpdateBill", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ID", bill.ID);
                    command.Parameters.AddWithValue("@BillNumber", bill.BillNumber);
                    command.Parameters.AddWithValue("@ClientID", bill.ClientID);
                    command.Parameters.AddWithValue("@VoyageID", bill.VoyageID);
                    command.Parameters.AddWithValue("@GrossWeight", bill.GrossWeight);
                    command.Parameters.AddWithValue("@NetWeight", bill.NetWeight);

                    connection.Open();

                    int rowsAffected = Convert.ToInt32(command.ExecuteScalar());

                    return rowsAffected > 0;
                }
            }
        }


        // DELETE
        public bool DeleteBill(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("DeleteBill", connection))
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
        public List<Bill> SearchBills(string search)
        {
            List<Bill> bills = new List<Bill>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SearchBills", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Search", search);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bills.Add(new Bill
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                BillNumber = reader["BillNumber"].ToString(),
                                ClientID = Convert.ToInt32(reader["ClientID"]),
                                VoyageID = Convert.ToInt32(reader["VoyageID"]),
                                GrossWeight = Convert.ToDecimal(reader["GrossWeight"]),
                                NetWeight = Convert.ToDecimal(reader["NetWeight"])
                            });
                        }
                    }
                }
            }

            return bills;
        }
    }
}