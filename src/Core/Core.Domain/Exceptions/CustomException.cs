using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain

{
    public class CustomExceptionBase : Exception
    {
        public CustomExceptionBase() : base()
        {
        }

        public CustomExceptionBase(string message) : base(message)
        {
        }
    }



    //public class AccessDeniedException : CustomExceptionBase
    //{
    //    public AccessDeniedException() : base()
    //    {
    //    }

    //    public AccessDeniedException(string message) : base(message)
    //    {
    //    }
    //}
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException(string message) : base(message)
        { }

        public AccessDeniedException(string message, Exception inner) : base(message, inner)
        { }

        //public AccessDeniedException() : base(ExceptionMessage.CF_NoAuthorize_NoPermission_Message.ToDescription())
        //{ }

        public AccessDeniedException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        { }
    }

    public class LoginUnauthorizedException : Exception
    {
        public LoginUnauthorizedException(string message) : base(message)
        { }
    }

    public class ConcurrentUnauthorizedException : Exception
    {
        public ConcurrentUnauthorizedException(string message) : base(message)
        { }
    }

    //public class ResourceNotFoundException : CustomExceptionBase
    //{
    //    public ResourceNotFoundException() : base()
    //    {
    //    }

    //    public ResourceNotFoundException(string message) : base(message)
    //    {
    //    }
    //}

    //public class UnExpectedException : CustomExceptionBase
    //{
    //    public UnExpectedException() : base()
    //    {
    //    }
    //    public UnExpectedException(string message) : base(message)
    //    {
    //    }
    //}
    //[Serializable]
    public class ResourceNotFoundException : HttpException
    {
        public ResourceNotFoundException() : base((int)HttpStatusCode.NotFound, "The resource cannot be found.")
        { }
        public ResourceNotFoundException(string message) : base((int)HttpStatusCode.NotFound, message) { }
        //public ResourceNotFoundException(SerializationInfo info, StreamingContext context) :
        //    base(info, context)
        //{ }
    }

    public class HttpUnhandledException : HttpException
    {

        public HttpUnhandledException() : base((int)HttpStatusCode.MethodNotAllowed)
        {
        }


        public HttpUnhandledException(string message)
            : base((int)HttpStatusCode.MethodNotAllowed, message) { }


        public HttpUnhandledException(string message, Exception innerException)
          : base((int)HttpStatusCode.MethodNotAllowed, message, innerException)
        {

        }
    }

    //[Serializable]
    public class UnExpectedException : HttpException
    {
        public UnExpectedException() : base((int)HttpStatusCode.Unused, "Exception error encountered. Please capture a screenshot of this page and send to Help-IT for assistance.")
        { }
        public UnExpectedException(string message) : base((int)HttpStatusCode.Unused, message) { }
        //public UnExpectedException(SerializationInfo info, StreamingContext context) :
        //    base(info, context)
        //{ }
    }

    public class Error500Exception : CustomExceptionBase
    {
        public Error500Exception(string message) : base(message)
        {
        }
    }

    public class Error404Exception : CustomExceptionBase
    {
        public Error404Exception() : base()
        {
        }
    }

    public class ErrorHandleException : CustomExceptionBase
    {
        public ErrorHandleException(string message) : base(message)
        {
        }
    }

    public class NotExistException : CustomExceptionBase
    {
        public NotExistException(string sourceName) : base($"The {sourceName} is not exist.")
        {
        }
    }

    public class WarningHandleException : CustomExceptionBase
    {
        public WarningHandleException(string message) : base(message)
        {
        }
    }

    public class GraphForbiddenException : CustomExceptionBase
    {
        public GraphForbiddenException() : base()
        {
        }
    }

    public class WrongVideoStatusException : CustomExceptionBase
    {
        public WrongVideoStatusException(string message) : base(message)
        {
        }
    }

    public class GraphMailNicknameAlreadyExistsException : CustomExceptionBase
    {
        public GraphMailNicknameAlreadyExistsException() : base()
        {
        }
    }

    public class BusinessException : CustomExceptionBase
    {
        public BusinessException() : base()
        {
        }
    }

    public class SkipException : CustomExceptionBase
    {
        public SkipException() : base()
        {
        }
    }





    public class UnsupportedMethodException : NotSupportedException
    {
        public string MethodName
        {
            get;
            private set;
        }

        public UnsupportedMethodException(string methodName)
        {
            MethodName = methodName;
        }
    }

    public class FatalException : Exception
    {
        public FatalException(string msg)
       : base(msg)
        {

        }

    }

}