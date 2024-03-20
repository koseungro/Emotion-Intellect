/// 작성자: 김윤빈
/// 작성일: 2020-08-04
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FNI
{
    public enum EmotionVideoOption
    {
        // 시작에 앞서
        BeforeStart,

        // 상황 1
        Situation1,

        Situation2,

        Situation3,

        Situation4,

        Record
    }

    public class GuideManager : BaseScript
    {
        #region Singleton
        private static GuideManager _instance;
        public static GuideManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GuideManager>();
                    if (_instance == null)
                        Debug.LogError("GuideManager를 찾을 수 없습니다. ");
                }
                return _instance;
            }
        }
        #endregion

        public TextMeshProUGUI guideTextMain;
        public TextMeshProUGUI guideText;

        public TextMeshProUGUI recordTextMain;
        public TextMeshProUGUI recordText;

        Button recordStartBtn;

        GameObject guidePanel;
        GameObject recordPanel;

        // time은 0~1까지 deltaTime을 계속 더해서 지속시간으로 씀
        private float time = 0f;

        // 페이드 중인지 확인할 때 사용
        private bool isFade = false;

        private float F_time = 1f;

        public EmotionVideoOption emotionOption;

        private void Start()
        {
            guidePanel = transform.Find("CurvedCanvas/GuidePanel").gameObject;
            recordPanel = transform.Find("CurvedCanvas/RecordPanel").gameObject;
            recordStartBtn = recordPanel.transform.Find("RecordStartBtn").GetComponent<Button>();
        }

        private void OnEnable()
        {
            UIManager.Instance.OnObjectControl += AllOff;
        }

        public override void AllOff(bool active)
        {
            base.AllOff(active);
        }

        public override void Show()
        {
            base.Show();
            if (emotionOption == EmotionVideoOption.Record)
            {
                recordPanel.SetActive(true);
                guidePanel.SetActive(false);
            }
            else
            {
                recordPanel.SetActive(false);
                guidePanel.SetActive(true);
            }
        }


        public void SetComment(EmotionVideoOption Option)
        {
            switch (Option)
            {
                case EmotionVideoOption.BeforeStart:
                    guideTextMain.text = "스트레스 사건들";
                    guideText.text = "최근 가장 스트레스 받았던 사건을 1분간 떠올려보세요." +"\n"
                        + "그 때의 장소, 시간, 누구와 같이 있었는지, 어떤일이 있었는지," + "\n" 
                        + "어떠한 감정이 들었는지 가능한 구체적으로 생각해보세요.";                    
                    break;

                case EmotionVideoOption.Situation1:
                    guideTextMain.text = "자존감이 낮아지는 사건들";
                    guideText.text = "자존감이 가장 낮아졌던 사건을 1분간 떠올려보세요."+"\n"
                        + " 그 때의 장소, 시간, 누구와 같이 있었는지, 어떤일이 있었는지," + "\n" 
                        + "어떠한 감정이 들었는지 가능한 구체적으로 생각해보세요.";
                    break;

                case EmotionVideoOption.Situation2:
                    guideTextMain.text = "수동공격 하는 사람들";
                    guideText.text = "최근 수동공격 하는 사람들과 있었던 사건을 1분간 떠올려보세요." + "\n" 
                        + " 그 때의 장소, 시간, 누구와 같이 있었는지, 어떤일이 있었는지," + "\n" 
                        + "어떠한 감정이 들었는지 가능한 구체적으로 생각해보세요.";
                    break;

                case EmotionVideoOption.Situation3:
                    guideTextMain.text = "적당한 주장을 막는 사람들";
                    guideText.text = "최근 적당한 주장을 막는 사람들과 있었던 사건을 1분간 떠올려보세요." + "\n" 
                        + " 그 때의 장소, 시간, 누구와 같이 있었는지, 어떤일이 있었는지," + "\n" 
                        + "어떠한 감정이 들었는지 가능한 구체적으로 생각해보세요.";
                    break;

                case EmotionVideoOption.Situation4:
                    guideTextMain.text = "불합리한 비난을 하는 사람들";
                    guideText.text = "불합리한 비난을 하는 사람들과 있었던 사건을 1분간 떠올려보세요." + "\n" 
                        + " 그 때의 장소, 시간, 누구와 같이 있었는지, 어떤일이 있었는지," + "\n" 
                        + "어떠한 감정이 들었는지 가능한 구체적으로 생각해보세요.";
                    break;

                case EmotionVideoOption.Record:
                    recordTextMain.text = "자기 연습 훈련";
                    recordText.text = "앞서 배운 기술을 정리하는 시간입니다." + "\n"
                        +"이 훈련은 녹음기능을 사용해 훈련을 진행합니다." + "\n"
                        +"녹음된 음성은 훈련기록/조회용 으로만 사용됩니다.";
                    break;
            }

            //Comment = str;
            //text.text = comment;
            if (Option != EmotionVideoOption.Record)
            {
                //StartCoroutine(GuideRoutine());
            }            
        }
        
        public void StartButtonClick()
        {
            MainManager.Instance.NextState();
        }

        IEnumerator GuideRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            MainManager.Instance.NextState();
            if (emotionOption != EmotionVideoOption.Record)
            {
                yield return new WaitForSeconds(5f);
                fade();
            }
            //yield return new WaitForSeconds(5f);
            //MainManager.Instance.NextState();
        }

        void fade()
        {
            StartCoroutine(FadeRoutine());
        }

        public IEnumerator FadeRoutine()
        {
            isFade = true;

            Color alpha = guideText.color;

            // 0으로 한번 초기화
            time = 0;

            while (alpha.a > 0f)
            {
                // 매 프레임 deltatime을 F_time으로 나눈 값을 time에 더해줌
                time += Time.deltaTime / F_time;
                // 부드럽게
                alpha.a = Mathf.Lerp(1, 0, time);

                guideText.color = alpha;
                yield return null;
            }

            // 0으로 한번 초기화
            time = 0;
            guideText.text = "이제 시작합니다.";

            // 1초 대기시간
            yield return new WaitForSeconds(1f);

            while (alpha.a < 1f)
            {
                // 매 프레임 deltatime을 F_time으로 나눈 값을 time에 더해줌
                time += Time.deltaTime / F_time;
                // 부드럽게
                alpha.a = Mathf.Lerp(0, 1, time);

                guideText.color = alpha;
                yield return null;
            }
            isFade = false;            
            yield return null;
        }


    }
}