using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfFormApp.Core
{
    public class Parameter
    {
        #region MEMBERS

        /// <summary>
        /// Gets or sets the maximum size, in bytes, of the data within the column.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Parameter Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Parameter Value
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// Parameter Direction
        /// </summary>
        public ParameterDirection Direction { get; set; }

        public DbType DBType { get; set; }
        #endregion

        #region CONSTRUCTOR
        //Create init method

        /// <summary>
        /// Default constructor used for serialization
        /// </summary>
        public Parameter()
        {

        }
        /// <summary>
        /// Overloaded Constructor with Parameter Direction and field size
        /// </summary>
        /// <param name="pName"> Parameter Name</param>
        /// <param name="pValue">Parameter Value</param>
        /// <param name="pDirection">Parameter Direction</param>
        /// <param name="size">Field size</param>
        //public Parameter(string pName, object pValue, ParameterDirection pDirection, int size)
        //{
        //    Init(pName, pValue, pDirection);
        //    this.Size = size;
        //}

        /// <summary>
        /// Overloaded Constructor with Parameter Direction
        /// </summary>
        /// <param name="pName"> Parameter Name</param>
        /// <param name="pValue">Parameter Value</param>
        /// <param name="pDirection">Parameter Direction</param>
        public Parameter(string pName, object pValue, ParameterDirection pDirection)
        {
            Init(pName, pValue, pDirection);
        }

        public Parameter(string pName, object pValue, ParameterDirection pDirection, DbType dbType)
        {
            Init(pName, pValue, pDirection, dbType);
        }

        /// <summary>
        /// Overloaded Constructor with out paramater direction
        /// </summary>
        /// <param name="pName"> Parameter Name</param>
        /// <param name="pValue">Parameter Value</param>
        /// <param name="pDirection">Parameter Direction</param>
        public Parameter(string pName, object pValue)
        {

            Init(pName, pValue, ParameterDirection.Input);
        }

        /// <summary>
        ///  Intializig the constructor
        /// </summary>
        private void Init(string pName, object pValue, ParameterDirection pDirection)
        {
            Name = pName;
            Value = pValue;
            Direction = pDirection;
            DBType = DbType.String;
        }
        private void Init(string pName, object pValue, ParameterDirection pDirection, DbType dbType)
        {
            Name = pName;
            Value = pValue;
            DBType = dbType;
            Direction = pDirection;
        }
        #endregion
    }
}
