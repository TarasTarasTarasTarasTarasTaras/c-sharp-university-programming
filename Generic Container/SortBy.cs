namespace Generic_Container
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    class SortBy<T> : IComparer<T>
    {
        private PropertyInfo _propertyInfo;
        public SortBy(PropertyInfo property)
        {
            _propertyInfo = property;
        }

        public int Compare([AllowNull] T x, [AllowNull] T y)
        {
            return String.Compare(_propertyInfo.GetValue(x).ToString(), _propertyInfo.GetValue(y).ToString());
        }
    }
}