using System.Collections.Generic;
using HRMS.Models;

namespace HRMS.ViewModels.Account
{
    public class UserProfileInformation<T>
    {
        public ApplicationUser applicationUser { get; }
        public IList<T> result { get; }

        public UserProfileInformation(ApplicationUser applicationUser, IList<T> result)
        {
            this.applicationUser = applicationUser;
            this.result = result;
        }
    }
}
