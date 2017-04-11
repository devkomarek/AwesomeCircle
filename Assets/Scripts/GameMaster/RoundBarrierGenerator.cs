using System;
using System.Collections.Generic;
using Assets.Scripts.ScriptableObject;
using UnityEngine;

namespace Assets.Scripts.GameMaster{
    [Serializable]
    public class RoundBarrierGenerator : MonoBehaviour{
        [SerializeField] private float _speed;
        [SerializeField] private float _startPosition;
        [SerializeField] private Material _roundBarrierMaterial;
        [SerializeField] private int _dotConcentration;
        [SerializeField] private int _numCapVertices;
        [SerializeField] private Gradient _gradient;
        [SerializeField] private float _width;

        private GameObject _roundBarrierGameObject;
        private List<Segment> _segmentsList;
        private string _nameRoundBarrier;

        #region Properties
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public float StartPosition
        {
            get { return _startPosition; }
            set { _startPosition = value; }
        }
        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public Gradient Gradient
        {
            get { return _gradient; }
            set { _gradient = value; }
        }

        public int NumCapVertices
        {
            get { return _numCapVertices; }
            set { _numCapVertices = value; }
        }

        public int DotConcentration
        {
            get { return _dotConcentration; }
            set { _dotConcentration = value; }
        }

        public Material RoundBarrierMaterial
        {
            get { return _roundBarrierMaterial; }
            set { _roundBarrierMaterial = value; }
        }
        #endregion

        public void CreateRoundBarrier(RoundBarrier roundBarrierData)
        {
            GetRoundBarrierData(roundBarrierData);
            CreateGameObjectRoundBarrier();
            CreateOtherElementsRoundBarrier();
           // StartDrawCircle(roundBarrierData, randomRotation);
        }

        private void GetRoundBarrierData(RoundBarrier roundBarrierData)
        {
            //_segmentsList = new List<Segment>();
            _nameRoundBarrier = roundBarrierData.RoundBarrierName;
            _segmentsList = roundBarrierData.SegmentsList;
        }

        private void StartDrawCircle(RoundBarrier roundBarrierData, int randomRotation)
        {
//            RoundBarrierDraw roundBarrierDraw = _roundBarrierGameObject.GetComponent<RoundBarrierDraw>();
//            roundBarrierDraw.Speed = Speed;
//            roundBarrierDraw.StartPosition = StartPosition;
//            roundBarrierDraw.StartDrawCircle(roundBarrierData, randomRotation);
        }

        private void CreateGameObjectRoundBarrier()
        {

            _roundBarrierGameObject = new GameObject
            {
                name = _nameRoundBarrier,
                tag = "RoundBarrier"
            };
            //_roundBarrierGameObject.AddComponent<RoundBarrierDraw>();
        }

        private void CreateOtherElementsRoundBarrier()
        {
            var tempSegmentsList = _segmentsList.GetRange(0, _segmentsList.Count);
            for (var i = 0; i < tempSegmentsList.Count; i++)
            {
                var segmentGameObject = ReturnGameObjectSegmentSettingsSetUpInHerarchy(tempSegmentsList[i].TagSegment);

                for (var j = 0; j < tempSegmentsList.Count; j++)
                {
                    if (tempSegmentsList[i].TagSegment == tempSegmentsList[j].TagSegment)
                    {
                        var barrier = ReturnGameObjectBarrierSetUpInHerarchy(tempSegmentsList[i].TagSegment, segmentGameObject);
                        AddComponentsToGameObject(barrier);
                        if (j != i)
                        {
                            tempSegmentsList.RemoveRange(j, 1);
                            j--;
                        }
                    }
                }
            }
        }

        private GameObject ReturnGameObjectBarrierSetUpInHerarchy(string tag, GameObject Go)
        {
            var barrier = new GameObject
            {
                name = tag,
                tag = tag
            };
            barrier.transform.parent = Go.transform;
            return barrier;
        }

        private GameObject ReturnGameObjectSegmentSettingsSetUpInHerarchy(string tag)
        {
            var segmentGameObject = new GameObject(tag + "Settings")
            {
                name = tag + "Settings",
                tag = tag + "Settings"
            };
            segmentGameObject.transform.parent = _roundBarrierGameObject.transform;
            return segmentGameObject;
        }

        private void AddComponentsToGameObject(GameObject go)
        {
            go.AddComponent<LineRenderer>();
        }
    }
}