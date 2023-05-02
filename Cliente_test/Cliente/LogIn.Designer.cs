namespace Version_1
{
    partial class LogIn
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.acceptButton = new System.Windows.Forms.Button();
            this.userBox = new System.Windows.Forms.TextBox();
            this.userLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.registerButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.connlbl = new System.Windows.Forms.Label();
            this.Mostrar_Contraseña = new System.Windows.Forms.CheckBox();
            this.Enviar_Consulta = new System.Windows.Forms.Button();
            this.Consulta1 = new System.Windows.Forms.RadioButton();
            this.Consulta2 = new System.Windows.Forms.RadioButton();
            this.Consulta3 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.connect_status = new System.Windows.Forms.TextBox();
            this.emailBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Desconectar_Btn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.num_usuarios = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.loading_text = new System.Windows.Forms.Label();
            this.Close_Btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Location = new System.Drawing.Point(12, 200);
            this.acceptButton.Margin = new System.Windows.Forms.Padding(2);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(56, 19);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "Log in";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // userBox
            // 
            this.userBox.Location = new System.Drawing.Point(66, 54);
            this.userBox.Margin = new System.Windows.Forms.Padding(2);
            this.userBox.Name = "userBox";
            this.userBox.Size = new System.Drawing.Size(109, 20);
            this.userBox.TabIndex = 1;
            this.userBox.Text = "pepa";
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(65, 37);
            this.userLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(29, 13);
            this.userLabel.TabIndex = 2;
            this.userLabel.Text = "User";
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(64, 75);
            this.passwordLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(53, 13);
            this.passwordLabel.TabIndex = 4;
            this.passwordLabel.Text = "Password";
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(66, 90);
            this.passwordBox.Margin = new System.Windows.Forms.Padding(2);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(109, 20);
            this.passwordBox.TabIndex = 3;
            this.passwordBox.Text = "pig";
            // 
            // registerButton
            // 
            this.registerButton.Location = new System.Drawing.Point(179, 200);
            this.registerButton.Margin = new System.Windows.Forms.Padding(2);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(56, 19);
            this.registerButton.TabIndex = 5;
            this.registerButton.Text = "Register";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(305, 84);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(211, 501);
            this.dataGridView1.TabIndex = 6;
            // 
            // connlbl
            // 
            this.connlbl.AutoSize = true;
            this.connlbl.Location = new System.Drawing.Point(540, 51);
            this.connlbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.connlbl.Name = "connlbl";
            this.connlbl.Size = new System.Drawing.Size(0, 13);
            this.connlbl.TabIndex = 8;
            // 
            // Mostrar_Contraseña
            // 
            this.Mostrar_Contraseña.AutoSize = true;
            this.Mostrar_Contraseña.Location = new System.Drawing.Point(68, 113);
            this.Mostrar_Contraseña.Margin = new System.Windows.Forms.Padding(2);
            this.Mostrar_Contraseña.Name = "Mostrar_Contraseña";
            this.Mostrar_Contraseña.Size = new System.Drawing.Size(101, 17);
            this.Mostrar_Contraseña.TabIndex = 3;
            this.Mostrar_Contraseña.Text = "Show password";
            this.Mostrar_Contraseña.UseVisualStyleBackColor = true;
            this.Mostrar_Contraseña.CheckedChanged += new System.EventHandler(this.Mostrar_Contraseña_CheckedChanged);
            // 
            // Enviar_Consulta
            // 
            this.Enviar_Consulta.Location = new System.Drawing.Point(94, 67);
            this.Enviar_Consulta.Margin = new System.Windows.Forms.Padding(2);
            this.Enviar_Consulta.Name = "Enviar_Consulta";
            this.Enviar_Consulta.Size = new System.Drawing.Size(56, 19);
            this.Enviar_Consulta.TabIndex = 11;
            this.Enviar_Consulta.Text = "Consult";
            this.Enviar_Consulta.UseVisualStyleBackColor = true;
            this.Enviar_Consulta.Click += new System.EventHandler(this.Enviar_Consulta_Click);
            // 
            // Consulta1
            // 
            this.Consulta1.AutoSize = true;
            this.Consulta1.Location = new System.Drawing.Point(14, 24);
            this.Consulta1.Margin = new System.Windows.Forms.Padding(2);
            this.Consulta1.Name = "Consulta1";
            this.Consulta1.Size = new System.Drawing.Size(65, 17);
            this.Consulta1.TabIndex = 8;
            this.Consulta1.TabStop = true;
            this.Consulta1.Text = "Ranking";
            this.Consulta1.UseVisualStyleBackColor = true;
            // 
            // Consulta2
            // 
            this.Consulta2.AutoSize = true;
            this.Consulta2.Location = new System.Drawing.Point(94, 24);
            this.Consulta2.Margin = new System.Windows.Forms.Padding(2);
            this.Consulta2.Name = "Consulta2";
            this.Consulta2.Size = new System.Drawing.Size(83, 17);
            this.Consulta2.TabIndex = 9;
            this.Consulta2.TabStop = true;
            this.Consulta2.Text = "Online users";
            this.Consulta2.UseVisualStyleBackColor = true;
            // 
            // Consulta3
            // 
            this.Consulta3.AutoSize = true;
            this.Consulta3.Location = new System.Drawing.Point(182, 24);
            this.Consulta3.Margin = new System.Windows.Forms.Padding(2);
            this.Consulta3.Name = "Consulta3";
            this.Consulta3.Size = new System.Drawing.Size(78, 17);
            this.Consulta3.TabIndex = 10;
            this.Consulta3.TabStop = true;
            this.Consulta3.Text = "New Game";
            this.Consulta3.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.Consulta1);
            this.panel2.Controls.Add(this.Enviar_Consulta);
            this.panel2.Controls.Add(this.Consulta3);
            this.panel2.Controls.Add(this.Consulta2);
            this.panel2.Location = new System.Drawing.Point(23, 281);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(269, 102);
            this.panel2.TabIndex = 13;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.connect_status);
            this.panel3.Controls.Add(this.emailBox);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.userLabel);
            this.panel3.Controls.Add(this.userBox);
            this.panel3.Controls.Add(this.Mostrar_Contraseña);
            this.panel3.Controls.Add(this.passwordLabel);
            this.panel3.Controls.Add(this.registerButton);
            this.panel3.Controls.Add(this.passwordBox);
            this.panel3.Controls.Add(this.acceptButton);
            this.panel3.Location = new System.Drawing.Point(23, 28);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(269, 233);
            this.panel3.TabIndex = 14;
            // 
            // connect_status
            // 
            this.connect_status.BackColor = System.Drawing.Color.Red;
            this.connect_status.ForeColor = System.Drawing.Color.White;
            this.connect_status.Location = new System.Drawing.Point(182, 6);
            this.connect_status.Name = "connect_status";
            this.connect_status.Size = new System.Drawing.Size(75, 20);
            this.connect_status.TabIndex = 19;
            this.connect_status.Text = "Disconnected";
            this.connect_status.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // emailBox
            // 
            this.emailBox.Location = new System.Drawing.Point(68, 159);
            this.emailBox.Margin = new System.Windows.Forms.Padding(2);
            this.emailBox.Name = "emailBox";
            this.emailBox.Size = new System.Drawing.Size(109, 20);
            this.emailBox.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 144);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Email (Only for register)";
            // 
            // Desconectar_Btn
            // 
            this.Desconectar_Btn.Location = new System.Drawing.Point(544, 531);
            this.Desconectar_Btn.Margin = new System.Windows.Forms.Padding(2);
            this.Desconectar_Btn.Name = "Desconectar_Btn";
            this.Desconectar_Btn.Size = new System.Drawing.Size(494, 79);
            this.Desconectar_Btn.TabIndex = 12;
            this.Desconectar_Btn.Text = "Disconnect";
            this.Desconectar_Btn.UseVisualStyleBackColor = true;
            this.Desconectar_Btn.Click += new System.EventHandler(this.Desconectar_Btn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.RosyBrown;
            this.pictureBox1.Image = global::Version_1.Properties.Resources.PortadaInvertedPacman;
            this.pictureBox1.Location = new System.Drawing.Point(544, 28);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(540, 435);
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.num_usuarios);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Location = new System.Drawing.Point(307, 28);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(211, 56);
            this.panel4.TabIndex = 23;
            // 
            // num_usuarios
            // 
            this.num_usuarios.AutoSize = true;
            this.num_usuarios.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.num_usuarios.Location = new System.Drawing.Point(68, 38);
            this.num_usuarios.Name = "num_usuarios";
            this.num_usuarios.Size = new System.Drawing.Size(82, 13);
            this.num_usuarios.TabIndex = 1;
            this.num_usuarios.Text = "Not available";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(40, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "Connected Users";
            // 
            // loading_text
            // 
            this.loading_text.AutoSize = true;
            this.loading_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loading_text.Location = new System.Drawing.Point(777, 483);
            this.loading_text.Name = "loading_text";
            this.loading_text.Size = new System.Drawing.Size(0, 13);
            this.loading_text.TabIndex = 25;
            // 
            // Close_Btn
            // 
            this.Close_Btn.Location = new System.Drawing.Point(1028, 5);
            this.Close_Btn.Margin = new System.Windows.Forms.Padding(2);
            this.Close_Btn.Name = "Close_Btn";
            this.Close_Btn.Size = new System.Drawing.Size(56, 19);
            this.Close_Btn.TabIndex = 13;
            this.Close_Btn.Text = "Close";
            this.Close_Btn.UseVisualStyleBackColor = true;
            this.Close_Btn.Click += new System.EventHandler(this.Close_Btn_Click);
            // 
            // LogIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RosyBrown;
            this.ClientSize = new System.Drawing.Size(1102, 621);
            this.Controls.Add(this.Close_Btn);
            this.Controls.Add(this.loading_text);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Desconectar_Btn);
            this.Controls.Add(this.connlbl);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.dataGridView1);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "LogIn";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogIn_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.TextBox userBox;
        private System.Windows.Forms.Label userLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Button registerButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label connlbl;
        private System.Windows.Forms.CheckBox Mostrar_Contraseña;
        private System.Windows.Forms.Button Enviar_Consulta;
        private System.Windows.Forms.RadioButton Consulta1;
        private System.Windows.Forms.RadioButton Consulta2;
        private System.Windows.Forms.RadioButton Consulta3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button Desconectar_Btn;
        private System.Windows.Forms.TextBox emailBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label num_usuarios;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox connect_status;
        private System.Windows.Forms.Label loading_text;
        private System.Windows.Forms.Button Close_Btn;
    }
}

