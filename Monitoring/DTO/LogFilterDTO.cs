using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data.Monitoring.DTO
{
    public class LogFilterDTO
    {

        int _Server_ID;
        public int Server_ID { get { return _Server_ID; } set { _Server_ID = value; } }

        int _Component_ID;
        public int Component_ID { get { return _Component_ID; } set { _Component_ID = value; } }

        byte _Type;
        public byte Type { get { return _Type; } set { _Type = value; } }

        string _Name;
        public string Name { get { return _Name; } set { _Name = value; } }

        int _Total;
        public int Total { get { return _Total; } set { _Total = value; } }

    }
}
