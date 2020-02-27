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
    public partial class MainForm : Form
    {
        public static MainForm Instance { get; set; }
        private MainForm()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        static MainForm()
        {
            Instance = new MainForm();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public void Log(string str)
        {
            this.Invoke(new Action(() =>
            {
                lbLogs.Items.Add(str);
                lbLogs.TopIndex = lbLogs.Items.Count - 1;
            }));
        }

        private void menuBtnConfig_Click(object sender, EventArgs e)
        {
            new FormConfig().ShowDialog();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            base.OnClosing(e);
        }
    }
}
