using System.Collections.Generic;
using System.Text;

namespace test_fnv1a;

public partial class TestFnv1a
{
  // Reference http://www.isthe.com/chongo/tech/comp/fnv/
  private static IEnumerable<object[]> TestVectors()
  {
    yield return new object [] { "", 0x811c9dc5u };
    yield return new object [] { "a", 0xe40c292cu };
    yield return new object [] { "b", 0xe70c2de5u };
    yield return new object [] { "c", 0xe60c2c52u };
    yield return new object [] { "d", 0xe10c2473u };
    yield return new object [] { "e", 0xe00c22e0u };
    yield return new object [] { "f", 0xe30c2799u };
    yield return new object [] { "fo", 0x6222e842u };
    yield return new object [] { "foo", 0xa9f37ed7u };
    yield return new object [] { "foob", 0x3f5076efu };
    yield return new object [] { "fooba", 0x39aaa18au };
    yield return new object [] { "foobar", 0xbf9cf968u };
    yield return new object [] { "\0", 0x050c5d1fu };
    yield return new object [] { "a\0", 0x2b24d044u };
    yield return new object [] { "b\0", 0x9d2c3f7fu };
    yield return new object [] { "c\0", 0x7729c516u };
    yield return new object [] { "d\0", 0xb91d6109u };
    yield return new object [] { "e\0", 0x931ae6a0u };
    yield return new object [] { "f\0", 0x052255dbu };
    yield return new object [] { "fo\0", 0xbef39fe6u };
    yield return new object [] { "foo\0", 0x6150ac75u };
    yield return new object [] { "foob\0", 0x9aab3a3du };
    yield return new object [] { "fooba\0", 0x519c4c3eu };
    yield return new object [] { "foobar\0", 0x0c1c9eb8u };
    yield return new object [] { "ch", 0x5f299f4eu };
    yield return new object [] { "cho", 0xef8580f3u };
    yield return new object [] { "chon", 0xac297727u };
    yield return new object [] { "chong", 0x4546b9c0u };
    yield return new object [] { "chongo", 0xbd564e7du };
    yield return new object [] { "chongo ", 0x6bdd5c67u };
    yield return new object [] { "chongo w", 0xdd77ed30u };
    yield return new object [] { "chongo wa", 0xf4ca9683u };
    yield return new object [] { "chongo was", 0x4aeb9bd0u };
    yield return new object [] { "chongo was ", 0xe0e67ad0u };
    yield return new object [] { "chongo was h", 0xc2d32fa8u };
    yield return new object [] { "chongo was he", 0x7f743fb7u };
  }

  
  private static IEnumerable<object[]> Utf8CornerCases()
  {
    yield return new object[] { "0" }; // character in range 0x0000 - 0x007f
    yield return new object[] { "§" }; // character in range 0x0080 - 0x07ff
    yield return new object[] { "∀" }; // character in range 0x0800 - 0xffff
    yield return new object[] { "😼" };// character in range 0xffff - 0xffff
    yield return new object[] { "\udd00\ud800" }; // explicit surrogate pair
    yield return new object[] { "\udd00" }; // mismatched high surrogate, hanging
    yield return new object[] { "\udd000" }; // mismatched high surrogate, not hanging
    yield return new object[] { "\udc00" }; // mismatched low surrogate
  }
}
