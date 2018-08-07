using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Workshop.Models
{
    public class TeamEvent
    { 
        [ForeignKey("Team")]
        [MinLength(0)]
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

        [ForeignKey("Event")]
        [MinLength(0)]
        public int EventId { get; set; }
        public virtual Event Event { get; set; }
    }
}
