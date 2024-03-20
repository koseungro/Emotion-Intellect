/// 작성자: 김윤빈
/// 작성일: 2020-07-20
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FNI
{
    public class ObjMoveTest : BaseScript
    {

        List<GameObject> TextObjList = new List<GameObject>();

        List<Transform> TextPositionList = new List<Transform>();

        Transform CenterPos;

        public GameObject TextObjParent;
        public GameObject TextPosParent;
        public GameObject Controller;

        float time;
        public float moveTime = 2f;


        private void Start()
        {
            AddObjList();
            AddPosList();
        }


        float length = 10000f;
        RaycastHit hit;

        public GameObject[] seaText;

        public GameObject[] seaTextDummy;
        private Vector3 currentSize = new Vector3(1.5f, 1.5f, 1.5f);
        public GameObject currentObj;
        bool isPointerEnter = false;

        public void TextColliderOnOff(bool value)
        {
            for (int cnt = 0; cnt < seaText.Length; cnt++)
            {
                seaText[cnt].GetComponent<BoxCollider>().enabled = value;
            }
        }

        public void BacKSize()
        {
            for (int cnt = 0; cnt < seaText.Length; cnt++)
            {
                if(seaText[cnt].name == currentObj.name)
                {
                    seaText[cnt].transform.localScale = currentSize;
                }
            }
        }

        private void Update()
        {

            Ray myRay = new Ray(Controller.transform.position, Controller.transform.forward);
            Debug.DrawRay(Controller.transform.position, Controller.transform.forward * length, Color.red, 0.1f);
            if (Physics.Raycast(myRay, out hit, length))
            {

                //var selection = hit.transform;
                //var selectionRenderer = selection.GetComponent<MeshRenderer>();
                //if (selectionRenderer != null)
                //{

                //}

                if (hit.transform.tag == "TextObject")
                {
                    if (hit.transform.parent.name != "CenterPos")
                    {
                        Debug.Log(hit.transform.name);
                        //hit.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", 1.02f);
                        hit.transform.gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
                        currentObj = hit.transform.gameObject;
                        isPointerEnter = true;
                    }
                }
                else if(hit.transform.tag != "TextObject")
                {
                    if (isPointerEnter == true)
                    {
                        BacKSize();
                    }
                    currentObj = null;
                    isPointerEnter = false;
                }

                if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
                {
                    Debug.Log("PrimaryIndexTrigger");
                    if (isDone == true)
                    {
                        if (hit.transform.parent.name == "CenterPos")
                        {

                        }
                        else
                        {
                            Debug.Log("MoveCenterRoutine");
                            if (hit.transform.tag == "TextObject")
                            {
                                StartCoroutine(MoveCenterRoutine(hit.transform.gameObject));
                            }
                        }
                    }                    
                }

            }
            else
            {
                if (isPointerEnter == true)
                {
                    BacKSize();
                }
                currentObj = null;
                isPointerEnter = false;
            }


            if (OVRInput.GetUp(OVRInput.Button.One))
            {
                Debug.Log("click");
                //StartCoroutine(MoveObjRoutine());
            }
        }

        /// <summary>
        /// 텍스트 오브젝트를 리스트에 추가해줍니다.
        /// </summary>
        void AddObjList()
        {
            for (int i = 0; i < TextObjParent.transform.childCount; i ++)
            {
                TextObjList.Add(TextObjParent.transform.GetChild(i).gameObject);
            }
            Debug.Log("TextObjList : "+ TextObjList.Count + " 개");
        }

        /// <summary>
        /// 오브젝트가 가야할 Transform값을 리스트에 추가해줍니다.
        /// </summary>
        void AddPosList()
        {
            for (int i = 0; i < TextPosParent.transform.childCount; i++)
            {
                if (TextPosParent.transform.GetChild(i).name != "CenterPos")
                {
                    TextPositionList.Add(TextPosParent.transform.GetChild(i));
                }
                else if (TextPosParent.transform.GetChild(i).name == "CenterPos")
                {
                    CenterPos = TextPosParent.transform.GetChild(i);
                    Debug.Log(TextPosParent.transform.GetChild(i).name + " : Center");
                }
            }
            Debug.Log("TextObjList : " + TextPositionList.Count + " 개");
        }

        void Ray_OnEnter(GameObject go)
        {
            go.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", 1.02f);
        }

        void Ray_OnExit(GameObject go)
        {
            go.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", 1f);
        }

        public void resetPosition()
        {
            for (int cnt = 0; cnt < TextObjList.Count; cnt++)
            {
                TextObjList[cnt].transform.position = new Vector3(0f, -25f, 12.35f);
            }
        }

        public void startMove()
        {
            SelfMercy.Instance.playableDirector.Pause();
            TextColliderOnOff(false);
            StartCoroutine(MoveObjRoutine());
        }

        public bool isMove = false;
        bool isDone = false;
        Transform originalTf;
        public Light pointLight;
        float F_time = 1.5f;
        float M_time = 0.5f;

        // 오브젝트를 켜준 후 전부 지정된 자리로 보내버림
        IEnumerator MoveObjRoutine()
        {
            isMove = true;


            //while (pointLight.intensity < 5)
            //{
            //    time += Time.deltaTime / F_time;
            //    pointLight.intensity = Mathf.Lerp(0, 5, time);
            //    yield return null;
            //}

            for (int cnt = 0; cnt < TextObjList.Count; cnt ++)
            {
                time = 0f;
                TextObjList[cnt].SetActive(true);

                // 먼저 센터 포지션으로 오브젝트를 올려준다
                while (true)
                {
                    time += Time.deltaTime * 0.2f;
                    //time += Time.deltaTime / M_time;

                    TextObjList[cnt].transform.position = Vector3.Lerp(TextObjList[cnt].transform.position, CenterPos.position, time);

                    if (TextObjList[cnt].transform.position == CenterPos.position) //new Vector3(0f, 0f, 7f)
                    {
                        break;
                    }
                    yield return null;
                }


                // 오디오 재생을 한 후 오디오 재생이 끝날 때 까지 센터 포지션에서 대기
                //GetComponent<AudioSource>().Play();
                //while (true)
                //{                    
                //    if (GetComponent<AudioSource>().isPlaying == false)
                //    {
                //        break;
                //    }
                //    yield return null;
                //}

                // 오디오 재생이 끝난 후 각자 자리로 보냄
                time = 0f;
                while (true)
                {
                    time += Time.deltaTime * 0.1f;
                    TextObjList[cnt].transform.position = Vector3.Lerp(TextObjList[cnt].transform.position, TextPositionList[cnt].position, time);

                    if (TextObjList[cnt].transform.position == TextPositionList[cnt].position)
                    {
                        break;
                    }
                    yield return null;
                }
            }

            //while (pointLight.intensity > 0)
            //{
            //    time += Time.deltaTime / F_time;
            //    pointLight.intensity = Mathf.Lerp(5, 0, time);
            //    yield return null;
            //}

            isMove = false;
            isDone = true;
            SelfMercy.Instance.playableDirector.Play();
        }

        public void PauseTimeLine()
        {
            SelfMercy.Instance.playableDirector.Pause();
        }

        public void StopAnimation()
        {
            // 단어 선택 전
            isFloating = true;
            SelfMercy.Instance.playableDirector.Pause();
            TextColliderOnOff(true);
        }

        // 현재 단어 선택 전 이면 true / 단어선택 후에는 false
        public bool isFloating = false;

       // 클릭 했을 때 센터Pos로
       IEnumerator MoveCenterRoutine(GameObject GO)
        {
            //go = Instantiate(TextObj, Pos1);
            //go.transform.parent = Base;
            TextColliderOnOff(false);
            if (CenterPos.childCount == 0 && isMove == false)
            {
                isMove = true;
                time = 0f;
                while (true)
                {
                    time += Time.deltaTime * 0.3f;
                    GO.transform.position = Vector3.Lerp(GO.transform.position, CenterPos.position, time);
                    if (GO.transform.position == CenterPos.position)
                    {
                        GO.transform.parent = CenterPos;
                        break;
                    }
                    yield return null;
                    
                }
                isMove = false;

            }
            else if (CenterPos.childCount != 0 && isMove == false)
            {
                //센터포지션에 다른 오브젝트가 있다면 얘를 먼저 원래 자리에 보낸 후 클릭한 오브젝트를 센터 포지션으로 불러와야함
                GameObject go = CenterPos.GetChild(0).gameObject;
                go.transform.parent = TextObjParent.transform;
                time = 0f;

                Transform tf = null;

                for (int cnt = 0; cnt < TextObjList.Count; cnt ++)
                {
                    if (go.transform.gameObject.name == TextObjList[cnt].name)
                    {
                        tf = TextPositionList[cnt];
                    }
                }
                isMove = true;
                while (true)
                {
                    time += Time.deltaTime * 0.3f;
                    go.transform.position = Vector3.Lerp(go.transform.position, tf.position, time);
                    if (go.transform.position == tf.position)
                    {
                        break;
                    }
                    yield return null;
                }

                time = 0f;
                while (true)
                {
                    time += Time.deltaTime * 0.3f;
                    GO.transform.position = Vector3.Lerp(GO.transform.position, CenterPos.position, time);
                    if (GO.transform.position == CenterPos.position)
                    {
                        GO.transform.parent = CenterPos;
                        break;
                    }
                    yield return null;

                }
                isMove = false;
            }

            yield return new WaitForSeconds(1f);
            // 클릭이 끝나면 다시 타임라인 Play
            isFloating = false;
            SelfMercy.Instance.playableDirector.Play();
        }

    }
}