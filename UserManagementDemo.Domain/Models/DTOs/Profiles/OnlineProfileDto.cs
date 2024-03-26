using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

using UserManagementDemo.Domain.Models.Features;

namespace UserManagementDemo.Domain.Models.DTOs
{
    public class OnlineProfileDto
    {
        [NotMapped]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [NotMapped]
        public string LastName { get; set; }
        
        [NotMapped]
        [JsonIgnore]
        public string PhoneNumber { get; set; }
        
        [NotMapped]
        [JsonIgnore]
        public string EmailAddress { get; set; }
        
        public DateTime DayOfBirth { get; set; } = DateTime.UtcNow;
        public string Occupation { get; set; }
        public string Website { get; set; }

        [JsonIgnore]
        public string Location { get; set; }
        
        [JsonIgnore]
        public string Language { get; set; }
    }
}
