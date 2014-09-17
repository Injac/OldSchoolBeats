using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OldSchoolBeats.Universal.Services {
    class NavigationService:INavigationService {
        public void NavigateTo(string frameType) {


            Type t = Type.GetType("OldSchoolBeats.Universal."+frameType);

            Frame rootFrame = Window.Current.Content as Frame;

            rootFrame.Navigate(t);

        }
    }
}
