using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHe.DomainModel
{
   [Table("Machine")]
   public class Machine
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int Id { get; set; }
      public string Name { get; set; }
      public DateTime Timestamp { get; set; }
      public virtual IList<MachineState> MachineStates { get; set; }
      public virtual IList<CycleCounter> CycleCounters { get; set; }
      public virtual IList<CycleInterruption> CycleInterruptions { get; set; }
      public virtual IList<ProcessVariable> ProcessVariables { get; set; }
   }

   [Table("MachineState")]
   public class MachineState
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int Id { get; set; }
      public string State { get; set; }
      public DateTime Timestamp { get; set; }

      public int MachineId { get; set; }
      public virtual Machine Machine { get; set; }
   }

   [Table("CycleCounter")]
   public class CycleCounter
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int Id { get; set; }
      public long Value { get; set; }
      public DateTime Timestamp { get; set; }

      public int MachineId { get; set; }
      public virtual Machine Machine { get; set; }
   }

   [Table("CycleInterruption")]
   public class CycleInterruption
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int Id { get; set; }
      public string Reason { get; set; }
      public DateTime Timestamp { get; set; }

      public int MachineId { get; set; }
      public virtual Machine Machine { get; set; }
   }

   [Table("ProcessVariable")]
   public class ProcessVariable
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int Id { get; set; }
      public string VariableName { get; set; }
      public float Value { get; set; }
      public DateTime Timestamp { get; set; }
      public int MachineId { get; set; }
      public virtual Machine Machine { get; set; }
   }
}
