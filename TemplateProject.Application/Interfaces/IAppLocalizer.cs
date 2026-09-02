using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Interfaces
{
    public interface IAppLocalizer
    {
        string this[string key] { get; }
        string GetString(string key, params object[] arguments);
    }
}
