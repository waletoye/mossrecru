using System;
using System.Collections.Generic;
using System.Linq;
using MonkeyCache.FileStore;

namespace mossrecru.Utilities
{
    public class Cache
    {
        public Cache()
        {
            Barrel.ApplicationId = "mossrecru";

            // var count = Count;
        }

        public bool UserExists(Guid candidateId)
        {
            return Barrel.Current.Exists(key: candidateId.ToString());
        }

        public bool UserAlreadyAccepted(Guid candidateId)
        {
            if (!UserExists(candidateId))
                return false;

            var user = Barrel.Current.Get<Models.CandidateModel>(key: candidateId.ToString());
            return user.Status == Models.CandidateModel.AcceptanceStatus.Accepted;
        }

        public bool UserAlreadyRejected(Guid candidateId)
        {
            if (!UserExists(candidateId))
                return false;

            var user = Barrel.Current.Get<Models.CandidateModel>(key: candidateId.ToString());
            return user.Status == Models.CandidateModel.AcceptanceStatus.Rejected;
        }

        public void AddUser(Models.CandidateModel candidate, bool isAccepted)
        {
            //never expiress?
            Barrel.Current.Add(key: candidate.CandidateId.ToString(), data: candidate, expireIn: TimeSpan.FromDays(Settings.AppSettings.DBExpiry));
        }

        public List<Models.CandidateModel> GetAcceptedCandidates()
        {
            var keys = Barrel.Current.GetKeys(MonkeyCache.CacheState.Active);

            var candidates = new List<Models.CandidateModel>();

            foreach (var key in keys)
            {
                var candidate = GetUser(Guid.Parse(key));

                if (candidate != null && candidate.Status == Models.CandidateModel.AcceptanceStatus.Accepted)
                    candidates.Add(candidate);
            }

            return candidates;
        }

        private Models.CandidateModel GetUser(Guid candidateId)
        {
            if (!UserExists(candidateId))
                return null;

            return Barrel.Current.Get<Models.CandidateModel>(key: candidateId.ToString());
        }

        private int Count => Barrel.Current.GetKeys(MonkeyCache.CacheState.Active).Count();
    }
}
