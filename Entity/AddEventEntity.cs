using System;

namespace Entity
{
    public class AddEventEntity
    {
        public long EventID { get; set; }
        public string EventName { get; set; }
        public string EventStartDate { get; set; }
        public string EventEndDate { get; set; }
        public string EventDiscription { get; set; }
        public string EventAddress { get; set; }
        public long EventCreatedBy { get; set; }
        public bool EventIsActive { get; set; }
        public string EventImage { get; set; }
        public string Flag { get; set; }

    }
}
