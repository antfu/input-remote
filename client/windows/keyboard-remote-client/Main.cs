using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Antnf.KeyboardRemote.Client
{
    public partial class Main : Form
    {
        private Receiver receiver;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            receiver = new Receiver("ws://localhost/ws/r");
            
        }
        
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            receiver.Connect();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            receiver.Close();
        }

    }
}
