using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ScriptableObject
{
    [Serializable]
    public class RoundBarrier
    {
        [SerializeField] private string _roundBarrierName;
        [SerializeField] private List<Segment> _segmentList;

        public string RoundBarrierName
        {
            get { return _roundBarrierName; }
            set { _roundBarrierName = value; }
        }

        public List<Segment> SegmentList
        {
            get { return _segmentList; }
            set { _segmentList = value; }
        }
    }
}