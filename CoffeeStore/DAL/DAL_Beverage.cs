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
    public class DAL_Beverage : DBConnect
    {
        public DataTable getAllBeverage()
        {
            try
            {
                string sql = $"Select BeverageID as 'Mã đồ uống', BeverageTypeName as 'Loại đồ uống', BeverageName as 'Tên', Price as 'Giá', ExistingAmount as 'Số lượng', Unit as 'Đơn vị' From BeverageName BN, BeverageType BT Where BN.BeverageTypeID=BT.BeverageTypeID";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable dsMon = new DataTable();
                da.Fill(dsMon);
                return dsMon;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            };
            return null;
        }       
        public int createNewBeverage(DTO_Beverage beverage)
        {
            int rs = 0;
            string sql = $"Insert into BeverageName values ('" + beverage.BeverageID + "','" + beverage.BeverageTypeID + "','" + beverage.BeverageName + "'," + beverage.Price + "," + beverage.Amount + ",'" + beverage.Unit + "')";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, getConnection());
                command.Connection.Open();
                rs = command.ExecuteNonQuery();
            }
            catch ( Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return rs;
        }
        public int deleteBeverage(string id)
        {
            int rs = 0;
            string sql = $"Delete From BeverageName Where BeverageID='" +id+ "'";
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
        public int editBeverage(DTO_Beverage beverage)
        {
            int rs = 0;
            string sql = $"Update BeverageName set BeverageTypeID='" + beverage.BeverageTypeID + "', BeverageName='" + beverage.BeverageName + "', Price=" + beverage.Price + ",ExistingAmount=" + beverage.Amount + ",Unit='" + beverage.Unit + "' Where BeverageID='" + beverage.BeverageID + "'";
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
            string ID = "B";
            int count = 0;
            int max = 0;
            string sql = "Select BeverageID from BeverageName";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, getConnection());
                command.Connection.Open();
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    count = Int16.Parse(reader["BeverageID"].ToString().Remove(0, 1));
                    if (count > max)
                        max = count;
                }
                max++;
                ID += max.ToString();
            }
            catch(Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
            return ID;
        }
        public List<String> GetBeverageType()
        {
            List<String> BeverageType = new List<string>();
            string sql = $"Select BeverageTypeName From BeverageType";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, getConnection());
                command.Connection.Open();
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    BeverageType.Add(reader["BeverageTypeName"].ToString());
                }    
            }
            catch(Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
            return BeverageType;
        }
        public string getBeverageTypeID(string beveragename)
        {
            string beveragetypeID = "";
            string sql = $"Select BeverageTypeID from BeverageType where BeverageTypeName='" + beveragename + "'";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, getConnection());
                command.Connection.Open();
                SQLiteDataReader value = command.ExecuteReader();
                while (value.Read())
                    beveragetypeID = value["BeverageTypeID"].ToString();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
            return beveragetypeID;
        }
        public DataTable getTop5()
        {
            try
            {
                string sql = $"Select A.BeverageID as 'Mã đồ uống', BeverageName as 'Tên', Price as 'Giá', SoLuong as 'Tổng số ly đã bán' From BeverageName BN, (select BeverageID, sum(Amount) as SoLuong from ReceiptDetail group by BeverageID order by SoLuong DESC limit 5) A Where BN.BeverageID=A.BeverageID";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable dsMon = new DataTable();
                da.Fill(dsMon);
                return dsMon;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            };
            return null;
        }       
    }
}
