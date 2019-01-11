using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using vozForums_Universal.Model;
using Windows.UI.Popups;
using vozForums_Universal.Helper;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views.Account
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class Setting : Page
    {
        LoginModel getLogin = new LoginModel();
        AppSettingModel appSetting = new AppSettingModel();
        public Setting()
        {
            this.InitializeComponent();
            string iconcolor = appSetting.ThemeSetting;
        }
        string theme;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            theme = appSetting.ThemeSetting;
            if (theme == "Dark")
            {
                DarkTheme.IsChecked = true;
                LightTheme.IsChecked = false;           
            }
            else
            {
                DarkTheme.IsChecked = false;
                LightTheme.IsChecked = true;
            }

            if (appSetting.UserName != null && appSetting.PassWord != null)
            {
                txtUSERNAME.Text = appSetting.UserName;
                txtPASSWORD.Password = appSetting.PassWord;
            }

            if (appSetting.token == null) btn_logout.IsEnabled = false;

            if (appSetting.DeviceName != null)
            {
                tbDeviceName.Text = appSetting.DeviceName;
            }
        }
        
        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            Boolean save;
            progressring.IsActive = true;
            txtUSERNAME.IsEnabled = false;
            txtPASSWORD.IsEnabled = false;

            if (isSave.IsChecked == true)
            {
                save = true;
            }
            else save = false;

            getLogin.LoginMethod(txtUSERNAME.Text,txtPASSWORD.Password, save);
            try
            {
                if (appSetting.token.Length > 10)
                {
                    ShowMessage("Đăng nhập thành công");
                    btn_logout.IsEnabled = true;
                }
                else ShowMessage("Đăng nhập thất bại\nCheck lại tài khoản và mật khẩu.");
            }
            catch
            {
                ShowMessage("Đăng nhập thất bại\nCheck lại tài khoản và mật khẩu.");
            }
            progressring.IsActive = false;
            txtUSERNAME.IsEnabled = true;
            txtPASSWORD.IsEnabled = true;
        }
        private void SaveTheme_Click(object sender, RoutedEventArgs e)
        {            
            if (DarkTheme.IsChecked == true)
            {
                tbrs.Text = "DarkTheme IsChecked";
                appSetting.ThemeSetting = "Dark";
                askToExitApp();
            }            
                
            if (LightTheme.IsChecked == true)
            {             
                tbrs.Text = "LightTheme IsChecked";
                appSetting.ThemeSetting = "Light";
                askToExitApp();
            }
        }
        public async void ShowMessage(string msgB)
        {
            var msg = new MessageDialog(msgB);
            var cancelBtn = new UICommand("Đóng");
            msg.Commands.Add(cancelBtn);
            IUICommand result = await msg.ShowAsync();
        }
        private async void askToExitApp()
        {
            var msg = new MessageDialog("Khởi động lại ứng dụng để thay đổi chủ đề!");
            var okBtn = new UICommand("Ngay bay giờ");
            var cancelBtn = new UICommand("Để sau");
            msg.Commands.Add(okBtn);
            msg.Commands.Add(cancelBtn);
            IUICommand result = await msg.ShowAsync();

            if (result != null && result.Label == "Ngay bay giờ")
            {
                Application.Current.Exit();
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            string device = tbDeviceName.Text;
            if (device.Length < 4)
            {
                showInputCha("Min Length = 4");
            }
            else
            {
                appSetting.DeviceName = device;
                showInputCha("Done");
            }
        }
        private async void showInputCha(string str)
        {
            var msg = new MessageDialog(str);
            var okBnt = new UICommand("Ok");
            msg.Commands.Add(okBnt);
            IUICommand result = await msg.ShowAsync();
        }

        private void btn_logout_Click(object sender, RoutedEventArgs e)
        {
            appSetting.PassWord = null;
            appSetting.UserName = null;
            appSetting.token = null;
            showInputCha("Đăng xuất thành công");
            txtUSERNAME.Text = string.Empty;
            txtPASSWORD.Password = string.Empty;
            btn_logout.IsEnabled = false;
        }

        private void txtUSERNAME_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUSERNAME.Text)) btn_login.IsEnabled = false;
            else btn_login.IsEnabled = true;
        }
        

    }
}
