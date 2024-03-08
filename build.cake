#addin nuget:?package=Cake.Docker&version=1.3.0
#addin nuget:?package=Cake.Http&version=4.0.0

var target = Argument("target", "Verify");
var configuration = Argument("configuration", "Release");
var rootDirectory = System.IO.Directory.GetCurrentDirectory();
var solutionPath = System.IO.Path.Join(rootDirectory, "Officify.sln");

var applicationUrls = new []
{
    "http://localhost:5001",
};

Task("Build")
    .Does(() => 
    {
        DotNetBuild(solutionPath);
    });

Task("UnitTests")
    .DoesForEach(GetFiles("tests/**/*.Tests.csproj"), project => {
        DotNetTest(project.FullPath);
    });

Task("StartApp")
    .Does(() => 
    {
        StartSignalREmulator();
        
        var composeSettings = new DockerComposeUpSettings
        {
            Build = true,
            Detach = true
        };
        DockerComposeUp(composeSettings);
    });

Task("WaitForApp")
    .IsDependentOn("StartApp")
    .DoesForEach(applicationUrls, (url) => {
        string response = null;
        do {
            var settings = new HttpSettings()
                .EnsureSuccessStatusCode(false)
                .SetNoCache();
                
            Information("Checking if application at {0} is ready...", url);
            response = HttpGet(url, settings);
            Verbose("Application at {0} returned {1}", url, response);
        } while(string.IsNullOrEmpty(response));
    });

Task("StopApp")
    .Does(() => 
    {
        Teardown();
    });

Task("AcceptanceTests")
    .IsDependentOn("WaitForApp")
    .DoesForEach(GetFiles("tests/**/*.Features.csproj"), (project) =>
    {
        DotNetTest(project.FullPath);
    })
    .Finally(() =>
    {
        Teardown();
    });
    
Task("Verify")
    .IsDependentOn("Build")
    .IsDependentOn("UnitTests")
    .IsDependentOn("AcceptanceTests");

RunTarget(target);

public void Teardown() 
{
    DockerComposeDown();
    StopSignalREmulator(signalRProcess);
}

public void StartSignalREmulator() 
{
    var processSettings = new ProcessSettings()
                .WithArguments(args =>
                    args.AppendQuoted("asrs-emulator")
                        .AppendQuoted("start")
                );
                
    Information("Starting SignalR Emulator...");
    StartAndReturnProcess("dotnet", processSettings); 
}

public void StopSignalREmulator() 
{
    var settings = new ProcessSettings()
        .WithArguments(args => args.AppendSwitch("i", "8888"));
        
    using (var process = StartAndReturnProcess("lsof", 
    if (process != null)
    {
        Information("Killing SignalR Emulator");
        process.Kill();
        Information("Killed SignalR Emulator exit code {0}", process.GetExitCode());
    }
}