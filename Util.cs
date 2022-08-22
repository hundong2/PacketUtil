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
            HEXVALUE = 1,
            BYTEARR = 2
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

        /// <summary>
        /// Get information of type, type size and Type of type
        /// </summary>
        /// <param name="type">type name string</param>
        /// <returns>Tuple variable of type information</returns>
        static public Tuple<int,Type> GetInfoType(string type)
        {
            Tuple<int, Type> returnSize = null;

            switch(type)
            {
                case "int":
                    returnSize = new Tuple<int, Type>(sizeof(int), typeof(int));
                    break;
                case "uint":
                    returnSize = new Tuple<int, Type>(sizeof(uint), typeof(uint)); 
                    break;
                case "float":
                    returnSize = new Tuple<int, Type>(sizeof(double), typeof(double));
                    break;
                case "double":
                    returnSize = new Tuple<int, Type>(sizeof(double), typeof(double));
                    break;
                case "short":
                    returnSize = new Tuple<int, Type>(sizeof(short), typeof(short));
                    break;
                case "ushort":
                    returnSize = new Tuple<int, Type>(sizeof(ushort), typeof(ushort));
                    break;
                case "byte":
                    returnSize = new Tuple<int, Type>(sizeof(byte), typeof(byte));
                    break;
                default:
                    break;
            }

            return returnSize;
        }


        /// <summary>
        /// Get Packet Value 
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="name"></param>
        /// <param name="pType"></param>
        /// <param name="typeofvalue"></param>
        /// <param name="length"></param>
        /// <param name="arrayposition"></param>
        /// <returns></returns>
        static public Object DecoderPacket(byte[] arr, string pType, string typeofvalue, int length, int arrayposition)
        {

            if (pType == ConstVariable.ORIGIN)
            {

                Byte[] SubData = new Byte[length];
                Buffer.BlockCopy(arr, arrayposition, SubData, 0, SubData.Length);
                return (object)SubData;
            }
            else if (pType == ConstVariable.TYPEVALUE)
            {
                var typeInfo = Util.GetInfoType(typeofvalue);
                if (typeInfo == null) return (object)null;
                var type = typeInfo.ToValueTuple().Item2;
                Byte[] SubData = new Byte[typeInfo.ToValueTuple().Item1];
                int offset = 0;
                Buffer.BlockCopy(arr, arrayposition, SubData, 0,Math.Min(length, arr.Length - arrayposition));

                if (type == typeof(sbyte)) return (sbyte)((sbyte)SubData[offset]);
                if (type == typeof(byte)) return (byte)SubData[offset];
                if (type == typeof(short)) return (short)((short)(SubData[offset + 1] << 8 | SubData[offset]));
                if (type == typeof(ushort)) return (ushort)((ushort)(SubData[offset + 1] << 8 | SubData[offset]));
                if (type == typeof(int)) return (int)(SubData[offset + 3] << 24 | SubData[offset + 2] << 16 | SubData[offset + 1] << 8 | SubData[offset]);
                if (type == typeof(uint)) return (uint)((uint)SubData[offset + 3] << 24 | (uint)SubData[offset + 2] << 16 | (uint)SubData[offset + 1] << 8 | SubData[offset]);
                if (type == typeof(long)) return (long)((long)SubData[offset + 7] << 56 | (long)SubData[offset + 6] << 48 | (long)SubData[offset + 5] << 40 | (long)SubData[offset + 4] << 32 | (long)SubData[offset + 3] << 24 | (long)SubData[offset + 2] << 16 | (long)SubData[offset + 1] << 8 | SubData[offset]);
                if (type == typeof(ulong)) return (ulong)((ulong)SubData[offset + 7] << 56 | (ulong)SubData[offset + 6] << 48 | (ulong)SubData[offset + 5] << 40 | (ulong)SubData[offset + 4] << 32 | (ulong)SubData[offset + 3] << 24 | (ulong)SubData[offset + 2] << 16 | (ulong)SubData[offset + 1] << 8 | SubData[offset]);
                if (type == typeof(double)) return (double)(BitConverter.ToDouble(SubData, 0));
                if (type == typeof(float)) return (float)(BitConverter.ToDouble(SubData, 0));
                throw new NotImplementedException();
            }
            return (object)null;
        }

    }
}
