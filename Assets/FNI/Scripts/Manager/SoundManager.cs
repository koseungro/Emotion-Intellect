/// 작성자: 김윤빈
/// 작성일: 2020-09-16
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

namespace FNI
{
    public class SoundManager : MonoBehaviour
    {
        public GameObject narrationVolumeObj;

        public GameObject videoVolumeObj;

        public AudioSource videoSource;

        public GameObject[] contentsSource;

        private void Update()
        {
            
        }

        public void videoSound()
        {
            float volume = (float)Math.Truncate(videoSource.volume * 10) / 10; 
            videoVolumeObj.GetComponent<TextMeshProUGUI>().text = volume.ToString();
        }

        public void ContentSound()
        {

        }

    }
}