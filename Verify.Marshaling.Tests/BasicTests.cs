

using Verify.Marshaling.Tests.Fixtures;
namespace Verify.Marshaling.Tests;

[UsesVerify]
[TestClass]
public partial class BasicTests: VerifyBase
{
    [TestMethod]
    public async Task BasicTest()
    {
        await Verify(typeof(SimpleStruct));
    }
}
