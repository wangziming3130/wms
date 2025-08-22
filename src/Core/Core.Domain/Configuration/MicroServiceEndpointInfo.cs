using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class MicroserviceEndpointInfo
    {
        public string System { get; set; }
        public string WareHouse { get; set; }
        public string Admin { get; set; }
        public string ModuleMgt { get; set; }
        public string ClassMgt { get; set; }
        public string LearningActivity { get; set; }
        public string Learning { get; set; }
        public string IndustryStandard { get; set; }
        public string Assessment { get; set; }
        public string Collaboration { get; set; }
        public string Project { get; set; }
        public string Report { get; set; }
        public string Storage { get; set; }

        public string Timer { get; set; }
        public string Identity { get; set; }
        public string GuideHelp { get; set; }
        public string Portal { get; set; }

        public string GetEndpoint(Components component)
        {
            var endpointUrl = "";
            switch (component)
            {
                case Components.SystemService: endpointUrl = System; break;
                case Components.WareHouseService: endpointUrl = WareHouse; break;
                //case Components.AdminService: endpointUrl = Admin; break;
                //case Components.ModuleMgtService: endpointUrl = ModuleMgt; break;
                //case Components.ClassMgtService: endpointUrl = ClassMgt; break;
                //case Components.LearningActivityService: endpointUrl = LearningActivity; break;
                //case Components.LearningService: endpointUrl = Learning; break;
                //case Components.IndustryStandardService: endpointUrl = IndustryStandard; break;
                //case Components.AssessmentService: endpointUrl = Assessment; break;
                //case Components.CollaborationService: endpointUrl = Collaboration; break;
                //case Components.ProjectService: endpointUrl = Project; break;
                //case Components.ReportService: endpointUrl = Report; break;
                //case Components.StorageService: endpointUrl = Storage; break;

                //case Components.TimerService: endpointUrl = Timer; break;
                case Components.IdentityServer: endpointUrl = Identity; break;
                //case Components.GuideHelp: endpointUrl = GuideHelp; break;
            }
            return endpointUrl;
        }

        public static string GetRouteUrl(Components component)
        {
            var routeUrl = "";
            switch (component)
            {
                case Components.SystemService: routeUrl = "system"; break;
                case Components.WareHouseService: routeUrl = "warehouse"; break;
                //case Components.AdminService: routeUrl = "admin"; break;
                //case Components.ModuleMgtService: routeUrl = "modulemgt"; break;
                //case Components.ClassMgtService: routeUrl = "classmgt"; break;
                //case Components.LearningActivityService: routeUrl = "learningactivity"; break;
                //case Components.LearningService: routeUrl = "learning"; break;
                //case Components.IndustryStandardService: routeUrl = "industrystandard"; break;
                //case Components.AssessmentService: routeUrl = "assessment"; break;
                //case Components.CollaborationService: routeUrl = "collaboration"; break;
                //case Components.ProjectService: routeUrl = "project"; break;
                //case Components.ReportService: routeUrl = "report"; break;
                //case Components.StorageService: routeUrl = "storage"; break;

                //case Components.TimerService: routeUrl = "timer"; break;
                case Components.IdentityServer: routeUrl = "identity"; break;
                //case Components.GuideHelp: routeUrl = "guidehelp"; break;
            }
            return routeUrl;
        }
    }
}
