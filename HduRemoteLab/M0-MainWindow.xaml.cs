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
        //公共信息定义
        public PwdWindow pwdWindow;
        public OperateWindow operateWindow;
        public WebSocket ws;
        public String server= "localhost:80";
        public Account account;
        //主窗口
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                //更新窗口，基础显示，高级隐藏
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    GridBasic.Visibility = Visibility.Visible;
                    GridHigh.Visibility = Visibility.Hidden;
                }));
                //初始消息
                AppendLog("欢迎进入创新实践基础教学平台！");
                //读取已存的账户密码数据
                if (File.Exists("./config.json"))
                {
                    var content = File.ReadAllText("./config.json");
                    var config = JsonConvert.DeserializeObject<IdPwd>(content);
                    //存在，就更新框体数据为已存数据
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        TextID.Text = config.id;
                        PwdBoxPwd.Password = config.password;
                    }));
                }
            }
            catch(Exception e)
            {
                //捕捉异常
                AppendLog(e.Message);
            }
            
        }
        // 消息列表添加数据记录
        private void AppendLog(string text)
        {
            var len = 30;
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                //消息长度一行为len
                //添加消息带上时间戳
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
        //登录操作
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        { 
            //信息填入
            var mode = "login";
            var loginMes = new IdPwd
            {
                id = TextID.Text,
                password = PwdBoxPwd.Password
            };
            var data = JsonConvert.SerializeObject(loginMes);
            //ws通信
            ws = new WebSocket("ws://" + server + "/mode=" + mode);
            ws.OnMessage += ( s, ee)=>{ 
                var recData = JsonConvert.DeserializeObject<AccountData>(ee.Data);
                if (recData.code == "100")
                {
                    account = JsonConvert.DeserializeObject<Account>(recData.data.ToString());
                    //更新窗体数据
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
                    AppendLog("抱歉，登录错误。错误代码：" + recData.code + ",错误信息：" + recData.mes);
                }
            };
            ws.OnClose += (s, ee) => {
                //AppendLog("服务器通讯结束!");
            };
            ws.Connect();
            //发送消息
            ws.Send(data);
        }
        //密码修改按键
        private void BtnPwd_Click(object sender, RoutedEventArgs e)
        {
            //打开窗体密码窗体，禁止基础窗体操作
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                GridHigh.IsEnabled = false;
            }));
            pwdWindow = new PwdWindow();
            pwdWindow.Topmost = true;
            pwdWindow.Show();
            //接收到修改密码委托
            pwdWindow.passNewPwd += ModifyPwd;
        }


        //修改密码委托
        private void ModifyPwd(string newPwd)
        {
            //密码修改消息定义
            var mode = "modify";
            var modifyMes = new IdPwd
            {
                id = account.id,
                password = newPwd
            };
            var data = JsonConvert.SerializeObject(modifyMes);
            //ws定义
            ws = new WebSocket("ws://" + server + "/mode=" + mode);
            ws.OnMessage += (s, ee) => { 
                var recMes = JsonConvert.DeserializeObject<BasicData>(ee.Data);
                //数据代码正常
                if (recMes.code == "200")
                {
                    AppendLog(recMes.mes);
                }
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    GridHigh.IsEnabled = true;
                }));
            };
            ws.OnClose += (s, ee) => {
                //AppendLog("服务器通讯结束!");
            };
            ws.Connect();
            ws.Send(data);
        }
        //账户切换
        private void BtnAccount_Click(object sender, RoutedEventArgs e)
        {
            account = new Account();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                GridHigh.Visibility = Visibility.Hidden;
                GridBasic.Visibility = Visibility.Visible;
                ListLog.Items.Clear();
            }));
        }
        //教师日志操作
        private void BtnLogView_Click(object sender, RoutedEventArgs e)
        {
          //ToDo
        }
        //远程操作打开
        private void BtnOperate_Click(object sender, RoutedEventArgs e)
        {
            GridMain.IsEnabled = false;
            operateWindow = new OperateWindow(server,account.id);
            operateWindow.Topmost = true;
            operateWindow.Show();
            operateWindow.activateGrid += AfterOperate;
        }
        //远程操作结束委托
        private void AfterOperate()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                GridMain.IsEnabled = true;
            }));
        }
        //窗体关闭处理
        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
