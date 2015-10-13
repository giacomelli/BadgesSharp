using System;
using BadgesSharp.Builders;
using HelperSharp;
using NUnit.Framework;
using Skahal.Infrastructure.Framework.Repositories;
using TestSharp;

namespace BadgesSharp.UnitTests
{
    [TestFixture]
    public class BadgeServiceTest
    {
        #region Fields
        private MemoryUnitOfWork m_unitOfWork;
        private MemoryRepository<Badge> m_repository;
        private BadgeService m_target;
        #endregion

        #region Setup
        [SetUp]
        public void Setup()
        {
            m_unitOfWork = new MemoryUnitOfWork();
            m_repository = new MemoryRepository<Badge>(m_unitOfWork, (b) => null);
            m_target = new BadgeService(m_repository, m_unitOfWork);
        }
        #endregion

        [Test]
        public void GetBadge_NoBuilderForBadge_Exception()
        {
            ExceptionAssert.IsThrowing(new InvalidOperationException("There is no builder for badge 'invalid badge'. The available badges are: {0}".With(String.Join(", ", BuilderService.AvailableBadgesNames))), () =>
            {
                m_target.GetBadge("owner1", "repository1", "invalid badge");
            });
        }

        [Test]
        public void GetBadge_NoData_DefaultBadge()
        {
            var badge = m_target.GetBadge("owner1", "repository1", "FxCop");
            Assert.IsNotNull(badge);
            Assert.AreEqual("owner1", badge.Owner);
            Assert.AreEqual("repository1", badge.Repository);
            Assert.AreEqual("FxCop", badge.Name);
            StringAssert.Contains("no data", badge.Svg);
            Assert.AreNotEqual(0, badge.GetData().Length);
        }

        [Test]
        public void GetBadge_ThereIsData_Badge()
        {
            m_repository.Add(new Badge()
            {
                Owner = "owner1",
                Repository = "repository1",
                Name = "FxCop",
                Svg = "saved one"
            });
            m_unitOfWork.Commit();

            var badge = m_target.GetBadge("owner1", "repository1", "FxCop");
            Assert.IsNotNull(badge);
            Assert.AreEqual("owner1", badge.Owner);
            Assert.AreEqual("repository1", badge.Repository);
            Assert.AreEqual("FxCop", badge.Name);
            Assert.AreEqual("saved one", badge.Svg);
        }

        [Test]
        public void CountBadges_NoArgs_TotalBadges()
        {
            Assert.AreEqual(0, m_target.CountBadges());

            m_repository.Add(new Badge()
            {
                Owner = "owner1",
                Repository = "repository1",
                Name = "FxCop",
                Svg = "saved one"
            });
            m_unitOfWork.Commit();

            Assert.AreEqual(1, m_target.CountBadges());
        }

        [Test]
        public void Save_New_Created()
        {
            var badge = new Badge()
            {
                Owner = "owner1",
                Repository = "repository1",
                Name = "FxCop",
                Svg = "saved one"
            };

            m_target.SaveBadge(badge);
            m_unitOfWork.Commit();

            Assert.AreEqual(1, m_target.CountBadges());
            var actual = m_target.GetBadge("owner1", "repository1", "FxCop");
            Assert.IsNotNull(actual);
            Assert.AreEqual("owner1", actual.Owner);
        }

        [Test]
        public void Save_Old_Update()
        {
            var badge = new Badge()
            {
                Owner = "owner1",
                Repository = "repository1",
                Name = "FxCop",
                Svg = "saved one"
            };

            m_target.SaveBadge(badge);
            m_unitOfWork.Commit();

            var old = m_target.GetBadge("owner1", "repository1", "FxCop");
            m_target.SaveBadge(old);

            Assert.AreEqual(1, m_target.CountBadges());
            var actual = m_target.GetBadge("owner1", "repository1", "FxCop");
            Assert.IsNotNull(actual);
            Assert.AreEqual("owner1", actual.Owner);
        }
    }
}
