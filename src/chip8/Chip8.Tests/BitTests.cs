using Chip8.Core;

namespace Chip8.Tests
{
    public class BitTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateBitIntTest()
        {
            var bit = new Bit(1);
            Assert.That(bit.Value, Is.EqualTo(1));
        }

        [Test]
        public void CreateBitBoolTest()
        {
            var bit = new Bit(true);
            Assert.That(bit.Value, Is.EqualTo(1));
        }

        [Test]
        public void BitAnd1Test()
        {
            Bit bit1 = new(1);
            Bit bit2 = new(0);
            Bit bit3 = bit1 & bit2;
            Assert.That(bit3.Value, Is.EqualTo(0));
        }

        [Test]
        public void BitAnd2Test()
        {
            Bit bit1 = new(1);
            Bit bit2 = new(1);
            Bit bit3 = bit1 & bit2;
            Assert.That(bit3.Value, Is.EqualTo(1));
        }

        [Test]
        public void BitAnd3Test()
        {
            Bit bit1 = new(0);
            Bit bit2 = new(0);
            Bit bit3 = bit1 & bit2;
            Assert.That(bit3.Value, Is.EqualTo(0));
        }

        [Test]
        public void BitOr1Test()
        {
            Bit bit1 = new(0);
            Bit bit2 = new(0);
            Bit bit3 = bit1 | bit2;
            Assert.That(bit3.Value, Is.EqualTo(0));
        }

        [Test]
        public void BitOr2Test()
        {
            Bit bit1 = new(1);
            Bit bit2 = new(0);
            Bit bit3 = bit1 | bit2;
            Assert.That(bit3.Value, Is.EqualTo(1));
        }

        [Test]
        public void BitOr3Test()
        {
            Bit bit1 = new(0);
            Bit bit2 = new(1);
            Bit bit3 = bit1 | bit2;
            Assert.That(bit3.Value, Is.EqualTo(1));
        }

        [Test]
        public void BitOr4Test()
        {
            Bit bit1 = new(1);
            Bit bit2 = new(1);
            Bit bit3 = bit1 | bit2;
            Assert.That(bit3.Value, Is.EqualTo(1));
        }

        [Test]
        public void Bitwise1Test()
        {
            Bit bit1 = new(1);
            Bit bit2 = ~bit1;
            Assert.That(bit2.Value, Is.EqualTo(0));
        }

        [Test]
        public void Bitwise2Test()
        {
            Bit bit1 = new(0);
            Bit bit2 = ~bit1;
            Assert.That(bit2.Value, Is.EqualTo(1));
        }

        [Test]
        public void BoolValue1Test()
        {
            Bit bit1 = new(0);
            Assert.That(bit1.BoolValue, Is.EqualTo(false));
        }

        [Test]
        public void BoolValue2Test()
        {
            Bit bit1 = new(1);
            Assert.That(bit1.BoolValue, Is.EqualTo(true));
        }

        [Test]
        public void GetIntValue1Test()
        {
            Bit bit1 = 1;
            Assert.That(bit1.BoolValue, Is.EqualTo(true));
        }

        [Test]
        public void GetIntValue2Test()
        {
            Bit bit1 = 0;
            Assert.That(bit1.BoolValue, Is.EqualTo(false));
        }

        [Test]
        public void ConvertInt1Test()
        {
            Bit bit1 = new(true);
            int val = bit1;
            Assert.That(val, Is.EqualTo(1));
        }

        [Test]
        public void ConvertInt2Test()
        {
            Bit bit1 = new(false);
            int val = bit1;
            Assert.That(val, Is.EqualTo(0));
        }

        [Test]
        public void Xor1Test()
        {
            Bit b1 = 1;
            Bit b2 = 1;
            Assert.That((b1 ^ b2).Value, Is.EqualTo(0));
        }

        [Test]
        public void Xor2Test()
        {
            Bit b1 = 1;
            Bit b2 = 0;
            Assert.That((b1 ^ b2).Value, Is.EqualTo(1));
        }

        [Test]
        public void Xor3Test()
        {
            Bit b1 = 0;
            Bit b2 = 1;
            Assert.That((b1 ^ b2).Value, Is.EqualTo(1));
        }

        [Test]
        public void Xor4Test()
        {
            Bit b1 = 0;
            Bit b2 = 0;
            Assert.That((b1 ^ b2).Value, Is.EqualTo(0));
        }

        [Test]
        public void EqualsTest()
        {
            Bit b1 = 1;
            Bit b2 = true;
            Assert.That(b1 == b2, Is.True);
        }

        [Test]
        public void NotEqualsTest()
        {
            Bit b1 = 1;
            Bit b2 = false;
            Assert.That(b1 != b2, Is.True);
        }
    }
}