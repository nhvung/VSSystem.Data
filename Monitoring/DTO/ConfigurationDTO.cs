using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.DTO;

namespace VSSystem.Data.Monitoring.DTO
{
    public class ConfigurationDTO : DataDTO
    {

        long _ID;
        public long ID { get { return _ID; } set { _ID = value; } }

        int _Server_ID;
        public int Server_ID { get { return _Server_ID; } set { _Server_ID = value; } }

        int _Component_ID;
        public int Component_ID { get { return _Component_ID; } set { _Component_ID = value; } }

        string _Path;
        public string Path { get { return _Path; } set { _Path = value; } }

        string _Value;
        public string Value { get { return _Value; } set { _Value = value; } }

        byte _Status;
        public byte Status { get { return _Status; } set { _Status = value; } }

        long _CreatedDateTime;
        public long CreatedDateTime { get { return _CreatedDateTime; } set { _CreatedDateTime = value; } }

        long _UpdatedDateTime;
        public long UpdatedDateTime { get { return _UpdatedDateTime; } set { _UpdatedDateTime = value; } }

    }
}
