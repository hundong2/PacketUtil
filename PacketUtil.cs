using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            // Display the number of command line arguments.
            byte[] temp = new byte[] { 0x00, 0x11, 0x11, 0x33, 0x11, 0x29, 0x5C, 0x8F, 0xC2, 0xF5, 0xA8, 0x28, 0x40, 0xAE, 0x47, 0x45, 0x41 };
            byte[] temp2 = new byte[8] { 0x29, 0x5C, 0x8F, 0xC2, 0xF5, 0xA8, 0x28, 0x40 };
            Values newval = Values.Builder("hello", 9, "ARRAY", 0);
            newval.AddSubValues(Values.Builder("h", 1, "int", 0));
            newval.AddSubValues(Values.Builder("h2", 1, "int", 1));
            newval.AddSubValues(Values.Builder("h3", 1, "int", 2));
            newval.AddSubValues(Values.Builder("h4", 1, "int", 3));
            newval.AddSubValues(Values.Builder("h5", sizeof(double), "double", 5));
            newval.AddSubValues(Values.Builder("h7", sizeof(float), "float", 13));

            //Values newv = GetValuesFromJson("packet.json");
            float f = 12.33f;
            double val = newval.GetValue<double>(temp, "h5", ConstVariable.TYPEVALUE, sizeof(double));
            double he = BitConverter.ToDouble(temp2, 0);
            string valstring = newval.GetValue(temp, "h7");
            Console.WriteLine(newval.ToString());
            Console.WriteLine(BitConverter.ToString(BitConverter.GetBytes(f)));
            Console.WriteLine(Util.PrintByteArray(temp, Util.VALUE_TYPE.HEXVALUE));
            
        }
        static public Values GetValuesFromJson(string path)
        {
            Values newval = null;
            using (StreamReader file = File.OpenText(path))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject json = (JObject)JToken.ReadFrom(reader);
                
                //foreach(var Obj in json)
                {
                    newval = Values.Builder(reader.Value.ToString(), 0, "struct", 0);
                }
            }
            return newval;
        }
    }
}
