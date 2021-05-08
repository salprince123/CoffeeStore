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
    public class DAL_Material : DBConnect
    {
        public DataTable SelectAllMaterial()
        {
            try
            {
                string sql = $"select * from Material";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable listMaterial = new DataTable();
                da.Fill(listMaterial);
                return listMaterial;
            }
            catch (Exception)
            {
            };
            return null;
        }
        public bool Create(String name, String unit)
        {
            //checking if material is already exist
            string checkMaterial = $"SELECT materialId FROM Material where MaterialName= '{name}' ";
            SQLiteDataAdapter checkDA = new SQLiteDataAdapter(checkMaterial, getConnection());
            DataTable checkMater = new DataTable();
            checkDA.Fill(checkMater);
            if(checkMater.Rows.Count!=0)
               return false;
            //create auto increase ID
            //Get max MaterialID
            String id = "";
            string tempSQL = "SELECT materialId FROM Material order by materialId desc LIMIT 1 ";
            SQLiteDataAdapter da = new SQLiteDataAdapter(tempSQL, getConnection());
            DataTable maxId = new DataTable();
            da.Fill(maxId);
            foreach (DataRow row in maxId.Rows)
            {
                id = row["MaterialID"].ToString();
            }
            //auto increase ID
            string newID = "Mater" +
                (Convert.ToInt32(id.Replace("Mater", "")) + 1)
                    .ToString()
                    .PadLeft(5, '0');
            //insert SQLite 
            string sql = $"insert into Material VALUES ('{newID}','{name}','{unit}');";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                insert.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
            
        }
        public bool Delete (String name)
        {
            string sql = $"delete from material where MaterialName='{name}'";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                return insert.ExecuteNonQuery() >0;                
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Update (String name, String unit)
        {
            string sql = $"update Material set MaterialName='{name}', Unit = '{unit}'  where MaterialName='{name}'";
            SQLiteCommand update = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                return update.ExecuteNonQuery() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

}
