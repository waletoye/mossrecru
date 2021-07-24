using System;
using System.ComponentModel;

namespace mossrecru.ViewModels
{
    public class BaseVM : INotifyPropertyChanged
    {
        public virtual event PropertyChangedEventHandler PropertyChanged;

        public BaseVM()
        {
            Models.DataStore.Candidates = Models.DataStore.Candidates ?? new System.Collections.Generic.List<Models.CandidateModel>();
            Models.DataStore.Technologies = Models.DataStore.Technologies ?? new System.Collections.Generic.List<Models.TechnologyModel>();
            Models.DataStore.Cache = Models.DataStore.Cache ?? new Utilities.Cache();
        }

        internal Models.Context Context => Models.DataStore.Context ?? new Models.Context();

        bool isRunning;
        public bool IsRunning
        {
            get => isRunning;
            set
            {
                isRunning = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
            }
        }
    }
}
