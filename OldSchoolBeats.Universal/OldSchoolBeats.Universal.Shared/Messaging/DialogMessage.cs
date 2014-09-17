using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;

namespace OldSchoolBeats.Universal.Messaging {

    //This code is taken from
    //http://dontcodetired.com/blog/post/Telling-a-View-to-display-a-Message-Dialog-from-the-ViewModel-With-MVVMLight-in-Windows-81-Store-Apps.aspx#comment
    //Which is an excellent solution by Jason Roberts
    public class ShowDialogMessage : MessageBase {
        public ICommand Yes {
            get;
            set;
        }
        public ICommand No {
            get;
            set;
        }
        public ICommand Cancel {
            get;
            set;
        }
    }


}
