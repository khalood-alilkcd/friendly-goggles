﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using System.Dynamic;
using System.Reflection;

namespace Services.DataShaping
{
    public class DataShaper<T> : IDataShaper<T> where T : class
    {
        public PropertyInfo[] Properties { get; set; }

        public DataShaper()
        {
            Properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }

        public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fieldsString)
        {
            /// to parse the input string that contains the fields we want to fetch
            var requiredProperties = GetRequiredProperties(fieldsString);
            return FetchData(entities, requiredProperties);
        }

        public ExpandoObject ShapeData(T entity, string fieldsString)
        {
            var requiredProperties = GetRequiredProperties(fieldsString);
            return FetchDataForEntity(entity, requiredProperties);
        }

        /// it parses the input string and return just the properties we need to return to the controller
        private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldsString) 
        {
            var requiredProperies = new List<PropertyInfo>();
            if(!string.IsNullOrWhiteSpace(fieldsString))
            {
                var fields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach(var field in fields)
                {
                    var property = Properties.FirstOrDefault(pi => 
                        pi.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));
                    if (property == null)
                        continue;
                    requiredProperies.Add(property);
                }            
            }
            else
            {
                requiredProperies = Properties.ToList();
            }
            return requiredProperies;
        }

        private IEnumerable<ExpandoObject> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedData = new List<ExpandoObject>();
            foreach (var entity in entities)
            {
                var shapedObject = FetchDataForEntity(entity, requiredProperties);
                shapedData.Add(shapedObject);
            }
            return shapedData;
        }
        
        private ShapedEntity FetchDataForEntity (T entity, IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedObject = new ShapedEntity();
            
            foreach(var property in requiredProperties)
            {
                var objectPropertyValue = property.GetValue(entity);
                /// ExpandoObject that emplements IDictionary<string, object>
                shapedObject.Entity.TryAdd(property.Name, objectPropertyValue);
            }

            var objectProperty = entity.GetType().GetProperty("Id"); 
            shapedObject.Id = (Guid)objectProperty.GetValue(entity);

            return shapedObject;
        }
    }
}