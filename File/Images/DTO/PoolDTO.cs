using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.DTO;

namespace VSSystem.Data.File.Images.DTO
{
    public class PoolDTO : DataDTO
    {

        string _Name;
        public string Name { get { return _Name; } set { _Name = value; } }

        string _Path;
        public string Path { get { return _Path; } set { _Path = value; } }

        string _Description;
        public string Description { get { return _Description; } set { _Description = value; } }

        long _CreatedDateTime;
        public long CreatedDateTime { get { return _CreatedDateTime; } set { _CreatedDateTime = value; } }
        protected override void InitEmptyValue()
        {
            base.InitEmptyValue();
            _CreatedDateTime = 0;
            _Path = string.Empty;
            _Description = string.Empty;
            _Name = string.Empty;
        }
    }
}
