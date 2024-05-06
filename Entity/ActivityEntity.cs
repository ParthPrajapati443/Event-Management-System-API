using System;

namespace Entity
{
    public class ActivityEntity
    {
        public long ActivityID { get; set; }
        public string ActivityName { get; set; }
        public string ActivityStartDate { get; set; }
        public string ActivityEndDate { get; set; }
        public string ActivityDiscription { get; set; }
        public long ActivityPrice { get; set; }
        public long EventID { get; set; }
        public bool IsActive { get; set; }
        public string Flag { get; set; }

    }
}
