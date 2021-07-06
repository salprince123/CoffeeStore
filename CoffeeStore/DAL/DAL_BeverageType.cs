using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeStore.DTO;

namespace CoffeeStore.DAL
{
    class DAL_BeverageType: DBConnect
    {
        public List<DTO_BeverageType> GetBeverageType()
        {
            List<DTO_BeverageType> BeverageType = new List<DTO_BeverageType>();
            string sql = $"Select BeverageTypeName, BeverageTypeID From BeverageType";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, getConnection());
                command.Connection.Open();
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DTO_BeverageType dto = new DTO_BeverageType();
                    dto.BeverageTypeID = reader["BeverageTypeID"].ToString();
                    dto.BeverageTypeName = reader["BeverageTypeName"].ToString();
                    BeverageType.Add(dto);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
            return BeverageType;
        }
        public int createNewBeverageType(DTO_BeverageType beveragetype)
        {
            int rs = 0;
            string sql = $"Insert into BeverageType values ('" + beveragetype.BeverageTypeID + "','" + beveragetype.BeverageTypeName + "')";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, getConnection());
                command.Connection.Open();
                rs = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return rs;
        }
        public int deleteBeverageType(string id)
        {
            int rs = 0;
            string sql = $"Delete From BeverageType Where BeverageTypeID='" + id + "'";
            if (checkConditionToDelete(id))
                try
                {
                    SQLiteCommand command = new SQLiteCommand(sql, getConnection());
                    command.Connection.Open();
                    rs = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }
            return rs;
        }
        public int editBeverageType(DTO_BeverageType beveragetype)
        {
            int rs = 0;
            Console.WriteLine(beveragetype.BeverageTypeID);
            string sql = $"Update BeverageType set BeverageTypeName='" + beveragetype.BeverageTypeName + "' Where BeverageTypeID='" + beveragetype.BeverageTypeID + "'";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, getConnection());
                command.Connection.Open();
                rs = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return rs;
        }
        //public List<String> 
        public string createID()
        {
            string ID = "BT";
            int count = 0;
            int max = 0;
            string sql = "Select BeverageTypeID from BeverageType";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, getConnection());
                command.Connection.Open();
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    count = Int16.Parse(reader["BeverageTypeID"].ToString().Remove(0, 2));
                    if (count > max)
                        max = count;
                }
                max++;
                ID += max.ToString();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("BT: "+ e.Message);
            }
            return ID;
        }
        public List<DTO_BeverageType> findBeverageType(string type)
        {
            List<DTO_BeverageType> BeverageType = new List<DTO_BeverageType>();
            string sql = $"Select BeverageTypeName, BeverageTypeID From BeverageType Where BeverageTypeName like'%" + type + "%'";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, getConnection());
                command.Connection.Open();
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DTO_BeverageType dto = new DTO_BeverageType();
                    dto.BeverageTypeID = reader["BeverageTypeID"].ToString();
                    dto.BeverageTypeName = reader["BeverageTypeName"].ToString();
                    BeverageType.Add(dto);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Find: "+e.Message);
            }
            return BeverageType;
           
        }
        public bool checkConditionToDelete(string ID)
        {
            bool result = true;
            try
            {
                string sql = $"Select BeverageTypeID From Beverage Where BeverageTypeID='" + ID + "'";
                SQLiteCommand cmd = new SQLiteCommand(sql, getConnection());
                SQLiteDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    result = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            };
            return result;
        }
    }
}
