using Logger.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;


namespace Logger
{
    public interface ILogHandler
    {
  
        Exception SaveError(Exception ex, MethodBase methodInfo, params object[] parameterValues);

        void LogError(Exception ex, MethodBase serviceMethodInfo, params object[] serviceParameterValues);

        void LogData(MethodBase methodInfo, object answer, TimeSpan? executeTime, params object[] parameterValues);

        void CreateOperationLog(OperationLog operationLog);

        void CreateSystemErrorLog(SystemErrorLog errorLog);


       
    }
}
