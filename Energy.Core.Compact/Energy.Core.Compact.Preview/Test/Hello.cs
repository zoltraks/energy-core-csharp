using System;

using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Energy.Core.Compact.Example.Test
{
    public class Hello: Energy.Interface.ITest
    {
        public void Test()
        {
            try
            {
                string commandName = Energy.Core.Application.GetCommandName();
                if (string.IsNullOrEmpty(commandName))
                {
                    throw new Exception("GetCommandName returned empty string");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    exception.Message,
                    "Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
            }
        }
    }
}
