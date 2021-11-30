using System;
using System.Collections.Generic;

namespace RestAPI_Demo.Models
{
     public interface EmployeeData
    {
         List<EmployeeModel> getEmployees();


         EmployeeModel getEmployee(int id);


         EmployeeModel createEmployee(EmployeeModel employee);


         void deleteEmployee(EmployeeModel employee);


         EmployeeModel editEmployee(EmployeeModel employee);

        
    }

}
