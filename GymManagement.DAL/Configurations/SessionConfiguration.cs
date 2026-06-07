using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint("CapacityCheck", "Capacity Between 1 and 25");
            tb.HasCheckConstraint("EndDateCheck", "EndDate > StartDate");
        });
    }
}
