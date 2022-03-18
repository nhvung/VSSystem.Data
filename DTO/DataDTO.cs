using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data.DTO
{
    public class DataDTO : SqlDbRecord
    {
        public DataDTO() : base()
        {
            base.InitEmptyValue();
        }
    }
}
