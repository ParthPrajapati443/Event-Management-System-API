using System.ComponentModel.DataAnnotations;

namespace Entity
{
    public class UserRegistrationEntity
    {
        public string Flag { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set;}
        public string UserEmail { get; set;}
        public string UserPassword { get; set;}
        [StringLength(15, MinimumLength = 2)]
        [RegularExpression("^[0-9]*$")]
        public string UserMobile { get; set;}

    }
}
