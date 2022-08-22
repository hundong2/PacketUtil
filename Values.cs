using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketUtil
{
    /// <summary>
    /// this class Used Util.cs in PacketUtil namespace 
    /// </summary>
    class Values 
    {
        public string Name { get; private set; }    //vale field name
        public int ArrayPosition { private get; set; } //start Position
        public int Length { get; private set; }     //vale field length
        public string TypeOfValue { private get; set; }
        
        private Dictionary<string, Values> SubValues = new Dictionary<string, Values>();

        #region Builder Class of Values
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="length"></param>
        /// <param name="typeval"></param>
        /// <param name="arrayposition"></param>
        /// <returns></returns>
        static public Values Builder(string name, int length, string typeval, int arrayposition)
        {
            return new Values(name, length, typeval, arrayposition);
        }
        #endregion
        #region Protected Constructor
        protected Values()
        {

        }
        protected Values(int length) { this.Length = length; ArrayPosition = 0; }
        protected Values(string name, int length)
        {
            Name = name;
            this.Length = length;
            ArrayPosition = 0;
        }
        protected Values(string name, int length, string typeval, int arrayposition)
        {
            Name = name;
            Length = length;
            TypeOfValue = typeval;
            ArrayPosition = arrayposition;
        }
        #endregion

        #region Add SubValues
        public void AddSubValues(Values pValue)
        {
            SubValues[pValue.Name] = pValue;
        }
        #endregion

        public T GetValue<T>(byte[] arr, string name, string pType, int variablesize)
        {
            if (SubValues.ContainsKey(name))
            {
                if ( pType == ConstVariable.ORIGIN)
                {
                    
                    Byte[] SubData = new Byte[SubValues[name].Length];
                    Buffer.BlockCopy(arr, SubValues[name].ArrayPosition, SubData, 0, SubData.Length);
                    return (T)(object)SubData;
                }
                else if( pType == ConstVariable.TYPEVALUE)
                {
                    Byte[] SubData = new Byte[variablesize];
                    int offset = 0;
                    Buffer.BlockCopy(arr, SubValues[name].ArrayPosition, SubData, 0, Math.Min(SubValues[name].Length, arr.Length - SubValues[name].ArrayPosition));
                    var type = typeof(T);
                    if (type == typeof(sbyte)) return (T)(object)((sbyte)SubData[offset]);
                    if (type == typeof(byte)) return (T)(object)SubData[offset];
                    if (type == typeof(short)) return (T)(object)((short)(SubData[offset + 1] << 8 | SubData[offset]));
                    if (type == typeof(ushort)) return (T)(object)((ushort)(SubData[offset + 1] << 8 | SubData[offset]));
                    if (type == typeof(int)) return (T)(object)(SubData[offset + 3] << 24 | SubData[offset + 2] << 16 | SubData[offset + 1] << 8 | SubData[offset]);
                    if (type == typeof(uint)) return (T)(object)((uint)SubData[offset + 3] << 24 | (uint)SubData[offset + 2] << 16 | (uint)SubData[offset + 1] << 8 | SubData[offset]);
                    if (type == typeof(long)) return (T)(object)((long)SubData[offset + 7] << 56 | (long)SubData[offset + 6] << 48 | (long)SubData[offset + 5] << 40 | (long)SubData[offset + 4] << 32 | (long)SubData[offset + 3] << 24 | (long)SubData[offset + 2] << 16 | (long)SubData[offset + 1] << 8 | SubData[offset]);
                    if (type == typeof(ulong)) return (T)(object)((ulong)SubData[offset + 7] << 56 | (ulong)SubData[offset + 6] << 48 | (ulong)SubData[offset + 5] << 40 | (ulong)SubData[offset + 4] << 32 | (ulong)SubData[offset + 3] << 24 | (ulong)SubData[offset + 2] << 16 | (ulong)SubData[offset + 1] << 8 | SubData[offset]);
                    if (type == typeof(double)) return (T)(object)(BitConverter.ToDouble(SubData, 0));
                    if (type == typeof(float)) return (T)(object)(BitConverter.ToDouble(SubData, 0));
                    throw new NotImplementedException();
                }
            }
            return (T)(object)null;
        }
        public string GetValue(byte[] arr, string name)
        {
            if( SubValues.ContainsKey(name) )
            {
                if(SubValues[name].TypeOfValue == "double" )
                    return this.GetValue<double>(arr, name, ConstVariable.TYPEVALUE, sizeof(double)).ToString();
                if (SubValues[name].TypeOfValue == "int")
                    return this.GetValue<int>(arr, name, ConstVariable.TYPEVALUE, sizeof(int)).ToString();
                if (SubValues[name].TypeOfValue == "float")
                    return this.GetValue<double>(arr, name, ConstVariable.TYPEVALUE, sizeof(double)).ToString();
            }
            return string.Empty;
        }
        public override string ToString()
        {
            string ReturnValue = string.Empty;
            foreach(KeyValuePair<string, Values> items in SubValues)
            {
                ReturnValue += string.Format("[{0}] {1} length {2}\n", items.Value.ArrayPosition, items.Key.ToString(), items.Value.Length);
            }
            //return base.ToString();
            return ReturnValue;
        }
    }
}
