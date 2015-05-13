using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        CustomSerialPort serial;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargarVelocidadesTransmision();
            listarPuertos();
        }

        #region Baudios
        private void cargarVelocidadesTransmision()
        {
            cboBaudios.Items.Clear();
            cboBaudios.Items.Add(2400);
            cboBaudios.Items.Add(4800);
            cboBaudios.Items.Add(9600);
            cboBaudios.Items.Add(14400);
            cboBaudios.Items.Add(19200);
            cboBaudios.Items.Add(28800);
            cboBaudios.Items.Add(38400);
            cboBaudios.SelectedIndex = 0;
        }
        #endregion

        #region Carga la lista de puertos serial de la pc
        private void listarPuertos()
        {
            cboPuerto.Items.Clear();
            string[] puertos = SerialPort.GetPortNames();
            foreach (string puerto in puertos)
            {
                cboPuerto.Items.Add(puerto);
            }
            cboPuerto.SelectedIndex = 0;
        }
        #endregion


        #region Evento clic de Boton Conectar
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (serial == null || !serial.SerialPort.IsOpen)
                {
                    serial = new CustomSerialPort(cboPuerto.Text, int.Parse(cboBaudios.Text)); 
                    serial.SerialPort.DataReceived +=SerialPort_DataReceived;
                    button2.Text = "Desconectar";
                    ConfigurarBotones(true);
                }
                else if (serial.SerialPort.IsOpen)
                {
                    serial.SerialPort.Close();
                    serial.SerialPort.Dispose();
                    button2.Text = "Conectar";
                    ConfigurarBotones(false);
                    textDistancia.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Configuracion de Botones
        private void ConfigurarBotones(bool flag){
            button1.Enabled = flag;
            button3.Enabled = flag;
            button4.Enabled = flag;
            button5.Enabled = flag;
        }
        #endregion


        #region Eventos de Botones de control
        private void button5_Click(object sender, EventArgs e)
        {
            serial.evaluarKey('w');
        }

        private void button1_Click(object sender, EventArgs e)
        {

            serial.evaluarKey('a');
        }

        private void button4_Click(object sender, EventArgs e)
        {
            serial.evaluarKey('s');
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            serial.evaluarKey('d');
        }


        private void button5_KeyPress(object sender, KeyPressEventArgs e)
        {
            serial.evaluarKey(e.KeyChar);
        }

        private void button1_KeyPress(object sender, KeyPressEventArgs e)
        {
            serial.evaluarKey(e.KeyChar);
        }

        private void button4_KeyPress(object sender, KeyPressEventArgs e)
        {
            serial.evaluarKey(e.KeyChar);
        }

        private void button3_KeyPress(object sender, KeyPressEventArgs e)
        {
            serial.evaluarKey(e.KeyChar);
        }
        #endregion

        #region Evento para recibir datos de serial
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string POT = serial.SerialPort.ReadLine();
            this.BeginInvoke(new LineReceivedEvent(LineReceived), POT);
        }

        private delegate void LineReceivedEvent(string POT);

        private void LineReceived(string POT)
        {
            textDistancia.Text = POT;
            serial.sleep();
        }
        #endregion
    }
}
