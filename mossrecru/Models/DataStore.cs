using System;
using System.Collections.Generic;

namespace mossrecru.Models
{
    public class DataStore
    {
        //singletons to hold temporary data
        internal static List<Models.CandidateModel> Candidates { get; set; }
        internal static List<Models.TechnologyModel> Technologies { get; set; }

        internal static Models.Context Context { get; set; }
        internal static Utilities.Cache Cache { get; set; }
    }
}
