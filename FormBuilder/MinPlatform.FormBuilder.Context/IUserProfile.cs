namespace MinPlatform.FormBuilder.Context
{
    using System.Security.Claims;

    public interface IUserProfile
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
