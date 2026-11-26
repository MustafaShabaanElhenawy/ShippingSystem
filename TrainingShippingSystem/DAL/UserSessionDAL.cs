using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TrainingShippingSystem.DAL
{
    public class UserSessionInfo
    {
        public string Username { get; set; }
        public string Role { get; set; }
    }

    public class UserSessionDAL
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["TrainingShippingDBConnection"].ConnectionString;

        public void Create(byte[] tokenHash, int userId, DateTime expiresAtUtc)
        {
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("CreateUserSession", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TokenHash", SqlDbType.VarBinary, 32).Value = tokenHash;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                cmd.Parameters.Add("@ExpiresAt", SqlDbType.DateTime2).Value = expiresAtUtc;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Returns null when the token is unknown, expired, or the user is inactive.
        public UserSessionInfo GetByTokenHash(byte[] tokenHash)
        {
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("GetUserSession", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TokenHash", SqlDbType.VarBinary, 32).Value = tokenHash;

                con.Open();

                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        return null;

                    return new UserSessionInfo
                    {
                        Username = r["Username"].ToString(),
                        Role = r["Role"].ToString()
                    };
                }
            }
        }

        public void Delete(byte[] tokenHash)
        {
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("DeleteUserSession", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TokenHash", SqlDbType.VarBinary, 32).Value = tokenHash;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}