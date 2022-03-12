namespace Generic_Container
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;


    static class Services<T> where T : new()
    {
        public static string PathProject { get; } = @"C:\Programming\C# university\Generic Container\Generic Container\Generic Container\";

        public static PropertyInfo[] GetPropertiesClassService()
        {
            PropertyInfo[] propertyInfo;
            Type type = typeof(T);
            propertyInfo = type.GetProperties();
            return propertyInfo;
        }
    }
}