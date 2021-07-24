using System;
using MonkeyCache.FileStore;

namespace mossrecru.Utilities
{
    public class Cache
    {
        public Cache()
        {
            Barrel.ApplicationId = "mossrecru";
        }

        public bool UserExists(Guid candidateId)
        {
            return Barrel.Current.Exists(key: candidateId.ToString());
        }

        public bool UserAlreadyAccepted(Guid candidateId)
        {
            if (!UserExists(candidateId))
                return false;

            var user = Barrel.Current.Get<Models.CacheDTO>(key: candidateId.ToString());
            return user.IsAccepted;
        }

        public bool UserAlreadyRejected(Guid candidateId)
        {
            if (!UserExists(candidateId))
                return false;

            var user = Barrel.Current.Get<Models.CacheDTO>(key: candidateId.ToString());
            return !user.IsAccepted;
        }

        public void AddUser(Models.CandidateModel candidate, bool isAccepted)
        {
            var user = new Models.CacheDTO
            {
                FullName = candidate.FullName,
                IsAccepted = isAccepted
            };

            //never expiress?
            Barrel.Current.Add(key: candidate.CandidateId.ToString(), data: user, expireIn: TimeSpan.FromDays(365));
        }
    }
}
