using System.Runtime.CompilerServices;
using Verify.Marshaling.Utilities;

namespace VerifyTests;

public static class VerifyMarshaling
{
    /// <summary>
    /// Gets if the plugin has been initialized or not.
    /// </summary>
    public static bool Initialized { get; private set; }

    public static void Initialize()
    {
        if (Initialized)
        {
            throw new InvalidOperationException($"{nameof(VerifyMarshaling)} Already Initialized");
        }

        Initialized = true;

        InnerVerifier.ThrowIfVerifyHasBeenRun();

        //TODO: we don't actually need to do anything here, but this keeps it in the VerifyTests namespace. See other TODO's in this file.
    }

    /// <summary>
    /// Given a C# type, validate its struct memory layout using Verify.
    /// </summary>
    /// <param name="t">Target type</param>
    /// <param name="settings">VerifySettings to apply if needed</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    //TODO: Validate this approach. This approach is a little hacky, but its how we "got things working" PRs to improve it are welcome.
    //TODO: Should we restrict the Typing of t? To just classes and structs?
    public static SettingsTask VerifyMemoryLayout(Type t, VerifySettings? settings = null, [CallerFilePath] string sourceFile = "", [CallerMemberName] string memberName = "")
    {
        // Somewhat hacky, Verify does this through Extensive orchestration
        // in the test Framework specific libraries. We just need to get the filename.
        var typeName = Path.GetFileNameWithoutExtension(sourceFile); 

        // This isn't used, it just needs to be not null.
        var pathInfo = new PathInfo();

        if (t.FullName == null)
            throw new InvalidOperationException("Type must have a full name");

        return new SettingsTask(settings, async settings => {
            settings.UseParameters(new[] { t });
            var v = new InnerVerifier(sourceFile,settings, typeName, memberName, new List<string>(){ "t" }, pathInfo);
            return await v.Verify(MarshalRecord.From(t));
        });
    }
}
