using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Homemade_DES
{
    internal class DES
    {
        private byte[] key;
        private byte[] text;
        public DES(byte[] key,byte[] text) {
            if(key.Length > 8 )
            {
                throw new Exception("Mngog bitov");
            }
            else
            {
                this.key = key;
            }
            if (text.Length > 8)
            {
                throw new Exception("Mngog bitov text");
            }
            else
            {
                this.text = text;
            }
        }

        public void Encoding()
        {

        }
        public void Decoding() 
        { 
            
        }
        public void CreateKeys()
        {
            byte[] randomMass = new byte[7];
            Random random = new Random();
            random.NextBytes(randomMass);
            string strKey = Convert.ToHexString(randomMass);
            byte[] byteKey = new byte[strKey.Length / 2];
            for (int i = 0; i < byteKey.Length; i++)
            {
                byteKey[i] = Convert.ToByte(strKey.Substring(i * 2, 2), 16);
            }
            BitArray bitKey = new BitArray(byteKey);
            Console.WriteLine("----------------------------------------");
            Console.WriteLine(bitKey.Count);
            foreach (bool b in bitKey)
            {
                Console.Write(b ? 1 : 0);
            }
            BitArray bitArray64 = new BitArray(64);
            int index = 0;
            int count = 0;
            int countbit = 0;
            foreach (bool b in bitKey)
            {
                if (count == 7)
                {
                    if (countbit % 2 == 0)
                    {
                        bitArray64[index] = true;
                    }
                    else
                    {
                        bitArray64[index] = false;
                    }
                    count = 0;
                    index++;
                    countbit = 0;
                }
                else
                {
                    if (b == true)
                    {
                        countbit++;
                    }
                    bitArray64[index] = b;
                    index++;
                }
            }
            Console.WriteLine(bitArray64.Count);
            Console.WriteLine("-----------------");
            foreach (bool b in bitArray64)
            {
                Console.Write(b ? 1 : 0);
            }
        }
    }
}
