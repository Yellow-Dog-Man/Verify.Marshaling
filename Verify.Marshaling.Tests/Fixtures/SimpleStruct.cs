using System.Runtime.InteropServices;

namespace Verify.Marshaling.Tests.Fixtures;

[StructLayout(LayoutKind.Sequential)]
public struct SimpleStruct
{
    [MarshalAs(UnmanagedType.I4)]
    public int Test;

    [MarshalAs(UnmanagedType.R4)]
    public float TestF;
}
