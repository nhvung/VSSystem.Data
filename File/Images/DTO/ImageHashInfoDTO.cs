using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.DTO;
using VSSystem.Data.File.DTO;

namespace VSSystem.Data.File.Images.DTO
{
    public class ImageHashInfoDTO : ItemInfoDTO
    {

        long _Image_ID;
        public long Image_ID { get { return _Image_ID; } set { _Image_ID = value; } }

        int _Width;
        public int Width { get { return _Width; } set { _Width = value; } }

        int _Height;
        public int Height { get { return _Height; } set { _Height = value; } }
    }
}
