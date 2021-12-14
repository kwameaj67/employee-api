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
        [Route("api/[controller]/getEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            return Ok(new {  payload =await _employeeService.getEmployees()});

        }

        // GET: single employee
        [HttpGet]
        [Route("api/[controller]/{id:int}")]
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
        [Route("api/[controller]/{name}")]
        public async Task<IActionResult> GetEmployeeByName(string name)
        {
            var result = await _employeeService.getEmployeeByName(name);
            if (result == null)
            {
                return NotFound($"No Employee with {name} was found");
            }
            return Ok(new {  payload = result });
        }

        // POST: create an employee
        [HttpPost]
        [Route("api/[controller]/createEmployee")]
        public async Task<IActionResult> CreateEmployee(EmployeeModel employee)
        {
             await _employeeService.createEmployee(employee);
            return Ok("Employee added succesfully!");
            //  return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + employee.Id, employee);

        }

        // DELETE: single employee
        [HttpDelete]
        [Route("api/[controller]/deleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
           
            var result = await _employeeService.deleteEmployee(id);
            Console.WriteLine(result);
            return Ok($"Employee with Id:{id} deleted successfully.");
        }

        // UPDATE: update a single employee
        [HttpPut]
        [Route("api/[controller]/editEmployee/{id}")]
        public async Task<IActionResult> EditEmployee(int id, EmployeeModel employee)
        {
            var result = await _employeeService.editEmployee(id, employee);
            if (result != null)
            {
                return Ok("Employee succesfully updated");
            }
            return NotFound($"Employee Id:{id} not found");
            //return Ok(new { payload = result });

        }
    }
}
