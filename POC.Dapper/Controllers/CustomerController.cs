using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POC.Dapper.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace POC.Dapper.Controllers
{
    public class CustomerController : Controller
    {
        public CustomerController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ActionResult Index()
        {
            List<Customer> customers = new List<Customer>();
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("CustomerConnection")))
            {
                customers = db.Query<Customer>("Select * From Customers").ToList();
            }

            return View(customers);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("CustomerConnection")))
                {
                    string sqlQuery = "INSERT INTO Customers (FirstName, LastName, Email) Values(@FirstName, @LastName, @Email)";

                    int rowsAffected = db.Execute(sqlQuery, customer);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            Customer customer = new Customer();
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("CustomerConnection")))
            {
                customer = db.Query<Customer>("SELECT * FROM Customers WHERE CustomerID =" + id, new { id }).SingleOrDefault();
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("CustomerConnection")))
                {
                    string sqlQuery = "UPDATE Customers SET FirstName='" + customer.FirstName +
                                    "',LastName='" + customer.LastName +
                                    "', Email='" + customer.Email +
                                    "' WHERE CustomerID=" + customer.CustomerID;

                    int rowsAffected = db.Execute(sqlQuery);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Customer customer = new Customer();
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("CustomerConnection")))
            {
                customer = db.Query<Customer>("SELECT * FROM Customers WHERE CustomerID =" + id, new { id }).SingleOrDefault();
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("CustomerConnection")))
                {
                    string sqlQuery = "DELETE FROM Customers WHERE CustomerID =" + id;

                    int rowsAffected = db.Execute(sqlQuery);
                }
                    return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
