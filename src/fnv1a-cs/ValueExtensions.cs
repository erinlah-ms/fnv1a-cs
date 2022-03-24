using Elms.Utils.RefExtensions;

namespace Elms.Utils.ValueExtensions
{
  /// <summary>
  /// Value overloads of the <see cref="RefExtensions.RefExtensions"/> class.
  /// </summary>
  /// <remarks>
  /// For current versions of C#. Allows method chaining off of Fnv1a.Create().
  /// 
  /// WARNING, because the Fnv1 instance is passed by value, calling `h.Hash(...)` will leave
  /// `h` unmodified. Prefer ReferenceExtensions where supported by your programming language.
  /// </remarks>
  public static class ValueExtensions
  {
    public static Fnv1a Hash(this Fnv1a h, byte b) => RefExtensions.RefExtensions.Hash(ref h, b);
    public static Fnv1a Hash(this Fnv1a h, ushort u) => RefExtensions.RefExtensions.Hash(ref h, u);
    public static Fnv1a Hash(this Fnv1a h, uint u) => RefExtensions.RefExtensions.Hash(ref h, u);
    public static Fnv1a Hash(this Fnv1a h, ulong u) => RefExtensions.RefExtensions.Hash(ref h, u);
    public static Fnv1a Hash(this Fnv1a h, sbyte s) => RefExtensions.RefExtensions.Hash(ref h, s);
    public static Fnv1a Hash(this Fnv1a h, short s) => RefExtensions.RefExtensions.Hash(ref h, s);
    public static Fnv1a Hash(this Fnv1a h, int i) => RefExtensions.RefExtensions.Hash(ref h, i);
    public static Fnv1a Hash(this Fnv1a h, long l) => RefExtensions.RefExtensions.Hash(ref h, l);
    public static Fnv1a Hash(this Fnv1a h, char c) => RefExtensions.RefExtensions.Hash(ref h, c);
    public static Fnv1a HashUtf16(this Fnv1a h, string s) => RefExtensions.RefExtensions.HashUtf16(ref h, s);
    public static Fnv1a HashUtf8(this Fnv1a h, string s) => RefExtensions.RefExtensions.HashUtf8(ref h, s);
    public static Fnv1a Hash(this Fnv1a h, byte[] bs) => RefExtensions.RefExtensions.Hash(ref h, bs);
  }
}
