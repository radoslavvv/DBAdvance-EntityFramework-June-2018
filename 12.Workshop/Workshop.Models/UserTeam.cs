﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Workshop.Models
{
    public class UserTeam
    {
        [ForeignKey("User")]
        [MinLength(0)]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Team")]
        [MinLength(0)]
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
    }
}
