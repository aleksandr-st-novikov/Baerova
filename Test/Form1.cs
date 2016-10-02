using Domain.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebUI.Helpers;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (EFConstantContext constantContext = new EFConstantContext())
            {
                string[] _params = null;
                //MessageBox.Show(Mailing.Send(_params));
            }
        }
    }
}
