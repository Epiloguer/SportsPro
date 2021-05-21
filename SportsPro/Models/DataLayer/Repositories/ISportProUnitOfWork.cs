using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SportsPro.Models
{
    public interface ISportsProUnitOfWork
    {
        Repository<Product> Products { get; }
        Repository<Customer> Customers { get; }
        Repository<Technician> Technicians { get; }
        Repository<Incident> Incidents { get; }

        //void DeleteCurrentBookAuthors(Book book);
        //void AddNewBookAuthors(Book book, int[] authorids);
        void Save();
    }
}
