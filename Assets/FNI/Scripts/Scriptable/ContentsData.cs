/// 작성자: 김윤빈
/// 작성일: 2020-06-29
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using Panic2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FNI
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New ContentsData", menuName = "FNI/ContentsData", order = 1)]
    public class ContentsData : ScriptableObject
    {
        // 콘텐츠 이름 별로 시나리오를 짜주면 됨.
        public string ContentsName = "";

        // 스트레스란? 에서 1회차 2회차를 구분
        public bool isFirst = false;
        public bool isSecond = false;

        // 1회차, 2회차 구분 설정 후 잠금
        public bool isRock = false;

        // 선택한 콘텐츠에서 실행할 작업 순서 및 정보
        public List<Contents> ContentsList = new List<Contents>();
        //public List<ButtonData> ButtonList = new List<ButtonData>();

    }

    [System.Serializable]
    public class Contents
    {
        public PlayState contentType;
        public EmotionVideoOption emotionVideoOption;
        public string movieName;
        public string animationName;
        public string guideComment;
        public float delayTime;
        public float waitVideoTime;
        public float xPos;
        public float yPos;
        public float zPos;
        public List<IS_ButtonData> buttonData;
        public List<ContentsData> nextContentsList;

        public Contents(PlayState contentType, EmotionVideoOption emotionVideoOption , string movieName, string animationName, string guideComment, List<IS_ButtonData> buttonData, List<ContentsData> nextContentsList, float delayTime, float waitVideoTime) 
        {
            this.contentType = contentType;
            this.emotionVideoOption = emotionVideoOption;
            this.movieName = movieName;
            this.animationName = animationName;
            this.guideComment = guideComment;
            this.buttonData = buttonData;
            this.nextContentsList = nextContentsList;
            this.delayTime = delayTime;
            this.waitVideoTime = waitVideoTime;
        }
    }

}