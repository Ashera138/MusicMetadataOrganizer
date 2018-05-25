using MusicMetadataUpdater_v2._0;
using System;
using System.Reflection;

namespace MetadataFileTests
{
    internal static class HelperMethodsForTesting
    {
        internal static bool PropertiesAreSetToDefaultValues(object obj)
        {
            bool propsAreSetToDefault = true;
            object defaultReferenceTypeValue = null;

            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                Type typeOfProp = property.PropertyType;
                object valueOfProp = property.GetValue(obj);
                if (typeOfProp.IsValueType)
                {
                    if (!Activator.CreateInstance(typeOfProp).Equals(valueOfProp))
                        propsAreSetToDefault = false;
                }
                else if (valueOfProp != defaultReferenceTypeValue)
                    propsAreSetToDefault = false;
            }
            return propsAreSetToDefault;
        }

        internal static bool HasANullablePropertyWithNullValue(object obj)
        {
            bool nullablePropValueIsNull = false;

            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                var propertyType = property.PropertyType;
                if (propertyType.IsNullableType() == true)
                {
                    if (property.GetValue(obj) == null)
                        nullablePropValueIsNull = true;
                }
            }

            return nullablePropValueIsNull;
        }
    }
}
