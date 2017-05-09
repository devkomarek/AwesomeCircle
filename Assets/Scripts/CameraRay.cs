using System.Collections.Generic;
using Assets.Scripts.GameMaster;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraRay : MonoBehaviour{
        private Camera _cam;
        private GM _gm;

        public List<Vector3> PointsList { get; set; }

        void Start ()
        {
            _cam = GetComponent<Camera>();
            PointsList = new List<Vector3>();
            _gm = GameObject.Find("Awesome Circle").transform.FindChild("Game Master").GetComponent<GM>();

        }
	
        // Update is called once per frame
        void Update () {

            if (Input.touchCount != 0 && _gm.IsLvlPlay)
            {
                foreach (var touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        Ray ray = _cam.ScreenPointToRay(touch.position);
                        PointsList.Add(ray.GetPoint(0));

                    }
                }
            }
        }
    }
}
