using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RestAPI_Demo.Models;

namespace RestAPI_Demo.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly NpgsqlConnection _connection;

        public EmployeeService(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("default");

            _connection = new NpgsqlConnection(connectionString);
        }

        public async Task<EmployeeModel> createEmployee(EmployeeModel employee)
        {
            string query = @"insert into employee(name, createdDate) values (@Name, @CreatedDate); ";
            var result = await _connection.ExecuteAsync(query, new
            {
                Id = Guid.NewGuid(),
                Name = employee.Name,
                CreatedDate = employee.CreatedDate
            }).ConfigureAwait(false);
                return employee;
           
        }
        public async Task<List<EmployeeModel>> getEmployees()
        {
            string query = @"select employeeid as id,name,createdDate from employee order by employeeid ASC";
            var result = await _connection.QueryAsync<EmployeeModel>(query);
            return (List<EmployeeModel>)result;
        }
        public async Task<EmployeeModel> getEmployee(int id)
        {
            string query = @"select employeeid as id,name,createdDate from employee where employeeid = @Id";

            var result = await _connection.QueryFirstOrDefaultAsync<EmployeeModel>(query, new
            {
                Id = id
            });
            return result;
        }

        public async  Task<EmployeeModel> getEmployeeByName(string name)
        {
            string query = @"select employeeid as id,name,createdDate from employee where name ilike @name";

            var result = await _connection.QueryAsync<EmployeeModel>(query, new
            {
                name = "%" + name + "%"
            }); 
            return (EmployeeModel)result;
        }


        public async Task<EmployeeModel> deleteEmployee(int id)
        {
            string query = @"delete from employee where employeeid = @id";
            var result = await _connection.QueryFirstOrDefaultAsync(query, new
            {
                id
            });
            return result;
        }

        public async Task<EmployeeModel> editEmployee(int id, EmployeeModel employee)
        {
            string query = @"update employee set 
                           name = @Name,
                           createdDate = @CreatedDate 
                           where employeeid = @id;";
            await _connection.ExecuteAsync(query, new
            {
                id = id,
                Name = employee.Name,
                CreatedDate = employee.CreatedDate
            });
            return employee;
        }

       

        /*
        private List<EmployeeModel> employees = new List<EmployeeModel>()
        {
            new EmployeeModel(){Id =1,Name = "Kwame Boateng",CreatedDate = DateTime.Now},
            new EmployeeModel(){Id = 2,Name = "Owusu Estella",CreatedDate = DateTime.Now},
            new EmployeeModel(){Id = 3,Name = "Ellen Asante",CreatedDate = DateTime.Now},
            new EmployeeModel(){Id = 4,Name = "Efua Ntiriwaa",CreatedDate = DateTime.Now},

        };
       
       
        public EmployeeModel getEmployee(int id)
        {
            return employees.SingleOrDefault(user => user.Id == id);
        }

        public List<EmployeeModel> getEmployees()
        {
            return employees;
        }

        public EmployeeModel createEmployee(EmployeeModel employee)
        {
            employee.Id = 1;
            employee.CreatedDate = DateTime.Now;
            employees.Add(employee);
            return employee;

        }

        public void deleteEmployee(EmployeeModel employee)
        {
            employees.Remove(employee);
        }

        public EmployeeModel editEmployee(EmployeeModel employee)
        {
            var existingEmployee = getEmployee(employee.Id);
            existingEmployee.Name = employee.Name;
            existingEmployee.CreatedDate = DateTime.Now;
            return employee;
        }

        Task<EmployeeModel[]> EmployeeData.getEmployees()
        {
            throw new NotImplementedException();
        }
        */


    }
}
