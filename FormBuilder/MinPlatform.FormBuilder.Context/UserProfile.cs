namespace MinPlatform.FormBuilder.Context
{
    using System.Collections.Generic;
    using System.Security.Claims;

    public sealed class UserProfile : IUserProfile
    {
        public string Id
        {
            get;
            set;
        }

        public ClaimsPrincipal UserInfo 
        { 
            get; 
            set;
        }
    }
}
