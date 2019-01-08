

namespace vozForums_Universal.Model
{
    public class ListThreadModel
    {
        public string ThreadName { get; set; }
        public string ThreadId { get; set; }
        public string ThreadCreate { get; set; }
        public string DayLastPost { get; set; }
        public string TimeLastPost { get; set; }
        public string UserLastPost { get; set; }
        public string ExtraTitle { get; set; }
        public string CountReply { get; set; }
        public string CountViews { get; set; }
        public string Rating { get; set; }
        public bool IsReaded { get; set; }
        public bool Stick { get; set; }
        public string HeightStick { get; set; }
        public string WidthStick { get; set; }
        public string ForegroundStick { get; set; }
        public string MarginStick { get; set; }
        public string BackgroundHomeColor { get; set; }
        public string TextblockExtraThreadColor { get; set; }
        public string TextblockCreateUserColor { get; set; }
    }
}
