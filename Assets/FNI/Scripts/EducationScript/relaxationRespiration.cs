/// 작성자: 김윤빈
/// 작성일: 2020-07-28
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using DG.Tweening;

namespace FNI
{
    public class relaxationRespiration : BaseScript
    {

        #region Singleton
        private static relaxationRespiration _instance;
        public static relaxationRespiration Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<relaxationRespiration>();
                    if (_instance == null)
                        Debug.LogError("relaxationRespiration를 찾을 수 없습니다. ");
                }
                return _instance;
            }
        }
        #endregion      


        public AudioClip[] audioClips;

        private void Awake()
        {
            myPlayableDirector = this.GetComponent<PlayableDirector>();
            myPlayableDirector.played += OnPlayableDirectorPlayed;
           
        }

        public void OnPlayableDirectorPlayed(PlayableDirector aDirector)
        {
           
        }


        private void Update()
        {
            if (this.gameObject.activeSelf == true)
            {
                //Debug.Log(pd.time + ": Time");
                //Debug.Log(pd.timeUpdateMode + ": Time");
            }
            if (OVRInput.GetUp(OVRInput.Button.Two))
            {
                //myPlayableDirector.playableAsset.
                Time.timeScale = 3f;
            }
            if (OVRInput.GetUp(OVRInput.Button.One))
            {
                //myPlayableDirector.playableAsset.
                Time.timeScale = 1f;
            }



        }
        private void Start()
        {
            GaugeRoutine = GaugeCountDownRoutine(4, 7, 8);
            SetContentName("이완호흡");
        }

        int cnt = 0;
        public TextMeshProUGUI timerText;

        public TextMeshProUGUI SubtitleText;

        public ContentsData contentsData;

        PlayableDirector myPlayableDirector;

        public TimelineAsset ts;

        public AudioSource audioSource;

        IEnumerator GaugeRoutine;

        public GameObject gauge;

        

        public void GaugeScaleUp()
        {
            gauge.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 1f);
        }

        public override void SetContentName(string contentName)
        {
            base.SetContentName(contentName);
        }

        public void Timer4Second()
        {
            StartCoroutine(CountDownRoutine(4));
        }
        public void Timer7Second()
        {
            StartCoroutine(CountDownRoutine(7));
        }

        public void Timer8Second()
        {
            StartCoroutine(CountDownRoutine(8));
        }

        IEnumerator CountDownRoutine(int time1)
        {
            Color color = timerText.color;
            color.a = 1f;
            timerText.color = color;
            while (time1 > 0)
            {               
                timerText.text = time1.ToString() + "초";
                yield return new WaitForSeconds(1f);
                time1--;
            }
            color.a = 0f;
            timerText.color = color;
            Debug.Log("end");

            //while (time2 > 0)
            //{
            //    timerText.text = time2.ToString();

            //    yield return new WaitForSeconds(1f);

            //    time2--;

            //}

            //while (time3 > 0)
            //{
            //    timerText.text = time3.ToString();

            //    yield return new WaitForSeconds(1f);

            //    time3--;

            //}

            yield return null;


        }
        
        public void TimerRepeatStart()
        {
            //StartCoroutine(CountDownRepeatRoutine(4,7,8));
            GaugeRoutine = GaugeCountDownRoutine(4, 7, 8);
            StartCoroutine(GaugeRoutine);
        }

        public void TimerRepeatStop()
        {
            //StopCoroutine(CountDownRepeatRoutine(4, 7, 8));
            StopCoroutine(GaugeRoutine);
        }

        IEnumerator CountDownRepeatRoutine(int time1, int time2, int time3)
        {
            Color color = timerText.color;
            color.a = 1f;
            timerText.color = color;

            while (time1 > 0)
            {
                timerText.text = time1.ToString() + "초";
                yield return new WaitForSeconds(1f);
                time1--;
            }

            while (time2 > 0)
            {
                timerText.text = time2.ToString() + "초";

                yield return new WaitForSeconds(1f);

                time2--;

            }

            while (time3 > 0)
            {
                timerText.text = time3.ToString() + "초";

                yield return new WaitForSeconds(1f);

                time3--;

            }

            color.a = 0f;
            timerText.color = color;
            yield return null;
        }


        public TextMeshProUGUI timeText;
        public TextMeshProUGUI engText;
        public TextMeshProUGUI korText;

        public GameObject Sec4;
        public GameObject[] checkMark4;

        public GameObject Sec7;
        public GameObject[] checkMark7;

        public GameObject Sec8;
        public GameObject[] checkMark8;

        void OnTimerObj(GameObject[] gameObjects)
        {
            for (int cnt = 0; cnt < gameObjects.Length; cnt++)
            {
                gameObjects[cnt].SetActive(true);
            }
        }

        void CountDownObj(GameObject[] gameObjects, int num)
        {
            if (num < 0)
            {
                return;
            }
            gameObjects[num].SetActive(false);
        }

        public Animator gaugeAnimator;

        IEnumerator GaugeCountDownRoutine(int time1, int time2, int time3)
        {
            //Color color = timerText.color;
            //color.a = 1f;
            //timerText.color = color;
            int num1 = time1;
            int num2 = time2;
            int num3 = time3;


            while (true)
            {

                //OnTimerObj(checkMark4);
                //OnTimerObj(checkMark7);
                //OnTimerObj(checkMark8);
                time1 = num1;
                time2 = num2;
                time3 = num3;

                while (time1 > -1)
                {
                    if (gaugeAnimator.GetInteger("Breath") != 3)
                    {
                        OnTimerObj(checkMark4);
                        gaugeAnimator.SetInteger("Breath", 3);
                    }
                    Sec4.SetActive(true);
                    Sec7.SetActive(false);
                    Sec8.SetActive(false);

                    timeText.text = time1.ToString() + "초";
                    engText.text = "BREATH IN";
                    korText.text = "숨을 들이쉬세요";

                    yield return new WaitForSeconds(1f);
                    time1--;
                    CountDownObj(checkMark4, time1);
                }

                while (time2 > -1)
                {

                    if (gaugeAnimator.GetInteger("Breath") != 1)
                    {
                        OnTimerObj(checkMark7);
                        gaugeAnimator.SetInteger("Breath", 1);
                    }
                    
                    Sec4.SetActive(false);
                    Sec7.SetActive(true);
                    Sec8.SetActive(false);

                    timeText.text = time2.ToString() + "초";
                    engText.text = "HOLD";
                    korText.text = "호흡을 멈추세요";

                    yield return new WaitForSeconds(1f);
                    time2--;
                    CountDownObj(checkMark7, time2);
                }

                while (time3 > -1)
                {
                    if (gaugeAnimator.GetInteger("Breath") != 2)
                    {
                        OnTimerObj(checkMark8);
                        gaugeAnimator.SetInteger("Breath", 2);
                    }
                    
                    Sec4.SetActive(false);
                    Sec7.SetActive(false);
                    Sec8.SetActive(true);

                    timeText.text = time3.ToString() + "초";
                    engText.text = "BREATH OUT";
                    korText.text = "숨을 천천히 내쉬세요";

                    yield return new WaitForSeconds(1f);
                    time3--;
                    CountDownObj(checkMark8, time3);
                }
                yield return null;
            }
        }



        public void MusicSoundOn()
        {
            //StartCoroutine(SoundRoutine());
            enumerator = MusicSoundRoutine();
            StartCoroutine(enumerator);
        }

        public void MusicSoundOff()
        {

        }

        // time은 0~1까지 deltaTime을 계속 더해서 지속시간으로 씀
        private float time = 0f;

        private float F_time = 1.5f;

        public AudioClip testAD;

        IEnumerator enumerator;


        IEnumerator MusicSoundRoutine()
        {
            this.audioSource.clip = testAD;

            time = 0;
            while (this.audioSource.volume < 1f)
            {
                time += Time.deltaTime / F_time;

                this.audioSource.volume = time;
                yield return null;
            }
            yield return null;
        }

        IEnumerator SoundRoutine()
        {
            while (myPlayableDirector.state == UnityEngine.Playables.PlayState.Playing)
            {
                switch (audioSource.clip.name)
                {
                    case "01_01":
                        SubtitleText.text = audioSource.clip.name;
                        break;
                    case "01_02":
                        SubtitleText.text = audioSource.clip.name;
                        break;
                }
                yield return null;
            }
            yield return null;
        }



        public void SetStage()
        {
            BackGroundChanger.Instance.StageSettingRender();
        }

        public void SetSpace()
        {
            BackGroundChanger.Instance.SpaceSettingRender();
        }


        public override void EndAnimation()
        {
            MainManager.Instance.StartContentsData(contentsData);
            BackGroundChanger.Instance.DefaultSettingRender();
        }

        public void test()
        {
            TimelineAsset timeline = (TimelineAsset)myPlayableDirector.playableAsset;
            var audioTrack = timeline.GetOutputTracks().FirstOrDefault(t => t is AudioTrack);
            var clip = audioTrack.GetClips().FirstOrDefault();
            var audioClip = ((AudioPlayableAsset)clip.asset).clip;
            Debug.Log(audioClip.name);
            Debug.Log(audioClip.GetType().FullName);

            //var timelineAsset = pd.playableAsset as TimelineAsset;

            //foreach (var track in timelineAsset.GetOutputTracks())
            //{
            //    if (track.name.Contains("Relaxation"))
            //    {
            //        pd.SetGenericBinding(track, audioSource);
            //    }

            //    //AudioTrack audioTrack = track as AudioTrack;

            //    //if (audioTrack == null)
            //    //{
            //    //    continue;
            //    //}
            //    //IEnumerable<TimelineClip> trackList = audioTrack.GetClips();

            //}
            //StartCoroutine(SoundRoutine());
        }
    }
}