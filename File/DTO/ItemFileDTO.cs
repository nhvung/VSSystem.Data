using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.DTO;

namespace VSSystem.Data.File.DTO
{
    public class ItemFileDTO : DataDTO
    {

        int _File_ID;
        public int File_ID { get { return _File_ID; } set { _File_ID = value; } }

        string _Path;
        public string Path { get { return _Path; } set { _Path = value; } }

        byte _Status;
        public byte Status { get { return _Status; } set { _Status = value; } }

        long _CreatedDateTime;
        public long CreatedDateTime { get { return _CreatedDateTime; } set { _CreatedDateTime = value; } }
        public ItemFileDTO()
        {
            _File_ID = 0;
            _Path = string.Empty;
            _Status = 0;
            _CreatedDateTime = 0;
        }
    }
}
