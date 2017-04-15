using UnityEngine;

namespace Assets.Scripts
{
    [ExecuteInEditMode]
    public class CameraImageEffect : MonoBehaviour{

        public Material EffectMaterial;

        void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            Graphics.Blit(src, dst, EffectMaterial);
        }

    }
}
