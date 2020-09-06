using CRUDWithADO_SP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRUDWithADO_SP.Controllers
{
    public class EmployeesController : Controller
    {
        //Common class to establish data to database
        //defined string
        private SqlConnection Connection()
        {
            try
            {
                SqlConnection connection = new SqlConnection(@"Data Source=(LocalDb)\LocalDBDemo;Initial Catalog=EmployeeDB;Integrated Security=True");
                return connection;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Index()  
        {
            DataTable dataTable = new DataTable();
            try
            {
                var con = Connection();
                con.Open();
                SqlCommand com = new SqlCommand("spGetAllEmployees", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(com);
                dataAdapter.Fill(dataTable);
                List<EmployeeModel> employees = new List<EmployeeModel>();
                employees = (from DataRow dr in dataTable.Rows
                         select new EmployeeModel()
                         {
                             EmployeeID = (int)(dr["EmployeeID"]),
                             FullName = (String)(dr["FullName"]),
                             Country = (String)(dr["Country"]),
                             State = (String)(dr["State"]),
                             City = (String)(dr["City"]),
                             DOB = (DateTime)(dr["DOB"]),
                         }).ToList();
                con.Close();
                return View(employees);
            }
            catch (Exception)
            {
                throw;
            }
                        
        }

        public ActionResult Create()
        {
            try
            {
                DataTable dataTable = new DataTable();
                var con = Connection();
                con.Open();
                SqlCommand com = new SqlCommand("spGetCountries", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(com);
                dataAdapter.Fill(dataTable);
                List<Country> country = new List<Country>();
                country = (from DataRow dr in dataTable.Rows
                           select new Country()
                           {
                               CountryId = Convert.ToInt32(dr["CountryId"]),
                               Name = dr["Name"].ToString()
                           }).ToList();
                ViewBag.CountryList = new SelectList(country, "CountryId", "Name");
            }
            catch (Exception)
            { }
            return View();
        }

        [HttpPost]
        public string Create(EmployeeModel employee)
        {

            try
            {
                int result = 0;
                string message = "";
                var con = Connection();
                con.Open();
                if (employee.EmployeeID == 0)
                {
                    SqlCommand sqlCommand = new SqlCommand("spAddEmployee", con);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@FullName", employee.FullName);
                    sqlCommand.Parameters.AddWithValue("@Country", employee.Country);
                    sqlCommand.Parameters.AddWithValue("@State", employee.State);
                    sqlCommand.Parameters.AddWithValue("@City", employee.City);
                    sqlCommand.Parameters.AddWithValue("@DOB", employee.DOB);
                    result = sqlCommand.ExecuteNonQuery();
                    if (result > 0)
                    {
                        message = "AddSuccess";
                    }
                    else
                    {
                        message = "Error";
                    }
                    return message;
                }
                else
                {
                    SqlCommand sqlCommand = new SqlCommand("spUpdateEmployee", con);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@FullName", employee.FullName);
                    sqlCommand.Parameters.AddWithValue("@Country", employee.Country);
                    sqlCommand.Parameters.AddWithValue("@State", employee.State);
                    sqlCommand.Parameters.AddWithValue("@City", employee.City);
                    sqlCommand.Parameters.AddWithValue("@DOB", employee.DOB);
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeID);
                    result = sqlCommand.ExecuteNonQuery();
                    if (result > 0)
                    {
                        message = "UpdateSuccess";
                    }
                    else
                    {
                        message = "Error";
                    }
                    return message;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        public string DeleteConfirmed(int id)
        {
            try
            {
                int result = 0;
                string message = "";
                var con = Connection();
                con.Open();
                SqlCommand sqlCommand = new SqlCommand("spDeleteEmployee", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@EmployeeId", id);
                result = sqlCommand.ExecuteNonQuery();
                if (result > 0)
                {
                    message = "Success";
                }
                else
                {
                    message = "Error";
                }
                return message;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string getstates(int country)
        {
            try
            {
                DataTable dataTable = new DataTable();
                var con = Connection();
                con.Open();
                SqlCommand com = new SqlCommand("spGetStates", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CountryId", country);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(com);
                dataAdapter.Fill(dataTable);
                var info = dataTable.AsEnumerable().Select(c => new SelectListItem { Text = c.Field<string>("Name"), Value = Convert.ToString(c.Field<int>("StateId")) }).ToList();
                //.Select(c => new SelectListItem { Text = c.["Name"], Value = c["Name"] }).ToList();
                return Newtonsoft.Json.JsonConvert.SerializeObject(info);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string GetCities(int state)
        {
            try
            {
                DataTable dataTable = new DataTable();
                var con = Connection();
                con.Open();
                SqlCommand com = new SqlCommand("spGetCities", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@StateId", state);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(com);
                dataAdapter.Fill(dataTable);
                var info = dataTable.AsEnumerable().Select(c => new SelectListItem { Text = c.Field<string>("Name"), Value = Convert.ToString(c.Field<int>("CityId")) }).ToList();
                //.Select(c => new SelectListItem { Text = c.["Name"], Value = c["Name"] }).ToList();
                return Newtonsoft.Json.JsonConvert.SerializeObject(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public JsonResult GetEmployeeById(int id)
        {
            try
            {
                DataTable dataTable = new DataTable();
                var con = Connection();
                con.Open();
                SqlCommand com = new SqlCommand("spGetEmployeeById", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@EmployeeId", id);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(com);
                dataAdapter.Fill(dataTable);
                con.Close();
                var employeeObj = dataTable.Rows[0];
                EmployeeModel employee = new EmployeeModel();
                if (dataTable != null && employeeObj != null)
                {
                    employee.EmployeeID = !string.IsNullOrEmpty(Convert.ToString(employeeObj["EmployeeID"])) ? Convert.ToInt32(employeeObj["EmployeeID"]) : 0;
                    employee.FullName = (string)employeeObj["FullName"];
                    employee.Country = (string)employeeObj["Country"];
                    employee.State = (string)employeeObj["State"];
                    employee.City = (string)employeeObj["City"];
                    employee.DOB = (DateTime)employeeObj["DOB"];
                }
                return Json(employee, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}