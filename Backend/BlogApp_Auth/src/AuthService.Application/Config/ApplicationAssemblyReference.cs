using System.Reflection;

namespace AuthService.Application.Config;

public class ApplicationAssemblyReference
{
    internal static readonly Assembly Assembly = typeof(ApplicationAssemblyReference).Assembly;
}