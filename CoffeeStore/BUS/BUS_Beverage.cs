﻿using CoffeeStore.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.BUS
{
    public class BUS_Beverage
    {
        DAL_Beverage dalmonan = new DAL_Beverage();
        public DataTable getAllBeverage()
        {
            return dalmonan.getAllBeverage();
        }
        public void fillForm(String maMon, ref Home homepage)
        {
            /*DataTable temp = searchMA(maMon);
            homepage.TenMon = temp.Rows[0]["TenMon"].ToString();
            homepage.LoaiMon = temp.Rows[0]["LoaiMon"].ToString();
            //tenMon = temp.Rows[1][1].ToString();*/
        }
    }
}
