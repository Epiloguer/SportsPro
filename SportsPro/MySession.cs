using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SportsPro.Models;

namespace SportsPro
{
    public class MySession
    {
        private const string TechnicianKey = "technician";

        private ISession session { get; set; }
        public MySession(ISession sess)
        {
            session = sess;
        }

        public Technician GetTechnician() =>
            session.GetObject<Technician>(TechnicianKey) ?? new Technician();

        public void SetTechnician(Technician technician) =>
            session.SetObject(TechnicianKey, technician);

    }
}
