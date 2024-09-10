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
        //вопрос нужно ли переворачивать bitarray для перевода в текст?
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

        public void coding(string text)
        {
            List<BitArray> blockCoding = new List<BitArray>();
            byte[] textByte = Encoding.UTF8.GetBytes(text);
            BitArray test = new BitArray(textByte);
            foreach (bool bitArray in test)
            {
                Console.Write(bitArray ? 1 : 0);
            }
            List<byte> textByteList  = textByte.ToList();
            int blockCout = (textByte.Length + 7) / 8;
            for (int i = 0; i < blockCout; i++)
            {
                byte[] temp = new byte[8];
                textByteList.CopyTo(0,temp,0,textByteList.Count < 8 ? textByteList.Count : 8);
                textByteList.RemoveRange(0,textByteList.Count < 8 ? textByteList.Count : 8);
                BitArray bitTemp = new BitArray(temp);
                Console.WriteLine(textByteList.Count < 8 ? textByteList.Count : 8);
                blockCoding.Add(bitTemp);
            }
            foreach(bool bit in blockCoding[3])
            {
                Console.Write(bit ? 1 : 0);
            }
            Console.WriteLine("гуд");
            Console.WriteLine(); 


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
                        bitArray64[index+1] = true;
                    }
                    else
                    {
                        bitArray64[index+1] = false;
                    }
                    bitArray64[index] = b;
                    count = 0;
                    index = index+2;
                    countbit = 0;
                }
                else
                {
                    if (b == true)
                    {
                        countbit++;
                    }
                    bitArray64[index] = b;
                    count++;
                    index++;
                }
            }
            Console.WriteLine("");
            Console.WriteLine(bitArray64.Count);
            Console.WriteLine("-----------------");
            foreach (bool b in bitArray64)
            {
                Console.Write(b ? 1 : 0);
            }
        }
    }
}
