/// 작성자: 김윤빈
/// 작성일: 2020-09-14
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace FNI
{
    public class NewBehaviourScript : MonoBehaviour
    {

        public FogVolumeRenderer center;
        public FogVolumeRenderer left;
        public FogVolumeRenderer right;
        public PostProcessLayer leftttt;

        public Text text1;
        public Text text2;
        public Text text3;

        public void Update()
        {
            Debug.Log(left.isActiveAndEnabled + ":reft");
            Debug.Log(right.isActiveAndEnabled + ":right");
            text1.text = left.isActiveAndEnabled + ":reft";
            text2.text = right.isActiveAndEnabled + ":right";
            text3.text = leftttt.isActiveAndEnabled + " : 왼쪽포스트";
        }

    }
}