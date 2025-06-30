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
        //TODO: Should this be placed in the metadata, the first parameter?
        // See: https://github.com/VerifyTests/Verify/blob/main/docs/converter.md that places meta data in the first parameter
        return new ConversionResult(null, "txt", System.Text.Json.JsonSerializer.Serialize(MarshalRecord.From(target)));
    }

    public static bool CanConvert(Type target, IReadOnlyDictionary<String, Object> context)
    {
        return (target.GetCustomAttribute<StructLayoutAttribute>() != null);
    }
}
