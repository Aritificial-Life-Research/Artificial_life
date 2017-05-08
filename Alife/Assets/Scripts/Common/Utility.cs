using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class Utillity
    {

//convert to a table index
        public static int vectorToBitToInt<T>(ref List<T> nodes,  List<int> nodeAddresses, bool reverseOrder)  where T : IComparable
        {
            int result = 0;
            if (reverseOrder)
            {

                for (int i = (int) nodeAddresses.Count() - 1; i >= 0; i--)
                {
                    result = (result << 1) + Bit(nodes[nodeAddresses[i]]);
                }
            }
            else
            {
                for (int i = 0; i < (int) nodeAddresses.Count(); i++)
                {
                    result = (result << 1) + Bit(nodes[nodeAddresses[i]]);
                }
            }
            return result;
        }

        public static int Bit<T>(T d) where T : IComparable
        {
            return d.CompareTo(0.0) > 0.0 ? 1 : 0;
            ;
        }
    }
}