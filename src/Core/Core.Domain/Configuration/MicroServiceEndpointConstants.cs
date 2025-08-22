using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public enum Components
    {
        SystemService = 0,
        WareHouseService = 1,
        //AdminService = 2,
        //ModuleMgtService = 3,
        //ClassMgtService = 4,
        //LearningActivityService = 5,
        //LearningService = 6,
        //IndustryStandardService = 7,
        //AssessmentService = 8,
        //CollaborationService = 9,
        //ProjectService = 10,
        //ReportService = 11,
        //StorageService = 12,

        //TimerService = 20,
        IdentityServer = 21,
        //GuideHelp = 90,
        APIGateWay = 100,
    }

    public class MicroServiceEndpointConstants
    {
        public class ApplicationHostName
        {
            public const string SystemService = "System Service";
            public const string WareHouseService = "WareHouse Service";
            //public const string AdminService = "Administration Service";
            //public const string ModuleMgtService = "Module Management Service";
            //public const string ClassMgtService = "Class Management Service";
            //public const string LearningActivityService = "Learning Activity Service";
            //public const string LearningService = "Learning Service";
            //public const string IndustryStandardService = "Industy Stardard Service";
            //public const string AssessmentService = "Assessment Service";
            //public const string CollaborationService = "Collaboration Service";
            //public const string ProjectService = "Project Service";
            //public const string ReportService = "Report Service";
            //public const string StorageService = "Storage Service";

            //public const string TimerService = "Timer Service";
            public const string IdentityServer = "Identity Server";
            //public const string GuideHelp = "Guide Help Service";
            public const string APIGateWay = "API Gateway";
        }

        public class ComponentsDescription
        {
            public const string SystemService = "System Service description";
            public const string WareHouseService = "WareHouse Service description";
            //public const string AdminService = "Administration Service description";
            //public const string ModuleMgtService = "Module Management Service description";
            //public const string ClassMgtService = "Class Management Service description";
            //public const string LearningActivityService = "Learning Activity Service description";
            //public const string LearningService = "Learning Service description";
            //public const string IndustryStandardService = "Industy Stardard Service description";
            //public const string AssessmentService = "Assessment Service description";
            //public const string CollaborationService = "Collaboration Service description";
            //public const string ProjectService = "Project Service description";
            //public const string ReportService = "Report Service description";
            //public const string StorageService = "Storage Service description";

            //public const string TimerService = "Timer Service description";
            public const string IdentityServer = "Identity Service description";
            //public const string GuideHelp = "Guide Help Service description";
            public const string APIGateWay = "API Gateway description";
        }

        public class MicroServicePorts
        {
            public const int System = 19000;
            public const int WareHouse = 19001;
            //public const int Admin = 19002;
            //public const int ModuleMgt = 19003;
            //public const int ClassMgt = 19004;
            //public const int LearningActivity = 19005;
            //public const int Learning = 19006;
            //public const int IndustryStandard = 19007;
            //public const int Assessment = 19008;
            //public const int Collaboration = 19009;
            //public const int Project = 19010;
            //public const int Report = 19011;
            //public const int Storage = 19012;

            //public const int Timer = 19020;
            public const int Identity = 19021;
            //public const int GuideHelp = 19090;
            public const int APIGateway = 19100;
        }
    }
}
