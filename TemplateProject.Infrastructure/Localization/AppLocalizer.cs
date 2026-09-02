using Microsoft.Extensions.Localization;
using QrAssignment.Application;
using QrAssignment.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Infrastructure.Localization
{
    public class AppLocalizer : IAppLocalizer
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AppLocalizer(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public string this[string key] => _localizer[key];

        public string GetString(string key, params object[] arguments) => _localizer[key, arguments];
    }
}
