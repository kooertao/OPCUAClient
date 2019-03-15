﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHe.DomainModel.DataMapping;

namespace LHe.DomainModel
{
   public class PersistenceManager
   {
      public void SaveMachineState(string machineName, string state, DateTime timestamp)
      {
         using (var db = new OPCDbContext())
         {
            var machine = GetOrCreateMachine(db, machineName, timestamp);

            db.MachineStates.Add(new MachineState
            {
               Machine = machine,
               State = state,
               Timestamp = timestamp
            });

            db.SaveChanges();
         }
      }

      public void SaveMachineCycleInterruption(string machineName, string downReason, DateTime timestamp)
      {
         using (var db = new OPCDbContext())
         {
            var machine = GetOrCreateMachine(db, machineName, timestamp);

            db.CycleInterruptions.Add(new CycleInterruption
            {
               Machine = machine,
               Reason = downReason,
               Timestamp = timestamp
            });

            db.SaveChanges();
         }
      }
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

      public void SaveMachineCycleCounter(string machineName, long cycleCounter, DateTime timestamp)
      {
         using (var db = new OPCDbContext())
         {
            var machine = GetOrCreateMachine(db, machineName, timestamp);
            db.CycleCounters.Add(new CycleCounter
            {
               Machine = machine,
               Value = cycleCounter,
               Timestamp = timestamp
            });
            db.SaveChanges();
         }
      }

   }
}
