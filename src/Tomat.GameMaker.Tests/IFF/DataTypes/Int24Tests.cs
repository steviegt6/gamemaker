using Tomat.GameMaker.IFF.DataTypes;

namespace Tomat.GameMaker.Tests.IFF.DataTypes;

[TestFixture]
public static class Int24Tests {
    // TODO: Figure out a clean way to check if these values remain equal in the
    // event of an overflow.
    
    [TestCase(Int24.MIN_VALUE)]
    [TestCase(-1)]
    [TestCase((int)UInt24.MIN_VALUE)]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(Int24.MAX_VALUE)]
    //[TestCase(unchecked((int)UInt24.MAX_VALUE))]
    public static void AssertInt24Int32Conversion(int value) {
        var int24 = (Int24)value;
        Assert.That((int)int24, Is.EqualTo(value));
    }

    //[TestCase(unchecked((uint)Int24.MIN_VALUE))]
    //[TestCase(unchecked((uint)-1))]
    [TestCase(UInt24.MIN_VALUE)]
    [TestCase(0u)]
    [TestCase(1u)]
    [TestCase((uint)Int24.MAX_VALUE)]
    [TestCase(UInt24.MAX_VALUE)]
    public static void AssertUInt24UInt32Conversion(uint value) {
        var uint24 = (UInt24)value;
        Assert.That((uint)uint24, Is.EqualTo(value));
    }
}
