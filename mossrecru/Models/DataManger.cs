using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mossrecru.Models
{
    public class DataManger
    {
        ISoapService soapService;

        public DataManger(ISoapService service)
        {
            soapService = service;
        }

        public Task<List<CandidateModel>> GetCandidates()
        {
            return soapService.GetCandidates();
        }

        public Task<List<TechnologyModel>> GetTechnologies()
        {
            return soapService.GetTechnologies();
        }
    }
}
