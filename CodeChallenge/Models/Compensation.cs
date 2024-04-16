﻿using System;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        public Employee Employee { get; set; }
        public double Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
