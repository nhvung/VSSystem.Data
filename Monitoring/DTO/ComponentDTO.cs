using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.DTO;

namespace VSSystem.Data.Monitoring.DTO
{
    public class ComponentDTO : DataDTO
    {

        int _ID;
        public int ID { get { return _ID; } set { _ID = value; } }

        int _Parent_ID;
        public int Parent_ID { get { return _Parent_ID; } set { _Parent_ID = value; } }

        int _Server_ID;
        public int Server_ID { get { return _Server_ID; } set { _Server_ID = value; } }

        string _Name;
        public string Name { get { return _Name; } set { _Name = value; } }

        byte _Type;
        public byte Type { get { return _Type; } set { _Type = value; } }

        int _HttpPort;
        public int HttpPort { get { return _HttpPort; } set { _HttpPort = value; } }
        int _HttpsPort;
        public int HttpsPort { get { return _HttpsPort; } set { _HttpsPort = value; } }

        byte _Status;
        public byte Status { get { return _Status; } set { _Status = value; } }

        long _CreatedDateTime;
        public long CreatedDateTime { get { return _CreatedDateTime; } set { _CreatedDateTime = value; } }

        long _UpdatedDateTime;
        public long UpdatedDateTime { get { return _UpdatedDateTime; } set { _UpdatedDateTime = value; } }

    }
}
