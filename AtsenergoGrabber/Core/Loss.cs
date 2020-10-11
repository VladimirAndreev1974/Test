using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtsenergoGrabber.Core
{
    public class Loss
    {
        public int Id { get; set; }
        public string Region { get; set; }
        public decimal Amount { get; set; }
        public string ReportDate { get; set; }
    }
}