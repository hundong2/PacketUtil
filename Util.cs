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

        #region Bit Field Calculator
        /// <summary>
        /// function name : GetByteFieldValue
        /// variable bit field variable check function
        /// 0 <= startPosition, length <= 32bit(4byte) 
        /// default used byte
        /// </summary>
        /// <typeparam name="T">type of variable</typeparam>
        /// T tpye : byte, unsigned short, signed short, unsigned integer, signed integer
        /// <param name="targetValue">target variable</param>
        /// <param name="startPos">bit start position</param>
        /// <param name="length">bit length from start position</param>
        /// <returns>check bit field variable ( and )</returns>
        static public T GetByteFieldValue<T>(T targetValue, int startPos, int length)
        {
            int andVariable = 0;
            foreach(var i in Enumerable.Range(0, length))
            {
                andVariable |= 1 << i;
            }
            if (typeof(T) == typeof(int))
                return (T)(object)(Convert.ToInt32(targetValue) & (andVariable << startPos));
            else if (typeof(T) == typeof(uint))
                return (T)(object)Convert.ChangeType((int)Convert.ToInt32(targetValue) & (andVariable << startPos), typeof(uint));
            else if (typeof(T) == typeof(ushort))
                return (T)(object)Convert.ChangeType((int)Convert.ToInt32(targetValue) & (andVariable << startPos), typeof(ushort));
            else if (typeof(T) == typeof(short))
                return (T)(object)Convert.ChangeType((int)Convert.ToInt32(targetValue) & (andVariable << startPos), typeof(short));
            else if (typeof(T) == typeof(float))
                return (T)(object)Convert.ChangeType((int)Convert.ToDouble(targetValue) & (andVariable << startPos), typeof(float));
            else if (typeof(T) == typeof(double))
                return (T)(object)Convert.ChangeType((int)Convert.ToDouble(targetValue)& (andVariable << startPos), typeof(double));
            return (T)(object)Convert.ToByte((int)Convert.ToByte(targetValue) & (andVariable << startPos));     
        }


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
        #endregion

        public static T As<T>(this object o)
        {
            return (T)o;
        }
    }
}
