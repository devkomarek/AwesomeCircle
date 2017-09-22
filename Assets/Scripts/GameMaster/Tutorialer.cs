using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ScriptableObject;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts.GameMaster
{
    public class Tutorialer : MonoBehaviour
    {
        private static string TRY_AGAIN = "Try Again";
        private static string AWESOME = "Awesome";
        public Animator BottomSlideBarAnimator;
        public Animator LeftSlideBarAnimator;
        private Animator _blackScreenAnimator;
        public Animator AwesomeCircleAnimator;
        public Text SlideBarText;
        public bool IsTutorial;
        private RoundBarrierGenerator _roundBarrierGenerator;
        private AudioController _audioController;
        private Hero _hero;
        private Gun _gun;
        private Timer _timer;
        private Randomizer _randomizer;
        private LvlManager _lvlManager;
        private CameraRay _cameraRay;
        private int _step;


        private void Awake()
        {
            _blackScreenAnimator = GameObject.Find("Awesome Circle").transform.Find("UI").Find("Tutorial").Find("Black_screen_tutorial").GetComponent<Animator>();
            _timer = GameObject.Find("Awesome Circle").transform.Find("UI").gameObject.GetComponent<Timer>();
            _cameraRay = GameObject.Find("Awesome Circle").transform.Find("Main Camera").GetComponent<CameraRay>();
            _roundBarrierGenerator = GetComponent<RoundBarrierGenerator>();
            _audioController = GameObject.Find("Awesome Circle").transform.Find("Audio").GetComponent<AudioController>();
            _hero = GameObject.Find("Awesome Circle").transform.Find("Hero").GetComponent<Hero>();
            _gun = GameObject.Find("Awesome Circle").transform.Find("Hero").GetComponent<Gun>();
            _lvlManager = GetComponent<LvlManager>();
            _randomizer = GetComponent<Randomizer>();
        }

        private bool _isRightSlideBarShow;
        private int _countRoundbarriers;
        private void Update()
        {
            if (IsTutorial)
            {
                _countRoundbarriers = GameObject.FindGameObjectsWithTag("RoundBarrier").Length;
                if (_step == -1 && _isRightSlideBarShow == false && _gun.IsOverloaded)
                {
                    _isRightSlideBarShow = true;
                    SlideBarText.text = TRY_AGAIN;
                    LeftSlideBarAnimator.SetTrigger("Show");
                }
                if (_gun.IsOverloaded == false)
                {
                    _isRightSlideBarShow = false;
                }
                    
                if (_step == -1 && _countRoundbarriers == 0)
                {
                    _step = 0;
                    SlideBarText.text = AWESOME;
                    LeftSlideBarAnimator.SetTrigger("Show");
                    Step2();
                }
                if (_step == -2 && _countRoundbarriers == 0)
                {
                    _step = 0;
                    SlideBarText.text = AWESOME;
                    LeftSlideBarAnimator.SetTrigger("Show");
                    StartCoroutine(NextSlide());
                }

            }
        }

        IEnumerator NextSlide()
        {
            yield return new WaitForSeconds(1f);
            BottomSlideBarAnimator.SetTrigger("Show");
            yield return new WaitForSeconds(5f);
            BottomSlideBarAnimator.SetTrigger("Show");
            yield return new WaitForSeconds(2f);
            BottomSlideBarAnimator.SetTrigger("Show");
            yield return new WaitForSeconds(3.5f);
            BottomSlideBarAnimator.SetTrigger("Show");
            AwesomeCircleAnimator.SetTrigger("Tutorial");
            _lvlManager.SetUpLvl(1);
            _audioController.Play();
            _randomizer.SetUpWaveDatabase(1);
            _timer.ResetTime();
            _timer.MinusTime = Time.time;
            _timer.enabled = true;
            IsTutorial = false;
            enabled = false;
        }
        public void StartTutorial()
        {
            IsTutorial = true;
            AwesomeCircleAnimator.SetTrigger("Tutorial");
            _audioController.Pause();
            Step1();           
        }

        private bool _isTryAgain;
        public void TryAgain()
        {
            if(_isTryAgain) return;
            _cameraRay.PointsList.Clear();
            _isTryAgain = true;
            _step = 0;
            _hero.enabled = false;
            _roundBarrierGenerator.Speed = 0;
            _roundBarrierGenerator.BasicsSpeed = 0;
            SlideBarText.text = TRY_AGAIN;
            LeftSlideBarAnimator.SetTrigger("Show");
            StartCoroutine(Reset());
        }

        private IEnumerator Reset()
        {
            yield return new WaitForSeconds(2f);
            _blackScreenAnimator.SetTrigger("Show");
            yield return new WaitForSeconds(0.5f);
            foreach (var roundBarrier in GameObject.FindGameObjectsWithTag("RoundBarrier"))
            {
                Destroy(roundBarrier);
            }
            _isTryAgain = false;
            Step2Again();
        }

        private IEnumerator WaitBetweenBarrier()
        {
            yield return new WaitForSeconds(0.7f);
            List<Segment> listSegment = new List<Segment>();
            listSegment.Add(new Segment() { Start = 0, End = 90, DotConcentration = 40 });
            _roundBarrierGenerator.CreateRoundBarrier(new RoundBarrier()
            {
                RoundBarrierName = "tutorial",
                SegmentsList = listSegment
            }, 0);
            yield return new WaitForSeconds(1f);
            _roundBarrierGenerator.CreateRoundBarrier(new RoundBarrier()
            {
                RoundBarrierName = "tutorial",
                SegmentsList = listSegment
            },90);
            yield return new WaitForSeconds(1f);
            _roundBarrierGenerator.CreateRoundBarrier(new RoundBarrier()
            {
                RoundBarrierName = "tutorial",
                SegmentsList = listSegment
            }, 180);
            yield return new WaitForSeconds(1f);
            _roundBarrierGenerator.CreateRoundBarrier(new RoundBarrier()
            {
                RoundBarrierName = "tutorial",
                SegmentsList = listSegment
            }, 270);
            yield return new WaitForSeconds(1f);
            _roundBarrierGenerator.CreateRoundBarrier(new RoundBarrier()
            {
                RoundBarrierName = "tutorial",
                SegmentsList = listSegment
            }, 360);
            _step = -2;
        }

        private void Step1()
        {
            _step = 1;
            _roundBarrierGenerator.StartPosition = 10;
            _roundBarrierGenerator.Speed = 0;
            _roundBarrierGenerator.BasicsSpeed = 0;
            List<Segment> listSegment = new List<Segment>();
            listSegment.Add(new Segment() {Start = 0,End = 90,DotConcentration = 40});
            _roundBarrierGenerator.CreateRoundBarrier(new RoundBarrier()
            {
                RoundBarrierName = "tutorial",
                SegmentsList = listSegment
            }, 0);
            BottomSlideBarAnimator.SetTrigger("Show");
            _step = -1;
        }

        private void Step2Again()
        {
            _step = 2;
            _cameraRay.PointsList.Clear();
            _gun.Ammo = 0.42f;
            _hero.enabled = true;
            _roundBarrierGenerator.StartPosition = 20.3f;
            _roundBarrierGenerator.Speed = 5;
            _roundBarrierGenerator.BasicsSpeed = 5;
            StartCoroutine(WaitBetweenBarrier());

        }

        private void Step2()
        {
            _step = 2;
            _cameraRay.PointsList.Clear();
            _gun.Ammo = 0.42f;
            _hero.enabled = true;
            _roundBarrierGenerator.StartPosition = 25.3f;
            _roundBarrierGenerator.Speed = 5;
            _roundBarrierGenerator.BasicsSpeed = 5;
            BottomSlideBarAnimator.SetTrigger("Show");
            StartCoroutine(WaitBetweenBarrier());

        }
    }
}
