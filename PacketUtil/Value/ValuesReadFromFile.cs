using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketUtil.Value
{
    static public class ValuesReadFromFile
    {

        const string strStartPosition = "POSITION";
        const string strLENGTH = "LENGTH";
        const string strTYPE = "STRUCT";


        /// <summary>
        /// Json File read from *.json ( bin/debug folder ) 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static public string[] GetValuesFromJson(string path)
        {
            string[] mTempValu = null;

            using (StreamReader file = File.OpenText(path))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject json = (JObject)JToken.ReadFrom(reader);

                foreach (var Obj in json)
                {
                    Console.WriteLine(Obj.Key.ToString());
                    GetValueFromJsonRecursive(Obj.Value.Value<JObject>());
                    //newval = Values.Builder(reader.Value.ToString(), 0, "struct", 0);
                }
            }
            return mTempValu;
        }

        static private string[] GetValueFromJsonRecursive(JObject json)
        {
            string[] mTempValue = null;
            string name, type, startPosition, length, lsb = "";

            foreach (var Obj in json)
            {
                Console.WriteLine(Obj.Key.ToString());
            }
            return mTempValue;
        }
    }
}
