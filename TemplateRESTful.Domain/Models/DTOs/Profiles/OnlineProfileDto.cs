using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using TemplateRESTful.Domain.Models.Features;

namespace TemplateRESTful.Domain.Models.DTOs
{
    public class OnlineProfileDto
    {
        [NotMapped]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [NotMapped]
        public string LastName { get; set; }
        [NotMapped]
        public string PhoneNumber { get; set; }
        [NotMapped]
        public string EmailAddress { get; set; }
        public DateTime DayOfBirth { get; set; }
        public string Occupation { get; set; }
        public string Website { get; set; }
        public string Location { get; set; }
        public string Language { get; set; }
    }
}
