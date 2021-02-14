using System;

namespace WebService.BenchTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
        }


        public static void Test()
        {

            var connStr = "Host=localhost;Port=3306;Database=ang_tz;Username=root;Password=ZlZlZlJeME21379!2345";
            // var conn = new MySqlConnection(connStr);
            // var conn = new UseMySql(Configuration.GetConnectionString("ConnectDatabase"), new MySqlServerVersion(new Version(8, 0, 23)))
        }
    }
}
