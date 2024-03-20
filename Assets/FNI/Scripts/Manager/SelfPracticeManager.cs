/// 작성자: 김윤빈
/// 작성일: 2020-08-11
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace FNI
{
    public class SelfPracticeManager : BaseScript
    {
        #region Singleton
        private static SelfPracticeManager _instance;
        public static SelfPracticeManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SelfPracticeManager>();
                    if (_instance == null)
                        Debug.LogError("SelfPracticeManager를 찾을 수 없습니다. ");
                }
                return _instance;
            }
        }
        #endregion

        public TextMeshProUGUI practiceText;

        public GameObject comment;

        // time은 0~1까지 deltaTime을 계속 더해서 지속시간으로 씀
        private float time = 0f;

        // 페이드 중인지 확인할 때 사용
        private bool isFade = false;

        private float F_time = 1f;

        private FNI_Record fni_Record;

        public ContentsData contentsData;

        private void OnEnable()
        {
            UIManager.Instance.OnObjectControl += AllOff;
        }

        private void Start()
        {
            fni_Record = FindObjectOfType<FNI_Record>();
        }

        private void Update()
        {
            if (OVRInput.GetUp(OVRInput.Button.One))
            {
                //StartCoroutine(FadeRoutine());
            }

            if (isRecordStart == true)
            {
                if (isRecordEnd)
                {
                    EndRecord();
                }
            }

        }

        public override void Show()
        {
            base.Show();
        }

        public override void AllOff(bool active)
        {
            base.AllOff(active);
        }

        public void EndRecord()
        {
            Debug.Log("녹음 끝");
            // 여기 콘텐츠 데이터를 넣어줄때 사용자 조건따라서 데이터 매니져에서 골라 넣어주면됨
            MainManager.Instance.StartContentsData(CheckVideo(GetUserInfo.contentcode, GetUserInfo.nth));
        }

        public ContentsData CheckVideo(string contentcode, int nth)
        {
            string secondVideo = "";
            ContentsData contentsData = null;
            MainManager.Instance.VideoNameRead();            

            if (secondVideo == "" || secondVideo == " ")
            {
                MainManager.Instance.VideoNameRead();
            }
            
            secondVideo = MainManager.Instance.videoName.videoName.Replace("1", "2");

            for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            {
                #region 보류
                //if (MainManager.Instance.videoName.videoName.Contains("1"))
                //{

                //}

                //switch (MainManager.Instance.videoName.videoName) 
                //{
                //    case "01_20M_1":
                //        secondVideo = "01_20M_2";
                //        break;

                //    case "01_20F_1":
                //        secondVideo = "01_20F_2";
                //        break;

                //    case "01_40F_1":
                //        secondVideo = "01_40F_2";
                //        break;

                //    case "01_40M_1":
                //        secondVideo = "01_40M_2";
                //        break;

                //        //////////////////////////////////////////////////////////////

                //    case "02_20M_1":
                //        secondVideo = "02_20M_2";
                //        break;

                //    case "02_20F_1":
                //        secondVideo = "02_20F_2";
                //        break;

                //    case "02_40F_1":
                //        secondVideo = "02_40F_2";
                //        break;

                //    case "02_40M_1":
                //        secondVideo = "02_40M_2";
                //        break;






                //    default:
                //        secondVideo = MainManager.Instance.videoName.videoName;
                //        break;
                //}
                #endregion 일단 보류 보류

                if (ContentsDataManager.Instance.emotionVideoList[cnt].name == secondVideo)
                {
                    Debug.Log(secondVideo + " : 두번째 영상");
                    contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                }
            }
            return contentsData;

            #region 기능 보류
            //if (contentcode.Contains("1"))
            //{
            //    MainManager.Instance.VideoNameRead();
            //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //    {
            //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == MainManager.Instance.videoName.videoName)
            //        {
            //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //        }
            //    }

            //    //if (nth == 0)
            //    //{
            //    //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //    //    {
            //    //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_20M_2")
            //    //        {
            //    //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //    //        }
            //    //    }
            //    //}
            //    //else if (nth == 1)
            //    //{
            //    //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //    //    {
            //    //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_20F_2")
            //    //        {
            //    //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //    //        }
            //    //    }
            //    //}
            //    //else if (nth == 2)
            //    //{
            //    //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //    //    {
            //    //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_40M_2")
            //    //        {
            //    //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //    //        }
            //    //    }
            //    //}
            //    //else if (nth == 3)
            //    //{
            //    //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //    //    {
            //    //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_40F_2")
            //    //        {
            //    //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //    //        }
            //    //    }
            //    //}
            //    //else if (nth > 3)
            //    //{
            //    //    List<ContentsData> DataList = new List<ContentsData>();

            //    //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //    //    {
            //    //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_20M_2" ||
            //    //            ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_20F_2" ||
            //    //            ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_40F_2" ||
            //    //            ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_40M_2")
            //    //        {
            //    //            DataList.Add(ContentsDataManager.Instance.emotionVideoList[cnt]);
            //    //        }
            //    //    }
            //    //    int num = UnityEngine.Random.Range(0, 4);
            //    //    contentsData = DataList[num];
            //    //}
            //}
            //else if (contentcode.Contains("2"))
            //{
            //    if (nth == 0)
            //    {
            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_20M_2")
            //            {
            //                contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //            }
            //        }
            //    }
            //    else if (nth == 1)
            //    {
            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_20F_2")
            //            {
            //                contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //            }
            //        }
            //    }
            //    else if (nth == 2)
            //    {
            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_40M_2")
            //            {
            //                contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //            }
            //        }
            //    }
            //    else if (nth == 3)
            //    {
            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_40F_2")
            //            {
            //                contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //            }
            //        }
            //    }
            //    else if (nth > 3)
            //    {
            //        List<ContentsData> DataList = new List<ContentsData>();

            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_20M_2" ||
            //                ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_20F_2" ||
            //                ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_40F_2" ||
            //                ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_40M_2")
            //            {
            //                DataList.Add(ContentsDataManager.Instance.emotionVideoList[cnt]);
            //            }
            //        }
            //        int num = UnityEngine.Random.Range(0, 4);
            //        contentsData = DataList[num];
            //    }
            //}
            //else if (contentcode.Contains("3"))
            //{
            //    if (nth == 0)
            //    {
            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_20M")
            //            {
            //                contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //            }
            //        }
            //    }
            //    else if (nth == 1)
            //    {
            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_20F")
            //            {
            //                contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //            }
            //        }
            //    }
            //    else if (nth == 2)
            //    {
            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_40M")
            //            {
            //                contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //            }
            //        }
            //    }
            //    else if (nth == 3)
            //    {
            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_40F")
            //            {
            //                contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //            }
            //        }
            //    }
            //    else if (nth > 3)
            //    {
            //        List<ContentsData> DataList = new List<ContentsData>();

            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_20M" ||
            //                ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_20F" ||
            //                ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_40F" ||
            //                ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_40M")
            //            {
            //                DataList.Add(ContentsDataManager.Instance.emotionVideoList[cnt]);
            //            }
            //        }
            //        int num = UnityEngine.Random.Range(0, 4);
            //        contentsData = DataList[num];
            //    }
            //}
            //else if (contentcode.Contains("4"))
            //{
            //    if (nth == 0)
            //    {
            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_20M")
            //            {
            //                contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //            }
            //        }
            //    }
            //    else if (nth == 1)
            //    {
            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_20F")
            //            {
            //                contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //            }
            //        }
            //    }
            //    else if (nth == 2)
            //    {
            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_40M")
            //            {
            //                contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //            }
            //        }
            //    }
            //    else if (nth == 3)
            //    {
            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_40F")
            //            {
            //                contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //            }
            //        }
            //    }
            //    else if (nth > 3)
            //    {
            //        List<ContentsData> DataList = new List<ContentsData>();

            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_20M" ||
            //                ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_20F" ||
            //                ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_40F" ||
            //                ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_40M")
            //            {
            //                DataList.Add(ContentsDataManager.Instance.emotionVideoList[cnt]);
            //            }
            //        }
            //        int num = UnityEngine.Random.Range(0, 4);
            //        contentsData = DataList[num];
            //    }
            //}
            //else if (contentcode.Contains("5"))
            //{
            //    if (nth == 0)
            //    {
            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_20M")
            //            {
            //                contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //            }
            //        }
            //    }
            //    else if (nth == 1)
            //    {
            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_20F")
            //            {
            //                contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //            }
            //        }
            //    }
            //    else if (nth == 2)
            //    {
            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_40M")
            //            {
            //                contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //            }
            //        }
            //    }
            //    else if (nth == 3)
            //    {
            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_40F")
            //            {
            //                contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
            //            }
            //        }
            //    }
            //    else if (nth > 3)
            //    {
            //        List<ContentsData> DataList = new List<ContentsData>();

            //        for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
            //        {
            //            if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_20M" ||
            //                ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_20F" ||
            //                ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_40F" ||
            //                ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_40M")
            //            {
            //                DataList.Add(ContentsDataManager.Instance.emotionVideoList[cnt]);
            //            }
            //        }
            //        int num = UnityEngine.Random.Range(0, 4);
            //        contentsData = DataList[num];
            //    }
            //}
            #endregion
        }



        public ContentsData SelectContent()
        {
            ContentsData contentsData = null;

            if (GetUserInfo.nth == 1)
            {
                contentsData = ContentsDataManager.Instance.emotionVideoList[0];
            }

            return contentsData;
        }

        bool isRecordStart = false;
        bool isRecordEnd = false;
        // 시작버튼에 넣어주면 됨
        public void StartButton()
        {
            isRecordStart = true;
            comment.SetActive(false);
            fni_Record.Show(RecordType.SoftTimeLimit, 180, true, true);
            //StartCoroutine(RecordRoutine());
        }

        // 1. 시작하면 배경 세팅 및 버튼있는 패널 켜주기

        // 2. 버튼 누르면 안내문구 잠시 나왔다가 사라지고 녹음 패널 On

        public GameObject backGround;
        public Texture recordTexture;
        public void StartSelfPractice()
        {
            // 배경 먼저 켜주고
            //BackGroundChanger.Instance.DefaultSettingRender();
            BackGroundChanger.Instance.SpaceSettingRender();
            //backGround.GetComponent<MeshRenderer>().material.mainTexture = recordTexture;
            //comment.SetActive(true);
            IEnumerator NextRoutine = NextStateRoutine();
            StartCoroutine(NextRoutine);
        }

        IEnumerator NextStateRoutine()
        {
            yield return new WaitForSeconds(0.3f);
            MainManager.Instance.NextState();
        }

        IEnumerator RecordRoutine()
        {
            isFade = true;

            Color alpha = practiceText.color;

            // 0으로 한번 초기화
            time = 0;

            while (alpha.a > 0f)
            {
                // 매 프레임 deltatime을 F_time으로 나눈 값을 time에 더해줌
                time += Time.deltaTime / F_time;
                // 부드럽게
                alpha.a = Mathf.Lerp(1, 0, time);

                practiceText.color = alpha;
                yield return null;
            }
            
            isFade = false;
            yield return null;
        }

        IEnumerator CountDownRoutine(int time1)
        {
            while (time1 > 0)
            {
                //timerText.text = time1.ToString();

                yield return new WaitForSeconds(1f);

                time1--;

            }
            yield return null;


        }

    }
}