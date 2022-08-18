using System;
using System.IO;
using System.Text;
using VSSystem.Data.File.Define;

namespace VSSystem.Data.File
{
    public class BinaryReader : System.IO.BinaryReader
    {
        public BinaryReader(Stream input) : base(input)
        {
        }

        public BinaryReader(Stream input, Encoding encoding) : base(input, encoding)
        {
        }

        public BinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }
        void VerifyType(EBinaryType type)
        {
            long pos = BaseStream.Position;
            var typeByte = base.ReadByte();
            EBinaryType bType = (EBinaryType)typeByte;
            if (bType != type)
            {
                BaseStream.Seek(pos, SeekOrigin.Begin);
                throw new Exception("Invalid Data Type");
            }
        }
        public byte[] ReadBytes()
        {
            VerifyType(EBinaryType.Bytes);
            int length = base.ReadInt32();
            byte[] buffer = new byte[length];
            base.Read(buffer, 0, length);
            return buffer;
        }

        public override bool ReadBoolean()
        {
            VerifyType(EBinaryType.Boolean);
            return base.ReadBoolean();
        }

        public override byte ReadByte()
        {
            VerifyType(EBinaryType.Byte);
            return base.ReadByte();
        }

        public override char ReadChar()
        {
            VerifyType(EBinaryType.Char);
            return base.ReadChar();
        }

        public override decimal ReadDecimal()
        {
            VerifyType(EBinaryType.Decimal);
            return base.ReadDecimal();
        }

        public override double ReadDouble()
        {
            VerifyType(EBinaryType.Double);
            return base.ReadDouble();
        }

        public override short ReadInt16()
        {
            VerifyType(EBinaryType.Int16);
            return base.ReadInt16();
        }

        public override int ReadInt32()
        {
            VerifyType(EBinaryType.Int32);
            return base.ReadInt32();
        }

        public override long ReadInt64()
        {
            VerifyType(EBinaryType.Int64);
            return base.ReadInt64();
        }

        public override sbyte ReadSByte()
        {
            VerifyType(EBinaryType.SByte);
            return base.ReadSByte();
        }

        public override float ReadSingle()
        {
            VerifyType(EBinaryType.Float);
            return base.ReadSingle();
        }

        public override string ReadString()
        {
            VerifyType(EBinaryType.String);
            int sLength = base.ReadInt32();
            var sBytes = base.ReadBytes(sLength);
            var s = Encoding.UTF8.GetString(sBytes);
            return s;
        }

        public override ushort ReadUInt16()
        {
            VerifyType(EBinaryType.UInt16);
            return base.ReadUInt16();
        }

        public override uint ReadUInt32()
        {
            VerifyType(EBinaryType.UInt32);
            return base.ReadUInt32();
        }

        public override ulong ReadUInt64()
        {
            VerifyType(EBinaryType.UInt64);
            return base.ReadUInt64();
        }
        public Stream ReadStream()
        {
            VerifyType(EBinaryType.Stream);
            long sLength = base.ReadInt64();
            return base.BaseStream;
        }
    }
}
