using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
using EmployeeWebAPI.Models;

namespace EmployeeWebAPI.Repositories
{
    public class DapperRepo
    {

        private string DbConnString(string dbname)
        {
            return ConfigurationManager.ConnectionStrings[dbname].ConnectionString;

            //Data Source=MyOracleDB;User Id=myUsername;Password=myPassword;Integrated Security=no;
            //Data Source=MyOracleDB;Integrated Security=yes;

        }

        public List<Employee> GetEmployees()
        {

            string sql = $"Select * FROM Employees";

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(DbConnString("SQLConnection")))
            {
                var employees = connection.Query<Employee>(sql).ToList();
                return employees;
            }
        }


        public string AddEmployees(List<Employee> employeees)
        {
            try
            {
                
                string sql = "INSERT INTO Employees (Name, Age, NickName) Values (@Name, @Age, @NickName)";

                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(DbConnString("SQLConnection")))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        foreach (var e in employeees)
                        {
                            connection.Execute(sql, e, transaction: transaction);
                        }

                        transaction.Commit();
                        // transaction.Rollback();
                    }

                    connection.Close();
                }

                
               return "Success";
            }
            catch(Exception ex)
            {

                
                return "Error - " + ex.Message + " " + ex.StackTrace;
            }
           
        }



        // 2. Transaction from Transaction Scope
        //        using (var transaction = new TransactionScope())
        //{
        //    var sql = "INSERT INTO Customers (CustomerName) Values (@CustomerName);";

        //    using (var connection = My.ConnectionFactory())
        //    {
        //        connection.Open();

        //        connection.Execute(sql, new {CustomerName = "Mark"});
        //        connection.Execute(sql, new { CustomerName = "Sam" });
        //        connection.Execute(sql, new { CustomerName = "John" });
        //    }

        //    transaction.Complete();
        //}



        //3) Using Dapper Transaction

        // string sql = "INSERT INTO Customers (CustomerName) Values (@CustomerName);";

       //using (var connection = new SqlConnection(FiddleHelper.GetConnectionStringSqlServerW3Schools()))
        //{
        //    connection.Open();
    
        //    using (var transaction = connection.BeginTransaction())
         //    {
        //        transaction.Execute(sql, new {CustomerName = "Mark"});
                //transaction.Execute(sql, new { CustomerName = "Sam" });
                //transaction.Execute(sql, new { CustomerName = "John" });

                //transaction.Commit();
        //    }
         //}



    }
}