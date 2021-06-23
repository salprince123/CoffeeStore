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
    class DAL_Receipt : DBConnect
    {
        public DataTable GetReceipt(int limit, int offset)
        {
            DataTable receipts = new DataTable();
            try
            {
                string sql = $"select Receipt.ReceiptID, Time, Receipt.EmployeeID, Employees.EmployeeName, Discount.DiscountID, DiscountValue, sum(Price * Amount) as Total from Receipt join Employees on Receipt.EmployeeID = Employees.EmployeeID left join Discount on Receipt.DiscountID = Discount.DiscountID join ReceiptDetail on Receipt.ReceiptID = ReceiptDetail.ReceiptID group by Receipt.ReceiptID limit {limit} offset {offset}";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(receipts);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return receipts;
        }

        public DateTime GetCreateDayByID(string id)
        {
            DataTable receipts = new DataTable();
            try
            {
                string sql = $"select Time from Receipt where ReceiptID = '{id}'";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(receipts);
                return TimeZone.CurrentTimeZone.ToLocalTime((DateTime)receipts.Rows[0]["Time"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DateTime(2021, 1, 1);
            }
            
        }    

        public DataTable GetReceipt(DateTime startDate, DateTime endDate, string keyword, int limit, int offset)
        {
            if (keyword == "")
                keyword = "R";
            string start = startDate.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
            string end = endDate.AddDays(1).ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
            DataTable receipts = new DataTable();
            try
            {
                string sql = $"select R.ReceiptID, Time, R.EmployeeID, E.EmployeeName, D.DiscountID, DiscountValue, sum(Price * Amount) as Total from Receipt as R join Employees as E on R.EmployeeID = E.EmployeeID left join Discount as D on R.DiscountID = D.DiscountID join ReceiptDetail as RD on R.ReceiptID = RD.ReceiptID where ((R.ReceiptID like '%{keyword}%') and (CAST(strftime('%s', Time) AS integer) >= CAST(strftime('%s', '{start}') AS integer)) and (CAST(strftime('%s', Time) AS integer) < CAST(strftime('%s', '{end}') AS integer))) group by R.ReceiptID limit {limit} offset {offset}";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(receipts);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return receipts;
        }

        public int CountReceipt()
        {
            DataTable receipts = new DataTable();
            try
            {
                string sql = $"select count(ReceiptID) from Receipt";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(receipts);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Int32.Parse(receipts.Rows[0].ItemArray[0].ToString());
        }

        public int CountReceipt(DateTime startDate, DateTime endDate, string keyword)
        {
            if (keyword == "")
                keyword = "R";
            string start = startDate.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
            string end = endDate.AddDays(1).ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");

            DataTable receipts = new DataTable();
            try
            {
                string sql = $"select count(ReceiptID) from Receipt where ((ReceiptID like '%{keyword}%') and (CAST(strftime('%s', Time) AS integer) >= CAST(strftime('%s', '{start}') AS integer)) and (CAST(strftime('%s', Time) AS integer) < CAST(strftime('%s', '{end}') AS integer)))";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(receipts);
                return Int32.Parse(receipts.Rows[0].ItemArray[0].ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
            
        }

        public DataTable GetTotalIncomeByMonth(int month, int year)
        {
            string strMonth = month.ToString().PadLeft(2, '0');
            DataTable data = new DataTable();
            try
            {
                string sql = $"select strftime('%d', Time) as Day, sum(Amount * Price * (1 - IFNULL(DiscountValue, 0)/100))  as TotalAfterDis from ReceiptDetail join Receipt on ReceiptDetail.ReceiptID = Receipt.ReceiptID left join Discount on Receipt.DiscountID = Discount.DiscountID where (strftime('%m', Time) = '{strMonth}' and strftime('%Y',Time) = '{year}') group by Discount.DiscountID, strftime('%d', Time)";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public DataTable GetTotalIncomeByYear(int year)
        {
            DataTable data = new DataTable();
            try
            {
                string sql = $"select strftime('%m', Time) as Month, sum(Amount * Price * (1 - IFNULL(DiscountValue, 0)/100)) as TotalAfterDis from ReceiptDetail join Receipt on ReceiptDetail.ReceiptID = Receipt.ReceiptID left join Discount on Receipt.DiscountID = Discount.DiscountID where strftime('%Y',Time) = '{year}' group by strftime('%m', Time)";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public string CreateReceipt(DTO_Receipt newReceipt)
        {
            DataTable receipts = GetReceipt(-1, 0);
            if (receipts.Rows.Count != 0)
            {
                string lastID = receipts.Rows[receipts.Rows.Count - 1]["ReceiptID"].ToString();
                newReceipt.ReceiptID = "R" +
                    (Convert.ToInt32(lastID.Replace("R", "")) + 1)
                        .ToString()
                        .PadLeft(9, '0');
            }
            else
                newReceipt.ReceiptID = "R000000001";

            //insert SQLite 
            string sql = $"INSERT INTO Receipt (ReceiptID, Time, EmployeeID, DiscountID) VALUES ('{newReceipt.ReceiptID}', DateTime('now'), '{newReceipt.EmployeeID}', '{newReceipt.DiscountID}')";
            if (newReceipt.DiscountID == "")
                sql = $"INSERT INTO Receipt (ReceiptID, Time, EmployeeID) VALUES ('{newReceipt.ReceiptID}', DateTime('now'), '{newReceipt.EmployeeID}')";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                insert.ExecuteNonQuery();
                return newReceipt.ReceiptID;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public bool DeleteReceiptByID(string id)
        {
            string sql = $"delete from Receipt where ReceiptID = '{id}'";
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
    }
}
