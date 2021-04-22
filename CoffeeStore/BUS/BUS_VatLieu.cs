using CoffeeStore.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.BUS
{
    class BUS_VatLieu
    {
        DAL_VatLieu dalVL = new DAL_VatLieu();
        public DataTable selectAll()
        {
            return dalVL.SelectAllVatLieu();
        }
    }
}
