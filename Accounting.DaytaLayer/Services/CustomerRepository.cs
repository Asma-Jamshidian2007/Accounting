using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Accounting.DataLayer.Repository;
using Accounting.ViewModels.Customers;

namespace Accounting.DataLayer.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly Accounting_DBEntities _db;
        public CustomerRepository(Accounting_DBEntities context)
        {
            _db = context;
        }
        public List<Customers> GetAllCustomers()
        {
            return _db.Customers.ToList();
        }

        public Customers GetCustomerById(int customerId)
        {
            return _db.Customers.Find(customerId);
        }

        public bool InsertCustomer(Customers customer)
        {
            try
            {
                _db.Customers.Add(customer);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateCustomer(Customers customer)
        {
            //try
            //{
            var local = _db.Set<Customers>()
                .Local
                .FirstOrDefault(f => f.CustomerID == customer.CustomerID);
            if (local != null)
            {
                _db.Entry(local).State = EntityState.Detached;
            }
            _db.Entry(customer).State = EntityState.Modified;
            return true;
            //}
            //catch
            //{
            //return false;
            //}
        }

        public bool DeleteCustomer(Customers customer)
        {
            try
            {
                _db.Entry(customer).State = EntityState.Deleted;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCustomer(int customerId)
        {
            try
            {
                var customer = GetCustomerById(customerId);
                DeleteCustomer(customer);
                return true;
            }
            catch
            {
                return false;
            }
        }

        IEnumerable<Customers> ICustomerRepository.GetCustomersByFilter(string parameter)
        {
            return _db.Customers.Where(c => c.FullName.Contains(parameter) || c.Email.Contains(parameter) || c.PhoneNumber.Contains(parameter)).ToList();
        }

        public List<ListCustomerViewModel> GetNameCustomer(string filter = "")
        {
            if (filter == "")
            {
                return _db.Customers.Select(C => new ListCustomerViewModel()
                {
                    CustomerID = C.CustomerID,
                    FullName = C.FullName,
                }).ToList();
            }

            return _db.Customers.Where(C => C.FullName.Contains(filter)).Select(C => new ListCustomerViewModel()
            {
                CustomerID = C.CustomerID,
                FullName = C.FullName,
            }).ToList();
        }

        public int GetCustomerIdByName(string name)
        {
            return _db.Customers.First(C => C.FullName == name).CustomerID;
        }

        public string GetCustomerNameById(int customerId)
        {
            return _db.Customers.Find(customerId).FullName;
        }
    }
}
