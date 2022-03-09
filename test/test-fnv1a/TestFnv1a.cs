using Elms.Utils;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace test_fnv1a;

[TestClass]
public partial class TestFnv1a
{
  [TestMethod]
  public void TestEmpty()
  {
    // arrange
    var h = Fnv1a.Create();

    // act

    // assert
    h.GetHashCode().Should().Be(unchecked((int)0x811c9dc5));
  }

  [DataTestMethod]
  [DynamicData(nameof(TestVectors), DynamicDataSourceType.Method)]
  public void TestAsciiVectors(byte[] input, uint expectedHash)
  {
    // arrange
    var h = Fnv1a.Create();

    // act
    h.Hash(input);

    // assert
    h.GetHashCode().Should().Be(unchecked((int)expectedHash));
  }

  [TestMethod]
  public void TestPrimitivesCoverage()
  {
    // TODO: why can't method chain on temporaries with ref returns?
    Fnv1a tmp;

    // byte 'a'
    tmp = Fnv1a.Create(); tmp.Hash(unchecked((byte)0x61u)).GetHashCode().Should().Be(unchecked((int)0xe40c292cu));
    tmp = Fnv1a.Create(); tmp.Hash(unchecked((sbyte)0x61u)).GetHashCode().Should().Be(unchecked((int)0xe40c292cu));

    // short 'fo'
    tmp = Fnv1a.Create(); tmp.Hash(unchecked((ushort)0x666fu)).GetHashCode().Should().Be(unchecked((int)0x6222e842u));
    tmp = Fnv1a.Create(); tmp.Hash(unchecked((short)0x666fu)).GetHashCode().Should().Be(unchecked((int)0x6222e842u));
    tmp = Fnv1a.Create(); tmp.Hash(unchecked((char)0x666fu)).GetHashCode().Should().Be(unchecked((int)0x6222e842u));

    // int 'foob'
    tmp = Fnv1a.Create(); tmp.Hash(unchecked((uint)0x666f6f62u)).GetHashCode().Should().Be(unchecked((int)0x3f5076efu));
    tmp = Fnv1a.Create(); tmp.Hash(unchecked((int)0x666f6f62u)).GetHashCode().Should().Be(unchecked((int)0x3f5076efu));

    // long 'chongo w'
    tmp = Fnv1a.Create(); tmp.Hash(unchecked((ulong)0x63686f6e676f2077ul)).GetHashCode().Should().Be(unchecked((int)0xdd77ed30u));
    tmp = Fnv1a.Create(); tmp.Hash(unchecked((long)0x63686f6e676f2077ul)).GetHashCode().Should().Be(unchecked((int)0xdd77ed30u));

    // utf16be b'foob' == "\u666f\u6f62"
    tmp = Fnv1a.Create(); tmp.Hash("\u666f\u6f62").GetHashCode().Should().Be(unchecked((int)0x3f5076efu));

    // Hand crafted utf16be test case. Not part of original test vectors
    tmp = Fnv1a.Create(); tmp.Hash("hello").GetHashCode().Should().Be(unchecked((int)0x1507ef4fu));
  }
}