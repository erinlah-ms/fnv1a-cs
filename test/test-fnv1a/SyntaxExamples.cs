using Erlams.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test_fnv1a
{
  using Erlams.Utils.RefExtensions;
  using FluentAssertions;

  [TestClass]
  public partial class SyntaxExamples
  {
    [TestMethod]
    public void RefSyntaxExample()
    {

      // arrange
      Fnv1a tmp;

      // act
      
      // Note, ideally this should read 
      //
      //   var result = Fnv1a.Create().Hash((byte)0);
      //
      // but C# 10.0 lacks support for ref binding to local temporaries.
      tmp = Fnv1a.Create();
      var result = tmp.Hash((byte)0);

      // assert
      result.Should().Be(unchecked((int)0));
    }
  }
}
