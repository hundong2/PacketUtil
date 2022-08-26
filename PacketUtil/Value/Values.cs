namespace PacketUtil
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    /// <summary>
    /// Packet Field Saving Class
    /// </summary>
    public class Values : IDisposable
    {


        /// <value>Name</value>
        public string Name { get; private set; }    //vale field name
        public int ArrayPosition { get; private set; } //start Position
        public int Length { get; private set; }     //vale field length
        public string TypeOfValue { get; private set; } //current tpye of class 
        public double LSB { get; private set; } // LSB Value (option)
        public Values Parent { get; private set; }
       
        public Dictionary<string, Values> SubValues = new Dictionary<string, Values>();
        #region Builder Class of Values
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="length"></param>
        /// <param name="typeval"></param>
        /// <param name="startposition"></param>
        /// <returns></returns>
        static public Values Builder(string name, string typeval, int startposition, int length, double lsb = 0)
        {
            return new Values(name, length, typeval, startposition, lsb);
        }
        #endregion
        #region Protected Constructor
        protected Values()
        {

        }
        /// <summary>
        /// Class of Values
        /// </summary>
        /// <param name="length">
        /// Values Packet Length
        /// </param>
          
        protected Values(int length) { this.Length = length; ArrayPosition = 0; }
        protected Values(string name, int length)
        {
            Name = name;
            this.Length = length;
            ArrayPosition = 0;
        }
        protected Values(string name, int length, string typeval, int arrayposition, double lsb = 0)
        {
            Name = name;
            var tempValue = Util.GetInfoType(typeval);
            if (tempValue != null)
                Length = length;
            else
                Length = 0;
            TypeOfValue = typeval;
            ArrayPosition = arrayposition;
            LSB = lsb;
        }
        #endregion
        #region Add SubValues
        /// <summary>
        /// Sub Field Variable Add 
        /// </summary>
        /// <param name="pValue"></param>
        public void AddSubValues(Values pValue)
        {
            pValue.Parent = this;
            SubValues[pValue.Name] = pValue;   
        }
        #endregion
        #region Value get function set
        /// <summary>
        /// String Value of Packet field 
        /// </summary>
        /// <param name="arr">packet array</param>
        /// <param name="name">field name of packet</param>
        /// <returns></returns>
        public string GetStrValue(byte[] arr, string name)
        {
            Object mValue = GetValue(arr, name, ConstVariable.TYPEVALUE);
            if (mValue == null) return string.Empty;
            return mValue.ToString();
        }


        /// <summary>
        /// Value Class, GetValue information function
        /// </summary>
        /// <param name="arr">packet value</param>
        /// <param name="name">packet field name</param>
        /// <param name="pType">return type</param>
        /// <returns></returns>
        public Object GetValue(byte[] arr, string name, string pType = ConstVariable.TYPEVALUE)
        {
            if (SubValues.ContainsKey(name))
            {
                if (pType == ConstVariable.ORIGIN)
                {

                    Byte[] SubData = new Byte[SubValues[name].Length];
                    Buffer.BlockCopy(arr, SubValues[name].ArrayPosition, SubData, 0, SubData.Length);
                    return (object)SubData;
                }
                else if (pType == ConstVariable.TYPEVALUE)
                {
                    var typeInfo = Util.GetInfoType(SubValues[name].TypeOfValue);
                    var type = typeInfo.ToValueTuple().Item2;
                    Byte[] SubData = new Byte[typeInfo.ToValueTuple().Item1];
                    int offset = 0;
                    Buffer.BlockCopy(arr, SubValues[name].ArrayPosition, SubData, 0, Math.Min(SubValues[name].Length, arr.Length - SubValues[name].ArrayPosition));
                    
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
            }
            return (object)null;
        }


        /// <summary>
        /// GetValue Type action function Generic 
        /// </summary>
        /// <typeparam name="T">type of value</typeparam>
        /// <param name="arr">packet array</param>
        /// <param name="name">name of field</param>
        /// <param name="pType">type of return ( byte array : ORIGIN, type value : TYPEVALUE )</param>
        /// <param name="variablesize">variable size</param>
        /// <returns></returns>
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
        #endregion
        #region function of bit field
        #region Public Function
        ///public bool SeekFieldName(string packetFieldName)
        /// <summary>
        /// Seek Field Name of Values Class 
        /// </summary>
        /// <param name="packetFieldName"></param>
        /// <returns></returns>
        public bool SeekFieldName(string packetFieldName)
        {
            if (GetBitFieldClass(packetFieldName) != null)
                return true;
            return false;
        }
        #endregion

        #region Protected 
        ///Protected Level 
        /// <summary>
        /// SeekField Function for that seeking field Name 
        /// </summary>
        /// <param name="cls">Values Class ( Member of Values Class )</param>
        /// <param name="fieldName">Packet Field Name</param>
        /// <returns>Class Values</returns>
        protected Values SeekField(Values cls, string fieldName)
        {
            Values ReturnValue = null;
            if (cls.SubValues.ContainsKey(fieldName))
            {
                ReturnValue = cls.SubValues[fieldName];
            }
            else
            {
                if(cls.SubValues.Count > 0)
                {
                    foreach(var mValue in cls.SubValues)
                    {
                        ReturnValue = SeekField(mValue.Value, fieldName);
                        if (ReturnValue != null) break;
                    }
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// Protected Get Bit Field Function 
        /// Protected Level
        /// </summary>
        /// <param name="fieldName">field Name</param>
        /// <returns>Class of Values class</returns>
        protected Values GetBitFieldClass(string fieldName)
        {
            Values ReturnValue = null;
            if( SubValues.ContainsKey(fieldName))
            {
                return SubValues[fieldName];
            }
            else
            {
                foreach (var myValue in SubValues)
                {
                    ReturnValue = SeekField(myValue.Value, fieldName);
                    if (ReturnValue != null) break;
                }
                    
            }
            return ReturnValue;
        }
        #endregion

        #endregion
        #region Decoder Packet 
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
        static public Object DecoderPacket(byte[] arr, string pType, Values myValue)
        {
            if(myValue.TypeOfValue == "bit")
            {
                object parentValue = DecoderPacket(arr, ConstVariable.TYPEVALUE, myValue.Parent);
                object bitValue = Util.GetByteFieldValue<object>(parentValue, myValue.ArrayPosition , myValue.Length);
                
                return bitValue;
            }
            if (pType == ConstVariable.ORIGIN)
            {

                Byte[] SubData = new Byte[myValue.Length];
                Buffer.BlockCopy(arr, myValue.ArrayPosition, SubData, 0, SubData.Length);
                return (object)SubData;
            }
            else if (pType == ConstVariable.TYPEVALUE)
            {
                var typeInfo = Util.GetInfoType(myValue.TypeOfValue);
                if (typeInfo == null) return (object)null;
                var type = typeInfo.ToValueTuple().Item2;
                Byte[] SubData = new Byte[typeInfo.ToValueTuple().Item1];
                int offset = 0;
                Buffer.BlockCopy(arr, myValue.ArrayPosition, SubData, 0, Math.Min(myValue.Length, arr.Length - myValue.ArrayPosition));

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
        #endregion
        #region recursive function set
        /// <summary>
        /// Overrided ToString function For Values Class 
        /// </summary>
        /// <returns>Output member field name</returns>
        public string ToParsing(ref byte[] packet)
        {
            string tap = "\t";
            string ReturnValue = "";
            if (TypeOfValue != "bit") tap = "";
            if(this.Parent != null)
            {
                ReturnValue += string.Format("{2} {0, -10} : {1,5}\n", Name, Values.DecoderPacket(packet, ConstVariable.TYPEVALUE, this), tap);
            }
            foreach (var mValue in SubValues)
            {
                ReturnValue += mValue.Value.ToParsing(ref packet);
            }
            return ReturnValue;

        }
        public override string ToString()
        {
            string tap = "\t";
            string ReturnValue = "";
            if (TypeOfValue != "bit")
                tap = "";
            ReturnValue += string.Format("{4} {3,-10} [{0,3} ~ {1,-3}] {2,-10}\n", ArrayPosition, ArrayPosition+Length -1, Name, TypeOfValue, tap);


            foreach (KeyValuePair<string, Values> items in SubValues)
            {
                ReturnValue += items.Value.ToString();
            }
            //return base.ToString();
            return ReturnValue;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
