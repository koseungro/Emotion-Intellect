/// 작성자: 김윤빈
/// 작성일: 2020-07-31
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
    public class SaveManager : MonoBehaviour
    {
        public void Start()
        {
            PlayerPrefs.SetString("SaveStartTime1", System.DateTime.Now.ToString());
            PlayerPrefs.SetString("SaveStartTime2", System.DateTime.Now.ToString());
            PlayerPrefs.Save();
            Debug.Log("SaveTime");
        }

        public void Load()
        {
            string str = PlayerPrefs.GetString("SaveStartTime1");
            Debug.Log(str + " : 시간");
        }

        void CheckData()
        {
            // 시간은 총 2번 저장됨
            // 만약 시간이 두개가 저장되있다면 둘다 삭제해주고 새롭게 저장하면 됨.
            // 아무것도 저장된게 없다면 순차적으로 저장해주면 됨.
        }

        private void Update()
        {
            if (OVRInput.GetUp(OVRInput.Button.Two))
            {
                
                if (PlayerPrefs.HasKey("SaveStartTime1") && PlayerPrefs.HasKey("SaveStartTime2"))
                {
                    //PlayerPrefs.DeleteAll();
                }
                if (PlayerPrefs.HasKey("SaveStartTime1") == false)
                {

                }
            }
        }


    }
}