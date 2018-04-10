using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HduRemoteLab
{
    /// <summary>
    /// PwdWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PwdWindow : Window
    {
        public delegate void PassNewPwd(string password);
        public event PassNewPwd passNewPwd;

        public PwdWindow()
        {
            InitializeComponent();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            var pwd1 = PwdBoxNewPwd.Password;
            var pwd2 = PwdBoxPwdAgain.Password;
            if(pwd1!=pwd2)
            {
                MessageBox.Show("两次新密码不一致！");
            }
            else
            {
                passNewPwd(pwd1);
                this.Close();
            }
        }
    }
}
