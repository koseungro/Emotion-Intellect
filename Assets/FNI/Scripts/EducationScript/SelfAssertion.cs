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
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace FNI
{
    public class SelfAssertion : BaseScript 
    {
        #region Singleton
        private static SelfAssertion _instance;
        public static SelfAssertion Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SelfAssertion>();
                    if (_instance == null)
                        Debug.LogError("SelfAssertion를 찾을 수 없습니다. ");
                }
                return _instance;
            }
        }
        #endregion

        public ContentsData contentsData;
        public GameObject backGroundUI;

        public Texture person1;
        public Texture person2;
        public Texture person3;
        public Texture person4;

        private Texture seletTexture;

        public Button male20;
        public Button female20;

        public Button male40;
        public Button female40;

        public PlayableDirector playableDirector;

        public LineDrawer lineDrawer;
        public GameObject pictureCanvas;

        FNI_Record fni_Record;

        private void Start()
        {

            fni_Record = FindObjectOfType<FNI_Record>();

            male20.onClick.AddListener(delegate { ButtonClick(0);});
            female20.onClick.AddListener(delegate { ButtonClick(1); });
            
            male40.onClick.AddListener(delegate { ButtonClick(2); });
            female40.onClick.AddListener(delegate { ButtonClick(3); });

            male20.interactable = false;
            female20.interactable = false;
            male40.interactable = false;
            female40.interactable = false;

            SetContentName("자기주장");
        }


        public override void SetContentName(string contentName)
        {
            base.SetContentName(contentName);
        }

        public void ButtonClick(int num)
        {
            switch (num)
            {
                case 0:
                    seletTexture = person1;
                    break;

                case 1:
                    seletTexture = person2;
                    break;

                case 2:
                    seletTexture = person3;
                    break;

                case 3:
                    seletTexture = person4;
                    break;
            }
            if (playableDirector.state == UnityEngine.Playables.PlayState.Paused)
            {
                isAction = false;
                playableDirector.Play();
            }            
        }


        bool isRecordStart = false;

        public void RecordTime()
        {
            playableDirector.Pause();
            fni_Record.Show(RecordType.TimeLimit, 15, true,true);
        }

        public void ButtonInteractableOn()
        {
            male20.interactable = true;
            female20.interactable = true;
            male40.interactable = true;
            female40.interactable = true;
        }

        public void ButtonInteractableOff()
        {
            male20.interactable = false;
            female20.interactable = false;
            male40.interactable = false;
            female40.interactable = false;
        }

        // 선긋기, 사진고르기 중이면 true 끝나면 false
        public bool isAction = true;

        public void PauseTimeLine()
        {
            
            if (lineDrawer.gameObject.activeSelf == true)
            {
                Debug.Log("lineDrawer");
                lineDrawer.isLine = true;
                isAction = true;
            }
            if (pictureCanvas.activeSelf == true)
            {
                Debug.Log("pictureCanvas");
                isAction = true;
            }

            ButtonInteractableOn();
            playableDirector.Pause();
        }

        public void PlayTimeLine()
        {
            playableDirector.Play();
        }

        public void SetPersonBG()
        {
            BackGroundChanger.Instance.DefaultSettingRender();
            backGroundUI.GetComponent<MeshRenderer>().material.mainTexture = seletTexture;
            backGroundUI.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        public void SetStage()
        {
            BackGroundChanger.Instance.StageSettingRender();
        }

        public override void EndAnimation()
        {
            MainManager.Instance.StartContentsData(contentsData);
            BackGroundChanger.Instance.DefaultSettingRender();
        }

        public void BreathTime()
        {
            playableDirector.Pause();
            GaugeRoutine = GaugeCountDownRoutine(4, 7, 8);
            StartCoroutine(GaugeRoutine);
        }
        IEnumerator GaugeRoutine;

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

            playableDirector.Play();
            yield return null;
            
        }

    }
}