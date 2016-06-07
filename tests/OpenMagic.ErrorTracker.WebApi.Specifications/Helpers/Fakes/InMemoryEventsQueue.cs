using System.Collections.Generic;
using System.Threading.Tasks;
using OpenMagic.ErrorTracker.Core.Events;
using OpenMagic.ErrorTracker.Core.Queues;

namespace OpenMagic.ErrorTracker.WebApi.Specifications.Helpers.Fakes
{
    public class InMemoryEventsQueue : IEventsQueue
    {
        // ReSharper disable once CollectionNeverQueried.Local
        private readonly List<IEvent> _events;

        public InMemoryEventsQueue()
        {
            _events = new List<IEvent>();
        }

        public IReadOnlyCollection<IEvent> Events => _events.AsReadOnly();

        public Task AddAsync(IEvent @event)
        {
            return Task.Run(() => _events.Add(@event));
        }
    }
}