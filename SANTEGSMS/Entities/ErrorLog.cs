using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    public class ErrorLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorSource { get; set; }
        public string ErrorStackTrace { get; set; }
        public DateTime ErrorDate { get; set; }
    }
}
