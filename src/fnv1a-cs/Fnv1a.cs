using System.Collections.Generic;

namespace Erlams.Utils
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
}
