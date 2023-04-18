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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ipBox = new System.Windows.Forms.TextBox();
            this.portBox = new System.Windows.Forms.TextBox();
            this.Conectar = new System.Windows.Forms.Button();
            this.Desconectar_Btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Location = new System.Drawing.Point(28, 200);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 23);
            this.acceptButton.TabIndex = 0;
            this.acceptButton.Text = "Log in";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // userBox
            // 
            this.userBox.Location = new System.Drawing.Point(90, 55);
            this.userBox.Name = "userBox";
            this.userBox.Size = new System.Drawing.Size(144, 22);
            this.userBox.TabIndex = 1;
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(87, 31);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(36, 16);
            this.userLabel.TabIndex = 2;
            this.userLabel.Text = "User";
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(87, 96);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(67, 16);
            this.passwordLabel.TabIndex = 4;
            this.passwordLabel.Text = "Password";
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(90, 115);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(144, 22);
            this.passwordBox.TabIndex = 3;
            // 
            // registerButton
            // 
            this.registerButton.Location = new System.Drawing.Point(225, 200);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(75, 23);
            this.registerButton.TabIndex = 5;
            this.registerButton.Text = "Register";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(628, 183);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(240, 220);
            this.dataGridView1.TabIndex = 6;
            // 
            // connlbl
            // 
            this.connlbl.AutoSize = true;
            this.connlbl.Location = new System.Drawing.Point(721, 133);
            this.connlbl.Name = "connlbl";
            this.connlbl.Size = new System.Drawing.Size(0, 16);
            this.connlbl.TabIndex = 8;
            // 
            // Mostrar_Contraseña
            // 
            this.Mostrar_Contraseña.AutoSize = true;
            this.Mostrar_Contraseña.Location = new System.Drawing.Point(90, 143);
            this.Mostrar_Contraseña.Name = "Mostrar_Contraseña";
            this.Mostrar_Contraseña.Size = new System.Drawing.Size(144, 20);
            this.Mostrar_Contraseña.TabIndex = 8;
            this.Mostrar_Contraseña.Text = "Mostrar contraseña";
            this.Mostrar_Contraseña.UseVisualStyleBackColor = true;
            this.Mostrar_Contraseña.CheckedChanged += new System.EventHandler(this.Mostrar_Contraseña_CheckedChanged);
            // 
            // Enviar_Consulta
            // 
            this.Enviar_Consulta.Location = new System.Drawing.Point(125, 82);
            this.Enviar_Consulta.Name = "Enviar_Consulta";
            this.Enviar_Consulta.Size = new System.Drawing.Size(75, 23);
            this.Enviar_Consulta.TabIndex = 9;
            this.Enviar_Consulta.Text = "Enviar";
            this.Enviar_Consulta.UseVisualStyleBackColor = true;
            this.Enviar_Consulta.Click += new System.EventHandler(this.Enviar_Consulta_Click);
            // 
            // Consulta1
            // 
            this.Consulta1.AutoSize = true;
            this.Consulta1.Location = new System.Drawing.Point(19, 29);
            this.Consulta1.Name = "Consulta1";
            this.Consulta1.Size = new System.Drawing.Size(87, 20);
            this.Consulta1.TabIndex = 10;
            this.Consulta1.TabStop = true;
            this.Consulta1.Text = "Consulta1";
            this.Consulta1.UseVisualStyleBackColor = true;
            // 
            // Consulta2
            // 
            this.Consulta2.AutoSize = true;
            this.Consulta2.Location = new System.Drawing.Point(125, 29);
            this.Consulta2.Name = "Consulta2";
            this.Consulta2.Size = new System.Drawing.Size(90, 20);
            this.Consulta2.TabIndex = 11;
            this.Consulta2.TabStop = true;
            this.Consulta2.Text = "Consulta 2";
            this.Consulta2.UseVisualStyleBackColor = true;
            // 
            // Consulta3
            // 
            this.Consulta3.AutoSize = true;
            this.Consulta3.Location = new System.Drawing.Point(242, 29);
            this.Consulta3.Name = "Consulta3";
            this.Consulta3.Size = new System.Drawing.Size(90, 20);
            this.Consulta3.TabIndex = 12;
            this.Consulta3.TabStop = true;
            this.Consulta3.Text = "Consulta 3";
            this.Consulta3.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.Consulta1);
            this.panel2.Controls.Add(this.Enviar_Consulta);
            this.panel2.Controls.Add(this.Consulta3);
            this.panel2.Controls.Add(this.Consulta2);
            this.panel2.Location = new System.Drawing.Point(12, 282);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(357, 125);
            this.panel2.TabIndex = 13;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.userLabel);
            this.panel3.Controls.Add(this.userBox);
            this.panel3.Controls.Add(this.Mostrar_Contraseña);
            this.panel3.Controls.Add(this.passwordLabel);
            this.panel3.Controls.Add(this.registerButton);
            this.panel3.Controls.Add(this.passwordBox);
            this.panel3.Controls.Add(this.acceptButton);
            this.panel3.Location = new System.Drawing.Point(15, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(354, 246);
            this.panel3.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(406, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 16);
            this.label2.TabIndex = 15;
            this.label2.Text = "IP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(406, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 16);
            this.label3.TabIndex = 16;
            this.label3.Text = "Port";
            // 
            // ipBox
            // 
            this.ipBox.Location = new System.Drawing.Point(450, 20);
            this.ipBox.Name = "ipBox";
            this.ipBox.Size = new System.Drawing.Size(100, 22);
            this.ipBox.TabIndex = 17;
            this.ipBox.Text = "192.168.56.102";
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(450, 57);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(100, 22);
            this.portBox.TabIndex = 18;
            this.portBox.Text = "9050";
            // 
            // Conectar
            // 
            this.Conectar.Location = new System.Drawing.Point(409, 138);
            this.Conectar.Name = "Conectar";
            this.Conectar.Size = new System.Drawing.Size(141, 92);
            this.Conectar.TabIndex = 19;
            this.Conectar.Text = "Conectar";
            this.Conectar.UseVisualStyleBackColor = true;
            this.Conectar.Click += new System.EventHandler(this.Conectar_Click);
            // 
            // Desconectar_Btn
            // 
            this.Desconectar_Btn.Location = new System.Drawing.Point(409, 311);
            this.Desconectar_Btn.Name = "Desconectar_Btn";
            this.Desconectar_Btn.Size = new System.Drawing.Size(141, 92);
            this.Desconectar_Btn.TabIndex = 20;
            this.Desconectar_Btn.Text = "Desconectar";
            this.Desconectar_Btn.UseVisualStyleBackColor = true;
            this.Desconectar_Btn.Click += new System.EventHandler(this.Desconectar_Btn_Click);
            // 
            // LogIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 427);
            this.Controls.Add(this.Desconectar_Btn);
            this.Controls.Add(this.connlbl);
            this.Controls.Add(this.Conectar);
            this.Controls.Add(this.portBox);
            this.Controls.Add(this.ipBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
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
        private System.Windows.Forms.Button Conectar;
        private System.Windows.Forms.Button Desconectar_Btn;
    }
}

