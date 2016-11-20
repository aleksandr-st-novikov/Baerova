using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebUI;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string[] _params = new string[] { "noreply@baeroff.com", "Baeroff.com", "Az_1234", "mail.baeroff.com", "587", "0"};
            await WebUI.Helpers.Mailing.SendNews(_params);
        }
    }
}
