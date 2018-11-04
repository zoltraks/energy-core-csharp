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
            Energy.Core.Log.Logger logger = new Energy.Core.Log.Logger();
            logger.Maximum = 10;     
        }

        private void buttonAddMessage_Click(object sender, EventArgs e)
        {
            //Energy.Core.Log.Default.Destination.Add(new Energy.Core.Log.Target.Event())
            Energy.Core.Log.Default.Write(textBoxMessage.Text, Energy.Enumeration.LogLevel.Message);
        }
    }
}
