﻿using System;

namespace TechnikiOptymalizacjiAG
{
    partial class TechnikiOptymalizacjiAGMainWindow
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
            this.components = new System.ComponentModel.Container();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.IterationThresholdRadioBtn = new System.Windows.Forms.RadioButton();
            this.TimeThresholdRadioBtn = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.IterationThresholdUpDown = new System.Windows.Forms.NumericUpDown();
            this.TimeThresholdUpDown = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.StartBtn = new System.Windows.Forms.Button();
            this.FunctionSelectionCombo = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Reset = new System.Windows.Forms.Button();
            this.groupBoxAG = new System.Windows.Forms.GroupBox();
            this.MutationProbTrackbar = new System.Windows.Forms.TrackBar();
            this.label8 = new System.Windows.Forms.Label();
            this.CrossingCombo = new System.Windows.Forms.ComboBox();
            this.SelectionCombo = new System.Windows.Forms.ComboBox();
            this.PopulationMaxUpDown = new System.Windows.Forms.NumericUpDown();
            this.PopulationMinUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxPSO = new System.Windows.Forms.GroupBox();
            this.MaxEpochUpDown = new System.Windows.Forms.NumericUpDown();
            this.ParticleQuantityUpDown = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.richTextAlGenet = new System.Windows.Forms.RichTextBox();
            this.richTextPSO = new System.Windows.Forms.RichTextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IterationThresholdUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeThresholdUpDown)).BeginInit();
            this.groupBoxAG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MutationProbTrackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopulationMaxUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopulationMinUpDown)).BeginInit();
            this.groupBoxPSO.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxEpochUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParticleQuantityUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.StartBtn);
            this.groupBox2.Controls.Add(this.FunctionSelectionCombo);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.Reset);
            this.groupBox2.Location = new System.Drawing.Point(21, 274);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(693, 118);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Warunki Funkcji";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.IterationThresholdRadioBtn);
            this.groupBox3.Controls.Add(this.TimeThresholdRadioBtn);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.IterationThresholdUpDown);
            this.groupBox3.Controls.Add(this.TimeThresholdUpDown);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(385, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(294, 81);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Warunek końca";
            // 
            // IterationThresholdRadioBtn
            // 
            this.IterationThresholdRadioBtn.AutoSize = true;
            this.IterationThresholdRadioBtn.Location = new System.Drawing.Point(159, 50);
            this.IterationThresholdRadioBtn.Name = "IterationThresholdRadioBtn";
            this.IterationThresholdRadioBtn.Size = new System.Drawing.Size(33, 17);
            this.IterationThresholdRadioBtn.TabIndex = 7;
            this.IterationThresholdRadioBtn.TabStop = true;
            this.IterationThresholdRadioBtn.Text = "i=";
            this.IterationThresholdRadioBtn.UseVisualStyleBackColor = true;
            this.IterationThresholdRadioBtn.CheckedChanged += new System.EventHandler(this.IterationThresholdRadioBtn_CheckedChanged);
            // 
            // TimeThresholdRadioBtn
            // 
            this.TimeThresholdRadioBtn.AutoSize = true;
            this.TimeThresholdRadioBtn.Location = new System.Drawing.Point(17, 50);
            this.TimeThresholdRadioBtn.Name = "TimeThresholdRadioBtn";
            this.TimeThresholdRadioBtn.Size = new System.Drawing.Size(34, 17);
            this.TimeThresholdRadioBtn.TabIndex = 6;
            this.TimeThresholdRadioBtn.TabStop = true;
            this.TimeThresholdRadioBtn.Text = "t=";
            this.TimeThresholdRadioBtn.UseVisualStyleBackColor = true;
            this.TimeThresholdRadioBtn.CheckedChanged += new System.EventHandler(this.TimeThresholdRadioBtn_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(35, 50);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(0, 13);
            this.label11.TabIndex = 4;
            // 
            // IterationThresholdUpDown
            // 
            this.IterationThresholdUpDown.Location = new System.Drawing.Point(198, 50);
            this.IterationThresholdUpDown.Name = "IterationThresholdUpDown";
            this.IterationThresholdUpDown.Size = new System.Drawing.Size(71, 20);
            this.IterationThresholdUpDown.TabIndex = 3;
            this.IterationThresholdUpDown.ValueChanged += new System.EventHandler(this.IterationThresholdUpDown_ValueChanged);
            // 
            // TimeThresholdUpDown
            // 
            this.TimeThresholdUpDown.Location = new System.Drawing.Point(57, 48);
            this.TimeThresholdUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.TimeThresholdUpDown.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.TimeThresholdUpDown.Name = "TimeThresholdUpDown";
            this.TimeThresholdUpDown.Size = new System.Drawing.Size(64, 20);
            this.TimeThresholdUpDown.TabIndex = 2;
            this.TimeThresholdUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.TimeThresholdUpDown.ValueChanged += new System.EventHandler(this.TimeThresholdUpDown_ValueChanged);
            this.TimeThresholdUpDown.Click += new System.EventHandler(this.TimeThresholdUpDown_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(195, 19);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Iteracja";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(37, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Timer [sekundy]";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // StartBtn
            // 
            this.StartBtn.Location = new System.Drawing.Point(110, 78);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(75, 23);
            this.StartBtn.TabIndex = 1;
            this.StartBtn.Text = "Start";
            this.StartBtn.UseVisualStyleBackColor = true;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // FunctionSelectionCombo
            // 
            this.FunctionSelectionCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FunctionSelectionCombo.FormattingEnabled = true;
            this.FunctionSelectionCombo.Items.AddRange(new object[] {
            "De Jong1",
            "Rosenbrock",
            "Rastrigin",
            "Schwefel"});
            this.FunctionSelectionCombo.Location = new System.Drawing.Point(73, 27);
            this.FunctionSelectionCombo.Name = "FunctionSelectionCombo";
            this.FunctionSelectionCombo.Size = new System.Drawing.Size(222, 21);
            this.FunctionSelectionCombo.TabIndex = 3;
            this.FunctionSelectionCombo.SelectedIndexChanged += new System.EventHandler(this.FunctionSelectionCombo_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(36, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "f(x,y) :";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // Reset
            // 
            this.Reset.Location = new System.Drawing.Point(220, 78);
            this.Reset.Name = "Reset";
            this.Reset.Size = new System.Drawing.Size(75, 23);
            this.Reset.TabIndex = 4;
            this.Reset.Text = "Reset";
            this.Reset.UseVisualStyleBackColor = true;
            this.Reset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // groupBoxAG
            // 
            this.groupBoxAG.Controls.Add(this.MutationProbTrackbar);
            this.groupBoxAG.Controls.Add(this.label8);
            this.groupBoxAG.Controls.Add(this.CrossingCombo);
            this.groupBoxAG.Controls.Add(this.SelectionCombo);
            this.groupBoxAG.Controls.Add(this.PopulationMaxUpDown);
            this.groupBoxAG.Controls.Add(this.PopulationMinUpDown);
            this.groupBoxAG.Controls.Add(this.label6);
            this.groupBoxAG.Controls.Add(this.label5);
            this.groupBoxAG.Controls.Add(this.label4);
            this.groupBoxAG.Controls.Add(this.label3);
            this.groupBoxAG.Controls.Add(this.label2);
            this.groupBoxAG.Controls.Add(this.label1);
            this.groupBoxAG.Location = new System.Drawing.Point(21, 21);
            this.groupBoxAG.Name = "groupBoxAG";
            this.groupBoxAG.Size = new System.Drawing.Size(343, 247);
            this.groupBoxAG.TabIndex = 6;
            this.groupBoxAG.TabStop = false;
            this.groupBoxAG.Text = "Algorytm Genetyczny";
            // 
            // MutationProbTrackbar
            // 
            this.MutationProbTrackbar.Location = new System.Drawing.Point(90, 199);
            this.MutationProbTrackbar.Maximum = 75;
            this.MutationProbTrackbar.Minimum = 1;
            this.MutationProbTrackbar.Name = "MutationProbTrackbar";
            this.MutationProbTrackbar.Size = new System.Drawing.Size(127, 45);
            this.MutationProbTrackbar.TabIndex = 15;
            this.MutationProbTrackbar.Value = 1;
            this.MutationProbTrackbar.Scroll += new System.EventHandler(this.MutationProbTrackbar_Scroll);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 152);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 13);
            this.label8.TabIndex = 14;
            // 
            // CrossingCombo
            // 
            this.CrossingCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CrossingCombo.FormattingEnabled = true;
            this.CrossingCombo.Location = new System.Drawing.Point(56, 144);
            this.CrossingCombo.Name = "CrossingCombo";
            this.CrossingCombo.Size = new System.Drawing.Size(239, 21);
            this.CrossingCombo.TabIndex = 12;
            this.CrossingCombo.SelectedIndexChanged += new System.EventHandler(this.CrossingCombo_SelectedIndexChanged);
            // 
            // SelectionCombo
            // 
            this.SelectionCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectionCombo.FormattingEnabled = true;
            this.SelectionCombo.Items.AddRange(new object[] {
            "Ruletka",
            "Turniej"});
            this.SelectionCombo.Location = new System.Drawing.Point(56, 81);
            this.SelectionCombo.Name = "SelectionCombo";
            this.SelectionCombo.Size = new System.Drawing.Size(239, 21);
            this.SelectionCombo.TabIndex = 11;
            this.SelectionCombo.SelectedIndexChanged += new System.EventHandler(this.SelectionCombo_SelectedIndexChanged);
            // 
            // PopulationMaxUpDown
            // 
            this.PopulationMaxUpDown.Location = new System.Drawing.Point(235, 34);
            this.PopulationMaxUpDown.Minimum = new decimal(new int[] {
            45,
            0,
            0,
            0});
            this.PopulationMaxUpDown.Name = "PopulationMaxUpDown";
            this.PopulationMaxUpDown.Size = new System.Drawing.Size(60, 20);
            this.PopulationMaxUpDown.TabIndex = 7;
            this.PopulationMaxUpDown.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
            this.PopulationMaxUpDown.ValueChanged += new System.EventHandler(this.PopulationMaxUpDown_ValueChanged);
            // 
            // PopulationMinUpDown
            // 
            this.PopulationMinUpDown.Location = new System.Drawing.Point(71, 34);
            this.PopulationMinUpDown.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.PopulationMinUpDown.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.PopulationMinUpDown.Name = "PopulationMinUpDown";
            this.PopulationMinUpDown.Size = new System.Drawing.Size(61, 20);
            this.PopulationMinUpDown.TabIndex = 6;
            this.PopulationMinUpDown.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.PopulationMinUpDown.ValueChanged += new System.EventHandler(this.PopulationMinUpDown_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(193, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Max";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Min";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(126, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Mutacja";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(126, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Krzyżowanie";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(132, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Selekcja";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(132, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Populacja";
            // 
            // groupBoxPSO
            // 
            this.groupBoxPSO.Controls.Add(this.MaxEpochUpDown);
            this.groupBoxPSO.Controls.Add(this.ParticleQuantityUpDown);
            this.groupBoxPSO.Controls.Add(this.label13);
            this.groupBoxPSO.Controls.Add(this.label12);
            this.groupBoxPSO.Location = new System.Drawing.Point(376, 22);
            this.groupBoxPSO.Name = "groupBoxPSO";
            this.groupBoxPSO.Size = new System.Drawing.Size(338, 246);
            this.groupBoxPSO.TabIndex = 8;
            this.groupBoxPSO.TabStop = false;
            this.groupBoxPSO.Text = "PSO";
            // 
            // MaxEpochUpDown
            // 
            this.MaxEpochUpDown.Location = new System.Drawing.Point(228, 110);
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
            this.MaxEpochUpDown.Size = new System.Drawing.Size(71, 20);
            this.MaxEpochUpDown.TabIndex = 13;
            this.MaxEpochUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.MaxEpochUpDown.ValueChanged += new System.EventHandler(this.MaxEpochUpDown_ValueChanged);
            // 
            // ParticleQuantityUpDown
            // 
            this.ParticleQuantityUpDown.Location = new System.Drawing.Point(228, 62);
            this.ParticleQuantityUpDown.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ParticleQuantityUpDown.Name = "ParticleQuantityUpDown";
            this.ParticleQuantityUpDown.Size = new System.Drawing.Size(71, 20);
            this.ParticleQuantityUpDown.TabIndex = 12;
            this.ParticleQuantityUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ParticleQuantityUpDown.ValueChanged += new System.EventHandler(this.ParticleQuantityUpDown_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(25, 110);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(123, 13);
            this.label13.TabIndex = 11;
            this.label13.Text = "Maksymalna ilość iteracji";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(25, 61);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(69, 13);
            this.label12.TabIndex = 8;
            this.label12.Text = "Ilość cząstek";
            // 
            // richTextAlGenet
            // 
            this.richTextAlGenet.Location = new System.Drawing.Point(21, 399);
            this.richTextAlGenet.Name = "richTextAlGenet";
            this.richTextAlGenet.ReadOnly = true;
            this.richTextAlGenet.Size = new System.Drawing.Size(343, 220);
            this.richTextAlGenet.TabIndex = 9;
            this.richTextAlGenet.Text = "";
            // 
            // richTextPSO
            // 
            this.richTextPSO.Location = new System.Drawing.Point(383, 399);
            this.richTextPSO.Name = "richTextPSO";
            this.richTextPSO.ReadOnly = true;
            this.richTextPSO.Size = new System.Drawing.Size(338, 220);
            this.richTextPSO.TabIndex = 0;
            this.richTextPSO.Text = "";
            // 
            // TechnikiOptymalizacjiAGMainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 644);
            this.Controls.Add(this.richTextPSO);
            this.Controls.Add(this.richTextAlGenet);
            this.Controls.Add(this.groupBoxPSO);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBoxAG);
            this.Name = "TechnikiOptymalizacjiAGMainWindow";
            this.Text = "Techniki Optymalizacji z użyciem Algorytmów Genetycznych";
            this.Load += new System.EventHandler(this.TechnikiOptymalizacjiAGMainWindow_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IterationThresholdUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeThresholdUpDown)).EndInit();
            this.groupBoxAG.ResumeLayout(false);
            this.groupBoxAG.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MutationProbTrackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopulationMaxUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopulationMinUpDown)).EndInit();
            this.groupBoxPSO.ResumeLayout(false);
            this.groupBoxPSO.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxEpochUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParticleQuantityUpDown)).EndInit();
            this.ResumeLayout(false);

        }

       



        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.ComboBox FunctionSelectionCombo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button Reset;
        private System.Windows.Forms.GroupBox groupBoxAG;
        private System.Windows.Forms.TrackBar MutationProbTrackbar;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox CrossingCombo;
        private System.Windows.Forms.ComboBox SelectionCombo;
        private System.Windows.Forms.NumericUpDown PopulationMaxUpDown;
        private System.Windows.Forms.NumericUpDown PopulationMinUpDown;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton IterationThresholdRadioBtn;
        private System.Windows.Forms.RadioButton TimeThresholdRadioBtn;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown IterationThresholdUpDown;
        private System.Windows.Forms.NumericUpDown TimeThresholdUpDown;
        private System.Windows.Forms.GroupBox groupBoxPSO;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown MaxEpochUpDown;
        private System.Windows.Forms.NumericUpDown ParticleQuantityUpDown;
        private System.Windows.Forms.RichTextBox richTextAlGenet;
        private System.Windows.Forms.RichTextBox richTextPSO;
        private System.Windows.Forms.ToolTip toolTip1;
        //private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

