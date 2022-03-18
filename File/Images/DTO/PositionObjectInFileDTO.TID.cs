using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data.File.Images.DTO
{
    public class PositionObjectInFileDTO<TID> : PositionObjectInFileDTO
    {
        TID _ID;
        public TID ID { get { return _ID; } set { _ID = value; } }
    }
}
