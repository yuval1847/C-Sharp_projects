namespace Prog_1_winforms_client
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
            this.txtMess = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.lstUsers = new System.Windows.Forms.ListBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Impact", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(477, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(291, 68);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chat Client";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtMess
            // 
            this.txtMess.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtMess.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.txtMess.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtMess.Location = new System.Drawing.Point(374, 457);
            this.txtMess.Name = "txtMess";
            this.txtMess.Size = new System.Drawing.Size(505, 48);
            this.txtMess.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.MidnightBlue;
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnSend.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnSend.Location = new System.Drawing.Point(532, 524);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(193, 81);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lstUsers
            // 
            this.lstUsers.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lstUsers.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lstUsers.FormattingEnabled = true;
            this.lstUsers.ItemHeight = 20;
            this.lstUsers.Location = new System.Drawing.Point(722, 199);
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.Size = new System.Drawing.Size(157, 224);
            this.lstUsers.TabIndex = 3;
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.MidnightBlue;
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnConnect.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnConnect.Location = new System.Drawing.Point(24, 24);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(193, 81);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = false;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.BackColor = System.Drawing.Color.Black;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblInfo.Location = new System.Drawing.Point(377, 199);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(262, 163);
            this.lblInfo.TabIndex = 5;
            this.lblInfo.Text = "lab";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkCyan;
            this.ClientSize = new System.Drawing.Size(1200, 632);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.lstUsers);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMess);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMess;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ListBox lstUsers;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label lblInfo;
    }
}

