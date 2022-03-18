using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data
{
    public class SqlDbAuthentication
    {
        string _server, _username, _password;
        int _port;

        public SqlDbAuthentication(string server, string username, string password, int port = 0)
        {
            _server = server;
            _username = username;
            _password = password;
            _port = port;
        }

        public string Password
        {
            get
            {
                return _password;
            }
        }

        public string Server
        {
            get
            {
                return _server;
            }
        }

        public string Username
        {
            get
            {
                return _username;
            }
        }

        public int Port { get { return _port; } }

        public override string ToString()
        {
            return string.Format("{0}|{1}|{2}", _server, _username, _password);
        }

        public bool Equals(SqlDbAuthentication obj)
        {
            return _server.Equals(obj.Server, StringComparison.InvariantCultureIgnoreCase) && _username.Equals(obj.Username, StringComparison.InvariantCultureIgnoreCase) && _password.Equals(obj.Password);
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(SqlDbAuthentication))
                return Equals((SqlDbAuthentication)obj);
            else return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
