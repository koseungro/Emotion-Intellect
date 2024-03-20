/// 작성자: 김윤빈
/// 작성일: 2020-06-29
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace FNI
{
    public class TestAnimation : BaseScript
    {
        PlayableDirector playableDirector;

        public void Start()
        {
            playableDirector = this.gameObject.GetComponent<PlayableDirector>();
            
            //playableDirector.Play();
        }
         
    }
}