using GameStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.DAL.Configuration
{
    public class ProductConfiuration: IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                 .HasIndex(o => new { o.Name, o.Platform, o.DateCreated, o.TotalRating });
            
        }
    }
}
