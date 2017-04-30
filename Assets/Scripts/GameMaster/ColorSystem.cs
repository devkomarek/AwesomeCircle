using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameMaster
{
    public class ColorSystem : MonoBehaviour
    {

        public Color CurrentColor;
        public Image[] Images;
        public Text[] Texts;
        private Gradient _laserGradient;
        private Light _light;
        private Gradient _gradient;
        
        // Use this for initialization
        void Start ()
        {
            _light = GameObject.Find("Awesome Circle").transform.FindChild("Plane").FindChild("Point light").GetComponent<Light>();
            _laserGradient = GameObject.Find("Awesome Circle").transform.FindChild("Hero").GetComponent<Hero>().LaserGradient;
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
            foreach (var image in Images)
            {
                image.color = CurrentColor;
            }
            foreach (var text in Texts)
            {
                text.color = CurrentColor;
            }
            Gradient(_gradient);
            Gradient(_laserGradient);
        }

        private void Gradient(Gradient g)
        {            
            GradientColorKey[] gck = g.colorKeys;
            gck[0].color = CurrentColor;
            gck[0].time = 0.0F;
            gck[1].color = CurrentColor;
            gck[1].time = 1F;
            g.colorKeys = gck;
        }
    }
}
