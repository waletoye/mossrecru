using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace mossrecru.ViewModels
{
    public class CandidateVM : BaseVM, INotifyPropertyChanged
    {
        public override event PropertyChangedEventHandler PropertyChanged;

        public CandidateVM()
        {

        }

        public ObservableCollection<Models.CandidateModel> CandidateSource { get; set; }
        private List<Models.CandidateModel> AllCandidates { get; set; }
        private List<Models.TechnologyModel> AllTechnologies { get; set; }

        /// <summary>
        /// Load all candidates, check first if technologies data is available
        /// </summary>
        /// <returns></returns>
        internal async Task<(bool isSuccessful, string message)> LoadCandidates()
        {
            IsRunning = true;

            var hasLoadedTechnologies = await LoadTechnologies();

            if (!hasLoadedTechnologies.isSuccessful)
            {
                IsRunning = false;
                return (false, "could not retrieve technoliges.");
            }


            var result = await Context.GetRequest<List<Models.CandidateModel>>(endPoint: "/candidates");

            if (result == null)
                return (false, null);

            if (result.Any())
            {
                AllCandidates = FilterPreviouslySelectedUser(result);

                //order by fullname
                Models.DataStore.Candidates = AllCandidates;

                CandidateSource = new ObservableCollection<Models.CandidateModel>(AllCandidates);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CandidateSource"));

                IsRunning = false;
                return (true, "success");
            }

            IsRunning = false;
            return (false, "Service returned empty data for candidates");
        }

        /// <summary>
        /// Filter prevuously accepted or rejected
        /// </summary>
        /// <param name="candidates"></param>
        /// <returns></returns>
        private List<Models.CandidateModel> FilterPreviouslySelectedUser(List<Models.CandidateModel> candidates)
        {
            var result = new List<Models.CandidateModel>();

            //filter previously accepted/rejected candidates
            foreach (var candidate in candidates)
            {
                if (!Models.DataStore.Cache.UserExists(candidate.CandidateId))
                {
                    result.Add(candidate);
                }
                else
                {
                    Debug.Write(candidate.CandidateId);
                }
            }

            return result.OrderBy(x => x.FullName).ToList();
        }

        /// <summary>
        /// Load all technologies for use with candidates model
        /// </summary>
        /// <returns></returns>
        private async Task<(bool isSuccessful, string message)> LoadTechnologies()
        {
            //if technologies, have been retrieved, continue to retrieve candidates
            if (AllTechnologies != null && AllTechnologies.Any())
                return (true, "success");

            var result = await Context.GetRequest<List<Models.TechnologyModel>>(endPoint: "/technologies");

            if (result == null)
                return (false, null);

            if (result.Any())
            {
                AllTechnologies = result.OrderBy(X => X.Name).ToList();
                Models.DataStore.Technologies = AllTechnologies;
                return (true, "success");
            }

            return (false, "Service returned empty data.");
        }


        /// <summary>
        /// Accept or reject a candidate, cache the candidateID
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <param name="isAccepted"></param>
        internal void ChangeCandidateStatus(object bindingContext, bool isAccepted)
        {
            var candidate = bindingContext as Models.CandidateModel;

            candidate.Status = isAccepted ? Models.CandidateModel.AcceptanceStatus.Accepted : Models.CandidateModel.AcceptanceStatus.Rejected;
            CandidateSource.Remove(candidate);

            //update AllCandidates
            var item = AllCandidates.Where(x => x.CandidateId == candidate.CandidateId).First();
            item.Status = candidate.Status;


            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CandidateSource"));


            //cache the candidate for persistent filtering
            Models.DataStore.Cache.AddUser(candidate, isAccepted: isAccepted);
        }


        /// <summary>
        /// Filter a candidate by accepted /all
        /// </summary>
        /// <param name="title">accepted or all</param>
        internal void FilterByStatus(string title)
        {
            title = title.ToLower();

            if (title == "show all")
            {
                var source = AllCandidates.Where(x => x.Status == default);//none
                CandidateSource = new ObservableCollection<Models.CandidateModel>(source);
            }
            else if (title == "accepted only")
            {
                var source = Models.DataStore.Cache.GetAcceptedCandidates();
                CandidateSource = new ObservableCollection<Models.CandidateModel>(source);
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CandidateSource"));
        }


        /// <summary>
        /// Filter a candidate by experience and number of years of experience
        /// </summary>
        /// <param name="tech">tech stack</param>
        /// <param name="exp">years of experience</param>
        internal void FilterByTechAndExperience(string tech, string exp)
        {
            IEnumerable<Models.CandidateModel> source = AllCandidates;

            int.TryParse(exp, out int yrOfExp);

            //check tech
            if (string.IsNullOrWhiteSpace(tech) || tech.ToLower() == "any")
            {
                source = source.Where(x => x.Experience != null && x.Experience.Any() && x.Status == default);
            }
            else
            {
                source = source.Where(x => x.Experience != null && x.Experience.Where(y => y.Technology == tech).Any() && x.Status == default);
            }

            //check experience, skip if less than one
            if (yrOfExp > 0)
            {
                source = source.Where(x => x.Experience != null && x.Experience.Where(y => y.Technology == tech && y.YearsOfExperience == yrOfExp).Any() && x.Status == default);
            }

            CandidateSource = new ObservableCollection<Models.CandidateModel>(source);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CandidateSource"));
        }

    }
}
