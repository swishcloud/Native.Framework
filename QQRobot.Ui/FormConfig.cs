using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QQRobot.Ui
{
    public partial class FormConfig : Form
    {
        public static FormConfig Instance { get; set; }
        private FormConfig()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            var regexs= Config.Instance.ReadRegexsConfig();
            foreach(var i in regexs)
            {
                txtRegexs.AppendText(i+"\r\n");
            }
        }
        static FormConfig()
        {
            Instance = new FormConfig();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Config.Instance.SaveRegexsConfig(txtRegexs.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            MessageBox.Show("ok");
        }
    }
}
