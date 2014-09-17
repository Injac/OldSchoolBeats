using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Xaml.Interactivity;
using OldSchoolBeats.Universal.Messaging;

namespace OldSchoolBeats.Universal.Behaviours {

    //This code is taken from
    //http://dontcodetired.com/blog/post/Telling-a-View-to-display-a-Message-Dialog-from-the-ViewModel-With-MVVMLight-in-Windows-81-Store-Apps.aspx#comment
    //Which is an excellent solution by Jason Roberts
    internal class DialogBehavior : DependencyObject, IBehavior {
        public DialogBehavior() {
            MessageText = "Are you sure?";
            YesText = "Yes";
            NoText = "No";
            CancelText = "Cancel";
        }


        public string Identifier {
            get;
            set;
        }
        public string MessageText {
            get;
            set;
        }

        public string TitleText {
            get;
            set;
        }

        public string YesText {
            get;
            set;
        }
        public string NoText {
            get;
            set;
        }
        public string CancelText {
            get;
            set;
        }


        public void Attach(DependencyObject associatedObject) {
            AssociatedObject = associatedObject;

            Messenger.Default.Register<ShowDialogMessage>(this, Identifier, ShowDialog);
        }

        public void Detach() {
            Messenger.Default.Unregister<ShowDialogMessage>(this, Identifier);
            AssociatedObject = null;
        }

        public DependencyObject AssociatedObject {
            get;
            private set;
        }

        private async void ShowDialog(ShowDialogMessage m) {
            var d = new MessageDialog(MessageText,TitleText);

            if (m.Yes != null) {
                d.Commands.Add(new UICommand(YesText, command => m.Yes.Execute(null)));
            }

            if (m.No != null) {
                d.Commands.Add(new UICommand(NoText, command => m.No.Execute(null)));
            }

            if (m.Cancel != null) {
                d.Commands.Add(new UICommand(CancelText, command => m.Cancel.Execute(null)));
            }

            if (m.Cancel != null) {
                d.CancelCommandIndex = (uint)d.Commands.Count - 1;
            }

            await d.ShowAsync();
        }
    }
}
