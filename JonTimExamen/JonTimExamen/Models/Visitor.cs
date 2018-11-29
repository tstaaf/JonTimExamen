using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JonTimExamen.Models
{
    public class Visitor
    {
        public int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Company { get; set; }
        public virtual DateTime CheckInTime { get; set; }
        public virtual DateTime CheckOutTime { get; set; }
        public virtual string RandomNumber { get; set; }
    }
}
