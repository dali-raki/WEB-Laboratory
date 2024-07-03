using Users.Domains.Users;

namespace GLAB.Domains.Models.Users
{
    public class User
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserStatus State { get; set; }
        public bool ConfirmedRegistration { get; set; }
        public UserProfile userProfile { get; set; }

        private User(string userId, string userName, UserStatus state, UserProfile userProfile)
        {
            UserId = userId;
            UserName = userName;
            Password = "1234";
            ConfirmedRegistration = false;
            State = state;
            this.userProfile = userProfile;
        }

        private User(string userId, bool confirmedRegistration, UserStatus state)
        {
            UserId = userId;
            ConfirmedRegistration = confirmedRegistration;
            State = state;
        }

        private User(string userId, string userName, UserStatus state, string password, UserProfile userProfile)
        {
            UserId = userId;
            UserName = userName;
            State = state;
            Password = password;
            ConfirmedRegistration = true;
            this.userProfile = userProfile;

        }

        public static User Create(string userId, string userName, UserStatus state, string password, UserProfile userProfile)
        {
            return new User(userId, userName, state, password, userProfile);
        }
        public static User Create(string userId, string userName, UserStatus state, UserProfile userProfile)
        {
            return new User(userId, userName, state, userProfile);
        }

        public static User Create(string userId, bool confirmedRegistration, UserStatus state)
        {
            return new User(userId, confirmedRegistration, state);
        }

        private void changeState(UserStatus state)
        {
            State = state;
        }

        public void Allow()
        {
            changeState(UserStatus.Allowed);
        }

        public void block()
        {
            changeState(UserStatus.Bloqued);
        }

        public void Delete()
        {
            changeState(UserStatus.Deleted);
        }
    }


}