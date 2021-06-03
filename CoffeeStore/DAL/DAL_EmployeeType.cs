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
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
