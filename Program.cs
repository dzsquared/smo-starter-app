using System;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc; 

namespace Azure.Samples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // it would be ideal for these to be passed in as command arguments
            String sqlServerLogin = "sa";
            String password = "P@ssw0rd";
            String remoteSvrName = "localhost";

            // create a remote server connection
            ServerConnection srvConn2 = new ServerConnection(remoteSvrName);
            srvConn2.LoginSecure = false;
            srvConn2.Login = sqlServerLogin;
            srvConn2.Password = password;
            Server srv = new Server(srvConn2);

            // print the server version to show the connection is established
            Console.WriteLine(srv.Information.Version);

            // specify the database
            String dbName = "workorder-database";
            Database db = srv.Databases[dbName]; 

             // Define a Scripter object and set some scripting options
            Scripter scrp = new Scripter(srv);  
            scrp.Options.ScriptDrops = false;  
            scrp.Options.WithDependencies = true;  
            scrp.Options.Indexes = true;   // To include indexes  
            scrp.Options.DriAllConstraints = true;   // to include referential constraints in the script  
        
            // Iterate through the tables in database and script each
            foreach (Table tb in db.Tables) {   
                // skip system tables
                if (tb.IsSystemObject == false) {  
                    Console.WriteLine("-- Scripting for table " + tb.Name);  
        
                    // Generating script and output to console
                    System.Collections.Specialized.StringCollection sc = scrp.Script(new Urn[]{tb.Urn});  
                    foreach (string st in sc) {
                        Console.WriteLine(st);
                    }  
                    Console.WriteLine("--");
                }  
            }   
        }
    }
}