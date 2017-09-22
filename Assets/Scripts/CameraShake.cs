using Assets.Scripts.GameMaster;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraShake : MonoBehaviour
    {
        public Transform camTransform;
        public GM GameMaster;
        public float shakeDuration = 0f;	
        public float shakeAmount = 0.7f;
        public float decreaseFactor = 1.0f;
	
        Vector3 originalPos;
	
        void Awake()
        {
            if (camTransform == null)
            {
                camTransform = GetComponent(typeof(Transform)) as Transform;
            }
        }
	
        void OnEnable()
        {
            originalPos = camTransform.localPosition;
        }

        void Update()
        {
            if (shakeDuration > 0)
            {
                camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;			
                shakeDuration -= Time.deltaTime * decreaseFactor;
                if (GameMaster.IsLvlPlay == false)
                {
                    shakeDuration = 0f;
                    camTransform.localPosition = originalPos;
                }
            }
            else
            {
                shakeDuration = 0f;
                camTransform.localPosition = originalPos;
            }
        }
    }
}
