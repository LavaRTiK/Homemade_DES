using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homemade_DES
{
    public static class Extensions
    {
        public static BitArray Append(this BitArray current, BitArray after)
        {
            var bools = new bool[current.Count + after.Count];
            current.CopyTo(bools, 0);
            after.CopyTo(bools, current.Count);
            return new BitArray(bools);
        }

        public static int[] ToBits(this BitArray current)
        {
            return current.OfType<bool>().Select(s => s ? 1: 0).ToArray();
        }
    }
}
