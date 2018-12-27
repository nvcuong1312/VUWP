using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vozForums_Universal
{
    public class Resource
    {
        // For All
        public const string STR_EMPTY = "";
        public const string STR_SPACE = " ";
        public const string STR_LAST_PAGE = "Last page";
        public const string STR_DELETE_MSG = "Delete";
        public const string STR_CONTENT_EXIST = "Bookmark already exist";
        public const string STR_DONE = "Done";
        public const string STR_ERROR = "Error";
        public const string STR_ERROR_LOGIN = "Login Error";
        public const string STR_LOGIN_SUCCESS = "Login Success";
        public const string STR_TIME_FORMAT = "yyyy-MM-dd HH-mm-ss";
        public const string STR_2C = "%2C";
        public const int ID_BOOKMARK = -1;
        public const int ID_SETTING = -2;
        public const int ID_SUPPORT = -3;
        public const int ID_ACCOUNT = -4;
        public const int ID_MESSAGE = -5;
        public const int ID_OUT = -6;
        public const int ID_BOX_START_DEFAULT = 26;
        public const string STR_VISIT = "Khách";
        public const string STR_LOGGING = "Logging...";
        public const string STR_IDBOX_EXIST = "Box is exist!";

        // For AppSetting
        public const string STR_DEVICE_NAME = "Devicename";
        public const string STR_USER_NAME = "UserName";
        public const string STR_PASSWORD = "PassWord";
        public const string STR_TOKEN = "Token";
        public const string STR_USER_ID = "UserId";
        public const string STR_IS_ENABLE_POST_LINK = "IsEnablePostLink";
        public const string STR_SAVE_STATE = "SaveState";
        public const string STR_THEME_SETTING = "ThemeSetting";
        public const string STR_TIME_LOGIN = "TimeLogin";
        public const string STR_TOTAL_POSTS = "TotalPosts";
        public const string TRUE = "True";
        public const string FALSE = "False";
        public const string OK = "OK";
        public const string NOK = "NOK";
        public const string STR_GUEST = "Guest";
        public const string STR_DEFAULT_NAME_DEVICE = " Windows Device ";
        public const string STR_SPLIT_DATA_BOX = "{xx}";
        public const string STR_BOX_START = "BoxStart";

        // ListThread
        public const int MAX_LENGTH = 70;
        public const string STR_CREATOR = "Người tạo:  ";
        public const string STR_LAST_POST = "Lastpost: ";
        public const string URL_NEW_THREAD = "https://forums.voz.vn/newthread.php?do=newthread&f=";
        public const string URL_LIST_THREAD = "https://forums.voz.vn/forumdisplay.php?f={rpIDBox}&daysprune=-1&order=desc&sort=lastpost&pp=20&page={rpIDPage}";

        // Thread
        public const string STR_VB_MSG = "vBulletin Message";
        public const string STR_REPLY_WITH_QUOTE = "Reply With Quote";
        public const string STR_MULTI_QUOTE = "Multi-Quote This Message";
        public const string STR_QUICK_REPLY = "Quick reply to this message";
        public const string URL_THREAD = "https://forums.voz.vn/showthread.php?t={rpIdThread}&page={rpIDPage}";
        public const string URL_GET_CONTENT_MESSAGE = "https://forums.voz.vn/newreply.php?do=newreply&p={rpID}";
        public const string URL_POST_COMMENT = "https://forums.voz.vn/newreply.php?do=postreply&t={rpID}";

        // ListMessage
        public const string STR_INBOX = "Inbox";
        public const string STR_OUTBOX = "Outbox";
        public const string URL_MESSAGE_IN = "https://forums.voz.vn/private.php";
        public const string URL_MESSAGE_OUT = "https://forums.voz.vn/private.php?s=&pp=50&folderid=-1";

        // Message
        public const string STR_NEW_MESSAGE = "New message";
        public const string URL_MESSAGE = "https://forums.voz.vn/private.php?do=showpm&pmid={rpID}";
        public const string URL_POST_MESSAGE = "https://forums.voz.vn/private.php?do=insertpm&pmid={rpID}";
        public const string URL_DELETE_MESSAGE = "https://forums.voz.vn/private.php?do=managepm&dowhat=delete&pmid={rpID}";

        public const string STR_POST_MESSAGE_DONE = "Sent message done!";

        // Account
        public const string URL_ACCOUNT = "https://forums.voz.vn/member.php?u={rpID}";

        // HttpClient
        public const string USER_AGENT = "User-Agent";
        public const string USER_AGENT_VALUE = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36";
        public const string COOKIES_VFPASSWORD = "vfpassword";
        public const string COOKIES_VFUSERID = "vfuserid";
        public const string COOKIES_VBULLETIN_MULTIQUOTE = "vbulletin_multiquote";
        public const string APPLICATION = "application/x-www-form-urlencoded";
        public const string URL_HOMEPAGE = "https://forums.voz.vn/";

        // Display Popup
        public const string STR_ERROR_NETWORK = "Lỗi mạng: \n * Kiểm tra lại kết nối, DNS \n * Có thể lỗi máy chủ. \n Vui lòng kiểm tra và tải lại trang";
        public const string STR_ERROR_INPUT = "Giá trị nhập không đúng format!";
        public const string STR_THREAD_DELETED = "Mod/Min del bài rồi. :'<";

        // Define Value
        public const int SIZE_WIDTH_SCREEN_600 = 600;
        public const int SIZE_WIDTH_SCREEN_700 = 800;
        public const int TOTAL_POST_MESSAGE = 15;
        public const int TOTAL_POST_URL_IMAGE = 20;        
    }
}