/// 작성자: 백인성 
/// 작성일: 2018-05-01 
/// 수정일: 2018-07-25
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력 
/// (2018-07-25) 백인성 
///    1. 함수 구조 수정 및 네이밍 규칙 적용

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.Video;
using UnityEngine.Events;
using FNI;
using System.IO;
using System.Text;
using UnityEngine.UI;

public partial class IS_VideoPlayer : MonoBehaviour
{
	#region Singleton
	private static IS_VideoPlayer _instance;
	public static IS_VideoPlayer Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<IS_VideoPlayer>();
				if (_instance == null)
					Debug.Log("IS_VideoPlayer를 찾을 수 없습니다. ");
			}
			return _instance;
		}
	}
    #endregion

    #region Property
    /// <summary>
    /// 현재 가지고 있는 비디오 플레이어입니다.
    /// </summary>
    public VideoPlayer MyVideoPlayer
	{
		get
		{
            if (m_VideoPlayer == null)
            {
                if (this.name == "Video360Player")
                {
                    m_VideoPlayer = transform.Find("VR3D").GetComponent<VideoPlayer>();
                }
                else
                {
                    m_VideoPlayer = transform.Find("GT_sphere90_vod_part").GetComponent<VideoPlayer>();
                }
                
            }
                


            return m_VideoPlayer;
		}
	}
	/// <summary>
	/// 영상의 메트리얼입니다.
	/// </summary>
	//public Material MyMat
	//{
	//	get
	//	{
	//		if (m_Mat == null)
	//			m_Mat = transform.Find("VR3D").GetComponent<MeshRenderer>().material;
	//		return m_Mat;
	//	}
	//}
    public Material MyMat
    {
        get
        {
            if (m_Mat == null)
            {
                if (this.name == "Video360Player")
                {
                    m_Mat = transform.Find("VR3D").GetComponent<MeshRenderer>().material;
                }
                else
                {
                    m_Mat = transform.Find("GT_sphere90_vod_part").GetComponent<MeshRenderer>().material;
                }
            }
            return m_Mat;
        }
    }
    /// <summary>
    /// 영상 메트리얼의 색상입니다.
    /// </summary>
    public Color MatColor
	{
		get
		{
			return MyMat.GetColor("_Color");
		}
		set
		{
			MyMat.SetColor("_Color", value);
		}
	}
	/// <summary>
	/// 현재 영상의 볼륨입니다.
	/// </summary>
	public float Valume
	{
		get
		{
			if (m_AudioSource == null)
				m_AudioSource = GetComponent<AudioSource>();

			return m_AudioSource.volume;
		}
		set
		{
			if (m_AudioSource == null)
				m_AudioSource = GetComponent<AudioSource>();

			m_AudioSource.volume = Mathf.Clamp01(value);
		}
	}
	/// <summary>
	/// 영상이 무엇이든 플레이 중인지 확인합니다.
	/// </summary>
	public bool IsPlaying { get { return MyVideoPlayer.isPlaying; } }
	/// <summary>
	/// 현재 플레이어가 영상을 반복하는 상태인제 확인합니다.
	/// </summary>
	public bool IsLooping { get { return m_isLoop; } }
	/// <summary>
	/// 영상이 준비 되었는지 확인합니다.
	/// </summary>
	public bool IsPrepared { get { return MyVideoPlayer.isPrepared; } }
	/// <summary>
	/// 영상재생이 완료 되었는지 확인합니다.
	/// </summary>
	public bool IsDone { get { return m_isDone; } }
	/// <summary>
	/// 재생중인 영상의 현재 프레임입니다.
	/// </summary>
	public long Frame { get { return MyVideoPlayer.frame; } set { MyVideoPlayer.frame = value; } }
	/// <summary>
	/// 재생중인 영상의 현재 시간입니다.
	/// </summary>
	public double nTime { get { return MyVideoPlayer.time; } }
	/// <summary>
	/// 영상의 총 길이입니다.
	/// </summary>
	public ulong Duration { get { return (ulong)(MyVideoPlayer.frameCount / MyVideoPlayer.frameRate); } }
	/// <summary>
	/// 현재 시간의 노말라이즈 입니다.
	/// </summary>
	public double NTime { get { return nTime / Duration; } set { MyVideoPlayer.time = value * Duration; } }

    public long NFrame { get { return Frame; } }

    public bool IsSeeking { get { return isSeeking; } }

    #endregion

    #region Public
    public bool showDebug = false;
	public double curTime = 0;
    public long curFrame;
	public GameObject noMovieTarget;
    #endregion

    #region Private


    private bool m_isLoop = false;
	/// <summary>
	/// 영상이 재생중인지 체크합니다.
	/// </summary>
    private bool m_isDone;

	/// <summary>
	/// Seek 시작시간
	/// </summary>
	private double m_sTime;
	/// <summary>
	/// Seek 종료시간
	/// </summary>
	private double m_eTime;

    private bool isSeeking = false;

    private bool loadAndFirstPlay = false; 

    private Material m_Mat;
    private VideoPlayer m_VideoPlayer;
    private AudioSource m_AudioSource;
	/// <summary>
	/// 시크에서 사용할 엑션입니다.
	/// </summary>
	private UnityAction m_action;
	private IEnumerator m_lateSeek_Routine;
    //private IEnumerator m_Playcheck_Routine;
    //private bool isSeeking = false;

    private bool isHMDPause = false;


    #endregion

    #region Event
    /// <summary>
    /// 이벤트 델리게이트
    /// </summary>
    /// <param name="videoEvent">이벤트의 종류입니다.</param>
    /// <param name="message">이벤트에 따른 메시지를 전송합니다.</param>
    public delegate void VideoEvent(VideoEventType videoEvent, IS_VideoPlayer source, string message);
    /// <summary>
    /// 영상재생 관련이벤트입니다.
    /// </summary>
    public event VideoEvent VideoFeedback;

    private void ErrorReceived_Event(VideoPlayer source, string message)
    {
        Debug.Log("[" + name + "] play Err : " + message);
		if(VideoFeedback != null)
			VideoFeedback(VideoEventType.ErrorReceived, this, message);
    }
    private void FrameReady_Event(VideoPlayer source, long frameIdx)
    {
        Debug.Log("[" + name + "] FrameReady : " + frameIdx);
		if (VideoFeedback != null)
			VideoFeedback(VideoEventType.FrameReady, this, "FrameReady : " + frameIdx);
    }
    private void LoopPointReached_Event(VideoPlayer source)
    {
        Debug.Log("[" + name + "] LoopPointReached");
		if (VideoFeedback != null)
			VideoFeedback(VideoEventType.LoopPointReached, this, "LoopPointReached");

        m_isDone = true;
    }

    private void PrepareCompleted_Event(VideoPlayer source)
    {
        Debug.Log("[" + name + "] Video PrepareCompleted");
		if (VideoFeedback != null)
			VideoFeedback(VideoEventType.PrepareCompleted, this, "PrepareCompleted");

        m_isDone = false;
    }
    private void SeekCompleted_Event(VideoPlayer source)
    {
        Debug.Log("[" + name + "] SeekCompleted");
		if (VideoFeedback != null)
			VideoFeedback(VideoEventType.SeekCompleted, this, "SeekCompleted");
        m_isDone = false;
    }
    private void Started_Event(VideoPlayer source)
    {
        Debug.Log("[" + name + "] Started");
		if (VideoFeedback != null)
			VideoFeedback(VideoEventType.Started, this, "Started");
    }
	#endregion
	
	#region Unity base method
	private void Awake()
	{
		MyVideoPlayer.errorReceived    += ErrorReceived_Event;
		MyVideoPlayer.frameReady       += FrameReady_Event;
		MyVideoPlayer.loopPointReached += LoopPointReached_Event;
		MyVideoPlayer.prepareCompleted += PrepareCompleted_Event;
		MyVideoPlayer.seekCompleted    += SeekCompleted_Event;
		MyVideoPlayer.started          += Started_Event;

		MyVideoPlayer.playOnAwake = false;


        IS_HMDManager.hmdPlayAction += delegate
        {
            if (this.gameObject.activeSelf == true)
            {
                if (IsPlaying == false && IsPrepared)
                {
                    Play();
                    isHMDPause = false;
                }                    
            }
        };
        IS_HMDManager.hmdPauseAction += delegate
        {
            if (this.gameObject.activeSelf == true)
            {
                
                if (IsPlaying == true)
                {
                    Pause();
                    isHMDPause = true;
                }                    
            }
        };
    }
    //private void Disable()
    //{
    //    MyVideoPlayer.errorReceived -= ErrorReceived_Event;
    //    MyVideoPlayer.frameReady -= FrameReady_Event;
    //    MyVideoPlayer.loopPointReached -= LoopPointReached_Event;
    //    MyVideoPlayer.prepareCompleted -= PrepareCompleted_Event;
    //    MyVideoPlayer.seekCompleted -= SeekCompleted_Event;
    //    MyVideoPlayer.started -= Started_Event;
    //}

    //private void OnApplicationFocus(bool focus)
    //{
    //    if (focus == false)
    //    {
    //        EffectSoundManager.Instance.EffectSource.Stop();
    //        BGMManager.Instance.BGMInit();
    //    }

    //    if (focus == true)
    //    {
    //        StartCoroutine(InitRoutine());
    //        //fileCheck();
    //    }
    //}


    private void Update()
    {
        curTime = nTime;
        curFrame = NFrame;


        if (IsPlaying)
        {
            if (IsLooping)
            {
                //Debug.Log(m_eTime + " < " + Time + " = " + (m_eTime < Time));
                if (m_eTime != 0 && m_eTime < nTime && isSeeking == false)
                {
                    Seek(m_sTime, m_eTime, true);

                    if (m_action != null)
                        m_action();
                }
            }
        }

        if (OVRInput.GetUp(OVRInput.Button.Four))
        {
            if (IsPlaying == true)
            {
                Pause();
                Time.timeScale = 0f;
            }
            else if (IsPlaying == false && IsPrepared == true)
            {
                Time.timeScale = 1f;
                Play();
            }            
        }
        if (OVRInput.GetUp(OVRInput.Button.Three))
        {
            if (IsPlaying == true)
            {
                Application.Quit();
            }            
        }

        if (IsPlaying == true && Input.GetKeyDown(KeyCode.Q))
        {
            //Time.timeScale += 3f;
        }
        if (IsPlaying == true && Input.GetKeyDown(KeyCode.W))
        {
            //Time.timeScale = 1f;
        }

    }
    #endregion

    /// <summary>
	/// 영상을 불러옵니다.
	/// </summary>
	/// <param name="path">확장자는 .mp4, .avi, .mov</param>
	public void LoadVideo()
    {
        //if (MyVideoPlayer.url == path)
        //	return;

        //MyVideoPlayer.clip = clip;
        MyVideoPlayer.Prepare();
        loadAndFirstPlay = true;

        Debug.Log("Can Set direct audio Volume : " + MyVideoPlayer.canSetDirectAudioVolume);
        Debug.Log("Can Set playback Speed : " + MyVideoPlayer.playbackSpeed);
        Debug.Log("Can Set skip on drop : " + MyVideoPlayer.skipOnDrop);
        Debug.Log("Can Set time : " + MyVideoPlayer.canSetTime);
        Debug.Log("Can step : " + MyVideoPlayer.canStep);
    }

    public string Path()
    {
        string path = "";

        // 1번 조검 GetUserInfo.contentcode 로 나눔
        switch (GetUserInfo.contentcode)
        {
            case "code1":
                path = "#1. 스트레스 사건들";
                break;

            case "code2":
                path = "#2. 자존감이 낮아지는 사건들";
                break;

            case "code3":
                path = "#3. 수동 공격하는 사람들";
                break;

            case "code4":
                path = "#4. 정당한 주장을 막는 사람들";
                break;

            case "code5":
                path = "#5. 불합리한 비난을 하는 사람들";
                break;
        }

        return path;
    }

    public void MovieLoad (string fileName)
    {
       
        string path;


#if UNITY_EDITOR
        StringBuilder sb = new StringBuilder("");
        sb.Append(@"file:///");
        sb.Append(Application.dataPath);
        sb.Replace("Assets", "Videos/");
        sb.Append(Path());
        sb.Append(fileName);
        path = sb.ToString();

#else
        //path = string.Format("{0}/../../fileName", Application.persistentDataPath);
        //path = Application.persistentDataPath + "/Videos/" + fileName;

        //path = System.Environment.CurrentDirectory.ToString();

        path = string.Format("{0}/../../Videos/" + fileName, Environment.CurrentDirectory);

#endif



        Debug.Log(path + " : path");
        MyVideoPlayer.source = VideoSource.Url;
        MyVideoPlayer.url = path;

        //MyVideoPlayer.url = "C:/Users/yren/Desktop/EITest/0806_test.mp4";

        if (MyVideoPlayer.url != null)
        {
            MyVideoPlayer.Prepare();
        }
    }

	public void LoadAndPlay(string name)
	{
        MovieLoad(name);

        LoadVideo();

        if ( MyVideoPlayer.isPlaying == false) //MyVideoPlayer.url != path &&
        {
            //Play();
            StartCoroutine(LatePlay_Routine());
        }        
    }

	public IEnumerator LatePlay_Routine()
	{
        while (!IsPrepared && isHMDPause == true) 
        {
            yield return null;
        }
        Play();       
    }

	public void PreparedPlay(bool isNextState = false)
	{
		if ( MyVideoPlayer.isPlaying == false) //MyVideoPlayer.url != "" &&
            StartCoroutine(PreparedPlay_Routine(isNextState));
	}


    /// <summary>
    /// 영상을 재생 합니다.
    /// 재생 하기 전, 먼저 LoadVideo(string path)를 호출하여 Prepare상태를 만들어야 합니다.
    /// 재생 후 다음 콘텐츠를 재생할거라면 isNextState를 true로 주면 됩니다.
    /// </summary>
    private IEnumerator PreparedPlay_Routine(bool isNextState = false)
	{
		if (!IsPrepared)
			MyVideoPlayer.Prepare();

		while (!IsPrepared) yield return null;
		Play();
        if (isNextState == true)
        {
            MainManager.Instance.NextState();
        }        
    }



    /// <summary>
    /// 영상을 재생 합니다.
    /// 재생 하기 전, 먼저 LoadVideo(string path)를 호출하여 Prepare상태를 만들어야 합니다.
    /// </summary>
    public void Play()
    {
        Debug.Log("Play");
        //if (Application.isEditor)
        //{
        //    MyVideoPlayer.playbackSpeed = 2f;
        //}
        if (this.name == "Video360Player")
        {
            MyMat.color = Color.white;
        }        
		noMovieTarget.SetActive(false);
        if (!IsPrepared) return;
        //if (loadAndFirstPlay == false)
        //	m_sTime = 0;
        //if (m_eTime == 0)
        //	m_eTime = Duration;

        MyVideoPlayer.Play();
        
    }
    /// <summary>
    /// 영상 재생을 멈추고 플레이 시간을 0으로 만듭니다.
    /// </summary>
    public void Stop()
    {
        if (!IsPlaying) return;

        MyVideoPlayer.Stop();
        noMovieTarget.SetActive(true);
        //if (m_Playcheck_Routine != null)
        //    StopCoroutine(m_Playcheck_Routine);
        //m_Playcheck_Routine = null;
    }
    /// <summary>
    /// 영상을 잠시 정지 합니다.
    /// </summary>
    public void Pause()
    {
        if (!IsPlaying) return;

        Debug.Log("Pause");
        MyMat.color = Color.gray;
        MyVideoPlayer.Pause();		
    }
    /// <summary>
    /// 영상을 잠시 정지 합니다.
    /// </summary>
    public void Resume()
    {
        if (IsPlaying) return;

        MyVideoPlayer.Play();
    }
    /// <summary>
    /// 영상을 처음부터 재시작합니다.
    /// </summary>
    public void Restart()
    {
        if (!IsPrepared) return;

        Pause();
        Seek(0);
    }
    /// <summary>
    /// 영상의 노멀타임 시간을 이동합니다.
    /// </summary>
    /// <param name="nTime">이동할 시간, 0~1의 노멀 타임</param>
    public void Seek_N(float nTime)
    {
        if (!MyVideoPlayer.canSetTime) return;
        if (!IsPrepared) return;

        nTime = Mathf.Clamp01(nTime);
        MyVideoPlayer.time = nTime * Duration;
    }
	/// <summary>
	/// 영상의 리얼타임 시간을 이동합니다.
	/// </summary>
	/// <param name="dTime">이동할 시간, 0~영상의 길이(s)</param>
    public void Seek(double dTime)
    {
		if (!MyVideoPlayer.canSetTime) return;
        if (!IsPrepared) return;

		m_sTime = dTime;
		m_eTime = Duration;

		MyVideoPlayer.time = dTime;
        if (this.name == "Video360Player")
        {
            MyMat.color = Color.white;
        }
        noMovieTarget.SetActive(false);
	}
	/// <summary>
	/// 영상의 리얼타임 시간을 이동합니다.
	/// </summary>
	/// <param name="startT">이동할 시간, 0~영상의 길이(s)</param>
	public void Seek(double startT, double endT, bool isLoop, UnityAction action = null)
	{
		if (isSeeking == false)
		{
			isSeeking = isLoop;
            if (!MyVideoPlayer.canSetTime) return;
            if (!IsPrepared) return;

            Debug.Log(string.Format("seek {0}/{1}/{2}", startT, endT, isLoop));
			
            m_sTime = startT;
			m_eTime = endT;
			m_isLoop = isLoop;
			m_action = action;

			if (!(m_sTime < nTime && nTime < m_eTime) || isLoop == false)
				MyVideoPlayer.time = startT;
            if (this.name == "Video360Player")
            {
                MyMat.color = Color.white;
            }
            noMovieTarget.SetActive(false);

			StartCoroutine(LateUnSeek());
		}
	}
	private IEnumerator LateUnSeek()
	{
		yield return new WaitForSeconds(1);
		isSeeking = false;
	}
	/// <summary>
	/// 영상의 리얼타임 시간을 이동합니다.
	/// </summary>
	/// <param name="startT">이동할 시간, 0~영상의 길이(s)</param>
	public void LateSeek(double startT, double endT, bool isLoop, UnityAction action = null)
	{
		if (m_lateSeek_Routine != null)
			StopCoroutine(m_lateSeek_Routine);
		m_lateSeek_Routine = LateSeek_Routine(startT, endT, isLoop, action);
		StartCoroutine(m_lateSeek_Routine);
	}
	private IEnumerator LateSeek_Routine(double startT, double endT, bool isLoop, UnityAction action = null)
	{
		if (IsPlaying == false) PreparedPlay();

		while (!IsPrepared) yield return null;
		while (!MyVideoPlayer.canSetTime) yield return null;

		Seek(startT, endT, isLoop, action);
	}
	/// <summary>
	/// 영상의 리얼타임 시간을 이동합니다.
	/// </summary>
	/// <param name="startT">이동할 시간, 0~영상의 길이(s)</param>
	public void LateSeek(double startT)
	{
		if (m_lateSeek_Routine != null)
			StopCoroutine(m_lateSeek_Routine);
		m_lateSeek_Routine = LateSeek_Routine(startT);
		StartCoroutine(m_lateSeek_Routine);
	}
	private IEnumerator LateSeek_Routine(double startT)
	{
		if (IsPlaying == false) PreparedPlay();
		while (!IsPrepared) yield return null;
		while (!MyVideoPlayer.canSetTime) yield return null;

		Seek(startT);
	}
	/// <summary>
	/// 영상 재생속도를 올립니다.
	/// </summary>
	public void IncrementPlaybackSpeed()
    {
        if (!MyVideoPlayer.canSetPlaybackSpeed) return;

        MyVideoPlayer.playbackSpeed += 1;
        MyVideoPlayer.playbackSpeed = Mathf.Clamp(MyVideoPlayer.playbackSpeed, 0, 10);
    }
    /// <summary>
    /// 영상 재생속도를 내립니다.
    /// </summary>
    public void DecrementPlaybackSpeed()
    {
        if (!MyVideoPlayer.canSetPlaybackSpeed) return;

        MyVideoPlayer.playbackSpeed -= 1;
        MyVideoPlayer.playbackSpeed = Mathf.Clamp(MyVideoPlayer.playbackSpeed, 0, 10);
    }
}

public enum VideoEventType
{
    /// <summary>
    /// 비디오 재생시 에러가 발생할 때
    /// </summary>
    ErrorReceived,
    /// <summary>
    /// 첫 프레임 로딩이 완료 되었을 때
    /// </summary>
    FrameReady,
    /// <summary>
    /// 반복되는 지점에 도착했을 때(영상 끝부분)
    /// </summary>
    LoopPointReached,
    /// <summary>
    /// 영상 재생 준비가 완료 되었을 때
    /// </summary>
    PrepareCompleted,
    /// <summary>
    /// 영상이동 완료 되었을 때
    /// </summary>
    SeekCompleted,
    /// <summary>
    /// 영상 재생을 시작 하였을 때
    /// </summary>
    Started,
    /// <summary>
    /// 영상이 끝나갑니다. FadeOut을 시작하세요.
    /// </summary>
    PlayEnd_FadeOut
}

#region 인트로 막아놓은 로직(보류)
//재생중일때 이쪽으로 들어옴
//      if (IsPlaying)
//{
//          if (fniPlayerManager != null)
//          {
//              fniPlayerManager.ChangePlayState(PlayState.VideoPlaying);
//          }
//          //1. 명령 없을때는 아무것도 실행 안시키면 됨.

//          //2. 인트로 재생 중에는 아무것도 안하고 대기, 인트로가 끝나면 end쪽으로 넘겨줌
//          if (introPlaying)
//          {
//              if (curTime > 67)
//              {
//                  //인트로 재생이 끝나고 콘텐츠 리스트에 하나라도 들어있으면 바꿔줌
//                  //리스트가 없다는건 명령이 안들어왔다는 거라서 introEnd를 바꿔줄 필요가 없음
//                  if (ContentsList.Count != 0)
//                  {
//                      //인트로 재생이 끝나면 end를 true로 바꿔줌
//                      introEnd = true;
//                      introPlaying = false;
//                  }                    
//              }
//          }
//          //3. 인트로 재생 끝나고 명령 들어왔을 때 introEnd가 true가 되면 들어옴.
//          else if (introEnd)
//          {
//              //Seek를 이동시키고 새롭게 들어온 리스트를 재생 시킨다.
//              videoCommand(ContentsList);
//          }
//}
#endregion

