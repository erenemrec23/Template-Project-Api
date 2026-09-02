using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FilterableAttribute : Attribute
    {
    }
}
