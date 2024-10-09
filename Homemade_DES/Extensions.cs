using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public static BitArray Fill(this BitArray current, byte[] data)
        {
            BitArray bitArray = new BitArray(data.Length * 8);
            for (int i = 0; i < data.Length; i++)
            {
                byte currentByte = data[i];
                for (int j = 0; j < 8; j++)
                {
                    bitArray[i * 8 + j] = (currentByte & (1 << (7 - j))) != 0;
                }
            }

            return bitArray;
        }
        public static int[] ToBits(this BitArray current)
        {
            return current.OfType<bool>().Select(s => s ? 1: 0).ToArray();
        }
    }
}
