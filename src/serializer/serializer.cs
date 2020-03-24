public partial class TN
{
    public abstract class SerializeStream
    {
        public abstract byte[] ByteData { get; }
        public abstract void Clear();
        public abstract void Reset();
        public abstract SerializeStream GetBool(out bool outValue);
        public abstract SerializeStream SetBool(bool inValue);
        public abstract SerializeStream GetByte(out byte outValue);
        public abstract SerializeStream SetByte(byte inValue);
        public abstract SerializeStream GetShort(out short outValue);
        public abstract SerializeStream SetShort(byte inValue);
        public abstract SerializeStream GetInt(out int outValue);
        public abstract SerializeStream SetInt(int inValue);
        public abstract SerializeStream GetLong(out long outValue);
        public abstract SerializeStream SetLong(long inValue);
        public abstract SerializeStream GetFloat(out float outValue);
        public abstract SerializeStream SetFloat(float inValue);
        public abstract SerializeStream GetDouble(out double outValue);
        public abstract SerializeStream SetFloat(double inValue);
        public abstract SerializeStream GetString(out string outValue);
        public abstract SerializeStream SetString(string inValue);
        public abstract SerializeStream GetVector3(out Vector3 outValue);
        public abstract SerializeStream SetVector3(Vector3 inValue);
    }

    public interface ISerializable
    {
        void Serialize(SerializeStream outStream);
        void Deserialize(SerializeStream inStream);
    }

    public class BinarySerializeStream : SerializeStream
    {
        protected int Offset { get; set; }
        protected List<byte> Bytes;
        public override byte[] ByteData
        {
            get
            {
                return Bytes.ToArray();
            }
        }
        public BinarySerializeStream()
        {
            Bytes = new List<byte>();
            Offset = 0;
        }
        public BinarySerializeStream(IEnumerable<byte> data)
        {
            Bytes = data.ToList();
            Offset = 0;
        }

        public override void Clear()
        {
            Bytes.Clear();
            Offset = 0;
        }

        public override void Reset()
        {
            Offset = 0;
        }

        public override SerializeStream GetBool(out bool outValue)
        {
            outValue = BitConverter.ToBoolean(Bytes.GetRange(Offset, sizeof(bool)).ToArray(), 0);
            Offset += sizeof(bool);
            return this;
        }
        public override SerializeStream SetBool(bool inValue)
        {
            Bytes.AddRange(BitConverter.GetBytes(inValue));
            return this;
        }
        public override SerializeStream GetByte(out byte outValue)
        {
            outValue = Bytes[Offset]; Offset += sizeof(byte);
            return this;
        }
        public override SerializeStream SetByte(byte inValue)
        {
            Bytes.Add(inValue);
            return this;
        }
        public override SerializeStream GetShort(out short outValue)
        {
            outValue = BitConverter.ToInt16(Bytes.GetRange(Offset, sizeof(short)).ToArray(), 0);
            Offset += sizeof(byte);
            return this;
        }
        public override SerializeStream SetShort(byte inValue)
        {
            Bytes.AddRange(BitConverter.GetBytes(inValue));
            return this;
        }
        public override SerializeStream GetInt(out int outValue)
        {
            outValue = BitConverter.ToInt32(Bytes.GetRange(Offset, sizeof(int)).ToArray(), 0);
            Offset += sizeof(int);
            return this;
        }
        public override SerializeStream SetInt(int inValue)
        {
            Bytes.AddRange(BitConverter.GetBytes(inValue));
            return this;
        }
        public override SerializeStream GetLong(out long outValue)
        {
            outValue = BitConverter.ToInt64(Bytes.GetRange(Offset, sizeof(long)).ToArray(), 0);
            Offset += sizeof(long);
            return this;
        }
        public override SerializeStream SetLong(long inValue)
        {
            Bytes.AddRange(BitConverter.GetBytes(inValue));
            return this;
        }
        public override SerializeStream GetFloat(out float outValue)
        {
            outValue = BitConverter.ToSingle(Bytes.GetRange(Offset, sizeof(float)).ToArray(), 0);
            Offset += sizeof(float);
            return this;
        }
        public override SerializeStream SetFloat(float inValue)
        {
            Bytes.AddRange(BitConverter.GetBytes(inValue));
            return this;
        }
        public override SerializeStream GetDouble(out double outValue)
        {
            outValue = BitConverter.ToDouble(Bytes.GetRange(Offset, sizeof(double)).ToArray(), 0);
            Offset += sizeof(double);
            return this;
        }
        public override SerializeStream SetDouble(double inValue)
        {
            Bytes.AddRange(BitConverter.GetBytes(inValue));
            return this;
        }
        public override SerializeStream GetString(out string outValue)
        {
            int length = BitConverter.ToInt32(Bytes.GetRange(
            Offset, sizeof(int)).ToArray(), 0);
            outValue = System.Text.UTF8Encoding.UTF8.GetString(
            Bytes.GetRange(Offset += sizeof(int), length).ToArray());
            Offset += length;
            return this;
        }
        public override SerializeStream SetString(string inValue)
        {
            Bytes.AddRange(BitConverter.GetBytes(inValue.Length));
            Bytes.AddRange(System.Text.UTF8Encoding.UTF8.GetBytes(inValue));
            return this;
        }
        public override SerializeStream GetVector3(out Vector3 outValue)
        {
            outValue = new Vector3(
            BitConverter.ToSingle(Bytes.GetRange(Offset, sizeof(float)).ToArray(), 0),
            BitConverter.ToSingle(Bytes.GetRange(
              Offset += sizeof(float), sizeof(float)).ToArray(), 0),
            BitConverter.ToSingle(Bytes.GetRange(
              Offset += sizeof(float), sizeof(float)).ToArray(), 0)
            );
            Offset += sizeof(float);
            return this;
        }
        public override SerializeStream SetVector3(Vector3 inValue)
        {
            Bytes.AddRange(BitConverter.GetBytes(inValue.X));
            Bytes.AddRange(BitConverter.GetBytes(inValue.Y));
            Bytes.AddRange(BitConverter.GetBytes(inValue.Z));
            return this;
        }
    }

    public class Formatter
    {
        private SerializeStream stream;

        public SerializeStream Stream { get { return stream; } private set { stream = value; } }

        public Formatter(SerializeStream sstream)
        {
            Stream = sstream;
        }

        public void Reset()
        {
            Stream.Reset();
        }

        public void Serialize(ISerializable inObj)
        {
            inObj.Serialize(stream);
        }

        public void Deserialize(ISerializable outObj)
        {
            outObj.Deserialize(stream);
        }
    }
}