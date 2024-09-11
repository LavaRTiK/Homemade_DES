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
            foreach (bool item in bittest)
            {
                Console.Write(item ? 1 : 0);
            }
            Console.WriteLine("-------------------------------------------------------------");
            foreach (bool item in bitmix)
            {
                Console.Write(item ? 1 : 0);
            }

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
