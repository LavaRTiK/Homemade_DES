using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

        public string Coding(byte[] text, byte[] key)
        {
            //GenereteList Keys
            List<BitArray> listKeys = CreateKeys(key);
            Console.WriteLine();
            foreach (var roundkey in listKeys)
            {
                Console.WriteLine(string.Join("", roundkey.ToBits()));
            }
            //Текст в блок битов List сохраняет блоки по 64 бит
            List<BitArray> blockCoding = new List<BitArray>();
            List<BitArray> listnewBlocksBits = new List<BitArray>();
            byte[] textByte = text;// Encoding.UTF8.GetBytes(text);
            List<byte> textByteList  = textByte.ToList();
            int blockCount = (textByte.Length + 7) / 8;
            for (int i = 0; i < blockCount; i++)
            {
                byte[] temp = new byte[8];
                textByteList.CopyTo(0,temp,0,textByteList.Count < 8 ? textByteList.Count : 8);
                textByteList.RemoveRange(0,textByteList.Count < 8 ? textByteList.Count : 8);
                BitArray bitTemp = new BitArray(64).Fill(temp);
                Console.WriteLine(textByteList.Count < 8 ? textByteList.Count : 8);
                blockCoding.Add(bitTemp);
            }
            Console.WriteLine("");
            Console.WriteLine("Биты до зашиврованния");
            foreach (BitArray bitArray in blockCoding)
            {
                Console.WriteLine("");
                foreach (bool bit in bitArray)
                {
                    Console.Write(bit?1:0);
                }
            }
            Console.WriteLine("");
            Console.WriteLine("end");
            for (int large = 0; large < blockCoding.Count; large++)
            {
                //blockCoding воходяший кол блоков по 64
                BitArray currentBlock = new BitArray(blockCoding[large]);
                int[] massIP = new int[] { 58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4, 62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8, 57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3, 61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7 };
                BitArray bitArrayIP = new BitArray(64);
                for (int i = 0; i < currentBlock.Length; i++)
                {
                    bitArrayIP[i] = currentBlock[massIP[i] - 1];
                }
                Console.WriteLine($"BitIp: {string.Join("", bitArrayIP.ToBits())}");
                BitArray rsultRound = new BitArray(bitArrayIP);
                //тут цикл
                for (int iterator = 0; iterator < 16; iterator++)
                {
                    Console.WriteLine("итерация"+iterator);
                    BitArray bitLeft = new BitArray(32);
                    BitArray bitRight = new BitArray(32);
                    int bitcount = 0;
                    for (int i = 0; i < rsultRound.Length; i++)
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
                    Console.WriteLine($"BitLeft: {string.Join("", bitLeft.ToBits())}");
                    Console.WriteLine($"BitRight: {string.Join("", bitRight.ToBits())}");
                    int[] massE = new int[] { 32, 1, 2, 3, 4, 5, 4, 5, 6, 7, 8, 9, 8, 9, 10, 11, 12, 13, 12, 13, 14, 15, 16, 17, 16, 17, 18, 19, 20, 21, 20, 21, 22, 23, 24, 25, 24, 25, 26, 27, 28, 29, 28, 29, 30, 31, 32, 1 };
                    BitArray bitERight = new BitArray(48);
                    for (int i = 0; i < massE.Length; i++)
                    {
                        bitERight[i] = bitRight[massE[i] - 1];
                    }
                    Console.WriteLine($"BitE: {string.Join("", bitERight.ToBits())}");
                    BitArray resultXor = bitERight.Xor(listKeys[iterator]); // тут ключ (парвая часть с ключом)
                    Console.WriteLine($"BitResultXor: {string.Join("", resultXor.ToBits())}");
                    Console.WriteLine(resultXor.Length);
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
                    int counter = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        BitArray dade = new BitArray(6);
                        for (int x = 0; x < 6; x++)
                        {
                            dade[x] = resultXor[counter];
                            counter++;
                        }
                        int test13 = S_BOX[i, Convert.ToInt16($"{Convert.ToInt16(dade[0])}{Convert.ToInt16(dade[5])}",2), Convert.ToInt16($"{Convert.ToInt16(dade[1])}{Convert.ToInt16(dade[2])}{Convert.ToInt16(dade[3])}{Convert.ToInt16(dade[4])}",2)];
                        list.Add(test13);

                    }
                    BitArray bitArray = new BitArray(32);
                    string bitString = "";
                    foreach (int value in list)
                    {
                        bitString += Convert.ToString(value, 2).PadLeft(4, '0');
                    }
                    for (int i = 0; i < bitString.Length; i++)
                    {
                        bitArray[i] = bitString[i] == '1';
                    }
                    Console.WriteLine(bitArray.Length);
                    Console.WriteLine($"BitArraySbox: {string.Join("", bitArray.ToBits())}");
                    int[] massP = new int[] { 16, 7, 20, 21, 29, 12, 28, 17, 1, 15, 23, 26, 5, 18, 31, 10, 2, 8, 24, 14, 32, 27, 3, 9, 19, 13, 30, 6, 22, 11, 4, 25 };
                    BitArray bitArrayP = new BitArray(32);
                    for (int i = 0; i < massP.Length; i++)
                    {
                        bitArrayP[i] = bitArray[massP[i] - 1];
                    }
                    Console.WriteLine($"BitArrayP: {string.Join("", bitArrayP.ToBits())}");
                    //bitaaryp с левой частю

                    BitArray BitArrayXorWithBitLeft = new BitArray(bitLeft);
                    BitArrayXorWithBitLeft.Xor(bitArrayP);

                    Console.WriteLine($"BitArrayXorWithBitLeft: {string.Join("",BitArrayXorWithBitLeft.ToBits())}");
                    //BitArrayXorWithBitLeft
                    int rsultCounter = 0;
                    for (int i = 0; i < rsultRound.Length; i++)
                    {
                        if (iterator == 15)
                        {
                            if (i <= 31)
                            {
                                rsultRound[i] = BitArrayXorWithBitLeft[rsultCounter];
                                rsultCounter++;
                                if (rsultCounter > 31)
                                {
                                    rsultCounter = 0;
                                }
                            }
                            else
                            {
                                rsultRound[i] = bitRight[rsultCounter];
                                rsultCounter++;
                            }
                        }
                        else
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
                                rsultRound[i] = BitArrayXorWithBitLeft[rsultCounter];
                                rsultCounter++;
                            }
                        }
                        
                    }
                    Console.WriteLine($"RsultRound: {string.Join("", rsultRound.ToBits())}");

                }
                //Console.WriteLine($"RsultRound: {string.Join("", rsultRound.ToBits())}");
                int[] massIPRevers = new int[] { 40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31, 38,6, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29, 36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27, 34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9, 49, 17, 57, 25 };
                BitArray  bitArrayIPRevers = new BitArray(64);
                for (int i = 0; i < rsultRound.Length; i++)
                {
                    bitArrayIPRevers[i] = rsultRound[massIPRevers[i] - 1];
                }
                Console.WriteLine($"BitArrayIPRevers: {string.Join("",bitArrayIPRevers.ToBits())}");
                listnewBlocksBits.Add(bitArrayIPRevers);
            }
            string hexText = "";
            foreach (BitArray item in listnewBlocksBits)
            {
                hexText += Convert.ToHexString(ConvertToByteArray(item));
            }
            return hexText;


        }
        public string Decoding(byte[] text,byte[] key) 
        {
            List<BitArray> listKeys = CreateKeys(key);
            Console.WriteLine();
            listKeys.Reverse();
            foreach (var roundkey in listKeys)
            {
                Console.WriteLine(string.Join("", roundkey.ToBits()));
            }
            //Текст в блок битов List сохраняет блоки по 64 бит
            List<BitArray> blockCoding = new List<BitArray>();
            List<BitArray> listnewBlocksBits = new List<BitArray>();
            byte[] textByte = text;// Encoding.UTF8.GetBytes(text);
            List<byte> textByteList = textByte.ToList();
            int blockCount = (textByte.Length + 7) / 8;
            for (int i = 0; i < blockCount; i++)
            {
                byte[] temp = new byte[8];
                textByteList.CopyTo(0, temp, 0, textByteList.Count < 8 ? textByteList.Count : 8);
                textByteList.RemoveRange(0, textByteList.Count < 8 ? textByteList.Count : 8);
                BitArray bitTemp = new BitArray(64).Fill(temp);
                Console.WriteLine(textByteList.Count < 8 ? textByteList.Count : 8);
                blockCoding.Add(bitTemp);
            }
            Console.WriteLine("");
            Console.WriteLine("Биты до зашиврованния");
            foreach (BitArray bitArray in blockCoding)
            {
                Console.WriteLine("");
                foreach (bool bit in bitArray)
                {
                    Console.Write(bit ? 1 : 0);
                }
            }
            Console.WriteLine("");
            Console.WriteLine("end");
            for (int large = 0; large < blockCoding.Count; large++)
            {
                //blockCoding воходяший кол блоков по 64
                BitArray currentBlock = new BitArray(blockCoding[large]);
                int[] massIP = new int[] { 58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4, 62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8, 57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3, 61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7 };
                BitArray bitArrayIP = new BitArray(64);
                for (int i = 0; i < currentBlock.Length; i++)
                {
                    bitArrayIP[i] = currentBlock[massIP[i] - 1];
                }
                Console.WriteLine($"BitIp: {string.Join("", bitArrayIP.ToBits())}");
                BitArray rsultRound = new BitArray(bitArrayIP);
                //тут цикл
                for (int iterator = 0; iterator < 16; iterator++)
                {
                    Console.WriteLine("итерация" + iterator);
                    BitArray bitLeft = new BitArray(32);
                    BitArray bitRight = new BitArray(32);
                    int bitcount = 0;
                    for (int i = 0; i < rsultRound.Length; i++)
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
                    Console.WriteLine($"BitLeft: {string.Join("", bitLeft.ToBits())}");
                    Console.WriteLine($"BitRight: {string.Join("", bitRight.ToBits())}");
                    int[] massE = new int[] { 32, 1, 2, 3, 4, 5, 4, 5, 6, 7, 8, 9, 8, 9, 10, 11, 12, 13, 12, 13, 14, 15, 16, 17, 16, 17, 18, 19, 20, 21, 20, 21, 22, 23, 24, 25, 24, 25, 26, 27, 28, 29, 28, 29, 30, 31, 32, 1 };
                    BitArray bitERight = new BitArray(48);
                    for (int i = 0; i < massE.Length; i++)
                    {
                        bitERight[i] = bitRight[massE[i] - 1];
                    }
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine($"BitE: {string.Join("", bitERight.ToBits())}");
                    BitArray resultXor = bitERight.Xor(listKeys[iterator]); // тут ключ (парвая часть с ключом)
                    Console.WriteLine($"BitResultXor: {string.Join("", resultXor.ToBits())}");
                    Console.WriteLine(resultXor.Length);
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
                    int counter = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        BitArray dade = new BitArray(6);
                        for (int x = 0; x < 6; x++)
                        {
                            dade[x] = resultXor[counter];
                            counter++;
                        }
                        int test13 = S_BOX[i, Convert.ToInt16($"{Convert.ToInt16(dade[0])}{Convert.ToInt16(dade[5])}", 2), Convert.ToInt16($"{Convert.ToInt16(dade[1])}{Convert.ToInt16(dade[2])}{Convert.ToInt16(dade[3])}{Convert.ToInt16(dade[4])}", 2)];
                        list.Add(test13);

                    }
                    BitArray bitArray = new BitArray(32);
                    string bitString = "";
                    foreach (int value in list)
                    {
                        bitString += Convert.ToString(value, 2).PadLeft(4, '0');
                    }
                    for (int i = 0; i < bitString.Length; i++)
                    {
                        bitArray[i] = bitString[i] == '1';
                    }
                    Console.WriteLine(bitArray.Length);
                    Console.WriteLine($"BitArraySbox: {string.Join("", bitArray.ToBits())}");
                    int[] massP = new int[] { 16, 7, 20, 21, 29, 12, 28, 17, 1, 15, 23, 26, 5, 18, 31, 10, 2, 8, 24, 14, 32, 27, 3, 9, 19, 13, 30, 6, 22, 11, 4, 25 };
                    BitArray bitArrayP = new BitArray(32);
                    for (int i = 0; i < massP.Length; i++)
                    {
                        bitArrayP[i] = bitArray[massP[i] - 1];
                    }
                    Console.WriteLine($"BitArrayP: {string.Join("", bitArrayP.ToBits())}");
                    //bitaaryp с левой частю

                    BitArray BitArrayXorWithBitLeft = new BitArray(bitLeft);
                    BitArrayXorWithBitLeft.Xor(bitArrayP);

                    Console.WriteLine($"BitArrayXorWithBitLeft: {string.Join("", BitArrayXorWithBitLeft.ToBits())}");
                    //BitArrayXorWithBitLeft
                    int rsultCounter = 0;
                    for (int i = 0; i < rsultRound.Length; i++)
                    {
                        if (iterator == 15)
                        {
                            if (i <= 31)
                            {
                                rsultRound[i] = BitArrayXorWithBitLeft[rsultCounter];
                                rsultCounter++;
                                if (rsultCounter > 31)
                                {
                                    rsultCounter = 0;
                                }
                            }
                            else
                            {
                                rsultRound[i] = bitRight[rsultCounter];
                                rsultCounter++;
                            }
                        }
                        else
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
                                rsultRound[i] = BitArrayXorWithBitLeft[rsultCounter];
                                rsultCounter++;
                            }
                        }

                    }
                    Console.WriteLine($"RsultRound: {string.Join("", rsultRound.ToBits())}");

                }
                //Console.WriteLine($"RsultRound: {string.Join("", rsultRound.ToBits())}");
                int[] massIPRevers = new int[] { 40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31, 38, 6, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29, 36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27, 34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9, 49, 17, 57, 25 };
                BitArray bitArrayIPRevers = new BitArray(64);
                for (int i = 0; i < rsultRound.Length; i++)
                {
                    bitArrayIPRevers[i] = rsultRound[massIPRevers[i] - 1];
                }
                Console.WriteLine($"BitArrayIPRevers: {string.Join("", bitArrayIPRevers.ToBits())}");
                listnewBlocksBits.Add(bitArrayIPRevers);
            }
            string hexText = "";
            foreach (BitArray item in listnewBlocksBits)
            {
                hexText += Convert.ToHexString(ConvertToByteArray(item));
            }
            return hexText;
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
        private List<BitArray> CreateKeys(byte[] key)
        {
            List<BitArray> listKeys = new List<BitArray>();
            BitArray bitKey = new BitArray(64).Fill(key);
            BitArray a = new BitArray(new byte[] {19});
            Console.WriteLine($"Key: {string.Join("", bitKey.ToBits())}");
            int[] massPC1 = new int[] { 57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18, 10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36, 63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4 };
            BitArray keyBit = new BitArray(56);
            for (int i = 0; i < massPC1.Length; i++)
            {
                keyBit[i] = bitKey[massPC1[i] - 1];
            }
            Console.WriteLine();
            Console.WriteLine($"Key after pc1: {string.Join("", keyBit.ToBits())}");

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
            for (int i = 0; i < 16; i++)
            {
                if (i == 0 || i == 1 || i == 8 || i == 15)
                {
                    bitArrayC = LeftShiftC(bitArrayC, 1);
                    bitArrayD = LeftShiftC(bitArrayD, 1);
                }
                else
                {
                    bitArrayC = LeftShiftC(bitArrayC, 2);
                    bitArrayD = LeftShiftC(bitArrayD, 2);
                }
                Console.WriteLine($"C{i}: {string.Join("", bitArrayC.ToBits())}");
                Console.WriteLine($"D{i}: {string.Join("", bitArrayD.ToBits())}");
                Console.WriteLine();

                BitArray bitArrayFoundKey = bitArrayC.Append(bitArrayD);
                
                BitArray bitArrayKey = new BitArray(48);
                int[] massKeyGenerate = new int[] { 14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4, 26, 8, 16, 7, 27, 20, 13, 2, 41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32 };
                for (int p = 0; p < massKeyGenerate.Length; p++)
                {
                    bitArrayKey[p] = bitArrayFoundKey[massKeyGenerate[p] - 1];
                }
                listKeys.Add(bitArrayKey);
            }
            foreach (BitArray bitArray in listKeys)
            {
                Console.WriteLine($"Ключ: {string.Join("", bitArray.ToBits())}");
            }
            return listKeys;
        }
        public BitArray LeftShiftC(BitArray bits, int count)
        {
            int length = bits.Length;
            BitArray result = new BitArray(length);
            for (int i = 0; i < length; i++)
            {
                var index = i - count;
                if (index < 0)
                {
                    index += length;
                }

                result[index] = bits[i];
            }
            return result;
        }
        private byte[] ConvertBitArrayToByteArray2(BitArray bitArray)
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
        private static byte[] ConvertToByteArray(BitArray bitArray)
        {
            int bytes = (bitArray.Length + 7) / 8;
            byte[] arr2 = new byte[bytes];

            for (int i = 0; i < bitArray.Length; i++)
            {
                if (bitArray[i])
                {
                    arr2[i / 8] |= (byte)(1 << (7 - (i % 8))); // Используйте 7 - (i % 8) для правильной позиции
                }
            }

            return arr2;
        }
        private byte[] ConvertBitArrayToByteArray(BitArray bitArray)
        {
            int bytes = (bitArray.Length + 7) / 8; // Количество байтов
            byte[] byteArray = new byte[bytes]; // Создаем массив байтов

            for (int i = 0; i < bitArray.Length; i++)
            {
                if (bitArray[i]) // Если бит установлен в 1
                {
                    byteArray[i / 8] |= (byte)(1 << (i % 8)); // Устанавливаем соответствующий бит в байте
                }
            }

            return byteArray; // Возвращаем массив байтов
        }

    }
}
