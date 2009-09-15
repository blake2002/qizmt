﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RDBMS_DBCORE;
using MySpace.DataMining.DistributedObjects;

namespace RDBMS_DBCORE_RegressionTests
{
    public partial class Program
    {
        public static void DbFunctions_NEXT_DAY()
        {
            DbFunctionTools tools = new DbFunctionTools();

            {
                Console.WriteLine("Testing DbFunctions.NEXT_DAY...");

                List<DbValue> args = new List<DbValue>();
                DateTime dt = DateTime.Parse("12/1/2000 10:00:00 AM");
                args.Add(tools.AllocValue(dt));
                DbFunctionArguments fargs = new DbFunctionArguments(args);

                DbValue valOutput = DbFunctions.NEXT_DAY(tools, fargs);
                ByteSlice bs = valOutput.Eval();
                DateTime output = tools.GetDateTime(bs);

                DateTime expected = DateTime.Parse("12/2/2000 10:00:00 AM");

                if (expected != output)
                {
                    throw new Exception("DbFunctions.NEXT_DAY has failed.  Expected result: " + expected.ToString() + ", but received: " + output.ToString());
                }
                else
                {
                    Console.WriteLine("Expected results received.");
                }
            }

        }
    }
}
