using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using System.Linq;
using OldSchoolBeats.ClientModel;


namespace OldSchoolBeats.Universal.ValueConverters {
    class UserMessagesValueConverter:IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {


            var userName = value as string;

            userName = userName.Split(':')[1];

            var locator = App.Current.Resources["Locator"] as ViewModel.ViewModelLocator;

            var data = locator.Main.SignalrMessages.Where(m => m.ToUser.Equals(userName)).ToList<SignalRMessage>();

            return data;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
