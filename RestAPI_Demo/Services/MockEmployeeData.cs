using System;
using System.Collections.Generic;
using System.Linq;
using RestAPI_Demo.Models;

namespace RestAPI_Demo.Services
{
    public class MockEmployeeData: EmployeeData
    {
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

       
    }
}
