using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PacketUtil
{
    [ComVisible(true)]
    public struct Bit : IComparable, IFormattable, IConvertible
    {
        string value { get; set; }

        
        public object GetByteFieldValue<T>(int startPos, int length)
        {
            int andVariable = 0;
            foreach (var i in Enumerable.Range(0, length))
            {
                andVariable |= 1 << i;
            }
            if (typeof(T) == typeof(int))
                return (object)(Convert.ToInt32(value) & (andVariable << startPos));
            else if (typeof(T) == typeof(uint))
                return (object)Convert.ChangeType((int)Convert.ToInt32(value) & (andVariable << startPos), typeof(uint));
            else if (typeof(T) == typeof(ushort))
                return (object)Convert.ChangeType((int)Convert.ToInt32(value) & (andVariable << startPos), typeof(ushort));
            else if (typeof(T) == typeof(short))
                return (object)Convert.ChangeType((int)Convert.ToInt32(value) & (andVariable << startPos), typeof(short));
            else if (typeof(T) == typeof(float))
                return (object)Convert.ChangeType((UInt32)Convert.ToUInt32(value) & (UInt32)(andVariable << startPos), typeof(float));
            else if (typeof(T) == typeof(double))
                return (object)Convert.ChangeType((UInt64)Convert.ToUInt64(value) & (UInt64)(andVariable << startPos), typeof(double));
            else if (typeof(T) == typeof(object))
                return (T)(object)Convert.ChangeType((UInt64)Convert.ToUInt64(value) & (UInt64)(andVariable << startPos), typeof(object));
            return (object)Convert.ToByte((int)Convert.ToByte(value) & (andVariable << startPos));
        }
        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public TypeCode GetTypeCode()
        {
            throw new NotImplementedException();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        public string ToString(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }
    }
}
