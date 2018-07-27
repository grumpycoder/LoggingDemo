using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace LoggingDemo.Core
{
    public static class Logger
    {
        private static readonly ILogger _perfLogger;
        private static readonly ILogger _usageLogger;
        private static readonly ILogger _errorLogger;
        private static readonly ILogger _diagnosticLogger;
        private static readonly string _connStr;

        static Logger()
        {
            _connStr = ConfigurationManager.ConnectionStrings["LoggingContext"].ConnectionString;
            //_perfLogger = new LoggerConfiguration()
            //    .WriteTo.File(path: @"c:\temp\logs\perf.txt")
            //    .WriteTo.MSSqlServer(_connStr, "Perf", autoCreateSqlTable: true, schemaName: "Logs",
            //            columnOptions: GetSqlColumnOptions(), batchPostingLimit: 1)
            //    .CreateLogger();

            //_usageLogger = new LoggerConfiguration()
            //    .WriteTo.File(path: @"c:\temp\logs\usage.txt")
            //    .WriteTo.MSSqlServer(_connStr, "Usage", autoCreateSqlTable: true, schemaName: "Logs",
            //        columnOptions: GetSqlColumnOptions(), batchPostingLimit: 1)
            //    .CreateLogger();

            //_errorLogger = new LoggerConfiguration()
            //    .WriteTo.File(path: @"c:\temp\logs\error.txt")
            //    .WriteTo.MSSqlServer(_connStr, "Error", autoCreateSqlTable: true, schemaName: "Logs",
            //        columnOptions: GetSqlColumnOptions(), batchPostingLimit: 1)
            //    .CreateLogger();

            //_diagnosticLogger = new LoggerConfiguration()
            //    .WriteTo.File(path: @"c:\temp\logs\diagnostic.txt")
            //    .WriteTo.MSSqlServer(_connStr, "Diagnostic", autoCreateSqlTable: true, schemaName: "Logs",
            //        columnOptions: GetSqlColumnOptions(), batchPostingLimit: 1)
            //    .CreateLogger();
            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

            _perfLogger = new LoggerConfiguration()
                //.WriteTo.File(path: @"c:\temp\logs\perf.txt")
                .WriteTo.MSSqlServer(_connStr, "Perf", autoCreateSqlTable: true,
                    columnOptions: GetSqlColumnOptions(), batchPostingLimit: 1)
                .CreateLogger();

            _usageLogger = new LoggerConfiguration()
                //.WriteTo.File(path: @"c:\temp\logs\usage.txt")
                .WriteTo.MSSqlServer(_connStr, "Usage", autoCreateSqlTable: true,
                    columnOptions: GetSqlColumnOptions(), batchPostingLimit: 1)
                .CreateLogger();

            _errorLogger = new LoggerConfiguration()
                //.WriteTo.File(path: @"c:\temp\logs\error.txt")
                .WriteTo.MSSqlServer(_connStr, "Error", autoCreateSqlTable: true,
                    columnOptions: GetSqlColumnOptions(), batchPostingLimit: 1)
                .CreateLogger();

            _diagnosticLogger = new LoggerConfiguration()
                //.WriteTo.File(path: @"c:\temp\logs\diagnostic.txt")
                .WriteTo.MSSqlServer(_connStr, "Diagnostic", autoCreateSqlTable: true,
                    columnOptions: GetSqlColumnOptions(), batchPostingLimit: 1)
                .CreateLogger();

        }

        public static ColumnOptions GetSqlColumnOptions()
        {
            var colOptions = new ColumnOptions();
            colOptions.Store.Remove(StandardColumn.Properties);
            colOptions.Store.Remove(StandardColumn.MessageTemplate);
            colOptions.Store.Remove(StandardColumn.Message);
            colOptions.Store.Remove(StandardColumn.Exception);
            colOptions.Store.Remove(StandardColumn.TimeStamp);
            colOptions.Store.Remove(StandardColumn.Level);

            colOptions.AdditionalDataColumns = new Collection<DataColumn>
            {
                new DataColumn {DataType = typeof(DateTime), ColumnName = "Timestamp"},
                new DataColumn {DataType = typeof(string), ColumnName = "Product"},
                new DataColumn {DataType = typeof(string), ColumnName = "Layer"},
                new DataColumn {DataType = typeof(string), ColumnName = "Location"},
                new DataColumn {DataType = typeof(string), ColumnName = "Message"},
                new DataColumn {DataType = typeof(string), ColumnName = "Hostname"},
                new DataColumn {DataType = typeof(string), ColumnName = "UserId"},
                new DataColumn {DataType = typeof(string), ColumnName = "UserName"},
                new DataColumn {DataType = typeof(string), ColumnName = "Exception"},
                new DataColumn {DataType = typeof(int), ColumnName = "ElapsedMilliseconds"},
                new DataColumn {DataType = typeof(string), ColumnName = "CorrelationId"},
                new DataColumn {DataType = typeof(string), ColumnName = "CustomException"},
                new DataColumn {DataType = typeof(string), ColumnName = "AdditionalInfo"},
            };

            return colOptions;
        }

        public static void WritePerf(LogDetail infoToLog)
        {
            //_perfLogger.Write(LogEventLevel.Information, "{@LogDetail}", infoToLog);
            _perfLogger.Write(LogEventLevel.Information,
                    "{Timestamp}{Message}{Layer}{Location}{Product}" +
                    "{CustomException}{ElapsedMilliseconds}{Exception}{Hostname}" +
                    "{UserId}{UserName}{CorrelationId}{AdditionalInfo}",
                    infoToLog.Timestamp, infoToLog.Message,
                    infoToLog.Layer, infoToLog.Location,
                    infoToLog.Product, infoToLog.CustomException,
                    infoToLog.ElapsedMilliseconds, infoToLog.Exception?.ToBetterString(),
                    infoToLog.Hostname, infoToLog.UserId,
                    infoToLog.UserName, infoToLog.CorrelationId,
                    infoToLog.AdditionalInfo
                );

        }
        public static void WriteUsage(LogDetail infoToLog)
        {
            //_usageLogger.Write(LogEventLevel.Information, "{@LogDetail}", infoToLog);

            _usageLogger.Write(LogEventLevel.Information,
                "{Timestamp}{Message}{Layer}{Location}{Product}" +
                "{CustomException}{ElapsedMilliseconds}{Exception}{Hostname}" +
                "{UserId}{UserName}{CorrelationId}{AdditionalInfo}",
                infoToLog.Timestamp, infoToLog.Message,
                infoToLog.Layer, infoToLog.Location,
                infoToLog.Product, infoToLog.CustomException,
                infoToLog.ElapsedMilliseconds, infoToLog.Exception?.ToBetterString(),
                infoToLog.Hostname, infoToLog.UserId,
                infoToLog.UserName, infoToLog.CorrelationId,
                infoToLog.AdditionalInfo
            );
        }
        public static void WriteError(LogDetail infoToLog)
        {
            if (infoToLog.Exception != null)
            {
                var procName = FindProcName(infoToLog.Exception);
                infoToLog.Location = string.IsNullOrEmpty(procName) ? infoToLog.Location : procName;
                infoToLog.Message = GetMessageFromException(infoToLog.Exception);
            }
            //_errorLogger.Write(LogEventLevel.Information, "{@LogDetail}", infoToLog);            
            _errorLogger.Write(LogEventLevel.Information,
                "{Timestamp}{Message}{Layer}{Location}{Product}" +
                "{CustomException}{ElapsedMilliseconds}{Exception}{Hostname}" +
                "{UserId}{UserName}{CorrelationId}{AdditionalInfo}",
                infoToLog.Timestamp, infoToLog.Message,
                infoToLog.Layer, infoToLog.Location,
                infoToLog.Product, infoToLog.CustomException,
                infoToLog.ElapsedMilliseconds, infoToLog.Exception?.ToBetterString(),
                infoToLog.Hostname, infoToLog.UserId,
                infoToLog.UserName, infoToLog.CorrelationId,
                infoToLog.AdditionalInfo
            );
        }
        public static void WriteDiagnostic(LogDetail infoToLog)
        {
            var writeDiagnostics = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableDiagnostics"]);
            if (!writeDiagnostics)
                return;

            //_diagnosticLogger.Write(LogEventLevel.Information, "{@LogDetail}", infoToLog);
            _diagnosticLogger.Write(LogEventLevel.Information,
                "{Timestamp}{Message}{Layer}{Location}{Product}" +
                "{CustomException}{ElapsedMilliseconds}{Exception}{Hostname}" +
                "{UserId}{UserName}{CorrelationId}{AdditionalInfo}",
                infoToLog.Timestamp, infoToLog.Message,
                infoToLog.Layer, infoToLog.Location,
                infoToLog.Product, infoToLog.CustomException,
                infoToLog.ElapsedMilliseconds, infoToLog.Exception?.ToBetterString(),
                infoToLog.Hostname, infoToLog.UserId,
                infoToLog.UserName, infoToLog.CorrelationId,
                infoToLog.AdditionalInfo
            );
        }

        private static string GetMessageFromException(Exception ex)
        {
            if (ex.InnerException != null)
                return GetMessageFromException(ex.InnerException);

            return ex.Message;
        }

        private static string FindProcName(Exception ex)
        {
            if (ex is SqlException sqlEx)
            {
                var procName = sqlEx.Procedure;
                if (!string.IsNullOrEmpty(procName))
                    return procName;
            }

            if (!string.IsNullOrEmpty((string)ex.Data["Procedure"]))
            {
                return (string)ex.Data["Procedure"];
            }

            if (ex.InnerException != null)
                return FindProcName(ex.InnerException);

            return null;
        }
    }
}