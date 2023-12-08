using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

using TemplateRESTful.Infrastructure.Utilities;

namespace TemplateRESTful.Persistence.Data.Actions
{
    public class TemporaryActions : ITempActionsContainer
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;

        private ITempDataDictionary TempActions => _tempDataDictionaryFactory.GetTempData(_httpContextAccessor.HttpContext);

        public TemporaryActions(
           IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
        }

        public T Get<T>(string data) where T : class
        {
            if (TempActions.ContainsKey(data) && TempActions[data] is string json)
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            return null;
        }

        public T Peek<T>(string data) where T : class
        {
            if (TempActions.ContainsKey(data) && TempActions.Peek(data) is string json)
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            return null;
        }

        public void Add(string data, object entity)
        {
            TempActions[data] = entity.ToJson();
        }

        public bool Remove(string data)
        {
            return TempActions.ContainsKey(data) && TempActions.Remove(data);
        }
    }
}
