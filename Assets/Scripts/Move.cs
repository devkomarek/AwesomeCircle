using UnityEngine;

namespace Assets.Scripts{
    public class Move : MonoBehaviour{
        private const float SCALE_LINE = 10f/100f;
        private Vector3[] _positionTable;
        private bool _first = true;
        public void Calculate(float speed,float dotConcentration, SegmentController segmentController, LineRenderer lineRenderer)
        {
            const float z = 0f;
            if (_first)
            {
                _positionTable = new Vector3[(int)dotConcentration];
                _first = false;
            }
            else
            {
                lineRenderer.GetPositions(_positionTable);
            }
            var segmentsCount = dotConcentration;
            var start = segmentController.StartSegment;
            var end = segmentController.EndSegment;

            for (var i = 0; i < segmentsCount; i++)
            {
                var x = (Mathf.Cos(Mathf.Deg2Rad*start)*speed)*SCALE_LINE*speed;
                var y = (Mathf.Sin(Mathf.Deg2Rad*start)*speed)*SCALE_LINE*speed;
                start += (end/segmentsCount);
                _positionTable[i] = new Vector3(x,y,z);
            }
            lineRenderer.SetPositions(_positionTable);

        }
    }
}