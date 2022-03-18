using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data.File.Images.IO
{
    public class ImagePositionInfo
    {
        long _Image_ID;
        public long Image_ID { get { return _Image_ID; } set { _Image_ID = value; } }

        string _Path;
        public string Path { get { return _Path; } set { _Path = value; } }

        long _Position;
        public long Position { get { return _Position; } set { _Position = value; } }
        public ImagePositionInfo()
        {
            _Image_ID = 0;
            _Path = string.Empty;
            _Position = 0;
        }
        public ImagePositionInfo(long image_ID, string path, long position)
        {
            _Image_ID = image_ID;
            _Path = path;
            _Position = position;
        }
    }
}
