/// 작성자: 김윤빈
/// 작성일: 2020-07-28
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
    public class SelfMercy : BaseScript
    {
        #region Singleton
        private static SelfMercy _instance;
        public static SelfMercy Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SelfMercy>();
                    if (_instance == null)
                        Debug.LogError("SelfMercy를 찾을 수 없습니다. ");
                }
                return _instance;
            }
        }

        #endregion


        TimelineAsset timelineAsset;
        public PlayableDirector playableDirector;

        public SkinnedMeshRenderer subHeartSkinMesh;

        public ContentsData contentsData;

        public void Start()
        {
            SetContentName("자기자비");
        }

        public override void SetContentName(string contentName)
        {
            base.SetContentName(contentName);
        }

        public void test()
        {
            playableDirector.Pause();
        }

        float time;
        float F_time = 1f;

        public void SetStage()
        {
            BackGroundChanger.Instance.StageSettingRender();
        }

        public void SetSea()
        {
            BackGroundChanger.Instance.SeaSettingRender();
        }


        public void SmileSubFace()
        {
            //skinnedMesh.SetBlendShapeWeight(6, 100);
            StartCoroutine(SmileRoutine());
        }

        IEnumerator SmileRoutine()
        {
            // 0으로 한번 초기화
            time = 0;

            //페이드 아웃 먼저, 알파값이 1보다 작으면 계속 반복
            while (subHeartSkinMesh.GetBlendShapeWeight(6) < 100)
            {
                // 매 프레임 deltatime을 F_time으로 나눈 값을 time에 더해줌
                time += Time.deltaTime / F_time;
                // 부드럽게
                float cnt = Mathf.Lerp(0, 100, time);
                subHeartSkinMesh.SetBlendShapeWeight(6, cnt);

                yield return null;
            }

            yield return null;
        }

        public void DefaultFace()
        {
            //skinnedMesh.SetBlendShapeWeight(6, 100);
            StartCoroutine(DefaultFaceRoutine());
        }


        IEnumerator DefaultFaceRoutine()
        {
            // 0으로 한번 초기화
            time = 0;

            //페이드 아웃 먼저, 알파값이 1보다 작으면 계속 반복
            while (subHeartSkinMesh.GetBlendShapeWeight(6) > 0)
            {
                // 매 프레임 deltatime을 F_time으로 나눈 값을 time에 더해줌
                time += Time.deltaTime / F_time;
                // 부드럽게
                float cnt = Mathf.Lerp(100, 0, time);
                subHeartSkinMesh.SetBlendShapeWeight(6, cnt);

                yield return null;
            }

            yield return null;
        }

        public override void EndAnimation()
        {
            MainManager.Instance.StartContentsData(contentsData);
            BackGroundChanger.Instance.DefaultSettingRender();
        }

    }
}