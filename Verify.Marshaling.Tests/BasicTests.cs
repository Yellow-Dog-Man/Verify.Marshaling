

using Verify.Marshaling.Tests.Fixtures;
namespace Verify.Marshaling.Tests;

[UsesVerify]
[TestClass]
public partial class BasicTests
{
    [DataRow(typeof(CMP_CompressOptions))]
    [DataRow(typeof(SimpleStruct))]
    [DataRow(typeof(NestedStruct))]
    [DataRow(typeof(StringStruct))]
    [DataRow(typeof(ArrayStruct))]
    [DataRow(typeof(NoAttributeStruct))]
    [DataRow(typeof(BigStruct))]
    [TestMethod]
    public async Task TestMarshalableTypes(Type t)
    {
        await VerifyMarshaling.VerifyMemoryLayout(t);
    }

    [TestMethod]
    public void TestBadStructs()
    {
        Assert.ThrowsAsync<InvalidOperationException>(async () => await VerifyMarshaling.VerifyMemoryLayout(typeof(BadStringStruct)));
        Assert.ThrowsAsync<InvalidOperationException>(async () => await VerifyMarshaling.VerifyMemoryLayout(typeof(AutoStruct)));
    }
}

