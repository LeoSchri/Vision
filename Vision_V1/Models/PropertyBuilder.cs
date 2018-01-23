using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static Vision_V1.Models.Types;

namespace Vision_V1.Models
{
    public class PropertyBuilder
    {
        public static List<PropInfo<T>> GetProps<T>(T item, List<int> propertySizes) where T:class
        {
            var propInfos = new List<PropInfo<T>>();
            var props = item.GetType().GetProperties();
            if(props != null && props.Any())
            {
                props.ToList().ForEach(prop =>
                {
                    if (prop.Name != "ID" && prop.Name != "LastModified" && IsOfPrimitiveType(prop.PropertyType))
                    {
                        var value = item.GetType().GetProperty(prop.Name).GetValue(item, null) != null ? item.GetType().GetProperty(prop.Name).GetValue(item, null).ToString() : " ";
                        propInfos.Add(new PropInfo<T>(prop.Name, value, prop.PropertyType,item));
                    }
                });
            }
            return propInfos;
        }

        public static List<PropInfo<T>> GetAllProps<T>(T item, List<int> propertySizes) where T : class
        {
            var propInfos = new List<PropInfo<T>>();
            var props = item.GetType().GetProperties();
            if (props != null && props.Any())
            {
                props.ToList().ForEach(prop =>
                {
                    if (prop.Name != "ID" && (IsOfPrimitiveType(prop.PropertyType) || IsNotList(prop.PropertyType)))
                    {
                        var value = item.GetType().GetProperty(prop.Name).GetValue(item, null) != null ? item.GetType().GetProperty(prop.Name).GetValue(item, null).ToString() : " ";
                        propInfos.Add(new PropInfo<T>(prop.Name, value, prop.PropertyType, item));
                    }
                });
            }
            return propInfos;
        }

        private static bool IsOfPrimitiveType(Type type)
        {
            if(type.IsPrimitive)
            {
                return true;
            }
            else
            {
                if(type == typeof(string))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsNotList(Type type)
        {
            if (type == typeof(IEnumerable))
                return false;
            else return true;
        }

    }
}