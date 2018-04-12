using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Threading;

namespace HduRemoteLab
{
    /// <summary>
    /// OperateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OperateWindow : Window
    {

        public delegate void ActivateGrid();
        public event ActivateGrid activateGrid;

        public WebSocket wsOperate;
        public string server;
        public string log_id;
        public string mode = "experiment";
        public BackMes backMes;
        public ExperimentData aim;
        public List<string> slaves;
        public ExperimentData experiments;
        public FlagData flagData;
        public OperateData operateData;
    

        public OperateWindow(string server,string log_id)
        {
            this.server = server;
            this.log_id = log_id;
            operateData.log_id = log_id;
            GridOperate.IsEnabled = false;
            InitializeComponent();

            wsOperate = new WebSocket("ws://" + server + "/mode=" + mode);
            wsOperate.OnMessage += (s, ee) => {
                var mes = JsonConvert.DeserializeObject<MesData>(ee.Data);
                if (mes.code == "100")
                {
                    experiments = JsonConvert.DeserializeObject<ExperimentData>(mes.message.ToString());
                    AppendLog("控制及实验获取完毕");

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        ComboSlaves.ItemsSource = experiments.slaves;
                        ComboSlaves.DisplayMemberPath = "name";
                    }));
                }
                else
                {
                    backMes = JsonConvert.DeserializeObject<BackMes>(mes.message.ToString());
                    AppendLog("错误代码：" + mes.code + ",错误信息：" + backMes.message);
                }
            };
            wsOperate.OnClose += (s, ee) => {
                AppendLog("服务器通讯结束!");
            };
            wsOperate.Connect();
            var data = "state";
            wsOperate.Send(data);
        }

        private void AppendLog(string text)
        {
            var len = 30;
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                ListResult.Items.Add("[" + DateTime.Now.ToString() + "] ");
                string temp;
                if (text.Length > len)
                    for (int i = 0; text.Length > len; i++)
                    {
                        temp = text.Substring(0, len);
                        ListResult.Items.Add(temp);
                        text = text.Substring(len);
                    }
               ListResult.Items.Add(text);
               ListResult.SelectedIndex = ListResult.Items.Count - 1;
               ListResult.ScrollIntoView(ListResult.SelectedItem);
            }));
        }

        private void ComboSlaves_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            operateData.slave_anme = ComboSlaves.SelectedValue.ToString();
            AppendLog("选中从机为:" + operateData.slave_anme);
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                ComboExperiment.ItemsSource = experiments.slaves[ComboSlaves.SelectedIndex].experiments;
            }));
        }

        private void ComboExperiment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            operateData.expriment_name = ComboExperiment.SelectedValue.ToString();
            AppendLog("选中实验为:" + operateData.expriment_name);
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                GridOperate.IsEnabled = false;
            }));
        }

        private void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            //先不实现
        }

        private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WindowsOperate_Closed(object sender, EventArgs e)
        {
            activateGrid();
        }

        
    }
}
