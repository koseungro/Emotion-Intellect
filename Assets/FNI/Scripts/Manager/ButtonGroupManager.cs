/// 작성자: 김윤빈
/// 작성일: 2020-07-07
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Panic2;
using UnityEngine.Events;

namespace FNI
{
    public class ButtonGroupManager : BaseScript
    {
        #region Singleton
        private static ButtonGroupManager _instance;
        public static ButtonGroupManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ButtonGroupManager>();
                    if (_instance == null)
                        Debug.LogError("ButtonGroupManager를 찾을 수 없습니다. ");
                }
                return _instance;
            }
        }
        #endregion

        private IS_Button button1;
        private IS_Button button2;
        private IS_Button button3;
        private IS_Button button4;
        private IS_Button button5;

        public Canvas buttonCanvas;

        public GameObject titleText;
        public GameObject dot;

        private List<IS_Button> buttonList = new List<IS_Button>();
        private void OnEnable()
        {
            UIManager.Instance.OnObjectControl += AllOff;
        }

        private void Start()
        {
            //buttonCanvas = transform.Find("CurvedCanvas").GetComponent<Canvas>();
            button1 = transform.Find("CurvedCanvas/Parent/Button1").GetComponent<IS_Button>();
            button2 = transform.Find("CurvedCanvas/Parent/Button2").GetComponent<IS_Button>();
            button3 = transform.Find("CurvedCanvas/Parent/Button3").GetComponent<IS_Button>();
            button4 = transform.Find("CurvedCanvas/Parent/Button4").GetComponent<IS_Button>();
            button5 = transform.Find("CurvedCanvas/Parent/Button5").GetComponent<IS_Button>();
            buttonList.Add(button1);
            buttonList.Add(button2);
            buttonList.Add(button3);
            buttonList.Add(button4);
            buttonList.Add(button5);

            ButtonActive(false);
        }

        public void ButtonActive(bool active)
        {
            for (int cnt = 0; cnt < buttonList.Count; cnt++)
            {
                buttonList[cnt].SetActive(active);
            }
        }

        public override void AllOff(bool active)
        {
            base.AllOff(active);
        }

        public void SetButtonData(List<IS_ButtonData> iS_ButtonDatas, List<ContentsData> contentsDatas)
        {
            // 첫 시작 일땐 그대로 버튼 세팅 해주면 됨
            if (MainManager.Instance.isStart == false)
            {
                Debug.Log("첫 버튼 세팅");
                for (int cnt = 0; cnt < iS_ButtonDatas.Count; cnt++)
                {
                    int num = cnt;
                    buttonList[cnt].Init(iS_ButtonDatas[cnt]);
                    buttonList[cnt].SetActive(true);
                    buttonList[num].AddListener(delegate {
                        NextContents(contentsDatas[num]);
                    });
                    buttonList[num].AddListener(delegate {
                        ButtonClick(buttonList[num].name);
                    });
                }
            }
            // 훈련을 보고 난 이후엔 훈련다시하기 버튼에 이전 타임라인 콘텐츠데이터 넣어주면됨
            else if(MainManager.Instance.isStart == true)
            {
                Debug.Log("두번째 버튼 세팅");
                for (int cnt = 0; cnt < iS_ButtonDatas.Count; cnt++)
                {
                    if (iS_ButtonDatas[cnt].name.Contains("SelfPractice"))
                    {
                        buttonList[cnt].Init(iS_ButtonDatas[cnt]);
                        buttonList[cnt].SetActive(true);
                        for (int cnt2 = 0; cnt2 < contentsDatas.Count; cnt2++)
                        {
                            if (contentsDatas[cnt2].name.Contains("SelfPractice"))
                            {
                                int num = cnt;
                                int num2 = cnt2;
                                buttonList[num].RemoveAllListeners();
                                buttonList[num].AddListener((UnityAction)delegate
                                {
                                    NextContents(contentsDatas[num2]);
                                });
                            }
                        }
                    }
                    else if (iS_ButtonDatas[cnt].name.Contains("RePlay"))
                    {
                        buttonList[cnt].Init(iS_ButtonDatas[cnt]);
                        buttonList[cnt].SetActive(true);

                        for (int cnt2 = 0; cnt2 < contentsDatas.Count; cnt2++)
                        {
                            if (contentsDatas[cnt2].name.Contains(ChangeTitle(GetUserInfo.category1)))
                            {

                                int num = cnt;
                                int num2 = cnt2;
                                buttonList[num].RemoveAllListeners();
                                buttonList[num].AddListener((UnityAction)delegate
                                {
                                    NextContents(contentsDatas[num2]);
                                });
                            }
                        }
                    }
                }
            }

            if (GetUserInfo.contentcode.Contains("1") && MainManager.Instance.isStart == false)
            {
                if (GetUserInfo.nth == 0)
                {
                    buttonList[0].SetActive(true);
                    buttonList[1].SetActive(false);
                }
                else if (GetUserInfo.nth == 1)
                {
                    buttonList[0].SetActive(false);
                    buttonList[1].SetActive(true);
                }
                else if (GetUserInfo.nth >= 2)
                {
                    buttonList[0].SetActive(true);
                    buttonList[1].SetActive(true);
                }
            }          

        }

        public void SetButtonPostion(float x, float y, float z)
        {
            Vector3 vector3;
            vector3.x = x;
            vector3.y = y;
            vector3.z = z;
            buttonCanvas.GetComponent<RectTransform>().localPosition = vector3;
        }

        public void ButtonClick(string name)
        {
            Debug.Log(name + " : 클릭한 버튼" );
        }

        public void NextContents(ContentsData contentsDatas)
        {
            Debug.Log(contentsDatas.ContentsName + " : contentsDatas.ContentsName");
            MainManager.Instance.StartContentsData(contentsDatas);
        }

        public override void Show()
        {
            base.Show();
            //parent.SetActive(true);
            if (MainManager.Instance.isStart == false)
            {
                titleText.GetComponent<TextMeshProUGUI>().text = "감정을 완화하기 위한 훈련을 시작해봅시다.";
                dot.SetActive(false);
            }
            else
            {
                titleText.GetComponent<TextMeshProUGUI>().text = GetUserInfo.category1+" 훈련을 완료 하셨습니다.";
                dot.SetActive(true);
            }
        }

        public string ChangeTitle(string engTitle)
        {
            string title = "";
            switch (engTitle)
            {
                case "이완호흡":
                    title = "RelaxationRespiration";
                    break;
                case "마음챙김":
                    title = "Mindfulness";
                    break;
                case "자기자비":
                    title = "SelfMercy";
                    break; 
                case "정서주도 행동 바꾸기":
                    title = "EmotionalDriven";
                    break; 
                case "단어 반복":
                    title = "WordRepetition";
                    break; 
                case "자기 진정":
                    title = "SelfCalibration";
                    break; 
                case "자기주장":
                    title = "SelfAssertion";
                    break; 
                case "상담가 되기":
                    title = "Counselor";
                    break; 
                case "시각화":
                    title = "Visualization";
                    break; 
                case "긍정적 자기진술":
                    title = "PositiveSelfDiagnosis";
                    break; 
            }
            return title;
        }


        private List<ContentsData> contentsDataList = new List<ContentsData>();
        public void SaveContentsData(List<ContentsData> contentsDatas)
        {
            Debug.Log(contentsDatas.Count + " : SaveContentsData");
            contentsDataList.Clear();
            for (int cnt = 0; cnt < contentsDatas.Count; cnt++)
            {
                int copyCnt = cnt;
                ContentsData contentsData = contentsDatas[cnt];
                buttonList[copyCnt].AddListener((UnityAction)delegate {
                    Debug.Log(contentsData.ContentsName + " : contentsDatas[copyCnt]");
                    NextContents(contentsData);
                });
            }
            //contentsDataList.Add(contentsDatas[cnt]);
        }

    }
}