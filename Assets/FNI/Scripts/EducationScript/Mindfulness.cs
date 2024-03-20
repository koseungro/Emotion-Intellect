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
using UnityEngine.UI.Extensions.Tweens;

namespace FNI
{
    public class Mindfulness : BaseScript
    {

        #region Singleton
        private static Mindfulness _instance;
        public static Mindfulness Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<Mindfulness>();
                    if (_instance == null)
                        Debug.LogError("Mindfulness를 찾을 수 없습니다. ");
                }
                return _instance;
            }
        }
        #endregion

        public void Start()
        {
            SetContentName("마음챙김");
        }

        public ParticleSystem ballParticle1;
        public ParticleSystem ballParticle2;
        public ParticleSystem ballParticle3;

        public SkinnedMeshRenderer subHeartSkinMesh;

        public ContentsData contentsData;

        public void SetStage()
        {
            BackGroundChanger.Instance.StageSettingRender();
        }

        public override void SetContentName(string contentName)
        {
            base.SetContentName(contentName);
        }

        public void SmileSubFace()
        {
            //skinnedMesh.SetBlendShapeWeight(6, 100);
            StartCoroutine(SmileRoutine());
        }

        float time;
        float F_time = 1f;

        IEnumerator SmileRoutine()
        {
            // 0으로 한번 초기화
            time = 0;

            //페이드 아웃 먼저, 알파값이 1보다 작으면 계속 반복
            while (subHeartSkinMesh.GetBlendShapeWeight(6) < 100)
            {
                // 매 프레임 deltatime을 F_time으로 나눈 값을 time에 더해줌
                time += Time.deltaTime / F_time;
                // 부드럽게
                float cnt = Mathf.Lerp(0, 100, time);
                subHeartSkinMesh.SetBlendShapeWeight(6, cnt);

                yield return null;
            }

            yield return null;
        }

        public void CloseEyeSubFace()
        {
            //skinnedMesh.SetBlendShapeWeight(6, 100);
            StartCoroutine(CloseEyeRoutine());
        }

        IEnumerator CloseEyeRoutine()
        {
            // 0으로 한번 초기화
            time = 0;

            //페이드 아웃 먼저, 알파값이 1보다 작으면 계속 반복
            while (subHeartSkinMesh.GetBlendShapeWeight(0) < 100)
            {
                // 매 프레임 deltatime을 F_time으로 나눈 값을 time에 더해줌
                time += Time.deltaTime / F_time;
                // 부드럽게
                float cnt = Mathf.Lerp(0, 100, time);
                float cnt2 = Mathf.Lerp(0, 50, time);
                subHeartSkinMesh.SetBlendShapeWeight(0, cnt);
                subHeartSkinMesh.SetBlendShapeWeight(6, cnt2);
                yield return null;
            }

            yield return null;
        }

        public void DefaultFace()
        {
            //skinnedMesh.SetBlendShapeWeight(6, 100);
            StartCoroutine(DefaultFaceRoutine());
        }

        IEnumerator DefaultFaceRoutine()
        {
            // 0으로 한번 초기화
            time = 0;

            //페이드 아웃 먼저, 알파값이 1보다 작으면 계속 반복
            while (subHeartSkinMesh.GetBlendShapeWeight(0) > 0)
            {
                // 매 프레임 deltatime을 F_time으로 나눈 값을 time에 더해줌
                time += Time.deltaTime / F_time;
                // 부드럽게
                float cnt = Mathf.Lerp(100, 0, time);
                float cnt2 = Mathf.Lerp(50, 0, time);
                subHeartSkinMesh.SetBlendShapeWeight(0, cnt);
                subHeartSkinMesh.SetBlendShapeWeight(6, cnt2);
                yield return null;
            }

            yield return null;
        }

        public void StartParticle()
        {
            ballParticle1.Play();
            ballParticle2.Play();
            ballParticle3.Play();
        }

        public void StopParticle()
        {
            ballParticle1.Stop();
            ballParticle2.Stop();
            ballParticle3.Stop();
        }

        public Animator gaugeAnimator;


        public TextMeshProUGUI timeText;
        public TextMeshProUGUI engText;
        public TextMeshProUGUI korText;

        public GameObject Sec4;
        public GameObject[] checkMark4;

        public GameObject Sec7;
        public GameObject[] checkMark7;

        public GameObject Sec8;
        public GameObject[] checkMark8;

        IEnumerator timeRoutine;

        public void TimerRepeatStart()
        {
            //StartCoroutine(CountDownRepeatRoutine(4,7,8));
            timeRoutine = GaugeCountDownRoutine(4, 7, 8);
            StartCoroutine(timeRoutine);
        }

        public void TimerRepeatStop()
        {
            //StopCoroutine(CountDownRepeatRoutine(4, 7, 8));
            StopCoroutine(timeRoutine);
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
                yield return null;
            }
        }

        public override void EndAnimation()
        {
            MainManager.Instance.StartContentsData(contentsData);
            BackGroundChanger.Instance.DefaultSettingRender();
        }

    }
}