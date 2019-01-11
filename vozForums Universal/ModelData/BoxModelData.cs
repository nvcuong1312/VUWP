using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vozForums_Universal.CommonControl;
using vozForums_Universal.Model;
using Windows.Storage;
using DataAccessLibrary;

namespace vozForums_Universal.ModelData
{
    public class BoxModelData
    {
        private List<BoxModel> homeModelsList { get; set; }

        public List<BoxModel> GetListBox()
        {
            return homeModelsList;
        }

        public void Add(BoxModel item)
        {
            DataAccess.AddDataBox(item.Id, item.NameParentBox, item.NameBox);
        }

        public void Delete(string IdBox)
        {
            DataAccess.DeleteDataBox(IdBox);
        }

        private void GetDataBox()
        {
            var DataBoxList = DataAccess.GetDataBox();

            foreach (var DataBox in DataBoxList)
            {
                var ItemInsert = new BoxModel()
                {
                    Id = DataBox.Key,
                    NameParentBox = DataBox.Value[0],
                    NameBox = DataBox.Value[1]
                };

                if (!homeModelsList.Contains(ItemInsert))
                {
                    homeModelsList.Add(ItemInsert);
                }
            }
        }

        private void Update()
        {
            try
            {
                //await FileIO.WriteLinesAsync(DataBox, BoxContent);
                DialogResult.DialogOnlyOk(Resource.DIALOG_DONE);
            }
            catch (Exception ex)
            {
                DialogResult.DialogOnlyOk(Resource.DIALOG_ERROR);
            }
        }

        public BoxModelData()
        {
            homeModelsList = new List<BoxModel>();
            homeModelsList.Add(new BoxModel() { NameParentBox = "Đại sảnh", NameBox = "Thông báo", Id = "2" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Đại sảnh", NameBox = "Thắc mắc & Góp ý", Id = "3" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Đại sảnh", NameBox = "Tin tức iNet", Id = "26" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Đại sảnh", NameBox = "Review sản phẩm", Id = "27" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Máy tính để bàn", NameBox = "Overclocking & Cooling", Id = "6" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Máy tính để bàn", NameBox = "Modding", Id = "151" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Máy tính để bàn", NameBox = "AMD", Id = "25" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Máy tính để bàn", NameBox = "Intel", Id = "24" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Máy tính để bàn", NameBox = "Mainboard & Memory", Id = "7" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Máy tính để bàn", NameBox = "Đồ họa máy tính", Id = "8" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Máy tính để bàn", NameBox = "Phần cứng chung", Id = "9" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Máy tính để bàn", NameBox = "miniPC", Id = "277" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Máy tính để bàn", NameBox = "Tự build máy chạy macOS", Id = "265" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Máy tính để bàn", NameBox = "Thiết bị ngoại vi & Phụ kiện", Id = "30" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Máy tính để bàn", NameBox = "Phần mềm", Id = "13" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Máy tính để bàn", NameBox = "Download", Id = "14" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Máy tính để bàn", NameBox = "Phát triển Phần mềm", Id = "148" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Máy tính để bàn", NameBox = "Trường đua", Id = "15" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Games", NameBox = "Thảo luận chung", Id = "11" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Games", NameBox = "DOTA2", Id = "245" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Games", NameBox = "League of Legends", Id = "253" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Games", NameBox = "Garena - Liên Quân Mobile", Id = "254" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Games", NameBox = "MMO -- Game Online", Id = "12" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Games", NameBox = "Pokemon GO", Id = "233" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Games", NameBox = "Crossfire Legends", Id = "281" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Games", NameBox = "Overwatch", Id = "237" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Games", NameBox = "Hearthstone", Id = "241" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Games", NameBox = "FPS", Id = "249" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Games", NameBox = "Liên Minh Huyền Thoại", Id = "178" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Sản phẩm công nghệ", NameBox = "Máy tính xách tay", Id = "47" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Sản phẩm công nghệ", NameBox = "Các sản phẩm Apple", Id = "108" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Sản phẩm công nghệ", NameBox = "Máy tính chuyên dụng", Id = "112" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Sản phẩm công nghệ", NameBox = "Thiết bị di động", Id = "32" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Sản phẩm công nghệ", NameBox = "Đồ điện tử & Thiết bị gia dụng", Id = "10" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Sản phẩm công nghệ", NameBox = "Multimedia", Id = "31" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Sản phẩm công nghệ", NameBox = "Mạng gia đình & Doanh nghiệp nhỏ", Id = "273" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Doanh nghiệp & Người dùng", NameBox = "DELL - Thông tin sản phẩm và dịch vụ chính hãng", Id = "213" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Doanh nghiệp & Người dùng", NameBox = "Hoanghamobile.com", Id = "222" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Doanh nghiệp & Người dùng", NameBox = "SaiBack.com - Cung cấp thiết bị lưu trữ chuyên nghiệp", Id = "257" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Doanh nghiệp & Người dùng", NameBox = "WD - Thương hiệu ổ cứng tin cây, dịch vụ hỗ trợ tốt nhất", Id = "224" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Doanh nghiệp & Người dùng", NameBox = "We can do", Id = "170" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Khu vui chơi giải trí", NameBox = "Chuyện trò linh tinh™", Id = "17" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Khu vui chơi giải trí", NameBox = "Điểm báo", Id = "33" });
            //homeModelsList.Add(new HomeModel() { NameBox = "Khu vui chơi giải trí", NameSubBox = "Các món ăn chơi", Id = "207" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Khu vui chơi giải trí", NameBox = "Superthreads", Id = "34" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Khu vui chơi giải trí", NameBox = "From f17 with Love", Id = "145" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Khu vui chơi giải trí", NameBox = "Offline", Id = "35" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Khu vui chơi giải trí", NameBox = "Ô tô & Xe máy", Id = "269" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Khu vui chơi giải trí", NameBox = "Chụp ảnh & Quay phim", Id = "288" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Khu vui chơi giải trí", NameBox = "Phim - Nhạc - Sách", Id = "286" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Khu vui chơi giải trí", NameBox = "Thể dục thể thao", Id = "287" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Khu thương mại", NameBox = "Máy tính để bàn", Id = "68" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Khu thương mại", NameBox = "Máy tính xách tay", Id = "72" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Khu thương mại", NameBox = "Điện thoại di động", Id = "76" });
            homeModelsList.Add(new BoxModel() { NameParentBox = "Khu thương mại", NameBox = "Các sản phẩm, dịch vụ khác", Id = "80" });

            GetDataBox();
        }
    }
}
