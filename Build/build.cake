var TARGET = Argument ("target", Argument ("t", "Default"));
var VERSION = EnvironmentVariable ("APPVEYOR_BUILD_VERSION") ?? Argument("build_version", "0.0.9999");
var CONFIG = Argument("configuration", EnvironmentVariable ("CONFIGURATION") ?? "Release");
var SLN = "../Sources/Microcharts.sln";
var NUSPEC = "../NuGet/Package.nuspec";
var NUSPEC_FORMS = "../NuGet/Package.Forms.nuspec";
var NUPKGFOLDER = "./nuget/";
var NUGET_APIKEY = Argument ("nuget_apikey", "");

Task("Build").Does(()=>
{
	NuGetRestore (SLN);
	MSBuild (SLN, c => {
		c.Configuration = CONFIG;
		c.MSBuildPlatform = Cake.Common.Tools.MSBuild.MSBuildPlatform.x86;
	});
});

Task ("Pack")
	.IsDependentOn ("Build")
	.Does (() =>
{
    if(!DirectoryExists(NUPKGFOLDER))
        CreateDirectory(NUPKGFOLDER);
        
	NuGetPack (NUSPEC, new NuGetPackSettings { 
		Version = VERSION,
		OutputDirectory = NUPKGFOLDER,
		BasePath = "./"
	});

	NuGetPack (NUSPEC_FORMS, new NuGetPackSettings { 
		Version = VERSION,
		OutputDirectory = NUPKGFOLDER,
		BasePath = "./"
	});
});

Task ("Push")
	.IsDependentOn ("Pack")
	.Does (() =>
{
	var packages = GetFiles(NUPKGFOLDER + "*."+VERSION+".nupkg");
	NuGetPush(packages, new NuGetPushSettings {
		Source = "https://www.nuget.org/api/v2/package",
		ApiKey = NUGET_APIKEY
	});
});

Task ("Clean").Does(() => 
{
	CleanDirectory ("./Sources/component/tools/");
	CleanDirectories (NUPKGFOLDER);
	CleanDirectories ("./**/bin");
	CleanDirectories ("./**/obj");
});

Task ("Default").IsDependentOn("Pack");

RunTarget (TARGET);