using System;

using XRMS.Business.Models;

namespace XRMS.Presentation
{
    public class GlobalObjects
    {
        private static DateTime _currentDateTime;
        private static Restaurant _restaurantInfo;

        private static User _systemUser;

        public static User SystemUser
        {
            get
            {
                if (_systemUser == null)
                {
                    _systemUser = new User { Id = 1, Name = "admin", Fullname = "Admin" };
                }
                return _systemUser;
            }
            set { _systemUser = value; }
        }

        public static DateTime CurrentDateTime
        {
            get { return _currentDateTime; }
            set { _currentDateTime = value; }
        }

        public static Restaurant RestaurantInfo
        {
            get { return _restaurantInfo; }
            set { _restaurantInfo = value; }
        }
    }
}
