using System.Collections.Generic;
using Assets.Scripts.GameMaster;
using Assets.Scripts.ScriptableObject;
using UnityEngine;

namespace Assets.Scripts{
    [System.Serializable]
    [SerializeField]
    public class RoundBarrierDraw : MonoBehaviour
    {
        private Move _move;
        private float _positionRoundBarrier;
        public List<Segment> SegmentsList;
        public bool IsDead = false;
        private RoundBarrierGenerator _roundBarrierGenerator;
        private GM _gm;

        public float PositionRoundBarrier
        {
            get { return _positionRoundBarrier; }
            set { _positionRoundBarrier = value; }
        }

        private void Start()
        {
            GameObject gameMaster = GameObject.Find("Awesome Circle").transform.FindChild("Game Master").gameObject;
            _roundBarrierGenerator = gameMaster.GetComponent<RoundBarrierGenerator>();
            _gm = gameMaster.GetComponent<GM>();
            gameObject.AddComponent<Move>();
            _move = GetComponent<Move>();
            _positionRoundBarrier = _roundBarrierGenerator.StartPosition;
        }

        private void Update()
        {
            if (SegmentsList != null)
            {
                GetPropertiesFromRoundBarrierGenerator();
                DrawSegments();
            }
        }

        private void GetPropertiesFromRoundBarrierGenerator()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                for (int j = 0; j < transform.GetChild(i).transform.childCount; j++)
                {
                    LineRenderer lineRenderer = transform.GetChild(i).GetChild(j).GetComponent<LineRenderer>();
                    if(IsDead == false)
                    lineRenderer.colorGradient = _roundBarrierGenerator.Gradient;
                    lineRenderer.material = _roundBarrierGenerator.RoundBarrierMaterial;
                    lineRenderer.numCapVertices = _roundBarrierGenerator.NumCapVertices;
                    lineRenderer.startWidth = _roundBarrierGenerator.Width;
                    lineRenderer.endWidth = _roundBarrierGenerator.Width;
                }
            }
        }

        private bool IsCollision(Vector3 collision)
        {
            return Vector3.Distance(collision, Vector3.zero) < _roundBarrierGenerator.PlayerZone;
        }

        private void DrawSegments()
        {
            _positionRoundBarrier -= _roundBarrierGenerator.Speed*Time.deltaTime;
            for (int i = 0; i < transform.childCount; i++)
            {
                for (int j = 0; j < transform.GetChild(i).childCount; j++)
                {
                    GameObject segmentGameObject = transform.GetChild(i).GetChild(j).gameObject;
                    SegmentController segmentController = segmentGameObject.GetComponent<SegmentController>();
                    LineRenderer lineRenderer = segmentGameObject.GetComponent<LineRenderer>();
                    lineRenderer.positionCount = segmentController.DotConcentration;
                    _move.Calculate(_positionRoundBarrier, segmentController.DotConcentration, segmentController, lineRenderer);
                    if (IsCollision(lineRenderer.GetPosition(0)) && IsDead == false)
                        _gm.EndGame();
                        // _gm.KillRoundBarrier(segmentGameObject);


                }

            }
        }
    }
}
