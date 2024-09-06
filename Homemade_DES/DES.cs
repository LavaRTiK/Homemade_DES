using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homemade_DES
{
    internal class DES
    {
        private byte[] key;
        private byte[] text;
        public DES(byte[] key,byte[] text) {
            if(key.Length > 7 )
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
        private int[] CreateKeys()
        {
            return new int[] { 1, 0, 1, 0, 1, 0, 1, 0 };
        }
    }
}
