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

        //TODO: we don't actually need to do anything here, but this keeps it in the VerifyTests namespace.
    }
    public static SettingsTask VerifyMemoryLayout(Type t, VerifySettings? settings = null, [CallerFilePath] string sourceFile = "", [CallerMemberName] string memberName = "")
    {
        // TODO: This feels like a massive hack, but technicallly works quite well. I ran out of energy!

        VerifierSettings.AssignTargetAssembly(t.Assembly);

        // Somewhat hacky, Verify does this through Extensive orchestration
        // in the test Framework specific libraries. We Just get the filename :S
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
