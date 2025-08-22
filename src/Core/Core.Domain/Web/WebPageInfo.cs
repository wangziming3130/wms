using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class ComboboxItem
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public bool Checked { get; set; }
        public string ExtraProps { get; set; }
    }

    public class TablePageParameter
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string SortKey { get; set; } = string.Empty;

        public bool IsAscending { get; set; }

        public string SearchContent { get; set; }

        public Dictionary<string, List<string>> Filters { get; set; } = new Dictionary<string, List<string>>();
    }

    public class TableQueryParameter<T, TOrder>
    {
        public Pager Pager { get; set; } = new Pager();
        public Expression<Func<T, bool>> Filter { get; set; }
        public Sorter<T, TOrder> Sorter { get; set; } = new Sorter<T, TOrder>();
    }

    public class TableInfo<T>
    {
        public List<T> Items { get; set; }
        public int PageCount { get; set; }
        public int TotalItemsCount { get; set; }
    }

    public class Pager
    {
        public int Index { get; set; }
        public int Size { get; set; }
    }

    public class Sorter<T, TResult>
    {
        public Expression<Func<T, TResult>> SortBy { get; set; }
        public Expression<Func<T, TResult>> ThenSortBy { get; set; }
        public bool IsAscending { get; set; }
    }

    public class DataCountInfo<T>
    {
        public List<T> Items { get; set; }
        public int TotalItemsCount { get; set; }
    }


    public class FilterKey
    {
        #region Common
        public const string School = "Schools";
        public const string Programme = "Programmes";
        public const string Semester = "Semesters";
        public const string IsStudent = "IsStudent";
        public const string Blank = "Blank";
        public const string User = "Users";
        public const string Staff = "Staffs";
        public const string FirstLoad = "FirstLoad";
        public const string HomePage = "HomePage";
        public const string Budget = "Budget";
        public const string SeletedIds = "SeletedIds";
        public const string UserStatus = "UserStatus";
        public const string MemberShips = "MemberShips";
        public const string ActionType = "ActionType";
        public const string Component = "Component";
        public const string StartTime = "StartTime";
        public const string EndTime = "EndTime";
        public const string Count = "Count";
        public const string UserId = "UserIds";
        public const string AuditActionType = "AuditActionType";
        public const string Components = "Components";
        public const string SearchKey = "SearchKey";
        public const string StartDate = "StartDate";
        public const string EndDate = "EndDate";
        #endregion

        #region Teach
        public const string Duration = "Durations";
        public const string Attendance = "Attendance";
        public const string Course = "Courses";
        public const string CourseCode = "CourseCodes";
        public const string CourseStatus = "CourseStatus";
        public const string Class = "Classes";
        public const string ClassCode = "ClassCodes";
        public const string Lesson = "Lessons";
        public const string LessonStatus = "Status";
        public const string ClassLesson = "ClassLessons";
        public const string LearningObject = "LearningObjects";
        public const string CourseProvider = "CourseProvider";
        public const string CourseCategory = "CourseCategory";
        public const string CourseType = "CourseType";
        public const string IsMember = "IsMember";
        public const string CourseFeeStatus = "CourseFeeStatus";
        public const string ClassRoster = "ClassRoster";
        #endregion

        #region Bank
        public const string BankStatus = "BankStatus";
        public const string BankType = "BankType";
        public const string QuestionTypes = "QuestionTypes";
        public const string CompetencyLevels = "CompetencyLevels";
        public const string BankCategory = "BankCategory";
        #endregion

        #region Learn
        public const string Tab = "Tabs";
        public const string AudienceType = "AudienceTypes";
        public const string TaskModelStatus = "TaskModelStatus";
        public const string TaskType = "TaskType";
        public const string TaskStatus = "TaskStatus";
        public const string Submit = "Submit";
        #endregion

        #region Marking
        public const string OnlySubmitted = "OnlySubmitted";
        public const string PublishStatus = "PublishStatus";
        public const string Marker = "Markers";
        public const string CoMarker = "CoMarkers";
        public const string ShowNeedToBeMarked = "ShowNeedToBeMarked";
        #endregion

        #region PeoplePickerAddressbook
        public const string SigninType = "SigninType";
        public const string UserType = "UserType";
        public const string SearchType = "SearchType";
        public const string Inactiveable = "Inactiveable";
        #endregion

        #region Export/Import User
        public const string IsExport = "IsExport";
        #endregion

        #region SSG
        public const string CourseReferenceId = "CourseReferenceId";
        public const string SyncType = "SyncType";
        public const string SyncTime = "SyncTime";
        public const string SyncStatus = "SyncStatus";
        public const string APIMethod = "APIMethod";
        public const string SyncJob = "SyncJob";
        public const string LoadFailedJob = "LoadFailedJob";
        #endregion
    }

    public class OperationResult<T>
    {
        public List<T> Items { get; set; }
        public OperationResultStatus Status { get; set; }
        public string Message { get; set; }
        public string Resource { get; set; }
    }
    public class OperationResult
    {
        public dynamic Items { get; set; }
        public OperationResultStatus Status { get; set; }
        public string Message { get; set; }
        public string Resource { get; set; }
    }


    public class OperationResultItem
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }

    public enum OperationResultStatus
    {
        Success = 0,
        Fail,
        Warn
    }
}
