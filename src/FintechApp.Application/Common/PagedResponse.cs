﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FintechApp.Application.Common
{
    public class PagedResponse<T> : ApiResponse<List<T>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
    }
}
