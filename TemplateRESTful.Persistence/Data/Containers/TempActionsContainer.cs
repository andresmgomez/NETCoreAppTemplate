using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateRESTful.Persistence.Data.Containers;

namespace TemplateRESTful.Persistence.Data.Actions
{
    public class TempActionsContainer<T> : IUserActions<T> where T : class
    {
        private readonly ITempActionsContainer _temporaryAction;
        private const string Key = "";

        public TempActionsContainer(ITempActionsContainer temporaryActions)
        {
            _temporaryAction = temporaryActions;
        }

        public IList<T> Get()
        {
            return _temporaryAction.Peek<List<T>>(Key) ?? new List<T>();
        }

        public IList<T> Read()
        {
            var generalActions = Get();
            Clear();
            return generalActions;
        }

         public void Add(T action)
        {
            var generalActions = _temporaryAction.Get<IEnumerable<T>>(Key) ?? new List<T>();
            var generalActionsList = generalActions.ToList();

            generalActionsList.Add(action);
            _temporaryAction.Add(Key, generalActionsList);
        }

        public void Clear()
        {
            _temporaryAction.Remove(Key);
        }

    }
}
