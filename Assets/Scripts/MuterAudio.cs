using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class MuterAudio : MonoBehaviour
    {
        public AudioListener AudioListener;
        public Image[] ImageVoulme;   
        public void InvertVolume()
        {
            foreach (var image in ImageVoulme)
            {
                image.enabled = !image.enabled;
            }
            AudioListener.enabled = !AudioListener.enabled;
        }
    }
}
