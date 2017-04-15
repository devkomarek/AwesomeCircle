using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraRay : MonoBehaviour{
        private Camera _cam;

        public List<Vector3> PointsList { get; set; }

        void Start ()
        {
            _cam = GetComponent<Camera>();
            PointsList = new List<Vector3>();

        }
	
        // Update is called once per frame
        void Update () {
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
