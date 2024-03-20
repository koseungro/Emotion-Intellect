/// 작성자: 김윤빈
/// 작성일: 2020-07-03
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;
using UnityEngine.Networking;
using System;

namespace FNI
{
    public class TestScript : MonoBehaviour
    {
        private delegate void test();
        private UnityAction actionTest;
        test te;

        private void Awake()
        {
            te += abc;
            actionTest += abc;

            te();
            actionTest();

            StartCoroutine(GetGoogleTime(abc));
        }

        void abc()
        {
            Debug.Log("a");
            
        }



        IEnumerator GetGoogleTime(UnityAction val)
        {
            const string url = "https://www.google.co.kr";
            var webrequst = UnityWebRequest.Head(url);
            yield return webrequst.SendWebRequest();
            DateTime serverTime = Convert.ToDateTime(webrequst.GetRequestHeader(name: "Date"));
            //val(serverTime.ToString());
            Debug.Log(serverTime.ToString());
            val();
        }



    }
}