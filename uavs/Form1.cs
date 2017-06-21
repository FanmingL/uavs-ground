using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace uavs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static bool serial1_open_flag;
        public static bool text_focus_flag;
        public static bool listening_flag;
        public static bool closing_flag;
        public static System.DateTime currentTime = System.DateTime.Now;
        public static String path_name = "uavs_data";
        public static String ctime;
        public static List<Byte> BufferToSave;
        public static FileStream Buffer_fs;
        public static StreamWriter Buffer_wr;

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (!serial1_open_flag)
            {

                if (this.comboBox2.Text.Length == 0)
                {
                    this.label8.Text = "请选择串口";
                    return;
                }
                string[] strPort = SerialPort.GetPortNames();
                if (strPort.Length == 0) { this.label8.Text = "本机无可用串口"; this.comboBox2.Items.Clear(); return; }
                    if (serial1_open_flag)
                {
                    this.label8.Text = "串口已经打开了,请先关闭";
                    return;

                }
                this.serialPort1.BaudRate = int.Parse(this.comboBox1.Text);
                this.serialPort1.PortName = this.comboBox2.Text;
                this.label8.Text = "串口已经打开了";
                this.serialPort1.DataBits = int.Parse(this.comboBox3.Text);
                switch (this.comboBox4.Text)
                {
                    case "None":
                        this.serialPort1.Parity = Parity.None;
                        break;
                    case "Odd":
                        this.serialPort1.Parity = Parity.Odd;
                        break;
                    case "Even":
                        this.serialPort1.Parity = Parity.Even;
                        break;
                    case "Mark":
                        this.serialPort1.Parity = Parity.Mark;
                        break;
                    case "Space":
                        this.serialPort1.Parity = Parity.Space;
                        break;
                    default:
                        MessageBox.Show("参数不正确", "Error");
                        break;
                }
                switch (this.comboBox5.Text)
                {
                    case "1":
                        this.serialPort1.StopBits = StopBits.One;
                        break;
                    case "1.5":
                        this.serialPort1.StopBits = StopBits.OnePointFive;
                        break;
                    case "2":
                        this.serialPort1.StopBits = StopBits.Two;
                        break;
                    default:
                        MessageBox.Show("参数不正确", "Error");
                        break;
                }

                this.serialPort1.Open();
                serial1_open_flag = true;
                String infom = "";
                infom += "端口号:" + this.serialPort1.PortName + "|";
                infom += "波特率:" + this.serialPort1.BaudRate.ToString() + "|";
                infom += "数据位:" + this.serialPort1.DataBits.ToString() + "|";
                infom += "停止位:" + this.serialPort1.StopBits + "|";
                infom += "校验位:" + this.serialPort1.Parity;
                this.label8.Text = infom;
                this.button1.Text = "关闭串口";
                this.button7.Text = "close serials";
                this.radioButton1.Enabled = false;
                this.radioButton2.Enabled = false;
                this.comboBox1.Enabled = false;
                this.comboBox2.Enabled = false;
                this.comboBox3.Enabled = false;
                this.comboBox4.Enabled = false;
                this.comboBox5.Enabled = false;
                this.button5.Enabled = false;
                timer1.Start();
                timer2.Start();

            }
            else
            {
                closing_flag = true;
                while (listening_flag) Application.DoEvents();
                this.label8.Text = "串口已关闭";
                if (!serial1_open_flag) return;
                this.serialPort1.DiscardOutBuffer();
                this.serialPort1.Close();
                this.label9.Text = "";
                serial1_open_flag = false;
                this.button1.Text = "打开串口";
                this.button7.Text = "open serials";

                this.radioButton1.Enabled = true;
                this.radioButton2.Enabled = true;
                this.comboBox1.Enabled = true;
                this.comboBox2.Enabled = true;
                this.comboBox3.Enabled = true;
                this.comboBox4.Enabled = true;
                this.comboBox5.Enabled = true;
                this.button5.Enabled = true;
                closing_flag = false;
                timer1.Stop();
                timer2.Stop();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.label8.Text = "串口已关闭";
            if (!serial1_open_flag) return;
            this.serialPort1.Close();
            serial1_open_flag = false;
            this.button1.Text = "打开串口";
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (!serial1_open_flag)
            {
                this.label8.Text = "串口未打开";
                return;

            }
            if (radioButton3.Checked)
            {
                String str = this.textBox1.Text;
                str = str.Replace(" ", "");
                if (str.Length % 2 != 0) { this.label8.Text = "请输入偶数个字节"; return; }

                Byte[] ans = new Byte[str.Length / 2];

                for (int i = 0; i < str.Length; i += 2)
                {
                    String str1 = str.Substring(i, 2);
                    ans[i / 2] = Convert.ToByte(str1, 16);
                }
                this.serialPort1.Write(ans, 0, ans.Length);
            }
            else
            {
                this.serialPort1.Write(this.textBox1.Text);
            }





        }

        private void button4_Click(object sender, EventArgs e)
        {
            // this.richTextBox1.Text = "";

             //   byte[] data = BufferToSave.ToArray();
              //  string data_s = "";
              //  for (int i = 0; i < data.Length; i++)
              //  {
             //       data_s += data[i].ToString("X2") + " ";
              //  }
              //  char[] data_ans = data_s.ToArray<char>();
               // Buffer_wr = File.AppendText(@path_name + "\\" + ctime + "\\buffer.txt");
               // Buffer_wr.Write(data_ans);
                BufferToSave.Clear();
               // Buffer_wr.Close();
                this.label10.Text = "BufferSize:" + BufferToSave.Count.ToString();
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)

        {
            timer1.Stop();
            timer2.Stop();
            timer3.Stop();
            if (serial1_open_flag)
            {
               // string[] str = SerialPort.GetPortNames();
                        this.serialPort1.Close();

                   
           
                
            }
            if (BufferToSave.Count != 0)
            {
           //     byte[] data = BufferToSave.ToArray();
            //    string data_s = "";
              //  for (int i = 0; i < data.Length; i++)
               // {
               //     data_s += data[i].ToString("X2") + " ";
               // }
               // char[] data_ans = data_s.ToArray<char>();
              //  Buffer_wr = File.AppendText(@path_name + "\\" + ctime + "\\buffer.txt");
              //  Buffer_wr.Write(data_ans);
                BufferToSave.Clear();
             //   Buffer_wr.Close();
            }


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        short x=-1000, y=-1000,vx, vy,target_x,target_y;
        Byte flightMode = 0;
        bool FlightLock = true;
        public void DataAnl(Byte[] R, int len)
        {
            Byte Tsum = 0;
            for (int i = 0; i < len - 1; i++) Tsum += R[i];
            if (!(Tsum == R[len - 1])) return;
            if (!((R[0] == 0xAA) && R[1] == 0xAA)) return;
            if (R[2] == 0x04)
            {
                FlightLock = (R[4] == 0);
                flightMode = R[5];
                x = (short)((R[6] << 8) | (R[7]));
                y = (short)((R[8] << 8) | (R[9]));
                target_x = (short)((R[10] << 8) | (R[11]));
                target_y = (short)((R[12] << 8) | (R[13]));
                //timer3.Start();
                this.label28.Text = "x=" + x.ToString();
                this.label27.Text = "y=" + y.ToString();
                fm.label1.Text = "y="+ y.ToString();
                fm.label2.Text = "x=" + x.ToString();
                PositionRefresh = true;
            }
            else if (R[2] == 0x05)
            {
                FlightLock = (R[4] == 0);
                flightMode = R[5];

            }
            
            //string text = "x=" +x.ToString() + ",vy=" + y.ToString() + ",h=" + vx.ToString() + ",vy=" + vy.ToString();
            //this.richTextBox1.Text = text;
        }


        int state = 0;
        Byte[] RxBufferData = new Byte[100];
        int _data_len = 0,_data_cnt=0;
        public void DataAnPr(Byte[] Data_recive)
        { 
            for (int i = 0; i < Data_recive.Length; i++) {
                Byte data = Data_recive[i];
                if (state == 0 && data == 0xAA)
                {
                    state = 1;
                    RxBufferData[0] = data;
                }
                else if (state == 1 && data == 0xAA)
                {
                    state = 2;
                    RxBufferData[1] = data;
                }
                else if (state == 2 && data < 0XF1)
                {
                    state = 3;
                    RxBufferData[2] = data;
                }
                else if (state == 3 && data < 50)
                {
                    state = 4;
                    RxBufferData[3] = data;
                    _data_len = data;
                    _data_cnt = 0;
                }
                else if (state == 4 && _data_len > 0)
                {
                    _data_len--;
                    RxBufferData[4 + _data_cnt++] = data;
                    if (_data_len == 0)
                        state = 5;
                }
                else if (state == 5)
                {
                    state = 0;
                    RxBufferData[4 + _data_cnt] = data;
                    
                    DataAnl(RxBufferData, _data_cnt + 5);
                }
                else
                    state = 0;
            }
        }



        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (closing_flag) return;
            listening_flag = true;
            int num_now = this.serialPort1.BytesToRead;
            Byte[] Data_recive = new Byte[num_now];
            this.serialPort1.Read(Data_recive, 0, Data_recive.Length);
            BufferToSave.AddRange(Data_recive.ToList<byte>());
            this.label10.Text = "BufferSize:" + BufferToSave.Count.ToString();
            label9.Text = "BytesToRead:" + num_now.ToString();
            DataAnPr(Data_recive);
            //if (this.radioButton1.Checked)
            //{
            //    String str = "";

            //    for (int i = 0; i < Data_recive.Length; i++)
            //    {
            //        str += Data_recive[i].ToString("X2") + " ";

            //    }
            //    this.richTextBox1.Text += str;
            //}
            //else
            //{
            //    label9.Text = "BytesToRead:" + num_now.ToString();
            //    String str = System.Text.Encoding.Default.GetString(Data_recive);
            //    this.richTextBox1.Text += str;

            //}
            listening_flag = false;
        }

        public void DrawGrid(Graphics g)
        {
         //   Graphics g = this.pictureBox1.CreateGraphics();
            Pen mypen = new Pen(Color.Black, 1);
            Graphics g1 = fm.pictureBox1.CreateGraphics();
            mypen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            mypen.DashPattern = new float[] { 5, 5 };
            for (int i = 1; i < 36; i+=1)
            {
                g.DrawLine(mypen, i * 100 / 3600.0f * this.pictureBox1.Width, 0, i * 100 / 3600.0f * this.pictureBox1.Width,
                    this.pictureBox1.Height);
                g1.DrawLine(mypen, i * 100 / 3600.0f * fm.pictureBox1.Width, 0, i * 100 / 3600.0f * fm.pictureBox1.Width,
                    fm.pictureBox1.Height);
            }
            for (int i = 1; i < 24; i+=1)
            {
                g.DrawLine(mypen, 0, i * 100 / 2400.0f * this.pictureBox1.Height, this.pictureBox1.Width,
                   i * 100 / 2400.0f * this.pictureBox1.Height);
                g1.DrawLine(mypen, 0, i * 100 / 2400.0f * fm.pictureBox1.Height, fm.pictureBox1.Width,
                                   i * 100 / 2400.0f * fm.pictureBox1.Height);
            }

        }

        public bool PositionRefresh = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            // this.Resize += new EventHandler(Form1_Resize);
            // autosize = new CtrlAutoSize(this);    //实例化对象并传递"this"
            // autosize.setTag(this);
            


            this.label8.Text = "串口已关闭";
            serial1_open_flag = false;
            text_focus_flag = false;
            listening_flag = false;
            closing_flag = false;
            ctime = currentTime.ToString();
            ctime = ctime.Replace(" ", "");
            ctime = ctime.Replace("/", "");
            ctime = ctime.Replace(":", "");
           // if (!File.Exists(path_name)) Directory.CreateDirectory(@path_name);
          //  Directory.CreateDirectory(@path_name + "\\" + ctime);
         //   Buffer_fs = File.Open(@path_name + "\\" + ctime + "\\buffer.txt", FileMode.Create);
          //  Buffer_fs.Close();

            string[] str = SerialPort.GetPortNames();
            BufferToSave = new List<byte>();

            this.label10.Text = "BufferSize:" + BufferToSave.Count.ToString();
            timer1.Enabled = true;
            timer3.Start();
            if (str.Length == 0)
            {
                comboBox2.Items.Clear();
                return;
            }
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(str);
            comboBox2.SelectedIndex = 0;
            Graphics g = this.pictureBox1.CreateGraphics();
            DrawGrid(g);

        }

        private void comboBox2_Click(object sender, EventArgs e)
        {


        }

        private void button5_Click(object sender, EventArgs e)
        {
            string[] str = SerialPort.GetPortNames();
            if (str.Length == 0)
            {
                this.label8.Text = "本机没有可用串口\n";
                return;
            }
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(str);
            comboBox2.SelectedIndex = 0;


        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!serial1_open_flag)
            {
                this.label8.Text = "串口未打开\n";
                return;

            }
            this.serialPort1.Write("\r\n");
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (serial1_open_flag)
            {
                Byte[] co = new Byte[] { 0xAA, 0xAF, 0x21, 0x01, 0xff, 0x7A };
                this.serialPort1.Write(co, 0, 6);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (text_focus_flag) return;
            this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
            this.richTextBox1.ScrollToCaret();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (BufferToSave.Count > 10000)
            //{
            //    byte[] data = BufferToSave.ToArray();
            //    string data_s = "";
            //    for (int i = 0; i < data.Length; i++)
            //    {
            //        data_s += data[i].ToString("X2") + " ";
            //    }
            //    char[] data_ans = data_s.ToArray<char>();
            //    Buffer_wr = File.AppendText(@path_name + "\\" + ctime + "\\buffer.txt");
            //    Buffer_wr.Write(data_ans);
            //    BufferToSave.Clear();
            //    Buffer_wr.Close();
            //    this.label10.Text = "BufferSize:" + BufferToSave.Count.ToString();
            //}
            timer1.Start();
        }

        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            text_focus_flag = true;
        }

        private void richTextBox1_Leave(object sender, EventArgs e)
        {
            text_focus_flag = false;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (radioButton3.Checked)
            {
                char temp = e.KeyChar;
                if (!((temp >= 48 && temp <= 57) || (temp <= 70 && temp >= 65) || (temp >= 97 && temp <= 102) || (temp == 8) || (temp == 32)))
                {
                    e.Handled = true;
                }
            }

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            byte[] data = BufferToSave.ToArray();
            string data_s = "";
            for (int i = 0; i < data.Length; i++)
            {
                data_s += data[i].ToString("X2") + " ";
            }
            char[] data_ans = data_s.ToArray<char>();
         //   Buffer_wr = File.AppendText(@path_name + "\\" + ctime + "\\buffer.txt");
         //   Buffer_wr.Write(data_ans);
         //  BufferToSave.Clear();
         //  Buffer_wr.Close();
            this.label10.Text = "BufferSize:" + BufferToSave.Count.ToString();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.richTextBox1.Text = "";
            }
            timer2.Start();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (serial1_open_flag)
            {
                Byte[] co = new Byte[] { 0xAA, 0xAF, 0x20, 0x01, 0xff, 0x79 };
                this.serialPort1.Write(co, 0, 6);
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
        private void button11_Click(object sender, EventArgs e)
        {
            //if (serial1_open_flag)
            //{

            //    UInt16 pitch = UInt16.Parse(this.pitch.Text), roll = UInt16.Parse(this.roll.Text),
            //        yaw = UInt16.Parse(this.yaw.Text), thr = UInt16.Parse(this.thr.Text);
            //    byte[] se = new byte[21];
            //    se[0] = 0xaa; se[1] = 0xaf; se[2] = 0x03; se[3] = 16;
            //    se[4] = (byte)(thr >> 8); se[5] = (byte)(thr);
            //    se[6] = (byte)(yaw >> 8); se[7] = (byte)(yaw);
            //    se[8] = (byte)(roll >> 8); se[9] = (byte)(roll);
            //    se[10] = (byte)(pitch >> 8); se[11] = (byte)(pitch);
            //    for (int i = 12; i < 20; i++) se[i] = 0;
            //    se[20] = 0;
            //    for (int i = 0; i < 20; i++)
            //    {
            //        se[20] += se[i];
            //    }
            //    this.serialPort1.Write(se, 0, 21);
            //}
            if (serial1_open_flag)
            {

                Byte[] co1 = new Byte[6] { 0xAA, 0xAF, 0x01, 0x01, 0x03, 0x00 };
                for (int i = 0; i < 5; i++)
                {
                    co1[5] += co1[i];
                }
                this.serialPort1.Write(co1, 0, 6);
                co1[4] = 0x04;
                co1[5] = 0x00;
                for (int i = 0; i < 5; i++)
                {
                    co1[5] += co1[i];
                }
                this.serialPort1.Write(co1, 0, 6);
            }
        }

        private void thr_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
        public class CtrlAutoSize
        {
            System.Windows.Forms.Form thisForm;
            private float x, y;
            float newx, newy;

            public CtrlAutoSize(System.Windows.Forms.Form form)
            {
                thisForm = form;
                x = thisForm.Width;
                y = thisForm.Height;
            }

            public void setTag(Control cons)
            {

                foreach (Control con in cons.Controls)
                {
                    con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                    if (con.Controls.Count > 0)
                    {
                        setTag(con);
                    }
                }
            }
            public void setControls(Control cons)
            {
                newx = cons.Width / x;
                newy = cons.Height / y;
                foreach (Control con in cons.Controls)
                {
                    string[] mytag = con.Tag.ToString().Split(new char[] { (':') });
                    float a = Convert.ToSingle(mytag[0]) * newx;
                    con.Width = (int)a;
                    a = Convert.ToSingle(mytag[1]) * newy;
                    con.Height = (int)a;
                    a = Convert.ToSingle(mytag[2]) * newx;
                    con.Left = (int)a;
                    a = Convert.ToSingle(mytag[3]) * newy;
                    con.Top = (int)a;
                    Single currentSize = Convert.ToSingle(mytag[4]) * newy;
                    con.Font = new System.Drawing.Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                    {
                        setControls(con);
                    }
                }
            }
        }
        CtrlAutoSize autosize;
        private void thr_KeyDown(object sender, KeyEventArgs e)
        {
            if (serial1_open_flag)
            {
                if (e.KeyValue == 13)
                {

                    UInt16 pitch = UInt16.Parse(this.pitch.Text), roll = UInt16.Parse(this.roll.Text),
                        yaw = UInt16.Parse(this.yaw.Text), thr = UInt16.Parse(this.thr.Text);
                    byte[] se = new byte[21];
                    se[0] = 0xaa; se[1] = 0xaf; se[2] = 0x03; se[3] = 16;
                    se[4] = (byte)(thr >> 8); se[5] = (byte)(thr);
                    se[6] = (byte)(yaw >> 8); se[7] = (byte)(yaw);
                    se[8] = (byte)(roll >> 8); se[9] = (byte)(roll);
                    se[10] = (byte)(pitch >> 8); se[11] = (byte)(pitch);
                    for (int i = 12; i < 20; i++) se[i] = 0;
                    se[20] = 0;
                    for (int i = 0; i < 20; i++)
                    {
                        se[20] += se[i];
                    }
                    this.serialPort1.Write(se, 0, 21);
                }
            }
        }

        private void roll_KeyDown(object sender, KeyEventArgs e)
        {
            if (serial1_open_flag)
            {
                if (e.KeyValue == 13)
                {
                    UInt16 pitch = UInt16.Parse(this.pitch.Text), roll = UInt16.Parse(this.roll.Text),
                        yaw = UInt16.Parse(this.yaw.Text), thr = UInt16.Parse(this.thr.Text);
                    byte[] se = new byte[21];
                    se[0] = 0xaa; se[1] = 0xaf; se[2] = 0x03; se[3] = 16;
                    se[4] = (byte)(thr >> 8); se[5] = (byte)(thr);
                    se[6] = (byte)(yaw >> 8); se[7] = (byte)(yaw);
                    se[8] = (byte)(roll >> 8); se[9] = (byte)(roll);
                    se[10] = (byte)(pitch >> 8); se[11] = (byte)(pitch);
                    for (int i = 12; i < 20; i++) se[i] = 0;
                    se[20] = 0;
                    for (int i = 0; i < 20; i++)
                    {
                        se[20] += se[i];
                    }
                    this.serialPort1.Write(se, 0, 21);
                }
            }
        }

        private void yaw_KeyDown(object sender, KeyEventArgs e)
        {
            if (serial1_open_flag)
            {
                if (e.KeyValue == 13)
                {
                    UInt16 pitch = UInt16.Parse(this.pitch.Text), roll = UInt16.Parse(this.roll.Text),
                        yaw = UInt16.Parse(this.yaw.Text), thr = UInt16.Parse(this.thr.Text);
                    byte[] se = new byte[21];
                    se[0] = 0xaa; se[1] = 0xaf; se[2] = 0x03; se[3] = 16;
                    se[4] = (byte)(thr >> 8); se[5] = (byte)(thr);
                    se[6] = (byte)(yaw >> 8); se[7] = (byte)(yaw);
                    se[8] = (byte)(roll >> 8); se[9] = (byte)(roll);
                    se[10] = (byte)(pitch >> 8); se[11] = (byte)(pitch);
                    for (int i = 12; i < 20; i++) se[i] = 0;
                    se[20] = 0;
                    for (int i = 0; i < 20; i++)
                    {
                        se[20] += se[i];
                    }
                    this.serialPort1.Write(se, 0, 21);
                }
            }
        }

        private void pitch_KeyDown(object sender, KeyEventArgs e)
        {
            if (serial1_open_flag)
            {
                if (e.KeyValue == 13)
                {
                    UInt16 pitch = UInt16.Parse(this.pitch.Text), roll = UInt16.Parse(this.roll.Text),
                        yaw = UInt16.Parse(this.yaw.Text), thr = UInt16.Parse(this.thr.Text);
                    byte[] se = new byte[21];
                    se[0] = 0xaa; se[1] = 0xaf; se[2] = 0x03; se[3] = 16;
                    se[4] = (byte)(thr >> 8); se[5] = (byte)(thr);
                    se[6] = (byte)(yaw >> 8); se[7] = (byte)(yaw);
                    se[8] = (byte)(roll >> 8); se[9] = (byte)(roll);
                    se[10] = (byte)(pitch >> 8); se[11] = (byte)(pitch);
                    for (int i = 12; i < 20; i++) se[i] = 0;
                    se[20] = 0;
                    for (int i = 0; i < 20; i++)
                    {
                        se[20] += se[i];
                    }
                    this.serialPort1.Write(se, 0, 21);
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (serial1_open_flag)
            {
                Byte[] co = new Byte[6] { 0xAA, 0xAF, 0x22, 0x01, 0x03, 0x7F };
                co[4] = byte.Parse(this.mode.Text);
                co[5] = 0;
                for (int i = 0; i < 5; i++)
                {
                    co[5] += co[i];
                }
                this.serialPort1.Write(co, 0, 6);
            }
        }
        short xLast = -1000, yLast = -1000;
        int targetLastx = -1000, targetLasty = -1000,targetLastx1=-1000,targetLasty1=-1000;
        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            
            if (!PositionRefresh)
            {
                if (FlightLock)
                {
                    this.pictureBox2.BackColor = Color.Green;
                    label23.Text = "已上锁    " + "模式" + flightMode.ToString();
                }
                else
                {
                    this.pictureBox2.BackColor = Color.Red;
                    label23.Text = "已解锁    " + "模式" + flightMode.ToString();
                }
                return; }
            Font drawFont = new Font("Arial", 16);
            Graphics g = this.pictureBox1.CreateGraphics();
            Pen mypen = new Pen(Color.Green, 5);
            Pen linePen = new Pen(Color.Red, 3);
            if (xLast == -1000) { xLast = x;yLast = y; timer3.Start(); return; }
            
            int xL_draw = xLast * pictureBox1.Width / 360, yL_draw = yLast * pictureBox1.Height / 240,
                x_draw = x * pictureBox1.Width / 360, y_draw = y * pictureBox1.Height / 240,
                x_target = target_x * pictureBox1.Width / 360, y_target = target_y * pictureBox1.Height / 240;
            if (targetLastx == -1000) { targetLastx = x_draw; targetLasty = y_draw;  }
            g.DrawLine(mypen, xL_draw, yL_draw, x_draw, y_draw);
            
         //   DrawTarget(x_target, y_target);
          //  g.DrawLine(linePen, targetLastx, targetLasty, x_target, y_target);
            targetLastx = x_target;targetLasty = y_target;


            Graphics g1 = fm.pictureBox1.CreateGraphics();
            int xL_draw1 = xLast * fm.pictureBox1.Width / 360, yL_draw1 = yLast * fm.pictureBox1.Height / 240,
                x_draw1 = x * fm.pictureBox1.Width / 360, y_draw1 = y * fm.pictureBox1.Height / 240,
            x_target1 = target_x * fm.pictureBox1.Width / 360, y_target1 = target_y * fm.pictureBox1.Height / 240;
            if (targetLastx1 == -1000) { targetLastx1 = x_draw; targetLasty1 = y_draw; }

            g1.DrawLine(mypen, xL_draw1, yL_draw1, x_draw1, y_draw1);
        //    fm.DrawTarget(x_target1, y_target1);
         //   g1.DrawLine(linePen, targetLastx, targetLasty, x_target, y_target);
            targetLastx1 = x_target1; targetLasty1 = y_target1;
            xLast = x;
            yLast = y;

            PositionRefresh = false;
            if (FlightLock) { this.pictureBox2.BackColor = Color.Green;
                label23.Text = "已上锁    " + "模式" + flightMode.ToString();
                xLast = -1000;
            } else
            {
                this.pictureBox2.BackColor = Color.Red;
                label23.Text = "已解锁    " + "模式" + flightMode.ToString();
            }
            
            timer3.Start();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        public void DrawTarget(int x,int y)
        {
            Graphics g = this.pictureBox1.CreateGraphics();
            Pen mypen = new Pen(Color.Red, 2);
          //  int xTarget = Int16.Parse(this.textBox6.Text) * pictureBox1.Width / 3600,
           // yTarget = Int16.Parse(this.textBox7.Text) * pictureBox1.Height / 2400;
            g.DrawLine(mypen, x, y + 10, x, y - 10);
            g.DrawLine(mypen, x + 10, y, x - 10, y);
        }
        private void button15_Click(object sender, EventArgs e)
        {
          //  Graphics g = this.pictureBox1.CreateGraphics();
           // Pen mypen = new Pen(Color.Red, 2);
            int xTarget = Int16.Parse(this.textBox6.Text)*pictureBox1.Width/3600,
            yTarget = Int16.Parse(this.textBox7.Text)*pictureBox1.Height/2400;
            //g.DrawLine(mypen, xTarget, yTarget+10, xTarget, yTarget-10);
            //g.DrawLine(mypen, xTarget + 10, yTarget, xTarget - 10, yTarget);
            DrawTarget(xTarget, yTarget);

          //  Graphics g1 = fm.pictureBox1.CreateGraphics();
          //  int xTarget = Int16.Parse(this.textBox6.Text) * pictureBox1.Width / 3600,
         //   yTarget = Int16.Parse(this.textBox7.Text) * pictureBox1.Height / 2400;
          //  g.DrawLine(mypen, xTarget, yTarget + 10, xTarget, yTarget - 10);
          //  g.DrawLine(mypen, xTarget + 10, yTarget, xTarget - 10, yTarget);

        }
        public void Init(Graphics g,PictureBox p)
        {
            Pen mypen = new Pen(Color.Red, 2);
          //  g.Clear(Color.White);
            DrawGrid(g);
            Point point1 = new Point( 944 * p.Width / 3600, 922 * p.Height / 2400),
                point2 = new Point(2656 * p.Width / 3600, 922 * p.Height / 2400),
                point3 = new Point(1271 * p.Width / 3600, 1928 * p.Height / 2400),
                point4 = new Point(1800 * p.Width / 3600, 300 * p.Height / 2400),
                point5 = new Point(2329 * p.Width / 3600, 1928 * p.Height / 2400);
            Point point6 = point1;
            Point[] path = new Point[6];
            path[0] = point1;
            path[1] = point2;
            path[2] = point3;
            path[3] = point4;
            path[4] = point5;
            path[5] = point6;
            g.DrawLines(mypen, path);
        }
        private void button18_Click(object sender, EventArgs e)
        {
            Graphics g = this.pictureBox1.CreateGraphics();
            Init(g,this.pictureBox1);

        }

        private void richTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (serial1_open_flag)
            {
                if (e.KeyValue == 38)          //w
                {
                    UInt16 pitch = 1, roll = 0,
                           yaw = 0, thr = 0;
                    byte[] se = new byte[21];
                    se[0] = 0xaa; se[1] = 0xaf; se[2] = 0x03; se[3] = 16;
                    se[4] = (byte)(thr >> 8); se[5] = (byte)(thr);
                    se[6] = (byte)(yaw >> 8); se[7] = (byte)(yaw);
                    se[8] = (byte)(roll >> 8); se[9] = (byte)(roll);
                    se[10] = (byte)(pitch >> 8); se[11] = (byte)(pitch);
                    for (int i = 12; i < 20; i++) se[i] = 0;
                    se[20] = 0;
                    for (int i = 0; i < 20; i++)
                    {
                        se[20] += se[i];
                    }
                    this.serialPort1.Write(se, 0, 21);
                    this.richTextBox2.Text = "GoAhead";

                }
                if (e.KeyValue == 40)          //s
                {
                    UInt16 pitch = 2, roll = 0,
                           yaw = 0, thr = 0;
                    byte[] se = new byte[21];
                    se[0] = 0xaa; se[1] = 0xaf; se[2] = 0x03; se[3] = 16;
                    se[4] = (byte)(thr >> 8); se[5] = (byte)(thr);
                    se[6] = (byte)(yaw >> 8); se[7] = (byte)(yaw);
                    se[8] = (byte)(roll >> 8); se[9] = (byte)(roll);
                    se[10] = (byte)(pitch >> 8); se[11] = (byte)(pitch);
                    for (int i = 12; i < 20; i++) se[i] = 0;
                    se[20] = 0;
                    for (int i = 0; i < 20; i++)
                    {
                        se[20] += se[i];
                    }
                    this.serialPort1.Write(se, 0, 21);
                    this.richTextBox2.Text = "GoBack";


                }
                if (e.KeyValue == 37)          //a
                {
                    UInt16 pitch = 3, roll = 0,
                           yaw = 0, thr = 0;
                    byte[] se = new byte[21];
                    se[0] = 0xaa; se[1] = 0xaf; se[2] = 0x03; se[3] = 16;
                    se[4] = (byte)(thr >> 8); se[5] = (byte)(thr);
                    se[6] = (byte)(yaw >> 8); se[7] = (byte)(yaw);
                    se[8] = (byte)(roll >> 8); se[9] = (byte)(roll);
                    se[10] = (byte)(pitch >> 8); se[11] = (byte)(pitch);
                    for (int i = 12; i < 20; i++) se[i] = 0;
                    se[20] = 0;
                    for (int i = 0; i < 20; i++)
                    {
                        se[20] += se[i];
                    }
                    this.serialPort1.Write(se, 0, 21);
                    this.richTextBox2.Text = "TurnLeft";


                }
                if (e.KeyValue ==39)          //d
                {
                    UInt16 pitch = 4, roll = 0,
                           yaw = 0, thr = 0;
                    byte[] se = new byte[21];
                    se[0] = 0xaa; se[1] = 0xaf; se[2] = 0x03; se[3] = 16;
                    se[4] = (byte)(thr >> 8); se[5] = (byte)(thr);
                    se[6] = (byte)(yaw >> 8); se[7] = (byte)(yaw);
                    se[8] = (byte)(roll >> 8); se[9] = (byte)(roll);
                    se[10] = (byte)(pitch >> 8); se[11] = (byte)(pitch);
                    for (int i = 12; i < 20; i++) se[i] = 0;
                    se[20] = 0;
                    for (int i = 0; i < 20; i++)
                    {
                        se[20] += se[i];
                    }
                    this.serialPort1.Write(se, 0, 21);
                    this.richTextBox2.Text = "TurnRight";
                }
                if (e.KeyValue == ' ')          //space
                {
                    UInt16 pitch = 5, roll = 0,
                           yaw = 0, thr = 0;
                    byte[] se = new byte[21];
                    se[0] = 0xaa; se[1] = 0xaf; se[2] = 0x03; se[3] = 16;
                    se[4] = (byte)(thr >> 8); se[5] = (byte)(thr);
                    se[6] = (byte)(yaw >> 8); se[7] = (byte)(yaw);
                    se[8] = (byte)(roll >> 8); se[9] = (byte)(roll);
                    se[10] = (byte)(pitch >> 8); se[11] = (byte)(pitch);
                    for (int i = 12; i < 20; i++) se[i] = 0;
                    se[20] = 0;
                    for (int i = 0; i < 20; i++)
                    {
                        se[20] += se[i];
                    }
                    this.serialPort1.Write(se, 0, 21);
                    this.richTextBox2.Text = "stop";
                }
            }

        }

        private void yaw_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Form1_Resize(object sender, EventArgs e)
        {

        }
         Form2 fm = new Form2();

        private void button17_Click(object sender, EventArgs e)
        {
            Graphics g = this.pictureBox1.CreateGraphics();
            DrawGrid(g);
        }

        private void Form1_Resize_1(object sender, EventArgs e)
        {
            Graphics g = this.pictureBox1.CreateGraphics();
            DrawGrid(g);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            fm.Show();
        }

        private void mode_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyValue == 13)
            {
                if (serial1_open_flag)
                {
                    Byte[] co = new Byte[6] { 0xAA, 0xAF, 0x22, 0x01, 0x03, 0x7F };
                    co[4] = byte.Parse(this.mode.Text);
                    co[5] = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        co[5] += co[i];
                    }
                    this.serialPort1.Write(co, 0, 6);
                }
            }

        }

        private void button13_Click(object sender, EventArgs e)
        {
            byte[] mode = new byte[6] { 0xAA, 0xAF, 0x22, 0x01, 0x03, 0x7F };
            mode[4] = byte.Parse(this.mode.Text);
            mode[5] = 0;
            for (int i = 0; i < 5; i++)
            {
                mode[5] += mode[i];
            }
            byte[] unlock_m = new byte[] { 0xAA, 0xAF, 0x20, 0x01, 0xff, 0x79 };
            byte[] lock_m = new byte[] { 0xAA, 0xAF, 0x21, 0x01, 0xff, 0x7A };
            UInt16 pitch = UInt16.Parse(this.pitch.Text), roll = UInt16.Parse(this.roll.Text),
                    yaw = UInt16.Parse(this.yaw.Text), thr = UInt16.Parse(this.thr.Text);
            byte[] se = new byte[21];
            se[0] = 0xaa; se[1] = 0xaf; se[2] = 0x03; se[3] = 16;
            se[4] = (byte)(thr >> 8); se[5] = (byte)(thr);
            se[6] = (byte)(yaw >> 8); se[7] = (byte)(yaw);
            se[8] = (byte)(roll >> 8); se[9] = (byte)(roll);
            se[10] = (byte)(pitch >> 8); se[11] = (byte)(pitch);
            for (int i = 12; i < 20; i++) se[i] = 0;
            se[20] = 0;
            for (int i = 0; i < 20; i++)
            {
                se[20] += se[i];
            }
            string ans="";
            for (int i = 0; i < 6; i++)
            {
                ans += mode[i].ToString("X2");
                ans += " ";
            }
            this.textBox4.Text = ans;
            ans = "";
            for (int i = 0; i < 6; i++)
            {
                ans += unlock_m[i].ToString("X2");
                ans += " ";
            }
            this.textBox2.Text = ans;
            ans = "";
            for (int i = 0; i < 6; i++)
            {
                ans += lock_m[i].ToString("X2");
                ans += " ";
            }
            this.textBox5.Text = ans;
            ans = "";
            for (int i = 0; i < 21; i++)
            {
                ans += se[i].ToString("X2");
                ans += " ";
            }
            this.textBox3.Text = ans;





        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        
        private void button14_Click(object sender, EventArgs e)
        {
            Graphics g = this.pictureBox1.CreateGraphics();
            g.Clear(Color.White);
            DrawGrid(g);

        }
    }
}
