using System.Runtime.InteropServices;
using Verify.Marshaling.Tests.Fixtures;
using Verify.Marshaling.Utilities;

namespace Verify.Marshaling.Tests;

// See: https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.unmanagedtype?view=net-9.0 for info

[TestClass]
public sealed class TestMarshalingSizes
{
    [DataRow(UnmanagedType.I1, 1)]
    [DataRow(UnmanagedType.U1, 1)]

    [DataRow(UnmanagedType.I2, 2)]
    [DataRow(UnmanagedType.U2, 2)]

    [DataRow(UnmanagedType.I4, 4)]
    [DataRow(UnmanagedType.U4, 4)]
    [DataRow(UnmanagedType.R4, 4)]

    [DataRow(UnmanagedType.I8, 8)]
    [DataRow(UnmanagedType.U8, 8)]
    [DataRow(UnmanagedType.R8, 8)]
    [DataRow(UnmanagedType.ByValTStr, 1)]
    [DataRow(UnmanagedType.SysInt, 8)]
    [DataRow(UnmanagedType.SysUInt, 8)]
    [DataRow(UnmanagedType.Error, 4)]
    [TestMethod]
    public void TestUnmanagedTypeSizes(UnmanagedType type, int expectedSize)
    {
        Assert.AreEqual(expectedSize, type.GetSize());
    }

    //TODO: Move these, Up one method when they are implemented
    [DataRow(UnmanagedType.LPStr, 1)]
    [DataRow(UnmanagedType.LPTStr, 1)]
    [DataRow(UnmanagedType.LPUTF8Str, 1)]
    [DataRow(UnmanagedType.LPWStr, 1)]
    [DataRow(UnmanagedType.IInspectable, 1)]
    [DataRow(UnmanagedType.IUnknown, 1)]
    [DataRow(UnmanagedType.HString, 1)]
    [DataRow(UnmanagedType.CustomMarshaler, 1)]
    [TestMethod]
    public void TestUnmanagedTypeSizesNotImplemented(UnmanagedType type, int expectedSize)
    {
        Assert.Throws<NotImplementedException>(() => type.GetSize());
    }

    [TestMethod]
    public void TestComplexUnmanagedTypes()
    {
        // [DataRow] needs constants, IntPtr is not a constant
        Assert.AreEqual(IntPtr.Size, UnmanagedType.FunctionPtr.GetSize());
        Assert.AreEqual(IntPtr.Size, UnmanagedType.SysInt.GetSize());
        Assert.AreEqual(UIntPtr.Size, UnmanagedType.SysUInt.GetSize());
    }

    [DataRow(5, UnmanagedType.ByValTStr, 5)]
    [DataRow(1, UnmanagedType.ByValTStr, 1)]
    [TestMethod]
    public void TestVariableLengthStrings(int expectedSize, UnmanagedType type, int size)
    {
        var m = new MarshalAsAttribute(type);
        m.SizeConst = size;

        Assert.AreEqual(expectedSize, m.GetSize());
    }

    [DataRow(5, UnmanagedType.ByValArray, UnmanagedType.U1, 5)]
    [DataRow(20, UnmanagedType.ByValArray, UnmanagedType.R4, 5)]
    [DataRow(20, UnmanagedType.LPArray, UnmanagedType.R4, 5)]
    [TestMethod]
    public void TestUnmanagedArrayLengths(int expectedSize, UnmanagedType type, UnmanagedType arraySubType, int size)
    {
        var m = new MarshalAsAttribute(type);
        m.SizeConst = size;
        m.ArraySubType = arraySubType;

        Assert.AreEqual(expectedSize, m.GetSize());
    }

    //TODO: Move these, Up one method when they are implemented
    [DataRow(20, UnmanagedType.SafeArray, UnmanagedType.R4, 5)]
    [DataRow(20, UnmanagedType.LPWStr, UnmanagedType.R4, 5)]
    [DataRow(20, UnmanagedType.BStr, UnmanagedType.R4, 5)]
    [TestMethod]
    public void TestNotImplementedArrayTypes(int expectedSize, UnmanagedType type, UnmanagedType arraySubType, int size)
    {
        Assert.Throws<NotImplementedException>(() => TestUnmanagedArrayLengths(expectedSize, type, arraySubType, size));
    }

    [TestMethod]
    public void TestSpecificFields()
    {
        var fields = typeof(CMP_CompressOptions)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Where(f => f.Name == "cmdSet");
        foreach (var f in fields)
        {
            Assert.AreEqual(960, MarshalRecord.GetSize(f));
        }
    }    
}
