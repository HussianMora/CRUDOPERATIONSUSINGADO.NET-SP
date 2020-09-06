using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDWithADO_SP.Models
{
    public class State
    {
        public int StateId { get; set; }

        public int Name { get; set; }

        public int CountryId { get; set; }
    }
}