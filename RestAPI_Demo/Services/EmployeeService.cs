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
        public async Task<IEnumerable<EmployeeModel>> getEmployees()
        {
            string query = @"select employeeid as id,name,createdDate from employee order by employeeid ASC";
            var result = await _connection.QueryAsync<EmployeeModel>(query);
            return (List<EmployeeModel>)result;
        }
        public async Task<EmployeeModel> getEmployee(int id)
        {
            string query = @"select employeeid as id,name,createdDate from employee where employeeid = @Id";

            return await _connection.QueryFirstOrDefaultAsync<EmployeeModel>(query, new
            {
                Id = id
            });
        }

        public async Task<EmployeeModel> deleteEmployee(int id)
        {
            string query = @"delete from employee where employeeid = @id";

            return await _connection.QueryFirstOrDefaultAsync(query, new
            {
                id
            }).ConfigureAwait(false);
        }

        public async Task<EmployeeModel> editEmployee(int id, EmployeeModel employee)
        {
            string query = @"update employee set 
                           name = @Name,
                           createdDate = @CreatedDate 
                           where employeeid = @id;";
             _ = await _connection.ExecuteAsync(query, new
            {
                id,
                Name = employee.Name,
                CreatedDate = employee.CreatedDate
            }).ConfigureAwait(false);

            return employee;
             
        }

        public async Task<EmployeeModel> getEmployeeByName(string name)
        {
            string query = @"select employeeid as id,name,createdDate from employee where name ilike @name";

            var result = await _connection.QueryAsync<EmployeeModel>(query, new
            {
                name = "%" + name + "%"
            });
            return (EmployeeModel)result;
        }
    }
}
