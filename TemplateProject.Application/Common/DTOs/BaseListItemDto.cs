namespace QrAssignment.Application.Common.DTOs
{
    public class BaseListItemDto
    {
        public string ModifiedUserFullName { get; set; }
        public string CreatedUserFullName { get; set; }


        public DateTimeOffset? ModifiedDateTime { get; set; }
        public DateTimeOffset? CreatedDateTime { get; set; }


        public long RevNum { get; set; }
    }
}
