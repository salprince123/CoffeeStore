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
    class BUS_EmployeeType
    {
        DAL_EmployeeType dalEmpType = new DAL_EmployeeType();

        public DataTable GetEmployeeTypes()
        {
            return dalEmpType.GetEmployeeTypes();
        }

        public string CreateEmployeeTypes(DTO_EmployeeType newEmpType)
        {
            return dalEmpType.CreateEmployeeType(newEmpType);
        }
    }
}
