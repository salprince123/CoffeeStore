using CoffeeStore.DAL;
using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.BUS
{
    class BUS_ReceiptDetail
    {
        DAL_ReceiptDetail dalReceiptDetail = new DAL_ReceiptDetail();
        public bool CreateReceiptDetail(DTO_ReceiptDetail newReceiptDetail)
        {
            return dalReceiptDetail.CreateReceiptDetail(newReceiptDetail);
        }    
    }
}
