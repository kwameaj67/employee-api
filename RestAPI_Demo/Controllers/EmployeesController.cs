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
        private EmployeeData _employeeData;
        private readonly NpgsqlConnection _connection;

        public EmployeesController(EmployeeData employeeData, IConfiguration configuration)
        {
            _employeeData = employeeData;

            string connectionString = configuration.GetConnectionString("default");

            _connection = new NpgsqlConnection(connectionString);

        }

        // GET: all employees
        [HttpGet]
        [Route("api/[controller]/getEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            string query = @"select employeeid as id,name,createdDate from employee";

            var result = await _connection.QueryAsync<EmployeeModel>(query);

            return Ok(new { count = result.Count(), payload = result });

        }

        // GET: single employee
        [HttpGet]
        [Route("api/[controller]/{id:int}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            string query = @"select employeeid as id,name,createdDate from employee where employeeid = @id";

            var result = await _connection.QueryFirstOrDefaultAsync<EmployeeModel>(query, new
            {
                id = id
            });
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
            string query = @"select employeeid as id,name,createdDate from employee where name ilike @name";

            var result = await _connection.QueryAsync<EmployeeModel>(query, new
            {
                name = "%"+name+"%"
            });;
            if (!result.Any())
            {
                return NotFound($" No employee with {name} was found");
            }

            return Ok(new { count = result.Count(), payload = result });
        }

        // POST: create an employee
        [HttpPost]
        [Route("api/[controller]/createEmployee")]
        public async Task<IActionResult> CreateEmployee(EmployeeModel employee)
        {
            //_employeeData.createEmployee(employee);
            string query = @"insert into employee(Name, CreatedDate) values (@Name, @CreatedDate); ";
            var result = await _connection.ExecuteAsync(query, new
            {
                ID = Guid.NewGuid(),
                Name = employee.Name,
                CreatedDate = DateTime.Now
            }).ConfigureAwait(false);
            Console.WriteLine(result);
            if (result > 0)
            {
                return Ok("Employee added succesfully!");
                //  return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + employee.Id, employee);
            }

            return BadRequest("Something went wrong");
        }

        // DELETE: single employee
        [HttpDelete]
        [Route("api/[controller]/deleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            return NotFound();
        }
        // EDIT: single employee
        [HttpPut]
        [Route("api/[controller]/editEmployee/{id}")]
        public IActionResult edit_Employee(int id, EmployeeModel employee)
        {

            return Ok(employee);

        }
    }
}
