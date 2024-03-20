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

namespace FNI
{
    public enum UIState 
    {

        None,

        // 감정촉발 영상
        VideoPlay,

        ButtonGroup,

        Animation,

        TakeOffHMD,

        Guide,

        resetPosition,

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


    /// <summary>
    /// UI Manager
    /// 페이드 인, 아웃 이미지 관리
    /// HMD착용 안내 이미지 관리
    /// </summary>
    public class UIManager : BaseScript
    {
        #region Singleton
        private static UIManager _instance;
        public static UIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<UIManager>();
                    if (_instance == null)
                        Debug.LogError("UIManager를 찾을 수 없습니다. ");
                }
                return _instance;
            }
        }
        #endregion

        [ColorHeader("HMD 착용 안내 이미지")]
        public GameObject HMDImage;

        [ColorHeader("녹음 오브젝트")]
        public FNI_Record FNI_Record;

        [ColorHeader("컨트롤러 안내 캔버스")]
        public GameObject ControllerCanvas;

        [ColorHeader("자리 조정")]
        public PositionManager positionManager;

        [ColorHeader("라인 그리기")]
        public LineDrawer lineDrawer;

        [ColorHeader("안내 화면")]
        public GuideManager guideManager;

        [ColorHeader("Fade 이미지")]
        public Image fadeImage;

        [ColorHeader("MainManager")]
        public MainManager mainManager;

        [ColorHeader("MainManager")]
        public ButtonGroupManager buttonGroupManager;

        private float F_time = 1.5f;

        [ColorHeader("비디오 플레이어")]
        public GameObject videoPlayer;

        [ColorHeader("종료 안내 캔버스")]
        public GameObject QuitCanvas;


        // time은 0~1까지 deltaTime을 계속 더해서 지속시간으로 씀
        private float time = 0f;

        // 페이드 중인지 확인할 때 사용
        private bool isFade = false;

        // 시작하자마자 오브젝트를 전부 꺼주기 위한 용도, 여기에 함수를 추가해주면 됨
        public ObjectControlHandler OnObjectControl; 

        private void Start()
        {
            IS_HMDManager.hmdPlayAction += delegate { HMDImageOnOff(false); };
            IS_HMDManager.hmdPauseAction += delegate { HMDImageOnOff(true); };

            //OnObjectControl(false);
        }


        public void ScreenState(UIState uIState)
        {
            switch (uIState)
            {
                case UIState.None:
                    OnObjectControl(false);
                    break;

                case UIState.ButtonGroup:
                    OnObjectControl(false);
                    buttonGroupManager.Show();
                    IEnumerator NextRoutine = NextStateRoutine();
                    StartCoroutine(NextRoutine);
                    break;

                case UIState.TakeOffHMD:
                    OnObjectControl(false);
                    break;

                case UIState.resetPosition:
                    OnObjectControl(false);
                    if (GetUserInfo.unityNum == 1 )
                    {
                        positionManager.Show();
                        //positionManager.numImage.SetActive(true);
                        //positionManager.textImage.SetActive(true);
                        positionManager.SetPosition();
                        positionManager.mainText.text = "편안한 자세로 정면을 응시해주세요.";
                    }
                    else if(GetUserInfo.unityNum > 1)
                    {
                        if (MainManager.Instance.Cur_ContentsName.Contains("Content") == true)
                        {
                            positionManager.Show();
                            positionManager.SetPosition();
                            positionManager.mainText.text = "편안한 자세로 정면을 응시해주세요.";
                        }
                        else
                        {
                            positionManager.Show();
                            positionManager.SetPosition();
                            positionManager.mainText.text = "잠시 후 영상이 시작됩니다.";
                        }

                    }

                    break;

                case UIState.Guide:
                    OnObjectControl(false);                    
                    guideManager.Show();
                    //StartCoroutine(NextStateRoutine());
                    break;

            }
            //mainManager.NextState();            
            
        }

        IEnumerator NextStateRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            mainManager.NextState();
        }

        public void SetActiveControllerCanvas(bool value)
        {
            ControllerCanvas.gameObject.SetActive(value);
        }

        /// <summary>
        /// HMD를 벗거나 쓸 때 HMD 착용 안내 이미지를 꺼주고 켜줍니다.
        /// </summary>
        /// <param name="value"></param>
        public void HMDImageOnOff(bool value)
        {
            if (mainManager.isEnd != true)
            {
                HMDImage.SetActive(value);
            }
        }

        #region 페이드 인 아웃 루틴

        /// <summary>
        /// 페이드 인, 아웃을 한번에 실행합니다.
        /// </summary>
        public void AutoFade()
        {
            StartCoroutine(FadeRoutine());
        }

        /// <summary>
        /// 페이드 아웃만 실행합니다.
        /// (화면을 가립니다.)
        /// </summary>
        public void FadeOut()
        {
            IEnumerator fadeOut = FadeOutRoutine();
            StartCoroutine(fadeOut);
        }

        /// <summary>
        /// 페이드 인만 실행합니다.
        /// (화면이 다시 보입니다.)
        /// </summary>
        public void FadeIn()
        {
            IEnumerator fadeIn = FadeInRoutine();
            StartCoroutine(fadeIn);
        }

        public IEnumerator FadeRoutine()
        {
            isFade = true;

            Color alpha = fadeImage.color;

            // 0으로 한번 초기화
            time = 0;

            //페이드 아웃 먼저, 알파값이 1보다 작으면 계속 반복
            while (alpha.a < 1f)
            {
                // 매 프레임 deltatime을 F_time으로 나눈 값을 time에 더해줌
                time += Time.deltaTime / F_time;
                // 부드럽게
                alpha.a = Mathf.Lerp(0, 1, time);

                fadeImage.color = alpha;
                yield return null;
            }

            // 0으로 한번 초기화
            time = 0;


            // 1초 대기시간
            yield return new WaitForSeconds(1f);

            while (alpha.a > 0f)
            {
                // 매 프레임 deltatime을 F_time으로 나눈 값을 time에 더해줌
                time += Time.deltaTime / F_time;
                // 부드럽게
                alpha.a = Mathf.Lerp(1, 0, time);

                fadeImage.color = alpha;
                yield return null;
            }
            mainManager.NextState();
            isFade = false;
            yield return null;
        }

        /// <summary>
        /// 화면 어두워짐
        /// </summary>
        /// <returns></returns>
        public IEnumerator FadeOutRoutine()
        {
            isFade = true;

            Color alpha = fadeImage.color;

            // 0으로 한번 초기화
            time = 0;
            //페이드 아웃 먼저, 알파값이 1보다 작으면 계속 반복
            while (alpha.a < 1f)
            {
                // 매 프레임 deltatime을 F_time으로 나눈 값을 time에 더해줌
                time += Time.deltaTime / F_time;
                // 부드럽게
                alpha.a = Mathf.Lerp(0, 1, time);

                fadeImage.color = alpha;
                yield return null;
            }            
            isFade = false;
            yield return null;
            mainManager.NextState();
        }

        /// <summary>
        /// 화면 밝아짐
        /// </summary>
        /// <returns></returns>
        public IEnumerator FadeInRoutine()
        {
            isFade = true;

            Color alpha = fadeImage.color;

            // 0으로 한번 초기화
            time = 0;
            // 1초 대기시간
            //yield return new WaitForSeconds(delayTime);

            while (alpha.a > 0f)
            {
                // 매 프레임 deltatime을 F_time으로 나눈 값을 time에 더해줌
                time += Time.deltaTime / F_time;
                // 부드럽게
                alpha.a = Mathf.Lerp(1, 0, time);

                fadeImage.color = alpha;
                yield return null;
            }
            
            isFade = false;
            yield return null;
            mainManager.NextState();
        }
        #endregion
    }
}