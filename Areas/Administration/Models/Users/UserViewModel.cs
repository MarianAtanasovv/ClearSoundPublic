namespace ClearSoundCompany.Areas.Administration.Models.Users
{
    public class UserViewModel
    {
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool CanYouLockIt { get; set; }
        public string Phone { get; set; }

        //2021-09-17 11:55:31.6423126 +00:00
        public string LockoutDate { get; set; }

        public string LockoutTime { get; set; }
        public int AccessFailedCount { get; set; }
    }
}