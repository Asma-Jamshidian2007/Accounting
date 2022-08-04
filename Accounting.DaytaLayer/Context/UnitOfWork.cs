using System;
using Accounting.DataLayer.Repository;
using Accounting.DataLayer.Services;

namespace Accounting.DataLayer.Context
{
    public class UnitOfWork : IDisposable
    {
        private readonly Accounting_DBEntities _db = new Accounting_DBEntities();

        private ICustomerRepository _customerRepository;

        public ICustomerRepository CustomerRepository =>
            _customerRepository ?? (_customerRepository = new CustomerRepository(_db));


        private GenericRepository<Accounting> _accountinGenericRepository;

        public GenericRepository<Accounting> AccountinGenericRepository =>
            _accountinGenericRepository ?? (_accountinGenericRepository = new GenericRepository<Accounting>(_db));

        public void save()
        {
            _db.SaveChanges();
        }

        public void Dispose() => _db.Dispose();


    }
}
