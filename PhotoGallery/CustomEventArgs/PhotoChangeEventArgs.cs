using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGallery.CustomEventArgs
{
    public class PhotoChangeEventArgs : EventArgs
    {
        public bool Previous { get; set; }
        public bool Next { get; set; }
    }
}
