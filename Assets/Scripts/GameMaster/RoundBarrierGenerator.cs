using System;
using System.Collections.Generic;
using Assets.Scripts.ScriptableObject;
using UnityEngine;

namespace Assets.Scripts.GameMaster{
    [Serializable]
    public class RoundBarrierGenerator : MonoBehaviour
    {
        public GM Gm;
        public float BasicsSpeed;
        [SerializeField] private float _speed;
        [SerializeField] private float _startPosition;
        [SerializeField] private Material _roundBarrierMaterial;
        [SerializeField] private int _numCapVertices;
        [SerializeField] private int _numCornerVertices;
        [SerializeField] private Gradient _gradient;
        [SerializeField] private float _width;
        [SerializeField] private float _playerZone;

        private GameObject _roundBarrierGameObject;
        private List<Segment> _segmentsList;
        private List<SegmentController> _segmentControllersList;
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

        public int NumCornerVertices
        {
            get { return _numCornerVertices; }
            set { _numCornerVertices = value; }
        }

        public Material RoundBarrierMaterial
        {
            get { return _roundBarrierMaterial; }
            set { _roundBarrierMaterial = value; }
        }

        public float PlayerZone
        {
            get { return _playerZone; }
            set { _playerZone = value; }
        }
        #endregion

        public void CreateRoundBarrier(RoundBarrier roundBarrierData, int offset)
        {
            GetRoundBarrierData(roundBarrierData);
            CreateGameObjectRoundBarrier();
            CreateOtherElementsRoundBarrier();
            if(Gm.IsAwesomeHexagonActive)
                AddDataToSegmentControllerHexagon(offset);
            else
                AddDataToSegmentController(offset);
        }

        private void GetRoundBarrierData(RoundBarrier roundBarrierData)
        {
            _segmentControllersList = new List<SegmentController>();
            _nameRoundBarrier = roundBarrierData.RoundBarrierName;
            _segmentsList = roundBarrierData.SegmentsList;
        }

        private void CreateGameObjectRoundBarrier()
        {

            _roundBarrierGameObject = new GameObject
            {
                name = _nameRoundBarrier,
                tag = "RoundBarrier"
            };
            _roundBarrierGameObject.AddComponent<RoundBarrierDraw>();
            _roundBarrierGameObject.GetComponent<RoundBarrierDraw>().SegmentsList = _segmentsList;
        }

        private void AddDataToSegmentController(int offset)
        {
            for (int i = 0; i < _segmentControllersList.Count; i++)
            {
                if (Math.Abs(_segmentsList[i].End) == 45)
                    _segmentControllersList[i].DotConcentration = 6;
                else if (Math.Abs(_segmentsList[i].End) < 90)
                    _segmentControllersList[i].DotConcentration = 10;
                else if (Math.Abs(_segmentsList[i].End) <= 90)
                    _segmentControllersList[i].DotConcentration = 20;
                else if (Math.Abs(_segmentsList[i].End) <= 180)
                    _segmentControllersList[i].DotConcentration = 26;
                else if (Math.Abs(_segmentsList[i].End) <= 270)
                    _segmentControllersList[i].DotConcentration = 33;
                else
                    _segmentControllersList[i].DotConcentration = 50;
                _segmentControllersList[i].StartSegment = _segmentsList[i].Start + offset;
                _segmentControllersList[i].EndSegment = _segmentsList[i].End;
            }
        }

        private void AddDataToSegmentControllerHexagon(int offset)
        {
            for (int i = 0; i < _segmentControllersList.Count; i++)
            {
                if (Math.Abs(_segmentsList[i].End) == 45)
                    _segmentControllersList[i].DotConcentration = 3;
                else if (Math.Abs(_segmentsList[i].End) < 90)
                    _segmentControllersList[i].DotConcentration = 3;
                else if (Math.Abs(_segmentsList[i].End) <= 90)
                    _segmentControllersList[i].DotConcentration = 3;
                else if (Math.Abs(_segmentsList[i].End) <= 180)
                    _segmentControllersList[i].DotConcentration = 5;
                else if (Math.Abs(_segmentsList[i].End) <= 270)
                    _segmentControllersList[i].DotConcentration = 5;
                else
                    _segmentControllersList[i].DotConcentration = 7;

                
                _segmentControllersList[i].StartSegment = _segmentsList[i].Start + offset;
                if (_segmentsList[i].End > 300)
                    _segmentControllersList[i].EndSegment = _segmentsList[i].End - 90;
                else
                {
                    if(_segmentsList[i].End == -90)
                        _segmentControllersList[i].EndSegment = _segmentsList[i].End - 40;
                    else if (_segmentsList[i].End == 90)
                        _segmentControllersList[i].EndSegment = _segmentsList[i].End + 40;
                    else if (_segmentsList[i].End >= -90 && _segmentsList[i].End <= 0)
                    _segmentControllersList[i].EndSegment = _segmentsList[i].End - 70;
                    else if (_segmentsList[i].End <= 90 && _segmentsList[i].End >= 0)
                    {
                        _segmentControllersList[i].EndSegment = _segmentsList[i].End + 70;
                    }
                    else
                    {
                        _segmentControllersList[i].EndSegment = _segmentsList[i].End;
                    }
                }
                    
            }
        }

        private void ManageDotConcentration()
        {
            
        }

        private void CreateOtherElementsRoundBarrier()
        {
            var tempSegmentsList = _segmentsList.GetRange(0, _segmentsList.Count);
            for (var i = 0; i < tempSegmentsList.Count; i++)
            {
                var segmentGameObject = ReturnGameObjectSegmentSettingsSetUpInHerarchy("Hit");

                for (var j = 0; j < tempSegmentsList.Count; j++)
                {
                    if (tempSegmentsList[i].TagSegment == tempSegmentsList[j].TagSegment)
                    {
                        var barrier = ReturnGameObjectBarrierSetUpInHerarchy("Hit", segmentGameObject);
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
            go.AddComponent<SegmentController>();
            go.AddComponent<LineRenderer>();
            _segmentControllersList.Add(go.GetComponent<SegmentController>());
        }



    }
}