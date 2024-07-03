using System.ComponentModel;


namespace Users.Domains.Users
{
    public enum UserProfile
    {
        [Description("Laboratory mangaers")]
        MainManager = 1,

        [Description("Laboratory admins")]
        LabADmin = 2,

        [Description("Laboratory members")]
        LabMember = 3
    }
}