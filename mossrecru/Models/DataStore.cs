using System;
using System.Collections.Generic;

namespace mossrecru.Models
{
    public class DataStore
    {
        //singletons to hold temporary data

        internal static List<Models.CandidateModel> Candidates { get; set; }
        internal static List<Models.TechnologyModel> Technologies { get; set; }

        //network connection
        internal static Models.Context Context { get; set; }

        //db
        internal static Utilities.Cache Cache { get; set; }

        public static DataManger DataManger { get; set; }
    }
}
