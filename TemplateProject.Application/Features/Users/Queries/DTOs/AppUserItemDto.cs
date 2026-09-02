using QrAssignment.Application.Common.DTOs;

namespace QrAssignment.Application.Features.Users.Queries.DTOs
{
    // Form/GetById DTO'su. AppRole tarafindaki RoleItemDto : BaseItemDto ile ayni desen.
    public class AppUserItemDto : BaseItemDto
    {
        public AppUserItemDto() { }

        public AppUserItemDto(Guid? id, string firstName, string lastName, string email, byte[] rowVersion, bool twoFactorEnabled)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            RowVersion = rowVersion;
            TwoFactorEnabled = twoFactorEnabled;
        }

        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string FullName => $"{FirstName} {LastName}".Trim();

        public bool TwoFactorEnabled { get; set; }
    }
}
