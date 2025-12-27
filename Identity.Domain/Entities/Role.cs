using Identity.Domain.Common;
using Identity.Domain.Enums;

namespace Identity.Domain.Entities;

public class Role : BaseEntity
{
    public UserRole Name { get; private set; }

    private Role() { } // EF safe

    public Role(UserRole role)
    {
        Name = role;
    }
}
