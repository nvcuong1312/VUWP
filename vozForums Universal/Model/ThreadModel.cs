using System;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;
using vozForums_Universal.CommonControl;

namespace vozForums_Universal.Model
{
    public class ThreadModel
    {
        EasClientDeviceInformation eas = new EasClientDeviceInformation();
        AppSettingModel setting = new AppSettingModel();
        public string UserName { get; set; }
        public string TypeMember { get; set; }
        public string JoinDate { get; set; }
        public string totalPosts { get; set; }
        public string location { get; set; }
        public string sign { get; set; }
        public string idUser { get; set; }
        public string Posts { get; set; }
        public string ImgAvatar { get; set; }
        public string PostCount { get; set; }
        public string Comment { get; set; }
        public string TitleTopic { get; set; }
        public string TimePost { get; set; }
        public string LinkImage { get; set; }
        public string htmlcontent { get; set; }
        public string idComment { get; set; }
        public string TextblockCreateUserColor { get; set; }
        public string TextblockMessageColor { get; set; }
        public string TextblockQuoteColor { get; set; }
        public string BackgroundTimeColor { get; set; }
        public string BackgroundPanelUserColor { get; set; }
        public string TextblockTimePostColor { get; set; }
        public string BackgroundThread { get; set; }
        public string enableQuote { get; set; }

        public string Message(string message)
        {
            string DeviceName = "";
            if (setting.DeviceName != null)
            {
                DeviceName = setting.DeviceName;
            }
            else DeviceName = eas.SystemProductName.ToString() + " ";

            string usingApp = string.Empty;
            if (LoginModel.checkPost == "Ok")
            {
                usingApp = "Sent from [B][I]" + DeviceName + "[/I][/B] " + " using [URL=\"https://www.microsoft.com/vi-vn/store/p/vozforums-universal/9nblggh438xd\"]Vozforums Universal[/URL]";
            }
            else
                usingApp = "Sent from [B][I]" + DeviceName + "[/I][/B] " + " using Vozforums Universal";

            message = DefineEmoticon.ConvertMessage(message);
            message = message + Environment.NewLine + Environment.NewLine + usingApp;           
            return message;
        }
    }

}
