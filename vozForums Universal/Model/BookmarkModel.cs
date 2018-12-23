using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vozForums_Universal.Model
{
    class BookmarkModel
    {
        private string _Name;
        public string Name
        {
            get
            {
                return _Name + " - Page" + Page;
            }

            set
            {
                _Name = value;
            }
        }

        public string ID { get; set; }  
        
        public string Page { get; set; }

        public string Content
        {
            get
            {
                return ID + "_" + Page;
            }
        }
    }
}
