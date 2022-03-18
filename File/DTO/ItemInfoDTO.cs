using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.DTO;

namespace VSSystem.Data.File.DTO
{
    public class ItemInfoDTO : DataDTO
    {

        long _ID;
        public long ID { get { return _ID; } set { _ID = value; } }

        int _File_ID;
        public int File_ID { get { return _File_ID; } set { _File_ID = value; } }

        long _Position;
        public long Position { get { return _Position; } set { _Position = value; } }

        string _Path;
        public string Path { get { return _Path; } set { _Path = value; } }
        public ItemInfoDTO()
        {
            _ID = 0;
            _File_ID = 0;
            _Position = 0;
            _Path = string.Empty;
        }
    }
}
