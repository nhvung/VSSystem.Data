using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.DTO;

namespace VSSystem.Data.File.Images.DTO
{
    public class PositionObjectInFileDTO : DataDTO
    {
        string _File_ID;
        public string File_ID { get { return _File_ID; } set { _File_ID = value; } }

        string _RootPath;
        public string RootPath { get { return _RootPath; } set { _RootPath = value; } }

        string _RelativePath;
        public string RelativePath { get { return _RelativePath; } set { _RelativePath = value; } }

        long _Offset;
        public long Offset { get { return _Offset; } set { _Offset = value; } }

        int _Length;
        public int Length { get { return _Length; } set { _Length = value; } }
    }
}
