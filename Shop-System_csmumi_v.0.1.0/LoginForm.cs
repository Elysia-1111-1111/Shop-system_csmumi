using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Shop_System_csmumi_v._0._1._0
{
    public partial class LoginForm : Form
    {
        int i = 3;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void login_Click(object sender, EventArgs e)
        {
            string account = txtUser.Text;
            string password = txtPassword.Text;
            if (LoginSystem.VerifyLogin(account, password))
            {
                System.Windows.Forms.MessageBox.Show("登入成功 歡迎使用超商結帳系統。", "系統提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                超商結帳系統 mainform = new 超商結帳系統();
                mainform.ShowDialog();
                this.Close();
            }
            else
            {
                i--;
                if (i <= 0)
                {
                    System.Windows.Forms.MessageBox.Show("密碼錯誤超過 3 次！系統強制安全關閉。", "安全警告", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    // 4. 強制關閉整個應用程式的標準寫法
                    System.Windows.Forms.Application.Exit();
                }
                // 8. 如果回傳 false，代表調皮輸入錯誤，無情打槍
                else
                {
                    System.Windows.Forms.MessageBox.Show($"帳號或密碼錯誤！您還剩餘 {i} 次機會。", "登入失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // 9. 防呆小貼心：自動幫他清空打錯的密碼框，並把游標重新聚焦回密碼框，方便重新輸入
                    txtPassword.Clear();
                    txtPassword.Focus();
                    
                }
                   
            }
        }
    }
}
