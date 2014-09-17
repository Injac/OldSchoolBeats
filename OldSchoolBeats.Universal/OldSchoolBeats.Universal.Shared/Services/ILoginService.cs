using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace OldSchoolBeats.Universal.Services {
    public interface ILoginService {

        MobileServiceUser MobUser {
            get;
            set;
        }


        Task<bool> Login();

        Task<bool> LogOut();

        Task<bool> UserLoggedIn();
    }
}
