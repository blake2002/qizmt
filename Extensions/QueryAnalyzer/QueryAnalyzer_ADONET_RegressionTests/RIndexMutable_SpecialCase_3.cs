﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace QueryAnalyzer_ADONET_RegressionTests
{
    public partial class Program
    {
        public static void RIndexMutable_SpecialCase_3()
        {
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            string tablename = "rselect_test_" + guid;
            string indexname = guid + "apple";
            string indexname_order = guid + "order";

            string[] allhosts = null;
            {
                string[] installs = Exec.Shell("Qizmt slaveinstalls").Trim()
                    .Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                allhosts = new string[installs.Length];

                for (int ip = 0; ip < installs.Length; ip++)
                {
                    string[] parts = installs[ip].Split(' ');
                    allhosts[ip] = parts[0];
                }
            }

            System.Data.Common.DbProviderFactory fact = DbProviderFactories.GetFactory("DSpace_DataProvider");

            {
                Console.WriteLine("Creating data, table, and index...");

                string job = (@"<SourceCode>
  <Jobs>
    <Job Name=`Cleanup_Previous_Data` Custodian=`` Email=``>
      <IOSettings>
        <JobType>local</JobType>
      </IOSettings>
      <Add Reference=`System.Data.dll` Type=`system`/>
      <Using>System.Data</Using>
      <Using>System.Data.Common</Using>
      <Local>
        <![CDATA[
            public virtual void Local()
            {
                //Clean up previous data.
               Shell(@`Qizmt del data_Input" + guid + @"*`);
                Shell(@`Qizmt del data_Output" + guid + @"*`);   
                
                System.Data.Common.DbProviderFactory fact = DbProviderFactories.GetFactory(`Qizmt_DataProvider`);

                using (DbConnection conn = fact.CreateConnection())
                {
                    try
                    {
                        conn.ConnectionString = `Data Source = localhost`;
                        conn.Open();
                        DbCommand cmd = conn.CreateCommand();
                        cmd.CommandText = `drop table " + tablename + @"`;
                        cmd.ExecuteNonQuery();  
                        cmd.CommandText = `drop rindex " + indexname + @"`;
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch
                    {

                    }
                }           
            }
        ]]>
      </Local>
    </Job>
    <Job Name=`data_CreateSampleData` Custodian=`` Email=`` Description=`Create sample data`>
      <IOSettings>
        <JobType>remote</JobType>
        <DFS_IO>
          <DFSReader></DFSReader>
          <DFSWriter>dfs://data_Input" + guid + @".txt</DFSWriter>
        </DFS_IO>
      </IOSettings>
      <Remote>
        <![CDATA[
            public virtual void Remote(RemoteInputStream dfsinput, RemoteOutputStream dfsoutput)
            {
                dfsoutput.WriteLine(`1`);
            }
        ]]>
      </Remote>
    </Job>
    <Job Name=`data` Custodian=`` Email=``>
      <IOSettings>
        <JobType>mapreduce</JobType>
        <KeyLength>nInt</KeyLength>
        <DFSInput>dfs://data_Input" + guid + @".txt</DFSInput>
        <DFSOutput>dfs://data_Output" + guid + @"1.bin@nInt,nInt</DFSOutput>
        <OutputMethod>grouped</OutputMethod>
      </IOSettings>
      <Add Reference=`RDBMS_DBCORE.dll` Type=`dfs`/>
      <Using>RDBMS_DBCORE</Using>
      <MapReduce>
        <Map>
          <![CDATA[
            public virtual void Map(ByteSlice line, MapOutput output)
            {                
                DbRecordset rkey = DbRecordset.Prepare();
                rkey.PutInt(4);                
                output.Add(rkey.ToByteSlice(), ByteSlice.Prepare());
            }
        ]]>
        </Map>
        <Reduce>
          <![CDATA[
            public override void Reduce(ByteSlice key, ByteSliceList values, ReduceOutput output)
            {
                {
                    DbRecordset rkey = DbRecordset.Prepare();
                    rkey.PutInt(-15);
                    rkey.PutInt(1);                    
                    output.Add(rkey.ToByteSlice());      
                } 
                {
                    DbRecordset rkey = DbRecordset.Prepare();
                    rkey.PutInt(-15);
                    rkey.PutInt(2);                    
                    output.Add(rkey.ToByteSlice());      
                }
                {
                    DbRecordset rkey = DbRecordset.Prepare();
                    rkey.PutInt(-15);
                    rkey.PutInt(3);                    
                    output.Add(rkey.ToByteSlice());      
                }  
                {
                    DbRecordset rkey = DbRecordset.Prepare();
                    rkey.PutInt(-15);
                    rkey.PutInt(4);                    
                    output.Add(rkey.ToByteSlice());      
                } 
                {
                    DbRecordset rkey = DbRecordset.Prepare();
                    rkey.PutInt(-15);
                    rkey.PutInt(5);                    
                    output.Add(rkey.ToByteSlice());      
                }     
                {
                    DbRecordset rkey = DbRecordset.Prepare();
                    rkey.PutInt(-15);
                    rkey.PutInt(6);                    
                    output.Add(rkey.ToByteSlice());      
                }           
            }
        ]]>
        </Reduce>
      </MapReduce>
    </Job>
    <Job Name=`data` Custodian=`` Email=``>
      <IOSettings>
        <JobType>mapreduce</JobType>
        <KeyLength>nInt</KeyLength>
        <DFSInput>dfs://data_Input" + guid + @".txt</DFSInput>
        <DFSOutput>dfs://data_Output" + guid + @"2.bin@nInt,nInt</DFSOutput>
        <OutputMethod>grouped</OutputMethod>
      </IOSettings>
      <Add Reference=`RDBMS_DBCORE.dll` Type=`dfs`/>
      <Using>RDBMS_DBCORE</Using>
      <MapReduce>
        <Map>
          <![CDATA[
            public virtual void Map(ByteSlice line, MapOutput output)
            {                
                DbRecordset rkey = DbRecordset.Prepare();
                rkey.PutInt(4);                
                output.Add(rkey.ToByteSlice(), ByteSlice.Prepare());
            }
        ]]>
        </Map>
        <Reduce>
          <![CDATA[
            public override void Reduce(ByteSlice key, ByteSliceList values, ReduceOutput output)
            {
               {
                    DbRecordset rkey = DbRecordset.Prepare();
                    rkey.PutInt(-15);
                    rkey.PutInt(7);                    
                    output.Add(rkey.ToByteSlice());      
                }  
               {
                    DbRecordset rkey = DbRecordset.Prepare();
                    rkey.PutInt(-15);
                    rkey.PutInt(8);                    
                    output.Add(rkey.ToByteSlice());      
                }     
            }
        ]]>
        </Reduce>
      </MapReduce>
    </Job>
    <Job Name=`data` Custodian=`` Email=``>
      <IOSettings>
        <JobType>mapreduce</JobType>
        <KeyLength>nInt</KeyLength>
        <DFSInput>dfs://data_Input" + guid + @".txt</DFSInput>
        <DFSOutput>dfs://data_Output" + guid + @"3.bin@nInt,nInt</DFSOutput>
        <OutputMethod>grouped</OutputMethod>
      </IOSettings>
      <Add Reference=`RDBMS_DBCORE.dll` Type=`dfs`/>
      <Using>RDBMS_DBCORE</Using>
      <MapReduce>
        <Map>
          <![CDATA[
            public virtual void Map(ByteSlice line, MapOutput output)
            {                
                DbRecordset rkey = DbRecordset.Prepare();
                rkey.PutInt(4);                
                output.Add(rkey.ToByteSlice(), ByteSlice.Prepare());
            }
        ]]>
        </Map>
        <Reduce>
          <![CDATA[
            public override void Reduce(ByteSlice key, ByteSliceList values, ReduceOutput output)
            {
               {
                    DbRecordset rkey = DbRecordset.Prepare();
                    rkey.PutInt(-15);
                    rkey.PutInt(9);                    
                    output.Add(rkey.ToByteSlice());      
                }      
               {
                    DbRecordset rkey = DbRecordset.Prepare();
                    rkey.PutInt(-15);
                    rkey.PutInt(10);                    
                    output.Add(rkey.ToByteSlice());      
                } 
            }
        ]]>
        </Reduce>
      </MapReduce>
    </Job>
    <Job Name=`data` Custodian=`` Email=``>
      <IOSettings>
        <JobType>mapreduce</JobType>
        <KeyLength>nInt</KeyLength>
        <DFSInput>dfs://data_Input" + guid + @".txt</DFSInput>
        <DFSOutput>dfs://data_Output" + guid + @"4.bin@nInt,nInt</DFSOutput>
        <OutputMethod>grouped</OutputMethod>
      </IOSettings>
      <Add Reference=`RDBMS_DBCORE.dll` Type=`dfs`/>
      <Using>RDBMS_DBCORE</Using>
      <MapReduce>
        <Map>
          <![CDATA[
            public virtual void Map(ByteSlice line, MapOutput output)
            {                
                DbRecordset rkey = DbRecordset.Prepare();
                rkey.PutInt(4);                
                output.Add(rkey.ToByteSlice(), ByteSlice.Prepare());
            }
        ]]>
        </Map>
        <Reduce>
          <![CDATA[
            public override void Reduce(ByteSlice key, ByteSliceList values, ReduceOutput output)
            {
               {
                    DbRecordset rkey = DbRecordset.Prepare();
                    rkey.PutInt(-15);
                    rkey.PutInt(11);                    
                    output.Add(rkey.ToByteSlice());      
                }  
               {
                    DbRecordset rkey = DbRecordset.Prepare();
                    rkey.PutInt(-15);
                    rkey.PutInt(12);                    
                    output.Add(rkey.ToByteSlice());      
                }
               {
                    DbRecordset rkey = DbRecordset.Prepare();
                    rkey.PutInt(-15);
                    rkey.PutInt(13);                    
                    output.Add(rkey.ToByteSlice());      
                }    
            }
        ]]>
        </Reduce>
      </MapReduce>
    </Job>
    <Job Name=`Prepare_tblFriends_Table_Create_RIndex` Custodian=`` Email=``>
      <IOSettings>
        <JobType>local</JobType>
      </IOSettings>
      <Add Reference=`RDBMS_DBCORE.dll` Type=`dfs`/>
      <Add Reference=`System.Data.dll` Type=`system`/>
      <Using>RDBMS_DBCORE</Using>
      <Using>System.Data</Using>
      <Using>System.Data.Common</Using>
      <Local>
        <![CDATA[
            public virtual void Local()
            {
                Shell(@`qizmt combine data_Output" + guid + @"1.bin data_Output" + guid + @"2.bin data_Output" + guid + @"3.bin data_Output" + guid + @"4.bin +data_Output" + guid + @".bin`);
                
                System.Data.Common.DbProviderFactory fact = DbProviderFactories.GetFactory(`Qizmt_DataProvider`);
                
                using (DbConnection conn = fact.CreateConnection())
                {
                    conn.ConnectionString = `Data Source = localhost`;
                    conn.Open();
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = `create table " + tablename + @" (id int, rid int)`;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = `insert into " + tablename + @" bind 'dfs://data_Output" + guid + @".bin'`;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                using (DbConnection conn = fact.CreateConnection())
                {
                    conn.ConnectionString = `Data Source = localhost`;
                    conn.Open();
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = `create rindex " + indexname + @" from " + tablename + @" pinmemoryHASH ON id`;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }         
        }
        ]]>
      </Local>
    </Job>
  </Jobs>
</SourceCode>").Replace('`', '"');
                string tempdir = @"\\" + System.Net.Dns.GetHostName() + @"\" + Environment.CurrentDirectory.Replace(':', '$') + @"\" + Guid.NewGuid().ToString().Replace("-", "");
                if (System.IO.Directory.Exists(tempdir))
                {
                    System.IO.Directory.Delete(tempdir, true);
                }
                System.IO.Directory.CreateDirectory(tempdir);
                string tempjobname = Guid.NewGuid().ToString();
                System.IO.File.WriteAllText(tempdir + @"\" + tempjobname, job);

                Exec.Shell("Qizmt importdir \"" + tempdir + "\"");

                Exec.Shell("dspace exec " + tempjobname);

                //Clean up
                Exec.Shell(@"Qizmt del " + tempjobname);
                System.IO.Directory.Delete(tempdir, true);
            }

            {
                Console.WriteLine("RInsert/RDeleting rindex...");
                DbConnection conn = fact.CreateConnection();
                conn.ConnectionString = "Data Source = " + string.Join(",", allhosts) + "; rindex=pooled";
                conn.Open();
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "rdelete from " + indexname + " where key=-15";
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            {
                Console.WriteLine("RSelecting from rindex...");
                DbConnection conn = fact.CreateConnection();
                conn.ConnectionString = "Data Source = " + string.Join(",", allhosts) + "; rindex=pooled";
                conn.Open();
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "rselect * from " + indexname + " where key=-15";
                DbDataReader reader = cmd.ExecuteReader();
                int cnt = 0;
                List<KeyValuePair<int, int>> results = new List<KeyValuePair<int, int>>();
                bool found = false;
                while (reader.Read())
                {
                    throw new Exception("Not supposed to return any results.");
                }
                reader.Close();
                conn.Close();
            }

            {
                DbConnection conn = fact.CreateConnection();
                conn.ConnectionString = "Data Source = localhost";
                conn.Open();
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "drop table " + tablename;
                cmd.ExecuteNonQuery();
                cmd.CommandText = "drop rindex " + indexname;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
