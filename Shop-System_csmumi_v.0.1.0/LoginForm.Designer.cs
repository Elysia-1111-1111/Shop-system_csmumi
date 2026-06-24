namespace Shop_System_csmumi_v._0._1._0
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            login = new Button();
            label1 = new Label();
            label2 = new Label();
            txtUser = new TextBox();
            txtPassword = new TextBox();
            SuspendLayout();
            // 
            // login
            // 
            login.BackColor = Color.FromArgb(255, 128, 0);
            login.Font = new Font("Consolas", 50F, FontStyle.Bold);
            login.ForeColor = Color.FromArgb(16, 20, 29);
            login.Location = new Point(-7, 317);
            login.Name = "login";
            login.Size = new Size(816, 108);
            login.TabIndex = 0;
            login.Text = "Login";
            login.UseVisualStyleBackColor = false;
            login.Click += login_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Consolas", 50F, FontStyle.Bold);
            label1.Location = new Point(70, 69);
            label1.Name = "label1";
            label1.Size = new Size(293, 79);
            label1.TabIndex = 1;
            label1.Text = "Account";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Consolas", 50F, FontStyle.Bold);
            label2.Location = new Point(70, 185);
            label2.Name = "label2";
            label2.Size = new Size(330, 79);
            label2.TabIndex = 2;
            label2.Text = "Password";
            // 
            // txtUser
            // 
            txtUser.BackColor = Color.FromArgb(160, 165, 181);
            txtUser.Font = new Font("Consolas", 40F, FontStyle.Bold);
            txtUser.ForeColor = Color.FromArgb(16, 20, 29);
            txtUser.Location = new Point(408, 79);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(327, 70);
            txtUser.TabIndex = 3;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.FromArgb(160, 165, 181);
            txtPassword.Font = new Font("Consolas", 40F, FontStyle.Bold);
            txtPassword.ForeColor = Color.FromArgb(16, 20, 29);
            txtPassword.Location = new Point(408, 194);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(327, 70);
            txtPassword.TabIndex = 4;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(16, 20, 29);
            ClientSize = new Size(800, 420);
            Controls.Add(txtPassword);
            Controls.Add(txtUser);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(login);
            Font = new Font("Consolas", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = Color.FromArgb(0, 245, 255);
            Name = "LoginForm";
            Text = "LoginForm";
            Load += LoginForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button login;
        private Label label1;
        private Label label2;
        private TextBox txtUser;
        private TextBox txtPassword;

        private void label1_Click_1(object sender, EventArgs e)
        {
            // 如果不需要任何行為，留空即可；或把你要的邏輯寫在這裡
        }
    }
}