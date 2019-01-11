using DataAccessLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vozForums_Universal.CommonControl;
using vozForums_Universal.Model;

namespace vozForums_Universal.ModelData
{
    class BookmarkModelData
    {
        public List<BookmarkModel> GetDataBookmark()
        {
            List<BookmarkModel> ListBookmark = new List<BookmarkModel>();
            var DtBm = DataAccess.GetDataBookmark();
            foreach (var dt in DtBm)
            {
                ListBookmark.Add(new BookmarkModel()
                {
                    ID = dt.Key.Split('_')[0],
                    Name = dt.Value,
                    Page = dt.Key.Split('_')[1]
                });
            }

            return ListBookmark;
        }

        public void Add(string ID, string Title, string Page)
        {
            var Key = ID + "_" + Page;
            var xxx = DataAccess.GetDataBookmark();
            if (xxx.ContainsKey(Key))
            {
                DialogResult.DialogOnlyOk(Resource.DIALOG_CONTENT_EXIST);
            }
            else
            {
                DataAccess.AddDataBookmark(ID, Title, Page);
                DialogResult.DialogOnlyOk(Resource.DIALOG_DONE);
            }
        }

        public void Delete(string ID, string Page)
        {
            DataAccess.DeleteDataBookmark(ID, Page);
        }
    }
}
