using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using WebSocketSharp;

namespace HduRemoteLab
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public WebSocket ws;
        public String server= "localhost:80";
        public Account account;
        public PwdWindow pwdWindow;
        public BackMes backMes;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    GridBasic.Visibility = Visibility.Visible;
                    GridHigh.Visibility = Visibility.Hidden;
                }));
                AppendLog("欢迎进入创新实践基础教学平台！");
                //默认账户密码
                if (File.Exists("./config.json"))
                {
                    var content = File.ReadAllText("./config.json");
                    var config = JsonConvert.DeserializeObject<IdPwd>(content);
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        BtnLogin.Content = "登录";
                        TextID.Text = config.id;
                        PwdBoxPwd.Password = config.password;
                    }));
                    
                }
            }
            catch(Exception e)
            {
                AppendLog(e.Message);
            }
            
        }

        private void AppendLog(string text)
        {
            var len = 30;
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                ListLog.Items.Add("[" + DateTime.Now.ToString() + "] ");
                string temp;
                if(text.Length>len)
                    for(int i=0;text.Length>len;i++)
                    { 
                        temp = text.Substring(0, len);
                        ListLog.Items.Add(temp);
                        text = text.Substring(len);
                    }
                ListLog.Items.Add(text);
                ListLog.SelectedIndex = ListLog.Items.Count - 1;
                ListLog.ScrollIntoView(ListLog.SelectedItem);
            }));
        }


        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        { 
            var mode = "login";
            var loginMes = new IdPwd
            {
                id = TextID.Text,
                password = PwdBoxPwd.Password
            };
            var data = JsonConvert.SerializeObject(loginMes);
            ws = new WebSocket("ws://" + server + "/mode=" + mode);
            ws.OnMessage += ( s, ee)=>{ 
                var mes = JsonConvert.DeserializeObject<MesData>(ee.Data);
                if (mes.code == "100")
                {
                    account = JsonConvert.DeserializeObject<Account>(mes.mes.ToString());
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                     {
                         GridBasic.Visibility = Visibility.Hidden;
                         GridHigh.Visibility = Visibility.Visible;
                         TextAccount.Text = "当前账户：" + account.name + "(" + account.role + ")";
                         if (account.role != "teacher")
                             BtnLogView.IsEnabled = false;
                     }));
                    File.WriteAllText("./config.json", data);
                    AppendLog("登录成功。当前登录账户：" + account.name);
                }
                else
                { 
                    backMes = JsonConvert.DeserializeObject<BackMes>(mes.mes.ToString());
                    AppendLog("抱歉，登录错误。错误代码：" + mes.code + ",错误信息：" + backMes.message);
                }
            };
            ws.OnClose += (s, ee) => {
                AppendLog("服务器通讯结束!");
            };
            ws.Connect();
            ws.Send(data);
        }


        private void BtnPwd_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                GridHigh.IsEnabled = false;
            }));
            pwdWindow = new PwdWindow();
            pwdWindow.Topmost = true;
            pwdWindow.Show();
            pwdWindow.passNewPwd += ModifyPwd;
        }

        private void ModifyPwd(string newPwd)
        {
            var mode = "modify";

            var modifyMes = new IdNewpwd
            {
                id = account.id,
                new_password = newPwd
            };

            var data = JsonConvert.SerializeObject(modifyMes);

            ws = new WebSocket("ws://" + server + "/mode=" + mode);
            ws.OnMessage += (s, ee) => { 
                var mes = JsonConvert.DeserializeObject<MesData>(ee.Data);
                if (mes.code == "104")
                {
                    backMes = JsonConvert.DeserializeObject<BackMes>(mes.mes.ToString());
                    AppendLog(backMes.message);
                }
                else
                {
                    backMes = JsonConvert.DeserializeObject<BackMes>(mes.mes.ToString());
                    AppendLog("抱歉，修改密码出错。错误代码：" + mes.code + ",错误信息：" + backMes.message);
                }
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    GridHigh.IsEnabled = true;
                 
                }));
            };
            ws.OnClose += (s, ee) => {
                AppendLog("服务器通讯结束!");
            };
            ws.Connect();
            ws.Send(data);
        }


        private void BtnAccount_Click(object sender, RoutedEventArgs e)
        {
            account = new Account();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                GridHigh.Visibility = Visibility.Hidden;
                GridBasic.Visibility = Visibility.Visible;
            }));
        }

        private void BtnLogView_Click(object sender, RoutedEventArgs e)
        {
          //ToDo
        }

        private void BtnOperate_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
