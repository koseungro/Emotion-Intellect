/// 작성자: 김윤빈
/// 작성일: 2020-07-28
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace FNI
{
    public class Visualization : BaseScript
    {

        #region Singleton
        private static Visualization _instance;
        public static Visualization Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<Visualization>();
                    if (_instance == null)
                        Debug.LogError("Visualization를 찾을 수 없습니다. ");
                }
                return _instance;
            }
        }
        #endregion

        public ContentsData contentsData;

        public TextMeshProUGUI timeText;
        public TextMeshProUGUI engText;
        public TextMeshProUGUI korText;

        public GameObject Sec4;
        public GameObject[] checkMark4;

        public GameObject Sec7;
        public GameObject[] checkMark7;

        public GameObject Sec8;
        public GameObject[] checkMark8;

        public Animator gaugeAnimator;

        IEnumerator GaugeRoutine;

        PlayableDirector myPlayableDirector;
        private void Start()
        {
            myPlayableDirector = this.GetComponent<PlayableDirector>();
            
            SetContentName("시각화");
        }

        public override void SetContentName(string contentName)
        {
            base.SetContentName(contentName);
        }

        public void SetStage()
        {
            BackGroundChanger.Instance.StageSettingRender();
        }

        public void SetSky()
        {
            BackGroundChanger.Instance.SkySettingRender();
        }

        public override void EndAnimation()
        {
            MainManager.Instance.StartContentsData(contentsData);
            BackGroundChanger.Instance.DefaultSettingRender();
        }

        public void TimerRepeatStart()
        {
            myPlayableDirector.Pause();
            GaugeRoutine = GaugeCountDownRoutine(4, 7, 8);
            //StartCoroutine(CountDownRepeatRoutine(4,7,8));
            StartCoroutine(GaugeRoutine);
        }

        public void TimerRepeatStop()
        {
            //StopCoroutine(CountDownRepeatRoutine(4, 7, 8));
            Sec4.SetActive(true);
            Sec7.SetActive(false);
            Sec8.SetActive(false);

            timeText.text = "4초";
            engText.text = "BREATH IN";
            korText.text = "숨을 들이쉬세요";

            StopCoroutine(GaugeRoutine);
            count = 0;
        }

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

        int count = 0;
        IEnumerator GaugeCountDownRoutine(int time1, int time2, int time3)
        {
            //Color color = timerText.color;
            //color.a = 1f;
            //timerText.color = color;
            int num1 = time1;
            int num2 = time2;
            int num3 = time3;            

            while (count < 3)
            {
                OnTimerObj(checkMark4);
                OnTimerObj(checkMark7);
                OnTimerObj(checkMark8);
                time1 = num1;
                time2 = num2;
                time3 = num3;

                while (time1 > -1)
                {
                    if (gaugeAnimator.GetInteger("Breath") != 3)
                    {
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
                count++;
                yield return null;
            }

            myPlayableDirector.Play();
            yield return null;
        }







    }
}