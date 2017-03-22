using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ScriptableObject
{
    [Serializable]
    public class Wave
    {
        [SerializeField] private List<RoundBarrier> _roundBarrierDataList;
        [SerializeField] private string _timeBetweenRoundBarrier;
        [SerializeField] private string _waveDataName;

        public string WaveDataName
        {
            get { return _waveDataName; }
            set { _waveDataName = value; }
        }

        public List<RoundBarrier> RoundBarrierDataList
        {
            get { return _roundBarrierDataList; }
            set { _roundBarrierDataList = value; }
        }

        public string TimeBetweenRoundBarrier
        {
            get { return _timeBetweenRoundBarrier; }
            set { _timeBetweenRoundBarrier = value; }
        }
    }
}