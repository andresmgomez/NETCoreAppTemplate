using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace UserManagementDemo.Domain.Models.Entities
{
    public class OnlineProfile
    {
        [Key]
        public string Id { get; set; }
        [JsonIgnore]
        public byte[] Picture { get; set; }

        [NotMapped]
        public string FirstName { get; set; }

        [StringLength(25, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [NotMapped]
        public string LastName { get; set; }
        [NotMapped]
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DayOfBirth { get; set; }

        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Occupation { get; set; }

        [StringLength(25, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        [Display(Name = "Personal Website")]
        public string Website { get; set; }

        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        [Display(Name = "Current Location")]
        public string Location { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [Display(Name = "Preferred Language")]
        public string Language { get; set; }
    }
}
