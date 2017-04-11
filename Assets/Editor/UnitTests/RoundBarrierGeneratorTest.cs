using System.Collections.Generic;
using Assets.Scripts.GameMaster;
using Assets.Scripts.ScriptableObject;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests
{
    [TestFixture]
    public class RoundBarrierGeneratorTest
    {
        //Naming Standards for Unit Tests <method>_Should<expected>_When<condition expected>
        private RoundBarrierGenerator _roundBarrierGenerator;
        [SetUp]
        public void Init()
        {
            _roundBarrierGenerator = GameObject.Find("Awesome Circle").transform.FindChild("Game Master").GetComponent<RoundBarrierGenerator>();
        }
        [TearDown]
        public void Cleanup()
        {
            _roundBarrierGenerator = null;
        }
        [Test]
        public void CreateRoundBarrier_ShouldSetUpRightInHierarchy_WhenTrue()
        {
            RoundBarrier roundBarrier = ScriptableObject.CreateInstance<RoundBarrier>();
            List<Segment> segmentsList = new List<Segment> {ScriptableObject.CreateInstance<Segment>()};
            segmentsList[0].Start = 0;
            segmentsList[0].End = -90;
            segmentsList[0].TagSegment = "Hit";
            roundBarrier.RoundBarrierName = "r-0.0.0";
            roundBarrier.SegmentsList = segmentsList;
            _roundBarrierGenerator.CreateRoundBarrier(roundBarrier);

            Assert.AreEqual(GameObject.Find("r-0.0.0").transform.name, "r-0.0.0");

            Assert.NotNull(GameObject.Find("r-0.0.0").transform.FindChild("HitSettings"));
            Assert.AreEqual(GameObject.Find("r-0.0.0").transform.FindChild("HitSettings").GetChild(0).tag, "Hit");
        }

    }
}
