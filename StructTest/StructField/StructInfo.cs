using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StructTest.StructField
{
    public struct IntStruct
    {
        int Integer;
    }
    [StructLayout(LayoutKind.Sequential, Pack =1)]
    public struct FloatStruct
    {
        float Float;
    }
    public struct ByteStruct
    {
        byte Byte;
    }
    unsafe public struct UnsafeByteArray
    {
        public fixed byte Byte[10];
    }
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct StructLaytoutStruct
    {
        [FieldOffset(0)] byte First_byte;
        [FieldOffset(1)] byte Second_byte;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct StructLayoutStructSequencial
    {
        byte First_byte;
        byte Second_byte;
        int Third_byte;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct StructLayoutStructSequencial_Pack0
    {
        byte First_byte;
        byte Second_byte;
        int Third_byte;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 10)]
    public struct StructLayoutStructSequencialSize
    {
        byte First_byte;
        byte Second_byte;
        int Third_byte;
    }

    public struct StructInnerStruct
    {
        FloatStruct Floatstruct;
    }
    public struct StructInnerStruct2
    {
        [MarshalAs(UnmanagedType.Struct)]
        FloatStruct Floatstruct;
        byte mValue;
        byte mValue2;
    }
    public struct StructInnerStruct3
    {
        [MarshalAs(UnmanagedType.Struct)]
        MixStruct2 MixSturct;//13
        byte mValue;
        byte mValue2;
    }
    public struct StructInnerStruct4
    {
        [MarshalAs(UnmanagedType.Struct, SizeConst = 10)]
        MixStruct2[] MixSturct;//13
        byte mValue;
        byte mValue2;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct StructInnerStruct5
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public MixSturct3[] MixSturct;//13
        [MarshalAs(UnmanagedType.I1)] byte mValue;
        [MarshalAs(UnmanagedType.I1)] byte mValue2;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MixStruct
    {
        byte Byte1;
        byte Byte2;
        double Double1;
        byte Byte3;
    }

    unsafe public struct MixStruct2
    {
        byte Byte1;
        byte Byte2;
        fixed byte ByteArr[10];
        byte Byte3;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MixSturct3
    {

        [MarshalAs(UnmanagedType.I1)] byte Byte1;
        [MarshalAs(UnmanagedType.I1)] byte Byte2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] ByteArr;
        [MarshalAs(UnmanagedType.I1)] byte Byte3;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class myClass
    {
        byte[] mValue;
        public myClass()
        {
            mValue = new byte[9];
        }
    }



}
