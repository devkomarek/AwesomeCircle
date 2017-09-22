using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class MuterAudio : MonoBehaviour
    {
        public AudioController AudioController;
        public Image[] ImageVoulme;
        private bool _muteAudio = false;
        public void InvertVolume()
        {

            foreach (var image in ImageVoulme)
            {
                image.enabled = !image.enabled;
            }
            _muteAudio = !_muteAudio;

            AudioController.ButtonSmallMuterPLay(_muteAudio);
        }

    }
}
