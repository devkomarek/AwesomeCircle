using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ScriptableObject
{
    [Serializable]
    public class Wave
    {
        [SerializeField] private List<RoundBarrier> _roundBarriersList;
        [SerializeField] private string _timeBetweenRoundBarriers;
        [SerializeField] private string _waveName;

        public string WaveName
        {
            get { return _waveName; }
            set { _waveName = value; }
        }

        public List<RoundBarrier> RoundBarriersList
        {
            get { return _roundBarriersList; }
            set { _roundBarriersList = value; }
        }

        public string TimeBetweenRoundBarriers
        {
            get { return _timeBetweenRoundBarriers; }
            set { _timeBetweenRoundBarriers = value; }
        }
    }
}