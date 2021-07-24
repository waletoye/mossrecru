using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace mossrecru.Models
{
    public class CandidateModel
    {
        //internal AcceptanceStatus Status { get; set; }

        public Guid CandidateId { get; set; }

        public string FullName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Gender { get; set; }

        public string Sex => Gender == 0 ? "Male" : "Female";

        public Uri ProfilePicture { get; set; }

        public string Email { get; set; }

        public string FavoriteMusicGenre { get; set; }

        public string Dad { get; set; }

        public string Mom { get; set; }

        public bool CanSwim { get; set; }

        public string Barcode { get; set; }

        string developerProfile;
        public string DeveloperProfile
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(developerProfile))
                    return developerProfile;

                var builder = new StringBuilder();

                foreach (History history in Experience)
                {
                    builder.Append(history.Description + "; ");
                }

                developerProfile = builder.ToString();
                return developerProfile;
            }
        }

        [JsonProperty("experience")]
        public History[] Experience { get; set; }

        public partial class History
        {
            [JsonProperty("technologyId")]
            public Guid TechnologyId { private get; set; }

            [JsonProperty("yearsOfExperience")]
            public int YearsOfExperience { private get; set; }

            public string Technology => Models.DataStore.Technologies.Where(x => x.TechnologyId == TechnologyId).Select(x => x.Name).FirstOrDefault();

            public string Description => $"{Technology} - {YearsOfExperience}yrs";
        }

        //public enum AcceptanceStatus
        //{
        //    Selected,
        //    Rejected,
        //    None
        //}
    }
}
