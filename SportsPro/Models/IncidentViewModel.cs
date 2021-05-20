using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.Models
{
    public class IncidentViewModel
    {
        public Incident Incident { get; set; }
        public Customer Customer { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Product> Products { get; set; }
        public List<Technician> Technicians { get; set; }
        public string DesiredAction { get; set; }
    }
}
