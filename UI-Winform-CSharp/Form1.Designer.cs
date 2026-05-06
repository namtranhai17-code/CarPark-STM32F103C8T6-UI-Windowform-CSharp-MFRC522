namespace MONITOR_Ver1_0
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
            this.components = new System.ComponentModel.Container();
            this.groupBoxCONN = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxCOM = new System.Windows.Forms.ComboBox();
            this.comboBoxBaudRate = new System.Windows.Forms.ComboBox();
            this.buttonReLoad = new System.Windows.Forms.Button();
            this.buttonConn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonCreateCard = new System.Windows.Forms.Button();
            this.buttonMonitor = new System.Windows.Forms.Button();
            this.buttonControlBarieL = new System.Windows.Forms.Button();
            this.buttonControlBarieR = new System.Windows.Forms.Button();
            this.labelRealTime = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonDay = new System.Windows.Forms.Button();
            this.buttonWeek = new System.Windows.Forms.Button();
            this.buttonMonth = new System.Windows.Forms.Button();
            this.timerRealTime = new System.Windows.Forms.Timer(this.components);
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.textBoxTypeCard = new System.Windows.Forms.TextBox();
            this.groupBoxMonitor = new System.Windows.Forms.GroupBox();
            this.buttonConfirm = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxTimeOut = new System.Windows.Forms.TextBox();
            this.textBoxTimeIn = new System.Windows.Forms.TextBox();
            this.textBoxOutdatedTime = new System.Windows.Forms.TextBox();
            this.textBoxPrice = new System.Windows.Forms.TextBox();
            this.textBoxCheckData = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBoxCONN.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBoxMonitor.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxCONN
            // 
            this.groupBoxCONN.Controls.Add(this.label2);
            this.groupBoxCONN.Controls.Add(this.label1);
            this.groupBoxCONN.Controls.Add(this.comboBoxCOM);
            this.groupBoxCONN.Controls.Add(this.comboBoxBaudRate);
            this.groupBoxCONN.Controls.Add(this.buttonReLoad);
            this.groupBoxCONN.Controls.Add(this.buttonConn);
            this.groupBoxCONN.Location = new System.Drawing.Point(12, 45);
            this.groupBoxCONN.Name = "groupBoxCONN";
            this.groupBoxCONN.Size = new System.Drawing.Size(269, 226);
            this.groupBoxCONN.TabIndex = 0;
            this.groupBoxCONN.TabStop = false;
            this.groupBoxCONN.Text = "Kết nối RS232";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Baudrate";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "COM";
            // 
            // comboBoxCOM
            // 
            this.comboBoxCOM.FormattingEnabled = true;
            this.comboBoxCOM.Location = new System.Drawing.Point(111, 36);
            this.comboBoxCOM.Name = "comboBoxCOM";
            this.comboBoxCOM.Size = new System.Drawing.Size(136, 24);
            this.comboBoxCOM.TabIndex = 1;
            this.comboBoxCOM.Text = "(Select COM)";
            // 
            // comboBoxBaudRate
            // 
            this.comboBoxBaudRate.FormattingEnabled = true;
            this.comboBoxBaudRate.Items.AddRange(new object[] {
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "38400",
            "56000",
            "57600",
            "115200"});
            this.comboBoxBaudRate.Location = new System.Drawing.Point(111, 66);
            this.comboBoxBaudRate.Name = "comboBoxBaudRate";
            this.comboBoxBaudRate.Size = new System.Drawing.Size(136, 24);
            this.comboBoxBaudRate.TabIndex = 1;
            this.comboBoxBaudRate.Text = "(Select Baudrate)";
            // 
            // buttonReLoad
            // 
            this.buttonReLoad.BackColor = System.Drawing.Color.White;
            this.buttonReLoad.Location = new System.Drawing.Point(61, 115);
            this.buttonReLoad.Name = "buttonReLoad";
            this.buttonReLoad.Size = new System.Drawing.Size(136, 39);
            this.buttonReLoad.TabIndex = 0;
            this.buttonReLoad.Text = "Tải lại COM";
            this.buttonReLoad.UseVisualStyleBackColor = false;
            this.buttonReLoad.Click += new System.EventHandler(this.buttonReload_Click);
            // 
            // buttonConn
            // 
            this.buttonConn.BackColor = System.Drawing.Color.White;
            this.buttonConn.Location = new System.Drawing.Point(61, 160);
            this.buttonConn.Name = "buttonConn";
            this.buttonConn.Size = new System.Drawing.Size(136, 39);
            this.buttonConn.TabIndex = 0;
            this.buttonConn.Text = "Kết nối";
            this.buttonConn.UseVisualStyleBackColor = false;
            this.buttonConn.Click += new System.EventHandler(this.buttonConn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonCreateCard);
            this.groupBox1.Controls.Add(this.buttonMonitor);
            this.groupBox1.Controls.Add(this.buttonControlBarieL);
            this.groupBox1.Controls.Add(this.buttonControlBarieR);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(287, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(193, 226);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Điều khiển";
            // 
            // buttonCreateCard
            // 
            this.buttonCreateCard.BackColor = System.Drawing.Color.White;
            this.buttonCreateCard.Location = new System.Drawing.Point(25, 28);
            this.buttonCreateCard.Name = "buttonCreateCard";
            this.buttonCreateCard.Size = new System.Drawing.Size(136, 39);
            this.buttonCreateCard.TabIndex = 0;
            this.buttonCreateCard.Text = "Tạo vé xe";
            this.buttonCreateCard.UseVisualStyleBackColor = false;
            this.buttonCreateCard.Click += new System.EventHandler(this.buttonCreateCard_Click);
            // 
            // buttonMonitor
            // 
            this.buttonMonitor.BackColor = System.Drawing.Color.White;
            this.buttonMonitor.Location = new System.Drawing.Point(25, 73);
            this.buttonMonitor.Name = "buttonMonitor";
            this.buttonMonitor.Size = new System.Drawing.Size(136, 39);
            this.buttonMonitor.TabIndex = 0;
            this.buttonMonitor.Text = "Giám sát";
            this.buttonMonitor.UseVisualStyleBackColor = false;
            this.buttonMonitor.Click += new System.EventHandler(this.buttonMonitor_Click);
            // 
            // buttonControlBarieL
            // 
            this.buttonControlBarieL.BackColor = System.Drawing.Color.White;
            this.buttonControlBarieL.Location = new System.Drawing.Point(25, 163);
            this.buttonControlBarieL.Name = "buttonControlBarieL";
            this.buttonControlBarieL.Size = new System.Drawing.Size(136, 39);
            this.buttonControlBarieL.TabIndex = 0;
            this.buttonControlBarieL.Text = "Mở rào chắn trái";
            this.buttonControlBarieL.UseVisualStyleBackColor = false;
            this.buttonControlBarieL.Click += new System.EventHandler(this.buttonControlBarieL_Click);
            // 
            // buttonControlBarieR
            // 
            this.buttonControlBarieR.BackColor = System.Drawing.Color.White;
            this.buttonControlBarieR.Location = new System.Drawing.Point(25, 118);
            this.buttonControlBarieR.Name = "buttonControlBarieR";
            this.buttonControlBarieR.Size = new System.Drawing.Size(136, 39);
            this.buttonControlBarieR.TabIndex = 0;
            this.buttonControlBarieR.Text = "Mở rào chắn phải";
            this.buttonControlBarieR.UseVisualStyleBackColor = false;
            this.buttonControlBarieR.Click += new System.EventHandler(this.buttonControlBarieR_Click);
            // 
            // labelRealTime
            // 
            this.labelRealTime.AutoSize = true;
            this.labelRealTime.Location = new System.Drawing.Point(9, 9);
            this.labelRealTime.Name = "labelRealTime";
            this.labelRealTime.Size = new System.Drawing.Size(14, 16);
            this.labelRealTime.TabIndex = 2;
            this.labelRealTime.Text = "_";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonSave);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(486, 179);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(315, 92);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Xuất báo cáo";
            // 
            // buttonSave
            // 
            this.buttonSave.BackColor = System.Drawing.Color.White;
            this.buttonSave.Location = new System.Drawing.Point(95, 26);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(136, 39);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = false;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonDay);
            this.groupBox3.Controls.Add(this.buttonWeek);
            this.groupBox3.Controls.Add(this.buttonMonth);
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new System.Drawing.Point(486, 45);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(315, 121);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Tạo vé xe";
            // 
            // buttonDay
            // 
            this.buttonDay.BackColor = System.Drawing.Color.White;
            this.buttonDay.Location = new System.Drawing.Point(210, 46);
            this.buttonDay.Name = "buttonDay";
            this.buttonDay.Size = new System.Drawing.Size(89, 39);
            this.buttonDay.TabIndex = 0;
            this.buttonDay.Text = "Vé ngày";
            this.buttonDay.UseVisualStyleBackColor = false;
            this.buttonDay.Click += new System.EventHandler(this.buttonDay_Click);
            // 
            // buttonWeek
            // 
            this.buttonWeek.BackColor = System.Drawing.Color.White;
            this.buttonWeek.Location = new System.Drawing.Point(115, 46);
            this.buttonWeek.Name = "buttonWeek";
            this.buttonWeek.Size = new System.Drawing.Size(89, 39);
            this.buttonWeek.TabIndex = 0;
            this.buttonWeek.Text = "Vé tuần";
            this.buttonWeek.UseVisualStyleBackColor = false;
            this.buttonWeek.Click += new System.EventHandler(this.buttonWeek_Click);
            // 
            // buttonMonth
            // 
            this.buttonMonth.BackColor = System.Drawing.Color.White;
            this.buttonMonth.Location = new System.Drawing.Point(20, 46);
            this.buttonMonth.Name = "buttonMonth";
            this.buttonMonth.Size = new System.Drawing.Size(89, 39);
            this.buttonMonth.TabIndex = 0;
            this.buttonMonth.Text = "Vé tháng";
            this.buttonMonth.UseVisualStyleBackColor = false;
            this.buttonMonth.Click += new System.EventHandler(this.buttonMonth_Click);
            // 
            // timerRealTime
            // 
            this.timerRealTime.Tick += new System.EventHandler(this.timerRealTime_Tick);
            // 
            // serialPort1
            // 
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // textBoxTypeCard
            // 
            this.textBoxTypeCard.Enabled = false;
            this.textBoxTypeCard.Location = new System.Drawing.Point(170, 28);
            this.textBoxTypeCard.Name = "textBoxTypeCard";
            this.textBoxTypeCard.Size = new System.Drawing.Size(213, 22);
            this.textBoxTypeCard.TabIndex = 5;
            this.textBoxTypeCard.Text = "Vé tháng";
            this.textBoxTypeCard.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBoxMonitor
            // 
            this.groupBoxMonitor.Controls.Add(this.buttonConfirm);
            this.groupBoxMonitor.Controls.Add(this.label7);
            this.groupBoxMonitor.Controls.Add(this.label5);
            this.groupBoxMonitor.Controls.Add(this.label6);
            this.groupBoxMonitor.Controls.Add(this.label4);
            this.groupBoxMonitor.Controls.Add(this.label3);
            this.groupBoxMonitor.Controls.Add(this.textBoxTimeOut);
            this.groupBoxMonitor.Controls.Add(this.textBoxTimeIn);
            this.groupBoxMonitor.Controls.Add(this.textBoxOutdatedTime);
            this.groupBoxMonitor.Controls.Add(this.textBoxPrice);
            this.groupBoxMonitor.Controls.Add(this.textBoxTypeCard);
            this.groupBoxMonitor.Enabled = false;
            this.groupBoxMonitor.Location = new System.Drawing.Point(12, 277);
            this.groupBoxMonitor.Name = "groupBoxMonitor";
            this.groupBoxMonitor.Size = new System.Drawing.Size(789, 161);
            this.groupBoxMonitor.TabIndex = 6;
            this.groupBoxMonitor.TabStop = false;
            this.groupBoxMonitor.Text = "Quản lý xe ra vào bãi";
            // 
            // buttonConfirm
            // 
            this.buttonConfirm.BackColor = System.Drawing.Color.White;
            this.buttonConfirm.Location = new System.Drawing.Point(569, 106);
            this.buttonConfirm.Name = "buttonConfirm";
            this.buttonConfirm.Size = new System.Drawing.Size(136, 39);
            this.buttonConfirm.TabIndex = 0;
            this.buttonConfirm.Text = "Xác nhận phí gửi";
            this.buttonConfirm.UseVisualStyleBackColor = false;
            this.buttonConfirm.Click += new System.EventHandler(this.buttonConfirm_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(406, 59);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 16);
            this.label7.TabIndex = 6;
            this.label7.Text = "Thời gian ra";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(406, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "Thời gian vào";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(58, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 16);
            this.label6.TabIndex = 6;
            this.label6.Text = "Hạn sử dụng";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(58, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Phí gửi xe";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(58, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Loại vé";
            // 
            // textBoxTimeOut
            // 
            this.textBoxTimeOut.Enabled = false;
            this.textBoxTimeOut.Location = new System.Drawing.Point(511, 56);
            this.textBoxTimeOut.Name = "textBoxTimeOut";
            this.textBoxTimeOut.Size = new System.Drawing.Size(213, 22);
            this.textBoxTimeOut.TabIndex = 5;
            this.textBoxTimeOut.Text = "00:00:00 23/04/2026";
            this.textBoxTimeOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxTimeIn
            // 
            this.textBoxTimeIn.Enabled = false;
            this.textBoxTimeIn.Location = new System.Drawing.Point(511, 28);
            this.textBoxTimeIn.Name = "textBoxTimeIn";
            this.textBoxTimeIn.Size = new System.Drawing.Size(213, 22);
            this.textBoxTimeIn.TabIndex = 5;
            this.textBoxTimeIn.Text = "00:00:00 23/04/2026";
            this.textBoxTimeIn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxOutdatedTime
            // 
            this.textBoxOutdatedTime.Enabled = false;
            this.textBoxOutdatedTime.Location = new System.Drawing.Point(170, 84);
            this.textBoxOutdatedTime.Name = "textBoxOutdatedTime";
            this.textBoxOutdatedTime.Size = new System.Drawing.Size(213, 22);
            this.textBoxOutdatedTime.TabIndex = 5;
            this.textBoxOutdatedTime.Text = "00:00:00 23/04/2026";
            this.textBoxOutdatedTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxPrice
            // 
            this.textBoxPrice.Enabled = false;
            this.textBoxPrice.Location = new System.Drawing.Point(170, 56);
            this.textBoxPrice.Name = "textBoxPrice";
            this.textBoxPrice.Size = new System.Drawing.Size(213, 22);
            this.textBoxPrice.TabIndex = 5;
            this.textBoxPrice.Text = "000.000";
            this.textBoxPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxCheckData
            // 
            this.textBoxCheckData.Location = new System.Drawing.Point(807, 64);
            this.textBoxCheckData.Multiline = true;
            this.textBoxCheckData.Name = "textBoxCheckData";
            this.textBoxCheckData.Size = new System.Drawing.Size(503, 374);
            this.textBoxCheckData.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(807, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(202, 16);
            this.label8.TabIndex = 8;
            this.label8.Text = "TextBox check dữ liệu nhận được";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1322, 454);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxCheckData);
            this.Controls.Add(this.groupBoxMonitor);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.labelRealTime);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxCONN);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đồ án tốt nghiệp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.groupBoxCONN.ResumeLayout(false);
            this.groupBoxCONN.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBoxMonitor.ResumeLayout(false);
            this.groupBoxMonitor.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxCONN;
        private System.Windows.Forms.ComboBox comboBoxBaudRate;
        private System.Windows.Forms.Button buttonReLoad;
        private System.Windows.Forms.Button buttonConn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxCOM;
        private System.Windows.Forms.Button buttonControlBarieR;
        private System.Windows.Forms.Label labelRealTime;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonCreateCard;
        private System.Windows.Forms.Button buttonMonitor;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonDay;
        private System.Windows.Forms.Button buttonWeek;
        private System.Windows.Forms.Button buttonMonth;
        private System.Windows.Forms.Timer timerRealTime;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox textBoxTypeCard;
        private System.Windows.Forms.GroupBox groupBoxMonitor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPrice;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxTimeIn;
        private System.Windows.Forms.TextBox textBoxOutdatedTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxTimeOut;
        private System.Windows.Forms.Button buttonConfirm;
        private System.Windows.Forms.Button buttonControlBarieL;
        private System.Windows.Forms.TextBox textBoxCheckData;
        private System.Windows.Forms.Label label8;
    }
}

