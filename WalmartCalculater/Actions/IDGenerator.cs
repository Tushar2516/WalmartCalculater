using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalmartCalculater.Actions
{
   public class IDGenerator
    {
        static int lastGroupId =0;
        public static int GetIdForGroup()
        {
            return lastGroupId++;
        }

        static int lastProductId = 0;
        public static int GetIdForProduct()
        {
            return lastProductId++;
        }
    }
}
