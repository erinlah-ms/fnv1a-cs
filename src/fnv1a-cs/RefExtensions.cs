using System.Text;

namespace Elms.Utils.RefExtensions
{
  /// <remarks>
  /// Struct instance methods aren't allowed ref returns (error CS8170), but static methods can.
  /// Track issue <a href="https://github.com/dotnet/csharplang/issues/4260">csharplang 4260</a>.
  /// In interim, define such methods as extension methods.
  /// 
  /// Additionally, C# (as of 10.0) still does not yet allow ref parameters to bind to temporaries.
  /// If you want to use method chaining for convenience, use the ValueExtensions instead.
  /// 
  /// Examples
  /// 
  ///    using Elms.Utils.RefExtensions;
  ///    Fnv1a tmp;
  ///    
  ///    tmp = Fnv1a.Create();
  ///    var result = tmp.Hash("example").GetHashCode();
  /// 
  /// Until temporaries can be ref bound, use the following, albeit with more value copies:
  /// 
  ///    using Elms.Utils.ValueExtensions;
  ///
  ///    var result = Fnv1a.Create().Hash("example").GetHashCode();
  /// 
  /// <remarks>
  public static class RefExtensions
  {
    #region builtin unsigned integral types
    public static ref Fnv1a Hash(this ref Fnv1a h, byte b)
    {
      h.Update(b);
      return ref h;
    }

    public static ref Fnv1a Hash(this ref Fnv1a h, ushort u)
    {
      unchecked {
        h.Update((byte)(0xffu & (u>>8)));
        h.Update((byte)(0xffu & (u)));
      }
      return ref h;
    }
    public static ref Fnv1a Hash(this ref Fnv1a h, uint u)
    {
      unchecked {
        h.Update((byte)(0xffu & (u>>24)));
        h.Update((byte)(0xffu & (u>>16)));
        h.Update((byte)(0xffu & (u>>8)));
        h.Update((byte)(0xffu & (u)));
      }
      return ref h;
    }
    public static ref Fnv1a Hash(this ref Fnv1a h, ulong u)
    {
      unchecked {
        h.Update((byte)(0xfful & (u>>56)));
        h.Update((byte)(0xfful & (u>>48)));
        h.Update((byte)(0xfful & (u>>40)));
        h.Update((byte)(0xfful & (u>>32)));
        h.Update((byte)(0xfful & (u>>24)));
        h.Update((byte)(0xfful & (u>>16)));
        h.Update((byte)(0xfful & (u>>8)));
        h.Update((byte)(0xfful & (u)));
      }
      return ref h;
    }
    #endregion

    #region signed integral types
    public static ref Fnv1a Hash(this ref Fnv1a h, sbyte s) => ref h.Hash(unchecked((byte)s));
    public static ref Fnv1a Hash(this ref Fnv1a h, short s) => ref h.Hash(unchecked((ushort)s));
    public static ref Fnv1a Hash(this ref Fnv1a h, int i) => ref h.Hash(unchecked((uint)i));
    public static ref Fnv1a Hash(this ref Fnv1a h, long l) => ref h.Hash(unchecked((ulong)l));
    #endregion

    #region builtin utf16 text types
    /// <summary>
    /// Utf16-be hash
    /// <summary>
    public static ref Fnv1a Hash(this ref Fnv1a h, char c) => ref h.Hash(unchecked((ushort)c));

    /// <summary>
    /// Utf16-be hash
    /// <summary>
    public static ref Fnv1a HashUtf16(this ref Fnv1a h, string s)
    {
      foreach (var c in s)
      {
        h.Hash((ushort)c);
      }
      return ref h;
    }
    #endregion

    #region utf8 hashing
    /// <summary>
    /// Utf8
    /// <summary>
    /// <remarks>
    /// Assumes all surrogate pairs are properly paired. To match parity with Encoding.UTF8.GetBytes, mismatched
    /// surrogate pairs will be replaced with \ufffd
    /// </remarks>
    public static ref Fnv1a HashUtf8(this ref Fnv1a h, string s)
    {
      unchecked
      {
        for (int i = 0; i < s.Length; ++i)
        {
          uint current = s[i];

          if (char.IsSurrogate((char)current))
          {
            if (char.IsHighSurrogate((char)current) && i < s.Length + 1 && char.IsLowSurrogate(s[i + 1]))
            {
              // handle pair
              current = (uint)char.ConvertToUtf32((char)current, s[i + 1]);
              ++i;
            }
            else
            {
              // substitute with replacement character, as Encoding.UTF8 would do.
              current = '\ufffd';
            }
          }

          if (current <= 0x007f)
          {
            h.Hash((byte)current);
          }
          else
          {
            HashMultibyteUtf8(ref h, current);
          }
        }
      }
      return ref h;
    }

    private static void HashMultibyteUtf8(ref Fnv1a h, uint utf32codepoint)
    {
      unchecked
      {
        if (utf32codepoint <= 0x7ff)
        {
          // reference https://en.wikipedia.org/wiki/UTF-8#Encoding
          var byte2 = (byte)(0b10000000u | (0b00111111u & utf32codepoint));
          utf32codepoint >>= 6;
          var byte1 = (byte)(0b11000000u | (0b00011111u & utf32codepoint));
          h.Hash(byte1);
          h.Hash(byte2);
        }
        else if (utf32codepoint <= 0xffff)
        {
          // reference https://en.wikipedia.org/wiki/UTF-8#Encoding
          var byte3 = (byte)(0b10000000u | (0b00111111u & utf32codepoint));
          utf32codepoint >>= 6;
          var byte2 = (byte)(0b10000000u | (0b00111111u & utf32codepoint));
          utf32codepoint >>= 6;
          var byte1 = (byte)(0b11100000u | (0b00001111u & utf32codepoint));
          h.Hash(byte1);
          h.Hash(byte2);
          h.Hash(byte3);
        }
        else
        {
          // reference https://en.wikipedia.org/wiki/UTF-8#Encoding
          var byte4 = (byte)(0b10000000u | (0b00111111u & utf32codepoint));
          utf32codepoint >>= 6;
          var byte3 = (byte)(0b10000000u | (0b00111111u & utf32codepoint));
          utf32codepoint >>= 6;
          var byte2 = (byte)(0b10000000u | (0b00111111u & utf32codepoint));
          utf32codepoint >>= 6;
          var byte1 = (byte)(0b11110000u | (0b00000111u & utf32codepoint));
          h.Hash(byte1);
          h.Hash(byte2);
          h.Hash(byte3);
          h.Hash(byte4);
        }
      }
    }
    #endregion

    #region Arrays
    public static ref Fnv1a Hash(this ref Fnv1a h, byte[] bs)
    {
      foreach (var b in bs)
      {
        h.Hash(b);
      }
      return ref h;
    }
    #endregion
  }
}
