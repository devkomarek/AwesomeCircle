using UnityEngine;

namespace Assets.Scripts
{
    public class SegmentController : MonoBehaviour
    {
        [SerializeField]private float _startSegment;
        [SerializeField]private float _endSegment;
        [SerializeField]private int _dotConcentration;

        public int DotConcentration
        {
            get { return _dotConcentration; }
            set { _dotConcentration = value; }
        }

        public float EndSegment
        {
            get { return _endSegment; }
            set { _endSegment = value; }
        }

        public float StartSegment
        {
            get { return _startSegment; }
            set { _startSegment = value; }
        }
    }
}
