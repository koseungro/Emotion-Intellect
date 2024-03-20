/// 작성자: 김윤빈
/// 작성일: 2020-07-28
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FNI
{
    public class SelfCalibration : BaseScript
    {
        #region Singleton
        private static SelfCalibration _instance;
        public static SelfCalibration Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SelfCalibration>();
                    if (_instance == null)
                        Debug.LogError("SelfCalibration를 찾을 수 없습니다. ");
                }
                return _instance;
            }
        }
        #endregion

        public ContentsData contentsData;
        public GameObject backGroundUI;
        public Texture natureBG;
        public Texture blackNatureBG;
        public Texture officeBG;

        private void Start()
        {
            SetContentName("자기 진정");
        }

        public override void SetContentName(string contentName)
        {
            base.SetContentName(contentName);
        }


        public void SetNatureBG()
        {
            // 자연 영상 틀어줘야함
            BackGroundChanger.Instance.DefaultSettingRender();
            //backGroundUI.GetComponent<MeshRenderer>().material.mainTexture = natureBG;
            if (IS_VideoPlayer.Instance.gameObject.activeSelf == false)
            {
                IS_VideoPlayer.Instance.gameObject.SetActive(true);
            }
            IS_VideoPlayer.Instance.MovieLoad("01.mp4");
            IS_VideoPlayer.Instance.PreparedPlay(true);
            IS_VideoPlayer.Instance.MyVideoPlayer.isLooping = true;
        }

        public void SetBlackNatureBG()
        {
            //backGroundUI.GetComponent<MeshRenderer>().material.mainTexture = blackNatureBG;

            // 자연 영상 틀어줘야함
            BackGroundChanger.Instance.DefaultSettingRender();
            //backGroundUI.GetComponent<MeshRenderer>().material.mainTexture = natureBG;
            if (IS_VideoPlayer.Instance.gameObject.activeSelf == false)
            {
                IS_VideoPlayer.Instance.gameObject.SetActive(true);
            }
            IS_VideoPlayer.Instance.MovieLoad("01_Black.mp4");
            IS_VideoPlayer.Instance.PreparedPlay(true);
            IS_VideoPlayer.Instance.MyVideoPlayer.isLooping = true;
        }

        public void SetOfficeBG()
        {
            // 자연 영상 종료
            IS_VideoPlayer.Instance.MyVideoPlayer.isLooping = false;
            IS_VideoPlayer.Instance.Stop();
            backGroundUI.GetComponent<MeshRenderer>().material.mainTexture = officeBG;
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

    }
}