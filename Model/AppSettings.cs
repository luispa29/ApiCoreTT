using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AppSettings
    {
        public string? DefaultConnection { get; set; }

        public string? Secret { get; set; }

        public bool Docker { get; set; }
    }
}
