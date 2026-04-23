using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Core
{
    public class PagedList<T,TCurser>
    {
        public List<T> Items { get; set; } = [];
        public TCurser? NextCursor { get; set; }
    }
}
