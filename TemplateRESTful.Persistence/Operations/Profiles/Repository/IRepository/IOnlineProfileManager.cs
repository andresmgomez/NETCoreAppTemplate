using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Domain.Models.Entities.Profiles;
using TemplateRESTful.Persistence.Operations.Generic;

namespace TemplateRESTful.Persistence.Operations.Profiles
{
    public interface IOnlineProfileManager : IGenericRepositoryAsync<OnlineProfile> 
    {
        OnlineProfile SetOnlineProfile(ApplicationUser onlineUser);
    }
}
