using QrAssignment.Application.Common.DTOs;

namespace QrAssignment.Application.Features.Users.Queries.DTOs
{
    // Liste DTO'su. AppRole tarafindaki RoleListItemDto : BaseListItemDto ile ayni desen
    // (RevNum + audit kullanici adlari + tarihler base'den geliyor).
    public class AppUserListItemDto : BaseListItemDto
    {
        public AppUserListItemDto(
            Guid id,
            string firstName,
            string lastName,
            string fullName,
            string email,
            long revNum,
            string modifiedUserFullName,
            string createdUserFullName,
            DateTimeOffset? modifiedDateTime,
            DateTimeOffset createdDateTime)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            FullName = fullName;
            Email = email;
            RevNum = revNum;
            ModifiedUserFullName = modifiedUserFullName;
            CreatedUserFullName = createdUserFullName;
            ModifiedDateTime = modifiedDateTime;
            CreatedDateTime = createdDateTime;
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
