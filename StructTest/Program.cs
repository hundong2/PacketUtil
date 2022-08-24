using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using StructTest.StructField;
namespace StructTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int IntStructSize = Marshal.SizeOf(typeof(IntStruct));
            int FloatStructSize = Marshal.SizeOf(typeof(FloatStruct));
            int ByteStructSize = Marshal.SizeOf(typeof(ByteStruct));
            int UnsafeByteArraySize = Marshal.SizeOf(typeof(UnsafeByteArray));
            int StructLaytoutStructSize = Marshal.SizeOf(typeof(StructLaytoutStruct));
            int StructLayoutStructSequencialSize = Marshal.SizeOf(typeof(StructLayoutStructSequencial));
            int StructLayoutStructSequencialPackSize = Marshal.SizeOf(typeof(StructLayoutStructSequencial_Pack0));
            int StructLayoutStructSequencialSizeSize = Marshal.SizeOf(typeof(StructLayoutStructSequencialSize));
            int SturctInnerSturctSize = Marshal.SizeOf(typeof(StructInnerStruct));
            int SturctInnerSturctMixSize = Marshal.SizeOf(typeof(StructInnerStruct2));
            int SturctInnerSturctMixSize2 = Marshal.SizeOf(typeof(StructInnerStruct3));
            try
            {
                int SturctInnerSturctMixSize3 = Marshal.SizeOf(typeof(StructInnerStruct4));
            }
            catch
            {

            }
            int SturctInnerSturctMixSize4 = Marshal.SizeOf(typeof(StructInnerStruct5));
            int MixStructSize = Marshal.SizeOf(typeof(MixStruct));
            myClass mCls = new myClass();
            int ClassSize = Marshal.SizeOf(mCls);
            int MixStructArrSize = Marshal.SizeOf(typeof(MixStruct2));
        }
    }
}
