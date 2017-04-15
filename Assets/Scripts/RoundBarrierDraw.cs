using System.Collections.Generic;
using Assets.Scripts.GameMaster;
using Assets.Scripts.ScriptableObject;
using UnityEngine;

namespace Assets.Scripts{
    [System.Serializable]
    [SerializeField]
    public class RoundBarrierDraw : MonoBehaviour
    {
        //        [SerializeField]
        //        private RoundBarrierData _roundBarrierData;
        //        [SerializeField]
        //        private float _speed;
        //        [SerializeField]
        //        private float _startPosition;
        //        [SerializeField]
        //        private List<SegmentData> _segmentsList;
        //        private List<SegmentData> _settingsList;
        //        private float _oldSegmentStart;
        //        private float _width;
        //        private bool _barrierIsCreated;
        //        private bool _isCollision;
        //        public List<List<Vector3>> PositionList = new List<List<Vector3>>();   
        //        private List<bool> _listRoundBarrierDone = new List<bool>();
        //        private int _numCapVertices = 0;

        private Move _move;
        private float _positionRoundBarrier;



        public List<Segment> SegmentsList; 
        private RoundBarrierGenerator _roundBarrierGenerator;
        private GM _gm;

        public float PositionRoundBarrier
        {
            get { return _positionRoundBarrier; }
            set { _positionRoundBarrier = value; }
        }

        void Start()
        {
            GameObject gameMaster = GameObject.Find("Awesome Circle").transform.FindChild("Game Master").gameObject;
            _roundBarrierGenerator = gameMaster.GetComponent<RoundBarrierGenerator>();
            _gm = gameMaster.GetComponent<GM>();
            gameObject.AddComponent<Move>();
            _move = GetComponent<Move>();
            _positionRoundBarrier = _roundBarrierGenerator.StartPosition;
        }
        void Update()
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
            _positionRoundBarrier -= _roundBarrierGenerator.Speed * Time.deltaTime;
            for (int i = 0 ; i < transform.childCount; i++)
            {
                for (int j = 0; j < transform.GetChild(i).childCount; j++)
                {
                    GameObject segmentGameObject = transform.GetChild(i).GetChild(j).gameObject;
                    SegmentController segmentController = segmentGameObject.GetComponent<SegmentController>();
                    LineRenderer lineRenderer = segmentGameObject.GetComponent<LineRenderer>();
                    lineRenderer.numPositions = _roundBarrierGenerator.DotConcentration;
                    _move.Calculate(_positionRoundBarrier, _roundBarrierGenerator.DotConcentration, segmentController, lineRenderer);
                            if (IsCollision(lineRenderer.GetPosition(0)))
                                  _gm.EndGame();                             

                }
           
            }
        }
       

        private Gradient ReturnGradient(Gradient g)
        {
            GradientColorKey[] gck;
            GradientAlphaKey[] gak;
            gck = new GradientColorKey[3];
            gck[0].color = Color.black;
            gck[0].time = 0.0F;
            gck[1].color = Color.white;
            gck[1].time = 0.5F;
            gck[2].color = Color.black;
            gck[2].time = 1.0F;
            gak = new GradientAlphaKey[4];
            gak[0].alpha = 0.0F;
            gak[0].time = 0.0F;
            gak[1].alpha = 1.0F;
            gak[1].time = 0.2F;
            gak[2].alpha = 1.0F;
            gak[2].time = 0.8F;
            gak[3].alpha = 0.0F;
            gak[3].time = 1.0F;
            g.SetKeys(gck, gak);
            return g;
        }

        #region Events
//        void OnDestroy()
//        {
//            Resources.Load<RoundBarrierDatabase>("RoundDatabase").Remove(RoundBarrierData);
//            if (Time.frameCount % 30 == 0)
//            {
//                System.GC.Collect();
//            }
//        }
//        public void OnUpdated(object source, EventArgs eventArgs)
//        {
//            CreateSettingsList();
//            SetUpPropertiesInGameObject();
//        }
        #endregion
    }
}
