using System.Reflection;
using System.Runtime.InteropServices;
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

        VerifierSettings.RegisterFileConverter<Type>(
            MarshalingConverter.Convert, 
            MarshalingConverter.CanConvert);
    }
    
}

public static class MarshalingConverter
{
    public static ConversionResult Convert(Type target, IReadOnlyDictionary<String, Object> context)
    {
        return new ConversionResult(null, "txt", System.Text.Json.JsonSerializer.Serialize(MarshalRecord.From(target)));
    }

    public static bool CanConvert(Type target, IReadOnlyDictionary<String, Object> context)
    {
        return (target.GetCustomAttribute<StructLayoutAttribute>() != null);
    }
}
