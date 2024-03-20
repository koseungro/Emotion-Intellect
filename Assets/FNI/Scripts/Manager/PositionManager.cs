/// 작성자: 김윤빈
/// 작성일: 2020-07-07
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
    public class PositionManager : BaseScript
    {
        #region Singleton
        private static PositionManager _instance;
        public static PositionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PositionManager>();
                    if (_instance == null)
                        Debug.LogError("PositionManager를 찾을 수 없습니다. ");
                }
                return _instance;
            }
        }
        #endregion

        AudioSource audioSource;

        public GameObject numImage;
        public GameObject textImage;
        public TextMeshProUGUI mainText;

        private void OnEnable()
        {
            UIManager.Instance.OnObjectControl += AllOff;
        }

        private void Awake()
        {
            audioSource = this.GetComponent<AudioSource>();
        }

        public override void AllOff(bool active)
        {
            base.AllOff(active);
        }

        public void SetPosition()
        {
            time = 3;
            IEnumerator PositionRoutine = SetPositionRoutine();
            StartCoroutine(PositionRoutine);
            IEnumerator TextCountRoutine = CountRoutine();
            StartCoroutine(TextCountRoutine);
        }

        int time = 3;
        public TextMeshProUGUI timeText;

        IEnumerator CountRoutine()
        {   
            Debug.Log("시작");
            while (time > -1)
            {
                timeText.text = time.ToString() + "초";
                yield return new WaitForSeconds(1f);
                time--;
            }
        }

        IEnumerator SetPositionRoutine()
        {
            //yield return new WaitForSeconds(0.5f);
            //audioSource.Play();
            yield return new WaitForSeconds(4f);
            //audioSource.Stop();
            OVRManager.display.RecenterPose();
            MainManager.Instance.NextState();
        }

    }
}