using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace PdfFormApp.Core
{
    /// <summary>
    /// BaseEntityController
    /// </summary>
    [Serializable]
    public static class BaseEntityController
    {
        #region FILL ENTITY FROM READER


        /// <summary>
        /// Generic method to fill an object from IDataReader
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="reader">IDataReader</param>
        /// <returns>Templated Object</returns>
        public static T FillEntity<T>(IDataReader reader) where T : class, new()
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            ObjectMapperCollection<T> objReader = null;

            T filledObject = null;
            try
            {
                #region Commented Code
                /*                using (var objReader = new ObjectMapperCollection<T>(reader))
                {
                    foreach (T obj in objReader)
                    {
                        filledObject = obj;
                    }
                }
 */
                #endregion
                objReader = new ObjectMapperCollection<T>(reader);

                foreach (T obj in objReader)
                {
                    filledObject = obj;
                }
            }
            catch
            {
                throw;
            }
            return filledObject;
        }


        /// <summary>
        /// Generic method to fill a list from DataReader
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="reader">IDataReader</param>
        /// <returns></returns>
        public static ICollection<T> FillEntities<T>(IDataReader reader) where T : class, new()
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            Collection<T> objectList = new Collection<T>();

            //try
            //{
            using (var objReader = new ObjectMapperCollection<T>(reader))
            {
                foreach (T obj in objReader)
                {
                    objectList.Add(obj);
                }
            }
            //}
            //catch
            //{
            //    throw;
            //}

            return objectList;
        }
        #endregion

        #region FILL ENTITY FROM DATASET
        /// <summary>
        /// Fill Entities From Table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static Collection<T> FillEntities<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            return FillEntities<T>(table.Rows);
        }
        /// <summary>
        /// Fill Entities From RowCollection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static Collection<T> FillEntities<T>(DataRowCollection rows)
        {
            Collection<T> list = null;

            if (rows != null)
            {
                list = new Collection<T>();

                foreach (DataRow row in rows)
                {
                    T item = FillEntity<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }
        /// <summary>
        /// Fill Entity From Row
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public static T FillEntity<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    object value;

                    try
                    {
                        value = row[column.ColumnName];

                        if (prop != null && value != null && value != DBNull.Value)
                        {
                            prop.SetValue(obj, value, null);
                        }
                    }
                    catch
                    {
                        // You can log something here
                        throw;
                    }
                }
            }

            return obj;
        }
        #endregion

    }

    #region HELPER CLASSES

    /// <summary>
    /// ObjectMapperCollection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    internal sealed class ObjectMapperCollection<T> : IEnumerable<T>, IEnumerable, IDisposable where T : class, new()
    {
        #region "Private Members/Methods"
        private Enumerator enumerator;
        //IDataReader reader;
        #endregion

        #region "Constructors"
        /// <summary>
        /// ObjectMapperCollection
        /// </summary>
        /// <param name="reader"></param>
        public ObjectMapperCollection(IDataReader reader)
        {
            //this.reader = reader;
            this.enumerator = new Enumerator(reader);
        }
        #endregion

        #region "IEnumerable<T> Implementations"
        public IEnumerator<T> GetEnumerator()
        {
            Enumerator e = this.enumerator;

            if (e == null)
            {
                throw new InvalidOperationException("Cannot enumerate more than once");
            }

            this.enumerator = null;
            return e;
        }
        #endregion

        #region "IEnumerable Implementation"
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        /// <summary>
        /// Enumerator
        /// </summary>
        [Serializable]
        class Enumerator : IEnumerator<T>, IEnumerator, IDisposable
        {
            #region "Private Members/Methods"
            private IDataReader reader;
            private PropertyInfo[] properties;
            private int[] propertyLookup;
            private T current;

            /// <summary>
            /// InitPropertyLookup
            /// </summary>
            private void InitPropertyLookup()
            {
                Dictionary<string, int> map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase); //Initialize the dictionary map that will contain string and int

                //Initialize dictionary with values >>
                for (int i = 0, n = this.reader.FieldCount; i < n; i++)
                {
                    map.Add(this.reader.GetName(i), i);
                }
                //Initialize dictionary with values <<

                this.propertyLookup = new int[this.properties.Length]; //array for property lookup

                //Iterate on property array and add values to array >>
                for (int i = 0, n = this.properties.Length; i < n; i++)
                {
                    int index = -1;

                    if (!map.TryGetValue(this.properties[i].Name, out index)) //Get value - try, if not exist, add -1
                    {
                        index = -1;
                    }
                    this.propertyLookup[i] = index;
                }
                //Iterate on property array and add values to array <<
            }
            #endregion

            #region "Constructors"
            internal Enumerator(IDataReader rdr)
            {
                this.reader = rdr;
                this.properties = typeof(T).GetProperties();
            }
            #endregion

            #region "IEnumerator<T>, IEnumerator Implementations"
            /// <summary>
            /// Current
            /// </summary>
            public T Current
            {
                get { return this.current; }
            }

            /// <summary>
            /// IEnumerator.Current
            /// </summary>
            object IEnumerator.Current
            {
                get { return this.current; }
            }

            /// <summary>
            /// MoveNext
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                if (this.reader.Read()) //Read from data reader
                {
                    if (this.propertyLookup == null) //Check if property lookup is null
                    {
                        this.InitPropertyLookup(); //Initialize the lookup
                    }
                    T instance = new T(); //Create instance

                    //Iterate on property array >>
                    for (int i = 0, n = this.properties.Length; i < n; i++)
                    {
                        int index = this.propertyLookup[i]; //Get index

                        if (index >= 0) //Check if index is valid
                        {
                            PropertyInfo pi = this.properties[i]; //Get property

                            if (pi.CanWrite) //Check if it is write-able
                            {
                                if (this.reader.IsDBNull(index)) //Check for null
                                {
                                    pi.SetValue(instance, null, null); //Set null value
                                }
                                else
                                {
                                    object val = this.reader.GetValue(index);

                                    if (pi.PropertyType.Name == "Boolean")
                                    {
                                        val = Convert.ToInt16(val) > 0;
                                    }

                                    if (pi.PropertyType.Name == "String")
                                    {
                                        val = Convert.ToString(val);
                                    }

                                    pi.SetValue(instance, val, null); //Set value for property
                                }
                            }
                        }
                    }
                    //Iterate on property array <<

                    this.current = instance; //set current to instance
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Reset
            /// </summary>
            public void Reset()
            {
            }
            #endregion

            #region "IDisposable Members"
            public void Dispose()
            {
                this.reader.Dispose();
            }
            #endregion
        }

        #region IDisposable Members
        public void Dispose()
        {
            if (this.enumerator != null)
            {
                this.enumerator.Dispose();
            }
        }
        #endregion
    }
    # endregion Helper Classes
}
