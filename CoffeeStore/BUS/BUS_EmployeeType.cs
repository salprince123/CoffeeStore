using CoffeeStore.DAL;
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
    }
}
