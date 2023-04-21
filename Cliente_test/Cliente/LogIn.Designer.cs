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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ipBox = new System.Windows.Forms.TextBox();
            this.portBox = new System.Windows.Forms.TextBox();
            this.Desconectar_Btn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.num_usuarios = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.jugar = new System.Windows.Forms.Button();
            this.loading_text = new System.Windows.Forms.Label();
            this.Close_Btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Location = new System.Drawing.Point(16, 246);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 23);
            this.acceptButton.TabIndex = 0;
            this.acceptButton.Text = "Log in";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // userBox
            // 
            this.userBox.Location = new System.Drawing.Point(88, 67);
            this.userBox.Name = "userBox";
            this.userBox.Size = new System.Drawing.Size(144, 22);
            this.userBox.TabIndex = 1;
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(87, 45);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(36, 16);
            this.userLabel.TabIndex = 2;
            this.userLabel.Text = "User";
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(85, 92);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(67, 16);
            this.passwordLabel.TabIndex = 4;
            this.passwordLabel.Text = "Password";
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(88, 111);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(144, 22);
            this.passwordBox.TabIndex = 3;
            // 
            // registerButton
            // 
            this.registerButton.Location = new System.Drawing.Point(239, 246);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(75, 23);
            this.registerButton.TabIndex = 5;
            this.registerButton.Text = "Register";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(409, 135);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(281, 516);
            this.dataGridView1.TabIndex = 6;
            // 
            // connlbl
            // 
            this.connlbl.AutoSize = true;
            this.connlbl.Location = new System.Drawing.Point(720, 63);
            this.connlbl.Name = "connlbl";
            this.connlbl.Size = new System.Drawing.Size(0, 16);
            this.connlbl.TabIndex = 8;
            // 
            // Mostrar_Contraseña
            // 
            this.Mostrar_Contraseña.AutoSize = true;
            this.Mostrar_Contraseña.Location = new System.Drawing.Point(90, 139);
            this.Mostrar_Contraseña.Name = "Mostrar_Contraseña";
            this.Mostrar_Contraseña.Size = new System.Drawing.Size(124, 20);
            this.Mostrar_Contraseña.TabIndex = 8;
            this.Mostrar_Contraseña.Text = "Show password";
            this.Mostrar_Contraseña.UseVisualStyleBackColor = true;
            this.Mostrar_Contraseña.CheckedChanged += new System.EventHandler(this.Mostrar_Contraseña_CheckedChanged);
            // 
            // Enviar_Consulta
            // 
            this.Enviar_Consulta.Location = new System.Drawing.Point(125, 82);
            this.Enviar_Consulta.Name = "Enviar_Consulta";
            this.Enviar_Consulta.Size = new System.Drawing.Size(75, 23);
            this.Enviar_Consulta.TabIndex = 9;
            this.Enviar_Consulta.Text = "Consult";
            this.Enviar_Consulta.UseVisualStyleBackColor = true;
            this.Enviar_Consulta.Click += new System.EventHandler(this.Enviar_Consulta_Click);
            // 
            // Consulta1
            // 
            this.Consulta1.AutoSize = true;
            this.Consulta1.Location = new System.Drawing.Point(19, 29);
            this.Consulta1.Name = "Consulta1";
            this.Consulta1.Size = new System.Drawing.Size(78, 20);
            this.Consulta1.TabIndex = 10;
            this.Consulta1.TabStop = true;
            this.Consulta1.Text = "Ranking";
            this.Consulta1.UseVisualStyleBackColor = true;
            // 
            // Consulta2
            // 
            this.Consulta2.AutoSize = true;
            this.Consulta2.Location = new System.Drawing.Point(125, 29);
            this.Consulta2.Name = "Consulta2";
            this.Consulta2.Size = new System.Drawing.Size(102, 20);
            this.Consulta2.TabIndex = 11;
            this.Consulta2.TabStop = true;
            this.Consulta2.Text = "Online users";
            this.Consulta2.UseVisualStyleBackColor = true;
            // 
            // Consulta3
            // 
            this.Consulta3.AutoSize = true;
            this.Consulta3.Location = new System.Drawing.Point(242, 29);
            this.Consulta3.Name = "Consulta3";
            this.Consulta3.Size = new System.Drawing.Size(95, 20);
            this.Consulta3.TabIndex = 12;
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
            this.panel2.Location = new System.Drawing.Point(31, 627);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(357, 125);
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
            this.panel3.Location = new System.Drawing.Point(31, 35);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(357, 286);
            this.panel3.TabIndex = 14;
            // 
            // connect_status
            // 
            this.connect_status.BackColor = System.Drawing.Color.Red;
            this.connect_status.ForeColor = System.Drawing.Color.White;
            this.connect_status.Location = new System.Drawing.Point(242, 8);
            this.connect_status.Margin = new System.Windows.Forms.Padding(4);
            this.connect_status.Name = "connect_status";
            this.connect_status.Size = new System.Drawing.Size(99, 22);
            this.connect_status.TabIndex = 19;
            this.connect_status.Text = "Desconectado";
            this.connect_status.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // emailBox
            // 
            this.emailBox.Location = new System.Drawing.Point(90, 196);
            this.emailBox.Name = "emailBox";
            this.emailBox.Size = new System.Drawing.Size(144, 22);
            this.emailBox.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 177);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "Email (Only for register)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 16);
            this.label2.TabIndex = 15;
            this.label2.Text = "IP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 16);
            this.label3.TabIndex = 16;
            this.label3.Text = "Port";
            // 
            // ipBox
            // 
            this.ipBox.Location = new System.Drawing.Point(95, 45);
            this.ipBox.Name = "ipBox";
            this.ipBox.Size = new System.Drawing.Size(152, 22);
            this.ipBox.TabIndex = 17;
            this.ipBox.Text = "192.168.56.102";
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(95, 106);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(152, 22);
            this.portBox.TabIndex = 18;
            this.portBox.Text = "50053";
            // 
            // Desconectar_Btn
            // 
            this.Desconectar_Btn.Location = new System.Drawing.Point(726, 659);
            this.Desconectar_Btn.Name = "Desconectar_Btn";
            this.Desconectar_Btn.Size = new System.Drawing.Size(659, 97);
            this.Desconectar_Btn.TabIndex = 20;
            this.Desconectar_Btn.Text = "Disconnect";
            this.Desconectar_Btn.UseVisualStyleBackColor = true;
            this.Desconectar_Btn.Click += new System.EventHandler(this.Desconectar_Btn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.RosyBrown;
            this.pictureBox1.Image = global::Version_1.Properties.Resources.PortadaInvertedPacman;
            this.pictureBox1.Location = new System.Drawing.Point(726, 35);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(720, 535);
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.portBox);
            this.panel1.Controls.Add(this.ipBox);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(31, 385);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(357, 170);
            this.panel1.TabIndex = 22;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.num_usuarios);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Location = new System.Drawing.Point(409, 35);
            this.panel4.Margin = new System.Windows.Forms.Padding(4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(281, 69);
            this.panel4.TabIndex = 23;
            // 
            // num_usuarios
            // 
            this.num_usuarios.AutoSize = true;
            this.num_usuarios.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.num_usuarios.Location = new System.Drawing.Point(90, 47);
            this.num_usuarios.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.num_usuarios.Name = "num_usuarios";
            this.num_usuarios.Size = new System.Drawing.Size(107, 17);
            this.num_usuarios.TabIndex = 1;
            this.num_usuarios.Text = "No disponible";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(24, 9);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(231, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "Usuarios Connectados";
            // 
            // jugar
            // 
            this.jugar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jugar.ForeColor = System.Drawing.Color.White;
            this.jugar.Location = new System.Drawing.Point(409, 658);
            this.jugar.Margin = new System.Windows.Forms.Padding(4);
            this.jugar.Name = "jugar";
            this.jugar.Size = new System.Drawing.Size(281, 97);
            this.jugar.TabIndex = 24;
            this.jugar.Text = "Jugar";
            this.jugar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.jugar.UseVisualStyleBackColor = true;
            this.jugar.Visible = false;
            // 
            // loading_text
            // 
            this.loading_text.AutoSize = true;
            this.loading_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loading_text.Location = new System.Drawing.Point(1036, 594);
            this.loading_text.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.loading_text.Name = "loading_text";
            this.loading_text.Size = new System.Drawing.Size(0, 17);
            this.loading_text.TabIndex = 25;
            // 
            // Close_Btn
            // 
            this.Close_Btn.Location = new System.Drawing.Point(1371, 6);
            this.Close_Btn.Name = "Close_Btn";
            this.Close_Btn.Size = new System.Drawing.Size(75, 23);
            this.Close_Btn.TabIndex = 26;
            this.Close_Btn.Text = "Close";
            this.Close_Btn.UseVisualStyleBackColor = true;
            this.Close_Btn.Click += new System.EventHandler(this.Close_Btn_Click);
            // 
            // LogIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RosyBrown;
            this.ClientSize = new System.Drawing.Size(1469, 764);
            this.Controls.Add(this.Close_Btn);
            this.Controls.Add(this.loading_text);
            this.Controls.Add(this.jugar);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Desconectar_Btn);
            this.Controls.Add(this.connlbl);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.dataGridView1);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Name = "LogIn";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ipBox;
        private System.Windows.Forms.TextBox portBox;
        private System.Windows.Forms.Button Desconectar_Btn;
        private System.Windows.Forms.TextBox emailBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label num_usuarios;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button jugar;
        private System.Windows.Forms.TextBox connect_status;
        private System.Windows.Forms.Label loading_text;
        private System.Windows.Forms.Button Close_Btn;
    }
}

