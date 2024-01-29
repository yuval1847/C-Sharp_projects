namespace Lesson_1_v3_calculator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btn_1 = new System.Windows.Forms.Button();
            this.btn_2 = new System.Windows.Forms.Button();
            this.btn_3 = new System.Windows.Forms.Button();
            this.btn_0 = new System.Windows.Forms.Button();
            this.btn_6 = new System.Windows.Forms.Button();
            this.btn_5 = new System.Windows.Forms.Button();
            this.btn_4 = new System.Windows.Forms.Button();
            this.btn_9 = new System.Windows.Forms.Button();
            this.btn_8 = new System.Windows.Forms.Button();
            this.btn_7 = new System.Windows.Forms.Button();
            this.result_btn = new System.Windows.Forms.Button();
            this.plus_btn = new System.Windows.Forms.Button();
            this.minus_btn = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_result = new System.Windows.Forms.Label();
            this.btn_clear = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_1
            // 
            this.btn_1.BackColor = System.Drawing.Color.MidnightBlue;
            this.btn_1.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btn_1.ForeColor = System.Drawing.Color.White;
            this.btn_1.Location = new System.Drawing.Point(12, 414);
            this.btn_1.Name = "btn_1";
            this.btn_1.Size = new System.Drawing.Size(128, 93);
            this.btn_1.TabIndex = 0;
            this.btn_1.Text = "1";
            this.btn_1.UseVisualStyleBackColor = false;
            this.btn_1.Click += new System.EventHandler(this.btn_1_Click);
            // 
            // btn_2
            // 
            this.btn_2.BackColor = System.Drawing.Color.MidnightBlue;
            this.btn_2.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btn_2.ForeColor = System.Drawing.Color.White;
            this.btn_2.Location = new System.Drawing.Point(146, 414);
            this.btn_2.Name = "btn_2";
            this.btn_2.Size = new System.Drawing.Size(128, 93);
            this.btn_2.TabIndex = 1;
            this.btn_2.Text = "2\r\n";
            this.btn_2.UseVisualStyleBackColor = false;
            this.btn_2.Click += new System.EventHandler(this.btn_2_Click);
            // 
            // btn_3
            // 
            this.btn_3.BackColor = System.Drawing.Color.MidnightBlue;
            this.btn_3.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btn_3.ForeColor = System.Drawing.Color.White;
            this.btn_3.Location = new System.Drawing.Point(280, 414);
            this.btn_3.Name = "btn_3";
            this.btn_3.Size = new System.Drawing.Size(128, 93);
            this.btn_3.TabIndex = 2;
            this.btn_3.Text = "3";
            this.btn_3.UseVisualStyleBackColor = false;
            this.btn_3.Click += new System.EventHandler(this.btn_3_Click);
            // 
            // btn_0
            // 
            this.btn_0.BackColor = System.Drawing.Color.MidnightBlue;
            this.btn_0.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btn_0.ForeColor = System.Drawing.Color.White;
            this.btn_0.Location = new System.Drawing.Point(12, 513);
            this.btn_0.Name = "btn_0";
            this.btn_0.Size = new System.Drawing.Size(396, 93);
            this.btn_0.TabIndex = 3;
            this.btn_0.Text = "0";
            this.btn_0.UseVisualStyleBackColor = false;
            this.btn_0.Click += new System.EventHandler(this.btn_0_Click);
            // 
            // btn_6
            // 
            this.btn_6.BackColor = System.Drawing.Color.MidnightBlue;
            this.btn_6.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btn_6.ForeColor = System.Drawing.Color.White;
            this.btn_6.Location = new System.Drawing.Point(280, 315);
            this.btn_6.Name = "btn_6";
            this.btn_6.Size = new System.Drawing.Size(128, 93);
            this.btn_6.TabIndex = 6;
            this.btn_6.Text = "6";
            this.btn_6.UseVisualStyleBackColor = false;
            this.btn_6.Click += new System.EventHandler(this.btn_6_Click);
            // 
            // btn_5
            // 
            this.btn_5.BackColor = System.Drawing.Color.MidnightBlue;
            this.btn_5.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btn_5.ForeColor = System.Drawing.Color.White;
            this.btn_5.Location = new System.Drawing.Point(146, 315);
            this.btn_5.Name = "btn_5";
            this.btn_5.Size = new System.Drawing.Size(128, 93);
            this.btn_5.TabIndex = 5;
            this.btn_5.Text = "5";
            this.btn_5.UseVisualStyleBackColor = false;
            this.btn_5.Click += new System.EventHandler(this.btn_5_Click);
            // 
            // btn_4
            // 
            this.btn_4.BackColor = System.Drawing.Color.MidnightBlue;
            this.btn_4.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btn_4.ForeColor = System.Drawing.Color.White;
            this.btn_4.Location = new System.Drawing.Point(12, 315);
            this.btn_4.Name = "btn_4";
            this.btn_4.Size = new System.Drawing.Size(128, 93);
            this.btn_4.TabIndex = 4;
            this.btn_4.Text = "4";
            this.btn_4.UseVisualStyleBackColor = false;
            this.btn_4.Click += new System.EventHandler(this.btn_4_Click);
            // 
            // btn_9
            // 
            this.btn_9.BackColor = System.Drawing.Color.MidnightBlue;
            this.btn_9.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btn_9.ForeColor = System.Drawing.Color.White;
            this.btn_9.Location = new System.Drawing.Point(280, 216);
            this.btn_9.Name = "btn_9";
            this.btn_9.Size = new System.Drawing.Size(128, 93);
            this.btn_9.TabIndex = 9;
            this.btn_9.Text = "9";
            this.btn_9.UseVisualStyleBackColor = false;
            this.btn_9.Click += new System.EventHandler(this.btn_9_Click);
            // 
            // btn_8
            // 
            this.btn_8.BackColor = System.Drawing.Color.MidnightBlue;
            this.btn_8.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btn_8.ForeColor = System.Drawing.Color.White;
            this.btn_8.Location = new System.Drawing.Point(146, 216);
            this.btn_8.Name = "btn_8";
            this.btn_8.Size = new System.Drawing.Size(128, 93);
            this.btn_8.TabIndex = 8;
            this.btn_8.Text = "8";
            this.btn_8.UseVisualStyleBackColor = false;
            this.btn_8.Click += new System.EventHandler(this.btn_8_Click);
            // 
            // btn_7
            // 
            this.btn_7.BackColor = System.Drawing.Color.MidnightBlue;
            this.btn_7.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btn_7.ForeColor = System.Drawing.Color.White;
            this.btn_7.Location = new System.Drawing.Point(12, 216);
            this.btn_7.Name = "btn_7";
            this.btn_7.Size = new System.Drawing.Size(128, 93);
            this.btn_7.TabIndex = 7;
            this.btn_7.Text = "7";
            this.btn_7.UseVisualStyleBackColor = false;
            this.btn_7.Click += new System.EventHandler(this.btn_7_Click);
            // 
            // result_btn
            // 
            this.result_btn.BackColor = System.Drawing.Color.MidnightBlue;
            this.result_btn.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.result_btn.ForeColor = System.Drawing.Color.White;
            this.result_btn.Location = new System.Drawing.Point(414, 510);
            this.result_btn.Name = "result_btn";
            this.result_btn.Size = new System.Drawing.Size(128, 93);
            this.result_btn.TabIndex = 10;
            this.result_btn.Text = "=";
            this.result_btn.UseVisualStyleBackColor = false;
            this.result_btn.Click += new System.EventHandler(this.result_btn_Click);
            // 
            // plus_btn
            // 
            this.plus_btn.BackColor = System.Drawing.Color.MidnightBlue;
            this.plus_btn.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.plus_btn.ForeColor = System.Drawing.Color.White;
            this.plus_btn.Location = new System.Drawing.Point(414, 411);
            this.plus_btn.Name = "plus_btn";
            this.plus_btn.Size = new System.Drawing.Size(128, 93);
            this.plus_btn.TabIndex = 11;
            this.plus_btn.Text = "+";
            this.plus_btn.UseCompatibleTextRendering = true;
            this.plus_btn.UseVisualStyleBackColor = false;
            this.plus_btn.Click += new System.EventHandler(this.plus_btn_Click);
            // 
            // minus_btn
            // 
            this.minus_btn.BackColor = System.Drawing.Color.MidnightBlue;
            this.minus_btn.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.minus_btn.ForeColor = System.Drawing.Color.White;
            this.minus_btn.Location = new System.Drawing.Point(414, 315);
            this.minus_btn.Name = "minus_btn";
            this.minus_btn.Size = new System.Drawing.Size(128, 93);
            this.minus_btn.TabIndex = 12;
            this.minus_btn.Text = "-";
            this.minus_btn.UseCompatibleTextRendering = true;
            this.minus_btn.UseVisualStyleBackColor = false;
            this.minus_btn.Click += new System.EventHandler(this.minus_btn_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.MediumBlue;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label_result);
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(530, 189);
            this.panel1.TabIndex = 13;
            // 
            // label_result
            // 
            this.label_result.AutoSize = true;
            this.label_result.Font = new System.Drawing.Font("Segoe MDL2 Assets", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_result.ForeColor = System.Drawing.Color.White;
            this.label_result.Location = new System.Drawing.Point(-25, 0);
            this.label_result.Name = "label_result";
            this.label_result.Size = new System.Drawing.Size(120, 144);
            this.label_result.TabIndex = 0;
            this.label_result.Text = "0";
            this.label_result.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_clear
            // 
            this.btn_clear.BackColor = System.Drawing.Color.MidnightBlue;
            this.btn_clear.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btn_clear.ForeColor = System.Drawing.Color.White;
            this.btn_clear.Location = new System.Drawing.Point(414, 216);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(128, 93);
            this.btn_clear.TabIndex = 14;
            this.btn_clear.Text = "Clear";
            this.btn_clear.UseCompatibleTextRendering = true;
            this.btn_clear.UseVisualStyleBackColor = false;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Navy;
            this.ClientSize = new System.Drawing.Size(551, 615);
            this.Controls.Add(this.btn_clear);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.minus_btn);
            this.Controls.Add(this.plus_btn);
            this.Controls.Add(this.result_btn);
            this.Controls.Add(this.btn_9);
            this.Controls.Add(this.btn_8);
            this.Controls.Add(this.btn_7);
            this.Controls.Add(this.btn_6);
            this.Controls.Add(this.btn_5);
            this.Controls.Add(this.btn_4);
            this.Controls.Add(this.btn_0);
            this.Controls.Add(this.btn_3);
            this.Controls.Add(this.btn_2);
            this.Controls.Add(this.btn_1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Calculator";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_1;
        private System.Windows.Forms.Button btn_2;
        private System.Windows.Forms.Button btn_3;
        private System.Windows.Forms.Button btn_0;
        private System.Windows.Forms.Button btn_6;
        private System.Windows.Forms.Button btn_5;
        private System.Windows.Forms.Button btn_4;
        private System.Windows.Forms.Button btn_9;
        private System.Windows.Forms.Button btn_8;
        private System.Windows.Forms.Button btn_7;
        private System.Windows.Forms.Button result_btn;
        private System.Windows.Forms.Button plus_btn;
        private System.Windows.Forms.Button minus_btn;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_result;
        private System.Windows.Forms.Button btn_clear;
    }
}

