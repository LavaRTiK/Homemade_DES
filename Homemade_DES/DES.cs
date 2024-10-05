﻿using System;
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
        //public DES(byte[] key,byte[] text) {
        //    if(key.Length > 8 )
        //    {
        //        throw new Exception("Mngog bitov");
        //    }
        //    else
        //    {
        //        this.key = key;
        //    }
        //    if (text.Length > 8)
        //    {
        //        throw new Exception("Mngog bitov text");
        //    }
        //    else
        //    {
        //        this.text = text;
        //    }
        //}

        public string Coding(string text,string key)
        {
            //GenereteList Keys
            List<BitArray> listKeys = CreateKeys(key);
            BitArray bitKey = new BitArray(UnicodeEncoding.UTF8.GetBytes(key));


            //Текст в блок битов List сохраняет блоки по 64 бит
            List<BitArray> blockCoding = new List<BitArray>();
            List<BitArray> listnewBlocksBits = new List<BitArray>();
            byte[] textByte = Encoding.UTF8.GetBytes(text);
            BitArray test = new BitArray(textByte);
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
            for (int large = 0; large < blockCoding.Count; large++)
            {
                //blockCoding воходяший кол блоков по 64
                BitArray bittest = new BitArray(test);
                int[] mass = new int[] { 58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4, 62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8, 57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3, 61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7 };
                BitArray bitmix = new BitArray(64);
                for (int i = 0; i < bittest.Length; i++)
                {
                    bitmix[i] = bittest[mass[i] - 1];
                }
                BitArray rsultRound = new BitArray(bitmix);
                //тут цикл
                for (int iterator = 0; iterator < 16; iterator++)
                {
                    BitArray bitLeft = new BitArray(32);
                    BitArray bitRight = new BitArray(32);
                    int bitcount = 0;
                    for (int i = 0; i < rsultRound.Length - 1; i++)
                    {
                        if (i <= 31)
                        {
                            bitLeft[bitcount] = rsultRound[i];
                            bitcount++;
                            if (bitcount > 31)
                            {
                                bitcount = 0;
                            }
                        }
                        else
                        {
                            bitRight[bitcount] = rsultRound[i];
                            bitcount++;
                        }
                    }

                    foreach (bool item in bitLeft)
                    {
                        Console.Write(item ? 1 : 0);
                    }
                    Console.WriteLine("-----------------------------------");
                    foreach (bool item in bitRight)
                    {
                        Console.Write(item ? 1 : 0);
                    }
                    int[] massE = new int[] { 32, 1, 2, 3, 4, 5, 4, 5, 6, 7, 8, 9, 8, 9, 10, 11, 12, 13, 12, 13, 14, 15, 16, 17, 16, 17, 18, 19, 20, 21, 20, 21, 22, 23, 24, 25, 24, 25, 26, 27, 28, 29, 28, 29, 30, 31, 32, 1 };
                    BitArray bitERight = new BitArray(48);
                    for (int i = 0; i < massE.Length; i++)
                    {
                        bitERight[i] = bitRight[massE[i] - 1];
                    }
                    Console.WriteLine("");
                    foreach (bool item in bitERight)
                    {
                        Console.Write(item ? 1 : 0);
                    }
                    BitArray result = new BitArray(48);
                    BitArray keys = new BitArray(48);
                    result = bitERight.Xor(listKeys[iterator]); // тут ключ
                    #region S_BOX
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
                    #endregion
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
                    int[] massP = new int[] { 16, 7, 20, 21, 29, 12, 28, 17, 1, 15, 23, 26, 5, 18, 31, 10, 2, 8, 24, 14, 32, 27, 3, 9, 19, 13, 30, 6, 22, 11, 4, 25 };
                    BitArray bitArrayP = new BitArray(32);
                    for (int i = 0; i < massP.Length; i++)
                    {
                        bitArrayP[i] = bitArray[massP[i] - 1];
                    }
                    Console.WriteLine("ready");
                    //bitaaryp с левой частю

                    Console.WriteLine("");
                    foreach (bool item in bitArrayP)
                    {
                        Console.Write(item ? 1 : 0);
                    }
                    Console.WriteLine("");
                    foreach (bool item in bitLeft)
                    {
                        Console.Write(item ? 1 : 0);
                    }
                    Console.WriteLine("");
                    BitArray tested = new BitArray(bitLeft);
                    tested.Xor(bitArrayP);
                    foreach (bool item in tested)
                    {
                        Console.Write(item ? 1 : 0);
                    }
                    Console.WriteLine("");
                    //tested правая часть
                    int rsultCounter = 0;
                    for (int i = 0; i < rsultRound.Length; i++)
                    {
                        if (i <= 31)
                        {
                            rsultRound[i] = bitRight[rsultCounter];
                            rsultCounter++;
                            if (rsultCounter > 31)
                            {
                                rsultCounter = 0;
                            }
                        }
                        else
                        {
                            rsultRound[i] = bitArrayP[rsultCounter];
                            rsultCounter++;
                        }
                    }

                }
                int[] massIP = new int[] { 40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31, 38, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29, 36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27, 34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9, 49, 17, 57, 25 };
                BitArray bitArrayIP = new BitArray(64);
                for (int i = 0; i < bitArrayIP.Length; i++)
                {
                    bitArrayIP[i] = rsultRound[massIP[i] - 1];
                }
                listnewBlocksBits.Add(bitArrayIP);
            }
            string codeText = "";
            foreach (BitArray item in listnewBlocksBits)
            {
                codeText += BitConverter.ToString(ConvertToByteArray(item));
            }
            return codeText;


        }
        public void Decoding() 
        { 
            
        }
        public string CreateKey()
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
            string key = BitConverter.ToString(ConvertToByteArray(bitArray64)).Replace("-", "");
            Console.WriteLine("");
            Console.WriteLine("Початковий ключ  |"+strKey);
            Console.WriteLine();

            Console.WriteLine("Вихід ключа після операції  |" + key);
            return key;
        }
        private List<BitArray> CreateKeys(string key)
        {
            List<BitArray> listKeys = new List<BitArray>();
            byte[] byteKey = new byte[key.Length / 2];

            for (int i = 0; i < byteKey.Length; i++)
            {
                byteKey[i] = Convert.ToByte(key.Substring(i * 2, 2), 16);
            }
            BitArray bitKey = new BitArray(byteKey);

            Console.WriteLine("---------------" + bitKey.Length);
            foreach (bool bit in bitKey)
            {
                Console.Write(bit ? 1 : 0);
            }
            int[] massPC1 = new int[] { 57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18, 10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36, 63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4 };
            BitArray keyBit = new BitArray(56);
            for (int i = 0; i < massPC1.Length; i++)
            {
                keyBit[i] = bitKey[massPC1[i] - 1];
            }
            BitArray bitArrayC = new BitArray(28);
            BitArray bitArrayD = new BitArray(28);
            int couter = 0;
            for (int i = 0; i < keyBit.Length; i++)
            {
                if (i < 28)
                {
                    bitArrayC[i] = keyBit[couter];
                    couter++;
                    if (couter > 27)
                    {
                        couter = 0;
                    }
                }
                else
                {
                    bitArrayD[couter] = keyBit[i];
                    couter++;
                }
            }
            int[] massKeyGenerate = new int[] { 14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4, 26, 8, 16, 7, 27, 20, 13, 2, 41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32 };
            int[] massGet = new int[] { 4, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4, 26, 8, 16, 7, 27, 20, 13, 2, 41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32 };
            for (int i = 0; i < 16; i++)
            {
                if (i == 0 || i == 1 || i == 8 || i == 15)
                {
                    bitArrayC = LeftShiftC(bitArrayC, 1);
                    bitArrayD = LeftShiftC(bitArrayC, 1);
                }
                else
                {
                    bitArrayC = LeftShiftC(bitArrayC, 2);
                    bitArrayD = LeftShiftC(bitArrayD, 2);
                }
                BitArray bitArrayFoundKey = new BitArray(56);
                couter = 0;
                for (int x = 0; x < bitArrayFoundKey.Length; x++)
                {
                    if (x < 28)
                    {
                        bitArrayFoundKey[x] = bitArrayC[couter];
                        couter++;
                        if (couter > 27)
                        {
                            couter = 0;
                        }
                    }
                    else
                    {
                        bitArrayFoundKey[x] = bitArrayD[couter];
                        couter++;
                    }
                }
                BitArray bitArrayKey = new BitArray(48);
                for (int p = 0; p < massKeyGenerate.Length; p++)
                {
                    bitArrayKey[p] = bitArrayFoundKey[massKeyGenerate[p] - 1];
                }
                listKeys.Add(bitArrayKey);
            }
            return listKeys;
        }
        public BitArray LeftShiftC(BitArray bits, int count)
        {
            int length = bits.Length;
            BitArray result = new BitArray(length);
            for (int i = 0; i < length; i++)
            {
                result[i] = bits[(i + count) % length];
            }
            return result;
        }
        private byte[] ConvertToByteArray(BitArray bitArray)
        {
            int bytes = (bitArray.Length + 7) / 8;
            byte[] arr2 = new byte[bytes];
            int bitIndex = 0;
            int byteIndex = 0;

            for (int i = 0; i < bitArray.Length; i++)
            {
                if (bitArray[i])
                {
                    arr2[byteIndex] |= (byte)(1 << bitIndex);
                }

                bitIndex++;
                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    byteIndex++;
                }
            }

            return arr2;
        }
    }
}
