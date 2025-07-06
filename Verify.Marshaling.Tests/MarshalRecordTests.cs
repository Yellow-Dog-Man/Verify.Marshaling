using Verify.Marshaling.Tests.Fixtures;
using Verify.Marshaling.Utilities;

namespace Verify.Marshaling.Tests;

[TestClass]
public sealed class MarshalRecordTests
{
    [DataRow(true, typeof(NestedStruct), nameof(NestedStruct.A))]
    [DataRow(true, typeof(CMP_CompressOptions), nameof(CMP_CompressOptions.deviceInfo))]
    [DataRow(true, typeof(CMP_CompressOptions), nameof(CMP_CompressOptions.perfStats))]

    [DataRow(false, typeof(CMP_CompressOptions), nameof(CMP_CompressOptions.size))]
    [DataRow(false, typeof(CMP_CompressOptions), nameof(CMP_CompressOptions.cmdSet))]
    [DataRow(false, typeof(CMP_CompressOptions), nameof(CMP_CompressOptions.sourceFormat))]
    [DataRow(false, typeof(SimpleStruct), nameof(SimpleStruct.Test))]
    [DataRow(false, typeof(StringStruct), nameof(StringStruct.A))]
    [DataRow(false, typeof(StringStruct), nameof(StringStruct.B))]
    [DataRow(false, typeof(ArrayStruct), nameof(ArrayStruct.A))]
    [TestMethod]
    public void TestShouldNest(bool shouldNest, Type t, string fieldName)
    {
        Assert.IsFalse(string.IsNullOrEmpty(fieldName));

        var field = t.GetField(fieldName);

        Assert.IsNotNull(field);

        Assert.AreEqual(shouldNest, MarshalRecord.ShouldNest(field));         
    }
}
