using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Workshop.Models
{
    public class Invitation
    {
        public Invitation()
        {
            this.IsActive = true;
        }

        [Key]
        [MinLength(0)]
        public int Id { get; set; }

        [ForeignKey("InvitedUser")]
        [MinLength(0)]
        public int InvitedUserId { get; set; }
        public virtual User InvitedUser { get; set; }

        [ForeignKey("Team")]
        [MinLength(0)]
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

        public bool IsActive { get; set; }
    }
}
