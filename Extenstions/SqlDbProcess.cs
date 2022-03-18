using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data
{
    public partial class SqlDbProcess : SqlProcess
    {
        const string _GET_TABLES_SQLSERVER = "select col.table_name, col.column_name, col.data_type, col.character_maximum_length, col.numeric_precision, col.numeric_scale, col.is_nullable, replace(pri.constraint_type, 'PRIMARY KEY','PRI') as column_key, col.character_set_name, columnproperty(object_id(col.table_schema+'.'+ col.table_name), col.column_name, 'isidentity') as is_identity "
                              + "from information_schema.columns col "
                              + "left join "
                              + "("
                              + "select kcu.column_name, tc.constraint_type, kcu.table_name "
                              + "from information_schema.key_column_usage kcu, information_schema.table_constraints tc "
                              + "where kcu.constraint_name = tc.constraint_name and tc.constraint_type = 'primary key') pri "
                              + "on pri.column_name = col.column_name and pri.table_name = col.table_name "
                              + "order by col.table_name, col.ordinal_position";

        const string _GET_INDEXES_SQLSERVER = "select table_name = t.name, index_name = ind.name, column_name = col.name, ind.is_unique % 2 as is_unique "
                        + "from sys.indexes ind "
                        + "join sys.index_columns ic on ind.object_id = ic.object_id and ind.index_id = ic.index_id "
                        + "join sys.columns col on ic.object_id = col.object_id and ic.column_id = col.column_id "
                        + "join sys.tables t on ind.object_id = t.object_id "
                        + "where ind.is_primary_key = 0";


        const string _GET_TABLES_MYSQL = "select col.table_name, col.column_name, col.data_type, col.character_maximum_length, col.numeric_precision, col.numeric_scale, col.is_nullable, col.column_key, col.character_set_name, if(extra = 'auto_increment', 1, 0) as is_identity from information_schema.columns col where col.table_schema = DATABASE() order by col.table_name, col.ordinal_position";

        const string _GET_INDEXES_MYSQL = "select table_name, index_name, column_name, (non_unique + 1) % 2 as is_unique from information_schema.statistics where index_name <> 'primary' and table_schema = DATABASE();";

        const string _GET_TABLE_NAMES_SQLSERVER = "select table_name from information_schema.tables  order by table_name";

        const string _GET_TABLE_NAMES_MYSQL = "select table_name from information_schema.tables where table_schema = database() order by table_name";

        const string _GET_PROCDURE_NAMES_SQLSERVER = "select routine_name from information_schema.routines where routine_type = 'PROCEDURE' order by routine_name";

        const string _GET_PROCDURE_NAMES_MYSQL = "select * from information_schema.routines where routine_type = 'PROCEDURE' and routine_schema = database() order by routine_name";

        const string _GET_TRIGGER_NAMES_SQLSERVER = "select sysobjects.name as trigger_name, object_name(parent_obj) as table_name from sysobjects where sysobjects.type = 'tr' order by sysobjects.name;";

        const string _GET_TRIGGER_NAMES_MYSQL = "select trigger_name from information_schema.TRIGGERS where trigger_schema = database() order by trigger_name";

        internal class SqlDbTableStruture
        {
            string _table_name;
            public string table_name { get { return _table_name; } set { _table_name = value; } }

            string _column_name;
            public string column_name { get { return _column_name; } set { _column_name = value; } }

            string _data_type;
            public string data_type { get { return _data_type; } set { _data_type = value; } }

            long _character_maximum_length;
            public long character_maximum_length { get { return _character_maximum_length; } set { _character_maximum_length = value; } }

            long _numeric_precision;
            public long numeric_precision { get { return _numeric_precision; } set { _numeric_precision = value; } }

            long _numeric_scale;
            public long numeric_scale { get { return _numeric_scale; } set { _numeric_scale = value; } }

            string _is_nullable;
            public string is_nullable { get { return _is_nullable; } set { _is_nullable = value; } }

            string _column_key;
            public string column_key { get { return _column_key; } set { _column_key = value; } }

            string _character_set_name;
            public string character_set_name { get { return _character_set_name; } set { _character_set_name = value; } }


            int _is_identity;
            public int is_identity { get { return _is_identity; } set { _is_identity = value; } }

        }


        internal class SqlDbTableIndex
        {
            string _table_name;
            public string table_name { get { return _table_name; } set { _table_name = value; } }

            string _index_name;
            public string index_name { get { return _index_name; } set { _index_name = value; } }

            string _column_name;
            public string column_name { get { return _column_name; } set { _column_name = value; } }

            byte _is_unique;
            public byte is_unique { get { return _is_unique; } set { _is_unique = value; } }
        }
    }
}
