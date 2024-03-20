/// 작성자: 김윤빈
/// 작성일: 2020-07-28
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace FNI
{
    public class EmotionalDriven : BaseScript
    {

        #region Singleton
        private static EmotionalDriven _instance;
        public static EmotionalDriven Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<EmotionalDriven>();
                    if (_instance == null)
                        Debug.LogError("EmotionalDriven를 찾을 수 없습니다. ");
                }
                return _instance;
            }
        }
        #endregion

        public void Start()
        {
            SetContentName("정서주도 행동 바꾸기");
        }

        public TextMeshProUGUI card1;
        public TextMeshProUGUI card2;
        public TextMeshProUGUI card3;

        public SkinnedMeshRenderer subHeartSkinMesh;

        public void YellowHeartGloomy()
        {
            subHeartSkinMesh.SetBlendShapeWeight(6, 0);
            subHeartSkinMesh.SetBlendShapeWeight(4, 100);
        }

        public void SetCard()
        {
            card1.text = GetUserInfo.Ecard1;
            card2.text = GetUserInfo.Ecard2;
            card3.text = GetUserInfo.Ecard3;
        }

        public override void SetContentName(string contentName)
        {
            base.SetContentName(contentName);
        }
        public void SetStage()
        {
            BackGroundChanger.Instance.StageSettingRender();
        }

        public Material[] carMaterials;
                
        public void CarFadeOut()
        {            
            for (int cnt = 0; cnt < carMaterials.Length; cnt++)
            {
                carMaterials[cnt].DOFade(0f, 1.3f);
            }
        }

        public void CarFadeIn()
        {
            for (int cnt = 0; cnt < carMaterials.Length; cnt++)
            {
                carMaterials[cnt].DOFade(1f, 1.3f);
            }
        }

        public ContentsData contentsData;

        public override void EndAnimation()
        {
            MainManager.Instance.StartContentsData(contentsData);
            BackGroundChanger.Instance.DefaultSettingRender();
        }


    }
}