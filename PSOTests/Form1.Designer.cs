namespace PSOTests
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
            this.FunctionSelectionCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ParticleQuantityUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.MaxEpochUpDown = new System.Windows.Forms.NumericUpDown();
            this.StartBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ilgraf = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.resetB = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ParticleQuantityUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxEpochUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // FunctionSelectionCombo
            // 
            this.FunctionSelectionCombo.FormattingEnabled = true;
            this.FunctionSelectionCombo.Items.AddRange(new object[] {
            "2*x^2+x-2",
            "x^2+sin(3 cos(5x))",
            "x^4+x^3-7x^2-5x+10",
            "sin(2 x)+log(x^2)",
            "|(log(x^2)|"});
            this.FunctionSelectionCombo.Location = new System.Drawing.Point(219, 155);
            this.FunctionSelectionCombo.Name = "FunctionSelectionCombo";
            this.FunctionSelectionCombo.Size = new System.Drawing.Size(217, 21);
            this.FunctionSelectionCombo.TabIndex = 4;
            this.FunctionSelectionCombo.Text = "Proszę wybrać funkcję do optymalizacji";
            this.FunctionSelectionCombo.SelectedIndexChanged += new System.EventHandler(this.FunctionSelectionCombo_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(91, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Funkcja do optymalizacji";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // ParticleQuantityUpDown
            // 
            this.ParticleQuantityUpDown.Location = new System.Drawing.Point(94, 62);
            this.ParticleQuantityUpDown.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ParticleQuantityUpDown.Name = "ParticleQuantityUpDown";
            this.ParticleQuantityUpDown.Size = new System.Drawing.Size(49, 20);
            this.ParticleQuantityUpDown.TabIndex = 6;
            this.ParticleQuantityUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ParticleQuantityUpDown.ValueChanged += new System.EventHandler(this.ParticleQuantityUpDown_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Ilość cząstek";
            // 
            // MaxEpochUpDown
            // 
            this.MaxEpochUpDown.Location = new System.Drawing.Point(484, 62);
            this.MaxEpochUpDown.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.MaxEpochUpDown.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.MaxEpochUpDown.Name = "MaxEpochUpDown";
            this.MaxEpochUpDown.Size = new System.Drawing.Size(52, 20);
            this.MaxEpochUpDown.TabIndex = 8;
            this.MaxEpochUpDown.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.MaxEpochUpDown.ValueChanged += new System.EventHandler(this.MaxEpochUpDown_ValueChanged);
            // 
            // StartBtn
            // 
            this.StartBtn.Location = new System.Drawing.Point(160, 237);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(75, 23);
            this.StartBtn.TabIndex = 9;
            this.StartBtn.Text = "Start";
            this.StartBtn.UseVisualStyleBackColor = true;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(322, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Maksymalna ilość iteracji";
            // 
            // ilgraf
            // 
            this.ilgraf.AutoScroll = true;
            this.ilgraf.AutoSize = true;
            this.ilgraf.BackColor = System.Drawing.Color.White;
            this.ilgraf.Location = new System.Drawing.Point(16, 287);
            this.ilgraf.Name = "ilgraf";
            this.ilgraf.Size = new System.Drawing.Size(474, 295);
            this.ilgraf.TabIndex = 11;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(508, 287);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(344, 295);
            this.richTextBox1.TabIndex = 12;
            this.richTextBox1.Text = "";
            // 
            // resetB
            // 
            this.resetB.Enabled = false;
            this.resetB.Location = new System.Drawing.Point(325, 237);
            this.resetB.Name = "resetB";
            this.resetB.Size = new System.Drawing.Size(75, 23);
            this.resetB.TabIndex = 13;
            this.resetB.Text = "Reset";
            this.resetB.UseVisualStyleBackColor = true;
            this.resetB.Click += new System.EventHandler(this.resetB_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 594);
            this.Controls.Add(this.resetB);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.ilgraf);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.StartBtn);
            this.Controls.Add(this.MaxEpochUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ParticleQuantityUpDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FunctionSelectionCombo);
            this.Name = "Form1";
            this.Text = "Optymalizacja metodą roju cząstek";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ParticleQuantityUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxEpochUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox FunctionSelectionCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown ParticleQuantityUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown MaxEpochUpDown;
        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel ilgraf;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button resetB;
    }
}

