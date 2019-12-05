using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omok
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            Hide();
            playForm PlayForm = new playForm();
            PlayForm.FormClosed += new FormClosedEventHandler(childForm_Closed);
            PlayForm.Show();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
        void childForm_Closed(object sender ,FormClosedEventArgs e)
        {
            Show();
        }
    }
}
