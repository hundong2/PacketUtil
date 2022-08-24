using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PacketUtil.Value;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PacketUtil
{
    class PacketUtil
    {
        static void Main(string[] args)
        {
            object mValue = returnTest();
            Type mType = mValue.GetType();


            
            TestUtilGetByteFieldValue();
            //TestJsonRead();
            //testValueUtil();
        }
        static public void testValueUtil()
        {
            Values mTemp = ValuesUtil.GetValuesFromStrFormat("hello,uint,0,2,1");
            Console.WriteLine(mTemp.Name, mTemp.ArrayPosition, mTemp.Length);
        }

        static public void TestJsonRead()
        {
            string[] newv = ValuesReadFromFile.GetValuesFromJson("packet.json");
        }
        /// <summary>
        /// Util.cs Test of function GetByteFieldValue() Example 
        /// </summary>
        static public void TestUtilGetByteFieldValue()
        {
            // Display the number of command line arguments.
            byte[] temp = new byte[] { 0x1A, 0x11, 0x11, 0x33, 0x11, 0x29, 0x5C, 0x8F, 0xC2, 0xF5, 0xA8, 0x28, 0x40, 0xAE, 0x47, 0x45, 0x41 };
            byte[] temp2 = new byte[8] { 0x29, 0x5C, 0x8F, 0xC2, 0xF5, 0xA8, 0x28, 0x40 };
            Values newval = Values.Builder("hello", "ARRAY", 0, 9);
            newval.AddSubValues(Values.Builder("h", "byte", 0, 1)); //name, length, type, stPosition
            newval.AddSubValues(Values.Builder("h2", "ushort", 1, 2));
            newval.AddSubValues(Values.Builder("h3", "int", 3, 4));
            var BuilderByteFiled = Values.Builder("h4", "int", 3, 4);
            BuilderByteFiled.AddSubValues(Values.Builder("h4123", "bit", 0, 4));
            newval.AddSubValues(BuilderByteFiled);
            newval.AddSubValues(Values.Builder("h5", "double", 5, sizeof(double)));
            newval.AddSubValues(Values.Builder("h7", "float", 13, sizeof(float)));

            bool check = newval.SeekFieldName("h41234");
            
           
            //parsing temp packet variable 
            Console.WriteLine(newval.ToParsing(ref temp));
            Console.WriteLine(newval.ToString());
            
            //get value
            Type t = typeof(double);
            float f = 12.33f;
            double val = newval.GetValue<double>(temp, "h5", ConstVariable.TYPEVALUE, sizeof(double));
            object value123 = newval.GetValue(temp, "h5");
            string value123str = value123.ToString(); //object return value function 
            string value123strfunc = newval.GetStrValue(temp, "h5"); //string return function

            ushort val_short = newval.GetValue<ushort>(temp, "h2", ConstVariable.TYPEVALUE, sizeof(ushort));
            byte bytevval = newval.GetValue<byte>(temp, "h", ConstVariable.TYPEVALUE, sizeof(byte));
            byte tmepValue_1 = Util.GetByteFieldValue<byte>(bytevval, 0, 5);
            ushort tempValue_2 = Util.GetByteFieldValue<ushort>(val_short, 0, 16);


            double he = BitConverter.ToDouble(temp2, 0);
            
            byte tmepValue1 = Util.GetByteFieldValue<byte>(0xFF, 0, 2); //0 bit ~ 1, 2 bit ( length 3 )
            ushort tempShort = Util.GetByteFieldValue<ushort>(0xF1FF, 0, 1); // 0bit, 10 bit ( length 3)
            ushort tempShort2 = Util.GetByteFieldValue<ushort>(0xF1FF, 1, 2); // 1bit ~ 2, 10 bit ( length 2)
            ushort tempShort3 = Util.GetByteFieldValue<ushort>(0xF1FF, 2, 3); // 2bit ~ 5, 10 bit ( length 3)
            ushort tempShort4 = Util.GetByteFieldValue<ushort>(0xF1FF, 3, 4); // 3bit ~ 7, 10 bit ( length 4)
            ushort tempShort5 = Util.GetByteFieldValue<ushort>(0xF1FF, 4, 5); // 4bit ~ 9, 10 bit ( length 5)
            ushort tempShort6 = Util.GetByteFieldValue<ushort>(0xF1FF, 6, 7); // 5bit ~ 11, 10 bit ( length 6)
            double tempDouble = Util.GetByteFieldValue<double>(61951.23, 0, 64);
            int tempShort7 = Util.GetByteFieldValue<int>(61951, 6, 7); // 6bit ~ 13, 10 bit ( length 7)
            Console.WriteLine(tmepValue1.ToString());
            Console.WriteLine(Util.PrintByteArray(temp, Util.VALUE_TYPE.HEXVALUE));
        }

        static public object returnTest()
        {
            object mValue;
            mValue = (double)1;
            return (double)mValue;
        }
    }
}
