using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public static class Extensions
    {
        public static bool HasTrue(this BitArray bitarray)
        {
            for(int i = 0; i < bitarray.Count; i++)
            {
                if(bitarray[i])
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasFalse(this BitArray bitarray)
        {
            for (int i = 0; i < bitarray.Count; i++)
            {
                if (!bitarray[i])
                {
                    return true;
                }
            }
            return false;
        }

    }
}
