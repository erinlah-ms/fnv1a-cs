namespace Elms.Utils
{
  /// <summary>
  /// Implementation of Fnv1a 32bit hash algorithm
  /// <summary>
  /// <remarks>
  /// Unless otherwise stated, hashes are performed on data using big-endian interpretation.
  ///
  /// See <a href="http://www.isthe.com/chongo/tech/comp/fnv/">isthe</a> for background on this algorithm.
  /// </remarks>
  public struct Fnv1a
  {
    private uint _h;

    public const uint Prime = 16777619u;
    public const uint Offset = 2166136261u;

    public Fnv1a(uint init)
    {
      _h = init;
    }

    public static Fnv1a Create()
    {
      return new Fnv1a(Offset);
    }

    internal void Update(byte b)
    {
       unchecked {
         var h = _h;
         h ^= (uint)b;
        h *= Prime;
        _h = h;
      }
    }

    public override int GetHashCode()
    {
      return unchecked((int)_h);
    }
  }

  /// <remarks>
  /// Struct instance methods aren't allowed ref returns (error CS8170), but static methods can.
  /// Track issue <a href="https://github.com/dotnet/csharplang/issues/4260">csharplang 4260</a>
  ///
  /// In interim, define such methods as extension methods.
  /// <remarks>
  public static class Extensions
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
    public static ref Fnv1a Hash(this ref Fnv1a h, string s)
    {
      foreach (var c in s)
      {
        h.Hash(c);
      }
      return ref h;
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
