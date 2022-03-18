using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data.File.DTO
{
    public class MappingObjectDTO : Data.DTO.DataDTO
    {

        long _ID;
        public long ID { get { return _ID; } set { _ID = value; } }

        long _Item_ID;
        public long Item_ID { get { return _Item_ID; } set { _Item_ID = value; } }

        long _Base_ID;
        public long Base_ID { get { return _Base_ID; } set { _Base_ID = value; } }

        long _CreatedDateTime;
        public long CreatedDateTime { get { return _CreatedDateTime; } set { _CreatedDateTime = value; } }

    }
}
