using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ScriptableObject
{
    [Serializable]
    public class Wave
    {
<<<<<<< HEAD
        [SerializeField] private List<RoundBarrier> _roundBarriersList;
        [SerializeField] private string _timeBetweenRoundBarriers;
=======
        [SerializeField] private List<RoundBarrier> _roundBarrierList;
        [SerializeField] private string _timeBetweenRoundBarrier;
>>>>>>> 50e8ffb4ca5021c608acb8af7e75ed2bf3cbed22
        [SerializeField] private string _waveName;

        public string WaveName
        {
            get { return _waveName; }
            set { _waveName = value; }
        }

<<<<<<< HEAD
        public List<RoundBarrier> RoundBarrierLists
        {
            get { return _roundBarriersList; }
            set { _roundBarriersList = value; }
=======
        public List<RoundBarrier> RoundBarrierList
        {
            get { return _roundBarrierList; }
            set { _roundBarrierList = value; }
>>>>>>> 50e8ffb4ca5021c608acb8af7e75ed2bf3cbed22
        }

        public string TimeBetweenRoundBarriers
        {
            get { return _timeBetweenRoundBarriers; }
            set { _timeBetweenRoundBarriers = value; }
        }
    }
}