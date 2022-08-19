using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketUtil
{
    public static class Util
    {
        public enum VALUE_TYPE
        {
            INTVALUE = 0,
            HEXVALUE = 1
        };
        /// <summary>
        /// Byte Array convert to string value
        /// </summary>
        /// <param name="bytes">byte array</param>
        /// <param name="val_type">Type of string format</param>
        /// <returns></returns>
        static public string PrintByteArray(byte[] bytes, VALUE_TYPE val_type = VALUE_TYPE.INTVALUE)
        {
            string hex_val = "";
            if ( val_type == VALUE_TYPE.HEXVALUE)
            {
                hex_val = BitConverter.ToString(bytes);
            }
            else if( val_type == VALUE_TYPE.INTVALUE)
                hex_val =  string.Join(" ", bytes);
            return hex_val;
        }

        public static T As<T>(this object o)
        {
            return (T)o;
        }
    }
}
