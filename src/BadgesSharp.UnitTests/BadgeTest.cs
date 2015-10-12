using NUnit.Framework;

namespace BadgesSharp.UnitTests
{
    [TestFixture]
    public class BadgeTest
    {
        [Test]
        public void FileName_OwnerRepositoryBadge_ValidFileName()
        {
            var target = new Badge();
            target.Owner = "one";
            target.Repository = "two";
            target.Name = "three";

            Assert.AreEqual(@"one\two\three.svg", target.FileName);
        }
    }
}
