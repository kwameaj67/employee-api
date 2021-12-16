using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestAPI_Demo.Models
{
     public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeModel>> getEmployees();
        Task<EmployeeModel> getEmployeeByName(string name);
        Task<EmployeeModel> getEmployee(int id);
        Task<EmployeeModel> createEmployee(EmployeeModel employee);
        Task<EmployeeModel> deleteEmployee(int id);
        Task<EmployeeModel> editEmployee(int id,EmployeeModel employee);

        
    }

}
