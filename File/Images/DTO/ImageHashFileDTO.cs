using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.File.DTO;

namespace VSSystem.Data.File.Images.DTO
{
    public class ImageHashFileDTO : ItemFileDTO
    {

        int _ImportType;
        public int ImportType { get { return _ImportType; } set { _ImportType = value; } }

        public ImageHashFileDTO() : base() 
        {
            _ImportType = 0;
        }
    }
}
