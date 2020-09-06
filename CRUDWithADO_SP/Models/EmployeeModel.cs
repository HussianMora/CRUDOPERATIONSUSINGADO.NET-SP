using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDWithADO_SP.Models
{
    public class EmployeeModel
    {
        public int EmployeeID { get; set; }

        public string FullName { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public DateTime DOB { get; set; }
    }
}