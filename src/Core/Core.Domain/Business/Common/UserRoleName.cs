using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public enum UserRoleName
    {
        [Description("None")]
        None = 0,

        [Description("Super Admin")]
        SuperAdmin = 1,

        [Description("System Owner")]
        SystemOwner = 2,

        [Description("ORG Exam")]
        ORGExam = 3,

        [Description("ORG Admin")]
        ORGAdmin = 4,

        [Description("CET Admin")]
        CETAdmin = 5,

        [Description("PFP Admin")]
        PFPAdmin = 6,

        [Description("IIP Admin")]
        IIPAdmin = 7,

        [Description("Survey Admin")]
        SurveyAdmin = 8,

        [Description("School Admin")]
        SchoolAdmin = 9,

        [Description("Programme Manager")]
        ProgrammeManager = 10,

        [Description("Head Of Department")]
        HeadOfDepartment = 11,

        [Description("Module Manager")]
        ModuleManager = 12,

        [Description("Co-Module Manager")]
        CoModuleManager = 13,

        [Description("Module Admin")]
        ModuleAdmin = 14,

        [Description("Lecturer")]
        Lecturer = 15,

        [Description("Covering Lecturer")]
        CoveringLecturer = 16,

        [Description("Mentor")]
        Mentor = 17,

        [Description("Observer")]
        Observer = 18,

        [Description("Student")]
        Student = 19,

        [Description("Lesson Designer")]
        LessonDesigner = 20,

        [Description("Reviewer")]
        Reviewer = 21,

        [Description("Approver")]
        Approver = 22,

        [Description("Supervisor")]
        Supervisor = 23,

        [Description("Assessor")]
        Assessor = 24,

        [Description("Reporting Officer")]
        ReportingOfficer = 25,

        [Description("Class Owner")]
        ClassOwner = 26,

        [Description("Liaison Officer")]
        LiaisonOfficer = 27,

        [Description("Workflow Approver")]
        WorkflowApprover = 28,

        [Description("Lesson Reviewer")]
        LessonReviewer = 29,

        [Description("Lesson Approver")]
        LessonApprover = 30,

        [Description("Assistant Director (Academic)")]
        ADAC = 31,

        [Description("App Support")]
        AppSupport = 32,

        [Description("LO Assessor")]
        LOAssessor = 33,

        [Description("PA Reviewer")]
        PAReviewer = 34,

        [Description("Marker")]
        QuizMarker = 35,

        [Description("PA Assessor")]
        PAAssessor = 36,

        [Description("Archival Viewer")]
        ArchivalViewer = 37,

        [Description("Archival Module Manager")]
        ArchivalModuleManager = 38,

        [Description("Programme Head")]
        ProgrammeHead = 39,

        [Description("Module/Class Admin")]
        ModuleClassAdmin = 40,

        [Description("Module/Class Admin(Read)")]
        ModuleClassAdminRead = 41,

        [Description("Customized Role")]
        CustomizedRole = 66,

        [Description("OAS Admin")]
        OASAdmin = 42,

        [Description("Learning Path Viewer")]
        LearningPathViewer = 43,
    }
}
