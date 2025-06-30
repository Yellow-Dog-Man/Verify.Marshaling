

using System.Runtime.InteropServices;

namespace Verify.Marshaling.Tests.Fixtures;

// Copied from: https://github.com/Yellow-Dog-Man/Compressonator.NET/blob/main/Compressonator.NET/Structs/CMP_CompressOptions.cs
// Abridged in some areas for brevity. We're only testing its structure not its definition.
// This struct, and issues with it, caused this library to be written

public enum CMP_FORMAT: uint
{
    Unknown = 0x0000,  // Undefined texture format.
    RGBA_8888 = 0x0050,
    //.... Rest of enum snipped.
}

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct AMD_CMD
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string strCommand;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    public string strParameter;
}

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void CMP_PrintInfoStr([MarshalAs(UnmanagedType.LPStr)] string infoStr);

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class CMP_CompressOptions
{
    [MarshalAs(UnmanagedType.U4)]
    public uint size = (uint)Marshal.SizeOf<CMP_CompressOptions>();

    // New to v4.5
    // Flags to control parameters in Brotli-G compression preconditioning
    [MarshalAs(UnmanagedType.U1)]
    public bool doPreconditionBRLG;
    [MarshalAs(UnmanagedType.U1)]
    public bool doDeltaEncodeBRLG;
    [MarshalAs(UnmanagedType.U1)]
    public bool doSwizzleBRLG;

    // New to v4.3

    [MarshalAs(UnmanagedType.U4)]
    public uint pageSize;

    // New to v4.2

    [MarshalAs(UnmanagedType.U1)]
    public bool useRefinementSteps;
    [MarshalAs(UnmanagedType.I4)]
    public int refinementSteps;

    // v4.1 and older settings

    [MarshalAs(UnmanagedType.U1)]
    public bool useChannelWeighting;
    [MarshalAs(UnmanagedType.R4)]
    public float weightingRed;
    [MarshalAs(UnmanagedType.R4)]
    public float weightingGreen;
    [MarshalAs(UnmanagedType.R4)]
    public float weightingBlue;
    [MarshalAs(UnmanagedType.U1)]
    public bool useAdaptiveWeighting;

    [MarshalAs(UnmanagedType.U1)]
    public bool DXT1UseAlpha;
    [MarshalAs(UnmanagedType.U1)]
    public bool useGPUDecompress;
    [MarshalAs(UnmanagedType.U1)]
    public bool useCGCompress;

    [MarshalAs(UnmanagedType.U1)]
    public byte alphaThreshold;

    [MarshalAs(UnmanagedType.U1)]
    public bool disableMultiThreading = false;

    [MarshalAs(UnmanagedType.U4)]
    public int compressionSpeed;
    [MarshalAs(UnmanagedType.U4)]
    public int GPUDecode;
    [MarshalAs(UnmanagedType.U4)]
    public int encodeWidth = 4;
    [MarshalAs(UnmanagedType.U4)]
    public uint numThreads = 0;

    [MarshalAs(UnmanagedType.R4)]
    public float quality = 1.0f;

    [MarshalAs(UnmanagedType.U1)]
    public bool restrictColour;
    [MarshalAs(UnmanagedType.U1)]
    public bool restrictAlpha;

    [MarshalAs(UnmanagedType.U4)]
    public uint modeMask;

    [MarshalAs(UnmanagedType.I4)]
    public int numCmds = 0; // Should always be 0, unless actually being used.
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
    public AMD_CMD[] cmdSet;

    [MarshalAs(UnmanagedType.R4)]
    public float inputDefog;
    [MarshalAs(UnmanagedType.R4)]
    public float inputExposure;
    [MarshalAs(UnmanagedType.R4)]
    public float inputKneeLow;
    [MarshalAs(UnmanagedType.R4)]
    public float inputKneeHigh;
    [MarshalAs(UnmanagedType.R4)]
    public float inputGamma;
    [MarshalAs(UnmanagedType.R4)]
    public float inputFilterGamma;

    [MarshalAs(UnmanagedType.I4)]
    public int cmpLevel;
    [MarshalAs(UnmanagedType.I4)]
    public int posBits;
    [MarshalAs(UnmanagedType.I4)]
    public int texCbits;
    [MarshalAs(UnmanagedType.I4)]
    public int normalBits;
    [MarshalAs(UnmanagedType.I4)]
    public int genericBits;

    [MarshalAs(UnmanagedType.I4)]
    public int vCacheSize;
    [MarshalAs(UnmanagedType.I4)]
    public int vCacheFIFOsize;
    [MarshalAs(UnmanagedType.R4)]
    public float overdrawACMR;
    [MarshalAs(UnmanagedType.I4)]
    public int simplifyLOD;
    [MarshalAs(UnmanagedType.U1)]
    public bool vertexFetch;

    [MarshalAs(UnmanagedType.U4)]
    public CMP_FORMAT sourceFormat;
    [MarshalAs(UnmanagedType.U4)]
    public CMP_FORMAT destFormat;
    [MarshalAs(UnmanagedType.U1)]
    public bool format_support_hostEncoder;
}