using CoffeeStore.DTO;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
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
                string sql = $"Select BeverageID, BeverageName, Link,BeverageTypeName, Price, IsOutOfStock From BeverageName BN, BeverageType BT Where BN.BeverageTypeID=BT.BeverageTypeID";
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
            string sql = $"Insert into BeverageName values ('" + beverage.BeverageID + "','" + beverage.BeverageTypeID + "','" + beverage.BeverageName + "'," + beverage.Price + ", false, 'Cup', @image)";
            
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, getConnection());
                command.Parameters.Add("@image", DbType.Binary, 20).Value = beverage.Link;
                command.Connection.Open();
                rs = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return rs;
        }
        public int deleteBeverage(string id)
        {
            int rs = 0;
            string sql = $"Delete From BeverageName Where BeverageID='" + id + "'";
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
        public int editBeverage(DTO_Beverage beverage)
        {
            int rs = 0;
            Console.WriteLine(beverage.BeverageID);
            string sql = $"Update BeverageName set BeverageTypeID='" + beverage.BeverageTypeID + "', BeverageName='" + beverage.BeverageName + "', Price=" + beverage.Price + ",IsOutOfStock=" + beverage.IsOutOfStock + ",Unit='" + beverage.Unit + $"' , Link=@image Where BeverageID='" + beverage.BeverageID + "'";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, getConnection());
                command.Parameters.Add("@image", DbType.Binary, 20).Value = beverage.Link;
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
            catch (Exception e)
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
            catch (Exception e)
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
                string sql = $"Select A.BeverageID as 'Mã món', BeverageName as 'Tên món', Price as 'Giá', SoLuong as 'Tổng số ly đã bán' From BeverageName BN, (select BeverageID, sum(Amount) as SoLuong from ReceiptDetail group by BeverageID order by SoLuong DESC limit 5) A Where BN.BeverageID=A.BeverageID";
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
        public DataTable findBeverage(string type, string name)
        {
            try
            {
                string sql = "";
                if (type.Length != 0 && name.Length == 0)
                    sql = $"Select BeverageID, BeverageName, BeverageTypeName, Price From BeverageName BN, BeverageType BT Where BN.BeverageTypeID=BT.BeverageTypeID and (BT.BeverageTypeName='" + type + "')";
                else if (type.Length != 0 && name.Length != 0)
                    sql = $"Select BeverageID, BeverageName, BeverageTypeName, Price From BeverageName BN, BeverageType BT Where BN.BeverageTypeID=BT.BeverageTypeID and (BT.BeverageTypeName='" + type + "' and BN.BeverageName like '%" + name + "%')";
                else if (type.Length == 0 && name.Length != 0)
                    sql = $"Select BeverageID, BeverageName, BeverageTypeName, Price From BeverageName BN, BeverageType BT Where BN.BeverageTypeID=BT.BeverageTypeID and ( BN.BeverageName like '%" + name + "%')";
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
        public bool checkConditionToDelete(string ID)
        {
            bool result = true;
            try
            {
                string sql = $"Select BeverageID From ReceiptDetail Where BeverageID='" + ID + "'";
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
        public bool ChangeIsOutOfStockValue(string id, bool isOutOfStock)
        {
            string isOutOfStockStr = "0";
            if (isOutOfStock)
                isOutOfStockStr = "1";

            string sql = $"Update BeverageName set IsOutOfStock = {isOutOfStockStr} Where BeverageID='{id}'";
            SQLiteCommand update = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                update.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return false;
            }

        }
        public DataTable GetBeverageTypeInfo()
        {
            DataTable beverTypes = new DataTable();
            try
            {
                string sql = $"Select * From BeverageType";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(beverTypes);
            }
            catch
            {

            }
            return beverTypes;
        }

        public DataTable GetBeverageOrderBySellAmount(DateTime startDate, DateTime endDate)
        {
            string start = startDate.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
            string end = endDate.AddDays(1).ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
            DataTable BeverData = new DataTable();
            try
            {
                string sql = $"select BeverageName.BeverageID, BeverageName, sum(Amount) as SellAmount from BeverageName join ReceiptDetail on ReceiptDetail.BeverageID = BeverageName.BeverageID join Receipt on ReceiptDetail.ReceiptID = Receipt.ReceiptID where (CAST(strftime('%s', Time) AS integer) >= CAST(strftime('%s', '{start}') AS integer)) and (CAST(strftime('%s', Time) AS integer) < CAST(strftime('%s', '{end}') AS integer)) group by BeverageName.BeverageID order by sum(Amount) asc";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());

                da.Fill(BeverData);
                return BeverData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            };
            return BeverData;
        }

        public DataTable GetBeverageOrderBySellIncome(DateTime startDate, DateTime endDate)
        {
            string start = startDate.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
            string end = endDate.AddDays(1).ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
            DataTable BeverData = new DataTable();
            try
            {
                string sql = $"select BeverageName.BeverageID, BeverageName, sum(Amount * ReceiptDetail.Price * (1 - IFNULL(DiscountValue, 0)/100)) as SellIncome from BeverageName join ReceiptDetail on ReceiptDetail.BeverageID = BeverageName.BeverageID join Receipt on ReceiptDetail.ReceiptID = Receipt.ReceiptID left join Discount on Receipt.DiscountID = Discount.DiscountID where (CAST(strftime('%s', Time) AS integer) >= CAST(strftime('%s', '{start}') AS integer)) and (CAST(strftime('%s', Time) AS integer) < CAST(strftime('%s', '{end}') AS integer)) group by BeverageName.BeverageID order by SellIncome asc";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());

                da.Fill(BeverData);
                return BeverData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            };
            return BeverData;
        }



    }
}