using QrAssignment.Application.Common.Excel;

namespace QrAssignment.Application.Features.Feedbacks.DTOs
{
    public class FeedbackListItemExcelDto
    {
        public FeedbackListItemExcelDto() { }

        public FeedbackListItemExcelDto(string code)
        {
            Code = code; 
        }

        [ExcelColumn("Excel.Title.Code", Order = 1)]
        public string Code { get; set; }
         
    }
}
