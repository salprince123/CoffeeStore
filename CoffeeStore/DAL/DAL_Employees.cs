using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DAL
{
    class DAL_Employees : DBConnect
    {
        public string GetPasswordByID(string ID)
        {
            string pass = "";
            try
            {
                string sql = $"select Password from Employees where EmployeeID = '{ID}'";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable listPass = new DataTable();
                da.Fill(listPass);
                pass = listPass.Rows[0].ItemArray[0].ToString();
            }
            catch
            {

            }
            return pass;
        }

        public DataTable GetActiveEmployees()
        {
            DataTable employees = new DataTable();
            try
            {
                string sql = $"select EmployeeID, EmployeeName, EmployeeType.EmployeeTypeName, Password from Employees join EmployeeType on Employees.EmployeeTypeID = EmployeeType.EmployeeTypeID where State = '1'";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(employees);
            }
            catch
            {

            }
            return employees;
        }
    }
}
