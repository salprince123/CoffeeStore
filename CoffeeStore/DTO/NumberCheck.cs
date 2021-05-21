using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DTO
{
    public class NumberCheck
    {
        public static bool IsNumber(string str)
        {
            short i;
            return Int16.TryParse(str, out i);
        }
    }
}
