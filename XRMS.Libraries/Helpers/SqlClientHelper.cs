using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace XRMS.Libraries.Helpers
{
    public class SqlClientHelper
    {
        //public static String ConnectionString = "Data Source=XUAN-PC;Initial Catalog=NewRestaurant;User ID=sa;Password=Mt50603113!;Connection Timeout=20";
        //public static string _ConStr = ConfigurationManager.ConnectionStrings["XRMS"].ConnectionString;
        public static string _ConStr = "Data Source=XUAN-PC;Initial Catalog=XRMS;User ID=sa;Password=Mt50603113!;Connection Timeout=20";

        //public static string _ConStr = "ConnectionString";

        public static void GetConnectionString()
        {
            _ConStr = ConfigurationManager.ConnectionStrings["XRMS"].ConnectionString;
        }
        /// <summary>
        /// Create parameter.
        /// </summary>
        /// <returns>SqlParameter</returns>
        public static SqlParameter CreateParameter(string parameterName, SqlDbType type, object parameterValue)
        {
            SqlParameter parameter = new SqlParameter(parameterName, type);
            if (parameterValue == null)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = parameterValue;
            return parameter;
        }

        /// <summary>
        /// Create parameter.
        /// </summary>
        /// <returns>SqlParameter</returns>
        public static SqlParameter CreateParameter(string parameterName, object parameterValue)
        {
            return new SqlParameter(parameterName, parameterValue);
        }

        /// <summary>
        /// Set the connection, command, and then execute the command with non query.
        /// </summary>
        /// <returns>number of rows affected</returns>
        public static Int32 ExecuteNonQueryBase(String connectionString, String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    // There're three command types: StoredProcedure, Text, TableDirect. The TableDirect 
                    // type is only for OLE DB.  
                    cmd.CommandType = commandType;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();

                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Set the connection, command, and then execute the command and only return one value.
        /// </summary>
        /// <returns>Object</returns>
        public static Object GetExecuteScalarBase(String connectionString, String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    // There're three command types: StoredProcedure, Text, TableDirect. The TableDirect 
                    // type is only for OLE DB.  
                    cmd.CommandType = commandType;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Set the connection, command, and then execute the command with query and return the reader.
        /// </summary>
        /// <returns>IDataReader</returns>
        public static IDataReader GetDataReaderBase(String connectionString, String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            using (SqlCommand cmd = new SqlCommand(commandText, conn))
            {
                // There're three command types: StoredProcedure, Text, TableDirect. The TableDirect 
                // type is only for OLE DB.  
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);

                conn.Open();
                // When using CommandBehavior.CloseConnection, the connection will be closed when the 
                // IDataReader is closed.
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return (IDataReader) reader;
            }
        }

        /// <summary>
        /// Set the connection, command, and then execute the command to get DataSet.
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSetBase(String connectionString, String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    // There're three command types: StoredProcedure, Text, TableDirect. The TableDirect 
                    // type is only for OLE DB.  
                    cmd.CommandType = commandType;
                    cmd.Parameters.AddRange(parameters);
                    conn.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataSet dataset = new DataSet();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataset);
                    return dataset;
                }
            }
        }

        #region Functions to excute SQL Query Statement with CommandType as Text

        public static Int32 ExecuteNonQuery(String connectionString, String commandText, params SqlParameter[] parameters)
        {
            return ExecuteNonQueryBase(connectionString, commandText, CommandType.Text, parameters);
        }

        public static object GetExecuteScalar(String connectionString, String commandText, params SqlParameter[] parameters)
        {
            return GetExecuteScalarBase(connectionString, commandText, CommandType.Text, parameters);
        }

        public static IDataReader GetDataReader(String connectionString, String commandText, params SqlParameter[] parameters)
        {
            return GetDataReaderBase(connectionString, commandText, CommandType.Text, parameters);
        }

        public static DataSet GetDataSet(String connectionString, String commandText, params SqlParameter[] parameters)
        {
            return GetDataSetBase(connectionString, commandText, CommandType.Text, parameters);
        }

        #endregion

        #region Functions to excute SQL Query Statement with CommandType as StoredProcedure

        public static Int32 ExecuteNonQueryStoredProcedure(String connectionString, String commandText, params SqlParameter[] parameters)
        {
            return ExecuteNonQueryBase(connectionString, commandText, CommandType.StoredProcedure, parameters);
        }

        public static object GetExecuteScalarStoredProcedure(String connectionString, String commandText, params SqlParameter[] parameters)
        {
            return GetExecuteScalarBase(connectionString, commandText, CommandType.StoredProcedure, parameters);
        }

        public static IDataReader GetDataReaderStoredProcedure(String connectionString, String commandText, params SqlParameter[] parameters)
        {
            return GetDataReaderBase(connectionString, commandText, CommandType.StoredProcedure, parameters);
        }

        public static DataSet GetDataSetStoredProcedure(String connectionString, String commandText, params SqlParameter[] parameters)
        {
            return GetDataSetBase(connectionString, commandText, CommandType.StoredProcedure, parameters);
        }
        #endregion
    }
}
