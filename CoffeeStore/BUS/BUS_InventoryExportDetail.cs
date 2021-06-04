﻿using CoffeeStore.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.BUS
{
    public class BUS_InventoryExportDetail
    {
        DAL_InventoryExportDetail temp = new DAL_InventoryExportDetail();
        public DataTable SelectAllImportDetail()
        {
            return temp.SelectAllExportDetail();
        }
        public DataTable SelectAllExportDetailGroupByName()
        {
            return temp.SelectAllExportDetailGroupByName();
        }
        public DataTable Find(String key)
        {
            return temp.FindWithKeyWord(key);
        }
    }
}
