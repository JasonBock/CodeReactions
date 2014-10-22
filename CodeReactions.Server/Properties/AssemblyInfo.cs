using System.Reflection;
using CodeReactions.Server;
using Microsoft.Owin;

[assembly: AssemblyTitle("CodeReactions.Server")]
[assembly: AssemblyProduct("CodeReactions.Server")]
[assembly: AssemblyCopyright("Copyright ©  2014")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: OwinStartup(typeof(Startup))]
