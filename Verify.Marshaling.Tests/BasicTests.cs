

using Verify.Marshaling.Tests.Fixtures;
using Verify.Marshaling.Utilities;
namespace Verify.Marshaling.Tests;

[TestClass]
public partial class BasicTests: VerifyBase
{
    [DataRow(typeof(CMP_CompressOptions))]
    [DataRow(typeof(SimpleStruct))]
    [TestMethod]
    public async Task MarshalTypesTest(Type t)
    {
        // TODO: I want to use Verify() with this, without explicit conversions.
        // A converter is registered in VerifyMarshaling.cs, but it is blocked by:
        // https://github.com/VerifyTests/Verify/issues/1475
        // If we can fix this up then the following would be possible:
        // await Verify(t);

        // Until then, we do the conversion manually
        await Verify(MarshalRecord.From(t)).UseTextForParameters(t.Name);
    }
}

