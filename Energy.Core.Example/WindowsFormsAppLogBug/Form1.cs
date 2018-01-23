using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsAppLogBug
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Energy.Core.Bug.Write("Form Load");

            //Energy.Core.Log.Default.Destination += new Energy.Log.Target.Event();
            Energy.Core.Log logger = new Energy.Core.Log();
            logger.Maximum = 10;     
        }
    }
}
