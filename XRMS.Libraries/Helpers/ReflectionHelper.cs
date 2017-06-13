using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.ComponentModel;

namespace XRMS.Libraries.Helpers
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// Perform a deep copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static object CloneObject(object source)
        {
            object opTarget = null;
            try
            {
                //grab the type and create a new instance of that type
                Type opSourceType = source.GetType();
                opTarget = Activator.CreateInstance(opSourceType);

                //grab the properties
                PropertyInfo[] opPropertyInfo = opSourceType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                //iterate over the properties and if it has a 'set' method assign it from the source TO the target
                foreach (PropertyInfo item in opPropertyInfo)
                {
                    if (item.CanWrite)
                    {
                        //value types can simply be 'set'
                        if (item.PropertyType.IsValueType || item.PropertyType.IsEnum || item.PropertyType.Equals(typeof(System.String)))
                        {
                            item.SetValue(opTarget, item.GetValue(source, null), null);
                        }
                        //object/complex types need to recursively call this method until the end of the tree is reached
                        else
                        {
                            object opPropertyValue = item.GetValue(source, null);
                            if (opPropertyValue == null)
                            {
                                item.SetValue(opTarget, null, null);
                            }
                            else
                            {
                                item.SetValue(opTarget, CloneObject(opPropertyValue), null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return opTarget;
        }


        /// <summary>
        /// Perform a deep copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static void CopyObject(object source, object dest)
        {
            /*if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }*/

            try
            {
                //grab the type and create a new instance of that type
                Type opSourceType = source.GetType();
                //object opTarget = CreateInstanceOfType(opSourceType);

                //grab the properties
                PropertyInfo[] opPropertyInfo = opSourceType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                //iterate over the properties and if it has a 'set' method assign it from the source TO the target
                foreach (PropertyInfo item in opPropertyInfo)
                {
                    if (item.CanWrite)
                    {
                        //value types can simply be 'set'
                        if (item.PropertyType.IsValueType || item.PropertyType.IsEnum || item.PropertyType.Equals(typeof(System.String)))
                        {
                            item.SetValue(dest, item.GetValue(source, null), null);
                        }
                        //object/complex types need to recursively call this method until the end of the tree is reached
                        else
                        {
                            object opPropertyValue = item.GetValue(source, null);
                            if (opPropertyValue == null)
                            {
                                item.SetValue(dest, null, null);
                            }
                            else
                            {
                                CopyObject(opPropertyValue, item.GetValue(dest, null));
                                //item.SetValue(dest, CloneObject(opPropertyValue), null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return the new item
            //return opTarget;
        }

        public static bool CompareObject<T>(T object1, T object2)
        {
            //Get the type of the object
            Type type = typeof(T);

            //return false if any of the object is false
            if (object.Equals(object1, default(T)) || object.Equals(object2, default(T)))
                return false;

            //Loop through each properties inside class and get values for the property from both the objects and compare
            foreach (System.Reflection.PropertyInfo property in type.GetProperties())
            {
                if (property.Name != "ExtensionData")
                {
                    string Object1Value = string.Empty;
                    string Object2Value = string.Empty;
                    if (type.GetProperty(property.Name).GetValue(object1, null) != null)
                        Object1Value = type.GetProperty(property.Name).GetValue(object1, null).ToString();
                    if (type.GetProperty(property.Name).GetValue(object2, null) != null)
                        Object2Value = type.GetProperty(property.Name).GetValue(object2, null).ToString();
                    if (Object1Value.Trim() != Object2Value.Trim())
                    {
                        return false;
                    }
                }
                // recursive compare complext properties
                else
                {
                    /*bool value = type.GetMethod("CompareObject")
                                     .MakeGenericMethod(Type.GetType(property.Name))
                                     .Invoke(type.GetProperty(property.Name).GetValue(object1, null),
                                             type.GetProperty(property.Name).GetValue(object2, null));*/
                }
            }
            return true;
        }

        public static bool DeepCompareObject(object source, object dest)
        {
            /*if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }*/

            try
            {
                //grab the type and create a new instance of that type
                Type opSourceType = source.GetType();
                //object opTarget = CreateInstanceOfType(opSourceType);

                //grab the properties
                PropertyInfo[] opPropertyInfo = opSourceType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                //iterate over the properties and if it has a 'set' method assign it from the source TO the target
                foreach (PropertyInfo item in opPropertyInfo)
                {
                    if (item.CanWrite)
                    {
                        //value types can simply be 'set'
                        if (item.PropertyType.IsValueType || item.PropertyType.IsEnum || item.PropertyType.Equals(typeof(System.String)))
                        {
                            if (item.GetValue(source, null) != item.GetValue(dest, null))
                                return false;
                        }
                        //object/complex types need to recursively call this method until the end of the tree is reached
                        /*else
                        {
                            object sourcePropertyValue = item.GetValue(source, null);
                            object destPropertyValue = item.GetValue(dest, null);
                            if (sourcePropertyValue != null && destPropertyValue != null)
                            {
                                return DeepCompareObject(item.GetValue(source, null), item.GetValue(dest, null));
                            }
                            else if(sourcePropertyValue == null && destPropertyValue == null)
                            {
                                
                               
                            }
                            else
                            {
                                return false;
                            }
                            
                        }*/
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return the new item
            return true;
        }

        public static bool CompareEx(this object obj, object another)
        {
            if (ReferenceEquals(obj, another)) return true;
            if ((obj == null) || (another == null)) return false;
            if (obj.GetType() != another.GetType()) return false;

            //properties: int, double, DateTime, etc, not class
            if (!obj.GetType().IsClass) return obj.Equals(another);

            var result = true;
            foreach (var property in obj.GetType().GetProperties())
            {
                var objValue = property.GetValue(obj);
                var anotherValue = property.GetValue(another);
                //Recursion
               // if (!objValue.DeepCompare(anotherValue)) result = false;
            }
            return result;
        }

        //DataTable dt = YourList.ToDataTable();

        public static DataTable ToDataTable<T>(this List<T> iList)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);


                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            object[] values = new object[propertyDescriptorCollection.Count];
            foreach (T iListItem in iList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}
