﻿using CoffeeStore.DTO;
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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

        public int CountEmployees()
        {
            DataTable result = new DataTable();
            try
            {
                string sql = $"select count(EmployeeID) from Employees";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(result);
                return Int32.Parse(result.Rows[0].ItemArray[0].ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }    

        public DataTable GetEmployees(int limit, int offset)
        {
            DataTable employees = new DataTable();
            try
            {
                string sql = $"select EmployeeID, EmployeeName, EmployeeType.EmployeeTypeName, Password, State from Employees join EmployeeType on Employees.EmployeeTypeID = EmployeeType.EmployeeTypeID LIMIT {limit} OFFSET {offset}";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(employees);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                Console.WriteLine(ex.Message);
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
                Console.WriteLine(ex.Message);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool SetState(string empID, bool state)
        {
            string sql;
            if (state)
                sql = $"update Employees set State = '1' where EmployeeID = '{empID}'";
            else
                sql = $"update Employees set State = '0' where EmployeeID = '{empID}'";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                insert.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }    

        public bool Delete(string empID)
        {
            bool isDelete = !IsDoingAnything(empID);
            if (isDelete)
            {
                string sql = $"delete from Employees where EmployeeID = '{empID}'";
                SQLiteCommand delete = new SQLiteCommand(sql, getConnection().OpenAndReturn());
                try
                {
                    delete.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                return false;
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
                    return true;

                sql = $"select count(EmployeeID) from InventoryExport where EmployeeID = '{empID}'";
                da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(countData);
                if (countData.Rows[0].ItemArray[0].ToString() != "0")
                    return true;

                sql = $"select count(EmployeeID) from Receipt where EmployeeID = '{empID}'";
                da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(countData);
                if (countData.Rows[0].ItemArray[0].ToString() != "0")
                    return true;

                sql = $"select count(EmployeeID) from PaymentVoucher where EmployeeID = '{empID}'";
                da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(countData);
                if (countData.Rows[0].ItemArray[0].ToString() != "0")
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return true;
            }
        }    
    }
}
