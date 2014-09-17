using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldSchoolBeats.Universal.ViewModel {





    /// <summary>
    /// Our client-class
    /// for Artists.
    /// </summary>
    public class BindableOldSchoolArtist:ObservableObject {

        private string artist;
        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        /// <value>
        /// The artist.
        /// </value>
        public string Artist {
            get {
                return artist;
            }

            set {
                artist = value;
                RaisePropertyChanged("Artist");
            }
        }


        string yearsArchive;

        /// <summary>
        /// Gets or sets the years archive.
        /// </summary>
        /// <value>
        /// The years archive.
        /// </value>
        public string YearsArchive {
            get {
                return yearsArchive;
            }

            set {
                yearsArchive = value;
                RaisePropertyChanged("YearsArchive");
            }
        }


        private string relatedStyles;

        /// <summary>
        /// Gets or sets the related styles.
        /// </summary>
        /// <value>
        /// The related styles.
        /// </value>
        public string RelatedStyles {
            get {
                return relatedStyles;
            }

            set {
                relatedStyles = value;
                RaisePropertyChanged("RelatedStyles");
            }
        }


        string imageUrl;

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>
        /// The image URL.
        /// </value>
        public string ImageUrl {
            get {
                return imageUrl;
            }

            set {
                this.imageUrl = value;
                RaisePropertyChanged("ImageUrl");
            }
        }

    }
}
