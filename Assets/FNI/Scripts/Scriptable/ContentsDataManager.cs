/// 작성자: 김윤빈
/// 작성일: 2020-07-01
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FNI
{
    public class ContentsDataManager : MonoBehaviour
    {
        #region Singleton
        private static ContentsDataManager _instance;
        public static ContentsDataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ContentsDataManager>();
                    if (_instance == null)
                        Debug.LogError("ContentsDataManager를 찾을 수 없습니다. ");
                }
                return _instance;
            }
        }
        #endregion

        // 첫번째, 영상만 보는 리스트
        public List<ContentsData> emotionVideoList = new List<ContentsData>();

        // 두번째, 애니, 자기연습, 영상 리스트
        public List<ContentsData> contentsDataList = new List<ContentsData>();

        public void SelectContent(int unityNum, int nth)
        {

        }

    }
}