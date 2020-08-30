using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RemoteAssistantStorage.Models
{
    [Table("LOG_SYSTEM")]
    public class LogSystem
    {
        ///<summary>
        /// Identity колона
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Message { get; set; } 
        public DateTime MessageDate { get; set; }
    }
}
