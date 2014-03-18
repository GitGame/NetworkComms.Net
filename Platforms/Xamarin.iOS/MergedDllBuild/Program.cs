﻿//
//  Copyright 2009-2014 NetworkComms.Net Ltd.
//
//  This source code is made available for reference purposes only.
//  It may not be distributed and it may not be made publicly available.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILMerging;
using System.IO;
using System.Reflection;

namespace MergedDllBuild
{
    class Program
    {
        static void Main(string[] args)
        {
#if !DEBUG
            Version networkCommsVersion =
                new Version(Assembly.ReflectionOnlyLoad("NetworkCommsDotNet").FullName.
                    Split(',').
                    Where(s => s.Split('=').Length == 2).
                    ToDictionary(s => s.Split('=')[0].Trim(), s => s.Split('=')[1].Trim())["Version"]);

            string targetPlatform = "v4";
            string msCoreLibDirectory = @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\MonoTouch\v1.0";

            bool coreBuildEnabled = false;
            bool completeBuildEnabled = true;

            #region Merge Core
            if (coreBuildEnabled)
            {
                Directory.CreateDirectory("MergedCore");
                File.Delete(@"MergedCore\MergeLog.txt");
                ILMerge coreMerge = new ILMerge();

                List<string> coreAssembles = new List<string>();
                coreAssembles.Add("NetworkCommsDotNet.dll");
                coreAssembles.Add("protobuf-net.dll");
                coreAssembles.Add("ProtobufSerializer.dll");

                coreMerge.SetInputAssemblies(coreAssembles.ToArray());
                coreMerge.Version = networkCommsVersion;

                coreMerge.TargetKind = ILMerge.Kind.Dll;
                coreMerge.SetTargetPlatform(targetPlatform, msCoreLibDirectory);
                coreMerge.XmlDocumentation = true;

                coreMerge.KeyFile = "networkcomms.net.snk";

                coreMerge.OutputFile = @"MergedCore\NetworkCommsDotNetCore.dll";

                coreMerge.Log = true;
                coreMerge.LogFile = @"MergedCore\MergeLog.txt";

                coreMerge.Merge();
            }
            #endregion

            #region Merge Complete
            if (completeBuildEnabled)
            {
                Directory.CreateDirectory("MergedComplete");
                File.Delete(@"MergedComplete\MergeLog.txt");
                ILMerge completeMerge = new ILMerge();

                List<string> completeAssembles = new List<string>();
                completeAssembles.Add("NetworkCommsDotNet.dll");
                completeAssembles.Add("protobuf-net.dll");
                completeAssembles.Add("ProtobufSerializer.dll");

                completeMerge.SetInputAssemblies(completeAssembles.ToArray());
                completeMerge.Version = networkCommsVersion;

                completeMerge.TargetKind = ILMerge.Kind.Dll;
                completeMerge.SetTargetPlatform(targetPlatform, msCoreLibDirectory);
                completeMerge.XmlDocumentation = true;

                completeMerge.KeyFile = "networkcomms.net.snk";

                completeMerge.OutputFile = @"MergedComplete\NetworkCommsDotNetComplete.dll";

                completeMerge.Log = true;
                completeMerge.LogFile = @"MergedComplete\MergeLog.txt";

                completeMerge.Merge();
            }
            #endregion
#endif
        }
    }
}
