using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingWPApp.ListItemClass
{
    class MainMenuItem
    {
        public string Title
        {
            get;
            set;
        }

        public string Images
        {
            get;
            set;
        }

        public MainMenuItem(string title, string image)
            {
                this.Title = title;
                this.Images = image;
                
            }
    }
}
