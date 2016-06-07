using System.Reflection;
using System.Runtime.CompilerServices;
using Anotar.LibLog;
using OpenMagic.AspNet.WebApi;

[assembly: AssemblyTitle(Constants.Assembly.Title + ".WebApi")]
[assembly: AssemblyDescription("Store, retrieve and update Raygun messages")]
[assembly: AssemblyProduct(Constants.Assembly.Product)]
[assembly: AssemblyCopyright(Constants.Assembly.Copyright)]
[assembly: AssemblyVersion(Constants.Assembly.Version)]
[assembly: AssemblyFileVersion(Constants.Assembly.FileVersion)]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("OpenMagic.ErrorTracker.WebApi.Specifications")]
[assembly: LogMinimalMessage]