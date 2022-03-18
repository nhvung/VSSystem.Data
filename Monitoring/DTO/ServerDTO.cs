using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.DTO;

namespace VSSystem.Data.Monitoring.DTO
{
    public class ServerDTO : DataDTO
    {

        int _ID;
        public int ID { get { return _ID; } set { _ID = value; } }

        string _Name;
        public string Name { get { return _Name; } set { _Name = value; } }

        string _IPAddress;
        public string IPAddress { get { return _IPAddress; } set { _IPAddress = value; } }

        string _HostUrl;
        public string HostUrl { get { return _HostUrl; } set { _HostUrl = value; } }

        byte _Status;
        public byte Status { get { return _Status; } set { _Status = value; } }

        long _CreatedDateTime;
        public long CreatedDateTime { get { return _CreatedDateTime; } set { _CreatedDateTime = value; } }

        long _UpdatedDateTime;
        public long UpdatedDateTime { get { return _UpdatedDateTime; } set { _UpdatedDateTime = value; } }

    }
}
