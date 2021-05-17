using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DTO
{
    public class DTO_Beverage
    {
        public string BeverageID { get; set; }
        public string BeverageTypeID { get; set; }
        public string BeverageName { get; set; }
        public int Amount { get; set; }
        public int Price { get; set; }
        public string Unit { get; set; }
    }
}
