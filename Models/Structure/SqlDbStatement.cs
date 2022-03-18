using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data
{
    public class SqlDbStatement
    {
        public enum Type : int { Undefine = 0, Table = 1, Procedure = 2, Trigger = 4 }

        Type _StatementType;
        public Type StatementType { get { return _StatementType; } set { _StatementType = value; } }

        string _Name;
        public string Name { get { return _Name; } set { _Name = value; } }

        List<string> _CreateStatements;

        public List<string> CreateStatements { get { return _CreateStatements; } set { _CreateStatements = value; } }
        public SqlDbStatement()
        {
            _StatementType = Type.Undefine;
            _Name = "";
            _CreateStatements = new List<string>();
        }
        public SqlDbStatement(string name, List<string> createStatements, Type type)
        {
            _StatementType = type;
            _Name = name;
            _CreateStatements = createStatements;
        }
    }
}
