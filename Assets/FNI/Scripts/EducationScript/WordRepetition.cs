/// 작성자: 김윤빈
/// 작성일: 2020-07-28
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FNI
{
    public class WordRepetition : BaseScript
    {

        #region Singleton
        private static WordRepetition _instance;
        public static WordRepetition Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<WordRepetition>();
                    if (_instance == null)
                        Debug.LogError("WordRepetition를 찾을 수 없습니다. ");
                }
                return _instance;
            }
        }
        #endregion

        public GameObject[] textFirst;
        public GameObject[] textSecond;
        public TextMeshProUGUI timerText;
        public ContentsData contentsData;
        IEnumerator textRoutine;
        IEnumerator timerRoutine;
        public Animator textAnimator;

        public TextMeshProUGUI card1;
        public TextMeshProUGUI card2;
        public TextMeshProUGUI card3;

        public GameObject floatText;

        public void SetCard()
        {
            card1.text = GetUserInfo.Ecard1;
            card2.text = GetUserInfo.Ecard2;
            card3.text = GetUserInfo.Ecard3;
        }

        private void Start()
        {
            timerRoutine = CountDownRoutine(30);
            textRoutine = TextAnimRoutine();
            SetContentName("단어 반복");
        }

        public override void SetContentName(string contentName)
        {
            base.SetContentName(contentName);
        }

        public void SetFloatAnim()
        {
            floatText.transform.position = new Vector3(0f, -2f, 3.8f);
            textAnimator.SetInteger("FloatText", 1);
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
      
        public void StartTimer()
        {
            StartCoroutine(timerRoutine);
        }

        public void StopTimer()
        {
            StopCoroutine(timerRoutine);
        }

        IEnumerator CountDownRoutine(int time1)
        {
            Color color = timerText.color;
            color.a = 1f;
            timerText.color = color;
            while (time1 > -1)
            {
                timerText.text = time1.ToString() + "초 남았습니다.";
                yield return new WaitForSeconds(1f);
                time1--;
            }
            color.a = 0f;
            timerText.color = color;
            Debug.Log("end");

            yield return null;
        }


        public void StartTextRoutine()
        {
            StartCoroutine(textRoutine);
        }
         
        public void StopTextRoutine()
        {
            StopCoroutine(textRoutine);
            for (int cnt = 0; cnt < textFirst.Length; cnt++)
            {
                textFirst[cnt].SetActive(true);
            }
            for (int cnt = 0; cnt < textSecond.Length; cnt++)
            {
                textSecond[cnt].SetActive(true);
            }
        }

        IEnumerator TextAnimRoutine()
        {
            while (true)
            {
                for (int cnt = 0; cnt < textFirst.Length; cnt++)
                {
                    textFirst[cnt].SetActive(true);
                }
                for (int cnt = 0; cnt < textSecond.Length; cnt++)
                {
                    textSecond[cnt].SetActive(false);
                }
                yield return new WaitForSeconds(0.7f);
                for (int cnt = 0; cnt < textFirst.Length; cnt++)
                {
                    textFirst[cnt].SetActive(false);
                }
                for (int cnt = 0; cnt < textSecond.Length; cnt++)
                {
                    textSecond[cnt].SetActive(true);
                }
                yield return new WaitForSeconds(0.7f);
            }
        }



    }
}