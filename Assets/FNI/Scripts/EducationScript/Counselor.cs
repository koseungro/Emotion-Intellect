/// 작성자: 김윤빈
/// 작성일: 2020-07-28
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FNI
{
    public class Counselor : BaseScript
    {
        #region Singleton
        private static Counselor _instance;
        public static Counselor Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<Counselor>();
                    if (_instance == null)
                        Debug.LogError("Counselor를 찾을 수 없습니다. ");
                }
                return _instance;
            }
        }
        #endregion

        private void Start()
        {
            SetContentName("상담가 되기");
        }

        public TextMeshProUGUI card1;
        public TextMeshProUGUI card2;
        public TextMeshProUGUI card3;

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

        public ContentsData contentsData;

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