/// 작성자: 김윤빈
/// 작성일: 2020-06-29
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace FNI
{
    public delegate void NextStateHandler();
    public delegate void VideoPlayHandler();
    public delegate void ObjectControlHandler(bool active);

    public class BaseScript : MonoBehaviour
    {

        // 각 캔버스 오브젝트를 모두 찾아서 반환
        public GameObject parent
        {
            get
            {
                if (transform.GetChild(0).gameObject != null)
                {
                    parentObject = transform.GetChild(0).gameObject;
                }
                else
                {
                    parentObject = null;
                }
                return parentObject;
            }
        }

        private GameObject parentObject;

        public virtual void Start()
        {
            //SetParent();
            if (transform.GetChild(0).gameObject != null)
            {
                parentObject = transform.GetChild(0).gameObject;
            }
        }

        public virtual void SetParent()
        {
            parentObject.SetActive(false);
            //gameObjects.Remove(parent);
           // Debug.Log(gameObjects.Count + " : count");
        }

        public virtual void Show()
        {
            parentObject.SetActive(true);
            //gameObjects.Add(parent);
            //Debug.Log(gameObjects[0].name + " : name");
        }

        public virtual void AllOff(bool active)
        {
            parent.SetActive(active);
        }

        public virtual void QuitApplication()
        {
            StartCoroutine(QuitRoutine());
        }

        IEnumerator QuitRoutine() 
        {
            Debug.Log("종료 됩니다.");
            // 종료하기 전 데이터 왔다갔다 잘 되는지 체크후에 완료되면 종료시키기
            yield return new WaitForSeconds(0.5f);
            Application.Quit();
        }

        public virtual void SetContentName(string contentName)
        {
            GetUserInfo.category1 = contentName;
            MainManager.Instance.isStart = true;
        }

        public virtual void EndAnimation()
        {
        }

    }
}