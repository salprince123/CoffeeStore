﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeStore.DTO;
using CoffeeStore.DAL;
using System.Data;

namespace CoffeeStore.BUS
{
    class BUS_Discount
    {
        public DataTable getAllDiscount()
        {
            DAL_Discount dal = new DAL_Discount();
            return dal.getAllDiscount();
        }
        public DTO_Discount findDiscount(string ID)
        {
            DAL_Discount dal = new DAL_Discount();
            return dal.findDiscount(ID);
        }
        public DataTable findDiscount(string startdate, string enddate)
        {
            DAL_Discount dal = new DAL_Discount();
            return dal.findDiscount(startdate,enddate);
        }

        public int createNewDiscount(DTO_Discount dto)
        {
            DAL_Discount dal = new DAL_Discount();
            return dal.createNewDiscount(dto);
        }
        public int editDiscount(DTO_Discount dto)
        {
            DAL_Discount dal = new DAL_Discount();
            return dal.editDiscount(dto);
        }
        public int deleteDiscount(string ID)
        {
            DAL_Discount dal = new DAL_Discount();
            return dal.deleteDiscount(ID);
        }
        public string ID()
        {
            DAL_Discount dal = new DAL_Discount();
            return dal.createID();
        }
    }
}
