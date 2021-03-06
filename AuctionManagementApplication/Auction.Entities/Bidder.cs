﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Entities
{
    public class Bidder
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public double BidPrice { get; set; }
        public DateTime DateTime { get; set; }
        public  virtual Product Product { get; set; }
        public  virtual  User User { get; set; }

    }
}
