using System.Runtime.CompilerServices;

namespace Verify.Marshaling.Tests;

public static class Initializer
{
    [ModuleInitializer]
    public static void Initialize() =>
        VerifierSettings.InitializePlugins();
}
