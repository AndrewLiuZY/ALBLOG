﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ALBLOG.Domain.Model
{
    public class LogPage
    {
        public IEnumerable<Log> Logs { get; set; }

        public bool HaveLast { get; set; }

        public bool HaveNext { get; set; }

        public int Index { get; set; }

        public int PageCount { get; set; }

        public int Size { get; set; }

    }
}
