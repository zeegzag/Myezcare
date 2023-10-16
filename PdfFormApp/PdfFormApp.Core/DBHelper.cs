using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfFormApp.Core
{
    public class DBHelper
    {
        #region "Private Members"

        private readonly string _connectionString = string.Empty;
        private readonly Database _dataBase = null;

        private DbCommand _dbCommand = null;

        #endregion

        #region "Public Fields"

        public Database DataBase { get { return _dataBase; } }

        public DbCommand DBCommand { get { return _dbCommand; } }


        #endregion

        #region "Constructor"

        public DBHelper(string connectionString)
        {
            _dataBase = new SqlDatabase(connectionString);
            _connectionString = connectionString;
        }

        public DBHelper()
        {
            _dataBase = new SqlDatabase(ApplicationSettings.ConnectionString);
        }

        #endregion

        #region "ExecuteNonQuery"




        public object GetParameterValue(string pParam)
        {
            return this._dataBase.GetParameterValue(this.DBCommand, pParam);
        }
        private DbCommand PrepareCommand(DbConnection connection, string pCommandText, Parameter[] pParameters)
        {
            _dbCommand = this.DataBase.GetStoredProcCommand(pCommandText);
            _dbCommand.Parameters.Clear(); // Issue: The SqlParameter is already contained by another SqlParameterCollection
            _dbCommand.Connection = connection;
            _dbCommand.CommandText = pCommandText;
            _dbCommand.CommandType = CommandType.StoredProcedure;
            _dbCommand.CommandTimeout = 0;


            if (pParameters != null)
            {
                pParameters.Where(p => p.Direction == ParameterDirection.Input).ToList().ForEach(p => { this.DataBase.AddInParameter(_dbCommand, p.Name, p.DBType, p.Value); });
                pParameters.Where(p => p.Direction == ParameterDirection.Output).ToList().ForEach(p => { this.DataBase.AddOutParameter(_dbCommand, p.Name, p.DBType, int.MaxValue); });
            }
            return _dbCommand;
        }
        public DbConnection OpenConnection()
        {
            DbConnection connection = this.DataBase.CreateConnection();
            connection.Open();
            return connection;
        }

        public void CloseConnection(DbConnection connection)
        {
            if ((connection != null) && (connection.State == ConnectionState.Open))
            {
                connection.Close();
            }
        }
        public int ExecuteNonQuery(string pCommandText, Parameter[] pParameters)
        {
            DbConnection connection = null;
            try
            {
                connection = OpenConnection();
                PrepareCommand(connection, pCommandText, pParameters);
                return DBCommand.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (DBCommand != null)
                    DBCommand.Dispose();
                CloseConnection(connection);
            }
        }

        #endregion

        #region "Data Reader Methods"

        /// <summary>
        /// Parameterless reader
        /// </summary>
        /// <param name="pCommandText"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string pCommandText)
        {
            object[] pParameters = { };
            return this.DataBase.ExecuteReader(pCommandText, pParameters);
        }

        /// <summary>
        /// This function is used to fetch data using Data Reader	
        /// </summary>
        /// <param name="pCommandText">Command Text</param>
        /// <param name="pParameters">Parameters Collection</param>
        /// <returns>Data Reader</returns>
        public IDataReader ExecuteReader(string pCommandText, Parameter[] pParameters)
        {
            try
            {
                var cmd = this.DataBase.GetStoredProcCommand(pCommandText);
                cmd.CommandTimeout = 0;

                if (pParameters != null)
                {
                    pParameters.ToList().ForEach(p => { this.DataBase.AddInParameter(cmd, p.Name, p.DBType, p.Value); });
                }
                var dataReader = this.DataBase.ExecuteReader(cmd);
                return dataReader;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns list of Entities without parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pCommandText"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteSprocAccessor<T>(string pCommandText) where T : class, new()
        {
            Parameter[] pParameters = { };
            var result = this.ExecuteSprocAccessor<T>(pCommandText, pParameters);
            return result.ToList();
        }

        /// <summary>
        /// This method will return List of Entities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pCommandText"></param>
        /// <param name="pParameters"></param>
        /// <returns></returns>
        public ICollection<T> ExecuteSprocAccessor<T>(string pCommandText, Parameter[] pParameters) where T : class, new()
        {
            var dataReader = ExecuteReader(pCommandText, pParameters);
            ICollection<T> list = BaseEntityController.FillEntities<T>(dataReader);
            return list;
        }


        
        #endregion

        #region EXECUTE SCALAR METHODS

        /// <summary>
        /// This function is used to invoke Execute Scalar Method
        /// </summary>
        /// <param name="pCommandText">Comand Text</param>
        /// <param name="pParameters">Parameters Collection</param>       
        /// <returns></returns>
        public object ExecuteScalar(string pCommandText, Parameter[] pParameters)
        {
            try
            {
                object val = null;
                using (var cmd = this.DataBase.GetStoredProcCommand(pCommandText))
                {
                    cmd.CommandTimeout = 0;

                    cmd.Connection = this.DataBase.CreateConnection();

                    cmd.Connection.Open();

                    if (pParameters != null)
                    {
                        pParameters.ToList().ForEach(p => { this.DataBase.AddInParameter(cmd, p.Name, p.DBType, p.Value); });
                    }

                    val = cmd.ExecuteScalar();
                    cmd.Connection.Close();
                }

                return val;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region "Single Result"
        public T GetEntity<T>(string pCommandText, Parameter[] pParameters) where T : class, new()
        {
            var dataReader = ExecuteReader(pCommandText, pParameters);
            return BaseEntityController.FillEntity<T>(dataReader);
        }
        #endregion
        

        #region "Helper Methods"
        private List<DataTable> GetMultipleTables(string pCommandText, Parameter[] pParameters)
        {
            var dbHelper = new DBHelper();
            var reader = dbHelper.ExecuteReader(pCommandText, pParameters);
            var tables = new List<DataTable>();

            while (!reader.IsClosed)
            {
                while (!reader.IsClosed)
                {
                    var table = new DataTable();
                    table.Load(reader);
                    tables.Add(table);
                }
                if (!reader.IsClosed)
                {
                    reader.NextResult();
                }
            }

            return tables;
        }
        #endregion

        #region DataSet only for Deesktop app
        public DataSet GetDataSet(string pCommandText)
        {
            var dbCommand = this.DataBase.GetStoredProcCommand(pCommandText);
            return this.DataBase.ExecuteDataSet(dbCommand);
        }
        #endregion
    }
}
