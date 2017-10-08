using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHe.DomainModel.DataMapping;

namespace LHe.DomainModel
{
   public class PersistenceManager
   {
      public void SaveMachine(string machineName, DateTime timestamp)
      {
         using (var db = new OPCDbContext())
         {
            var machine = GetOrCreateMachine(db, machineName, timestamp);
            db.SaveChanges();
         }
      }

      private Machine GetOrCreateMachine(OPCDbContext db, string machineName, DateTime timestamp)
      {
         var machine = db.Machines.FirstOrDefault(m => m.Name == machineName);

         if (machine == null)
         {
            machine = new Machine
            {
               Name = machineName,
               Timestamp = timestamp
            };
            db.Machines.Add(machine);
         }

         return machine;
      }

      public void SaveMachineProcessVariable(string machineName, string variableName, float value, DateTime timestamp)
      {
         using (var db = new OPCDbContext())
         {
            var machine = GetOrCreateMachine(db, machineName, timestamp);

            db.ProcessVariables.Add(new ProcessVariable
            {
               Machine = machine,
               VariableName = variableName,
               Value = value,
               Timestamp = timestamp
            });

            db.SaveChanges();
         }
      }
   }
}
