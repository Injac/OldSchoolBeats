using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Windows.Security.Credentials;
using System.Linq;
using Windows.UI.Popups;
using GalaSoft.MvvmLight.Messaging;
using OldSchoolBeats.Universal.Messaging;


namespace OldSchoolBeats.Universal.Services {
    public class WamsLoginService:ILoginService {
        public MobileServiceUser MobUser {
            get;
            set;
        }


        public WamsLoginService() {

        }

        public async Task<bool> Login() {

            // This sample uses the Facebook provider.
            var provider = "MicrosoftAccount";

            // Use the PasswordVault to securely store and access credentials.
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = null;

            var dialogMessage = new ShowDialogMessage() { };


            while (credential == null) {
                try {
                    // Try to get an existing credential from the vault.
                    credential = vault.FindAllByResource(provider).FirstOrDefault();
                }

                catch (Exception) {
                    // When there is no matching resource an error occurs, which we ignore.
                }

                if (credential != null) {
                    // Create a user from the stored credentials.
                    MobUser = new MobileServiceUser(credential.UserName);
                    credential.RetrievePassword();
                    MobUser.MobileServiceAuthenticationToken = credential.Password;

                    // Set the user from the stored credentials.
                    App.MobileService.CurrentUser = MobUser;


                }

                else {
                    try {
                        // Login with the identity provider.
                        MobUser = await App.MobileService
                                  .LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);

                        // Create and store the user credentials.
                        credential = new PasswordCredential(provider,
                                                            MobUser.UserId, MobUser.MobileServiceAuthenticationToken);
                        vault.Add(credential);
                    }

                    catch (MobileServiceInvalidOperationException ex) {

                        Messenger.Default.Send<ShowDialogMessage>(dialogMessage,MessagingIdentifiers.LOGIN_ERROR_MESSAGE);
                    }
                }

                this.MobUser = App.MobileService.CurrentUser;

                Messenger.Default.Send<ShowDialogMessage>(dialogMessage, MessagingIdentifiers.LOGIN_SUCCESS_MESSAGE);





            }

            return true;
        }

        public async Task<bool> LogOut() {

            return await Task.Run<bool>( ()=> {
                var provider = "MicrosoftAccount";

                PasswordVault vault = new PasswordVault();

                try {
                    // Try to get an existing credential from the vault.
                    var credential = vault.FindAllByResource(provider).FirstOrDefault();

                    if(credential != null) {
                        //We remove the credential here
                        vault.Remove(credential);

                        return true;
                    }

                    return false;
                }

                catch (Exception) {
                    return false;
                }
            });
        }


        public async Task<bool> UserLoggedIn() {
            return await Task.Run<bool>(() => {
                var provider = "MicrosoftAccount";

                PasswordVault vault = new PasswordVault();

                try {
                    // Try to get an existing credential from the vault.
                    var credential = vault.FindAllByResource(provider).FirstOrDefault();

                    if (credential != null) {

                        credential.RetrievePassword();

                        App.MobileService.CurrentUser = new MobileServiceUser(credential.UserName) {
                            MobileServiceAuthenticationToken = credential.Password
                        };

                        this.MobUser = App.MobileService.CurrentUser;

                        return true;
                    }

                    return false;
                }

                catch (Exception) {
                    return false;
                }
            });
        }
    }
}
