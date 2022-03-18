using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data.File.Images.IO
{
    public class ImageInfo
    {

        long _ID;
        public long ID { get { return _ID; } set { _ID = value; } }

        byte[] _Data;
        public byte[] Data { get { return _Data; } set { _Data = value; } }
        public ImageInfo()
        {
            _ID = 0;
            _Data = new byte[0];
        }
        public ImageInfo(long id)
        {
            _ID = id;
            _Data = new byte[0];
        }
        public ImageInfo(long id, byte[] data)
        {
            _ID = id;
            _Data = data;
        }
    }
}
