using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ScriptableObject
{
    [Serializable]
    public class Wave
    {
        [SerializeField] private List<RoundBarrier> _roundBarrierList;
        [SerializeField] private string _timeBetweenRoundBarrier;
        [SerializeField] private string _waveName;

        public string WaveName
        {
            get { return _waveName; }
            set { _waveName = value; }
        }

        public List<RoundBarrier> RoundBarrierList
        {
            get { return _roundBarrierList; }
            set { _roundBarrierList = value; }
        }

        public string TimeBetweenRoundBarrier
        {
            get { return _timeBetweenRoundBarrier; }
            set { _timeBetweenRoundBarrier = value; }
        }
    }
}