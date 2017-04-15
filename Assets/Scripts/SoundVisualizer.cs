using UnityEngine;

namespace Assets.Scripts
{
    public class SoundVisualizer : MonoBehaviour
    {
        private const int SAMPLE_SIZE = 512;
        public float RmsValue;
        public float DbValue;
        public float RmsDivided = 0.1f;
        public float VisualIncreaser = 50;
        public float SmoothSpeed = 10.0f;
        public float MaxVisualScale = 25.0f;
        public float KeepPercentage = 0.1f;
        public float BackgroundIntensity;
        public Material BackgorundMaterial;
        public Color MinColr;
        public Color MaxColor;
        public float SmoothLight = 0.5f;


        private AudioSource _source;
        private AudioController _audioController;
        private float[] _samples;
        private float[] _spectrum;
        private Transform[] _visualTable;
        private float[] _visualScale;
        private int _amnVisual = 20;
        void Start ()
        {
            _audioController = GetComponent<AudioController>();
            _source = _audioController.CourentSource;
            _samples = new float[SAMPLE_SIZE];
            _spectrum = new float[SAMPLE_SIZE];

            // SpawnLine();
            SpawnCircle();
        }

        private void SpawnLine()
        {
            _visualScale = new float[_amnVisual];
            _visualTable = new Transform[_amnVisual];

            for (int i = 0; i < _amnVisual; i++)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                _visualTable[i] = go.transform;
                _visualTable[i].position = new Vector3(-10,0,0);
                _visualTable[i].position += Vector3.right*i;
            
            }
        }

        private void SpawnCircle()
        {
            GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
            _visualScale = new float[_amnVisual];
            _visualTable = new Transform[_amnVisual];

            for (int i = 0; i < _amnVisual; i++)
            {
                GameObject go = cubes[i];
                _visualTable[i] = go.transform;
            }

        }


//    GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
//    float angleModifier = 1f;
//    float radius = 9.2f;
//    _visualScale = new float[_amnVisual];
//        _visualTable = new Transform[_amnVisual];
//        Vector3 center = Vector3.zero;
//
//        for (int i = 0; i<_amnVisual; i++)
//        {
//            float ang = i * angleModifier / _amnVisual;
//    ang = ang* Mathf.PI*2;
//
//    float x = center.x + Mathf.Cos(ang) * radius;
//    float y = center.y + Mathf.Sin(ang) * radius;
//
//    Vector3 pos = center + new Vector3(x, y, 0);
//    GameObject go = cubes[i];
//    go.transform.position = pos;
//            go.transform.rotation = Quaternion.LookRotation(Vector3.forward, pos);
//            _visualTable[i] = go.transform;
//        }

        void Update () {
            AnalyzeSound();
            UpdateVisual();
            UpdateBackground();
        }

        private void UpdateBackground()
        {
            BackgroundIntensity -= Time.deltaTime * SmoothLight;
            if (BackgroundIntensity < DbValue/40)
                BackgroundIntensity = DbValue/40;

            BackgorundMaterial.color = Color.Lerp(MaxColor, MinColr, -BackgroundIntensity);
        }

        private void UpdateVisual()
        {
            int visualIndex = 0;
            int spectrumIndex = 0;
            int averageSize = (int)(SAMPLE_SIZE * KeepPercentage/_amnVisual);

            while (visualIndex < _amnVisual)
            {
                int j = 0;
                float sum = 0;
                while (j<averageSize)
                {
                    sum += _spectrum[spectrumIndex];
                    spectrumIndex++;
                    j++;
                }

                float scaleY = sum/averageSize*VisualIncreaser;
                _visualScale[visualIndex] -= Time.deltaTime*SmoothSpeed;
                if (_visualScale[visualIndex] < scaleY)
                    _visualScale[visualIndex] = scaleY;
                if (_visualScale[visualIndex] > MaxVisualScale)
                    _visualScale[visualIndex] = MaxVisualScale;

                _visualTable[visualIndex].localScale = Vector3.one + Vector3.up*_visualScale[visualIndex];
                visualIndex++;
            }
        }

        public void StartSoundVisual()
        {
            _source = _audioController.CourentSource;
        }

        private void AnalyzeSound()
        {
            if(_source == null || _source.isPlaying == false) return;
            _source.GetOutputData(_samples, 0);

            //RMS
            int i = 0;
            float sum = 0;
            for (;  i< SAMPLE_SIZE; i++)
            {
                sum += _samples[i]*_samples[i];
            }
            RmsValue = Mathf.Sqrt(sum/SAMPLE_SIZE);

            //DB
            DbValue = 20*Mathf.Log10(RmsValue/RmsDivided);

            //Spectrum
            _source.GetSpectrumData(_spectrum, 0, FFTWindow.Blackman);
        }
    }
}
