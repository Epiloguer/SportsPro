using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.Models.DataLayer
{
    internal class RegistrationConfig : IEntityTypeConfiguration<CustProd>
    {
      
            public void Configure(EntityTypeBuilder<CustProd> entity)
            {
            entity
           .HasKey(cp => new { cp.CustomerID, cp.ProductID });

            //one-to-many relationship between Customer and CustProd
            entity
                 .HasOne(cp => cp.Customer)
                .WithMany(c => c.CustProds)
                .HasForeignKey(cp => cp.CustomerID);

            //one-to-many relationship between Product and CustProd
            entity
                   .HasOne(cp => cp.Product)
                .WithMany(p => p.CustProds)
                .HasForeignKey(cp => cp.ProductID);


        }
        

    }

}

