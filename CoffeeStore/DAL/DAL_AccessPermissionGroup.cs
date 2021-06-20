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
    class DAL_AccessPermissionGroup : DBConnect
    {
        public bool CreateAccessPermissionGroup(DTO_AccessPermissionGroup newAccPerGr)
        {
            //insert SQLite 
            string sql = $"insert into AccessPermissionGroup('EmployeeTypeID','AccessPermissionID') VALUES ('{newAccPerGr.EmployeeTypeID}','{newAccPerGr.AccessPermissionID}')";
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

        public bool DeleteAccessPermissionGroup(DTO_AccessPermissionGroup deleteAccPerGr)
        {
            string sql = $"delete from AccessPermissionGroup where AccessPermissionID = '{deleteAccPerGr.AccessPermissionID}' and EmployeeTypeID = '{deleteAccPerGr.EmployeeTypeID}'";
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

        public bool DeleteByEmpTypeID(string id)
        {
            string sql = $"delete from AccessPermissionGroup where EmployeeTypeID = '{id}'";
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

        public bool isHavePermission(string typeId, string permissionID)
        {
            DataTable data = new DataTable();
            try
            {
                string sql = $"select count(EmployeeTypeID) from AccessPermissionGroup where AccessPermissionID = '{permissionID}' and EmployeeTypeID = '{typeId}'";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(data);
                if (data.Rows[0].ItemArray[0].ToString() == "1")
                    return true;
            }
            catch
            {

            }
            return false;
        }    
    }
}
