﻿using System;
using System.ComponentModel.DataAnnotations;

namespace GameBot.Domain
{
    public class Order
    {
        public Guid OrderId { get; set; }

        [Required]
        public int Coins { get; set; }

        public string User { get; set; }
    }
}
