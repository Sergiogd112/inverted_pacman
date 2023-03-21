namespace Version_1
{
    partial class Register
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
            this.registerButton = new System.Windows.Forms.Button();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.userLabel = new System.Windows.Forms.Label();
            this.userBox = new System.Windows.Forms.TextBox();
            this.addressLabel = new System.Windows.Forms.Label();
            this.addressBox = new System.Windows.Forms.TextBox();
            this.ipLabel = new System.Windows.Forms.Label();
            this.ipBox = new System.Windows.Forms.TextBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.portBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // registerButton
            // 
            this.registerButton.Location = new System.Drawing.Point(110, 240);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(75, 23);
            this.registerButton.TabIndex = 11;
            this.registerButton.Text = "Register";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(107, 140);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(67, 16);
            this.passwordLabel.TabIndex = 10;
            this.passwordLabel.Text = "Password";
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(110, 170);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(100, 22);
            this.passwordBox.TabIndex = 9;
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(107, 57);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(76, 16);
            this.userLabel.TabIndex = 8;
            this.userLabel.Text = "User Name";
            // 
            // userBox
            // 
            this.userBox.Location = new System.Drawing.Point(110, 86);
            this.userBox.Name = "userBox";
            this.userBox.Size = new System.Drawing.Size(100, 22);
            this.userBox.TabIndex = 7;
            // 
            // addressLabel
            // 
            this.addressLabel.AutoSize = true;
            this.addressLabel.Location = new System.Drawing.Point(261, 57);
            this.addressLabel.Name = "addressLabel";
            this.addressLabel.Size = new System.Drawing.Size(58, 16);
            this.addressLabel.TabIndex = 13;
            this.addressLabel.Text = "Address";
            // 
            // addressBox
            // 
            this.addressBox.Location = new System.Drawing.Point(264, 86);
            this.addressBox.Name = "addressBox";
            this.addressBox.Size = new System.Drawing.Size(100, 22);
            this.addressBox.TabIndex = 12;
            // 
            // ipLabel
            // 
            this.ipLabel.AutoSize = true;
            this.ipLabel.Location = new System.Drawing.Point(544, 57);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(19, 16);
            this.ipLabel.TabIndex = 15;
            this.ipLabel.Text = "IP";
            // 
            // ipBox
            // 
            this.ipBox.Location = new System.Drawing.Point(547, 86);
            this.ipBox.Name = "ipBox";
            this.ipBox.Size = new System.Drawing.Size(100, 22);
            this.ipBox.TabIndex = 14;
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(544, 141);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(31, 16);
            this.portLabel.TabIndex = 17;
            this.portLabel.Text = "Port";
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(547, 170);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(100, 22);
            this.portBox.TabIndex = 16;
            // 
            // Register
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.portBox);
            this.Controls.Add(this.ipLabel);
            this.Controls.Add(this.ipBox);
            this.Controls.Add(this.addressLabel);
            this.Controls.Add(this.addressBox);
            this.Controls.Add(this.registerButton);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.passwordBox);
            this.Controls.Add(this.userLabel);
            this.Controls.Add(this.userBox);
            this.Name = "Register";
            this.Text = "Resgister";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button registerButton;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Label userLabel;
        private System.Windows.Forms.TextBox userBox;
        private System.Windows.Forms.Label addressLabel;
        private System.Windows.Forms.TextBox addressBox;
        private System.Windows.Forms.Label ipLabel;
        private System.Windows.Forms.TextBox ipBox;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.TextBox portBox;
    }
}