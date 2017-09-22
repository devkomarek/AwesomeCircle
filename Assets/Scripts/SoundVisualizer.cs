using Assets.Scripts.GameMaster;
using UnityEngine;

namespace Assets.Scripts
{
    public class SoundVisualizer : MonoBehaviour
    {
        private const int SAMPLE_SIZE = 512;
        public float RmsValue;
        public float DbValue;
        public float PitchValue;
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
        public GameObject Hero;
        public RoundBarrierGenerator SpeedBarrier;
        public float MinSpeedBarrier;
        public float MaxSpeedBarrier;
        public float MinSizeHero;
        public float MaxSizeHero;
        public int Band;

        public Light LightGame;
        public float MaxIntensity;
        public float MinIntensity;
        

        private AudioSource _source;
        private AudioController _audioController;
        private float[] _samples;
        private float[] _spectrum;
        private Transform[] _visualTable;
        private float[] _visualScale;
        private int _amnVisual = 20;
        private GameObject[] _cubes;
        private float _audioProfile = 0.8f;
        private float[] _freqBand = new float[8];
        private float[] _bandBuffer = new float[8];
        private float[] _bufferDecrease = new float[8];
        private float[] _freqBandHighest = new float[8];

        public float[] _audioBand = new float[8];
        private float[] _audioBandBuffer = new float[8];
        void Start ()
        {
            _audioController = GetComponent<AudioController>();
            _source = _audioController.CourentSource;
            _samples = new float[SAMPLE_SIZE];
            _spectrum = new float[SAMPLE_SIZE];
            SpawnCircle();
            AudioProfile();
        }

        private void SpawnCircle()
        {
            _cubes = GameObject.FindGameObjectsWithTag("Cube");
            _visualScale = new float[_amnVisual];
            _visualTable = new Transform[_amnVisual];

            for (int i = 0; i < _amnVisual; i++)
            {
                GameObject go = _cubes[i];
                _visualTable[i] = go.transform;
            }

        }

        void Update () {
            AnalyzeSound();
            UpdateVisual();
            UpdateBackground();
            MakeFrequencyBands();
            BandBuffer();
            CreateAudioBands();
            UpdateHero();
            UpdateLight();
            UpdateBarriersSpeed();
            UpdateWidthBarriers();
        }

        private void UpdateWidthBarriers()
        {
            float a = (_audioBandBuffer[Band] * 1.2f) + 1.2f;
            SpeedBarrier.Width = a;
        }

        private void UpdateHero()
        {
            Hero.transform.localScale = new Vector3((_audioBandBuffer[Band] * MaxSizeHero) + MinSizeHero, transform.localScale.y, (_audioBandBuffer[Band] * MaxSizeHero) + MinSizeHero);
        }

        private void UpdateBarriersSpeed()
        {
            float a = (_audioBandBuffer[Band]*SpeedBarrier.BasicsSpeed/4) + SpeedBarrier.BasicsSpeed;
            if (SpeedBarrier.BasicsSpeed + 3 < a)
            {
                SpeedBarrier.Speed = SpeedBarrier.BasicsSpeed + 3;
            }
            else
                SpeedBarrier.Speed = a;
        }

        private void UpdateLight()
        {
            LightGame.intensity = (_audioBandBuffer[Band] * (MaxIntensity - MinIntensity)) + MinIntensity;
        }

        void AudioProfile()
        {
            for (int i = 0; i < 8; i++)
            {
                _freqBandHighest[i] = _audioProfile;
            }
        }

        void CreateAudioBands()
        {
            for (int i = 0; i < 8; i++)
            {
                if (_freqBand[i] > _freqBandHighest[i])
                {
                    _freqBandHighest[i] = _freqBand[i];
                }
                _audioBand[i] = (_freqBand[i] / _freqBandHighest[i]);
                _audioBandBuffer[i] = (_bandBuffer[i] / _freqBandHighest[i]);
            }
        }

        void MakeFrequencyBands()
        {
            int count = 0;
            for (int i = 0; i < 8; i++)
            {
                float average = 0;
                int sampleCount = (int)Mathf.Pow(2, i) * 2;

                if (i == 0)
                {
                    sampleCount -= 1;
                }
                if (i == 7)
                {
                    sampleCount += 2;
                }
                for (int j = 0; j < sampleCount; j++)
                {
                    average += _spectrum[count] * (count + 1);
                    count++;
                }
                average /= count;
                _freqBand[i] = average * 10;
            }
        }

        void BandBuffer()
        {
            for (int g = 0; g < 8; ++g)
            {
                if (_freqBand[g] > _bandBuffer[g])
                {
                    _bandBuffer[g] = _freqBand[g];
                    _bufferDecrease[g] = 0.005f;
                }
                if (_freqBand[g] < _bandBuffer[g])
                {
                    _bandBuffer[g] -= _bufferDecrease[g];
                    _bufferDecrease[g] *= 1.3f;
                }
            }
        }

        private void UpdateBackground()
        {
            BackgroundIntensity -= Time.deltaTime * SmoothLight;
            if (BackgroundIntensity < DbValue/40)
                BackgroundIntensity = DbValue/40;

            BackgorundMaterial.color = Color.Lerp(MaxColor, MinColr, BackgroundIntensity);
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

            _source.GetSpectrumData(_spectrum, 0, FFTWindow.Hanning);
        }

//        public void TheEnd()
//        {
//            foreach (var cube in _cubes)
//            {
//                Vector3 position = cube.transform.localPosition;
//                position.z = 40;
//                cube.transform.localPosition = Vector3.Slerp(cube.transform.localPosition, position, 1);
//            }            
//        }
//
//        public void ContinueAfterTheEnd()
//        {
//            GameObject cubes = GameObject.FindGameObjectWithTag("Cubes");
//            Vector3 position = cubes.transform.localPosition;
//            position.y = -39123f;
//            cubes.transform.localPosition = Vector3.Slerp(cubes.transform.localPosition, position, 1);
//        }
    }
}
