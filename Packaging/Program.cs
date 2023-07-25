using Cake.Common;
using Cake.Common.IO;
using Cake.Core;
using Cake.Frosting;
using System.IO.Compression;
using System.IO;
using System;
using System.Collections.Generic;

public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .Run(args);
    }
}

public class BuildContext : FrostingContext
{
    public string MsBuildConfiguration { get; set; }

    public BuildContext(ICakeContext context)
        : base(context)
    {
        MsBuildConfiguration = context.Argument("configuration", "Release");
    }
}

[TaskName("Clean")]
public sealed class CleanTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        Console.WriteLine("Cleaning bin directories...");
        if (Directory.Exists("../out"))
        {
            context.CleanDirectory($"../out");
        }
        else
        {
            Directory.CreateDirectory("../out");
        }
        context.CleanDirectory("./temp");
    }
}

[TaskName("Build")]
[IsDependentOn(typeof(CleanTask))]
public sealed class BuildTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
    }
}

[TaskName("Package")]
[IsDependentOn(typeof(BuildTask))]
public sealed class PackageTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        Console.WriteLine("Packaging files...");
        Directory.CreateDirectory("./temp");
        // Get Mupen plugins
        Directory.CreateDirectory("./temp/plugins");
        List<string> plugins = new List<string> {
            "mupen64plus-rsp-hle_x64_Release.zip",
            "mupen64plus-input-sdl_x64_Release.zip",
            "mupen64plus-audio-sdl_x64_Release.zip",
            "GLideN64_x64_Release_mupenplus.zip"
        };
        foreach(string plugin in plugins)
        {
            ZipFile.ExtractToDirectory($"../bin/{(plugin)}", "./temp/plugins");
        }
        // Get Mupen core files
        List<string> core = new List<string>
        {
            "mupen64plus-core_x64_Release.zip",
            "Modloader64_x64_Release.zip"
        };
        foreach(string c in core)
        {
            ZipFile.ExtractToDirectory($"../bin/{(c)}", "./temp");
        }
        // Dependencies
        foreach(var file in Directory.EnumerateFiles("../mupen64plus-win32-deps", "*.dll", SearchOption.AllDirectories))
        {
            if (file.Contains("x64"))
            {
                File.Copy(file, $"./temp/{Path.GetFileName(file)}");
            }
        }
        // Cleanup
        foreach(var f in Directory.EnumerateFiles("./temp"))
        {
            if (Path.GetExtension(f) == ".pdb" || Path.GetExtension(f) == ".json" || Path.GetExtension(f) == ".xml" || Path.GetExtension(f) == ".exe")
            {
                File.Delete(f);
            }
        }
        List<string> DeleteMe = new List<string> {
            "ImGui.NET.API.dll",
            "ModLoader.API.dll"
        };
        foreach(var d in DeleteMe)
        {
            File.Delete($"./temp/{d}");
        }
        ZipFile.CreateFromDirectory("./temp", "../out/ModLoader64.zip");
        context.CleanDirectory("./temp");
    }
}

[TaskName("Default")]
[IsDependentOn(typeof(PackageTask))]
public class DefaultTask : FrostingTask
{
}