using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// Thu vien ket noi
using System.IO;
using System.IO.Ports;
using System.Diagnostics;
using System.Linq.Expressions;

// Thu vien xu ly video
using AForge.Video;
using AForge.Video.DirectShow;
using System.Text.RegularExpressions;

namespace Project_I
{
    public partial class Form1 : Form
    {
        FilterInfoCollection cameras;
        VideoCaptureDevice cam;
        String InputData = String.Empty; // Khai báo string buff dùng cho hiển thị dữ liệu sau này.

        public object SaveFileDialog1 { get; private set; }

        delegate void SetTextCallback(string text); // Khai bao delegate SetTextCallBack voi tham so string
        public Form1()
        {
            InitializeComponent();
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceive);
            string[] BaudRate = { "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200" };
            comboBox2.Items.AddRange(BaudRate);
            string[] StopBits = { "1", "2", "0"};
            comboBox3.DataSource = StopBits;
            cameras = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in cameras)
            {
                comboBox5.Items.Add(device.Name);
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = SerialPort.GetPortNames();// Quét các cổng COM đang hoạt động lên comboBox1
            comboBox2.SelectedIndex = 3;
            comboBox5.SelectedIndex = 4;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                label5.Text = ("Chưa kết nối");
                label5.ForeColor = Color.Red;

            }
            else if (serialPort1.IsOpen)
            {
                label5.Text = ("Đã kết nối");
                label5.ForeColor = Color.Green;
            }
        }

        private void DataReceive(object obj, SerialDataReceivedEventArgs e)

        {
            InputData = serialPort1.ReadExisting();
            if (InputData != String.Empty)
            {
                //textbox1.Txt = InputData; // Ko dùng đc như thế này vì khác threads .
                Solution(InputData); // Ủy quyền tại đây. Gọi delegate đã khai báo trước đó.
                
            }
        }
        private void Solution(string text)
        {
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(Solution); // khởi tạo 1 delegate mới gọi đến SetText
                this.Invoke(d, new object[] { text });
            }
            else
            {
                string except ="\n";
                this.textBox1.Text += Regex.Replace(text, @"[^a-zA-Z0-9]+except+", string.Empty); // Loại bỏ các kí tự đặc biệt
                saveFileDialog1.InitialDirectory = "D:\\HUST\\Project I\\src\\Project-I\\Data";
                //SaveFileDialog1.DefaultExt = ".jpg";
                saveFileDialog1.FileName = Regex.Replace(text, @"[^a-zA-Z0-9]", string.Empty) +".jpg"; // Lưu hình ảnh với tên file = ID thẻ
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image.Save(saveFileDialog1.FileName);
                }
            }

        }

        // Conect với Serial Port I
        private void button2_Click(object sender, EventArgs e)
        {
            // Ket noi cong COM
            if (!serialPort1.IsOpen)
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                switch (comboBox3.Text)
                {
                    case "1":
                        serialPort1.StopBits = System.IO.Ports.StopBits.One;
                        break;
                    case "2":
                        serialPort1.StopBits = System.IO.Ports.StopBits.Two;
                        break;
                    case "0":
                        serialPort1.StopBits = System.IO.Ports.StopBits.None;
                        break;
                    default:
                        // code block
                        break;
                }
                serialPort1.StopBits = System.IO.Ports.StopBits.One;
                serialPort1.Open();
            }
            // Ket noi Camera
            if (cam != null && cam.IsRunning)
            {
                cam.Stop();
            }
            cam = new VideoCaptureDevice(cameras[comboBox5.SelectedIndex].MonikerString);
            cam.NewFrame += Cam_NewFrame;
            cam.Start();
        }

        // hien thi noi dung camera len picture box
        private void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = bitmap;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.Close(); // ngat ket noi COM
            if (cam != null && cam.IsRunning)
            {
                cam.Stop(); // Ngat ket noi camera
            }
        }

        private void label6_Click_1(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
