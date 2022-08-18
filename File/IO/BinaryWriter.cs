using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VSSystem.Data.File.Define;
using VSSystem.Data.File.Images.Define;

namespace VSSystem.Data.File
{
    public class BinaryWriter : System.IO.BinaryWriter
    {
        protected BinaryWriter()
        {
        }
        public BinaryWriter(Stream output) : base(output)
        {
        }

        public BinaryWriter(Stream output, Encoding encoding) : base(output, encoding)
        {
        }

        public BinaryWriter(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen)
        {
        }
        void WriteType(EBinaryType type)
        {
            base.Write((byte)type);
        }
        public override void Write(bool value)
        {
            WriteType(EBinaryType.Boolean);
            base.Write(value);
        }
        public override void Write(byte value)
        {
            WriteType(EBinaryType.Byte);
            base.Write(value);
        }
        public override void Write(byte[] buffer)
        {
            WriteType(EBinaryType.Bytes);
            base.Write(buffer.Length);
            base.Write(buffer);
        }
        public override void Write(char ch)
        {
            WriteType(EBinaryType.Char);
            base.Write(ch);
        }
        public override void Write(decimal value)
        {
            WriteType(EBinaryType.Decimal);
            base.Write(value);
        }
        public override void Write(double value)
        {
            WriteType(EBinaryType.Double);
            base.Write(value);
        }
        public override void Write(float value)
        {
            WriteType(EBinaryType.Float);
            base.Write(value);
        }
        public override void Write(int value)
        {
            WriteType(EBinaryType.Int32);
            base.Write(value);
        }
        public override void Write(long value)
        {
            WriteType(EBinaryType.Int64);
            base.Write(value);
        }
        public override void Write(sbyte value)
        {
            WriteType(EBinaryType.SByte);
            base.Write(value);
        }
        public override void Write(short value)
        {
            WriteType(EBinaryType.Int16);
            base.Write(value);
        }
        public override void Write(string value)
        {
            WriteType(EBinaryType.String);
            base.Write(value.Length);
            var sBytes = Encoding.UTF8.GetBytes(value);
            base.Write(sBytes, 0, sBytes.Length);
        }
        public override void Write(uint value)
        {
            WriteType(EBinaryType.UInt32);
            base.Write(value);
        }
        public override void Write(ulong value)
        {
            WriteType(EBinaryType.UInt64);
            base.Write(value);
        }
        public override void Write(ushort value)
        {
            WriteType(EBinaryType.UInt16);
            base.Write(value);
        }
        public void Write(Stream value)
        {
            WriteType(EBinaryType.Stream);
            base.Write(value.Length);
            value.CopyTo(base.BaseStream);
        }
    }
}
