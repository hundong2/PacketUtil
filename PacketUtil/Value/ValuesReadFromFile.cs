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
        
        const string strStartPosition = "start";
        const string strLength = "length";
        const string strType = "type";
        const string structType = "struct";
        const string strLsb = "lsb";
        const string strBit = "bit";

        /// <summary>
        /// Json File read from *.json ( bin/debug folder ) 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static public Dictionary<string,Values> GetValuesFromJson(string path)
        {
            Dictionary<string, Values> jsonReadPacketInformation;

            using (StreamReader file = File.OpenText(path))
            using (JsonTextReader reader = new JsonTextReader(file))
            {

                JObject json = (JObject)JToken.ReadFrom(reader);
                jsonReadPacketInformation = GetValueFromJson(json);
                //newval = Values.Builder(reader.Value.ToString(), 0, "struct", 0);
                
            }
            return jsonReadPacketInformation;
        }

        /// <summary>
        /// Get Value From Json File
        /// </summary>
        /// <param name="json">JObject Class</param>
        /// <returns>Dictionary Value ( string and Values class )</returns>
        static private Dictionary<string, Values> GetValueFromJson(JObject json)
        {
            string name = ""; string type = ""; int stPosition = 0; int length = 0; double lsb = 0;
            Dictionary<string, Values> mmm = new Dictionary<string, Values>();
            foreach (var Obj in json)
            {
                var tempValue = Obj.Value.Contains(strType);
                name = Obj.Key.ToString();
                if( Obj.Value[strType] != null )
                    type = Obj.Value[strType].Value<string>();
                if( Obj.Value[strLength] != null )
                    length = Obj.Value[strLength].Value<int>();
                if( Obj.Value[strStartPosition] != null )
                    stPosition = Obj.Value[strStartPosition].Value<int>();
                if ( Obj.Value[strLsb] != null )
                    lsb = Obj.Value[strLsb].Value<double>();
                mmm[name] = Values.Builder(name, type, stPosition, length);

                if ( type == structType && Obj.Value[structType] != null)
                {
                    var vmal = mmm[name];
                    GetValueFromJsonRecursive(Obj.Value[structType].Value<JObject>(), ref vmal, 0);
                }
            }
            return mmm;
        }
        /// <summary>
        /// Get value From JObject Class ( for Recursive call )
        /// </summary>
        /// <param name="json">JObject</param>
        /// <param name="values">Values Class</param>
        /// <param name="start">start position</param>
        static private void GetValueFromJsonRecursive(JObject json, ref Values values,int start)
        {
            string name = ""; string type = ""; int stPosition = 0; int length = 0; double lsb = 0;
            foreach (var Obj in json)
            {
                var tempValue = Obj.Value.Contains(strType);

                name = Obj.Key.ToString();
                if (Obj.Value[strType] != null)
                    type = Obj.Value[strType].Value<string>();
                if (Obj.Value[strLength] != null)
                    length = Obj.Value[strLength].Value<int>();
                if (Obj.Value[strStartPosition] != null)
                {
                    stPosition = Obj.Value[strStartPosition].Value<int>();
                }  
                else
                {
                    stPosition = start;
                }
                if (Obj.Value[strLsb] != null)
                    lsb = Obj.Value[strLsb].Value<double>();

                values.AddSubValues(Values.Builder(name, type, stPosition, length));
                if(type == structType && Obj.Value[structType] != null)
                {
                    var vmal = values.SubValues[name];
                    GetValueFromJsonRecursive(Obj.Value[structType].Value<JObject>(), ref vmal, stPosition);
                }
                if(Obj.Value[strBit] != null )
                {
                     var vmal = values.SubValues[name];
                     GetValueFromJsonRecursive(Obj.Value[strBit].Value<JObject>(), ref vmal, 0);

                }
                start += length;
            }
        }
    }
}
