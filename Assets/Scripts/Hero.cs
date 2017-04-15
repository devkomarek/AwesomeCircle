using Assets.Scripts.GameMaster;
using UnityEngine;

namespace Assets.Scripts{
    public class Hero : MonoBehaviour{
        private LineRenderer _laserRenderer;
        private Vector2 _firePointPosition;
        private Gun _gun;

        private CameraRay _camRay;

        private void Start()
        {
            _camRay = GameObject.Find("Awesome Circle").transform.FindChild("Main Camera").GetComponent<CameraRay>();
            _gun = GetComponent<Gun>();
            _laserRenderer = GetComponent<LineRenderer>();
            _laserRenderer.numPositions = 5;
            _laserRenderer.enabled = false;
            _laserRenderer.useWorldSpace = true;

            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        private void Update()
        {
            Shoot();
        }

        private bool _isHit = false;
        private void Shoot()
        {
            if (_camRay.PointsList.Count > 0)
            {
                GameObject[] roundBarrierGameObjects = GameObject.FindGameObjectsWithTag("RoundBarrier");
                foreach (var point in _camRay.PointsList)
                {
                    _firePointPosition = point;
                }
                _camRay.PointsList.Clear();


                if (roundBarrierGameObjects == null)
                {
                    if (!_gun.IsOverloaded)
                    {
                        SetPositionLaserRenderer(205f);
                        _gun.Miss();
                    }
              
                    return;
                }

                _isHit = false;
                RoundBarrierIntersection(roundBarrierGameObjects);
                if(_isHit == false)
                    if (!_gun.IsOverloaded)
                    {
                        SetPositionLaserRenderer(200f);
                        _gun.Miss();
                    }
            }
        }

        private void RoundBarrierIntersection(GameObject[] roundBarrierGameObjects)
        {
            Vector2 forwardFirePointPosition = new Vector2(_firePointPosition.x*30, _firePointPosition.y*30);
            foreach (var roundBarrier in roundBarrierGameObjects)
            {
                for (int i = 0; i < roundBarrier.transform.childCount; i++)
                {
                    for (int j = 0; j < roundBarrier.transform.GetChild(i).childCount; j++)
                    {
                        LineRenderer lineRenderer = roundBarrier.transform.GetChild(i).GetChild(j).GetComponent<LineRenderer>();
                        for (int k = 0; k < lineRenderer.numPositions - 1; k++)
                        {
                            if (LineIntersection.FasterLineSegmentIntersection(Vector3.zero, forwardFirePointPosition, lineRenderer.GetPosition(k), lineRenderer.GetPosition(k+1)))
                                if (!_gun.IsOverloaded)
                                {
                                    GM.KillRoundBarrier(roundBarrier);
                                    SetPositionLaserRenderer(Vector3.Distance(Vector3.zero,lineRenderer.GetPosition(k)));
                                    _isHit = true;
                                    return;
                                }
                        }                       
                    }
                }       
            }
        }
    

        private void SetPositionLaserRenderer(float distance)
        {
            Vector3 hit = new Vector3(_firePointPosition.x*((distance*0.90f)/_firePointPosition.magnitude), _firePointPosition.y*((distance*0.90f)/_firePointPosition.magnitude));
            Vector3 center = new Vector3(hit.x/2, hit.y/2);
            Vector3 hit2To5 = new Vector3(center.x/2,center.y/2);
            Vector3 hit4To5 = new Vector3((center.x+hit.x)/2,(center.y+hit.y)/2);
            _laserRenderer.SetPosition(0, Vector2.zero);
            _laserRenderer.SetPosition(1, hit2To5);
            _laserRenderer.SetPosition(2, center);
            _laserRenderer.SetPosition(3, hit4To5);
            _laserRenderer.SetPosition(4, hit);
            StartCoroutine(GM.ShootEffect(_laserRenderer));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            GM.KillRoundBarrier(other.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject);
        }
    }
}