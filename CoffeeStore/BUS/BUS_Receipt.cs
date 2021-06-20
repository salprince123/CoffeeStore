using CoffeeStore.DAL;
using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.BUS
{
    class BUS_Receipt
    {
        DAL_Receipt dalReceipt = new DAL_Receipt();
        public string CreateReceipt(DTO_Receipt newReceipt)
        {
            return dalReceipt.CreateReceipt(newReceipt);
        }

        public DataTable GetReceipts()
        {
            return dalReceipt.GetReceipt();
        }
    }
}
