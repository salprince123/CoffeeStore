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

        public bool CreateEmployee(DTO_Employees newEmp)
        {
            //insert SQLite 
            string sql = $"insert into Employees('EmployeeID','EmployeeName','EmployeeTypeID','Password', 'State') VALUES ('{newEmp.EmployeeID}','{newEmp.EmployeeName}','{newEmp.EmployeeTypeID}','{newEmp.Password}', '1')";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                insert.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }    
    }
}
