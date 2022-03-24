using Elms.Utils;
using Elms.Utils.ValueExtensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace test_fnv1a;

[TestClass]
public partial class TestFnv1a
{
  [TestMethod]
  public void SyntaxRefSyntaxExample()
  {
    // arrange

    // act
    var result = Fnv1a.Create().Hash((byte)0);

    // assert
    result.Should().Be(unchecked((int)0x811c9dc5));
  }

  [TestMethod]
  public void TestEmpty()
  {
    // arrange

    // act
    var result = Fnv1a.Create().GetHashCode();

    // assert
    result.Should().Be(unchecked((int)0x811c9dc5));
  }

  [DataTestMethod]
  [DynamicData(nameof(TestVectors), DynamicDataSourceType.Method)]
  public void TestAsciiVectors(string input, uint expectedHash)
  {
    // arrange

    // act
    var result = Fnv1a.Create().HashUtf8(input).GetHashCode();

    // assert
    result.Should().Be(unchecked((int)expectedHash));
  }

  /// <summary>
  /// 
  /// </summary>
  [DataTestMethod]
  [DynamicData(nameof(Utf8CornerCases), DynamicDataSourceType.Method)]
  public void TestUtf8InternalEncoding(string input)
  {
    // arrange

    // act
    var expectedBytes = Encoding.UTF8.GetBytes(input);
    var expected = Fnv1a.Create().Hash(expectedBytes).GetHashCode();
    var actual = Fnv1a.Create().HashUtf8(input).GetHashCode();

    // assert
    actual.Should().Be(expected);
  }


  [TestMethod]
  public void TestPrimitivesCoverage()
  {
    // TODO: why can't method chain on temporaries with ref returns?

    // byte 'a'
    Fnv1a.Create().Hash(unchecked((byte)0x61u)).GetHashCode().Should().Be(unchecked((int)0xe40c292cu));
    Fnv1a.Create().Hash(unchecked((sbyte)0x61u)).GetHashCode().Should().Be(unchecked((int)0xe40c292cu));

    // short 'fo'
    Fnv1a.Create().Hash(unchecked((ushort)0x666fu)).GetHashCode().Should().Be(unchecked((int)0x6222e842u));
    Fnv1a.Create().Hash(unchecked((short)0x666fu)).GetHashCode().Should().Be(unchecked((int)0x6222e842u));
    Fnv1a.Create().Hash(unchecked((char)0x666fu)).GetHashCode().Should().Be(unchecked((int)0x6222e842u));

    // int 'foob'
    Fnv1a.Create().Hash(unchecked((uint)0x666f6f62u)).GetHashCode().Should().Be(unchecked((int)0x3f5076efu));
    Fnv1a.Create().Hash(unchecked((int)0x666f6f62u)).GetHashCode().Should().Be(unchecked((int)0x3f5076efu));

    // long 'chongo w'
    Fnv1a.Create().Hash(unchecked((ulong)0x63686f6e676f2077ul)).GetHashCode().Should().Be(unchecked((int)0xdd77ed30u));
    Fnv1a.Create().Hash(unchecked((long)0x63686f6e676f2077ul)).GetHashCode().Should().Be(unchecked((int)0xdd77ed30u));

    // utf16be b'foob' == "\u666f\u6f62"
    Fnv1a.Create().HashUtf16("\u666f\u6f62").GetHashCode().Should().Be(unchecked((int)0x3f5076efu));
  }
}