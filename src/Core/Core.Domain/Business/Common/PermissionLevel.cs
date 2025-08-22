using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public enum PermissionLevel
    {


        Invalid = -1,

        Any = -2,

        [Description("[LP]: View Module Page")]
        ViewModulePage = 0,

        [Description("[SI]: View Student Profile")]//SI-FA-Quiz
        ViewStudentProfile = 1,

        [Description("[SI]: View Student Section")]
        ViewStudentSection = 2,

        [Description("[LP]: View Learning Path")]
        ViewLearningPath = 3,

        [Description("[SI]: View Lesson Publishing")]
        ViewLessonPublishing = 4,

        [Description("[SI]: View Grade Publishing")]
        ViewGradePublishing = 5,

        [Description("[SI]: View Grade Statics")]
        ViewGradeStatics = 6,

        [Description("[SI]: View Attendance")]
        ViewAttendance = 7,



        [Description("[SI]: View LICN")]
        ViewLICN = 9,

        [Description("[SI]: View Class Schedule")]
        ViewClassSchedule = 10,

        //[Obsolete("No longer used")]
        [Description("[SI]: View Lecturer Allocation")]
        ViewLecturerAllocation = 11,



        [Description("[CC]: View Announcement")]
        ViewAnnouncement = 14,

        [Description("[SLA]: View Student Submission")]
        ViewStudentSubmission = 15,

        [Description("[SLA]: View Staff Submission")]
        ViewStaffSubmission = 16,

        [Description("[SS]: View Survey Report")]
        ViewSurveyReport = 17,

        [Description("[SS]: View Survey Response")]
        ViewSurveyResponse = 18,



        [Description("[FA]: View Module Cutoff")]
        ViewModuleCutoff = 23,

        [Description("[FA]: View Computing Page")]
        ViewComputingPage = 24,

        [Description("[FA]: View Assessment")]
        ViewAssessment = 25,

        [Description("[FA]: View Final Grade")]
        ViewFinalGrade = 26,

        [Description("[CC]: View Poll")]
        ViewPoll = 27,

        //[Obsolete("Please Use View Poll Details instead.")]
        [Description("[CC]: View Poll Result")]
        ViewPollResult = 28,

        [Description("[CC]: View Discussion Forum")]
        ViewDiscussionForum = 29,

        [Description("[CC]: View Wiki")]
        ViewWiki = 30,

        [Description("[CC]: View Blog")]
        ViewBlog = 31,

        [Description("[CC]: View Skype Chat")]
        ViewSkypeChat = 32,

        [Description("[CC]: View Media Gallery")]
        ViewMediaGallery = 33,

        [Description("[CC]: View Calendar")]
        ViewCalendar = 34,

        [Description("[CC]: View ShareDrive")]
        ViewShareDrive = 35,

        [Description("[CC]: View Learning Space")]
        ViewLearningSpace = 36,

        [Description("[CC]: View Yammer")]
        ViewYammer = 37,

        [Description("[FA]: Apply For Change Grade")]
        ApplyForChangeGrade = 38,

        [Description("[Common]: Approve Workflow")]
        ApproveWorkflow = 39,

        //[Description("[PA]: Bid Project")]
        //BidProject = 40,

        [Description("[CF]: Configure Freeze Settings")]
        ConfigureFreezeSettings = 41,

        [Description("[CF]: Configure Archival Settings")]
        ConfigureArchivalSettings = 42,

        [Description("[CF]: Configure Email Settings")]
        ConfigureEmailSettings = 43,



        [Description("[CF]: Configure Sync Settings")]
        ConfigureSyncSettings = 47,

        [Description("[LP]: Configure Module Settings")]
        ConfigureModuleSettings = 48,

        //[Description("[PA]: Configure FYP Settings")]
        //ConfigureFYPSettings = 49,

        [Description("[PA]: Configure IIP Settings")]
        ConfigureIIPSettings = 50,

        [Description("[CF]: Configure Global Archival Settings")]
        ConfigureGlobalArchivalSettings = 51,

        [Description("[Common]: Configure Global Settings")]
        ConfigureGlobalSettings = 52,

        [Description("[PA]: Configure PD Settings")]
        ConfigurePDSettings = 53,

        [Description("[LP]: View Learning Path Lesson")]
        ViewLearningPathLesson = 54,

        [Description("[LP]: Design Learning Path")]
        DesignLearningPath = 55,

        [Description("[LP]: Design Module Level Learning Object")]
        DesignModuleLevelLearningObject = 56,

        [Description("[PA]: Internship Allocation")]
        InternshipAllocation = 57,

        [Description("[CF]: Generate Admin Report")]
        GenerateAdminReport = 58,

        [Description("[CC]: Take Poll")]
        TakePoll = 59,

        [Description("[FA]: PFP Result Slip Printout")]
        PFPResultSlipPrintout = 60,

        //[Description("[PA]: Project Review")]
        //ProjectReview = 61,

        //[Description("[PA]: Publish Project Allocation Result")]
        //PublishProjectAllocationResult = 62,

        //[Description("[PA]: Rank Project")]
        //RankProject = 63,

        [Description("[PA]: Student Activities")]
        StudentActivities = 64,

        [Description("[SLA]: Student Submission")]
        StudentSubmission = 65,

        [Description("[PA]: Student Status Management")]
        StudentStatusManagement = 66,

        [Description("[SLA]: Staff Submission")]
        StaffSubmission = 67,

        [Description("[FA]: SA Configuration")]
        SAConfiguration = 68,

        [Description("[PA]: Track Milestone")]
        TrackMilestone = 69,

        //[Description("[PA]: Track Project")]
        //TrackProject = 70,

        [Description("[PA]: Track Internship Allocation")]
        TrackInternshipAllocation = 71,

        [Description("[PA]: Track Internship")]
        TrackInternship = 72,

        //[Description("[PA]: Team Formation")]
        //TeamFormation = 73,

        //[Description("[PA]: Team Management")]
        //TeamManagement = 74,

        [Description("[Task Common]: Take LO Task")]
        TakeLOTask = 75,

        [Description("[FA]: Take Assessment Task")]
        TakeAssessmentTask = 76,



        [Description("[SS]: Take Survey")]
        TakeSurvey = 79,

        [Description("[CF]: Manage Account")]
        ManageAccount = 80,

        [Description("[LP]: Manage Module Page")]
        ManageModulePage = 81,

        [Description("[LP]: Manage Lesson")]
        ManageLesson = 82,



        [Description("[LP]: Manage Module Level Learning Path")]
        ManageModuleLevelLearningPath = 84,

        [Description("[LP]: Manage Learning Object")]
        ManageLearningObject = 85,

        [Description("[SI]: Manage Links")]
        ManageLinks = 86,

        [Description("[Common]: Manage Module")]
        ManageModule = 87,



        [Description("[CC]: Manage Poll")]
        ManagePoll = 92,

        [Description("[CC]: Manage Discussion Forum")]
        ManageDiscussionForum = 93,

        [Description("[CC]: Manage Wiki")]
        ManageWiki = 94,

        [Description("[CC]: Manage Blog")]
        ManageBlog = 95,

        [Description("[CC]: Manage Comments")]
        ManageComments = 96,

        [Description("[CC]: Manage Skype Chat")]
        ManageSkypeChat = 97,

        [Description("[PA]: Manage Internship Details")]
        ManageInternshipDetails = 98,

        [Description("[CC]: Manage Media Gallery")]
        ManageMediaGallery = 99,

        [Description("[CC]: Manage Announcement")]
        ManageAnnouncement = 100,

        [Description("[CC]: Manage Calendar")]
        ManageCalendar = 101,

        [Description("[CC]: Manage ShareDrive")]
        ManageShareDrive = 102,

        [Description("[CC]: Manage Learning Space")]
        ManageLearningSpace = 103,

        [Description("[CC]: Manage Yammer")]
        ManageYammer = 104,

        [Description("[FA]: Manage Assessment Item")]
        ManageAssessmentItem = 105,

        //[Obsolete("No longer used")]
        [Description("[FA]: Manage Score Grade Mapping")]
        ManageScoreGradeMapping = 106,

        [Description("[FA]: Manage Grade Mapping")]
        ManageGradeScoreMapping = 107,

        [Description("[FA]: Manage Student Assessment")]
        ManageStudentAssessment = 108,

        [Description("[FA]: Manage Assessment")]
        ManageAssessment = 109,

        [Description("[FA]: Manage Global Assessment Configuration")]
        ManageGlobalAssessmentConfiguration = 110,

        [Description("[FA]: ManageModuleCutoff")]
        ManageModuleCutoff = 111,

        [Description("[SLA]: Manage Student Submission")]
        ManageStudentSubmission = 112,

        [Description("[SLA]: Manage Staff Submission")]
        ManageStaffSubmission = 113,

        [Description("[SS]: Manage Survey")]
        ManageSurvey = 114,

        [Description("[SS]: Manage Adhoc Survey")]
        ManageAdhocSurvey = 115,

        [Description("[SLA]: Manage Module Bank")]
        ManageModuleBank = 116,

        [Description("[FA]: Manage SA Grade Change Workflow")]
        ManageSAGradeChangeWF = 117,

        [Description("[FA]: Manage Module Assessment Setting")]
        ManageModuleAssessmentSetting = 118,

        [Description("[FA]: Manage Grading Page")]
        ManageGradingPage = 119,

        [Description("[FA]: Manage Computing Page")]
        ManageComputingPage = 120,

        [Description("[PA]: Manage Company")]
        ManageCompany = 121,

        [Description("[FA]: Manage Supplementary List")]
        ManageSupplementaryList = 122,

        [Description("[FA]: Manage Final Grade")]
        ManageFinalGrade = 123,

        [Description("[FA]: Manage LO Grade")]
        ManageLOGrade = 124,

        //[Description("[PA]: Manage Project Details")]
        //ManageProjectDetails = 125,

        //[Description("[PA]: Manage My Proposal")]
        ManageMyProposal = 126,

        [Description("[SLA]: Manage Submission Category")]
        ManageSubmissionCategory = 127,

        [Description("[CC]: View Comments")]
        ViewComments = 128,

        [Description("[CC]: Manage Wiki Content")]
        ManageWikiContent = 129,



        [Description("[LP]: Review Request")]
        ReviewRequest = 131,

        [Description("[LP]: Approve Request")]
        ApproveRequest = 132,

        //[Obsolete("No longer used")]
        [Description("[FA]: View Score Grade Mapping")]
        ViewScoreGradeMapping = 133,

        [Description("[FA]: View Grade Score Mapping")]
        ViewGradeScoreMapping = 134,

        [Description("[FA]: Approve/Reject Grade change (PFP)")]
        ApproveRejectGradeChange = 135,

        [Description("[CC]: Download ShareDrive")]
        DownloadShareDrive = 136,

        [Description("[CC]: Join Learning Space")]
        JoinLearningSpace = 137,

        [Description("[CC]: Join Skype Chat")]
        JoinSkypeChat = 138,

        [Description("[CC]: View Poll Details for LD")]
        ViewPollDetails = 139,

        [Description("[CC]: View Discussion Forum for LD")]
        ViewDiscussionForumDetails = 140,

        [Description("[CC]: View Skype Chat for LD")]
        ViewSkypeChatDetails = 141,



        [Description("[LP]: Manage LP Module")]
        ManageLPModule = 143,

        [Description("[PA]: View Milestone Tracking")]
        ViewMilestoneTracking = 144,

        //[Description("[PA]: View Project Details")]
        //ViewProjectDetails = 145,
        //[Description("[PA]: View My Proposal")]
        //ViewMyProposal = 146,
        //[Description("[PA]: View Project Review")]
        //ViewProjectReview = 147,
        //[Description("[PA]: View Bid Project")]
        //ViewBidProject = 148,
        //[Description("[PA]: View Track Project")]
        //ViewTrackProject = 149,
        //[Description("[PA]: View Rank Project")]
        //ViewRankProject = 150,
        //[Description("[PA]: View Team Formation")]
        //ViewTeamFormation = 151,
        //[Description("[PA]: View Publish Project Allocation Result")]
        //ViewPublishProjectAllocationResult = 152,
        //[Description("[PA]: View Team Management")]
        //ViewTeamManagement = 153,
        [Description("[PA]: View Internship Allocation")]
        ViewInternshipAllocation = 154,
        [Description("[PA]: View Company")]
        ViewCompany = 155,
        [Description("[PA]: View Track Internship Allocation")]
        ViewTrackInternshipAllocation = 156,
        [Description("[CF]: Manage SQSP Setting")]
        ManageSQSPSetting = 157,

        [Description("[CC]: View Communication Collaboration")]
        ViewCommunicationCollaboration = 158,
        [Description("[LP]: View Staff Material")]
        ViewStaffMaterial = 159,
        [Description("[LP]: Manage Staff Material")]
        ManageStaffMaterial = 160,
        [Description("[LP]: View Lesson")]
        ViewLesson = 161,
        [Description("[PA]: View Internship Track")]
        ViewInternshipTrack = 162,
        [Description("[PA]: View Internship Details")]
        ViewInternshipDetails = 163,
        [Description("[LP]: View Module Level LearningPath")]
        ViewModuleLevelLearningPath = 164,
        [Description("[LP]: Manage Workflow")]
        ManageWorkflow = 165,

        #region PA FYP/SP Levels
        [Description("[PA]: Configure Project Module Setting")]
        FYPSPConfigModule = 166,
        [Description("[PA]: Manage Project Proposal")]
        FYPSPManageProposal = 167,
        [Description("[PA]: View Project Proposal")]
        FYPSPViewProposal = 168,
        [Description("[PA]: Manage Project Details")]
        FYPSPManageProjectDetails = 169,
        [Description("[PA]: View Project Details")]
        FYPSPViewProjectDetails = 170,
        [Description("[PA]: Manage Role Assignment")]
        FYPSPRoleAssignment = 171,
        [Description("[PA]: View Team Formation")]
        FYPSPViewTeam = 172,
        [Description("[PA]: Team Formation")]
        FYPSPTeamFormation = 173,
        [Description("[PA]: Manage Team")]
        FYPSPManageTeam = 174,
        [Description("[PA]: Project Bidding")]
        FYPProjectBidding = 175,
        [Description("[PA]: View Project Bidding")]
        FYPViewProjectBidding = 176,
        [Description("[PA]: Project Review")]
        FYPSPProjectReview = 177,
        [Description("[PA]: Project Rank")]
        FYPProjectRank = 178,
        [Description("[PA]: Project Allocation")]
        FYPSPProjectAllocation = 179,
        [Description("[PA]: Publish Project Allocation")]
        FYPSPPublishProjectAllocation = 180,
        [Description("[PA]: Project Tracking")]
        FYPSPProjectTracking = 181,
        #endregion

        [Description("[CF]: Configure Approval Process Setting")]
        ConfigureApprovalProcessSetting = 182,

        [Description("[Quiz]: Manage Quiz Module Bank")]
        ManageQuizModuleBank = 183,
        [Description("[Quiz]: Manage Quiz Personal Bank")]
        ManageQuizPersonalBank = 184,
        [Description("[Task Common]: Grade LO Task")]
        GradeLOTask = 185,
        [Description("[Task Common]: View LO Task Report")]
        ViewLOTaskReport = 186,
        [Description("[LP]:View Class Level Learning Path")]
        ViewClassLevelLearningPath = 187,
        [Description("[LP]:Design Class Level Learning Path")]
        DesignClassLevelLearningPath = 188,
        [Description("[LP]:Design Module Level Learning Path")]
        DesignModuleLevelLearningPath = 189,
        [Description("[LP]:Design Class Level Learning Ojbect")]
        DesignClassLevelLearningObject = 190,
        [Description("[LP]:Manage Class Level Learning Path")]
        ManageClassLevelLearningPath = 191,

        [Description("[FA]: View Grading Page")]
        ViewGradingPage = 192,
        [Description("[FA]: View LO Grade")]
        ViewLOGrade = 193,

        [Description("[CC]: View Announcement Template Bank")]
        ViewAnnouncementTemplateBank = 194,
        [Description("[CC]: View Poll Template Bank")]
        ViewPollTemplateBank = 195,
        [Description("[CC]: Import Calendar Excel")]
        ImportCalendarExcel = 196,
        [Description("[CF]: Configure Common Topic")]
        ConfigureCommonTopic = 197,

        [Description("[LP]: View LP Module Setting")]
        ViewLPModuleSetting = 198,
        [Description("[LP]: Manage LP Module Setting")]
        ManageLPModuleSetting = 199,

        [Description("[CC]: View CC LO Activity Details")]
        ViewCCLOActivityDetails = 200,
        [Description("[CC]: View CC MyTask Activity Details")]
        ViewCCMyTaskActivityDetails = 201,
        [Description("[CC]: View CC TodayActivity Details")]
        ViewCCTodayActivityDetails = 202,

        [Description("[CC]: Manage Rating Configuration")]
        ManageRatingConfiguration = 203,
        [Description("[CC]: View Rating Configuration")]
        ViewRatingConfiguration = 204,

        [Description("[CC]: View Tag Configuration")]
        ViewTagConfiguration = 205,
        [Description("[CC]: Manage Tag Configuration")]
        ManageTagConfiguration = 206,

        [Description("[CF]: Manage CET Payment Approval")]
        ManageCETPaymentApproval = 207,

        [Description("[CF]: Manage CET Payment Approval Setting")]
        ManageCETPaymentApprovalSetting = 208,

        [Description("[FA]: View Global Assessment Configuration")]
        ViewGlobalAssessmentConfiguration = 210,

        [Description("[FA]: View SA Configuration")]
        ViewSAConfiguration = 211,

        [Description("[FA]: View PFP Result Slip Printout")]
        ViewPFPResultSlipPrintout = 212,

        [Description("[SLA]: View Submission Category")]
        ViewSubmissionCategory = 251,

        [Description("[CF]: View Sync Settings")]
        ViewSyncSettings = 253,
        [Description("[CF]: View Attendance/Grade Sync Settings")]
        ViewAttendanceGradeSyncSettings = 254,
        [Description("[CF]: Configure Attendance/Grade Sync Settings")]
        ConfigureAttendanceGradeSyncSettings = 255,
        [Description("[CF]: View CET Payment Group")]
        ViewCETPaymentGroup = 256,
        [Description("[CF]: Configure CET Payment Group")]
        ConfigureCETPaymentGroup = 257,
        [Description("[CF]: View SQSP Setting")]
        ViewSQSPSetting = 258,
        [Description("[CF]: View Email Settings")]
        ViewEmailSettings = 259,
        [Description("[CF]: View Admin Report")]
        ViewAdminReport = 260,
        [Description("[CF]: View Approval Process Setting")]
        ViewApprovalProcessSetting = 261,
        [Description("[CF]: View Redirection Page")]
        ViewRedirectionPage = 262,
        [Description("[CF]: Configure Redirection Page")]
        ConfigureRedirectionPage = 263,
        [Description("[Common]: View Global Settings")]
        ViewGlobalSettings = 264,
        #region CM related  count: 26

        [Description("[CM]: View Attendance Page")]
        ViewAttendancePage = 8,

        [Description("[CM]: View Class Roster")]
        ViewClassRoster = 12,

        [Description("[CM]: View Al Workflow Report")]
        ViewAlWorkflowReport = 13,

        [Description("[CM]: View Class")]
        ViewClass = 19,

        [Description("[CM]: View Lessons")]
        ViewLessons = 20,

        [Description("[CM]: View Class Team Assignment")]
        ViewClassTeamAssignment = 21,

        [Description("[CM]: View Lesson Configuration Page")]
        ViewLessonConfigurationPage = 22,

        [Description("[CM]: Configure Class Settings")]
        ConfigureClassSettings = 44,

        [Description("[CM]: Configure LMS Class Settings")]
        ConfigureLMSClassSettings = 45,

        [Description("[CM]: Configure Lesson Settings")]
        ConfigureLessonSettings = 46,

        [Description("[CM]: Take Attendance")]
        TakeAttendance = 77,

        [Description("[CM]: Take Lecturer Attendance")]
        TakeLecturerAttendance = 78,

        [Description("[CM]: Manage Lesson List")]
        ManageLessonList = 83,

        [Description("[CM]: Manage LMS Module")]
        ManageLMSModule = 88,

        [Description("[CM]: Manage Debarment Rule")]
        ManageDebarmentRule = 89,

        [Description("[CM]: Manage Venue List")]
        ManageVenueList = 90,

        [Description("[CM]: Manage Class Team")]
        ManageClassTeam = 91,

        [Description("[CM]: View LMS Module")]
        ViewLMSModule = 130,

        [Description("[CM]: Manage Workflow Setting")]
        ManageWorkflowSetting = 142,

        [Description("[CM]: View Venue List")]
        ViewVenueList = 252,

        [Description("[CM]: View Workflow Setting")]
        ViewWorkflowSetting = 265,

        [Description("[CM]: View Globally Not Recorded")]
        ViewGloballyNotRecorded = 266,

        [Description("[CM]: Manage Globally Not Recorded")]
        ManageGloballyNotRecorded = 267,

        [Description("[CM]: View Debarment Rule")]
        ViewDebarmentRule = 268,

        [Description("[CM]: Manage Attendance Setting")]
        ManageAttendanceSetting = 269,

        [Description("[CM]: View Attendance Setting")]
        ViewAttendanceSetting = 270,

        #endregion
        [Description("[Common]: View Module Class Configuration")]
        ViewModuleClassConfiguration = 271,
        [Description("[Common]: Manage Module Class Configuration")]
        ManageModuleClassConfiguration = 272,
        [Description("[LP]: View Module Store Setting")]
        ViewModuleStoreSetting = 273,
        [Description("[LP]: Configure Module Store Setting")]
        ConfigureModuleStoreSetting = 274,
        [Description("[FA]: View SA Store Setting")]
        ViewSAStoreSetting = 275,
        [Description("[FA]: Configure SA Store Setting")]
        ConfigureSAStoreSetting = 276,
        [Description("[CM]: Configure Al Workflow Report")]
        ConfigureAlWorkflowReport = 277,
        [Description("[FA]: View Module Assessment Setting")]
        ViewModuleAssessmentSetting = 278,
        [Description("[FA]: Manage Supplementary List")]
        ViewSupplementaryList = 279,
        [Description("[FA]: View Project CheckList")]
        ViewFYPSPCheckList = 280,
        [Description("[FA]: Manage Project CheckList")]
        ManageFYPSPCheckList = 281,

        [Description("[CC]: Manage CC Student Submission")]
        ManageCCStudentSubmission = 282,

        [Description("[FA]: View MakeUp Exam")]
        ViewMUEList = 283,

        [Description("[FA]: Manage MakeUp Exam")]
        ManageMUEList = 284,

        [Description("[CF]: Configure Class Allocation")]
        ConfigureClassAllocation = 285,

        [Description("[CM]: View Lessons Learning Objects")]
        ViewLessonsLOs = 286,
    }
}
