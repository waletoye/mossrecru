using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using mossrecru.Models;

namespace mossrecru.Droid
{
    public class SoapService : ISoapService
    {
        app.ifs.aero.RecruitmentService service;

        public SoapService()
        {
            service = new app.ifs.aero.RecruitmentService();

            service.GetCandidatesCompleted += Service_GetCandidatesCompleted;
            service.GetTechnologiesCompleted += Service_GetTechnologiesCompleted;
        }

        public List<CandidateModel> CandidateItems { get; private set; }
        TaskCompletionSource<bool> getCandidateRequestComplete = null;

        private static CandidateModel AllCandidates(app.ifs.aero.Candidate item)
        {
            var candidate = new CandidateModel
            {
                CandidateId = Guid.Parse(item.CandidateId),
                Barcode = item.Barcode,
                CanSwim = item.CanSwim,
                Dad = item.Dad,
                Email = item.Email,
                //Experience = item.Experience,
                FavoriteMusicGenre = item.FavoriteMusicGenre,
                FirstName = item.FirstName,
                //Gender = item.Gender,
                FullName = item.FullName,
                LastName = item.LastName,
                Mom = item.Mom,
                ProfilePicture = new Uri(item.ProfilePicture)
            };

            var experience = new List<CandidateModel.History>();

            foreach (var history in item.Experience)
            {
                experience.Add(new CandidateModel.History
                {
                    TechnologyId = Guid.Parse(history.TechnologyId),
                    YearsOfExperience = history.YearsOfExperience
                });
            }

            candidate.Experience = experience.ToArray();

            return candidate;
        }
        private void Service_GetCandidatesCompleted(object sender, app.ifs.aero.GetCandidatesCompletedEventArgs e)
        {
            try
            {
                getCandidateRequestComplete = getCandidateRequestComplete ?? new TaskCompletionSource<bool>();

                CandidateItems = new List<CandidateModel>();

                foreach (var item in e.Result)
                {
                    CandidateItems.Add(AllCandidates(item));
                }
                getCandidateRequestComplete?.TrySetResult(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\t\tERROR {0}", ex.Message);
            }
        }

        async Task<List<CandidateModel>> ISoapService.GetCandidates()
        {
            getCandidateRequestComplete = new TaskCompletionSource<bool>();
            service.GetCandidatesAsync();
            await getCandidateRequestComplete.Task;
            return CandidateItems;
        }

        public List<TechnologyModel> TechnologyItems { get; private set; }
        TaskCompletionSource<bool> getTechnologyRequestComplete = null;

        private static TechnologyModel AllTechnologies(app.ifs.aero.Technology item)
        {
            return new TechnologyModel
            {
                Name = item.Name,
                TechnologyId = Guid.Parse(item.Guid)
            };
        }
        private void Service_GetTechnologiesCompleted(object sender, app.ifs.aero.GetTechnologiesCompletedEventArgs e)
        {
            try
            {
                getTechnologyRequestComplete = getTechnologyRequestComplete ?? new TaskCompletionSource<bool>();

                TechnologyItems = new List<TechnologyModel>();

                foreach (var item in e.Result)
                {
                    TechnologyItems.Add(AllTechnologies(item));
                }
                getTechnologyRequestComplete?.TrySetResult(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\t\tERROR {0}", ex.Message);
            }
        }


        async Task<List<TechnologyModel>> ISoapService.GetTechnologies()
        {
            getTechnologyRequestComplete = new TaskCompletionSource<bool>();
            service.GetTechnologiesAsync();
            await getTechnologyRequestComplete.Task;
            return TechnologyItems;
        }
    }
}
