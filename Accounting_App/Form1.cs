using System;
using System.Windows.Forms;
using Accounting.DataLayer;
using Accounting.DataLayer.Services;
using Accounting.Utility.Convertor;

namespace Accounting_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FrmCustomers frmCustomers = new FrmCustomers();
            frmCustomers.ShowDialog();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.ToShamsi();
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FrmNewTransactions  frmNewTransactions = new FrmNewTransactions();
            frmNewTransactions.ShowDialog();
        }

        private void btnReportPay_Click(object sender, EventArgs e)
        {
            FrmReport frmReport = new FrmReport();
            frmReport.TypeId = 2;
            frmReport.ShowDialog();
        }

        private void btnReportRecive_Click(object sender, EventArgs e)
        {
            FrmReport frmReport = new FrmReport();
            frmReport.TypeId = 1;
            frmReport.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
