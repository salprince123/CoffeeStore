﻿using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DAL
{
    class DAL_ReceiptDetail : DBConnect
    {
        public bool CreateReceiptDetail(DTO_ReceiptDetail newReceiptDetail)
        {
            //insert SQLite 
            string sql = $"INSERT INTO ReceiptDetail (ReceiptID, BeverageID, Amount, Price) VALUES ('{newReceiptDetail.ReceiptID}', '{newReceiptDetail.BeverageID}', {newReceiptDetail.Amount}, {newReceiptDetail.Price})";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                insert.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public DataTable GetDetailByID(string id)
        {
            DataTable receiptDetails = new DataTable();
            try
            {
                string sql = $"SELECT Time, EmployeeName, BeverageName, Amount, DiscountID, ReceiptDetail.Price as UnitPrice, sum(Amount * ReceiptDetail.Price) as Total from Receipt join ReceiptDetail on Receipt.ReceiptID = ReceiptDetail.ReceiptID join Employees on Receipt.EmployeeID = Employees.EmployeeID join BeverageName on ReceiptDetail.BeverageID = BeverageName.BeverageID where Receipt.ReceiptID = '{id}' group by BeverageName";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(receiptDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return receiptDetails;
        }
    }
}
