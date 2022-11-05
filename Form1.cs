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

namespace Project_I
{
    public partial class Form1 : Form
    {
        string InputData = String.Empty; // Khai báo string buff dùng cho hiển thị dữ liệu sau này.

        delegate void SetTextCallback(string text); // Khai bao delegate SetTextCallBack voi tham so string
        public Form1()
        {
            InitializeComponent();
            //serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceive);
            string[] BaudRate = { "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200" };
            comboBox2.Items.AddRange(BaudRate);
            //string[] StopBits = { "StopBits.One", "StopBits.Two", "StopBits.None"};
            //comboBox3.DataSource = StopBits;

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
        // Conect với Serial Port I
        private void button2_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {

                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                // Cái này hiện em đang chỉnh bằng Code vì em không biết chuyển từ text sang câu lệnh như thế nào :(
                serialPort1.StopBits = StopBits.One;
                serialPort1.Open();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.Close(); 
        }
    }
}
