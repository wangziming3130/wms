using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using Core.Domain;
using Core.Utility;
using System.Net.Http.Headers;

namespace Core.Web
{
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    //[APIObjectResult]
    //[APIResultException]
    //[Authorize]
    public class APIBaseController
    {
        protected static readonly SiasunLogger logger = SiasunLogger.GetInstance(MethodBase.GetCurrentMethod().DeclaringType);
        protected internal readonly HttpContext _httpContext;

        public APIBaseController(IHttpContextAccessor accessor)
        {
            _httpContext = accessor.HttpContext;
        }

        #region Controller Action
        [NonAction]
        public virtual OkResult Ok()
            => new OkResult();
        [NonAction]
        public virtual OkObjectResult Ok([ActionResultObjectValue] object value)
            => new OkObjectResult(value);
        [NonAction]
        public virtual ContentResult Content(string content, MediaTypeHeaderValue contentType)
        {
            var result = new ContentResult
            {
                Content = content,
                ContentType = contentType?.ToString()
            };

            return result;
        }
        [NonAction]
        public virtual ContentResult Content(string content)
            => Content(content, (MediaTypeHeaderValue)null);

        [NonAction]
        public virtual JsonResult Json([ActionResultObjectValue] object value)
            => new JsonResult(value);
        #endregion

        #region For User
        protected AccountIdentity CurrentUser
        {
            get
            {
                return _httpContext.GetCurrentUser();
            }
        }

        public bool IsModuleClassAdmin
        {
            get
            {
                return IsInRole(UserRoleName.ModuleClassAdmin);
            }
        }

        public bool IsSuperAdmin
        {
            get
            {
                return IsInRole(UserRoleName.SuperAdmin);
            }
        }

        public bool IsPM
        {
            get
            {
                return IsInRole(UserRoleName.ProgrammeManager);
            }
        }

        public bool IsModuleAdmin
        {
            get
            {
                return IsInRoles(new List<UserRoleName> { UserRoleName.ORGAdmin, UserRoleName.PFPAdmin, UserRoleName.CETAdmin });
            }
        }

        public bool IsORGExam
        {
            get
            {
                return IsInRole(UserRoleName.ORGExam);
            }
        }

        public bool IsORGAdmin
        {
            get
            {
                return IsInRole(UserRoleName.ORGAdmin);
            }
        }

        public bool IsPFPAdmin
        {
            get
            {
                return IsInRole(UserRoleName.PFPAdmin);
            }
        }

        public bool IsIIPAdmin
        {
            get
            {
                return IsInRole(UserRoleName.IIPAdmin);
            }
        }

        public bool IsCETAdmin
        {
            get
            {
                return IsInRole(UserRoleName.CETAdmin);
            }
        }

        public bool IsAppSupport
        {
            get
            {
                return IsInRole(UserRoleName.AppSupport);
            }
        }

        public bool IsSchoolAdmin
        {
            get
            {
                return IsInRole(UserRoleName.SchoolAdmin);
            }
        }

        public bool IsADAC
        {
            get
            {
                return IsInRole(UserRoleName.ADAC);
            }
        }

        public bool IsOASAdmin
        {
            get
            {
                return IsInRole(UserRoleName.OASAdmin);
            }
        }

        public bool IsLecturer
        {
            get
            {
                return IsInRoles(new List<UserRoleName> { UserRoleName.Lecturer, UserRoleName.CoveringLecturer });
            }
        }

        public bool IsClassOwner
        {
            get
            {
                return IsInRoles(new List<UserRoleName> { UserRoleName.Lecturer });
            }
        }

        public bool IsMarker
        {
            get
            {
                return IsInRoleExact(UserRoleName.QuizMarker);
            }
        }

        public bool IsStudent
        {
            get
            {
                return IsInRoleExact(UserRoleName.Student);
            }
        }

        public bool IsModuleClassAdminRead
        {
            get
            {
                return IsInRoleExact(UserRoleName.ModuleClassAdminRead);
            }
        }

        public bool IsModuleManager
        {
            get
            {
                return new List<UserRoleName>()
                        {
                            UserRoleName.ModuleAdmin,
                            UserRoleName.ModuleManager,
                            UserRoleName.CoModuleManager,
                            UserRoleName.LessonDesigner
                        }.Any(a => IsInRoleExact(a));
            }
        }

        public bool IsOnlyModuleClassAdmin
        {
            get
            {
                return !IsSuperAdmin && !IsModuleManager && !IsLecturer && !IsClassOwner && IsModuleClassAdmin;
            }
        }

        public bool IsOnlyModuleClassAdminRead
        {
            get
            {
                return !new List<UserRoleName>
                        {
                            UserRoleName.SuperAdmin,
                            UserRoleName.ORGExam,
                            UserRoleName.AppSupport,
                            UserRoleName.PFPAdmin,
                            UserRoleName.CETAdmin,
                            UserRoleName.ORGAdmin,
                            UserRoleName.SchoolAdmin,
                            UserRoleName.ADAC,
                            UserRoleName.ModuleAdmin,
                            UserRoleName.ModuleManager,
                            UserRoleName.CoModuleManager,
                            UserRoleName.LessonDesigner,
                            UserRoleName.Lecturer,
                            UserRoleName.CoveringLecturer,
                            UserRoleName.ClassOwner,
                            UserRoleName.ModuleClassAdmin,
                            UserRoleName.OASAdmin
                        }.Any(a => IsInRoleExact(a)) && IsInRoleExact(UserRoleName.ModuleClassAdminRead);
            }
        }


        [NonAction]
        public bool IsInRoleExact(UserRoleName role)
        {
            return CurrentUser.IsInRoleExact(role);
        }

        [NonAction]
        public bool IsInRole(UserRoleName role)
        {
            return CurrentUser.IsInRole(role);
        }

        [NonAction]
        public bool IsInRoles(List<UserRoleName> roles)
        {
            if (roles != null && roles.Count > 0)
            {
                return CurrentUser.IsInRoles(roles);
            }
            return false;
        }
        #endregion

        #region For Entity
        //protected T InitCreationInfo<T>(T entity) where T : BaseEntity
        //{
        //    entity.CreatedTimeTicks = DateTime.UtcNow.Ticks;
        //    entity.ModifiedTimeTicks = entity.CreatedTimeTicks;
        //    entity.CreatedById = _httpContext.GetUserId();
        //    entity.ModifiedById = entity.CreatedById;
        //    return entity;
        //}
        //protected T InitUpdateInfo<T>(T entity) where T : BaseEntity
        //{
        //    entity.ModifiedTimeTicks = DateTime.UtcNow.Ticks;
        //    entity.ModifiedById = _httpContext.GetUserId();
        //    return entity;
        //}
        #endregion

        #region For File
        //protected FileContentResult GetExcelExportFileContent(byte[] bytes, string fileName)
        //{
        //    fileName = FormatDownloadFileName(fileName, ExcelConstants.ExcelSuffix);
        //    SetResponseHeaderForFileContent(fileName);
        //    return new FileContentResult(bytes, ExcelConstants.ContentType)
        //    {
        //        FileDownloadName = fileName
        //    };
        //}

        protected FileStreamResult GetExcelExportFileStreamContent(FileStream fs, string fileName)
        {
            //fileName = FormatDownloadFileName(fileName, ExcelConstants.ExcelSuffix);
            SetResponseHeaderForFileContent(fileName);
            return new FileStreamResult(fs, "application/octet-stream")
            {
                FileDownloadName = fileName
            };
        }

        //protected FileContentResult GetZipFileContentForExcel(List<ZipFileInfo> files, string zipFileName)
        //{
        //    zipFileName = FormatDownloadFileName(zipFileName, ZipFileConstants.ZipFileExtension);
        //    SetResponseHeaderForFileContent(zipFileName);

        //    files.ForEach(f => f.FileName = FormatDownloadFileName(f.FileName, ExcelConstants.ExcelSuffix));
        //    var zipFileContent = ZipHelper.GetZipArchive(files);
        //    return new FileContentResult(zipFileContent, ZipFileConstants.ZipContentType)
        //    {
        //        FileDownloadName = zipFileName
        //    };
        //}

        //private string FormatDownloadFileName(string fileName, string fileExtension)
        //{
        //    var dateTime = UnifiedTimezone.ConvertToSpecifiedTimeZone(DateTime.UtcNow, GetTimeOffset());
        //    //return $"{fileName} {dateTime.ToString(ExcelConstants.DateTimeFormat)}{fileExtension}";
        //    return string.Format(fileName, dateTime.ToString(ExcelConstants.DateTimeFormat)) /*+ fileExtension*/;

        //}

        private void SetResponseHeaderForFileContent(string fileName)
        {
            fileName = System.Uri.EscapeUriString(fileName);
            _httpContext.Response.Headers["Content-Disposition"] = "attachment;filename=\"" + fileName + "\"";
            _httpContext.Response.Headers["Access-Control-Expose-Headers"] = $"filename,Content-Disposition,{LoggerDefault.CorrelationIdRequestHeader}";
            _httpContext.Response.Headers["filename"] = fileName;
        }
        #endregion

        #region Other
        protected double GetTimeOffset()
        {
            double timeoffset = 0;
            _httpContext.Request.Headers.TryGetValue("Timeoffset", out var timeoffsetstr);
            string[] timeoffsetArr = timeoffsetstr.ToArray();
            if (timeoffsetArr.Length > 0)
            {
                Double.TryParse(timeoffsetArr[0], out timeoffset);
            }
            return timeoffset;
        }
        #endregion
    }
}
