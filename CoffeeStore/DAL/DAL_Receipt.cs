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
        public DataTable GetReceipt()
        {
            DataTable receipts = new DataTable();
            try
            {
                string sql = $"select Receipt.ReceiptID, Time, Receipt.EmployeeID, Employees.EmployeeName, Discount.DiscountID, DiscountValue, sum(Price * Amount) as Total from Receipt join Employees on Receipt.EmployeeID = Employees.EmployeeID left join Discount on Receipt.DiscountID = Discount.DiscountID join ReceiptDetail on Receipt.ReceiptID = ReceiptDetail.ReceiptID group by Receipt.ReceiptID";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(receipts);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return receipts;
        }    

        public string CreateReceipt(DTO_Receipt newReceipt)
        {
            DataTable receipts = GetReceipt();
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
    }
}
