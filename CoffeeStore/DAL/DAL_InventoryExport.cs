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
    class DAL_InventoryExport : DBConnect
    {
        public bool Delete(String id)
        {
            string sql = $"delete from inventoryExport where ExportID='{id}'";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                return insert.ExecuteNonQuery() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public DataTable SelectAllExport()
        {
            try
            {
                string sql = $"select employeename, exportid, exportdate from InventoryExport Imp Join Employees employ on employ.EmployeeID=Imp.EmployeeID";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable listExport = new DataTable();
                da.Fill(listExport);
                return listExport;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
                return new DataTable();
            };

        }
        public DataTable SelectDetail(String id)
        {
            try
            {
                string sql = $"select detail.MaterialID,unit,materialname as 'Tên',amount as 'Số lượng' from InventoryExportDetail detail Join Material mater on detail.MaterialID= mater.MaterialID where exportID='{id}'";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable listImport = new DataTable();
                da.Fill(listImport);
                return listImport;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
                return new DataTable();
            };
        }
        public string Create(String name, String date, String description)
        {
            //create auto increase ID
            //Get max MaterialID
            String id = "Exp0000000";
            string tempSQL = "SELECT exportID FROM InventoryExport order by exportID desc LIMIT 1 ";
            SQLiteDataAdapter da = new SQLiteDataAdapter(tempSQL, getConnection());
            DataTable maxId = new DataTable();
            da.Fill(maxId);
            foreach (DataRow row in maxId.Rows)
            {
                id = row["exportID"].ToString();
            }
            //auto increase ID
            string newID = "Exp" +
                (Convert.ToInt32(id.Replace("Exp", "")) + 1)
                    .ToString()
                    .PadLeft(7, '0');
            //get employid from name 
            String employId = "";
            string tempSQL1 = $"select employeeid from Employees where employeename= '{name}' ";
            SQLiteDataAdapter da1 = new SQLiteDataAdapter(tempSQL1, getConnection());
            DataTable employid = new DataTable();
            da1.Fill(employid);
            foreach (DataRow row in employid.Rows)
            {
                employId = row["EmployeeID"].ToString();
            }
            //insert SQLite 
            string sql = $"insert into InventoryExport('ExportID','EmployeeID','ExportDate','Description') VALUES ('{newID}','{employId}','{date}', '{description}');";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                insert.ExecuteNonQuery();
                return newID;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }
        public string SelectDescription(String id)
        {
            try
            {
                string sql = $"select Description from InventoryExport where exportID='{id}'";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable listImport = new DataTable();
                da.Fill(listImport);
                foreach (DataRow row in listImport.Rows)
                {
                    return row["Description"].ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
                
            };
            return null;
        }
        public void updateDescription(String exportID, String value)
        {
            String sql = $"update InventoryExport set description='{value}' where exportid='{exportID}'";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                insert.ExecuteNonQuery();
            }
            catch (Exception)
            {
            }
        }
    }
}
