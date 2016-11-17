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
namespace Com_port_tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = textBox1.Text;
                serialPort1.BaudRate = int.Parse(textBox2.Text);
                serialPort1.DataBits = 8;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Parity = Parity.None;
                serialPort1.Open();
                serialPort1.DataReceived += ComPort_DataReceived;
                //serialPort1. += ComPort_DataReceived;
                label3.Text = "Connected";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }
        void ComPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (true)
            {
                if (serialPort1.IsOpen == false) break;
                String str = to_hex_string(serialPort1.ReadLine());
                if (checkBox1.Checked)
                {
                    
                    //richTextBox1.Text += str;
                    richTextBox1.Invoke((MethodInvoker)delegate()
                    {
                        richTextBox1.Text += str + "\n";
                        
                    });
                }
            }
        }
        string to_hex_string(string str)
        {
            char[] charValues = str.ToCharArray();
            string hexOutput = "";
            foreach (char _eachChar in charValues)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(_eachChar);
                // Convert the decimal value to a hexadecimal value in string form.
                hexOutput += String.Format("-0x{0:X}", value);
                // to make output as your eg 
                //  hexOutput +=" "+ String.Format("{0:X}", value);

            }
            return hexOutput;
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
        }
        public static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            hex = hex.Replace(";", "");
            hex = hex.Replace(".", "");
            hex = hex.Replace(",", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            
            return raw;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] bytes = FromHex(textBox3.Text);
                serialPort1.Write(bytes, 0, bytes.Length);
                string str = System.Text.Encoding.Default.GetString(bytes);
                richTextBox2.Text += to_hex_string(str) + "\n";
                label3.Text = "Connected";
            }
            catch (Exception ex)
            {
                label3.Text = "Disconnected";
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
        }
    }
}
