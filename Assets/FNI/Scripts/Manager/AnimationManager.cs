/// 작성자: 김윤빈
/// 작성일: 2020-07-07
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace FNI
{
    public class AnimationManager : BaseScript
    {
        #region Singleton
        private static AnimationManager _instance;
        public static AnimationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<AnimationManager>();
                    if (_instance == null)
                        Debug.LogError("AnimationManager를 찾을 수 없습니다. ");
                }
                return _instance;
            }
        }
        #endregion
                
        PlayableDirector playableDirector;        

        public List<GameObject> playableDirectorList = new List<GameObject>();

        List<GameObject> animationObjectList = new List<GameObject>();

        private void Awake()
        {
            //playableDirector = GetComponent<PlayableDirector>();
            FindAnimationObject();
            //playableDirector.playableAsset = timelineAssetList[0];
            //playableDirector.Play();
        }

        // 게임오브젝트를 전부 찾아서 리스트에 추가
        private void FindAnimationObject()
        {
            for (int cnt = 0; cnt < transform.Find("Parent").childCount; cnt++)
            {
                animationObjectList.Add(transform.Find("Parent").GetChild(cnt).gameObject);
            }
        }

        private void OnEnable()
        {
            UIManager.Instance.OnObjectControl += AllOff;
            //playableDirector.stopped += OnPlayableDirectorStopped;
        }        

        public override void AllOff(bool active)
        {
            base.AllOff(active);
        }

        // 애니메이션이 켜지면서 재생할 오브젝트를 같이 켜주면 됨
        public override void Show()
        {
            base.Show();
        }

        /// <summary>
        /// 애니메이션을 찾아서 해당 타임라인이 들어있는 오브젝트를 찾아서 켜줍니다.
        /// </summary>
        /// <param name="animationName"></param>
        public void LoadAnimation(string animationName)
        {
            for (int cnt = 0; cnt < playableDirectorList.Count; cnt++)
            {
                if (playableDirectorList[cnt].name.Contains(animationName))
                {
                    playableDirectorList[cnt].SetActive(true);
                    //playableDirectors[cnt].GetComponent<PlayableDirector>().time = 0f;
                    playableDirectorList[cnt].GetComponent<PlayableDirector>().Play();
                }
            }
            StartCoroutine(nextRoutine());
            //playableDirector.time = 0f;
            //playableDirector.Play();

            //for (int cnt = 0; cnt < timelineAssetList.Count; cnt++)
            //{
            //    if (timelineAssetList[cnt].name == animationName)
            //    {
            //        playableDirector.playableAsset = timelineAssetList[cnt];
            //    }
            //}
            //playableDirector.time = 0f;
            //playableDirector.Play();
        }

        IEnumerator nextRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            MainManager.Instance.NextState();
        }

        // 타임라인 끝나면 호출됨
        void OnPlayableDirectorStopped(PlayableDirector aDirector)
        {
            //if (playableDirector == aDirector)
            Debug.Log("PlayableDirector named " + aDirector.name + " is now stopped.");
            //MainManager.Instance.NextState();

        }


        private void Update()
        {
            if (OVRInput.GetUp(OVRInput.Button.Two))
            {
                //isTest = true;
            }
        }

        public void TimeLinePause()
        {
            playableDirector.Pause();
            StartCoroutine(PlayRoutine());
        }

        public void TimeLineEnd()
        {
            playableDirector.Stop();
        }

        bool isTest = false;

        IEnumerator PlayRoutine()
        {
            // 일정 조건 이후 다시 플레이 하도록
            //while (!isTest)  yield return null;
            yield return new WaitForSeconds(1.5f);
            playableDirector.Play();
        }

        public void testVideo()
        {
            //IS_VideoPlayer.Instance.MovieLoad("01.mp4");
            //IS_VideoPlayer.Instance.MyVideoPlayer.isLooping = true;
            //IS_VideoPlayer.Instance.PreparedPlay();
            FNI_Record.Instance.Show(RecordType.TimeLimit, 10, true, false);

            //FNI_Record.Instance.RecordReady(RecordType.TimeLimit);
        }

    }
}