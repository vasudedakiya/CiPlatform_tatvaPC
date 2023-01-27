using CIPlatform.DataModels;

namespace CIPlatform.Models
{
    public class AdminModel
    {
       
        public List<User>? users { get; set; }

        public List<CmsPage>? cmsPages { get; set; }

        public List<Mission>? missions { get; set; }

        public List<Theme>? themes { get; set; }

        public List<Skill>? skills { get; set; }

        public List<MissionApplication>? missionApplications { get; set; }

        public List<Story>? stories { get; set; }

        public List<Banner>? banners { get; set; }

    }
}
