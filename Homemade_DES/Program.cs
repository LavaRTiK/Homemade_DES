using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Text;

namespace Homemade_DES
{
    internal class Program
    {
        static void Main(string[] args)
        {
            byte[] dad = new byte[8];
            byte[] cad = new byte[8];
            DES des = new DES(dad,cad);
            //des.coding("aaaa");
            //Перестановка
            byte[] test = new byte[8];
            test = Encoding.UTF8.GetBytes("babababa");
            BitArray bittest = new BitArray(test);
            int[] mass = new int[]{58,50,42,34,26,18,10,2,60,52,44,36,28,20,12,4,62,54,46,38,30,22,14,6,64,56,48,40,32,24,16,8,57,49,41,33,25,17,9,1,59,51,43,35,27,19,11,3,61,53,45,37,29,21,13,5,63,55,47,39,31,23,15,7};
            BitArray bitmix = new BitArray(64);
            for (int i = 0; i < bittest.Length; i++)
            {
                bitmix[i] = bittest[mass[i]-1];
            }
            BitArray bitLeft = new BitArray(32);
            BitArray bitRight = new BitArray(32);
            int bitcount = 0;
            for (int i = 0; i < bitmix.Length-1; i++)
            {
                if (i <= 31)
                {
                    bitLeft[bitcount] = bitmix[i];
                    bitcount++;
                    if(bitcount > 31)
                    {
                        bitcount= 0;
                    }
                }
                else 
                {
                    bitRight[bitcount] = bitmix[i];
                    bitcount++;
                }
            }

            foreach (bool item in bitLeft)
            {
                Console.Write(item ? 1:0);
            }
            Console.WriteLine("-----------------------------------");
            foreach (bool item in bitRight)
            {
                Console.Write(item ? 1 : 0);
            }
            int[] massE = new int[] {32,1,2,3,4,5,4,5,6,7,8,9,8,9,10,11,12,13,12,13,14,15,16,17,16,17,18,19,20,21,20,21,22,23,24,25,24,25,26,27,28,29,28,29,30,31,32,1};
            BitArray bitERight = new BitArray(48);
            for (int i = 0;i < massE.Length; i++)
            {
                bitERight[i] = bitRight[massE[i]-1];
            }
            Console.WriteLine("");
            foreach (bool item in bitERight)
            {
                Console.Write(item ? 1:0);
            }
            BitArray result = new BitArray(48);
            BitArray key = new BitArray(48);
            result = bitERight.Xor(key); // тут ключ
            int[,,] S_BOX = new int[8, 4, 16]
            {
                {   //S1
                    {14,4,13,1,2,15,11,8,3,10,6,12,5,9,0,7},
                    {0,15,7,4,14,2,13,1,10,6,12,11,9,5,3,8},
                    {4,1,14,8,13,6,2,11,15,12,9,7,3,10,5,0},
                    {15,12,8,2,4,9,1,7,5,11,3,14,10,0,6,13}
                },
                {   //S2
                    {15,1,8,14,6,11,3,4,9,7,2,13,12,0,5,10},
                    {3,13,4,7,15,2,8,14,12,0,1,10,6,9,11,5},
                    {0,14,7,11,10,4,13,1,5,8,12,6,9,3,2,15},
                    {13,8,10,1,3,15,4,2,11,6,7,12,0,5,14,9}
                },
                {   //S3
                    {10,0,9,14,6,3,15,5,1,13,12,7,11,4,2,8},
                    {13,7,0,9,3,4,6,10,2,8,5,14,12,11,15,1},
                    {13,6,4,9,8,15,3,0,11,1,2,12,5,10,14,7},
                    {1,10,13,0,6,9,8,7,4,15,14,3,11,5,2,12}
                },
                {   //S4
                    {7,13,14,3,0,6,9,10,1,2,8,5,11,12,4,15},
                    {13,8,11,5,6,15,0,3,4,7,2,12,1,10,14,9},
                    {10,6,9,0,12,11,7,13,15,1,3,14,5,2,8,4},
                    {3,15,0,6,10,1,13,8,9,4,5,11,12,7,2,14}
                },
                {   //S5
                    {2,12,4,1,7,10,11,6,8,5,3,15,13,0,14,9},
                    {14,11,2,12,4,7,13,1,5,0,15,10,3,9,8,6},
                    {4,2,1,11,10,13,7,8,15,9,12,5,6,3,0,14},
                    {11,8,12,7,1,14,2,13,6,15,0,9,10,4,5,3}
                },
                {   //S6
                    {12,1,10,15,9,2,6,8,0,13,3,4,14,7,5,11},
                    {10,15,4,2,7,12,9,5,6,1,13,14,0,11,3,8},
                    {9,14,15,5,2,8,12,3,7,0,4,10,1,13,11,6},
                    {4,3,2,12,9,5,15,10,11,14,1,7,6,0,8,13}
                },
                {   //S7
                    {4,11,2,14,15,0,8,13,3,12,9,7,5,10,6,1},
                    {13,0,11,7,4,9,1,10,14,3,5,12,2,15,8,6},
                    {1,4,11,13,12,3,7,14,10,15,6,8,0,5,9,2},
                    {6,11,13,8,1,4,10,7,9,5,0,15,14,2,3,12}
                },
                {   //S8
                    {13,2,8,4,6,15,11,1,10,9,3,14,5,0,12,7},
                    {1,15,13,8,10,3,7,4,12,5,6,11,0,14,9,2},
                    {7,11,4,1,9,12,14,2,0,6,10,13,15,3,5,8},
                    {2,1,14,7,4,10,8,13,15,12,9,0,3,5,6,11}
                }
            };
            //тут переменая где будут все 32 байта после цыкла
            List<int> list = new List<int>();   
            BitArray bitArray = new BitArray(32);
            int couter = 0;
            for (int i = 0; i < 8; i++)
            {
                BitArray dade = new BitArray(6);
                for (int x = 0; x < 6; x++)
                {
                    result[couter] = dade[x];
                    couter++;
                }
                Console.WriteLine("");
                foreach (bool item in dade)
                {
                    Console.Write(item ? 1 : 0);
                }
                Console.WriteLine("");
                int test13 = S_BOX[i, Convert.ToInt16($"{Convert.ToInt16(dade[0])}{Convert.ToInt16(dade[5])}"), Convert.ToInt16($"{Convert.ToInt16(dade[1])}{Convert.ToInt16(dade[2])}{Convert.ToInt16(dade[3])}{Convert.ToInt16(dade[4])}")];
                list.Add(test13);
                Console.WriteLine(test13 + "итерация" + i);
                Console.WriteLine();
                
            }
            string bitString = "";
            foreach (int value in list)
            {
                bitString += Convert.ToString(value, 2);
            }
            for (int i = 0; i < bitString.Length; i++)
            {
                bitArray[i] = bitString[i] == '1';
            }
            Console.WriteLine(bitArray.Length);




            //des.CreateKeys();
            #region test
            //Вывод текста с BitArayy
            //byte[] test = new byte[8];
            //test = Encoding.UTF8.GetBytes("aaaaaaaa");
            //BitArray bittest = new BitArray(test);
            //for (int i = bittest.Length-1; i >= 0; i--)
            //{
            //    Console.Write(bittest[i]? 1 : 0);
            //}


            ////Console.WriteLine("Hello, World!");
            //List<bool> list = new List<bool>();
            //byte[] bytes = new byte[7];
            //bytes = Encoding.UTF8.GetBytes("123456");
            ////Convert.FromBase64String // повертає байти
            //string str = "";
            ////69ACD63259E2679B
            //string hexKey = "69ACD63259E2679B";
            ////6856D61958E1334D
            //byte[] sda = new byte[7];
            //Random random = new Random();
            //random.NextBytes(sda);
            //string strr = Convert.ToHexString(sda);
            //byte[] byteKey1 = new byte[strr.Length / 2];
            //for (int i = 0; i < byteKey1.Length; i++)
            //{
            //    byteKey1[i] = Convert.ToByte(strr.Substring(i * 2, 2), 16);
            //}
            //BitArray bittest1 = new BitArray(byteKey1);
            //foreach (bool b in bittest1)
            //{
            //    Console.Write(b ? 1 : 0);
            //}
            //Console.WriteLine(bittest1.Length + "|" + bittest1.Count);
            //Console.WriteLine("----------------------------------------------");


            //// Перетворення шістнадцяткового ключа у масив байтів
            //byte[] byteKey = new byte[hexKey.Length / 2];
            //for (int i = 0; i < byteKey.Length; i++)
            //{
            //    byteKey[i] = Convert.ToByte(hexKey.Substring(i * 2, 2), 16);
            //}
            //BitArray bittest = new BitArray(byteKey);
            //foreach (bool b in bittest)
            //{
            //    Console.Write(b ? 1 : 0);
            //}
            //Console.WriteLine("--------------------------------------------");
            //byte[] testbyte = Convert.FromBase64String(str);

            ////BitArray bitArray = new BitArray(bytes);
            //Console.WriteLine(bittest.Count); 
            //BitArray bitArray64 = new BitArray(64);
            //int index = 0;
            //int count = 0;
            //int countbit = 0;
            //foreach (bool b in bittest)
            //{
            //    if(count == 7)
            //    {
            //        if(countbit % 2 == 0)
            //        {
            //            bitArray64[index] = true;
            //        }
            //        else
            //        {
            //            bitArray64[index] = false;
            //        }
            //        count = 0;
            //        index++;
            //        countbit = 0;
            //    }
            //    else
            //    {
            //        if(b == true)
            //        {
            //            countbit++;
            //        }
            //        bitArray64[index] = b;
            //        index++;
            //    }
            //}

            //foreach (bool b in bittest)
            //{
            //    Console.Write(b ? 1 : 0);
            //}
            //Console.WriteLine("---------------");
            //Console.WriteLine(bitArray64.Count);
            //foreach (bool b in bitArray64)
            //{
            //    Console.Write(b ? 1 : 0);
            //}

            //Console.WriteLine(cout);
            //Console.WriteLine(bytes);
            //foreach(byte b in bytes)
            //{
            //    Console.WriteLine(b);
            //}
            //Console.WriteLine("------------------------------------");
            //foreach (byte b in bytes)
            //{
            //    byte temp = test2(b);
            //    Console.WriteLine(temp);
            //}
            //Console.WriteLine("-----------------------");
            //foreach (byte b in bytes)
            //{
            //    Console.WriteLine(b);
            //}
            //DES ds = new DES();
            #endregion
        }
        //public static byte test2(byte value)
        //{
        //    int onesCount = test(value);
        //    if( onesCount % 2 == 0 )
        //    {
        //        return (byte)(value | 0b10000000);
        //    }
        //    else
        //    {
        //        return (byte)(value | 0b01111111);
        //    }
        //}
        public static int test(byte value)
        {
            int cout = 0;
            for (int i = 0; i < 7; i++)
            {
                if ((value & (1 << i)) != 0)
                {
                    cout++;
                }
            }
            return cout;
        }
    }
}
