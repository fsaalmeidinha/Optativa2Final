namespace CaixeiroViajanteUI
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtEndereco = new System.Windows.Forms.TextBox();
            this.lblZona = new System.Windows.Forms.Label();
            this.btnLocalizar = new System.Windows.Forms.Button();
            this.pnlBancos = new System.Windows.Forms.Panel();
            this.txtRota = new System.Windows.Forms.TextBox();
            this.btnGerarRota = new System.Windows.Forms.Button();
            this.pnlBancos.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Endereço ou CEP (caso CEP: XXXXX-XXX)";
            // 
            // txtEndereco
            // 
            this.txtEndereco.Location = new System.Drawing.Point(16, 30);
            this.txtEndereco.Name = "txtEndereco";
            this.txtEndereco.Size = new System.Drawing.Size(301, 20);
            this.txtEndereco.TabIndex = 1;
            this.txtEndereco.TextChanged += new System.EventHandler(this.txtEndereco_TextChanged);
            // 
            // lblZona
            // 
            this.lblZona.AutoSize = true;
            this.lblZona.Location = new System.Drawing.Point(19, 59);
            this.lblZona.Name = "lblZona";
            this.lblZona.Size = new System.Drawing.Size(0, 13);
            this.lblZona.TabIndex = 2;
            // 
            // btnLocalizar
            // 
            this.btnLocalizar.Location = new System.Drawing.Point(323, 27);
            this.btnLocalizar.Name = "btnLocalizar";
            this.btnLocalizar.Size = new System.Drawing.Size(81, 23);
            this.btnLocalizar.TabIndex = 3;
            this.btnLocalizar.Text = "Localizar";
            this.btnLocalizar.UseVisualStyleBackColor = true;
            this.btnLocalizar.Visible = false;
            this.btnLocalizar.Click += new System.EventHandler(this.btnLocalizar_Click);
            // 
            // pnlBancos
            // 
            this.pnlBancos.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBancos.Controls.Add(this.txtRota);
            this.pnlBancos.Controls.Add(this.btnGerarRota);
            this.pnlBancos.Enabled = false;
            this.pnlBancos.Location = new System.Drawing.Point(16, 76);
            this.pnlBancos.Name = "pnlBancos";
            this.pnlBancos.Size = new System.Drawing.Size(712, 407);
            this.pnlBancos.TabIndex = 4;
            // 
            // txtRota
            // 
            this.txtRota.Location = new System.Drawing.Point(278, 1);
            this.txtRota.Multiline = true;
            this.txtRota.Name = "txtRota";
            this.txtRota.Size = new System.Drawing.Size(431, 405);
            this.txtRota.TabIndex = 6;
            // 
            // btnGerarRota
            // 
            this.btnGerarRota.Location = new System.Drawing.Point(197, 175);
            this.btnGerarRota.Name = "btnGerarRota";
            this.btnGerarRota.Size = new System.Drawing.Size(75, 23);
            this.btnGerarRota.TabIndex = 5;
            this.btnGerarRota.Text = "Gerar Rota";
            this.btnGerarRota.UseVisualStyleBackColor = true;
            this.btnGerarRota.Click += new System.EventHandler(this.btnGerarRota_Click);
            this.btnGerarRota.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnGerarRota_KeyDown);
            this.btnGerarRota.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnGerarRota_MouseDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 493);
            this.Controls.Add(this.pnlBancos);
            this.Controls.Add(this.btnLocalizar);
            this.Controls.Add(this.lblZona);
            this.Controls.Add(this.txtEndereco);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(751, 520);
            this.MinimumSize = new System.Drawing.Size(751, 520);
            this.Name = "Form1";
            this.Text = "Caixeiro Viajante";
            this.pnlBancos.ResumeLayout(false);
            this.pnlBancos.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEndereco;
        private System.Windows.Forms.Label lblZona;
        private System.Windows.Forms.Button btnLocalizar;
        private System.Windows.Forms.Panel pnlBancos;
        private System.Windows.Forms.Button btnGerarRota;
        private System.Windows.Forms.TextBox txtRota;
    }
}

