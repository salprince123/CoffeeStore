using CoffeeStore.DAL;
using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.BUS
{
    class BUS_Employees
    {
        DAL_Employees dalEmp = new DAL_Employees();
        public string GetPasswordByID(string ID)
        {
            return dalEmp.GetPasswordByID(ID);
        }

        public DataTable GetActiveEmployees()
        {
            return dalEmp.GetActiveEmployees();
        }
        
        public bool CreateEmployee(DTO_Employees newEmp)
        {
            return dalEmp.CreateEmployee(newEmp);
        }    
    }
}
