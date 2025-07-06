using System.Runtime.InteropServices;
using Verify.Marshaling.Tests.Fixtures;
using Verify.Marshaling.Utilities;

namespace Verify.Marshaling.Tests;

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
    //[DataRow(UnmanagedType.LPStr, 1)]
    //[DataRow(UnmanagedType.LPTStr, 1)]
    [DataRow(UnmanagedType.ByValTStr, 1)]
    [DataTestMethod]
    public void TestUnmanagedTypeSizes(UnmanagedType type, int expectedSize)
    {
        Assert.AreEqual(expectedSize, type.GetSize());
    }

    // [DataRow] needs constants, IntPtr is not a constant
    [TestMethod]
    public void TestComplexUnmanagedTypes()
    {
        Assert.AreEqual(IntPtr.Size, UnmanagedType.FunctionPtr.GetSize());
        Assert.AreEqual(IntPtr.Size, UnmanagedType.SysInt.GetSize());
        Assert.AreEqual(UIntPtr.Size, UnmanagedType.SysUInt.GetSize());
    }

    //case UnmanagedType.ByValTStr:
    //        return (GetSize(type)* attribute.SizeConst);
    //    case UnmanagedType.ByValArray:
    //        return (GetSize(attribute.ArraySubType)* attribute.SizeConst);
    //    case UnmanagedType.SafeArray:
    //        return (GetSize(attribute.ArraySubType)* attribute.SizeConst);

    [DataRow(5, UnmanagedType.ByValTStr, 5)]
    [DataRow(1, UnmanagedType.ByValTStr, 1)]
    [DataTestMethod]
    public void TestVariableLengthSizes(int expectedSize, UnmanagedType type, int size)
    {
        var m = new MarshalAsAttribute(type);
        m.SizeConst = size;

        Assert.AreEqual(expectedSize, m.GetSize());
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
