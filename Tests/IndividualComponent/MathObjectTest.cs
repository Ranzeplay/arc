using Arc.Compiler.Shared.CommandGeneration;
using Arc.Compiler.CommandGenerator.Extensions;

namespace Arc.Compiler.Tests.IndividualComponent
{
    [CancelAfter(1000)]
    [Category("IndividualComponent")]
    public class MathObjectTest
    {
        [Test]
        public void NumberToString()
        {
            var number = new NumberObject("233.3");
            Assert.That(number.ToString(true), Is.EqualTo("+233.3"));
        }

        [Test]
        public void NumberToByteArray()
        {
            var number = new NumberObject("23.33");
            var metadata = new PackageMetadata(0, 2, 2, 2, 0, 2);

            var array = number.ToPackageEncoding(metadata);

            Assert.That(array, Is.EqualTo(new byte[] { 0x01, 0x01, 0x00, 0x17, 0x01, 0x00, 0x21 }));
        }
    }
}
