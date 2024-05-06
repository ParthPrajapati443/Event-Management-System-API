using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class GetEventEntity
    {
        public int Id { get; set; }
        public long EventID { get; set; }
        public string EventCreatedBy { get; set; }
        public string Flag { get; set; }
    }
}
