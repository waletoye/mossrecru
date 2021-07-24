using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                    int a = 2;
                }
            }

            return result.OrderBy(x => x.FullName).ToList();
        }

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

        internal void ChangeCandidateStatus(object bindingContext, bool isAccepted)
        {
            var candidate = bindingContext as Models.CandidateModel;

            //candidate.Status = isAccepted ? Models.CandidateModel.AcceptanceStatus.Selected : Models.CandidateModel.AcceptanceStatus.Rejected;

            CandidateSource.Remove(candidate);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CandidateSource"));

            Models.DataStore.Cache.AddUser(candidate, isAccepted: isAccepted);
        }
    }
}
