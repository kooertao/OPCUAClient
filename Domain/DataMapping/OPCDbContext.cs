using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LHe.DomainModel.DataMapping
{
   public class OPCDbContext : DbContext
   {
      public DbSet<Machine> Machines { get; set; }
      public DbSet<MachineState> MachineStates { get; set; }
      public DbSet<CycleInterruption> CycleInterruptions { get; set; }
      public DbSet<ProcessVariable> ProcessVariables { get; set; }
      public OPCDbContext()
          : base("name=OPCDbContext")
      {
         Database.SetInitializer(new DropCreateDatabaseIfModelChanges<OPCDbContext>());
      }

      protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Machine>()
             .Property(u => u.Id);
      }
   }
}
