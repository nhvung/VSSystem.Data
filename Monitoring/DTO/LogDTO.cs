using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.DTO;

namespace VSSystem.Data.Monitoring.DTO
{
    public class LogDTO : DataDTO
    {

        long _ID;
        public long ID { get { return _ID; } set { _ID = value; } }

        int _Server_ID;
        public int Server_ID { get { return _Server_ID; } set { _Server_ID = value; } }

        int _Component_ID;
        public int Component_ID { get { return _Component_ID; } set { _Component_ID = value; } }

        byte _Type;
        public byte Type { get { return _Type; } set { _Type = value; } }

        string _Name;
        public string Name { get { return _Name; } set { _Name = value; } }

        string _Contents;
        public string Contents { get { return _Contents; } set { _Contents = value; } }

        long _CreatedTicks;
        public long CreatedTicks { get { return _CreatedTicks; } set { _CreatedTicks = value; } }

        string _Tag;
        public string Tag { get { return _Tag; } set { _Tag = value; } }

        long _ServerTicks;
        public long ServerTicks { get { return _ServerTicks; } set { _ServerTicks = value; } }

    }
}
