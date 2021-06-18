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
                string sql = $"select Password from Employees where EmployeeID = '{ID}' and State = '1'";
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

        public string GetEmpTypeByID(string ID)
        {
            string type = "";
            try
            {
                string sql = $"select EmployeeTypeID from Employees where EmployeeID = '{ID}'";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable listPass = new DataTable();
                da.Fill(listPass);
                type = listPass.Rows[0].ItemArray[0].ToString();
            }
            catch
            {

            }
            return type;
        }

        public string GetEmpNameByID(string ID)
        {
            string name = "";
            try
            {
                string sql = $"select EmployeeName from Employees where EmployeeID = '{ID}'";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable listPass = new DataTable();
                da.Fill(listPass);
                name = listPass.Rows[0].ItemArray[0].ToString();
            }
            catch
            {

            }
            return name;
        }

        public DTO_Employees GetEmpByID(string ID)
        {
            DataTable empData = new DataTable();
            DTO_Employees emp = new DTO_Employees();
            try
            {
                string sql = $"select EmployeeID, EmployeeName, EmployeeType.EmployeeTypeName, Password from Employees join EmployeeType on Employees.EmployeeTypeID = EmployeeType.EmployeeTypeID where State = '1' and Employees.EmployeeID = '{ID}'";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(empData);
                emp = new DTO_Employees(empData.Rows[0]["EmployeeID"].ToString(), empData.Rows[0]["EmployeeName"].ToString(), empData.Rows[0]["EmployeeTypeName"].ToString(), empData.Rows[0]["Password"].ToString());
            }
            catch
            {

            }
            return emp;
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

        public bool EditEmployee(DTO_Employees editedEmp)
        {
            string sql = $"update Employees set EmployeeName = '{editedEmp.EmployeeName}', EmployeeTypeID = '{editedEmp.EmployeeTypeID}', Password = '{editedEmp.Password}' where EmployeeID = '{editedEmp.EmployeeID}'";
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

        public bool EditPassword(string empID, string newPass)
        {
            string sql = $"update Employees set Password = '{newPass}' where EmployeeID = '{empID}'";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                insert.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int Delete(string empID)
        {
            bool isDelete = IsDoingAnything(empID);
            if (isDelete)
            {
                string sql = $"delete from Employees where EmployeeID = '{empID}'";
                SQLiteCommand delete = new SQLiteCommand(sql, getConnection().OpenAndReturn());
                try
                {
                    delete.ExecuteNonQuery();
                    return 1;
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                string sql = $"update Employees set State = '0' where EmployeeID = '{empID}'";
                SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
                try
                {
                    insert.ExecuteNonQuery();
                    return -1;
                }
                catch
                {
                    return 0;
                }
            }
        }    

        public bool IsDoingAnything(string empID)
        {
            try
            {
                DataTable countData = new DataTable();
                string sql = $"select count(EmployeeID) from InventoryImport where EmployeeID = '{empID}'";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(countData);
                if (countData.Rows[0].ItemArray[0].ToString() != "0")
                    return false;

                sql = $"select count(EmployeeID) from InventoryExport where EmployeeID = '{empID}'";
                da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(countData);
                if (countData.Rows[0].ItemArray[0].ToString() != "0")
                    return false;

                sql = $"select count(EmployeeID) from Receipt where EmployeeID = '{empID}'";
                da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(countData);
                if (countData.Rows[0].ItemArray[0].ToString() != "0")
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }    
    }
}
