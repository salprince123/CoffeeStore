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
    public class DAL_EmployeeType : DBConnect
    {
        public DataTable GetEmployeeTypes()
        {
            DataTable empTypes = new DataTable();
            try
            {
                string sql = $"select * from EmployeeType";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(empTypes);
            }
            catch
            {

            }
            return empTypes;
        }

        public int CountEmployeeTypes()
        {
            DataTable empTypes = new DataTable();
            try
            {
                string sql = $"select count(EmployeeTypeID) from EmployeeType";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(empTypes);
            }
            catch
            {

            }
            return Int32.Parse(empTypes.Rows[0].ItemArray[0].ToString());
        }

        public string GetNameByID(string id)
        {
            string name = "";
            DataTable empTypeName = new DataTable();
            try
            {
                string sql = $"select EmployeeTypeName from EmployeeType where EmployeeTypeID = '{id}'";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(empTypeName);
                name = empTypeName.Rows[0].ItemArray[0].ToString();
            }
            catch
            {

            }
            return name;
        }

        public string GetIDByName(string name)
        {
            string id = "";
            DataTable empTypeName = new DataTable();
            try
            {
                string sql = $"select EmployeeTypeID from EmployeeType where EmployeeTypeName = '{name}'";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(empTypeName);
                id = empTypeName.Rows[0].ItemArray[0].ToString();
            }
            catch
            {

            }
            return id;
        }

        public string CreateEmployeeType(DTO_EmployeeType newEmpType)
        {
            DataTable employeeType = GetEmployeeTypes();
            string lastID = employeeType.Rows[employeeType.Rows.Count - 1]["EmployeeTypeID"].ToString();
            newEmpType.EmployeeTypeID = "ET" +
                (Convert.ToInt32(lastID.Replace("ET", "")) + 1)
                    .ToString()
                    .PadLeft(3, '0');

            //insert SQLite
            string sql = $"insert into EmployeeType('EmployeeTypeID','EmployeeTypeName') VALUES ('{newEmpType.EmployeeTypeID}','{newEmpType.EmployeeTypeName}')";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                insert.ExecuteNonQuery();
                return newEmpType.EmployeeTypeID;
            }
            catch (Exception )
            {
                return "";
            }
        }

        public int Delete(string typeID)
        {
            bool isDelete = IsHaveEmployee(typeID);
            if (isDelete)
            {
                string sql = $"delete from EmployeeType where EmployeeTypeID = '{typeID}'";
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
                return 0;
            }
        }    

        public bool IsHaveEmployee(string typeID)
        {
            try
            {
                DataTable countData = new DataTable();
                string sql = $"select count(EmployeeTypeID) from Employees where EmployeeTypeID = '{typeID}'";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
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

        public bool EditEmployeeType(DTO_EmployeeType editEmpType)
        {
            string sql = $"update EmployeeType set EmployeeTypeName = '{editEmpType.EmployeeTypeName}' where EmployeeTypeID = '{editEmpType.EmployeeTypeID}'";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                insert.ExecuteNonQuery();
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }    
    }
}
