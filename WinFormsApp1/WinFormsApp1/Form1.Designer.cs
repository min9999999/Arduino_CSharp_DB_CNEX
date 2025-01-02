namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            button3 = new Button();
            label2 = new Label();
            button4 = new Button();
            button1 = new Button();
            comboBox1 = new ComboBox();
            button2 = new Button();
            label3 = new Label();
            messageLabel = new Label();
            button5 = new Button();
            pictureBox1 = new PictureBox();
            textBox1 = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(89, 30);
            label1.Name = "label1";
            label1.Size = new Size(93, 15);
            label1.TabIndex = 1;
            label1.Text = "서버연결 on/off";
            // 
            // button3
            // 
            button3.Location = new Point(19, 204);
            button3.Name = "button3";
            button3.Size = new Size(91, 33);
            button3.TabIndex = 5;
            button3.Text = "ON";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(89, 165);
            label2.Name = "label2";
            label2.Size = new Size(69, 15);
            label2.TabIndex = 4;
            label2.Text = "전구 on/off";
            // 
            // button4
            // 
            button4.Location = new Point(130, 204);
            button4.Name = "button4";
            button4.Size = new Size(91, 33);
            button4.TabIndex = 3;
            button4.Text = "OFF";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button1
            // 
            button1.Location = new Point(153, 57);
            button1.Name = "button1";
            button1.Size = new Size(91, 33);
            button1.TabIndex = 6;
            button1.Text = "Connect";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(26, 63);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 7;
            comboBox1.Click += comboBox1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(79, 459);
            button2.Name = "button2";
            button2.Size = new Size(91, 33);
            button2.TabIndex = 8;
            button2.Text = "Export";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(93, 423);
            label3.Name = "label3";
            label3.Size = new Size(77, 15);
            label3.TabIndex = 9;
            label3.Text = "CSV_EXPORT";
            // 
            // messageLabel
            // 
            messageLabel.AutoSize = true;
            messageLabel.Location = new Point(26, 101);
            messageLabel.Name = "messageLabel";
            messageLabel.Size = new Size(53, 15);
            messageLabel.TabIndex = 10;
            messageLabel.Text = "Message";
            // 
            // button5
            // 
            button5.Location = new Point(300, 57);
            button5.Name = "button5";
            button5.Size = new Size(91, 33);
            button5.TabIndex = 11;
            button5.Text = "Unity Start";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(300, 101);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(597, 322);
            pictureBox1.TabIndex = 12;
            pictureBox1.TabStop = false;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(19, 243);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(202, 150);
            textBox1.TabIndex = 13;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(930, 610);
            Controls.Add(textBox1);
            Controls.Add(pictureBox1);
            Controls.Add(button5);
            Controls.Add(messageLabel);
            Controls.Add(label3);
            Controls.Add(button2);
            Controls.Add(comboBox1);
            Controls.Add(button1);
            Controls.Add(button3);
            Controls.Add(label2);
            Controls.Add(button4);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Button button3;
        private Label label2;
        private Button button4;
        private Button button1;
        private ComboBox comboBox1;
        private Button button2;
        private Label label3;
        public Label messageLabel;
        private Button button5;
        private PictureBox pictureBox1;
        private TextBox textBox1;
    }
}
