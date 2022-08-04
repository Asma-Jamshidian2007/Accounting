using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using ValidationComponents;


namespace Accounting_App
{
    public partial class FrmAddOrEditCustomer : Form
    {
        public int customerID = 0;
        UnitOfWork db = new UnitOfWork();
        public FrmAddOrEditCustomer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                pcCustomerPicture.ImageLocation = openFile.FileName;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                var imageName = $"{Guid.NewGuid()}{Path.GetExtension(pcCustomerPicture.ImageLocation)}";
                var path = Path.Combine(Application.StartupPath, "Images");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                pcCustomerPicture.Image.Save(Path.Combine(path, imageName));
                var customers = new Customers()
                {
                    Address = txtAddress.Text,
                    Email = txtEmail.Text,
                    FullName = txtName.Text,
                    PhoneNumber = txtPhoneNumber.Text,
                    CustomerImage = imageName
                };
                if (customerID == 0)
                {
                    db.CustomerRepository.InsertCustomer(customers);
                }
                else
                {
                    customers.CustomerID = customerID;
                    db.CustomerRepository.UpdateCustomer(customers);
                }

                db.save();
                DialogResult = DialogResult.OK;
            }
        }

        private void FrmAddOrEditCustomer_Load(object sender, EventArgs e)
        {
            if (customerID != 0)
            {
                Text = "ویرایش شخص";
                btnSave.Text = "ویرایش";
                var customer = db.CustomerRepository.GetCustomerById(customerID);
                txtName.Text = customer.FullName;
                txtPhoneNumber.Text = customer.PhoneNumber;
                txtEmail.Text = customer.Email;
                txtAddress.Text = customer.Address;
                pcCustomerPicture.ImageLocation =
                    Path.Combine(Application.StartupPath, "image", customer.CustomerImage.Trim());
            }
        }
    }
}
