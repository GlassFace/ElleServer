using System.Collections.Concurrent;
using ElleWorld.Core.Miscellaneous;
using ElleWorld.Core.Network;

namespace ElleWorld.Core.Managers
{
    class SessionManager : Singleton<SessionManager>
    {
        public long LastSessionId { get; set; }
        public ConcurrentDictionary<long, WorldSession> Sessions;

        SessionManager()
        {
            Sessions = new ConcurrentDictionary<long, WorldSession>();
        }

        public bool Add(long id, WorldSession session)
        {
            return Sessions.TryAdd(id, session);
        }

        public bool Remove(long id)
        {
            WorldSession session;

            return Sessions.TryRemove(id, out session);
        }
    }
}
