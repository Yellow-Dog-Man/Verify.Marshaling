using System.Runtime.InteropServices;

namespace Verify.Marshaling.Utilities;

internal static class MarshalingExtensions
{
    internal static int? GetSize(this MarshalAsAttribute attribute)
    {
        var type = attribute.Value;
        switch (type)
        {
            case UnmanagedType.LPStr:
                return (GetSize(type) * attribute.SizeConst) + 1;
            case UnmanagedType.LPWStr:
            case UnmanagedType.LPTStr:
            case UnmanagedType.BStr:
                //TODO: how many bytes are these?
                throw new NotImplementedException($"{type} is not supported or unknown.");

            case UnmanagedType.ByValTStr:
                return (GetSize(type) * attribute.SizeConst);
            case UnmanagedType.ByValArray:
                return (GetSize(attribute.ArraySubType) * attribute.SizeConst);
            case UnmanagedType.SafeArray:
                return (GetSize(attribute.ArraySubType) * attribute.SizeConst);

            case UnmanagedType.Struct:
                throw new NotImplementedException($"Nested not supported");
            default:
                return GetSize(attribute.Value);
        }
    }

    internal static int GetSize(this UnmanagedType unmanagedType) { 
        switch (unmanagedType)
        {
            case UnmanagedType.I1:
            case UnmanagedType.U1:
                return 1;

            case UnmanagedType.I2:
            case UnmanagedType.U2:
                return 2;
            case UnmanagedType.I4:
            case UnmanagedType.U4:
            case UnmanagedType.R4:
                return 4;
            case UnmanagedType.I8:
            case UnmanagedType.U8:
            case UnmanagedType.R8:
                return 8;
            case UnmanagedType.Bool:
                return 4; // Marshal marshals bool as 4-byte Win32 BOOL
            case UnmanagedType.Error:
                return 4; // HRESULT
            case UnmanagedType.FunctionPtr:
                return IntPtr.Size;
            case UnmanagedType.SysInt:
                return IntPtr.Size;
            case UnmanagedType.SysUInt:
                return UIntPtr.Size;
            case UnmanagedType.ByValTStr:
                return 1;
        }
        throw new NotImplementedException($"{unmanagedType} is not supported or unknown.");
    }
}

