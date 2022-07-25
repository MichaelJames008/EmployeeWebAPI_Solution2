using EmployeeWebAPI.Models;
using EmployeeWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeWebAPI.Repositories;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Diagnostics;

namespace EmployeeWebAPI.Controllers
{
    public class HomeController : Controller
    {

        string Baseurl = ConfigurationManager.AppSettings["APIBaseURL"].ToString();

        public async Task<List<Employee>> GetEmployeesFromAPIAsync()
        {
            //using API

            var employees = new List<Employee>();

            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/employees");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var employeesapi = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list

                    //EmpInfo = JsonConvert.DeserializeObject<Bodies>(EmpResponse);
                    employees = JsonConvert.DeserializeObject<IEnumerable<Employee>>(employeesapi).ToList();
                }

            }

            return employees;
        }
        public async Task<ActionResult> Index()
        {

            //using List
            //var employees = new List<Employee>
            //{
            //    new Employee { Name = "Ruel Ebba", Age = 12, Id = 1, NickName = "Ruel"},
            //    new Employee { Name = "Rahul Katta", Age = 30, Id = 2, NickName = "Rah"},
            //    new Employee { Name = "Rachel Rodriquez", Age = 22, Id = 3, NickName = "Raqz"},
            //    new Employee { Name = "Michael James", Age = 35, Id = 4, NickName = "MJ"}

            //};


            //1. Using API EntityFramework
            //var employees = await GetEmployeesFromAPIAsync();

            //2. Using API EntityFramework
            // var repo = new DapperRepo();
            var repo = new AdoNetRepo();

            //// Add New Employees
            //var newemployees = new List<Employee>
            //{
            //    new Employee { Name = "Rudy Martin", Age = 22, NickName = "Rud"},
            //    new Employee { Name = "Marvin Lim", Age = 40,  NickName = "Mharvz"},

            //};

            //var addemp = repo.AddEmployees(newemployees);


            // Get Employee From Repo

            //var emprepo = new EmployeeRepository(new EmployeeDB());
            //var employees = emprepo.GetAll();


            //var cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ArchCodeFirst;Integrated Security=SSPI;";
            //var employees2 = new List<Employee>();

            //// using var con = new SqlConnection(cs);

            //using (IDbConnection con = new System.Data.SqlClient.SqlConnection(cs))
            //{
            //    con.Open();
            //    employees2 = con.Query<Employee>("SELECT * FROM Employees").ToList();
               
            //}

            //employees2.ForEach(e => Debug.WriteLine(e.Name));


            var employees = repo.GetEmployees();
            

            var EmployeeVM = new EmployeeViewModel
            {
                Employees = (IEnumerable<Employee>)employees
            };
           


            return View(EmployeeVM);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";



            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}