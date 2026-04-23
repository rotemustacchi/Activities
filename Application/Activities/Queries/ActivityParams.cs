using Application.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Activities.Queries
{
    public class ActivityParams : PagingtonParams<DateTime?>
    {
        public string? Filter { get; set; }
        public DateTime StartDate { get; set; }= DateTime.UtcNow;
    }
}
