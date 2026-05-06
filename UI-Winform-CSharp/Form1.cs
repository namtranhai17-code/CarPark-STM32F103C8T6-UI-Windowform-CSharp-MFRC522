using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace MONITOR_Ver1_0
{
    public partial class Form1 : Form
    {
        /*  +-------------------------------------------+
         *  |                 Variable                  |
         *  +-------------------------------------------+
         */
        /*================ Type of Cards ===================*/

        private List<ParkingLog> _parkingLogs = new List<ParkingLog>();
        private int _stt = 1;

        // State machine
        private enum MonitorState { Idle, WaitingConfirm }
        private MonitorState _monitorState = MonitorState.Idle;
        private string _pendingTypeCode = "";
        private string _pendingUID = "";
        private string _pendingCreate = "";
        private string _pendingExpire = "";
        private string _pendingInOut = "";  // "Vào" hoặc "Ra"

        string filePath = "";
        /*================= Transmission ===================*/
        byte[] data_t;
        /*=================== Reception ====================*/

        private string _receiveBuffer = "";
        /*  +-------------------------------------------+
         *  |                 Function                  |
         *  +-------------------------------------------+
         */
        
        public Form1()
        {
            InitializeComponent();
            timerRealTime.Enabled = true;
            data_t = new byte[2];
        }

        #region Form Handler
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comboBoxCOM.Items.Add(port);
            }

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                serialPort1.Open();
                data_t[0] = (byte)'S';
                data_t[1] = 0x50;
                serialPort1.Write(data_t, 0, 2);

                serialPort1.Close();
            }
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            var sFile = new SaveFileDialog();
            sFile.Filter = "CSV|*.csv|All Formats|*.*";
            sFile.FileName = $"CarPark_CheckInOut_{DateTime.Now:HH-mm_dd-MM-yyyy}.csv";
            if (sFile.ShowDialog() == DialogResult.OK)
            {
                using (var sWriter = new StreamWriter(sFile.FileName, true, Encoding.UTF8))
                {
                    filePath = sFile.FileName;

                    sWriter.WriteLine("STT, UID, Loại thẻ, Ngày tạo, Ngày hết hạn, Trạng thái, Thời gian, Phí gửi xe");
                }
            }
        }
        #endregion

        #region Real Time
        private void timerRealTime_Tick(object sender, EventArgs e)
        {
            labelRealTime.Text = DateTime.Now.ToString("hh:mm:ss tt dd/MM/yy");
        }
        #endregion

        #region CONNECTION
        private void buttonReload_Click(object sender, EventArgs e)
        {
            try
            {
                comboBoxCOM.Items.Clear();

                string[] ports = SerialPort.GetPortNames();

                comboBoxCOM.Items.AddRange(ports);

                if (ports.Length > 0)
                {
                    comboBoxCOM.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy cổng COM nào!", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải lại danh sách cổng COM" + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonConn_Click(object sender, EventArgs e)
        {
            try
            {
                if ((comboBoxCOM.SelectedIndex != -1) && (comboBoxBaudRate.SelectedIndex != -1))
                {
                    if (!serialPort1.IsOpen)
                    {
                        comboBoxCOM.Enabled = false;
                        comboBoxBaudRate.Enabled = false;
                        buttonReLoad.Enabled = false;
                        
                        buttonConn.BackColor = Color.Green;
                        buttonConn.Text = "Ngắt kết nối";
                        serialPort1.PortName = comboBoxCOM.Text;
                        serialPort1.BaudRate = int.Parse(comboBoxBaudRate.Text);
                        serialPort1.Open();


                        data_t[0] = (byte)'S';
                        data_t[1] = 0x51;
                        serialPort1.Write(data_t, 0, 2);

                        groupBox1.Enabled = true;
                        groupBox2.Enabled = true;
                    }
                    else
                    {
                        comboBoxCOM.Enabled = true;
                        comboBoxBaudRate.Enabled = true;
                        buttonReLoad.Enabled = true;
                        
                        buttonConn.BackColor = Color.White;
                        buttonCreateCard.BackColor = Color.White;
                        buttonMonitor.BackColor = Color.White;
                        buttonControlBarieR.BackColor = Color.White;
                        buttonControlBarieL.BackColor = Color.White;
                        buttonMonth.BackColor = Color.White;
                        buttonWeek.BackColor = Color.White;
                        buttonDay.BackColor = Color.White;

                        buttonConn.Text = "Kết nối";

                        data_t[0] = (byte)'S';
                        data_t[1] = 0x52;
                        serialPort1.Write(data_t, 0, 2);

                        serialPort1.Close();

                        groupBox1.Enabled = false;
                        groupBox2.Enabled = false;
                        groupBox3.Enabled = false;
                    }
                }
                else if (comboBoxCOM.SelectedIndex == -1)
                {
                    MessageBox.Show("Lỗi: " + "cổng COM chưa được chọn!",
                                    "Warning",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
                else if (comboBoxBaudRate.SelectedIndex == -1)
                {
                    MessageBox.Show("Lỗi: " + "Tốc độ Baud chưa được chọn!",
                                    "Warning",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }    
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi" + ex.Message);
            }
        }

        #endregion

        #region SAVE FILE .CSV
        private void buttonSave_Click(object sender, EventArgs e)
        {
            var sFile = new SaveFileDialog();
            sFile.Filter = "CSV|*.csv|Text|*.txt|PDF|*.pdf|All formats|*.*";
            sFile.FileName = $"REPORT {DateTime.Now:yyyy-MM-dd HH-mm-ss}.csv";

            if(sFile.ShowDialog() == DialogResult.OK)
            {
                using(var sWriter = new StreamWriter(sFile.FileName, false, Encoding.UTF8))
                {
                    sWriter.WriteLine("STT, UID, Loại thẻ, Ngày tạo, Ngày hết hạn, Trạng thái, Thời gian");

                    int stt = 1;
                    foreach(var card in _parkingLogs) 
                    {
                        sWriter.WriteLine($"{stt}, {card.UID}, {card.TypeCard}, {card.CreateDate}, {card.ExpireDate}, {card.Status}, {card.Time:hh:mm tt dd/MM/yyyy}");
                        stt++;
                    }

                }
            }
        }

        #endregion

        #region CONTROL

        #region CREATE CARD
        /*================ Send Data to VDK ===================*/
        private void Send_CardCreationData(int typeCard)
        {
            byte[] packet = new byte[8];
            // Lấy 2 số cuối của năm (2026 → 26)
            DateTime createDate = DateTime.Now;
            byte createYear = (byte)(createDate.Year % 100);
            DateTime expireDate;
            packet[0] = (byte)'C';          // Header

            packet[2] = (byte)createDate.Day;
            packet[3] = (byte)createDate.Month;
            packet[4] = createYear;

            if (typeCard == 0) // Month Card
            {
                expireDate = DateTime.Now.AddMonths(1);
                byte expireYear = (byte)(expireDate.Year % 100);
                packet[1] = 0xf0;               // Lệnh: tạo thẻ tháng
                packet[5] = (byte)expireDate.Day;
                packet[6] = (byte)expireDate.Month;
                packet[7] = expireYear;
            }
            else if (typeCard == 1) // Week Card
            {
                expireDate = DateTime.Now.AddDays(7);
                byte expireYear = (byte)(expireDate.Year % 100);
                packet[1] = 0xf1;               // Lệnh: tạo thẻ tuần
                packet[5] = (byte)expireDate.Day;
                packet[6] = (byte)expireDate.Month;
                packet[7] = expireYear;
            }
            else if (typeCard == 2) // Day Card
            {
                expireDate = DateTime.Now.AddYears(10);
                byte expireYear = (byte)(expireDate.Year % 100);
                packet[1] = 0xf2;               // Lệnh: tạo thẻ ngày
                packet[5] = (byte)expireDate.Day;
                packet[6] = (byte)expireDate.Month;
                packet[7] = expireYear;
            }
            if (serialPort1.IsOpen) serialPort1.Write(packet, 0, packet.Length);
        }
        private void buttonCreateCard_Click(object sender, EventArgs e)
        {
            if(buttonCreateCard.BackColor == Color.White)
            {
                groupBox3.Enabled = true;
                groupBoxMonitor.Enabled = false;

                buttonCreateCard.BackColor = Color.Green;
                buttonMonitor.BackColor = Color.White;
                buttonControlBarieR.BackColor = Color.White;
                buttonControlBarieL.BackColor = Color.White;
            }
            else
            {
                groupBox3.Enabled = false;

                buttonMonth.BackColor = Color.White;
                buttonWeek.BackColor = Color.White;
                buttonDay.BackColor = Color.White;

                buttonCreateCard.BackColor = Color.White;

            }
        }

        private void buttonMonth_Click(object sender, EventArgs e)
        {
            if (buttonMonth.BackColor == Color.White)
            {
                buttonMonth.BackColor = Color.Green;
                buttonWeek.BackColor = Color.White;
                buttonDay.BackColor = Color.White;

                if(buttonCreateCard.BackColor == Color.Green)
                {
                    Send_CardCreationData(0);
                }
            }
            else
            {
                buttonMonth.BackColor = Color.White;
            }
        }

        private void buttonWeek_Click(object sender, EventArgs e)
        {
            if (buttonWeek.BackColor == Color.White)
            {
                buttonWeek.BackColor = Color.Green;
                buttonMonth.BackColor = Color.White;
                buttonDay.BackColor = Color.White;

                if (buttonCreateCard.BackColor == Color.Green)
                {
                    Send_CardCreationData(1);
                }
            }
            else
            {
                buttonWeek.BackColor = Color.White;
            }
        }

        private void buttonDay_Click(object sender, EventArgs e)
        {
            if (buttonDay.BackColor == Color.White)
            {
                buttonDay.BackColor = Color.Green;
                buttonWeek.BackColor = Color.White;
                buttonMonth.BackColor = Color.White;

                if (buttonCreateCard.BackColor == Color.Green)
                {
                    Send_CardCreationData(2);
                }
            }
            else
            {
                buttonDay.BackColor = Color.White;
            }
        }
        #endregion

        #region MONITOR
        private void buttonMonitor_Click(object sender, EventArgs e)
        {
            if(buttonMonitor.BackColor == Color.White)
            {
                buttonCreateCard.BackColor = Color.White;
                buttonMonitor.BackColor = Color.Green;
                buttonControlBarieR.BackColor = Color.White;
                buttonControlBarieL.BackColor = Color.White;

                groupBox3.Enabled = false;
                groupBoxMonitor.Enabled = true;

                buttonMonth.BackColor = Color.White;
                buttonWeek.BackColor = Color.White;
                buttonDay.BackColor = Color.White;

                data_t[0] = (byte)'M';
                data_t[1] = 0xff;
                serialPort1.Write(data_t, 0, 2);
            }   
            else
            {
                buttonMonitor.BackColor = Color.White;
                groupBoxMonitor.Enabled = false;
            }    
        }
        #endregion

        #region OPEN MID
        private void buttonControlBarieR_Click(object sender, EventArgs e)
        {
            if(buttonControlBarieR.BackColor == Color.White)
            {
                buttonCreateCard.BackColor = Color.White;
                buttonMonitor.BackColor = Color.White;
                buttonControlBarieR.BackColor = Color.Green;

                groupBox3.Enabled = false;
                groupBoxMonitor.Enabled = false;

                buttonMonth.BackColor = Color.White;
                buttonWeek.BackColor = Color.White;
                buttonDay.BackColor = Color.White;

                data_t[0] = (byte)'A';
                data_t[1] = 0x21;
                serialPort1.Write(data_t, 0, 2);
            }   
            else
            {
                buttonControlBarieR.BackColor = Color.White;
                data_t[0] = (byte)'A';
                data_t[1] = 0x20;
                serialPort1.Write(data_t, 0, 2);
            }
        }

        private void buttonControlBarieL_Click(object sender, EventArgs e)
        {
            if (buttonControlBarieL.BackColor == Color.White)
            {
                buttonCreateCard.BackColor = Color.White;
                buttonMonitor.BackColor = Color.White;
                buttonControlBarieL.BackColor = Color.Green;

                groupBox3.Enabled = false;
                groupBoxMonitor.Enabled = false;

                buttonMonth.BackColor = Color.White;
                buttonWeek.BackColor = Color.White;
                buttonDay.BackColor = Color.White;

                data_t[0] = (byte)'B';
                data_t[1] = 0x11;
                serialPort1.Write(data_t, 0, 2);
            }
            else
            {
                buttonControlBarieL.BackColor = Color.White;
                data_t[0] = (byte)'B';
                data_t[1] = 0x10;
                serialPort1.Write(data_t, 0, 2);
            }
        }
        #endregion

        #endregion


        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                _receiveBuffer += serialPort1.ReadExisting();

                int idx;
                
                while ((idx = _receiveBuffer.IndexOf('\n')) >= 0)
                {
                    string line = _receiveBuffer.Substring(0, idx).Trim();

                    _receiveBuffer = _receiveBuffer.Substring(idx + 1);
                    if (string.IsNullOrEmpty(line)) continue;

                    // Xử lý frame kết quả đọc thẻ
                    if (line.StartsWith("R|"))
                    {
                        string status = line.Substring(2);  // lấy '0' hoặc '1'
                        this.Invoke((MethodInvoker)(() => HandleReadResult(status)));
                    }
                    else if (line.StartsWith("B") || line.StartsWith("UID")) // Hiển thị dữ liệu của các block bằng cách gọi Invoke StoreBlockData
                    {
                        string inf_card_line = line;
                        this.Invoke((MethodInvoker)(() => Store_Infor_Card_Data(inf_card_line)));
                    }
                    else // Vì thêm trường hợp "B" nên giờ phần này chỉ có thể hiển thị UID
                    {
                        // Các dòng dữ liệu bình thường (UID, Block...)
                        this.Invoke((MethodInvoker)(() =>
                        {
                            textBoxCheckData.AppendText(line + "\r\n");
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    textBoxCheckData.AppendText($"[Lỗi]: {ex.Message}\r\n");
                }));
            }
        }

        private Dictionary<string, string> _blockData = new Dictionary<string, string>();
        private void Store_Infor_Card_Data(string inf_card_line)
        {
            // line = "B04:09052026|0|0"
            if (inf_card_line.Length < 4) return;
            if (inf_card_line.StartsWith("UID"))
            {
                string UIDKey = inf_card_line.Substring(0, 3);
                string UIDVal = inf_card_line.Substring(5);
                _blockData[UIDKey] = UIDVal;
            }
            else if (inf_card_line.StartsWith("B"))
            {
                string blockKey = inf_card_line.Substring(0, 3);
                string blockVal = inf_card_line.Substring(4);
                _blockData[blockKey] = blockVal;
            }
            textBoxCheckData.AppendText(inf_card_line + "\r\n"); // Hiển thị dữ liệu từ các Block
        }

        private void HandleReadResult(string status)
        {
            if (status == "0")  // Đọc OK
            {
                ParseAndDisplay();
                textBoxCheckData.AppendText("[✓] Đọc thẻ thành công!\r\n");
                _blockData.Clear();  // reset cho lần đọc tiếp
                // TODO: parse dữ liệu block vừa nhận vào List<Card>

            }
            else if (status == "1")  // Đọc lỗi
            {
                textBoxCheckData.AppendText("[✗] Đọc thẻ lỗi — đang thử lại...\r\n");
                _blockData.Clear();
                // Không cần gửi gì — STM32 tự quét lại vì cardRead vẫn = 0
            }
        }

        // Chuyển đổi và hiển thị dữ liệu lên UI
        private void ParseAndDisplay()
        {
            if (!_blockData.TryGetValue("B04", out string b4)) return;

            var parts = b4.Split('|');
            if (parts.Length < 2) return;

            // Kiểm tra key
            if (parts[0] != "09052026")
            {
                textBoxCheckData.AppendText("[✗] Vé xe không hợp lệ\r\n");
                MessageBox.Show("Vé xe không hợp lệ", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // UID
            _pendingUID = "";
            if (_blockData.TryGetValue("UID", out string uid))
            {
                _pendingUID = uid;

            }

            // Loại thẻ
            _pendingTypeCode = parts[1];
            if (_pendingTypeCode == "0") textBoxTypeCard.Text = "Vé tháng";
            else if (_pendingTypeCode == "1") textBoxTypeCard.Text = "Vé tuần";
            else if (_pendingTypeCode == "2") textBoxTypeCard.Text = "Vé ngày";

            // Ngày tạo — Block 8: "C|09|05|26"
            _pendingCreate = "";
            if (_blockData.TryGetValue("B08", out string b8))
            {
                var p = b8.Split('|');
                if (p.Length >= 4)
                    _pendingCreate = p[1] + "/" + p[2] + "/20" + p[3];
            }

            // Ngày hết hạn — Block 9: "E|09|06|26"
            _pendingExpire = "";
            if (_blockData.TryGetValue("B09", out string b9))
            {
                var p = b9.Split('|');
                if (p.Length >= 4)
                {
                    _pendingExpire = p[1] + "/" + p[2] + "/20" + p[3];
                    textBoxOutdatedTime.Text = _pendingExpire;
                }
            }

            // Xác định trạng thái vào/ra dựa vào log CSV
            // Nếu lần cuối của UID này là "Vào" → lần này là "Ra", và ngược lại
            string lastStatus = GetLastStatus(_pendingUID);
            _pendingInOut = (lastStatus == "Vào") ? "Ra" : "Vào";

            textBoxCheckData.AppendText("[✓] Vé hợp lệ — " + _pendingInOut + "\r\n");
            textBoxCheckData.AppendText("Ấn Xác nhận để ghi nhận\r\n");

            if (_pendingInOut == "Vào")
            {
                data_t[0] = (byte)'M';
                data_t[1] = 0x21;
                serialPort1.Write(data_t, 0, 2);
            }
            else
            {
                data_t[1] = 0x11;
                serialPort1.Write(data_t, 0, 2);
            }

            _monitorState = MonitorState.WaitingConfirm;
        }

        private string GetLastStatus(string uid)
        {
            for (int i = _parkingLogs.Count - 1; i >= 0; i--)
            {
                if (_parkingLogs[i].UID == uid)
                    return _parkingLogs[i].Status;
            }
            return "Ra";  // Chưa có log → mặc định lần đầu là "Vào"
        }

        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            if (_monitorState != MonitorState.WaitingConfirm) return;

            decimal fee = 0m;
            List<SessionCharge> detail = new List<SessionCharge>();

            if (_pendingInOut == "Ra")
            {
                // Tìm thời gian vào từ log
                DateTime entryTime = GetLastEntryTime(_pendingUID);

                if (entryTime == DateTime.MinValue)
                {
                    // Không tìm được giờ vào trong phiên hiện tại
                    // Có thể xe vào từ phiên trước — tính từ đầu ngày
                    entryTime = DateTime.Today;
                    textBoxCheckData.AppendText("[!] Không tìm được giờ vào — tính từ đầu ngày\r\n");
                }

                fee = ParkingFeeCalculator.Calculate(
                    _pendingTypeCode,
                    _pendingExpire,
                    entryTime,
                    DateTime.Now,
                    out detail
                );

                // Hiển thị chi tiết phí
                textBoxCheckData.AppendText("── Chi tiết phí ──\r\n");
                foreach (var s in detail)
                    textBoxCheckData.AppendText(s.DateLabel + " " + s.SessionName + ": " + s.Price.ToString("N0") + "đ\r\n");
                textBoxCheckData.AppendText("Tổng: " + fee.ToString("N0") + "đ\r\n");

                textBoxPrice.Text = fee.ToString("N0") + "đ";
            }

            var log = new ParkingLog
            {
                STT = _stt++,
                UID = _pendingUID,
                TypeCard = textBoxTypeCard.Text,
                CreateDate = _pendingCreate,
                ExpireDate = _pendingExpire,
                Status = _pendingInOut,
                Time = DateTime.Now,
                Fee = fee
            };

            _parkingLogs.Add(log);
            AppendLogToCSV(log);

            string timeStr = log.Time.ToString("HH:mm  dd/MM/yy");
            if (_pendingInOut == "Vào")
            {
                textBoxTimeIn.Text = timeStr;
                textBoxTimeOut.Text = "--:--";
                data_t[0] = (byte)'A'; data_t[1] = 0x20;
                serialPort1.Write(data_t, 0, 2);
            }
            else
            {
                textBoxTimeOut.Text = timeStr;
                data_t[0] = (byte)'B'; data_t[1] = 0x10;
                serialPort1.Write(data_t, 0, 2);
            }

            textBoxCheckData.AppendText("[✓] Đã xác nhận: " + _pendingInOut + " lúc " + timeStr + "\r\n");

            _monitorState = MonitorState.Idle;
            _blockData.Clear();

            data_t[0] = (byte)'M';
            data_t[1] = 0xff;
            serialPort1.Write(data_t, 0, 2);
        }

        private DateTime GetLastEntryTime(string uid)
        {
            for (int i = _parkingLogs.Count - 1; i >= 0; i--)
            {
                if (_parkingLogs[i].UID == uid && _parkingLogs[i].Status == "Vào")
                    return _parkingLogs[i].Time;
            }
            return DateTime.MinValue;
        }

        private void AppendLogToCSV(ParkingLog log)
        {
            if (string.IsNullOrEmpty(filePath)) return;
            try
            {
                using (var writer = new StreamWriter(filePath, true, Encoding.UTF8))
                {
                    string feeStr = log.Fee > 0
                        ? log.Fee.ToString() + "đ"
                        : (log.Status == "Ra" ? "Miễn phí" : "-");

                    writer.WriteLine(
                        log.STT + "," +
                        log.UID + "," +
                        log.TypeCard + "," +
                        log.CreateDate + "," +
                        log.ExpireDate + "," +
                        log.Status + "," +
                        log.Time.ToString("hh:mm tt dd/MM/yy") + "," +
                        feeStr
                    );
                }
            }
            catch (Exception ex)
            {
                textBoxCheckData.AppendText("[Lỗi ghi CSV]: " + ex.Message + "\r\n");
            }
        }
    }

    /*  +-------------------------------------------+
     *  |                Class Card                 |
     *  +-------------------------------------------+
     */
    #region Class Card
    public static class ParkingPrice
    {
        public const decimal Monthly = 100000;
        public const decimal Weekly = 50000;
        public const decimal Morning = 3000;
        public const decimal Afternoon = 3000;
        public const decimal Night = 10000;
    }

    public class SessionCharge
    {
        public string SessionName { get; set; }
        public string DateLabel { get; set; }
        public decimal Price { get; set; }
    }

    public static class ParkingFeeCalculator
    {
        // Xác định buổi chứa thời điểm t
        private static void GetSessionBounds(
            DateTime t,
            out DateTime start, out DateTime end,
            out string name, out decimal price)
        {
            int hour = t.Hour;
            DateTime day = t.Date;

            if (hour >= 6 && hour < 12)
            {
                start = day.AddHours(6);
                end = day.AddHours(12);
                name = "Sáng";
                price = ParkingPrice.Morning;
            }
            else if (hour >= 12 && hour < 18)
            {
                start = day.AddHours(12);
                end = day.AddHours(18);
                name = "Chiều";
                price = ParkingPrice.Afternoon;
            }
            else
            {
                if (hour >= 18)
                {
                    start = day.AddHours(18);
                    end = day.AddDays(1).AddHours(6);
                }
                else
                {
                    start = day.AddDays(-1).AddHours(18);
                    end = day.AddHours(6);
                }
                name = "Đêm";
                price = ParkingPrice.Night;
            }
        }

        // Tính tiền theo buổi từ entryTime đến exitTime
        public static decimal CalcBySession(
            DateTime entryTime, DateTime exitTime,
            out List<SessionCharge> detail)
        {
            detail = new List<SessionCharge>();
            decimal total = 0;
            DateTime cursor = entryTime;

            while (cursor < exitTime)
            {
                DateTime sStart, sEnd;
                string sName;
                decimal sPrice;
                GetSessionBounds(cursor, out sStart, out sEnd, out sName, out sPrice);

                if (sStart < exitTime && sEnd > entryTime)
                {
                    detail.Add(new SessionCharge
                    {
                        SessionName = sName,
                        DateLabel = sStart.ToString("dd/MM"),
                        Price = sPrice
                    });
                    total += sPrice;
                }
                cursor = sEnd;
            }
            return total;
        }

        // Hàm tính phí chính
        public static decimal Calculate(
            string typeCode,
            string expireStr,       // "dd/MM/yyyy"
            DateTime entryTime,
            DateTime exitTime,
            out List<SessionCharge> detail)
        {
            detail = new List<SessionCharge>();

            if (typeCode == "0" || typeCode == "1") // Vé tháng / vé tuần
            {
                DateTime expire;
                if (!TryParseExpire(expireStr, out expire))
                    return CalcBySession(entryTime, exitTime, out detail);

                if (exitTime <= expire)
                    return 0m;  // còn hạn → miễn phí

                // Hết hạn → tính từ ngày hết hạn đến lúc ra
                DateTime chargeFrom = expire > entryTime ? expire : entryTime;
                return CalcBySession(chargeFrom, exitTime, out detail);
            }
            else // Vé ngày
            {
                return CalcBySession(entryTime, exitTime, out detail);
            }
        }

        // Parse "dd/MM/yyyy" → DateTime
        private static bool TryParseExpire(string s, out DateTime result)
        {
            result = DateTime.MinValue;
            if (string.IsNullOrEmpty(s)) return false;
            return DateTime.TryParseExact(
                s, "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out result);
        }
    }

    public class ParkingLog
    {
        public int STT { get; set; }
        public string UID { get; set; }
        public string TypeCard { get; set; }
        public string CreateDate { get; set; }
        public string ExpireDate { get; set; }
        public string Status { get; set; }
        public DateTime Time { get; set; }
        public decimal Fee { get; set; }
    }
    #endregion
}
