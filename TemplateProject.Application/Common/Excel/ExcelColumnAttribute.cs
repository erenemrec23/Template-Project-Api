namespace QrAssignment.Application.Common.Excel
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ExcelColumnAttribute : Attribute
    {
        public bool IncludeInSample { get; set; } = true;
        public int Order { get; set; }
        public string LocalizationKey { get; }
        public ExcelColumnAttribute(string localizationKey) => LocalizationKey = localizationKey;
    }
     
    public abstract class ExcelValidationAttributeBase : Attribute
    {
        public string? ErrorMessageKey { get; set; } 
        public string? ErrorMessage { get; set; }    
    }

    public sealed class ExcelRequiredAttribute : ExcelValidationAttributeBase { }

    public sealed class ExcelMaxLengthAttribute : ExcelValidationAttributeBase
    {
        public int Length { get; }
        public ExcelMaxLengthAttribute(int length) => Length = length;
    }

    public sealed class ExcelRangeAttribute : ExcelValidationAttributeBase
    {
        public double Min { get; set; } = double.MinValue;
        public double Max { get; set; } = double.MaxValue;
    }

    public sealed class ExcelRegexAttribute : ExcelValidationAttributeBase
    {
        public string Pattern { get; }
        public ExcelRegexAttribute(string pattern) => Pattern = pattern;
    }

    public sealed class ExcelUniqueInFileAttribute : ExcelValidationAttributeBase { }
}