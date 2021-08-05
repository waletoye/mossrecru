using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mossrecru.Models
{
    public interface ISoapService
    {
        Task<List<CandidateModel>> GetCandidates();

        Task<List<TechnologyModel>> GetTechnologies();
    }
}
