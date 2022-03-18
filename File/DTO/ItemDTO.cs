using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data.File.DTO
{
    public class ItemDTO : Data.DTO.DataDTO
    {

        byte[] _Sha1;
        /// <summary>
        /// Primary Key
        /// </summary>
        public byte[] Sha1 { get { return _Sha1; } set { _Sha1 = value; } }

        long _ID;
        public long ID { get { return _ID; } set { _ID = value; } }

        int _File_ID;
        public int File_ID { get { return _File_ID; } set { _File_ID = value; } }

        long _Position;
        public long Position { get { return _Position; } set { _Position = value; } }

        long _CreatedDateTime;
        public long CreatedDateTime { get { return _CreatedDateTime; } set { _CreatedDateTime = value; } }
        public ItemDTO()
        {
            _Sha1 = null;
            _ID = 0;
            _File_ID = 0;
            _Position = 0;
            _CreatedDateTime = 0;
        }
    }
}
