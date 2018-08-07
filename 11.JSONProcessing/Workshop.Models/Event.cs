using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Workshop.Models
{
    public class Event
    {
        public Event()
        {
            this.ParticipatingEventTeams = new List<TeamEvent>();
        }

        [Key]
        [MinLength(0)]
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }


        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [ForeignKey("Creator")]
        [MinLength(0)]
        public int CreatorId { get; set; }
        public virtual User Creator { get; set; }

        public virtual ICollection<TeamEvent> ParticipatingEventTeams { get; set; }


    }
}
