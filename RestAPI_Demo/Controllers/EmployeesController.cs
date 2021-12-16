using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RestAPI_Demo.Models;
using System;
using System.Linq;
using System.Threading.Tasks;



namespace RestAPI_Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private IEmployeeService _employeeService;
        private readonly NpgsqlConnection _connection;

        public EmployeesController(IEmployeeService employeeData, IConfiguration configuration)
        {
            _employeeService = employeeData;

            string connectionString = configuration.GetConnectionString("default");

            _connection = new NpgsqlConnection(connectionString);

        }

        // GET: all employees
        [HttpGet]
        [Route("getEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var result = await _employeeService.getEmployees();
                return Ok(new { count = result.Count(), payload = result });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error has occured");
            }

        }

        // GET: single employee
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var result = await _employeeService.getEmployee(id);
            Console.WriteLine(result);
            if (result == null)
            {
                return NotFound($"Employee with {id} not found");
            }
            return Ok(new { count = 1, payload = result });
        }
        // GET: single employee by name
        [HttpGet]
        [Route("searchByName/{name:alpha}")]
        public async Task<IActionResult> GetEmployeeByName(string name)
        {
            var result = await _employeeService.getEmployeeByName(name);
            if (result == null)
            {
                return NotFound($"No Employee with {name} was found");
            }
            return Ok(new { payload = result });
        }

        // POST: create an employee
        [HttpPost]
        [Route("createEmployee")]
        public async Task<IActionResult> CreateEmployee(EmployeeModel employee)
        {
             await _employeeService.createEmployee(employee);
            return Ok("Employee added succesfully!");
            //  return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + employee.Id, employee);

        }

        // DELETE: single employee
        [HttpDelete]
        [Route("deleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
           
            var result = await _employeeService.deleteEmployee(id);
            Console.WriteLine(result);
            return Ok($"Employee with Id:{id} deleted successfully.");
        }

        // UPDATE: update a single employee
        [HttpPut]
        [Route("editEmployee/{id}")]
        public async Task<IActionResult> EditEmployee(int id, EmployeeModel employee)
        {
            var result = await _employeeService.editEmployee(id, employee);
            if (result == null)
            {
                return NotFound($"Employee Id:{id} not found");
               
            }
            return Ok("Employee succesfully updated");
            //return Ok(new { payload = result });

        }
    }
}
