using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.SystemConsole.Themes;

namespace WebApi.ServicesConfig
{
    public class SerilogConfigs
    {

        
        /// <summary>
        /// Use Console for logging and set set configs
        /// </summary>
        public void UseConsole()
        {

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Information()
                .CreateLogger();
            
        }
        /// <summary>
        /// Use file for logging and set set configs
        /// </summary>
        public void UseFile()
        {

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("Serilog/Log.txt", rollingInterval: RollingInterval.Month, fileSizeLimitBytes: null, rollOnFileSizeLimit: true, retainedFileCountLimit: null)
                .MinimumLevel.Information()
                .CreateLogger();
          
        }
        /// <summary>
        /// Use SqlServer for logging and set set configs
        /// </summary>
        public void UseSqlServer()
        {

            var ColumnPtions = new ColumnOptions();//options
            Log.Logger = new LoggerConfiguration()
                .WriteTo.MSSqlServer(connectionString: "",
                sinkOptions: new MSSqlServerSinkOptions
                {
                    TableName = "LogEvents",
                    AutoCreateSqlTable = true
                })
                .MinimumLevel.Information()
                .CreateLogger();

        }
        /// <summary>
        /// Use Seq for logging and set set configs
        /// </summary>
        public void UseSeq()
        {
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Seq("http://localhost:5341/")
                .MinimumLevel.Information()
                .CreateLogger();
        }
    }
}
