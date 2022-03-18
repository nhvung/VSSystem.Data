using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data.File.Images.FDO
{
    public class ImageFDO
    {

        long _ID;
        public long ID { get { return _ID; } set { _ID = value; } }

        string _ImageType;
        public string ImageType { get { return _ImageType; } set { _ImageType = value; } }

        byte[] _Data;
        public byte[] Data { get { return _Data; } set { _Data = value; } }

        int _ImageIndex;
        public int ImageIndex { get { return _ImageIndex; } set { _ImageIndex = value; } }

        long _Original_ID;
        public long Original_ID { get { return _Original_ID; } set { _Original_ID = value; } }

    }
}
