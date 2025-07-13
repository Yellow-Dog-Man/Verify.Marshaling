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

[StructLayout(LayoutKind.Sequential)]
public struct NestedStruct
{
    public SimpleStruct A;
    public SimpleStruct B;
    public SimpleStruct C;
}

[StructLayout(LayoutKind.Sequential)]
public struct ArrayStruct
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public SimpleStruct[] A;
}

[StructLayout(LayoutKind.Explicit)]
public struct ExplicitStruct
{
    [FieldOffset(26)]
    public bool Z;

    [FieldOffset(0)]
    public bool A;

    [FieldOffset(1)]
    public bool B;

    [FieldOffset(6)]
    public bool F;
}

public struct StringStruct
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
    public string A;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
    public string B;
}

public struct BigStruct
{
    [MarshalAs(UnmanagedType.I4)]
    public int Test;

    [MarshalAs(UnmanagedType.R4)]
    public float TestF;

    [MarshalAs(UnmanagedType.I4)]
    public int TestA;

    [MarshalAs(UnmanagedType.I4)]
    public int TestB;

    // 20 SIMPLE STRUCTS
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
    public SimpleStruct[] cmdSet;
}

#region Bad Structs

/// <summary>
/// Automatic layout struct. NOT SUPPORTED.
/// </summary>
[StructLayout(LayoutKind.Auto)]
public struct AutoStruct { } 

/// <summary>
/// Has no attribute, treated as sequential.
/// </summary>
public struct NoAttributeStruct { } // NO ATTRIBUTE!

/// <summary>
/// Strings without a marshalas, cannot be marshaled correctly.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct BadStringStruct {
    public string A; // No Marshal As
}
#endregion

