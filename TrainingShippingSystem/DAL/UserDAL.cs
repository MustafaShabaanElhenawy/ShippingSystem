using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.DAL
{
    public class UserDAL
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["TrainingShippingDBConnection"].ConnectionString;

        public UserCredentials GetByUsername(string username)
        {
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("GetUserByUsername", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 100).Value = username;

                con.Open();

                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        return null;

                    return new UserCredentials
                    {
                        ID = (int)r["ID"],
                        Username = r["Username"].ToString(),
                        FullName = r["FullName"].ToString(),
                        Role = r["Role"].ToString(),
                        IsActive = (bool)r["IsActive"],
                        PasswordHash = (byte[])r["PasswordHash"],
                        PasswordSalt = r["PasswordSalt"] == DBNull.Value
                            ? null
                            : (byte[])r["PasswordSalt"]
                    };
                }
            }
        }

        // Throws SqlException (2601 / 2627) if the username already exists.
        public int Register(string username, string fullName, byte[] hash, byte[] salt)
        {
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("RegisterUser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 100).Value = username;
                cmd.Parameters.Add("@FullName", SqlDbType.NVarChar, 200).Value = fullName;
                cmd.Parameters.Add("@PasswordHash", SqlDbType.VarBinary, 32).Value = hash;
                cmd.Parameters.Add("@PasswordSalt", SqlDbType.VarBinary, 16).Value = salt;

                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public void UpdatePassword(int id, byte[] hash, byte[] salt)
        {
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("UpdateUserPassword", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@PasswordHash", SqlDbType.VarBinary, 32).Value = hash;
                cmd.Parameters.Add("@PasswordSalt", SqlDbType.VarBinary, 16).Value = salt;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}