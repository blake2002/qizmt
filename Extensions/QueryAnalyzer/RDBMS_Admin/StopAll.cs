﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDBMS_Admin
{
    partial class Program
    {
        private static void StopAll(string[] args)
        {
            string[] hosts = Utils.GetQizmtHosts();

            if (hosts.Length == 0)
            {
                Console.Error.WriteLine("No Qizmt host is found.");
                return;
            }

            bool proxy = false;
            if (args.Length > 1)
            {
                if (args[1].ToLower() == "-p")
                {
                    proxy = true;
                }
            }

            if (proxy)
            {
                RunViaJob(args[0]);
                return;
            }

            int threadcount = hosts.Length > 12 ? 12 : hosts.Length;

            RDBMS_Admin.ThreadTools<string>.Parallel(
            new Action<string>(
            delegate(string host)
            {
                try
                {
                    lock (hosts)
                    {
                        Console.Write("Stopping service on {0}... ", host);
                        Console.WriteLine(Exec.Shell(@"sc \\" + host + " stop QueryAnalyzer_Protocol", false));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Problem stopping service on {0}: {1}", host, e.Message);
                }
                
                System.Threading.Thread.Sleep(1000 * 2);
            }
            ), hosts, threadcount);
            
            Console.WriteLine("Done");
        }
    }
}
