using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LHe.DomainModel
{
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
