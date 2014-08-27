using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Mobile.Service;

namespace OldSchoolBeats.DataObjects {
    /// <summary>
    /// Our DTO class
    /// for Artists.
    /// </summary>
    public class OldSchoolArtist:EntityData {

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

    }
}