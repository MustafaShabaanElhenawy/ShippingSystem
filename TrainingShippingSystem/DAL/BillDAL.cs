


//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Data.SqlClient;
//using TrainingShippingSystem.Models;

//namespace TrainingShippingSystem.DAL
//{
//    public class BillsDAL
//    {
//        private readonly string connectionString =
//            ConfigurationManager.ConnectionStrings[
//                "TrainingShippingDBConnection"
//            ].ConnectionString;


//        public List<Bill> GetBills()
//        {
//            List<Bill> bills = new List<Bill>();

//            using (SqlConnection connection =
//                new SqlConnection(connectionString))
//            {
//                using (SqlCommand command =
//                    new SqlCommand("GetBills", connection))
//                {
//                    command.CommandType =
//                        CommandType.StoredProcedure;

//                    connection.Open();

//                    using (SqlDataReader reader =
//                        command.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            bills.Add(new Bill
//                            {
//                                ID =
//                                    Convert.ToInt32(reader["ID"]),

//                                BillNumber =
//                                    reader["BillNumber"].ToString(),

//                                ClientID =
//                                    Convert.ToInt32(reader["ClientID"]),

//                                VoyageID =
//                                    Convert.ToInt32(reader["VoyageID"]),

//                                GrossWeight =
//                                    Convert.ToDecimal(
//                                        reader["GrossWeight"]
//                                    ),

//                                NetWeight =
//                                    Convert.ToDecimal(
//                                        reader["NetWeight"]
//                                    )
//                            });
//                        }
//                    }
//                }
//            }

//            return bills;
//        }


//        public Bill GetBillByID(int id)
//        {
//            Bill bill = null;

//            using (SqlConnection connection =
//                new SqlConnection(connectionString))
//            {
//                using (SqlCommand command =
//                    new SqlCommand(
//                        "GetBillByID",
//                        connection
//                    ))
//                {
//                    command.CommandType =
//                        CommandType.StoredProcedure;

//                    command.Parameters.AddWithValue(
//                        "@ID",
//                        id
//                    );

//                    connection.Open();

//                    using (SqlDataReader reader =
//                        command.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            bill = new Bill
//                            {
//                                ID =
//                                    Convert.ToInt32(reader["ID"]),

//                                BillNumber =
//                                    reader["BillNumber"].ToString(),

//                                ClientID =
//                                    Convert.ToInt32(reader["ClientID"]),

//                                VoyageID =
//                                    Convert.ToInt32(reader["VoyageID"]),

//                                GrossWeight =
//                                    Convert.ToDecimal(
//                                        reader["GrossWeight"]
//                                    ),

//                                NetWeight =
//                                    Convert.ToDecimal(
//                                        reader["NetWeight"]
//                                    )
//                            };
//                        }
//                    }
//                }
//            }

//            return bill;
//        }


//        public int InsertBill(Bill bill)
//        {
//            using (SqlConnection connection =
//                new SqlConnection(connectionString))
//            {
//                using (SqlCommand command =
//                    new SqlCommand(
//                        "InsertBill",
//                        connection
//                    ))
//                {
//                    command.CommandType =
//                        CommandType.StoredProcedure;

//                    command.Parameters.AddWithValue(
//                        "@BillNumber",
//                        bill.BillNumber
//                    );

//                    command.Parameters.AddWithValue(
//                        "@ClientID",
//                        bill.ClientID
//                    );

//                    command.Parameters.AddWithValue(
//                        "@VoyageID",
//                        bill.VoyageID
//                    );

//                    command.Parameters.AddWithValue(
//                        "@GrossWeight",
//                        bill.GrossWeight
//                    );

//                    command.Parameters.AddWithValue(
//                        "@NetWeight",
//                        bill.NetWeight
//                    );

//                    connection.Open();

//                    return Convert.ToInt32(
//                        command.ExecuteScalar()
//                    );
//                }
//            }
//        }


//        public int UpdateBill(Bill bill)
//        {
//            using (SqlConnection connection =
//                new SqlConnection(connectionString))
//            {
//                using (SqlCommand command =
//                    new SqlCommand(
//                        "UpdateBill",
//                        connection
//                    ))
//                {
//                    command.CommandType =
//                        CommandType.StoredProcedure;

//                    command.Parameters.AddWithValue(
//                        "@ID",
//                        bill.ID
//                    );

//                    command.Parameters.AddWithValue(
//                        "@BillNumber",
//                        bill.BillNumber
//                    );

//                    command.Parameters.AddWithValue(
//                        "@ClientID",
//                        bill.ClientID
//                    );

//                    command.Parameters.AddWithValue(
//                        "@VoyageID",
//                        bill.VoyageID
//                    );

//                    command.Parameters.AddWithValue(
//                        "@GrossWeight",
//                        bill.GrossWeight
//                    );

//                    command.Parameters.AddWithValue(
//                        "@NetWeight",
//                        bill.NetWeight
//                    );

//                    connection.Open();

//                    return Convert.ToInt32(
//                        command.ExecuteScalar()
//                    );
//                }
//            }
//        }


//        public int DeleteBill(int id)
//        {
//            using (SqlConnection connection =
//                new SqlConnection(connectionString))
//            {
//                using (SqlCommand command =
//                    new SqlCommand(
//                        "DeleteBill",
//                        connection
//                    ))
//                {
//                    command.CommandType =
//                        CommandType.StoredProcedure;

//                    command.Parameters.AddWithValue(
//                        "@ID",
//                        id
//                    );

//                    connection.Open();

//                    return Convert.ToInt32(
//                        command.ExecuteScalar()
//                    );
//                }
//            }
//        }


//        public List<Bill> AdvancedSearchBills(
//            string billNumber,
//            int? clientID,
//            int? voyageID)
//        {
//            List<Bill> bills = new List<Bill>();

//            using (SqlConnection connection =
//                new SqlConnection(connectionString))
//            {
//                using (SqlCommand command =
//                    new SqlCommand(
//                        "AdvancedSearchBills",
//                        connection
//                    ))
//                {
//                    command.CommandType =
//                        CommandType.StoredProcedure;


//                    command.Parameters.AddWithValue(
//                        "@BillNumber",
//                        string.IsNullOrWhiteSpace(billNumber)
//                            ? (object)DBNull.Value
//                            : billNumber
//                    );


//                    command.Parameters.AddWithValue(
//                        "@ClientID",
//                        clientID.HasValue
//                            ? (object)clientID.Value
//                            : DBNull.Value
//                    );


//                    command.Parameters.AddWithValue(
//                        "@VoyageID",
//                        voyageID.HasValue
//                            ? (object)voyageID.Value
//                            : DBNull.Value
//                    );


//                    connection.Open();

//                    using (SqlDataReader reader =
//                        command.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            bills.Add(new Bill
//                            {
//                                ID =
//                                    Convert.ToInt32(reader["ID"]),

//                                BillNumber =
//                                    reader["BillNumber"].ToString(),

//                                ClientID =
//                                    Convert.ToInt32(
//                                        reader["ClientID"]
//                                    ),

//                                ClientName =
//                                    reader["ClientName"].ToString(),

//                                VoyageID =
//                                    Convert.ToInt32(
//                                        reader["VoyageID"]
//                                    ),

//                                VoyageNumber =
//                                    reader["VoyageNumber"].ToString(),

//                                GrossWeight =
//                                    Convert.ToDecimal(
//                                        reader["GrossWeight"]
//                                    ),

//                                NetWeight =
//                                    Convert.ToDecimal(
//                                        reader["NetWeight"]
//                                    )
//                            });
//                        }
//                    }
//                }
//            }

//            return bills;
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TrainingShippingSystem.Models;
namespace TrainingShippingSystem.DAL
{
    public class BillsDAL
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings[
                "TrainingShippingDBConnection"
            ].ConnectionString;
        public List<Bill> GetBills()
        {
            List<Bill> bills = new List<Bill>();
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand("GetBills", connection))
                {
                    command.CommandType =
                        CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader =
                        command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bills.Add(new Bill
                            {
                                ID =
                                    Convert.ToInt32(reader["ID"]),
                                BillNumber =
                                    reader["BillNumber"].ToString(),
                                ClientID =
                                    Convert.ToInt32(reader["ClientID"]),
                                VoyageID =
                                    Convert.ToInt32(reader["VoyageID"]),
                                GrossWeight =
                                    Convert.ToDecimal(
                                        reader["GrossWeight"]
                                    ),
                                NetWeight =
                                    Convert.ToDecimal(
                                        reader["NetWeight"]
                                    )
                            });
                        }
                    }
                }
            }
            return bills;
        }
        public Bill GetBillByID(int id)
        {
            Bill bill = null;
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand(
                        "GetBillByID",
                        connection
                    ))
                {
                    command.CommandType =
                        CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(
                        "@ID",
                        id
                    );
                    connection.Open();
                    using (SqlDataReader reader =
                        command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bill = new Bill
                            {
                                ID =
                                    Convert.ToInt32(reader["ID"]),
                                BillNumber =
                                    reader["BillNumber"].ToString(),
                                ClientID =
                                    Convert.ToInt32(reader["ClientID"]),
                                VoyageID =
                                    Convert.ToInt32(reader["VoyageID"]),
                                GrossWeight =
                                    Convert.ToDecimal(
                                        reader["GrossWeight"]
                                    ),
                                NetWeight =
                                    Convert.ToDecimal(
                                        reader["NetWeight"]
                                    )
                            };
                        }
                    }
                }
            }
            return bill;
        }
        public int InsertBill(Bill bill)
        {
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand(
                        "InsertBill",
                        connection
                    ))
                {
                    command.CommandType =
                        CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(
                        "@BillNumber",
                        bill.BillNumber
                    );
                    command.Parameters.AddWithValue(
                        "@ClientID",
                        bill.ClientID
                    );
                    command.Parameters.AddWithValue(
                        "@VoyageID",
                        bill.VoyageID
                    );
                    command.Parameters.AddWithValue(
                        "@GrossWeight",
                        bill.GrossWeight
                    );
                    command.Parameters.AddWithValue(
                        "@NetWeight",
                        bill.NetWeight
                    );
                    connection.Open();
                    return Convert.ToInt32(
                        command.ExecuteScalar()
                    );
                }
            }
        }
        public int UpdateBill(Bill bill)
        {
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand(
                        "UpdateBill",
                        connection
                    ))
                {
                    command.CommandType =
                        CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(
                        "@ID",
                        bill.ID
                    );
                    command.Parameters.AddWithValue(
                        "@BillNumber",
                        bill.BillNumber
                    );
                    command.Parameters.AddWithValue(
                        "@ClientID",
                        bill.ClientID
                    );
                    command.Parameters.AddWithValue(
                        "@VoyageID",
                        bill.VoyageID
                    );
                    command.Parameters.AddWithValue(
                        "@GrossWeight",
                        bill.GrossWeight
                    );
                    command.Parameters.AddWithValue(
                        "@NetWeight",
                        bill.NetWeight
                    );
                    connection.Open();
                    return Convert.ToInt32(
                        command.ExecuteScalar()
                    );
                }
            }
        }
        public int DeleteBill(int id)
        {
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand(
                        "DeleteBill",
                        connection
                    ))
                {
                    command.CommandType =
                        CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(
                        "@ID",
                        id
                    );
                    connection.Open();
                    return Convert.ToInt32(
                        command.ExecuteScalar()
                    );
                }
            }
        }
        public List<Bill> AdvancedSearchBills(
            string billNumber,
            int? clientID,
            int? voyageID)
        {
            List<Bill> bills = new List<Bill>();
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand(
                        "AdvancedSearchBills",
                        connection
                    ))
                {
                    command.CommandType =
                        CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(
                        "@BillNumber",
                        string.IsNullOrWhiteSpace(billNumber)
                            ? (object)DBNull.Value
                            : billNumber
                    );
                    command.Parameters.AddWithValue(
                        "@ClientID",
                        clientID.HasValue
                            ? (object)clientID.Value
                            : DBNull.Value
                    );
                    command.Parameters.AddWithValue(
                        "@VoyageID",
                        voyageID.HasValue
                            ? (object)voyageID.Value
                            : DBNull.Value
                    );
                    connection.Open();
                    using (SqlDataReader reader =
                        command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bills.Add(new Bill
                            {
                                ID =
                                    Convert.ToInt32(reader["ID"]),
                                BillNumber =
                                    reader["BillNumber"].ToString(),
                                ClientID =
                                    Convert.ToInt32(
                                        reader["ClientID"]
                                    ),
                                ClientName =
                                    reader["ClientName"].ToString(),
                                VoyageID =
                                    Convert.ToInt32(
                                        reader["VoyageID"]
                                    ),
                                VoyageNumber =
                                    reader["VoyageNumber"].ToString(),
                                GrossWeight =
                                    Convert.ToDecimal(
                                        reader["GrossWeight"]
                                    ),
                                NetWeight =
                                    Convert.ToDecimal(
                                        reader["NetWeight"]
                                    )
                            });
                        }
                    }
                }
            }
            return bills;
        }
        // =========================================
        // FILTER + SEARCH + PAGINATION + SORTING
        // =========================================
        public FilterBillsResult FilterBills(
            string billNumber,
            int? clientID,
            int? voyageID,
            int pageNumber,
            int pageSize,
            string sortBy,
            string sortDirection)
        {
            FilterBillsResult result =
                new FilterBillsResult();
            result.Bills =
                new List<Bill>();
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand(
                        "FilterBills",
                        connection
                    ))
                {
                    command.CommandType =
                        CommandType.StoredProcedure;
                    // Bill Number
                    command.Parameters.AddWithValue(
                        "@BillNumber",
                        string.IsNullOrWhiteSpace(billNumber)
                            ? (object)DBNull.Value
                            : billNumber
                    );
                    // Client ID
                    command.Parameters.AddWithValue(
                        "@ClientID",
                        clientID.HasValue
                            ? (object)clientID.Value
                            : DBNull.Value
                    );
                    // Voyage ID
                    command.Parameters.AddWithValue(
                        "@VoyageID",
                        voyageID.HasValue
                            ? (object)voyageID.Value
                            : DBNull.Value
                    );
                    // Pagination
                    command.Parameters.AddWithValue(
                        "@PageNumber",
                        pageNumber
                    );
                    command.Parameters.AddWithValue(
                        "@PageSize",
                        pageSize
                    );
                    // Sorting
                    command.Parameters.AddWithValue(
                        "@SortBy",
                        string.IsNullOrWhiteSpace(sortBy)
                            ? "ID"
                            : sortBy
                    );
                    command.Parameters.AddWithValue(
                        "@SortDirection",
                        string.IsNullOrWhiteSpace(sortDirection)
                            ? "DESC"
                            : sortDirection
                    );
                    connection.Open();
                    using (SqlDataReader reader =
                        command.ExecuteReader())
                    {
                        // =====================================
                        // FIRST RESULT SET
                        // Bills
                        // =====================================
                        while (reader.Read())
                        {
                            result.Bills.Add(
                                new Bill
                                {
                                    ID =
                                        Convert.ToInt32(
                                            reader["ID"]
                                        ),
                                    BillNumber =
                                        reader["BillNumber"]
                                            .ToString(),
                                    ClientID =
                                        Convert.ToInt32(
                                            reader["ClientID"]
                                        ),
                                    ClientName =
                                        reader["ClientName"]
                                            .ToString(),
                                    VoyageID =
                                        Convert.ToInt32(
                                            reader["VoyageID"]
                                        ),
                                    VoyageNumber =
                                        reader["VoyageNumber"]
                                            .ToString(),
                                    GrossWeight =
                                        Convert.ToDecimal(
                                            reader["GrossWeight"]
                                        ),
                                    NetWeight =
                                        Convert.ToDecimal(
                                            reader["NetWeight"]
                                        )
                                }
                            );
                        }
                        // =====================================
                        // SECOND RESULT SET
                        // Total Records
                        // =====================================
                        if (reader.NextResult())
                        {
                            if (reader.Read())
                            {
                                result.TotalRecords =
                                    Convert.ToInt32(
                                        reader["TotalRecords"]
                                    );
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
    // =========================================
    // FILTER BILLS RESULT
    // =========================================
    public class FilterBillsResult
    {
        public List<Bill> Bills { get; set; }
        public int TotalRecords { get; set; }
    }
}