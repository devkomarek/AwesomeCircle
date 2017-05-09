using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Assets.Scripts.ScriptableObject.Database;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameMaster{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class GM : MonoBehaviour
    {
        private const string LVL_IN_CIRCLE = "LEVEL ";

        public bool IsLvlPlay;
        public static GM Gm;
        public static WaitForSeconds ShootDuration = new WaitForSeconds(.07f);
        public bool TestMode = false;
        private static bool _gameHasEnded = false;
        public Animator AwesomeCircleAnimator;
        public Animator LightAnimator;
        public Animator BlackScreenAnimator;

        private AudioController _audioController;
        private Text _lvlText;
        private GameObject _smallCircle1;
        private GameObject _smallCircle2;
        private GameObject _bigCircle;
        private GameInfo _gameInfo;
        private Randomizer _randomizer;
        private LvlManager _lvlManager;
        private Timer _timer;
        private Text _timerText;
        private GameObject _overloadGun;
        private Hero _hero;
        private GameObject _heroGameObject;
        private Text _best;
        private bool _smallCircle1InTop = true;
        private int _previousState = 0;
        private WaveDatabase _waveDatabase;
        private Image _lockImage;
        private Animator _nextLvlAnimator;
        private Text _nextLvlText;
        private CameraRay _cameraRay;
        private Animator _animatorAwesome;
        private bool _gameStart = false;
        private CameraImageEffect _cameraImageEffect;
        private GameObject _bassCylinder;
        private CameraShake _cameraShake;
        private GameObject _mainCamera;
        private UnityAds _unityAds;
        private UnityAnalytics _unityAnalytics;

        private void Start()
        {
            GameObject awesomeCircle = GameObject.Find("Awesome Circle");
            GameObject ui = awesomeCircle.transform.FindChild("UI").gameObject;
            _unityAds = GetComponent<UnityAds>();
            _unityAnalytics = GetComponent<UnityAnalytics>();
            _mainCamera = awesomeCircle.transform.FindChild("Main Camera").gameObject;
            _bassCylinder = awesomeCircle.transform.FindChild("Hero").FindChild("Bass cylinder").gameObject;
            _cameraImageEffect = awesomeCircle.transform.FindChild("Main Camera").GetComponent<CameraImageEffect>();
            _nextLvlAnimator = ui.transform.FindChild("Next_lvl").GetComponent<Animator>();
            _nextLvlText = ui.transform.FindChild("Next_lvl").FindChild("Text").GetComponent<Text>();
            _audioController = awesomeCircle.transform.FindChild("Audio").GetComponent<AudioController>();
            _lvlText = ui.transform.FindChild("Lvl").FindChild("Text").GetComponent<Text>();
            _bigCircle = ui.transform.FindChild("Big_circle").gameObject;
            _smallCircle1 = _bigCircle.transform.FindChild("Small_circle").gameObject;
            _smallCircle2 = _bigCircle.transform.FindChild("Small_circle (1)").gameObject;
            _timerText = ui.transform.FindChild("Timer").transform.FindChild("Text").gameObject.GetComponent<Text>();
            _gameInfo = GetComponent<GameInfo>();
            _randomizer = GetComponent<Randomizer>();
            _lvlManager = GetComponent<LvlManager>();
            _timer = ui.gameObject.GetComponent<Timer>();
            _hero = awesomeCircle.transform.FindChild("Hero").GetComponent<Hero>();
            _best = ui.gameObject.transform.Find("Best").GetComponentInChildren<Text>();
            _heroGameObject = awesomeCircle.transform.FindChild("Hero").gameObject;
            _cameraRay = awesomeCircle.transform.FindChild("Main Camera").GetComponent<CameraRay>();
            _animatorAwesome = ui.transform.FindChild("Awesome").GetComponent<Animator>();
            _cameraShake = GetComponent<CameraShake>();
            _heroGameObject.SetActive(false);
            _best.text = GameManager.GetTimeBest(1);
        }

        private bool _isFirstAwesome = true;
        private void Update()
        {
            if(_gameInfo.Lvl == 5 && _timer.Seconds >= 60 && GameManager.IsEnd() == false)
                TheEnd();
            if (_timer.Seconds >= 60 && _isFirstAwesome && _gameStart)
            {
                _animatorAwesome.SetTrigger("Show");
                _isFirstAwesome = false;
            }
          //  Debug.Log(_isSwipe);
        }

        public void KillRoundBarrier(GameObject rb)
        {
            if (rb.transform.parent.childCount <= 1)
            {
                StartCoroutine(AnimateDeadBarrier(rb.GetComponent<LineRenderer>(), rb.transform.parent.parent.gameObject));
                rb.transform.parent.parent.gameObject.tag = "Dead";
            }
            else
            {
                StartCoroutine(AnimateDeadBarrier(rb.GetComponent<LineRenderer>(), rb));
                if (rb.transform.parent.childCount <= 1)
                {
                    StartCoroutine(AnimateDeadBarrier(rb.GetComponent<LineRenderer>(), rb.transform.parent.parent.gameObject));
                    rb.transform.parent.parent.gameObject.tag = "Dead";
                }

            }                
        }

        private IEnumerator AnimateDeadBarrier(LineRenderer lr, GameObject gO)
        {
            Color startColor = lr.endColor;
            Color endColor = startColor.linear;
            float elapsedTime = 0.0f;
            float totalTime = 0.05f;
            while (elapsedTime < totalTime)
            {
                elapsedTime += Time.deltaTime;
                float courentTime = elapsedTime/totalTime;
                lr.startColor = Color.Lerp(startColor, endColor, courentTime);
                lr.endColor = lr.startColor;
                yield return null;
            }
            Destroy(gO);
        }

        public void EndGame()
        {
            if(_gameHasEnded)
                return;
            IsLvlPlay = false;
            _isStart = false;
            _unityAnalytics.Analise();
            _randomizer.Disable();
            _bassCylinder.SetActive(false);
            _gameStart = false;
            _isRestart = false;
            SetActivite(false);
            if(_timer.Seconds >= 15)
            _unityAds.ShowAd();
            _lvlManager.FreezAllRoundBarriers();
            
          SetTriggers("EndGame");
            
            string actualTime = _timerText.text;
           if (GameManager.GetFloatBest(_gameInfo.Lvl) < GameManager.ConvertTimeToFloat(actualTime))
                GameManager.SetBest(_gameInfo.Lvl, actualTime);

            _best.text = GameManager.GetTimeBest(_gameInfo.Lvl);
            _audioController.StopLvlPlay();
        }

        public void StartGame()
        {
            
            if (_isStart == true || _isShowCredit == true || _isSwipe == true || GameManager.ReturnNameLvl(_gameInfo.Lvl).Substring(0,4) == "Lock") return;
            _isStart = true;
            _isFirstAwesome = true;
//            for (int i = 1; i < 6; i++)
//            {
//                if (AwesomeCircleAnimator.GetCurrentAnimatorStateInfo(0).IsName("Lvl " + i + " Menu"))
//                {
                    StartLvl(_gameInfo.Lvl);
//                }
//            }          
        }

        private bool _isStart = false;
        private void StartLvl(int lvl)
        {
            // if() return;         
            Debug.Log("hejo testujemy");
            _isMenu = false;
            StartCoroutine(UnlockHeroGun(0.5f));
            _smallCircle2.transform.FindChild("Lock").GetComponent<Image>().enabled = false;
            _smallCircle1.transform.FindChild("Lock").GetComponent<Image>().enabled = false;
            _bassCylinder.SetActive(true);
            _cameraImageEffect.enabled = true;
            SetTriggers("StartGame");
            
            _nextLvlAnimator.SetTrigger("Hide");
            _gameInfo.LvlName = GameManager.ReturnNameLvl(lvl);
            
            _audioController.StartLvlPlay();
            _gameInfo.Lvl = lvl;
            _timer.MinusTime = Time.time;
            _randomizer.SetUpWaveDatabase(lvl);
            _lvlText.text = _gameInfo.LvlName;
            _lvlManager.UnFreezAllRoundBarriers();
            _lvlManager.SetUpLvl(_gameInfo.Lvl);
            _cameraRay.PointsList.Clear();
            _heroGameObject.SetActive(true);
            _gameStart = true;
            if(TestMode == false)
            SetActivite(true);
        }

        public IEnumerator UnlockHeroGun(float time)
        {          
            yield return new WaitForSeconds(time);
            IsLvlPlay = true;
        }

        private bool _isMenu;
        public void BackToMenu()
        {
           // _unityAnalytics.SendData();
            if (_isMenu == false && _isRestart == false)
            {
                _isMenu = true;
                _cameraImageEffect.enabled = false;
                StartCoroutine(BackEffect());
                _gameInfo.CourentTime = "Time  00:00";
                _timer.ResetTime();
                SetActivite(false);
                _unityAds.Cancel = true;
                _heroGameObject.SetActive(false);
                DestroyAllRoundBarriers();
                _smallCircle1.transform.FindChild("lvl_name").GetComponent<Text>().text = GameManager.ReturnNameLvl(_gameInfo.Lvl);
                _smallCircle1.transform.FindChild("lvl_text").GetComponent<Text>().text = LVL_IN_CIRCLE + _gameInfo.Lvl;
                _smallCircle2.transform.FindChild("lvl_name").GetComponent<Text>().text = GameManager.ReturnNameLvl(_gameInfo.Lvl);
                _smallCircle2.transform.FindChild("lvl_text").GetComponent<Text>().text = LVL_IN_CIRCLE + _gameInfo.Lvl;
                _best.text = GameManager.GetTimeBest(_gameInfo.Lvl);
                GC.Collect();
            }
        }

        private bool _isRestart;
        public void RestartLvl()
        {
            if (_isRestart == false && _isMenu == false)
            {
                GC.Collect();
                _isRestart = true;
                StartCoroutine(UnlockHeroGun(0.25f));
                _unityAds.Cancel = true;
                _bassCylinder.SetActive(true);
                _gameInfo.CourentTime = "Time  00:00";
                _timer.ResetTime();
                SetActivite(true);

                _audioController.RestartLvlPlay();
                AwesomeCircleAnimator.SetTrigger("Lvl_" + _gameInfo.Lvl);
                SetTriggers("RestartLvl");

                DestroyAllRoundBarriers();
                _randomizer.SetUpWaveDatabase(_gameInfo.Lvl);

                _timer.MinusTime = Time.time;
                _lvlManager.UnFreezAllRoundBarriers();
                
            }
            _cameraRay.PointsList.Clear();
        }

        private bool _isShowCredit;
        public void ShowCredit()
        {
            if (_isShowCredit == false && _isStart == false && _isSwipe == false)
            {
                _isShowCredit = true;
                AwesomeCircleAnimator.SetTrigger("ShowCredit");              
            }

        }

        public void HideCredit()
        {           
            if (_isShowCredit)
            {
                _isShowCredit = false;
                AwesomeCircleAnimator.SetTrigger("HideCredit");
                if (_gameInfo.LvlName.Substring(0, 4) == "Lock")
                    AwesomeCircleAnimator.SetTrigger("Lvl_" + _gameInfo.Lvl + "_gray");
                else
                    AwesomeCircleAnimator.SetTrigger("Lvl_" + _gameInfo.Lvl);
            }
        }

        public void ShootEffect(LineRenderer lr)
        {
            StartCoroutine(Effect(lr));
        }

        public IEnumerator Effect(LineRenderer lr)
        {
            lr.enabled = true;
            yield return new WaitForSeconds(.05f);
            lr.enabled = false;
        }

        public IEnumerator BackEffect()
        {
            for (int i = 0; i < 100; i++)
            {
                if (i == 0)
                {
                    if (_gameInfo.Lvl == 2 || _gameInfo.Lvl == 4)
                        SetTriggers("BackToMenuEven");
                    else
                        SetTriggers("BackToMenuOdd");
                }
                yield return new WaitForSeconds(.05f);
                if (AwesomeCircleAnimator.GetCurrentAnimatorStateInfo(0).IsName("Back To Menu Even") ||
                    AwesomeCircleAnimator.GetCurrentAnimatorStateInfo(0).IsName("Back To Menu Odd"))
                {
                    SetTriggers("Lvl_" + _gameInfo.Lvl);
                    i += 100;
                }
            }
        }

        private void SetTriggers(string trigger)
        {
            AwesomeCircleAnimator.SetTrigger(trigger);
            LightAnimator.SetTrigger(trigger);
        }

        private void SetActivite(bool b)
        {
            _randomizer.enabled = b;
            _timer.enabled = b;
            _gameHasEnded = !b;
            _hero.enabled = b;
            
        }

        public void OverloadEffect()
        {
            _audioController.OverloadPlay();
            StartCoroutine(CameraShakeEffect());
        }

        IEnumerator CameraShakeEffect()
        {
            AwesomeCircleAnimator.enabled = false;
            _cameraShake.enabled = true;
            while (_cameraShake.shakeDuration > 0.01)
            {
                yield return null;
            }
            _cameraShake.enabled = false;
            _cameraShake.shakeDuration = 0.7f;
            _mainCamera.transform.position = new Vector3(0,0,-126.6f); //todo do poprawienia
            AwesomeCircleAnimator.enabled = true;
        }



        public void DestroyAllRoundBarriers()
        {
            foreach (var gm in GameObject.FindGameObjectsWithTag("RoundBarrier"))
            {
                Destroy(gm.gameObject);
            }
        }

        private void Swipe(string direction)
        {
            for (int i = 1; i < 6; i++)
            {
                if (AwesomeCircleAnimator.GetCurrentAnimatorStateInfo(0).IsName("Lvl " + i + " Menu") &&
                     _previousState != i ||
                    AwesomeCircleAnimator.GetCurrentAnimatorStateInfo(0).IsName("Lvl " + i +" Gray") &&
                    _previousState != i)
                {
                    Text lvlName;
                    Text lvlText;                   
                    SwipeToGray(i,direction);                
                    
                    if (i > 1 && direction == "Left" || i < 5 && direction == "Right")
                    {
                        if (_smallCircle1InTop == false)
                        {
                            lvlName = _smallCircle1.transform.FindChild("lvl_name").GetComponent<Text>();
                            lvlText = _smallCircle1.transform.FindChild("lvl_text").GetComponent<Text>();
                            _lockImage = _smallCircle1.transform.FindChild("Lock").GetComponent<Image>();
                            _smallCircle1InTop = true;
                        }
                        else
                        {
                            lvlName = _smallCircle2.transform.FindChild("lvl_name").GetComponent<Text>();
                            lvlText = _smallCircle2.transform.FindChild("lvl_text").GetComponent<Text>();
                            _lockImage = _smallCircle2.transform.FindChild("Lock").GetComponent<Image>();
                            _smallCircle1InTop = false;
                        }

                        if (direction == "Left")
                        {
                            _audioController.SwipeLeftPlay();
                            _gameInfo.LvlName = GameManager.ReturnNameLvl(i - 1);
                            _gameInfo.Lvl = i - 1;
                            _nextLvlText.text = ReturnNextLvlAtSeconds(i -1);
                        }

                        if (direction == "Right")
                        {
                            _audioController.SwipeRightPlay();
                            _gameInfo.LvlName = GameManager.ReturnNameLvl(i + 1);
                            _gameInfo.Lvl = i + 1;
                            _nextLvlText.text = ReturnNextLvlAtSeconds(i + 1);
                        }
                        lvlName.text = _gameInfo.LvlName;
                        lvlText.text = LVL_IN_CIRCLE + _gameInfo.Lvl;
                        _best.text = GameManager.GetTimeBest(_gameInfo.Lvl);
                        TurnImageLock();
                        _previousState = i;
                        

                    }

                    return;
                }
            }
        }

        private void TurnImageLock()
        {
            if (_gameInfo.LvlName.Substring(0,4) == "Lock")
            {
                _lockImage.enabled = true;
            }
            else
            {
                _lockImage.enabled = false;
            }
                
        }

        private bool _isSwipe;
        public void SwipeToRight()
        {
            if (_isStart == false && _isShowCredit == false && _isSwipe == false)
            {
                _isSwipe = true;
                StartCoroutine(WaitSwipe());
                Swipe("Right");
            }
        }

        public void SwipeToLeft()
        {
            if (_isStart == false && _isShowCredit == false && _isSwipe == false)
            {
                _isSwipe = true;
                StartCoroutine(WaitSwipe());
                Swipe("Left");
            }
        }

        public IEnumerator WaitSwipe()
        {
            yield return new WaitForSeconds(.3f);
            _isSwipe = false;
        }

        private void SwipeToGray(int lvl, string direction)
        {
            if (direction == "Right")
            {
                if (GameManager.LvlIsLock(lvl + 1))
                {
                    if (lvl + 1 == 6)
                    {
                        AwesomeCircleAnimator.SetTrigger("RightGray");
                        return;
                    }                      
                    else
                        SetTrigerToRightGray();
                }
                else
                {
                    SetTriggers("Swipe" + direction);
                }
            }
            if (direction == "Left")
            {
                if (GameManager.LvlIsLock(lvl - 1))
                {
                    switch (lvl - 1)
                    {
                        case 0:
                            AwesomeCircleAnimator.SetTrigger("Swipe" + direction);
                            break;
                        case 1:
                            if (AwesomeCircleAnimator.GetCurrentAnimatorStateInfo(0).IsName("Lvl " + lvl + " Gray"))
                                SetTriggerToLvl(lvl);
                            else
                            {
                                SetTriggers("Swipe" + direction);
                            }
                            break;
                        default:
                            SetTrigerToLeftGray();
                            break;
                    }
                  
                }
                else
                {
                    if (AwesomeCircleAnimator.GetCurrentAnimatorStateInfo(0).IsName("Lvl " + lvl + " Gray"))
                        SetTriggerToLvl(lvl);
                    else
                    {
                        SetTriggers("Swipe" + direction);
                    }
                }
            }

        }

        private void SetTriggerToLvl(int lvl)
        {
            SetTriggers("Lvl_" + (lvl - 1)); 
            _audioController.UnloackLvlPlay();
        }

        private void SetTrigerToLeftGray()
        {
            AwesomeCircleAnimator.SetTrigger("LeftGray");
            _audioController.LockLvlPlay();
        }

        private void SetTrigerToRightGray()
        {
            SetTriggers("RightGray");
            _audioController.LockLvlPlay();
        }

        private string _previousNextLevelName = "Next level at 10 seconds";
        private string ReturnNextLvlAtSeconds(int lvl)
        {
            if (GameManager.GetFloatBest(lvl) >= 3600)
            {
                _nextLvlAnimator.SetTrigger("Hide");
                return _previousNextLevelName;
            }
            string time = GameManager.GetTimeBest(lvl);
            string ss = time.Substring(time.Length - 5,2);
            int s = int.Parse(ss);
            if(s == 0)
                _nextLvlAnimator.SetTrigger("Hide");
            else
                _nextLvlAnimator.SetTrigger("Show");
            if(lvl == 5)
                _previousNextLevelName = "End Game at " + (60 - s).ToString() + " " + "seconds";
            else
                _previousNextLevelName = "Next level at " + (60 - s).ToString() + " " + "seconds";
            return _previousNextLevelName;
        }


        public void TheEnd()
        {
            GameManager.TheEnd();
            _audioController.EndAppPlay();
            _timer.TheEnd();
            _randomizer.Disable();
            DestroyAllRoundBarriers();
            _lvlManager.FreezAllRoundBarriers();
            GameManager.SetBest(_gameInfo.Lvl, _timerText.text);
            SetTriggers("TheEnd");
        }

        public void BackToTheGameAfterTheEnd()
        {
            BackToMenu();
        }
        public void StartGameScreen()
        {
            BlackScreenAnimator.SetTrigger("Start");
        }
    }
}