using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketUtil.Value
{
    static public class ValuesUtil
    {
        public const int valueParsingDataLength = 5;
        /// <summary>
        /// <value>formatCheckEnum Enum Value</value>
        /// <type>name, type, start : start position of packet, length of packet field, lsb : calculate for value</type>
        /// </summary>
        enum formatCheckEnum
        {
            name = 0,
            type = 1,
            start = 2,
            length = 3,
            lsb = 4
        };

        /// <summary>
        /// get Class of Values from string formatting ( name, type, start position, length, lsb )
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        static public Values GetValuesFromStrFormat(string format)
        {
            Values mTempValue = null;
            char[] delimiterChars = { ' ', ',', ':', '\t', '/' };
            string[] parsingData = format.Split(delimiterChars);
            if (parsingData.Length == valueParsingDataLength && GetTypeEffectivenessCheck(parsingData[(int)formatCheckEnum.type]) )
            {
                mTempValue = Values.Builder(parsingData[(int)formatCheckEnum.name]
                                            , parsingData[(int)formatCheckEnum.type]
                                            , Convert.ToInt32(parsingData[(int)formatCheckEnum.start])
                                            , Convert.ToInt32(parsingData[(int)formatCheckEnum.length]) 
                                            , Convert.ToDouble(parsingData[(int)formatCheckEnum.lsb])
                                            );
            }
            return mTempValue;
        }

        static public bool GetTypeEffectivenessCheck(string type)
        {
            bool ReturnValue = false;
            type = type.ToLower();
            switch (type)
            {
                case "int":
                case "uint":
                case "float":
                case "double":
                case "short":
                case "ushort":
                case "byte":
                case "bit":
                case "struct":
                    ReturnValue = true;
                    break;
                default:
                    ReturnValue =  false;
                    break;
            }
            return ReturnValue;
        }
    }
}
