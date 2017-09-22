using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameMaster;
using UnityEngine;

namespace Assets.Scripts{
    public class Hero : MonoBehaviour{
        public float Left, Top;
        public float _endShootEffect;
        public Gradient LaserGradient;
        private LineRenderer[] _laserRenderers;
        private List<Vector2> _firePointPositions;
        private Gun _gun;
        private GM _gm;      
        private CameraRay _camRay;
        private GameObject _shootEffect;
        private float _startShootEffect;

        private void Start()
        {
            _camRay = GameObject.Find("Awesome Circle").transform.Find("Main Camera").GetComponent<CameraRay>();
            _gm = GameObject.Find("Awesome Circle").transform.Find("Game Master").GetComponent<GM>();
            _shootEffect = transform.Find("Shoot Effect").gameObject;
            _startShootEffect = _shootEffect.transform.localScale.x;            
            _gun = GetComponent<Gun>();
            _positionsLineRenderer = new List<Vector3>();
            _rbToKill = new List<GameObject>();
            _firePointPositions = new List<Vector2>();
            _laserRenderers = GetComponentsInChildren<LineRenderer>();
            foreach (var laser in _laserRenderers)
            {
               laser.positionCount = 5;
               laser.enabled = false;
               laser.useWorldSpace = true;
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
            
        }

        private void Update()
        {
            Shoot();
        }

        private void Shoot()
        {
            if (_camRay.PointsList.Count > 0)
            {
                GameObject[] roundBarrierGameObjects = GameObject.FindGameObjectsWithTag("RoundBarrier");
                _firePointPositions.Clear();
                foreach (var point in _camRay.PointsList)
                {
                    _firePointPositions.Add(point);
                }
                _camRay.PointsList.Clear();
                _positionsLineRenderer.Clear();
                _rbToKill.Clear();
                RoundBarrierIntersection(roundBarrierGameObjects);
            }
        }

        private Vector2 ReturnForwardPoint(Vector2 f)
        {
            float a = f.y/f.x;

            float a1 = 0;
            float b1 = Left;
            float b2 = Top;


            float x = -1*(b1/(a + a1));
            float y = -1*((b1*a)/(a + a1));

            float x1 = -b2;
            float y1 = -b2*a;

            if (Vector2.Distance(Vector2.zero, new Vector2(x, y)) < Vector2.Distance(Vector2.zero, new Vector2(x1, y1)))
            {
                                if (f.y < 0 && f.x < 0)
                                    return new Vector2(x, y);
                                if (f.y < 0 && f.x > 0)
                                    return new Vector2(x, y);
                                if (f.y > 0 && f.x < 0)
                                    return new Vector2(-x, -y);
                                if (f.y > 0 && f.x > 0)
                                    return new Vector2(-x, -y);
                return new Vector2(x, y);
            }
            else
            {
                if (f.y < 0 && f.x < 0)
                    return new Vector2(x1, y1);
                if (f.y < 0 && f.x > 0)
                    return new Vector2(-x1, -y1);
                if (f.y > 0 && f.x < 0)
                    return new Vector2(x1, y1);
                if (f.y > 0 && f.x > 0)
                    return new Vector2(-x1, -y1);
                return new Vector2(x1, y1);
            }
        }
        //        LineRenderer lineRenderer = roundBarrier.transform.GetChild(i).GetChild(j).GetComponent<LineRenderer>();
        //        int positionCount = lineRenderer.positionCount;
        //        Vector3[] table = new Vector3[positionCount + 4];
        //        lineRenderer.GetPositions(table);
        //                            float w = 0.33f;
        //        float x = Mathf.Atan2(lineRenderer.GetPosition(positionCount - 1).y, lineRenderer.GetPosition(positionCount - 1).x) * Mathf.Rad2Deg;
        //        float res = x >= 0 ? x : x + 360;
        //        float r = Vector2.Distance(table[0], Vector2.zero);
        //        Vector3 p = new Vector3(Mathf.Cos(res * Mathf.Deg2Rad + w) * 10f, Mathf.Sin(res * Mathf.Deg2Rad + w) * 10f, 0);
        //        Vector3 p2 = new Vector3(Mathf.Cos(res * Mathf.Deg2Rad - w) * 10f, Mathf.Sin(res * Mathf.Deg2Rad - w) * 10f, 0);
        //        table[positionCount] = p;
        //                            table[positionCount + 1] = p2;
        //                            //
        //                            x = Mathf.Atan2(lineRenderer.GetPosition(positionCount - 1).y, lineRenderer.GetPosition(positionCount - 1).x) * Mathf.Rad2Deg;
        //                            res = x >= 0 ? x : x + 360;
        //                            p = new Vector3(Mathf.Cos(res* Mathf.Deg2Rad + w) * 10f , Mathf.Sin(res* Mathf.Deg2Rad + w) * 10f, 0);
        //                            p2 = new Vector3(Mathf.Cos(res* Mathf.Deg2Rad - w) * 10f , Mathf.Sin(res* Mathf.Deg2Rad - w) * 10f, 0);
        //                            table[positionCount + 2] = p;
        //                            table[positionCount + 3] = p2;


        //                            Vector2 b = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
        //                            Vector2 a = lineRenderer.GetPosition(lineRenderer.positionCount - 2);
        //
        //                            Vector2 aPrim = new Vector2((b.x - a.x) + b.x, (b.y - a.y) + b.y);
        //                            Vector2 rPrim = new Vector2((a.x*5)/6, (a.y * 5) / 6);
        //                            Vector2 sPrim = new Vector2((aPrim.x+rPrim.x)*3/5,(aPrim.y+rPrim.y)*3/5);
        //
        //                            table = new Vector3[lineRenderer.positionCount + 2];
        //                           // lineRenderer.GetPositions(table);
        //                            for (int k = 1; k < lineRenderer.positionCount +1; k++)
        //                            {
        //                                table[k] = lineRenderer.GetPosition(k - 1);
        //                            }
        //                            table[lineRenderer.positionCount + 1] = sPrim;
        //
        //
        //                           b = lineRenderer.GetPosition(0);
        //                           a = lineRenderer.GetPosition(1);
        //
        //                           aPrim = new Vector2((b.x - a.x) + b.x, (b.y - a.y) + b.y);
        //                           rPrim = new Vector2((a.x * 5) / 6, (a.y * 5) / 6);
        //                           sPrim = new Vector2((aPrim.x + rPrim.x)*3 / 5, (aPrim.y + rPrim.y)*3 / 5);
        //                            table[0] = sPrim;
        private List<GameObject> _rbToKill;
        private List<Vector3> _positionsLineRenderer;

        private void RoundBarrierIntersection(GameObject[] roundBarrierGameObjects)
        {
            for (int h = 0; h < _firePointPositions.Count; h++)
            {
                Vector2 forwardFirePointPosition = ReturnForwardPoint(new Vector2(_firePointPositions[h].x, _firePointPositions[h].y));
                foreach (var roundBarrier in roundBarrierGameObjects)
                {
                    for (int i = 0; i < roundBarrier.transform.childCount; i++)
                    {
                        for (int j = 0; j < roundBarrier.transform.GetChild(i).childCount; j++)
                        {
                            LineRenderer lineRenderer = roundBarrier.transform.GetChild(i).GetChild(j).GetComponent<LineRenderer>();
                            for (int k = 0; k < lineRenderer.positionCount - 1; k++)
                            {
                                if (LineIntersection.FasterLineSegmentIntersection(Vector3.zero, forwardFirePointPosition, lineRenderer.GetPosition(k), lineRenderer.GetPosition(k + 1)))
                                {
                                    _rbToKill.Add(roundBarrier.transform.GetChild(i).GetChild(j).gameObject);
                                    _positionsLineRenderer.Add(lineRenderer.GetPosition(k));
                                }
                            }
                        }
                    }
                }
                if (!_gun.IsOverloaded)
                {
                    
                    if (_rbToKill.Count > 1)
                    {
                        float distance = 1000;
                        int rbToKill = 0;
                        for (int i = 0; i < _rbToKill.Count; i++)
                        {
                            float courrent = Vector3.Distance(_positionsLineRenderer[i], Vector3.zero);
                            if (courrent < distance)
                            {
                                distance = courrent;
                                rbToKill = i;                               
                            }
                        }
                        if (_rbToKill[rbToKill].transform.parent.childCount <= 1)
                        {
                            _rbToKill[rbToKill].transform.parent.parent.GetComponent<RoundBarrierDraw>().IsDead = 10;
                        }
                        else
                        {
                            _rbToKill[rbToKill].transform.parent.parent.GetComponent<RoundBarrierDraw>().IsDead++;
                        }                       
                            _gm.KillRoundBarrier(_rbToKill[rbToKill]);
                            SetPositionLaserRenderer(distance, forwardFirePointPosition);
                    }
                    else if (_rbToKill.Count == 1)
                    {
                        if (_rbToKill[0].transform.parent.childCount <= 1)
                        {
                            _rbToKill[0].transform.parent.parent.GetComponent<RoundBarrierDraw>().IsDead = 10;
                        }
                        else
                        {
                            _rbToKill[0].transform.parent.parent.GetComponent<RoundBarrierDraw>().IsDead++;
                        }
                        _gm.KillRoundBarrier(_rbToKill[0]);
                            SetPositionLaserRenderer(Vector3.Distance(_positionsLineRenderer[0], Vector3.zero), forwardFirePointPosition);
                       // Debug.Log("childCount[0] <= 1");
                    }
                    else
                    {
                        SetPositionLaserRenderer(50f, forwardFirePointPosition);
                        _gun.Miss();
                    }
                }
                _positionsLineRenderer.Clear();
                _rbToKill.Clear();
            }
        }

        private int _gatlingIteration = 0;
        private void SetPositionLaserRenderer(float distance, Vector2 firePointPosition)
        {
            StartCoroutine(ShootEffect());

            Vector3 hit = new Vector3(firePointPosition.x*(distance/firePointPosition.magnitude), firePointPosition.y*(distance/ firePointPosition.magnitude));
            Vector3 center = new Vector3(hit.x/2, hit.y/2);
            Vector3 hit2To5 = new Vector3(center.x/2,center.y/2);
            Vector3 hit4To5 = new Vector3((center.x+hit.x)/2,(center.y+hit.y)/2);
            _laserRenderers[_gatlingIteration].SetPosition(0, Vector2.zero);
            _laserRenderers[_gatlingIteration].SetPosition(1, hit2To5);
            _laserRenderers[_gatlingIteration].SetPosition(2, center);
            _laserRenderers[_gatlingIteration].SetPosition(3, hit4To5);
            _laserRenderers[_gatlingIteration].SetPosition(4, hit);
            _laserRenderers[_gatlingIteration].colorGradient = LaserGradient;
            _gm.ShootEffect(_laserRenderers[_gatlingIteration]);
            _gatlingIteration++;
            if (_gatlingIteration == _laserRenderers.Length)
                _gatlingIteration = 0;
        }

        private IEnumerator ShootEffect()
        {
            float elapsedTime = 0.0f;
            float totalTime = 0.07f;
            while (elapsedTime < totalTime)
            {
                elapsedTime += Time.deltaTime;
                float courentTime = elapsedTime / totalTime;
                float animation = Mathf.Lerp(_startShootEffect, _endShootEffect, courentTime);
                _shootEffect.transform.localScale = new Vector3(animation, _shootEffect.transform.localScale.y, animation);
                yield return null;
            }
            _shootEffect.transform.localScale = new Vector3(_startShootEffect, _shootEffect.transform.localScale.y, _startShootEffect);
        }
    }
}