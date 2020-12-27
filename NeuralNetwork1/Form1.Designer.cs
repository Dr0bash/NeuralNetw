namespace NeuralNetwork1
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
            System.Windows.Forms.Label label7;
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.netTrainButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.infoStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.elapsedTimeLabel = new System.Windows.Forms.Label();
            this.вапрвапрToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.AIMLOutput = new System.Windows.Forms.TextBox();
            this.AIMLInput = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.TLGBotOnButton = new System.Windows.Forms.Button();
            this.TLGUsersMessages = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            label7.Location = new System.Drawing.Point(597, 89);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(44, 15);
            label7.TabIndex = 14;
            label7.Text = "Status:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(9, 10);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(500, 500);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(514, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Sample text";
            // 
            // netTrainButton
            // 
            this.netTrainButton.Location = new System.Drawing.Point(655, 34);
            this.netTrainButton.Name = "netTrainButton";
            this.netTrainButton.Size = new System.Drawing.Size(100, 30);
            this.netTrainButton.TabIndex = 11;
            this.netTrainButton.Text = "Обучить";
            this.netTrainButton.UseVisualStyleBackColor = true;
            this.netTrainButton.Click += new System.EventHandler(this.button1_Click);
            this.netTrainButton.MouseEnter += new System.EventHandler(this.netTrainButton_MouseEnter);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(514, 124);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(291, 22);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 10;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 519);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1182, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // infoStatusLabel
            // 
            this.infoStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.infoStatusLabel.Name = "infoStatusLabel";
            this.infoStatusLabel.Size = new System.Drawing.Size(80, 17);
            this.infoStatusLabel.Text = "Обучите сеть";
            this.infoStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // elapsedTimeLabel
            // 
            this.elapsedTimeLabel.AutoSize = true;
            this.elapsedTimeLabel.Location = new System.Drawing.Point(519, 149);
            this.elapsedTimeLabel.Name = "elapsedTimeLabel";
            this.elapsedTimeLabel.Size = new System.Drawing.Size(43, 13);
            this.elapsedTimeLabel.TabIndex = 12;
            this.elapsedTimeLabel.Text = "Время:";
            // 
            // вапрвапрToolStripMenuItem
            // 
            this.вапрвапрToolStripMenuItem.Name = "вапрвапрToolStripMenuItem";
            this.вапрвапрToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.вапрвапрToolStripMenuItem.Text = "вапрвапр";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.StatusLabel.Location = new System.Drawing.Point(640, 89);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(42, 15);
            this.StatusLabel.TabIndex = 15;
            this.StatusLabel.Text = "NONE";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.AIMLOutput);
            this.groupBox2.Controls.Add(this.AIMLInput);
            this.groupBox2.Location = new System.Drawing.Point(814, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(356, 184);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "AIML чат-бот";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(219, 58);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 34);
            this.button1.TabIndex = 2;
            this.button1.Text = "Обработать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // AIMLOutput
            // 
            this.AIMLOutput.Location = new System.Drawing.Point(13, 101);
            this.AIMLOutput.Multiline = true;
            this.AIMLOutput.Name = "AIMLOutput";
            this.AIMLOutput.Size = new System.Drawing.Size(328, 77);
            this.AIMLOutput.TabIndex = 1;
            // 
            // AIMLInput
            // 
            this.AIMLInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AIMLInput.Location = new System.Drawing.Point(11, 23);
            this.AIMLInput.Name = "AIMLInput";
            this.AIMLInput.Size = new System.Drawing.Size(330, 29);
            this.AIMLInput.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.TLGBotOnButton);
            this.groupBox3.Controls.Add(this.TLGUsersMessages);
            this.groupBox3.Location = new System.Drawing.Point(814, 201);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(356, 309);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Telegram";
            // 
            // TLGBotOnButton
            // 
            this.TLGBotOnButton.Location = new System.Drawing.Point(219, 19);
            this.TLGBotOnButton.Name = "TLGBotOnButton";
            this.TLGBotOnButton.Size = new System.Drawing.Size(122, 34);
            this.TLGBotOnButton.TabIndex = 1;
            this.TLGBotOnButton.Text = "BotOn";
            this.TLGBotOnButton.UseVisualStyleBackColor = true;
            this.TLGBotOnButton.Click += new System.EventHandler(this.TLGBotOnButton_Click);
            // 
            // TLGUsersMessages
            // 
            this.TLGUsersMessages.Location = new System.Drawing.Point(6, 66);
            this.TLGUsersMessages.Multiline = true;
            this.TLGUsersMessages.Name = "TLGUsersMessages";
            this.TLGUsersMessages.Size = new System.Drawing.Size(344, 237);
            this.TLGUsersMessages.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(522, 34);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(92, 30);
            this.button2.TabIndex = 18;
            this.button2.Text = "Загрузить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 541);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.netTrainButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(label7);
            this.Controls.Add(this.elapsedTimeLabel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Оригинальный студенческий перспетрон";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel infoStatusLabel;
        private System.Windows.Forms.Label elapsedTimeLabel;
        private System.Windows.Forms.Button netTrainButton;
        private System.Windows.Forms.ToolStripMenuItem вапрвапрToolStripMenuItem;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox AIMLOutput;
        private System.Windows.Forms.TextBox AIMLInput;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button TLGBotOnButton;
        private System.Windows.Forms.TextBox TLGUsersMessages;
        private System.Windows.Forms.Button button2;
    }
}

