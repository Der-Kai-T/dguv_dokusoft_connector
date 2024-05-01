using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Publish);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    
    [Solution]
    readonly Solution Solution;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath OutputDirectory => RootDirectory / "pub";

    Target ClearBinAndObj => _ => _.Before(Clean).Executes(() =>
    {
        foreach (var directory in SourceDirectory.GlobDirectories("**/bin", "**/obj"))
        {
            directory.DeleteDirectory();
        }
    });
    
    Target Clean => _ => _
        .Before(Restore)
        .DependsOn(ClearBinAndObj)
        .Executes(() => OutputDirectory.CreateOrCleanDirectory());

    Target Restore => _ => _
        .Executes(() => DotNetTasks.DotNetRestore(settings => settings.SetProjectFile(Solution)));

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() => DotNetTasks.DotNetBuild(settings =>
            settings.SetProjectFile(Solution).SetConfiguration(Configuration).EnableNoRestore()));

    Target Publish => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTasks.DotNetPublish(settings =>
                settings.SetOutput(OutputDirectory).SetRuntime("win-x64").SetConfiguration(Configuration.Release)
                    .SetSelfContained(false).SetPublishTrimmed(false));
        });

}
