using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameMaster
{
    public class ColorSystem : MonoBehaviour
    {

        public Color CurrentColor;
        public Image ButtonImage1;
        public Image ButtonImage2;
        public Image SmallCircleImage1;
        public Image SmallCircleImage2;
        public Image BigCircleImage;
        private Light _light;
        private Gradient _gradient;
        // Use this for initialization
        void Start ()
        {
            _light = GameObject.Find("Awesome Circle").transform.FindChild("Plane").FindChild("Point light").GetComponent<Light>();
            _gradient = GetComponent<RoundBarrierGenerator>().Gradient;
        }
	
        // Update is called once per frame
        void Update ()
        {
            CurrentColor = _light.color;
            UpdateColor();
        }

        private void UpdateColor()
        {
            ButtonImage1.color = CurrentColor;
            ButtonImage2.color = CurrentColor;
            SmallCircleImage1.color = CurrentColor;
            SmallCircleImage2.color = CurrentColor;
            BigCircleImage.color = CurrentColor;
            Gradient(_gradient);
        }

        private void Gradient(Gradient g)
        {
            
            GradientColorKey[] gck = g.colorKeys;
            GradientAlphaKey[] gak = g.alphaKeys;
            gck[0].color = CurrentColor;
            gck[0].time = 0.0F;
            gck[1].color = CurrentColor;
            gck[1].time = 1F;


            gak[0].alpha = 1.0F;
            gak[0].time = 0.0F;
            gak[1].alpha = 1.0F;
            gak[1].time = 1F;
            g.SetKeys(gck, gak);
        }
    }
}
