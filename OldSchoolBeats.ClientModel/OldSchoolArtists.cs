using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldSchoolBeats.ClientModel {





    /// <summary>
    /// Our client-class
    /// for Artists.
    /// </summary>
    public class OldSchoolArtist {


        public string Id {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        /// <value>
        /// The artist.
        /// </value>
        public string Artist {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the years archive.
        /// </summary>
        /// <value>
        /// The years archive.
        /// </value>
        public string YearsArchive {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the related styles.
        /// </summary>
        /// <value>
        /// The related styles.
        /// </value>
        public string RelatedStyles {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>
        /// The image URL.
        /// </value>
        public string ImageUrl {
            get;
            set;
        }

    }
}
