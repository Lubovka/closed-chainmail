namespace WindowsFormsApp6
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DiamParticle = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.DiamCircle = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.warningLabel = new System.Windows.Forms.Label();
            this.clusterLengthField = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chainSizeField = new System.Windows.Forms.TextBox();
            this.dotsParamField = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.timeBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.GrayText;
            this.groupBox1.Controls.Add(this.timeBox);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.DiamParticle);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.startButton);
            this.groupBox1.Controls.Add(this.DiamCircle);
            this.groupBox1.Controls.Add(this.dotsParamField);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.chainSizeField);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.warningLabel);
            this.groupBox1.Controls.Add(this.clusterLengthField);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.Window;
            this.groupBox1.Location = new System.Drawing.Point(39, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(341, 647);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Задаваемые параметры";
            // 
            // DiamParticle
            // 
            this.DiamParticle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DiamParticle.Location = new System.Drawing.Point(105, 415);
            this.DiamParticle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DiamParticle.Name = "DiamParticle";
            this.DiamParticle.Size = new System.Drawing.Size(133, 35);
            this.DiamParticle.TabIndex = 28;
            this.DiamParticle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(67, 380);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(189, 29);
            this.label7.TabIndex = 27;
            this.label7.Text = "Диаметр точки";
            // 
            // DiamCircle
            // 
            this.DiamCircle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DiamCircle.Location = new System.Drawing.Point(106, 340);
            this.DiamCircle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DiamCircle.Name = "DiamCircle";
            this.DiamCircle.Size = new System.Drawing.Size(133, 35);
            this.DiamCircle.TabIndex = 26;
            this.DiamCircle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(68, 305);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(188, 29);
            this.label6.TabIndex = 25;
            this.label6.Text = "Диаметр круга";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(214, 151);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 29);
            this.label5.TabIndex = 24;
            this.label5.Text = "2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(100, 72);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 29);
            this.label4.TabIndex = 23;
            this.label4.Text = "4 x";
            // 
            // warningLabel
            // 
            this.warningLabel.AutoSize = true;
            this.warningLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.warningLabel.Location = new System.Drawing.Point(14, 607);
            this.warningLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(317, 33);
            this.warningLabel.TabIndex = 22;
            this.warningLabel.Text = "Введите целые числа!";
            this.warningLabel.Visible = false;
            // 
            // clusterLengthField
            // 
            this.clusterLengthField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clusterLengthField.Location = new System.Drawing.Point(106, 249);
            this.clusterLengthField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.clusterLengthField.Name = "clusterLengthField";
            this.clusterLengthField.Size = new System.Drawing.Size(133, 35);
            this.clusterLengthField.TabIndex = 21;
            this.clusterLengthField.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(68, 214);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(202, 29);
            this.label3.TabIndex = 20;
            this.label3.Text = "Длина кластера";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(66, 120);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 29);
            this.label2.TabIndex = 19;
            this.label2.Text = "Размер кольчуги";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(50, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(236, 29);
            this.label1.TabIndex = 18;
            this.label1.Text = "Количество частиц";
            // 
            // chainSizeField
            // 
            this.chainSizeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chainSizeField.Location = new System.Drawing.Point(110, 169);
            this.chainSizeField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chainSizeField.Name = "chainSizeField";
            this.chainSizeField.Size = new System.Drawing.Size(92, 35);
            this.chainSizeField.TabIndex = 17;
            this.chainSizeField.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dotsParamField
            // 
            this.dotsParamField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dotsParamField.Location = new System.Drawing.Point(146, 68);
            this.dotsParamField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dotsParamField.Name = "dotsParamField";
            this.dotsParamField.Size = new System.Drawing.Size(92, 35);
            this.dotsParamField.TabIndex = 16;
            this.dotsParamField.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // startButton
            // 
            this.startButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.startButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.startButton.Location = new System.Drawing.Point(58, 550);
            this.startButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(212, 52);
            this.startButton.TabIndex = 15;
            this.startButton.Text = "Старт";
            this.startButton.UseVisualStyleBackColor = false;
            this.startButton.Click += new System.EventHandler(this.startButton_Click_1);
            // 
            // timeBox
            // 
            this.timeBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.timeBox.Location = new System.Drawing.Point(105, 500);
            this.timeBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.timeBox.Name = "timeBox";
            this.timeBox.Size = new System.Drawing.Size(133, 35);
            this.timeBox.TabIndex = 30;
            this.timeBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(78, 466);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(163, 29);
            this.label8.TabIndex = 29;
            this.label8.Text = "Тик времени";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1028, 703);
            this.Controls.Add(this.groupBox1);
            this.Cursor = System.Windows.Forms.Cursors.PanNW;
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Opacity = 0.99D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Иммитационная модель";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox DiamParticle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.TextBox DiamCircle;
        private System.Windows.Forms.TextBox dotsParamField;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox chainSizeField;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label warningLabel;
        private System.Windows.Forms.TextBox clusterLengthField;
        private System.Windows.Forms.TextBox timeBox;
        private System.Windows.Forms.Label label8;
    }
}

