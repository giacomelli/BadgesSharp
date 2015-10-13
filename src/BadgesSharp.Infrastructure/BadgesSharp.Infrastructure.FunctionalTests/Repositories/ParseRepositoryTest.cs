#if DEBUG
using System;
using System.Linq;
using BadgesSharp.Infrastructure.Repositories;
using NUnit.Framework;
using Skahal.Infrastructure.Framework.Repositories;

namespace BadgesSharp.Infrastructure.FunctionalTests.Repositories
{
    [TestFixture]
    public class ParseRepositoryTest
    {
        [Test]
        public void PersistNewItem_Entity_Persisted()
        {
            var entity = new StubEntity();
            entity.Date = DateTime.Now;
            entity.Integer = 2;
            entity.Text1 = "text1";
            entity.Text2 = "text2";
            entity.Text3 = "text3";

            var unitOfWork = new MemoryUnitOfWork();
            var target = new ParseRepository<StubEntity>(unitOfWork);

            target.Add(entity);
            unitOfWork.Commit();
        }

        [Test]
        public void FindAll_Filter_EntitiesFiltered()
        {
            var unitOfWork = new MemoryUnitOfWork();
            var target = new ParseRepository<StubEntity>(unitOfWork);

            foreach (var toRemove in target.FindAll())
            {
                target.Remove(toRemove);
                unitOfWork.Commit();
            }

            var entity1 = new StubEntity();
            entity1.Date = DateTime.Now;
            entity1.Text1 = "text1";
            entity1.Text2 = "text2";
            entity1.Text3 = "text3";

            var entity2 = new StubEntity();
            entity2.Date = DateTime.Now;
            entity2.Text1 = "text1";
            entity2.Text2 = "text2";
            entity2.Text3 = "text4";

            target.Add(entity1);
            target.Add(entity2);
            unitOfWork.Commit();

            var text1 = "text1";
            var actual = target.FindAll(0, 1, (b) => b.Text1 == text1 && b.Text2 == "text2" && b.Text3 == "text3");

            Assert.AreEqual(1, actual.Count());
            var first = actual.First();
            Assert.AreEqual("text3", first.Text3);

            actual = target.FindAll(0, 1, (b) => b.Text1 == text1 && b.Text2 == "text2" && b.Text3 == "text4");

            Assert.AreEqual(1, actual.Count());
            first = actual.First();
            Assert.AreEqual("text4", first.Text3);

            first = target.FindFirst((b) => b.Text1 == text1 && b.Text2 == "text2" && b.Text3 == "text3");
            Assert.AreEqual("text3", first.Text3);
        }
    }
}
#endif