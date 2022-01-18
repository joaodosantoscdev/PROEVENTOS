using Microsoft.AspNetCore.Identity;
using ProEventos.Domain.Enum;
using System.Collections.Generic;

namespace ProEventos.Domain.Identity
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Title Title { get; set; }
        public string Description { get; set; }
        public Function Function { get; set; }
        public string ImageURL { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}
