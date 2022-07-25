using EmployeeWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EmployeeWebAPI.Repositories
{
    public class AdoNetRepo
    {
        private string DbConnString(string dbname)
        {
            return ConfigurationManager.ConnectionStrings[dbname].ConnectionString;

        }

        public List<Employee> GetEmployees()
        {

            var employees = new List<Employee>();

            try
            {
                DataSet dsEmployee = new DataSet();
                string sql = $"Select * FROM Employees";

                using (SqlConnection connection = new System.Data.SqlClient.SqlConnection(DbConnString("SQLConnection")))
                {
                    SqlCommand objSqlCommand = new SqlCommand(sql, connection);
                    // objSqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter(objSqlCommand);
                    try
                    {
                        objSqlDataAdapter.Fill(dsEmployee);
                        dsEmployee.Tables[0].TableName = "Employees";
                      
                    }
                    catch (Exception ex)
                    {
                        return employees;
                    }
                }


                foreach (DataRow r in dsEmployee.Tables["Employees"].Rows)
                {
                    var e = new Employee();
                    e.Name = r["Name"].ToString();
                    e.NickName = r["NickName"].ToString();
                    var Age = r["Age"].ToString();
                    var Id = r["Id"].ToString();

                    if(!String.IsNullOrEmpty(Age)){ e.Age = Convert.ToInt32(Age); }
                    if(!String.IsNullOrEmpty(Id)) { e.Id = Convert.ToInt32(Id); }

                    employees.Add(e);
                }

                return employees;
            }
            catch (Exception ex)
            {
                return employees;
            }
        }


        public string AddEmployees(List<Employee> employees)
        {

            try
            {
                var sql = "";
                int _rows = 0;

                using (SqlConnection connection = new System.Data.SqlClient.SqlConnection(DbConnString("SQLConnection")))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    SqlTransaction sqlTran = connection.BeginTransaction();

                    connection.Open();

                    cmd.Transaction = sqlTran;

                    try
                    {
                        foreach (var e in employees)
                        {
                            sql = $"INSERT INTO Employees (Name, Age, NickName) Values ('{e.Name}', {e.Age},'{e.NickName}')";
                            cmd.CommandText = sql;
                            _rows = cmd.ExecuteNonQuery();
                        }

                        sqlTran.Commit();
                        connection.Close();
                    }
                    catch(Exception ex)
                    {

                        try
                        {
                            // Attempt to roll back the transaction.
                            sqlTran.Rollback();

                            throw;
                        }
                        catch (Exception exRollback)
                        {
                            // Throws an InvalidOperationException if the connection
                            // is closed or the transaction has already been rolled
                            // back on the server.
                            Console.WriteLine(exRollback.Message);
                        }

                        throw;

                    }
                }

                return "Success";

            }
            catch (Exception ex)
            {
                return "Error " + ex.Message + " " + ex.StackTrace;
            }



            

        }

    }
}


//public static IEnumerable<int> GetIds(string name)
//{
//    using (var conn = new SqlConnection("Your connection string comes here"))
//    using (var cmd = conn.CreateCommand())
//    {
//        conn.Open();
//        cmd.CommandText = "select ID from Customer where Name=@Name";
//        cmd.Parameters.AddWithValue("@Name", name);
//        using (var reader = cmd.ExecuteReader())
//        {
//            while (reader.Read())
//            {
//                yield return reader.GetInt32(reader.GetOrdinal("ID"));
//            }
//        }
//    }
//}


//// Assumes that connection is a valid SqlConnection object.  
//string queryString =
//  "SELECT CustomerID, CompanyName FROM dbo.Customers";
//SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);

//DataSet customers = new DataSet();
//adapter.Fill(customers, "Customers");


//// Assumes that customerConnection is a valid SqlConnection object.  
//// Assumes that orderConnection is a valid OleDbConnection object.  
//SqlDataAdapter custAdapter = new SqlDataAdapter(
//  "SELECT * FROM dbo.Customers", customerConnection);
//OleDbDataAdapter ordAdapter = new OleDbDataAdapter(
//  "SELECT * FROM Orders", orderConnection);

//DataSet customerOrders = new DataSet();

//custAdapter.Fill(customerOrders, "Customers");
//ordAdapter.Fill(customerOrders, "Orders");

//DataRelation relation = customerOrders.Relations.Add("CustOrders",
//  customerOrders.Tables["Customers"].Columns["CustomerID"],
//  customerOrders.Tables["Orders"].Columns["CustomerID"]);

//foreach (DataRow pRow in customerOrders.Tables["Customers"].Rows)
//{
//    Console.WriteLine(pRow["CustomerID"]);
//    foreach (DataRow cRow in pRow.GetChildRows(relation))
//        Console.WriteLine("\t" + cRow["OrderID"]);
//}