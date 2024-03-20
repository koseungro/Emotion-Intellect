/// 작성자: 김윤빈
/// 작성일: 2020-07-16
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using CurvedUI;
using FNI;
using FNIUnityEngine.hjchae;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace FNI
{
    public class LineDrawer : BaseScript, IPointerDownHandler ,IDragHandler, IEndDragHandler
    {

        public UILineRenderer[] lines;

        public Button button1;
        public Button button1_1;
        public Button button2;
        public Button button2_2;
        public Button button3;
        public Button button3_3;

        private void OnEnable()
        {
            UIManager.Instance.OnObjectControl += AllOff;
            ResetLine();
        }

        public override void AllOff(bool active)
        {
            base.AllOff(active);
        }

        public override void Show()
        {
            base.Show();
        }


        private void Start()
        {
            //button1.onClick.AddListener(delegate
            //{
            //    LineButton(button1.GetComponent<RectTransform>().localPosition);
            //});
            //button1_1.onClick.AddListener(delegate
            //{
            //    LineButton(button1_1.GetComponent<RectTransform>().localPosition);
            //});
            //button2.onClick.AddListener(delegate
            //{
            //    LineButton(button2.GetComponent<RectTransform>().localPosition);
            //});
            //button2_2.onClick.AddListener(delegate
            //{
            //    LineButton(button2_2.GetComponent<RectTransform>().localPosition);
            //});
            //button3.onClick.AddListener(delegate
            //{
            //    LineButton(button3.GetComponent<RectTransform>().localPosition);
            //});
            //button3_3.onClick.AddListener(delegate
            //{
            //    LineButton(button3_3.GetComponent<RectTransform>().localPosition);
            //});

            //Instantiate(lineRenderer1, mycanvas.transform);

            SetAddListener(button1);
            SetAddListener(button2);
            SetAddListener(button3);
            SetAddListener(button1_1);
            SetAddListener(button2_2);
            SetAddListener(button3_3);

        }


        public void SetAddListener(Button button)
        {
            EventTrigger trigger = button.GetComponent<EventTrigger>();

            EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();
            entryPointerDown.eventID = EventTriggerType.PointerDown;
            entryPointerDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
            trigger.triggers.Add(entryPointerDown);

            EventTrigger.Entry entryOnDrag = new EventTrigger.Entry();
            entryOnDrag.eventID = EventTriggerType.Drag;
            entryOnDrag.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
            trigger.triggers.Add(entryOnDrag);

            EventTrigger.Entry entryEndDrag = new EventTrigger.Entry();
            entryEndDrag.eventID = EventTriggerType.EndDrag;
            entryEndDrag.callback.AddListener((data) => { OnEndDrag((PointerEventData)data); });
            trigger.triggers.Add(entryEndDrag);
        }

        float length = 10000f;
        RaycastHit hit;
        public GameObject Controller;

        Vector2 pointOnCanvas;
        private void Update()
        {
            Ray myRay = new Ray(Controller.transform.position, Controller.transform.forward);
            Debug.DrawRay(Controller.transform.position, Controller.transform.forward * length, Color.blue, 0.1f);
            if (Physics.Raycast(myRay, out hit, length))
            {
                if (hit.transform.tag == "LineCanvas")
                {
                    Vector3 localHitPoint = hit.transform.gameObject.GetComponent<Canvas>().transform.worldToLocalMatrix.MultiplyPoint3x4(hit.point);
                    pointOnCanvas = new Vector3(0, 0, 0);
                    pointOnCanvas.x = localHitPoint.x;
                    pointOnCanvas.y = localHitPoint.y;           

                    #region 임시보관


                    // 맞으면 아웃라인
                    //hit.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", 1.02f);

                    //if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
                    //{
                    //    //Button1(pointOnCanvas);

                    //    //ButtonUp(pointOnCanvas);
                    //    //Debug.Log("GetUp");
                    //}

                    //if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
                    //{
                    //    if (isClick == true)
                    //    {
                    //        //Debug.Log("Get");
                    //        //ButtonDrag(pointOnCanvas);
                    //    }
                    //}

                    //if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
                    //{
                    //    //Button1(pointOnCanvas);

                    //    //Debug.Log("GetDown");
                    //    //ButtonPush(pointOnCanvas);
                    //}

                    //RectTransform CanvasRect = hit.transform.GetComponent<RectTransform>();

                    //then you calculate the position of the UI element
                    //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

                    //Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(WorldObject.transform.position);
                    //Vector2 WorldObject_ScreenPosition = new Vector2(
                    //((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
                    //((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));


                    #endregion
                }
            }


            if (OVRInput.GetUp(OVRInput.Button.Two))
            {
                //Debug.Log(LineConnection() + " : 체크");
                //ResetLine();
                Show();
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                //ResetLine();
            }
        }

        int cnt = 0;
        bool isClick = false;

        bool isObjClick = false;
        bool isUp = false;
        List<Vector2> pointList = new List<Vector2>();
        List<string> ClickObjNameList = new List<string>();

        List<string> CheckAnswer1 = new List<string>();
        List<string> CheckAnswer2 = new List<string>();
        List<string> CheckAnswer3 = new List<string>();

        bool[] LineCheck = new bool[] { false, false };

        string storageDownText;
        string storageUpText;


        //public void ButtonPoiterDown()
        //{

        //}

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log(eventData.pointerCurrentRaycast.gameObject.name + " : pointerCurrentRaycast.gameObject.name");
            if (cnt >= 3)
            {
                return;
            }

            // 클릭시 오브젝트가 있는가?
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                // 오브젝트 태그가 Text가 맞으면? 
                if (eventData.pointerCurrentRaycast.gameObject.tag.Contains("Text"))
                {
                    // 수정 예정
                    if (ClickCheck(eventData.pointerCurrentRaycast.gameObject.name) == true)
                    {
                        return;
                    }

                    // 오브젝트 태그 ok 아직 선택 안된 오브젝트 ok면 0번 인덱스 true
                    LineCheck[0] = true;

                    // 임시보관 후에 DragEnd에서 한번에 두개 다 텍스트 입력해줘야할듯
                    storageDownText = eventData.pointerCurrentRaycast.gameObject.name;

                    // 위, 아래 텍스트를 구분하여 bool값 설정
                    if (eventData.pointerCurrentRaycast.gameObject.tag.Contains("Up"))
                    {
                        isUp = true;
                    }
                    else if (eventData.pointerCurrentRaycast.gameObject.tag.Contains("Down"))
                    {
                        isUp = false;
                    }

                    pointList.Clear();
                    lines[cnt].gameObject.SetActive(true);
                    //pointList = new List<Vector2>(lines[cnt].Points);
                    pointList.Add(eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<RectTransform>().localPosition);
                    pointList.Add(eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<RectTransform>().localPosition);
                    lines[cnt].Points = pointList.ToArray();

                    isObjClick = true;
                }
                else
                {
                    LineCheck[0] = false;
                    isObjClick = false;
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (cnt >= 3)
            {
                return;
            }
            if (isObjClick == true)
            {
                //Debug.Log("OnDrag");
                lines[cnt].Points[1].x = pointOnCanvas.x;
                lines[cnt].Points[1].y = pointOnCanvas.y;
            }

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("enddrag");
            if (cnt >= 3)
            {
                return;
            }
            // 드래그가 끝나는 시점을 판단함

            // 드래그가 끝났을 때 원하는 오브젝트가 있으면 
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.tag.Contains("Text"))
                {
                    if (isUp == true)
                    {
                        if (eventData.pointerCurrentRaycast.gameObject.tag.Contains("Down"))
                        {
                            Debug.Log("OnEndDrag");
                            if (ClickCheck(eventData.pointerCurrentRaycast.gameObject.name) == true)
                            {
                                Debug.Log("No");
                                Clear();
                                return;
                            }
                            else
                            {
                                Debug.Log("OK");
                            }

                            LineCheck[1] = true;

                            storageUpText = eventData.pointerCurrentRaycast.gameObject.name;

                            if (LineConnection() == true)
                            {
                                if (cnt == 0)
                                {
                                    CheckAnswer1.Add(storageDownText);
                                    CheckAnswer1.Add(storageUpText);
                                }
                                else if (cnt == 1)
                                {
                                    CheckAnswer2.Add(storageDownText);
                                    CheckAnswer2.Add(storageUpText);
                                }
                                else if (cnt == 2)
                                {
                                    CheckAnswer3.Add(storageDownText);
                                    CheckAnswer3.Add(storageUpText);
                                }
                                ClickObjNameList.Add(storageDownText);
                                ClickObjNameList.Add(storageUpText);
                            }

                            //ClickObjNameList.Add(eventData.pointerCurrentRaycast.gameObject.name);
                            lines[cnt].Points[1].x = eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<RectTransform>().localPosition.x;
                            lines[cnt].Points[1].y = eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<RectTransform>().localPosition.y;
                            
                            cnt++;

                            if (cnt == 3)
                            {
                                Debug.Log("완료");
                                CheckAnswer();
                            }
                            isObjClick = false;
                        }
                        else
                        {
                            Clear();
                        }
                    }


                    else if (isUp == false)
                    {
                        if (eventData.pointerCurrentRaycast.gameObject.tag.Contains("Up"))
                        {
                            Debug.Log("OnEndDrag");
                            if (ClickCheck(eventData.pointerCurrentRaycast.gameObject.name) == true)
                            {
                                Debug.Log("No");
                                Clear();
                                return;
                            }
                            else
                            {
                                Debug.Log("OK");
                            }

                            LineCheck[1] = true;

                            storageUpText = eventData.pointerCurrentRaycast.gameObject.name;

                            if (LineConnection())
                            {
                                if (cnt == 0)
                                {
                                    CheckAnswer1.Add(storageDownText);
                                    CheckAnswer1.Add(storageUpText);
                                }
                                else if (cnt == 1)
                                {
                                    CheckAnswer2.Add(storageDownText);
                                    CheckAnswer2.Add(storageUpText);
                                }
                                else if (cnt == 2)
                                {
                                    CheckAnswer3.Add(storageDownText);
                                    CheckAnswer3.Add(storageUpText);
                                }
                                ClickObjNameList.Add(storageDownText);
                                ClickObjNameList.Add(storageUpText);
                            }

                            lines[cnt].Points[1].x = eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<RectTransform>().localPosition.x;
                            lines[cnt].Points[1].y = eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<RectTransform>().localPosition.y;
                            cnt++;
                            if (cnt == 3)
                            {
                                Debug.Log("완료");
                                CheckAnswer();
                            }
                            isObjClick = false;
                        }
                        else
                        {
                            Clear();
                        }
                    }
                }
                else if (eventData.pointerCurrentRaycast.gameObject.tag.Contains("Text") == false)
                {
                    Debug.Log("OnEndDrag Null");
                    Clear();
                }
            }
            // 드래그가 끝났을 때 아무것도 없는곳에서 드래그가 끝나면 라인을 초기화 해줘야함
            else //if (eventData.pointerCurrentRaycast.gameObject == null)
            {
                Debug.Log("OnEndDrag Null");
                Clear();
            }           
        }

        public bool isLine = false;

        public void CheckAnswer()
        {
            isLine = false;
            SelfAssertion.Instance.isAction = false;
            // 완료하면 버튼 전부 꺼준다.
            button1.interactable = false;
            button2.interactable = false;
            button3.interactable = false;
            button1_1.interactable = false;
            button2_2.interactable = false;
            button3_3.interactable = false;

            List<bool> AnswerList = new List<bool>();

            // 답 체크 시작
            for (int cnt = 0; cnt < CheckAnswer1.Count; cnt++)
            {
                if (CheckAnswer1[cnt].Contains("2") == true || CheckAnswer1[cnt].Contains("3") == true)
                {
                    Debug.Log("오답");
                    AnswerList.Add(false);
                }
                else
                {
                    Debug.Log("정답");
                    AnswerList.Add(true);
                }
            }

            for (int cnt = 0; cnt < CheckAnswer2.Count; cnt++)
            {
                if (CheckAnswer2[cnt].Contains("1") == true || CheckAnswer2[cnt].Contains("3") == true)
                {
                    Debug.Log("오답");
                    AnswerList.Add(false);
                }
                else
                {
                    Debug.Log("정답");
                    AnswerList.Add(true);
                }
            }

            for (int cnt = 0; cnt < CheckAnswer3.Count; cnt++)
            {
                if (CheckAnswer3[cnt].Contains("1") == true || CheckAnswer3[cnt].Contains("2") == true)
                {
                    Debug.Log("오답");
                    AnswerList.Add(false);
                }
                else
                {
                    Debug.Log("정답");
                    AnswerList.Add(true);
                }
            }

            for (int cnt = 0; cnt < AnswerList.Count; cnt++)
            {
                if (AnswerList[cnt] == false)
                {   
                    lines[0].Points[0].x = button1.GetComponent<RectTransform>().localPosition.x;
                    lines[0].Points[0].y = button1.GetComponent<RectTransform>().localPosition.y;

                    lines[0].Points[1].x = button1_1.GetComponent<RectTransform>().localPosition.x;
                    lines[0].Points[1].y = button1_1.GetComponent<RectTransform>().localPosition.y;



                    lines[1].Points[0].x = button2.GetComponent<RectTransform>().localPosition.x;
                    lines[1].Points[0].y = button2.GetComponent<RectTransform>().localPosition.y;

                    lines[1].Points[1].x = button2_2.GetComponent<RectTransform>().localPosition.x;
                    lines[1].Points[1].y = button2_2.GetComponent<RectTransform>().localPosition.y;



                    lines[2].Points[0].x = button3.GetComponent<RectTransform>().localPosition.x;
                    lines[2].Points[0].y = button3.GetComponent<RectTransform>().localPosition.y;

                    lines[2].Points[1].x = button3_3.GetComponent<RectTransform>().localPosition.x;
                    lines[2].Points[1].y = button3_3.GetComponent<RectTransform>().localPosition.y;
                    break;
                }
            }
            Debug.Log("정답 보여");
            StartCoroutine(ShowAnswerRoutine());
        }

        IEnumerator ShowAnswerRoutine()
        {
            int cnt = 0;
            while (cnt < 6)
            {
                lines[0].gameObject.SetActive(false);
                lines[1].gameObject.SetActive(false);
                lines[2].gameObject.SetActive(false);
                yield return new WaitForSeconds(0.5f);
                lines[0].gameObject.SetActive(true);
                lines[1].gameObject.SetActive(true);
                lines[2].gameObject.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                cnt++;
                yield return null;
            }
            ResetLine();
            SelfAssertion.Instance.PlayTimeLine();
            yield return null;
        }

        /// <summary>
        /// 클릭시 기존에 클릭했던건지 판별해주는 용도
        /// 기존에 이미 클릭을해서 선연결을 했다면 false를 리턴
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool ClickCheck(string name)
        {
            bool isSame = false;
            for (int cnt = 0; cnt < ClickObjNameList.Count; cnt++)
            {
                if (ClickObjNameList[cnt] == name)
                {
                    isSame = true;
                }
            }
            return isSame;
        }

        /// <summary>
        /// 양쪽에 라인 2개가 전부 연결되면 true를 반환, 한쪽이라도 연결이 안되면 false를 반환
        /// </summary>
        /// <returns></returns>
        public bool LineConnection()
        {
            bool isConnect = false;
            for (int cnt = 0; cnt < LineCheck.Length; cnt++)
            {
                if (LineCheck[cnt] == false)
                {
                    isConnect = false;
                    break;
                }
                else
                {
                    isConnect = true;
                }
            }
            return isConnect;
        }

        /// <summary>
        /// 라인 렌더러의 포인트를 모두 삭제해줌
        /// </summary>
        public void Clear()
        {
            storageDownText = null;
            storageUpText = null;

            LineCheck[0] = false;
            LineCheck[1] = false;

            lines[cnt].gameObject.SetActive(false);
            pointList.Clear();
            lines[cnt].Points = pointList.ToArray();
            isObjClick = false;
        }



        /// <summary>
        /// 라인을 꺼주고 초기화합니다.
        /// </summary>
        public void ResetLine()
        {
            button1.interactable = true;
            button2.interactable = true;
            button3.interactable = true;
            button1_1.interactable = true;
            button2_2.interactable = true;
            button3_3.interactable = true;

            storageDownText = null;
            storageUpText = null;

            LineCheck[0] = false;
            LineCheck[1] = false;

            isObjClick = false;

            ClickObjNameList.Clear();

            cnt = 0;

            for (int cnt = 0; cnt < lines.Length; cnt++)
            {
                pointList.Clear();
                lines[cnt].Points = pointList.ToArray();
                lines[cnt].gameObject.SetActive(false);
            }
        }
        


        /// <summary>
        /// 버튼을 2개 눌러서 라인을 연결해줍니다.
        /// </summary>
        /// <param name="vector2"></param>
        public void LineButton(Vector2 vector2)
        {
            Debug.Log(vector2.x);
            Debug.Log(vector2.y);
            lines[cnt].gameObject.SetActive(true);
            var point = new Vector2() { x = vector2.x, y = vector2.y };
            var pointList = new List<Vector2>(lines[cnt].Points);
            pointList.Add(point);
            Debug.Log(pointList.Count + " : 개");

            lines[cnt].Points = pointList.ToArray();

            if (pointList.Count == 2)
            {
                pointList.Clear();
                cnt++;
            }
        }

        /// <summary>
        /// 버튼을 처음 눌렀을 때 같은 위치값을 2개 넣어서 포인트를 생성해준다.
        /// </summary>
        /// <param name="vector2"></param>
        public void ButtonPush(Vector2 vector2, bool isNull = false)
        {
            //
            if (isNull == false)
            {
                lines[cnt].gameObject.SetActive(true);
                var pointList = new List<Vector2>(lines[cnt].Points);
                pointList.Add(vector2);
                pointList.Add(vector2);
                lines[cnt].Points = pointList.ToArray();

                isClick = true;
            }
            else if (isNull == true)
            {
                lines[cnt].gameObject.SetActive(false);
                var pointList = new List<Vector2>(lines[cnt].Points);
                pointList.Add(vector2);
                pointList.Add(vector2);
                lines[cnt].Points = pointList.ToArray();

                isClick = true;
            }

        }

        /// <summary>
        /// 버튼을 누른 후 드래그 하는동안 두번째 포인트를 Update에서 계속 따라가게 만들어준다.
        /// </summary>
        /// <param name="vector2"></param>
        public void ButtonDrag(Vector2 vector2)
        {
            lines[cnt].Points[1].x = vector2.x;
            lines[cnt].Points[1].y = vector2.y;
        }

        /// <summary>
        /// 버튼을 떼면 마지막 떼는 순간의 위치값을 두번째 포인트 값에 넣어줘서 선을 만들어준다.
        /// </summary>
        /// <param name="vector2"></param>
        public void ButtonUp(Vector2 vector2)
        {
            lines[cnt].Points[1].x = vector2.x;
            lines[cnt].Points[1].y = vector2.y;
            cnt++;
            isClick = false;
        }


        //private LineRenderer lineRend;
        //private Vector2 mousePos;
        //private Vector2 StartMousePos;

        //private void Start()
        //{
        //    lineRend = GetComponent<LineRenderer>();
        //    lineRend.positionCount = 2;
        //    Debug.Log(Camera.main.name + " : Camera.main");
        //}

        //private void Update()
        //{
        //    //if (OVRInput.GetUp(OVRInput.Button.Two))
        //    //{
        //    //    StartMousePos = Camera.main.ScreenToWorldPoint();
        //    //}
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        //        StartMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    }

        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        lineRend.SetPosition(0, new Vector3(StartMousePos.x, StartMousePos.y, 0f));
        //        lineRend.SetPosition(1, new Vector3(mousePos.x, mousePos.y, 0f));

        //    }

        //}

    }
}