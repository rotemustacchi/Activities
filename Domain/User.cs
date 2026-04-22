using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class User : IdentityUser
    {
        public string? DisplayName { get; set; }
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
        //navigation property for activities the user is attending
        public ICollection<ActivityAttendee> Activities { get; set; } = [];
        public ICollection<Photo> Photos { get; set; } = [];
        public ICollection<UserFollowing> Followers { get; set; } = [];
        public ICollection<UserFollowing> Followings { get; set; } = [];
    }
}
