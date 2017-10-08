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
      //public virtual IList<MachineState> MachineStates { get; set; }
      //public virtual IList<CycleCounter> CycleCounters { get; set; }
      //public virtual IList<CycleInterruption> CycleInterruptions { get; set; }
      public virtual IList<ProcessVariable> ProcessVariables { get; set; }
   }
}
