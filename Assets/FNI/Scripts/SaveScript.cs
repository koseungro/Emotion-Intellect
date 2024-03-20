/// 작성자: 김윤빈
/// 작성일: 2020-07-30
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using CurvedUI;
using FNI;
using FNIUnityEngine.hjchae;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace FNI
{
    public class SaveScript : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
    {

        public UILineRenderer lineRenderer1;
        public UILineRenderer lineRenderer2;
        public UILineRenderer lineRenderer3;

        public UILineRenderer[] lines;

        public Button button1;
        public Button button1_1;
        public Button button2;
        public Button button2_2;
        public Button button3;
        public Button button3_3;

        private void Start()
        {
            button1.onClick.AddListener(delegate
            {
                LineButton(button1.GetComponent<RectTransform>().localPosition);
            });
            button1_1.onClick.AddListener(delegate
            {
                LineButton(button1_1.GetComponent<RectTransform>().localPosition);
            });
            button2.onClick.AddListener(delegate
            {
                LineButton(button2.GetComponent<RectTransform>().localPosition);
            });
            button2_2.onClick.AddListener(delegate
            {
                LineButton(button2_2.GetComponent<RectTransform>().localPosition);
            });
            button3.onClick.AddListener(delegate
            {
                LineButton(button3.GetComponent<RectTransform>().localPosition);
            });
            button3_3.onClick.AddListener(delegate
            {
                LineButton(button3_3.GetComponent<RectTransform>().localPosition);
            });

            Instantiate(lineRenderer1, mycanvas.transform);
        }

        public GameObject test1;

        float length = 10000f;
        RaycastHit hit;
        public GameObject Controller;
        public Canvas mycanvas;

        Vector2 pointOnCanvas;
        private void Update()
        {

            Ray myRay = new Ray(Controller.transform.position, Controller.transform.forward);
            Debug.DrawRay(Controller.transform.position, Controller.transform.forward * length, Color.blue, 0.1f);
            if (Physics.Raycast(myRay, out hit, length))
            {
                if (hit.transform.tag == "Canvas")
                {
                    //RectTransform CanvasRect = hit.transform.GetComponent<RectTransform>();

                    //then you calculate the position of the UI element
                    //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

                    //Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(WorldObject.transform.position);
                    //Vector2 WorldObject_ScreenPosition = new Vector2(
                    //((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
                    //((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

                    //Debug.Log(WorldObject_ScreenPosition.x + " : x444444444444");
                    //Debug.Log(WorldObject_ScreenPosition.y + " : y444444444444");


                    Vector3 localHitPoint = hit.transform.gameObject.GetComponent<Canvas>().transform.worldToLocalMatrix.MultiplyPoint3x4(hit.point);
                    pointOnCanvas = new Vector3(0, 0, 0);
                    pointOnCanvas.x = localHitPoint.x;
                    pointOnCanvas.y = localHitPoint.y;
                    //button1.GetComponent<RectTransform>().localPosition = new Vector2(pointOnCanvas.x, pointOnCanvas.y);                  


                    if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
                    {
                        //Button1(pointOnCanvas);

                        //ButtonUp(pointOnCanvas);
                        //Debug.Log("GetUp");
                    }

                    if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
                    {
                        if (isClick == true)
                        {
                            //Debug.Log("Get");
                            //ButtonDrag(pointOnCanvas);
                        }
                    }

                    if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
                    {
                        //Button1(pointOnCanvas);

                        //Debug.Log("GetDown");
                        //ButtonPush(pointOnCanvas);
                    }

                }

                if (hit.transform.tag == "Finish")
                {
                    Debug.Log("PlayerPlayerPlayerPlayerPlayerPlayer");

                }

                //if (IPointerEnterHandler.pointerCurrentRaycast.gameObject != null)
                //{
                //    Debug.Log("IPointerEnterHandler.pointerCurrentRaycast.gameObject.nameIPointerEnterHandler.pointerCurrentRaycast.gameObject.nameIPointerEnterHandler.pointerCurrentRaycast.gameObject.name");
                //}

                // 맞으면 아웃라인
                //hit.transform.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", 1.02f);
            }
        }

        int cnt = 0;
        bool isClick = false;

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



        // 클릭하는 순간, UI 안에 포인터가 있다면 그 해당 UI중간값을 넘겨줘서 라인pointer[0] 값으로 넣어줌
        // 클릭을 떼는 순간 UI안에 포인터가 있으면 그 해당 UI 중간값을 넘겨줘서 라인pointer[1] 값으로 넣어줌
        // 위치값은 2개가 필요함

        // 위에 버튼 or 밑에버튼 아무거나 먼저 누르면 배열에 하나 저장을 하고....
        // 만약 중간에 버튼 뗐는데 아무것도 없다? 그러면 포인터 다 없엠

        bool isObjClick = false;
        string currentObjTag;
        List<Vector2> pointList = new List<Vector2>();

        public void OnPointerDown(PointerEventData eventData)
        {
            if (cnt >= 3)
            {
                return;
            }
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.tag.Contains("Text"))
                {
                    if (eventData.pointerCurrentRaycast.gameObject.tag.Contains("Up"))
                    {
                        currentObjTag = "Up";
                        Debug.Log(currentObjTag + " : currentObjTag");
                    }
                    else if (eventData.pointerCurrentRaycast.gameObject.tag.Contains("Down"))
                    {
                        currentObjTag = "Down";
                        Debug.Log(currentObjTag + " : currentObjTag");
                    }

                    pointList.Clear();
                    lines[cnt].gameObject.SetActive(true);
                    //pointList = new List<Vector2>(lines[cnt].Points);
                    pointList.Add(eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<RectTransform>().localPosition);
                    pointList.Add(eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<RectTransform>().localPosition);
                    lines[cnt].Points = pointList.ToArray();

                    isObjClick = true;
                }
            }
            else if (eventData.pointerCurrentRaycast.gameObject == null)
            {
                lines[cnt].gameObject.SetActive(false);
                var pointList = new List<Vector2>(lines[cnt].Points);
                pointList.Add(new Vector2(0, 0));
                pointList.Add(new Vector2(0, 0));
                lines[cnt].Points = pointList.ToArray();

                isObjClick = false;
            }




            // Debug.Log("OnPointerDownOnPointerDownOnPointerDownOnPointerDown");
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (cnt >= 3)
            {
                return;
            }
            if (isObjClick == true)
            {
                Debug.Log("OnDrag");
                lines[cnt].Points[1].x = pointOnCanvas.x;
                lines[cnt].Points[1].y = pointOnCanvas.y;
            }

        }

        public void OnEndDrag(PointerEventData eventData)
        {
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
                    Debug.Log(currentObjTag + " : currentObjTag");
                    if (currentObjTag == "Up")
                    {
                        if (eventData.pointerCurrentRaycast.gameObject.tag.Contains("Down"))
                        {
                            Debug.Log("OnEndDrag");
                            lines[cnt].Points[1].x = eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<RectTransform>().localPosition.x;
                            lines[cnt].Points[1].y = eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<RectTransform>().localPosition.y;
                            cnt++;
                            isObjClick = false;
                        }
                        else
                        {
                            lines[cnt].gameObject.SetActive(false);
                            //var pointList = new List<Vector2>(lines[cnt].Points);
                            //pointList.Add(new Vector2(0, 0));
                            //pointList.Add(new Vector2(0, 0));
                            pointList.Clear();
                            lines[cnt].Points = pointList.ToArray();
                            isObjClick = false;
                        }
                    }
                    else if (currentObjTag == "Down")
                    {
                        if (eventData.pointerCurrentRaycast.gameObject.tag.Contains("Up"))
                        {
                            Debug.Log("OnEndDrag");
                            lines[cnt].Points[1].x = eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<RectTransform>().localPosition.x;
                            lines[cnt].Points[1].y = eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<RectTransform>().localPosition.y;
                            cnt++;
                            isObjClick = false;
                        }
                        else
                        {
                            lines[cnt].gameObject.SetActive(false);
                            //var pointList = new List<Vector2>(lines[cnt].Points);
                            //pointList.Add(new Vector2(0, 0));
                            //pointList.Add(new Vector2(0, 0));
                            pointList.Clear();
                            lines[cnt].Points = pointList.ToArray();
                            isObjClick = false;
                        }
                    }

                }
                else if (eventData.pointerCurrentRaycast.gameObject.tag.Contains("Text") == false)
                {
                    Debug.Log("OnEndDrag Null");
                    lines[cnt].gameObject.SetActive(false);
                    //var pointList = new List<Vector2>(lines[cnt].Points);
                    //pointList.Add(new Vector2(0, 0));
                    //pointList.Add(new Vector2(0, 0));
                    pointList.Clear();
                    lines[cnt].Points = pointList.ToArray();
                    isObjClick = false;
                }
            }
            // 드래그가 끝났을 때 아무것도 없는곳에서 드래그가 끝나면 라인을 초기화 해줘야함
            else //if (eventData.pointerCurrentRaycast.gameObject == null)
            {
                Debug.Log("OnEndDrag Null");
                lines[cnt].gameObject.SetActive(false);
                pointList.Clear();
                //var pointList = new List<Vector2>(lines[cnt].Points);
                //pointList.Add(new Vector2(0, 0));
                //pointList.Add(new Vector2(0, 0));
                lines[cnt].Points = pointList.ToArray();
                isObjClick = false;
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