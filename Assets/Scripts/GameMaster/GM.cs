using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Assets.Scripts.ScriptableObject.Database;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace Assets.Scripts.GameMaster{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class GM : MonoBehaviour
    {
        public GameObject QuestionMarkGameObject;
        private const string LVL_IN_CIRCLE = "LEVEL ";
        public MeshRenderer[] CubeMeshes;
        public MeshRenderer[] AllCubeMeshes;
        public Image NextLvlImage;
        public Text NextLvlText;
        public Text TextHexagon;
        public GameObject PowerButtonGameObject;
        public bool IsLvlPlay;
        public static GM Gm;
        public static WaitForSeconds ShootDuration = new WaitForSeconds(.07f);
        public bool TestMode = false;
        private static bool _gameHasEnded = false;
        public Animator AwesomeCircleAnimator;
        public Animator LightAnimator;
        public Animator BlackScreenAnimator;
        public Animator CameraAnimator;

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
        private GameObject _nextLvl;
        private CameraRay _cameraRay;
        private Animator _animatorAwesome;
        private bool _gameStart = false;
        private GameObject _bassCylinder;
        private CameraShake _cameraShake;
        private GameObject _mainCamera;
        private UnityAds _unityAds;
        private UnityAnalytics _unityAnalytics;
        private Tutorialer _tutorialer;
        private SoundVisualizer _soundVisualizer;

        private void Start()
        {
            GameObject awesomeCircle = GameObject.Find("Awesome Circle");
            GameObject ui = awesomeCircle.transform.Find("UI").gameObject;
            _tutorialer = GetComponent<Tutorialer>();
            _unityAds = GetComponent<UnityAds>();
            _unityAnalytics = GetComponent<UnityAnalytics>();
            _mainCamera = awesomeCircle.transform.Find("Main Camera").gameObject;
            _bassCylinder = awesomeCircle.transform.Find("Hero").Find("Bass cylinder").gameObject;
            _nextLvlAnimator = ui.transform.Find("Next_lvl").GetComponent<Animator>();
            _nextLvl = ui.transform.Find("Next_lvl").gameObject;
            _nextLvlText = ui.transform.Find("Next_lvl").Find("Text").GetComponent<Text>();
            _audioController = awesomeCircle.transform.Find("Audio").GetComponent<AudioController>();
            _soundVisualizer = awesomeCircle.transform.Find("Audio").GetComponent<SoundVisualizer>();
            _lvlText = ui.transform.Find("Lvl").Find("Text").GetComponent<Text>();
            _bigCircle = ui.transform.Find("Big_circle").gameObject;
            _smallCircle1 = _bigCircle.transform.Find("Small_circle").gameObject;
            _smallCircle2 = _bigCircle.transform.Find("Small_circle (1)").gameObject;
            _timerText = ui.transform.Find("Timer").transform.Find("Text").gameObject.GetComponent<Text>();
            _gameInfo = GetComponent<GameInfo>();
            _randomizer = GetComponent<Randomizer>();
            _lvlManager = GetComponent<LvlManager>();
            _timer = ui.gameObject.GetComponent<Timer>();
            _hero = awesomeCircle.transform.Find("Hero").GetComponent<Hero>();
            _best = ui.gameObject.transform.Find("Best").GetComponentInChildren<Text>();
            _heroGameObject = awesomeCircle.transform.Find("Hero").gameObject;
            _cameraRay = awesomeCircle.transform.Find("Main Camera").GetComponent<CameraRay>();
            _animatorAwesome = ui.transform.Find("Awesome").GetComponent<Animator>();
            _cameraShake = GetComponent<CameraShake>();
            _heroGameObject.SetActive(false);           
            _randomNumberStartPlayingVoice = UnityEngine.Random.Range(90, 110);
            TextHexagon.enabled = PlayerPrefs.GetString("Awesome Hexagon Active", "false") == "true";
            IsAwesomeHexagonActive = PlayerPrefs.GetString("Awesome Hexagon Active", "false") == "true";
            _best.text = IsAwesomeHexagonActive ? GameManager.GetTimeHexagon(1) : GameManager.GetTimeBest(1);
            NextLvlImage.enabled = PlayerPrefs.GetString("Awesome Hexagon Active", "false") == "false";
            NextLvlText.enabled = PlayerPrefs.GetString("Awesome Hexagon Active", "false") == "false";
            _gameInfo.LvlName = IsAwesomeHexagonActive ? GameManager.ReturnNameLvlHexagon(1) : GameManager.ReturnNameLvl(1);
            _smallCircle1.transform.Find("lvl_name").GetComponent<Text>().text = _gameInfo.LvlName;
            foreach (var cubeMesh in CubeMeshes)
            {
                cubeMesh.enabled = false;
            }
        }

        private int _randomNumberStartPlayingVoice;
        private bool _isFirstIncredible = true;
        private bool _isFirstAwesome = true;
        private void Update()
        {
            if(_gameInfo.Lvl == 5 && _timer.Seconds >= 60 && GameManager.IsEnd() == false)
                TheEnd();
            if (_timer.Seconds >= 60 && _isFirstAwesome && _gameStart)
            {
                _animatorAwesome.SetTrigger("Show");
                if(!_isEnding)
                _audioController.PlayAwesome60Seconds();
                _isFirstAwesome = false;
            }
            if (_timer.Seconds >= _randomNumberStartPlayingVoice && _isFirstIncredible && _gameStart)
            {
                _audioController.PlayIncredible();
                _isFirstIncredible = false;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ShowDialogMassage();
            }

        }

        public void KillRoundBarrier(GameObject rb)
        {
           // if (rb.transform.parent.parent.GetComponent<RoundBarrierDraw>().IsDead)
          //  {
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
           // }
                     
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
            if (_tutorialer.IsTutorial == true)
            {
                _tutorialer.TryAgain();
                return;
            }
            if (_gameHasEnded)
                return;
            if (_timer.Seconds >= 15)
            {
                _unityAds.ShowAd();
            }
            //_cameraShake.enabled = false;
            IsLvlPlay = false;            
            _isStart = false;            
            _bassCylinder.SetActive(false);
            _gameStart = false;
            _isRestart = false;
            SetActivite(false);
    
            _unityAnalytics.Analise();
            _randomizer.Disable();

            _lvlManager.FreezAllRoundBarriers();
            CameraAnimator.speed = 0;
            SetTriggers("EndGame");
            
            string actualTime = _timerText.text;
            if (!IsAwesomeHexagonActive)
            {
                if (GameManager.GetFloatBest(_gameInfo.Lvl) < GameManager.ConvertTimeToFloat(actualTime))
                    GameManager.SetBest(_gameInfo.Lvl, actualTime);           

                _best.text = GameManager.GetTimeBest(_gameInfo.Lvl);
            }
            else
            {
                if (GameManager.GetFloatHexagon(_gameInfo.Lvl) < GameManager.ConvertTimeToFloat(actualTime))
                    GameManager.SetHexagon(_gameInfo.Lvl, actualTime);

                _best.text = GameManager.GetTimeHexagon(_gameInfo.Lvl);
            }
//            GameManager.SetHexagon(_gameInfo.Lvl, "Time 60:00");
//            GameManager.SetBest(_gameInfo.Lvl, "Time 60:00");
            _soundVisualizer.enabled = false;
            GetComponent<RoundBarrierGenerator>().Width = 1.42f;
            _audioController.StopLvlPlay();
        }

        IEnumerator WaitForGame(bool b)
        {
            yield return new WaitForSeconds(0.3f);
            foreach (var cubeMesh in CubeMeshes)
            {
                cubeMesh.enabled = b;
            }
        }

        public void StartGame()
        {
            if (!IsAwesomeHexagonActive)
            {
                if (_isStart == true || _isShowCredit == true || _isSwipe == true || GameManager.ReturnNameLvl(_gameInfo.Lvl).Substring(0, 4) == "Lock") return;
            }
            else
            {
                if (_isStart == true || _isShowCredit == true || _isSwipe == true || GameManager.ReturnNameLvlHexagon(_gameInfo.Lvl).Substring(0, 4) == "Lock") return;
            }
            IsAwesomeHexagonActive = PlayerPrefs.GetString("Awesome Hexagon Active", "false") == "true";
            _soundVisualizer.enabled = true;
            _isStart = true;
            _isFirstAwesome = true;
            _isFirstIncredible = true;           

            StartLvl(_gameInfo.Lvl);  
        }

        private bool _isStart = false;
        private void StartLvl(int lvl)
        {
            _isMenu = false;
            _isEnding = false;
            switch (lvl)
            {
                case 1:
                    _soundVisualizer.Band = 1;
                    break;
                case 2:
                    _soundVisualizer.Band = 7;
                    break;
                case 3:
                    _soundVisualizer.Band = 6;
                    break;
                case 4:
                    _soundVisualizer.Band = 6;
                    break;
                case 5:
                    _soundVisualizer.Band = 1;
                    break;
            }
            StartCoroutine(WaitForGame(true));
            StartCoroutine(UnlockHeroGun(0.5f));
            _soundVisualizer.KeepPercentage = 0.04f;
            _smallCircle2.transform.Find("Lock").GetComponent<Image>().enabled = false;
            _smallCircle1.transform.Find("Lock").GetComponent<Image>().enabled = false;
            _bassCylinder.SetActive(true);
           // _cameraImageEffect.enabled = true;
            SetTriggers("StartGame");
            
           if(!IsAwesomeHexagonActive)
              _gameInfo.LvlName = GameManager.ReturnNameLvl(lvl);
           else
              _gameInfo.LvlName = GameManager.ReturnNameLvlHexagon(lvl);
            _audioController.StartLvlPlay();
            _gameInfo.Lvl = lvl;
            _timer.MinusTime = Time.time;
            _lvlText.text = _gameInfo.LvlName;
            _lvlManager.UnFreezAllRoundBarriers();           
            _cameraRay.PointsList.Clear();
            _heroGameObject.SetActive(true);
            _gameStart = true;
            if (Math.Abs(GameManager.GetFloatBest(1)) < 1)
            {
                _tutorialer.enabled = true;
                _randomizer.enabled = true;
                _tutorialer.StartTutorial();
                return;
            }
            _randomizer.SetUpWaveDatabase(lvl);
            _nextLvlAnimator.SetTrigger("Hide");
            _lvlManager.SetUpLvl(_gameInfo.Lvl);
            if (TestMode == false)
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
                if (_unityAds.CountTryDisplay == 3) return;
                _isMenu = true;
                _soundVisualizer.enabled = true;
                StartCoroutine(WaitForGame(false));
                _audioController.ButtonStartPlay();
                _soundVisualizer.KeepPercentage = -5;

                CameraAnimator.speed = 1;
                StartCoroutine(BackEffect());
                _gameInfo.CourentTime = "Time  00:00";
                _timer.ResetTime();
                SetActivite(false);
                _unityAds.Cancel = true;
                _heroGameObject.SetActive(false);
                DestroyAllRoundBarriers();
                if (!IsAwesomeHexagonActive)
                {
                    _smallCircle1.transform.Find("lvl_name").GetComponent<Text>().text = GameManager.ReturnNameLvl(_gameInfo.Lvl);
                    _smallCircle1.transform.Find("lvl_text").GetComponent<Text>().text = LVL_IN_CIRCLE + _gameInfo.Lvl;
                    _smallCircle2.transform.Find("lvl_name").GetComponent<Text>().text = GameManager.ReturnNameLvl(_gameInfo.Lvl);
                    _smallCircle2.transform.Find("lvl_text").GetComponent<Text>().text = LVL_IN_CIRCLE + _gameInfo.Lvl;
                }
                else
                {
                    _smallCircle1.transform.Find("lvl_name").GetComponent<Text>().text = GameManager.ReturnNameLvlHexagon(_gameInfo.Lvl);
                    _smallCircle1.transform.Find("lvl_text").GetComponent<Text>().text = LVL_IN_CIRCLE + _gameInfo.Lvl;
                    _smallCircle2.transform.Find("lvl_name").GetComponent<Text>().text = GameManager.ReturnNameLvlHexagon(_gameInfo.Lvl);
                    _smallCircle2.transform.Find("lvl_text").GetComponent<Text>().text = LVL_IN_CIRCLE + _gameInfo.Lvl;
                }
                if(!IsAwesomeHexagonActive)
                    _best.text = GameManager.GetTimeBest(_gameInfo.Lvl);
                else
                    _best.text = GameManager.GetTimeHexagon(_gameInfo.Lvl);
                _nextLvlText.text = ReturnNextLvlAtSeconds(_gameInfo.Lvl);
                GC.Collect();
            }
        }

        private bool _isRestart;
        public void RestartLvl()
        {
            if (_isRestart == false && _isMenu == false && !Advertisement.isShowing)
            {
               // GC.Collect();
               if(_unityAds.CountTryDisplay == 3) return;
                _unityAds.Cancel =  true;
                _isFirstAwesome = true;
                _soundVisualizer.enabled = true;
                _isRestart = true;
                StartCoroutine(UnlockHeroGun(0.25f));
                _bassCylinder.SetActive(true);
                _gameInfo.CourentTime = "Time  00:00";
                _timer.ResetTime();
                SetActivite(true);
                _gameStart = true;
                _audioController.RestartLvlPlay();
                AwesomeCircleAnimator.SetTrigger("Lvl_" + _gameInfo.Lvl);
                CameraAnimator.speed = 1;
                CameraAnimator.SetTrigger("Reset");
//                CameraAnimator.playbackTime = 0f;
//                CameraAnimator.StartPlayback();
                SetTriggers("RestartLvl");

                DestroyAllRoundBarriers();
                _randomizer.SetUpWaveDatabase(_gameInfo.Lvl);

                _timer.MinusTime = Time.time;
                _lvlManager.UnFreezAllRoundBarriers();
                
            }
            _cameraRay.PointsList.Clear();
        }

        private bool _isShowCredit;
        private bool _isNextLvl = false;
        public GameObject AwesomeHexagonGameObject;
        public void ShowCredit()
        {
            if (_isShowCredit == false && _isStart == false && _isSwipe == false)
            {
                StartCoroutine(HideCubes());
                _audioController.ButtonStartPlay();
                _isShowCredit = true;
                if (_nextLvlAnimator.GetCurrentAnimatorStateInfo(0).IsName("Show"))
                    _isNextLvl = true; 
                else
                    _isNextLvl = false;
                AwesomeCircleAnimator.SetTrigger("ShowCredit");

                if (PlayerPrefs.GetString("Awesome Hexagon", "false") == "true")
                {
                    QuestionMarkGameObject.SetActive(false);
                    ImageGameObject.SetActive(false);
                    AwesomeHexagonGameObject.SetActive(true);
                    StartCoroutine(ShowAwesomeHexagon());                    
                    if (PlayerPrefs.GetString("Awesome Hexagon Active", "false") == "true")
                        AwesomeHexagonGameObject.transform.Find("Button").GetComponent<Image>().sprite = Resources.Load<Sprite>("Texture/" + "hexagon");
                    else
                        AwesomeHexagonGameObject.transform.Find("Button").GetComponent<Image>().sprite = Resources.Load<Sprite>("Texture/" + "circle");
                }
                else
                {
                    QuestionMarkGameObject.SetActive(false);
                    ImageGameObject.SetActive(false);
                    StartCoroutine(ShowQuestionMark());
                }
                         
            }

        }

        IEnumerator HideCubes()
        {
            yield return new WaitForSeconds(0.7f);
            foreach (var meshRenderer in AllCubeMeshes)
            {
                meshRenderer.enabled = false;
            }
        }

        public GameObject ImageGameObject;
        IEnumerator ShowQuestionMark()
        {
            yield return new WaitForSeconds(1f);
            QuestionMarkGameObject.SetActive(true);
            ImageGameObject.SetActive(true);
        }

        IEnumerator ShowAwesomeHexagon()
        {
            AwesomeHexagonGameObject.transform.Find("Button").GetComponent<Image>().enabled = false;
            yield return new WaitForSeconds(1f);
            AwesomeHexagonGameObject.transform.Find("Button").GetComponent<Image>().enabled = true;            
        }
        public void HideCredit()
        {           
            if (_isShowCredit)
            {
                _isShowCredit = false;
                _best.text = !IsAwesomeHexagonActive ? GameManager.GetTimeBest(_gameInfo.Lvl) : GameManager.GetTimeHexagon(_gameInfo.Lvl);
                NextLvlImage.enabled = PlayerPrefs.GetString("Awesome Hexagon Active", "false") == "false";
                NextLvlText.enabled = PlayerPrefs.GetString("Awesome Hexagon Active", "false") == "false";
                _audioController.ButtonStartPlay();
                
                StartCoroutine(WaitForMoment());
                AwesomeCircleAnimator.SetTrigger("HideCredit");

                if (PlayerPrefs.GetString("Awesome Hexagon Active", "false") == "true")
                {
                    _gameInfo.LvlName = GameManager.ReturnNameLvlHexagon(_gameInfo.Lvl);
                    if (GameManager.GetFloatHexagon(_gameInfo.Lvl - 1) >= 3600 || _gameInfo.Lvl == 1)
                    {
                        _soundVisualizer.SmoothSpeed = 52;
                        AwesomeCircleAnimator.SetTrigger("Lvl_" + _gameInfo.Lvl);
                        StartCoroutine(WaitLightToHide());
                        if(_lockImage != null)
                        _lockImage.enabled = false;
                    }
                    else
                    {
                        _soundVisualizer.SmoothSpeed = -52;
                        AwesomeCircleAnimator.SetTrigger("Lvl_" + _gameInfo.Lvl + "_gray");
                        StartCoroutine(WaitGrayToHide());
                        if (_lockImage != null)
                            _lockImage.enabled = true;
                    }
                        
                }
                else
                {
                    _gameInfo.LvlName = GameManager.ReturnNameLvl(_gameInfo.Lvl);
                    if (GameManager.GetFloatBest(_gameInfo.Lvl - 1) >= 3600 || _gameInfo.Lvl == 1)
                    {
                        _soundVisualizer.SmoothSpeed = 52;
                        AwesomeCircleAnimator.SetTrigger("Lvl_" + _gameInfo.Lvl);
                        StartCoroutine(WaitLightToHide());
                        if (_lockImage != null)
                            _lockImage.enabled = false;
                    }
                    else
                    {
                        _soundVisualizer.SmoothSpeed = -52;
                        AwesomeCircleAnimator.SetTrigger("Lvl_" + _gameInfo.Lvl + "_gray");
                        StartCoroutine(WaitGrayToHide());
                        if (_lockImage != null)
                            _lockImage.enabled = true;
                    }
                        
                }
            }
            _smallCircle1.transform.Find("lvl_name").GetComponent<Text>().text = _gameInfo.LvlName;
            _smallCircle2.transform.Find("lvl_name").GetComponent<Text>().text = _gameInfo.LvlName;
            StartCoroutine(ShowCubes());
        }

        IEnumerator ShowCubes()
        {
            yield return new WaitForSeconds(0.7f);
            foreach (var meshRenderer in AllCubeMeshes)
            {
                meshRenderer.enabled = true;
            }
            foreach (var meshRenderer in CubeMeshes)
            {
                meshRenderer.enabled = false;
            }
        }

        IEnumerator WaitGrayToHide()
        {
            yield return new WaitForSeconds(.7f);
            LightAnimator.Play("Gray");
            QuestionMarkGameObject.SetActive(false);
            ImageGameObject.SetActive(false);

        }

        IEnumerator WaitLightToHide()
        {
            yield return new WaitForSeconds(.7f);
            LightAnimator.Play("Lvl " + _gameInfo.Lvl);
        }

        public IEnumerator WaitForMoment()
        {
            yield return new WaitForSeconds(.7f);
            if (_isNextLvl)
            {
                _nextLvl.SetActive(true);
                _nextLvlAnimator.SetTrigger("Show");
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
            CameraAnimator.SetTrigger(trigger);
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
                            lvlName = _smallCircle1.transform.Find("lvl_name").GetComponent<Text>();
                            lvlText = _smallCircle1.transform.Find("lvl_text").GetComponent<Text>();
                            _lockImage = _smallCircle1.transform.Find("Lock").GetComponent<Image>();
                            _smallCircle1InTop = true;
                        }
                        else
                        {
                            lvlName = _smallCircle2.transform.Find("lvl_name").GetComponent<Text>();
                            lvlText = _smallCircle2.transform.Find("lvl_text").GetComponent<Text>();
                            _lockImage = _smallCircle2.transform.Find("Lock").GetComponent<Image>();
                            _smallCircle1InTop = false;
                        }

                        if (direction == "Left")
                        {
                            _audioController.SwipeLeftPlay();
                            if(!IsAwesomeHexagonActive)
                                _gameInfo.LvlName = GameManager.ReturnNameLvl(i - 1);
                            else
                                _gameInfo.LvlName = GameManager.ReturnNameLvlHexagon(i - 1);
                            _gameInfo.Lvl = i - 1;
                            _nextLvlText.text = ReturnNextLvlAtSeconds(i -1);
                        }

                        if (direction == "Right")
                        {
                            _audioController.SwipeRightPlay();
                            if (!IsAwesomeHexagonActive)
                                _gameInfo.LvlName = GameManager.ReturnNameLvl(i + 1);
                            else
                                _gameInfo.LvlName = GameManager.ReturnNameLvlHexagon(i + 1);
                            _gameInfo.Lvl = i + 1;
                            _nextLvlText.text = ReturnNextLvlAtSeconds(i + 1);
                        }
                        lvlName.text = _gameInfo.LvlName;
                        lvlText.text = LVL_IN_CIRCLE + _gameInfo.Lvl;
                        if(!IsAwesomeHexagonActive)
                            _best.text = GameManager.GetTimeBest(_gameInfo.Lvl);
                        else
                            _best.text = GameManager.GetTimeHexagon(_gameInfo.Lvl);
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
                if (!IsAwesomeHexagonActive)
                {
                    if (GameManager.LvlIsLock(lvl + 1))
                    {
                        if (lvl + 1 == 6)
                        {
                            _smallCircle2.transform.Find("lvl_text").GetComponent<Text>().text = "";
                            AwesomeCircleAnimator.SetTrigger("RightGray");
                            CameraAnimator.SetTrigger("RightGray");
                            return;
                        }
                        else
                        {

                            SetTrigerToRightGray();
                        }

                    }
                    else
                    {
                        if (_gameInfo.Lvl == 5)
                            _smallCircle2.transform.Find("lvl_text").GetComponent<Text>().text = "";
                        SetTriggers("Swipe" + direction);
                    }
                }
                else
                {
                    if (GameManager.LvlIsLockHexagon(lvl + 1))
                    {
                        if (lvl + 1 == 6)
                        {
                            _smallCircle2.transform.Find("lvl_text").GetComponent<Text>().text = "";
                            AwesomeCircleAnimator.SetTrigger("RightGray");
                            CameraAnimator.SetTrigger("RightGray");
                            return;
                        }
                        else
                        {

                            SetTrigerToRightGray();
                        }

                    }
                    else
                    {
                        if (_gameInfo.Lvl == 5)
                            _smallCircle2.transform.Find("lvl_text").GetComponent<Text>().text = "";
                        SetTriggers("Swipe" + direction);
                    }
                }
            }
            if (direction == "Left")
            {
                if (!IsAwesomeHexagonActive)
                {
                    if (GameManager.LvlIsLock(lvl - 1))
                    {
                        switch (lvl - 1)
                        {
                            case 0:
                                _smallCircle2.transform.Find("lvl_text").GetComponent<Text>().text = "";
                                AwesomeCircleAnimator.SetTrigger("Swipe" + direction);
                                CameraAnimator.SetTrigger("Swipe" + direction);
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
                else
                {
                    if (GameManager.LvlIsLockHexagon(lvl - 1))
                    {
                        switch (lvl - 1)
                        {
                            case 0:
                                _smallCircle2.transform.Find("lvl_text").GetComponent<Text>().text = "";
                                AwesomeCircleAnimator.SetTrigger("Swipe" + direction);
                                CameraAnimator.SetTrigger("Swipe" + direction);
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

        }

        private void SetTriggerToLvl(int lvl)
        {
            SetTriggers("Lvl_" + (lvl - 1)); 
            _audioController.UnloackLvlPlay();
        }

        private void SetTrigerToLeftGray()
        {
            AwesomeCircleAnimator.SetTrigger("LeftGray");
            CameraAnimator.SetTrigger("LeftGray");
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
            if (s == 0)
            {
                _nextLvlAnimator.SetTrigger("Hide");
                return _previousNextLevelName;               
            }
            if(s > 0 )
                _nextLvlAnimator.SetTrigger("Show");
            if(lvl == 5)
                _previousNextLevelName = "End Game at " + (60 - s).ToString() + " " + "seconds";
            else
                _previousNextLevelName = "Next level at " + (60 - s).ToString() + " " + "seconds";
            return _previousNextLevelName;
        }

        private bool _isEnding = false;
        public void TheEnd()
        {
            QuestionMarkGameObject.SetActive(false);
            ImageGameObject.SetActive(false);
            PlayerPrefs.SetString("Awesome Hexagon", "true");
            _isEnding = true;
            GameManager.TheEnd();
            _audioController.EndAppPlay();
            _timer.TheEnd();
            _randomizer.Disable();
            DestroyAllRoundBarriers();
            _lvlManager.FreezAllRoundBarriers();
            GameManager.SetBest(_gameInfo.Lvl, _timerText.text);
            _isSwipe = false;
            _soundVisualizer.KeepPercentage = -5;
            _isStart = false;
            SetTriggers("TheEnd");
        }

        public void PowerButton()
        {
            ShowDialogMassage();         
        }

        public void PowerButtonDialogYes()
        {
            PowerButtonGameObject.SetActive(false);
            _audioController.ButtonStartPlay();
            _audioController.ResumeLvl();
            Time.timeScale = 1;
        }

        public void ShowDialogMassage()
        {
            PowerButtonGameObject.SetActive(true);
            _audioController.ButtonStartPlay();
            _audioController.PauseLvl();
            Time.timeScale = 0;           
        }

        public void PowerButtonDialogNo()
        {
            Application.Quit();
        }

        public bool IsAwesomeHexagonActive = false;
//        private void CounterLeftClickButton()
//        {
//            if (_gameInfo.Lvl == 1 && PlayerPrefs.GetString("Awesome Hexagon","false") == "false")
//            {
//                _leftClickCounter++;
//                if (_leftClickCounter == 10)
//                {
//                    _audioController.PlayEasterEgg(0);
//                }
//                if (_leftClickCounter == 17)
//                {
//                    _audioController.PlayEasterEgg(1);
//                }
//                if (_leftClickCounter == 20)
//                {
//                    _audioController.PlayEasterEgg(0);
//                }
//                if (_leftClickCounter == 22)
//                {
//                    _audioController.PlayEasterEgg(2);
//                }
//                if (_leftClickCounter == 27)
//                {
//                    _audioController.PlayEasterEgg(3);
//                    PlayerPrefs.SetString("Awesome Hexagon", "true");
//                    PlayerPrefs.SetString("Awesome Hexagon Active", "true");
//                    TextHexagon.enabled = true;
//                    NextLvlImage.enabled = PlayerPrefs.GetString("Awesome Hexagon Active", "false") == "false";
//                    NextLvlText.enabled = PlayerPrefs.GetString("Awesome Hexagon Active", "false") == "false";
//                    IsAwesomeHexagonActive = true;
//                }
//            }
//        }

        public void ActiveOrDeactiveAwesomeHexagon()
        {
            if (_isShowCredit == false) return;
            _audioController.ButtonSmallPlay();
            if (PlayerPrefs.GetString("Awesome Hexagon Active", "false") == "true")
            {
                PlayerPrefs.SetString("Awesome Hexagon Active", "false");
                AwesomeHexagonGameObject.transform.Find("Button").GetComponent<Image>().sprite = Resources.Load<Sprite>("Texture/" + "circle");
                TextHexagon.enabled = false;
                IsAwesomeHexagonActive = false;
            }
            else
            {
                PlayerPrefs.SetString("Awesome Hexagon Active", "true");
                AwesomeHexagonGameObject.transform.Find("Button").GetComponent<Image>().sprite = Resources.Load<Sprite>("Texture/" + "hexagon");
                TextHexagon.enabled = true;
                IsAwesomeHexagonActive = true;
            }

        }

        public void BackToTheGameAfterTheEnd()
        {
            _audioController.ButtonStartPlay();
            _isMenu = false;
            _isRestart = false;
            _cameraRay.PointsList.Clear();
            BackToMenu();
        }
        public void StartGameScreen()
        {
            BlackScreenAnimator.SetTrigger("Start");
            _audioController.ButtonStartPlay();
        }
    }
}