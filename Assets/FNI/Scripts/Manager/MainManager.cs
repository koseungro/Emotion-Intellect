/// 작성자: 김윤빈
/// 작성일: 2020-06-22
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Events;
using System;
using System.Text;
using System.IO;
using DG.Tweening;
using UnityEngine.Playables;
using TMPro;

/// <summary>
/// contentsData의 리스트 순서대로 기능을 켜주고 꺼주면 됨 ㅇㅋ?
/// 
/// </summary>

namespace FNI
{
    public class VideoName 
    {
        public string videoName;
    }


    public enum PlayState
    {
        // 아무것도 하지않는 대기 상태
        Init,

        // 화면 어두워짐
        FadeOut,

        // 화면 밝아짐
        FadeIn,

        // 감정촉발 영상
        VideoLoadAndPlay,

        // 비디오가 재생되는 시간동안 대기
        WaitVideoTime,

        VideoPause,

        // 위치 설정?(3초)
        ResetPosition,

        // 버튼 나옴
        ButtonGroup,

        // 녹음
        Record,

        // 페이드 인, 아웃시 중간 딜레이 타임
        FadeDelayTime,

        // 안내 문구
        Guide,

        Animation,

        Quit,

        //이완 호흡
        Step1_1,
        //마음 챙김
        Step1_2,

        //자기 자비 훈련
        Step2_1,
        // 정서 주도 행동 바꾸기
        Step2_2,

        // 단어 반복 기법
        Step3_1,
        // 자기 진정 기법
        Step3_2,

        // 자기 주장 훈련
        Step4_1,
        // 상담가 되기
        Step4_2,

        // 시각화 기법(하늘 위에 구름)
        Step5_1,
        // 긍정적 자기 진술 훈련
        Step5_2
    }



    public class MainManager : BaseScript
    {

        DateTime startTime;
        DateTime endTime;

        #region Singleton
        private static MainManager _instance;
        public static MainManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MainManager>();
                    if (_instance == null)
                        Debug.LogError("MainManager를 찾을 수 없습니다. ");
                }
                return _instance;
            }
        }
        #endregion

        UIManager uiManager;

        IS_VideoPlayer videoPlayer;

        FNI_Record fni_Record;

        ContentsDataManager dataManager;

        ButtonGroupManager buttonGroupManager;

        AnimationManager animationManager;

        GuideManager guideManager;

        PlayState curState;

        public GameObject[] playableDirectors;

        private int ContentsPlayCount = 0;

        public ContentsData StartContents;

        ContentsData contentsData;

        // 현재 어떤 컨텐츠인지 반환
        public string Cur_ContentsName;

        // Timeline 시작전 == false / 시작 후 == true
        public bool isStart = false;

        public bool isContentsPlay = false;

        // 첫번째로 플레이 한 비디오 이름을 저장해놓고 두번째 저장할때도 같은걸 틀어줘야함
        //public string videoName;

        public void OnEnable()
        {
            IS_HMDManager.hmdPlayAction += delegate { HMDMounted(); };
            IS_HMDManager.hmdPauseAction += delegate { HMDUnMounted(); };   
        }
        string path;
        public void Start()
        {
            path = string.Format("{0}/../../Datas/playerData.json", Environment.CurrentDirectory); //Application.dataPath + "/playerData.json";
            isStart = false;
            //GetUserInfo.GetIniInfo();
            //ServerPath.GetIniInfo();
            GetUserInfo.GetIniInfo();
            ServerPath.GetIniInfo();

            Cur_ContentsName = StartContents.ContentsName;
            //pathTest();
            startTime = DateTime.Now;

            curState = PlayState.Init;

            dataManager = GetComponent<ContentsDataManager>();
            uiManager = GetComponent<UIManager>();
            videoPlayer = FindObjectOfType<IS_VideoPlayer>();
            fni_Record  = FindObjectOfType<FNI_Record>();
            buttonGroupManager = FindObjectOfType<ButtonGroupManager>();
            animationManager = FindObjectOfType<AnimationManager>();
            guideManager = FindObjectOfType<GuideManager>();

            Debug.Log(StartContents + " : StartContents");

            IEnumerator ResendRoutine = ResendJson();
            StartCoroutine(ResendRoutine);

            // 정보 가져오기
            //StartContents = dataManager.emotionVideoList

            if (GetUserInfo.unityNum == 1)
            {
                StartContentsData(CheckVideo(GetUserInfo.contentcode, GetUserInfo.nth));
            }
            else if (GetUserInfo.unityNum == 2)
            {
                StartContentsData(CheckContent(GetUserInfo.contentcode));
            }

            //StartContentsData(StartContents);

            //if (StartContents != null)
            //{
            //    StartContentsData(StartContents);
            //}



        }

        public ContentsData CheckContent(string contentcode)
        {
            ContentsData contentsData = null;

            if (contentcode.Contains("1"))
            {
                contentsData = ContentsDataManager.Instance.contentsDataList[0];
            }
            if (contentcode.Contains("2"))
            {
                contentsData = ContentsDataManager.Instance.contentsDataList[1];
            }
            if (contentcode.Contains("3"))
            {
                contentsData = ContentsDataManager.Instance.contentsDataList[2];
            }
            if (contentcode.Contains("4"))
            {
                contentsData = ContentsDataManager.Instance.contentsDataList[3];
            }
            if (contentcode.Contains("5"))
            {
                contentsData = ContentsDataManager.Instance.contentsDataList[4];
            }

            return contentsData;
        }
        public VideoName videoName = new VideoName();
        public ContentsData CheckVideo(string contentcode, int nth)
        {
            ContentsData contentsData = null;

            if (contentcode.Contains("1"))
            {
                List<ContentsData> DataList = new List<ContentsData>();

                for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                {
                    if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_20M_1" ||
                        ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_20F_1" ||
                        ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_40F_1" ||
                        ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_40M_1")
                    {
                        DataList.Add(ContentsDataManager.Instance.emotionVideoList[cnt]);
                    }
                }
                int num = UnityEngine.Random.Range(0, 4);
                
                Debug.Log(DataList[num].ContentsName + " : DataList[num].ContentsName");
                videoName.videoName = DataList[num].ContentsName;
                VideoNameSave();
                contentsData = DataList[num];
                //if (nth == 0)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_20M_1")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth == 1)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_20F_1")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth == 2)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_40M_1")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth == 3)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_40F_1")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth > 3)
                //{
                //    List<ContentsData> DataList = new List<ContentsData>();

                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_20M_1" ||
                //            ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_20F_1" ||
                //            ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_40F_1" ||
                //            ContentsDataManager.Instance.emotionVideoList[cnt].name == "01_40M_1")
                //        {
                //            DataList.Add(ContentsDataManager.Instance.emotionVideoList[cnt]);
                //        }
                //    }
                //    int num = UnityEngine.Random.Range(0,4);
                //    contentsData = DataList[num];
                //}
            }
            else if (contentcode.Contains("2"))
            {
                List<ContentsData> DataList = new List<ContentsData>();

                for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                {
                    if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_20M_1" ||
                        ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_20F_1" ||
                        ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_40F_1" ||
                        ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_40M_1")
                    {
                        DataList.Add(ContentsDataManager.Instance.emotionVideoList[cnt]);
                    }
                }
                int num = UnityEngine.Random.Range(0, 4);

                Debug.Log(DataList[num].ContentsName + " : DataList[num].ContentsName");
                videoName.videoName = DataList[num].ContentsName;
                VideoNameSave();
                contentsData = DataList[num];

                //if (nth == 0)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_20M_1")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth == 1)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_20F_1")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth == 2)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_40M_1")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth == 3)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_40F_1")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth > 3)
                //{
                //    List<ContentsData> DataList = new List<ContentsData>();

                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_20M_1" ||
                //            ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_20F_1" ||
                //            ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_40F_1" ||
                //            ContentsDataManager.Instance.emotionVideoList[cnt].name == "02_40M_1")
                //        {
                //            DataList.Add(ContentsDataManager.Instance.emotionVideoList[cnt]);
                //        }
                //    }
                //    int num = UnityEngine.Random.Range(0, 4);
                //    contentsData = DataList[num];
                //}
            }
            else if (contentcode.Contains("3"))
            {
                List<ContentsData> DataList = new List<ContentsData>();

                for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                {
                    if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_20M_1" ||
                        ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_20F_1" ||
                        ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_40F_1" ||
                        ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_40M_1")
                    {
                        DataList.Add(ContentsDataManager.Instance.emotionVideoList[cnt]);
                    }
                }
                int num = UnityEngine.Random.Range(0, 4);

                Debug.Log(DataList[num].ContentsName + " : DataList[num].ContentsName");
                videoName.videoName = DataList[num].ContentsName;
                VideoNameSave();
                contentsData = DataList[num];

                //if (nth == 0)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_20M")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth == 1)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_20F")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth == 2)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_40M")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth == 3)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_40F")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth > 3)
                //{
                //    List<ContentsData> DataList = new List<ContentsData>();

                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_20M" ||
                //            ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_20F" ||
                //            ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_40F" ||
                //            ContentsDataManager.Instance.emotionVideoList[cnt].name == "03_40M")
                //        {
                //            DataList.Add(ContentsDataManager.Instance.emotionVideoList[cnt]);
                //        }
                //    }
                //    int num = UnityEngine.Random.Range(0, 4);
                //    contentsData = DataList[num];
                //}
            }
            else if (contentcode.Contains("4"))
            {
                List<ContentsData> DataList = new List<ContentsData>();

                for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                {
                    if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_20M_1" ||
                        ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_20F_1" ||
                        ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_40F_1" ||
                        ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_40M_1")
                    {
                        DataList.Add(ContentsDataManager.Instance.emotionVideoList[cnt]);
                    }
                }
                int num = UnityEngine.Random.Range(0, 4);

                Debug.Log(DataList[num].ContentsName + " : DataList[num].ContentsName");
                videoName.videoName = DataList[num].ContentsName;
                VideoNameSave();
                contentsData = DataList[num];

                //if (nth == 0)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_20M")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth == 1)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_20F")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth == 2)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_40M")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth == 3)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_40F")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth > 3)
                //{
                //    List<ContentsData> DataList = new List<ContentsData>();

                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_20M" ||
                //            ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_20F" ||
                //            ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_40F" ||
                //            ContentsDataManager.Instance.emotionVideoList[cnt].name == "04_40M")
                //        {
                //            DataList.Add(ContentsDataManager.Instance.emotionVideoList[cnt]);
                //        }
                //    }
                //    int num = UnityEngine.Random.Range(0, 4);
                //    contentsData = DataList[num];
                //}
            }
            else if (contentcode.Contains("5"))
            {
                List<ContentsData> DataList = new List<ContentsData>();

                for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                {
                    if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_20M_1" ||
                        ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_20F_1" ||
                        ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_40F_1" ||
                        ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_40M_1")
                    {
                        DataList.Add(ContentsDataManager.Instance.emotionVideoList[cnt]);
                    }
                }
                int num = UnityEngine.Random.Range(0, 4);

                Debug.Log(DataList[num].ContentsName + " : DataList[num].ContentsName");
                videoName.videoName = DataList[num].ContentsName;
                VideoNameSave();
                contentsData = DataList[num];

                //if (nth == 0)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_20M")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth == 1)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_20F")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth == 2)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_40M")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth == 3)
                //{
                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_40F")
                //        {
                //            contentsData = ContentsDataManager.Instance.emotionVideoList[cnt];
                //        }
                //    }
                //}
                //else if (nth > 3)
                //{
                //    List<ContentsData> DataList = new List<ContentsData>();

                //    for (int cnt = 0; cnt < ContentsDataManager.Instance.emotionVideoList.Count; cnt++)
                //    {
                //        if (ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_20M" ||
                //            ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_20F" ||
                //            ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_40F" ||
                //            ContentsDataManager.Instance.emotionVideoList[cnt].name == "05_40M")
                //        {
                //            DataList.Add(ContentsDataManager.Instance.emotionVideoList[cnt]);
                //        }
                //    }
                //    int num = UnityEngine.Random.Range(0, 4);
                //    contentsData = DataList[num];
                //}
            }

            return contentsData;
        }
        
        private void Update()
        {
            if (contentsData != null)
            {
                Cur_ContentsName = contentsData.ContentsName;
            }            
            //if (OVRInput.GetUp(OVRInput.Button.Two))
            //{
            //    //NextState();
            //    //answer.category1 = "aaa";
            //    //StartCoroutine(answer.SendAnswerData());
            //    if (!IS_VideoPlayer.Instance.gameObject.activeSelf)
            //    {
            //        Time.timeScale += 3f;
            //        SubtitleManager.Instance.audioSource.pitch = 3f;
            //    }                
            //}

            //if (OVRInput.GetUp(OVRInput.Button.One))
            //{
            //    //StartCoroutine(answer.SendAnswerData());
            //    //NextState();
            //    //GetUserInfo.GetIniInfo();
            //    //ServerPath.GetIniInfo();
            //    //StartCoroutine(answer.SendAnswerData());
            //    if (!IS_VideoPlayer.Instance.gameObject.activeSelf)
            //    {
            //        Time.timeScale = 1f;
            //        SubtitleManager.Instance.audioSource.pitch = 1f;
            //    }
            //}

            if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstick))
            {
                OVRManager.display.RecenterPose();
            }

            //if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstickDown))
            //{
            //    testCanvas.SetActive(false);
            //}

            //if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstickUp))
            //{
            //    testCanvas.SetActive(true);
            //}
            //trackingSpaceText.text = trackingSpace.transform.position.x.ToString() + " : x // " + trackingSpace.transform.position.y.ToString() + " : y //  " + trackingSpace.transform.position.z.ToString() + " : z  -> trackingSpace";
            //ovrCameraRigText.text = ovrCameraRig.transform.position.x.ToString() + " : x // " + ovrCameraRig.transform.position.y.ToString() + " : y //  " + ovrCameraRig.transform.position.z.ToString() + " : z  -> ovrCameraRig";

            //if (Input.GetKeyDown(KeyCode.Z))
            //{
            //    Time.timeScale += 3f;
            //}
            //if (Input.GetKeyDown(KeyCode.X))
            //{
            //    Time.timeScale = 1f;
            //}

            
        }


        public GameObject testCanvas;

        public GameObject ovrCameraRig;
        public GameObject centerEye;
        public GameObject trackingSpace;

        public TextMeshProUGUI trackingSpaceText;
        public TextMeshProUGUI centerEyeText;
        public TextMeshProUGUI ovrCameraRigText;

        public ObjMoveTest objMove;

        public void PauseTimeLine()
        {
            for (int cnt = 0; cnt < playableDirectors.Length; cnt++)
            {
                if (playableDirectors[cnt].activeSelf == true)
                {
                    playableDirectors[cnt].GetComponent<PlayableDirector>().Pause();
                }
            }
        }

        public void StartTimeLine()
        {
            for (int cnt = 0; cnt < playableDirectors.Length; cnt++)
            {
                if (playableDirectors[cnt].activeSelf == true)
                {
                    if (objMove != null)
                    {
                        if (objMove.gameObject.activeSelf == true)
                        {
                            if (objMove.isFloating == true || objMove.isMove == true)
                            {

                            }
                            else
                            {
                                playableDirectors[cnt].GetComponent<PlayableDirector>().Play();
                            }
                        }
                    }
                    else if (playableDirectors[cnt].name == "SelfAssertion") //SelfAssertion.Instance.gameObject.activeSelf == true
                    {
                        if (SelfAssertion.Instance.isAction == true)
                        {
                            // 선긋기가 안끝났다면 타임라인를 Play하지 않습니다.
                        }
                        else
                        {
                            // 선긋기가 끝나면 HMD 재 착용시 Play
                            playableDirectors[cnt].GetComponent<PlayableDirector>().Play();
                        }
                    }
                    //else if (uiManager.lineDrawer.gameObject.activeSelf == true)
                    //{
                    //    Debug.Log("444444444");
                    //    if (uiManager.lineDrawer.isLine == true)
                    //    {

                    //    }
                    //    else
                    //    {
                    //        playableDirectors[cnt].GetComponent<PlayableDirector>().Play();
                    //    }
                    //}
                    else
                    {
                        playableDirectors[cnt].GetComponent<PlayableDirector>().Play();
                    }
                    
                }
            }
        }

        private void HMDMounted()
        {
            Time.timeScale = 1.0f;
            //SubtitleManager.Instance.audioSource.Play();
            if (isStart == true)
            {
                StartTimeLine();
            }
        }

        private void HMDUnMounted()
        {
            //SubtitleManager.Instance.audioSource.Pause();
            if (isStart == true)
            {
                PauseTimeLine();
            }
            Time.timeScale = 0f;

            if (isEnd == true)
            {
                Debug.Log("유니티 종료");
                Application.Quit();
                
            }            

        }


        public void Init()
        {
            // 카운트를 0으로 만들어서 처음부터 진행되도록 해줘야함
            ContentsPlayCount = 0;
            uiManager.ScreenState(UIState.None);
        }



        public void StartContentsData(ContentsData startContents)
        {
            Init();
            Debug.Log(startContents.name + " : startContents");
            //for (int cnt = 0; cnt < dataManager.contentsDataList.Count; cnt++)
            //{
            //    if (dataManager.contentsDataList[cnt] != null)
            //    {
            //        if (startContents.name == dataManager.contentsDataList[cnt].ContentsName)
            //        {
            //            contentsData = dataManager.contentsDataList[cnt];
            //            isContentsPlay = true;                       
            //        }
            //    }
            //}
            contentsData = startContents;
            NextState();
        }




        /// <summary>
        /// 기존에 저장된 json파일이 있다면 다시 한번 보내주고 파일 삭제
        /// </summary>
        public void CheckJson()
        {           
            //if (File.Exists(path) == true)
            //{
            //    Debug.Log("True");
            //    IEnumerator JsonRoutine;
            //    JsonRoutine = ResendJson();
            //    StartCoroutine(JsonRoutine);
            //}
        }
        
        public UserInfoJsonObject userInfo;
        //string path = string.Format("{0}/../../Datas/playerData.json", Environment.CurrentDirectory);

        IEnumerator ResendJson()
        {
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists == false)
            {
                //파일 없는경우

                Debug.Log("파일 없음");
                yield break;
            }
            else
            {
                Debug.Log("파일 있음");
            }


            LeadJson();
            WWWForm form1 = new WWWForm();
            string UserInfoDataJsonString = JsonUtility.ToJson(userInfo, prettyPrint: true);
            form1.AddField("json", UserInfoDataJsonString);
            WWW www1 = new WWW(ServerPath.sendDataPath, form1.data);
            yield return www1;

            if (www1.error == null)
            {
                if (www1.text == "OK")
                {
                    Debug.Log("전송완료");
                    File.Delete(path);
                }
            }
            else
            {
                Debug.Log("WWW Error: " + www1.error);
                int cnt = 0;
                while (cnt < 1)
                {
                    yield return www1;
                    cnt++;
                }
                // 한번 더 전송
                if (www1.error == null)
                {
                    if (www1.text == "OK")
                    {
                        File.Delete(path);
                    }
                }
                else
                {
                    //string path = Application.dataPath + "/playerData.json";
                    File.WriteAllText(path, UserInfoDataJsonString);
                }
            }
        }

        public void LeadJson()
        {
            //string path; //= Application.dataPath + "/playerData.json";
            //path = string.Format("{0}/../../Datas/playerData.json", Environment.CurrentDirectory);

            //FileInfo fileInfo = new FileInfo(path);

            //파일 있는경우
            string jsonData = File.ReadAllText(path);
            userInfo = JsonUtility.FromJson<UserInfoJsonObject>(jsonData);
        }





        public void NextState()
        {
            Debug.Log("NextState");
            if (CheckContents(ContentsPlayCount) == false)
            {
                State(contentsData);                
            }
            //else
            //{
            //    QuitApplication();
            //}
        }

        public bool CheckContents(int count)
        {
            bool isEnd = false;

            if (count >= contentsData.ContentsList.Count)
            {
                isEnd = true;
            }
            else
            {
                isEnd = false;
            }

            return isEnd;

        }
        public Answer answer = new Answer();
        public void State(ContentsData contentsData)
        {
            if (ContentsPlayCount < contentsData.ContentsList.Count)
            {
                switch (contentsData.ContentsList[ContentsPlayCount].contentType)
                {                    
                    case PlayState.ButtonGroup:
                        Debug.Log(ContentsPlayCount + " : ButtonGroup");
                        uiManager.ScreenState(UIState.ButtonGroup);
                        buttonGroupManager.SetButtonPostion(contentsData.ContentsList[ContentsPlayCount].xPos, contentsData.ContentsList[ContentsPlayCount].yPos, contentsData.ContentsList[ContentsPlayCount].zPos);
                        
                        buttonGroupManager.SetButtonData(contentsData.ContentsList[ContentsPlayCount].buttonData, contentsData.ContentsList[ContentsPlayCount].nextContentsList);                       
                        break;

                    case PlayState.Record:
                        Debug.Log("Record");
                        //fni_Record.Show();
                        //fni_Record.RecordReady();
                        uiManager.ScreenState(UIState.None);
                        SelfPracticeManager.Instance.StartSelfPractice();
                        SelfPracticeManager.Instance.Show();                        
                        break;

                    case PlayState.VideoLoadAndPlay:
                        BackGroundChanger.Instance.DefaultSettingRender();
                        if (IS_VideoPlayer.Instance.gameObject.activeSelf == false)
                        {
                            IS_VideoPlayer.Instance.gameObject.SetActive(true);
                        }
                        Debug.Log("VideoLoadAndPlay");
                        IS_VideoPlayer.Instance.MyVideoPlayer.isLooping = false;
                        Debug.Log(contentsData.ContentsList[ContentsPlayCount].movieName + " :VideoPlay");
                        IS_VideoPlayer.Instance.MovieLoad(contentsData.ContentsList[ContentsPlayCount].movieName);
                        IS_VideoPlayer.Instance.PreparedPlay(true);
                        uiManager.ScreenState(UIState.None);
                        break;

                    case PlayState.WaitVideoTime:
                        // 영상이 재생되는 시간동안 기다렸다가 끝나면 다음으로 넘겨줌
                        Debug.Log("WaitVideoTime");
                        IEnumerator WaitTimeRoutine = DelayRoutine(contentsData.ContentsList[ContentsPlayCount].waitVideoTime);
                        StartCoroutine(WaitTimeRoutine);
                        break;

                    case PlayState.FadeOut:
                        // 화면 어두워짐
                        Debug.Log("FadeOut");
                        uiManager.FadeOut();
                        //uIManager.ScreenState(UIState.FadeOut);
                        break;

                    case PlayState.FadeIn:
                        // 화면 밝아짐
                        Debug.Log("FadeIn");
                        uiManager.FadeIn();
                        //uIManager.ScreenState(UIState.FadeOut, contentsData.ContentsList[ContentsPlayCount].delayTime);
                        break;

                    case PlayState.FadeDelayTime:
                        Debug.Log(contentsData.ContentsList[ContentsPlayCount].delayTime + " : FadeDelayTime");
                        //StartCoroutine(DelayRoutine(contentsData.ContentsList[count].delayTime));
                        Invoke("NextState", contentsData.ContentsList[ContentsPlayCount].delayTime);
                        break;

                    case PlayState.VideoPause:
                        Debug.Log("VideoPause");
                        IS_VideoPlayer.Instance.Pause();
                        break;

                    case PlayState.Animation:
                        Debug.Log(contentsData.ContentsList[ContentsPlayCount].animationName + " : Animation");
                        // UI는 전부 꺼주고
                        // 애니만 재생해주면 될듯
                        uiManager.ScreenState(UIState.None);
                        animationManager.Show();
                        //AnimationManager.Instance.playableAssetName = contentsData.ContentsList[ContentsPlayCount].animationName;
                        AnimationManager.Instance.LoadAnimation(contentsData.ContentsList[ContentsPlayCount].animationName);
                        //AnimationManager.Instance.LoadAnimation("Step1Timeline2");
                        break;

                    case PlayState.Guide:
                        guideManager.emotionOption = contentsData.ContentsList[ContentsPlayCount].emotionVideoOption;
                        guideManager.SetComment(contentsData.ContentsList[ContentsPlayCount].emotionVideoOption);
                        uiManager.ScreenState(UIState.Guide);
                        break;

                    case PlayState.ResetPosition:
                        uiManager.ScreenState(UIState.resetPosition);
                        break;

                    case PlayState.Quit:
                        //QuitApplication();
                        Debug.Log("정보 전송 후 종료");
                        //answer.category1 = "aaa";

                        BackGroundChanger.Instance.DefaultSettingRender();
                        uiManager.ScreenState(UIState.None);
                        StartCoroutine(answer.SendAnswerData());
                        IEnumerator ExitRoutine;
                        ExitRoutine = QuitRoutine();
                        StartCoroutine(ExitRoutine);
                        break;
                }
                ContentsPlayCount++;
            }            
        }


        public void VideoNameSave()
        {
            string str = JsonUtility.ToJson(videoName, prettyPrint: true);
            Debug.Log(str);
            DirectoryInfo di = new DirectoryInfo(Application.streamingAssetsPath + "/VideoData");
            if (di.Exists == false)
            {
                di.Create();
                Debug.Log(di + " : VideoData 폴더 생성");
            }
            else if(di.Exists == true)
            {
                Debug.Log(di+ " : VideoData 폴더 있음");
            }


            FileInfo file = new FileInfo(di + "\\" + GetUserInfo.id + "-" + "videoName" + ".txt");

            // 파일이 있다면
            if (file.Exists == true)
            {
                file.Delete();

                FileStream fs = file.Create();
                TextWriter tw = new StreamWriter(fs);
                tw.Write(str);
                tw.Close();
                fs.Close();
            }
            // 파일이 없다면
            else if (file.Exists == false)
            {
                FileStream fs = file.Create();
                TextWriter tw = new StreamWriter(fs);
                tw.Write(str);
                tw.Close();
                fs.Close();
            }
        }

        public void VideoNameRead()
        {
            DirectoryInfo di = new DirectoryInfo(Application.streamingAssetsPath + "/VideoData");

            FileInfo file = new FileInfo(di + "\\" + GetUserInfo.id + "-" + "videoName" + ".txt");

            // 파일이 있다면
            if (file.Exists == true)
            {
                string jsonData = File.ReadAllText(di + "\\" + GetUserInfo.id + "-" + "videoName" + ".txt");
                videoName = JsonUtility.FromJson<VideoName>(jsonData);

                Debug.Log(videoName.videoName + " : 읽어옴");
            }
            else if (file.Exists == false)
            {
                Debug.Log("파일 없음 못읽어옴");
            }
        }


        public bool isEnd = false;

        public GameObject quitCanvas;
        public GameObject quitText;
        public GameObject quitImage;

        IEnumerator QuitRoutine()
        {
            Debug.Log("QuitRoutine 실행합니다.");
            while (true)
            {
                quitCanvas.SetActive(true);
                quitImage.SetActive(false);
                quitText.SetActive(true);
                if (isEnd == true)
                {
                    quitCanvas.SetActive(true);
                    quitText.SetActive(false);
                    quitImage.SetActive(true);
                    break;
                }
                yield return null;
            }
            NextState();
            yield return null;
        }


        IEnumerator DelayRoutine(float delayTime)
        {                        
            yield return new WaitForSeconds(delayTime);
            Debug.Log(delayTime + ": delayTime");
            //일시정지 혹은 정지
            IS_VideoPlayer.Instance.Pause(); 
            NextState();
        }


    }
}