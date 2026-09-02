using System;

namespace QrAssignment.Application.Attributes
{

    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnDisplayAttribute : Attribute
    {
        public int Order { get; }

        public ColumnDisplayAttribute(int order)
        {
            Order = order;
        }
    }

}
