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
       
        public DataTable SelectByName(List <String > name)
        {
            try
            {
                if (name == null) return null;
                String selectedName = $"(";
                for (int i = 0; i < name.Count - 1; i++)
                {
                    selectedName += $"'{name[i]}',";
                }
                selectedName += $"'{name[name.Count - 1]}')";
                string sql = $"SELECT * FROM Material where materialname IN {selectedName}";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable listMaterial = new DataTable();
                da.Fill(listMaterial);
                Console.WriteLine($"SELECT BY NAME ROW {listMaterial.Rows.Count}");
                return listMaterial;
            }
            catch (Exception)
            {

            };
            return null;
        }
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
        public void UpdateUsing(string name, string unit)
        {
            string sql = $"UPDATE Material SET isuse = '1', unit='{unit}' WHERE materialName= '{name}'";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                insert.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool Create(String name, String unit)
        {
            //checking if material is already exist
            string checkMaterial = $"SELECT * FROM Material where MaterialName= '{name}' ";
            SQLiteDataAdapter checkDA = new SQLiteDataAdapter(checkMaterial, getConnection());
            DataTable checkMater = new DataTable();
            checkDA.Fill(checkMater);
            if(checkMater.Rows.Count!=0)
            {
                foreach (DataRow row in checkMater.Rows)
                {
                    string isUse = row["isUse"].ToString();
                    if(isUse== "1")
                        return false;
                }
                UpdateUsing(name,unit);
                return true;

            }               
            //create auto increase ID
            //Get max MaterialID
            String id = "Mater00000";
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
            string sql = $"insert into Material VALUES ('{newID}','{name}','{unit}','1');";
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
            string sql = $"UPDATE Material SET isuse = '0' WHERE materialName= '{name}'";
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
        public bool Update (String oldName,String newName, String unit)
        {
            string sql = $"update Material set MaterialName='{newName}', Unit = '{unit}'  where MaterialName='{oldName}'";
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
