using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using Accounting.Utility.Convertor;
using Accounting.ViewModels.Customers;

namespace Accounting_App
{
    public partial class FrmReport : Form
    {
        public int TypeId = 0;
        public FrmReport()
        {
            InitializeComponent();
        }

        private void FrmReport_Load(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                List<ListCustomerViewModel> list = new List<ListCustomerViewModel>();
                list.Add(new ListCustomerViewModel()
                {
                    CustomerID = 0,
                    FullName = "انتخاب کنید"
                });
                list.AddRange(db.CustomerRepository.GetNameCustomer());
                cbCustomer.DataSource = list;
                cbCustomer.DisplayMember = "FullName";
                cbCustomer.ValueMember = "CustomerID";
            }

            if (TypeId == 2)
            {
                Text = "گزارش پرداختی ها";
            }
            else
            {
                Text = "گزارش دریافتی ها";
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            Filter();
        }

        void Filter()
        {
            using (UnitOfWork db = new UnitOfWork())
            {

                List<Accounting.DataLayer.Accounting> result = new List<Accounting.DataLayer.Accounting>();

                DateTime? starDate;
                DateTime? endDate;

                if ((int)cbCustomer.SelectedValue != 0)
                {
                    int customerId = int.Parse(cbCustomer.SelectedValue.ToString());
                    result.AddRange(db.AccountinGenericRepository.Get(a => a.TypeID == TypeId && a.CustomerID == customerId));
                }
                else
                {
                    result.AddRange(db.AccountinGenericRepository.Get(a => a.TypeID == TypeId));
                }

                if (txtFromDate.Text != "    /  /")
                {
                    starDate = Convert.ToDateTime(txtFromDate.Text);

                    result = result.Where(r => r.DateTime >= starDate.Value).ToList();
                }
                if (txtToDate.Text != "    /  /")
                {
                    endDate = Convert.ToDateTime(txtToDate.Text);

                    result = result.Where(r => r.DateTime <= endDate.Value).ToList();
                }




                /*dgReport.AutoGenerateColumns = false;
                dgReport.DataSource = result;*/
                dgReport.Rows.Clear();
                foreach (var Accounting in result)
                {
                    String customerName = db.CustomerRepository.GetCustomerNameById(Accounting.CustomerID);
                    dgReport.Rows.Add(Accounting.ID, customerName, Accounting.Amount, Accounting.DateTime.ToShamsi(), Accounting.Description);
                }
            }
        }

        private void dgReport_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Filter();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgReport.CurrentRow != null)
            {
                int id = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                if (RtlMessageBox.Show("از حذف مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        db.AccountinGenericRepository.Delete(id);
                        db.save();
                        Filter();
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgReport.CurrentRow != null)
            {
                int id = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                FrmNewTransactions frmNew = new FrmNewTransactions();
                frmNew.AccountId = id;
                if (frmNew.ShowDialog() == DialogResult.OK)
                {
                    Filter();
                }
            }
        }
    }
}
