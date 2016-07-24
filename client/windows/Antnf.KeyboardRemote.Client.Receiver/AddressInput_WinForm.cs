using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Antnf.KeyboardRemote.Client.Receiver
{
    public partial class AddressInput_WinForm : Form
    {
        public string url = "";
        public AddressInput_WinForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            url = textBox1.Text;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
