/// 작성자: 김윤빈
/// 작성일: 2020-06-29
/// 수정일: 
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력

using FNI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using NUnit.Framework;

namespace FNI
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ContentsData))]
    public class CustomSriptable : Editor
    {
        private ContentsData scriptable;
        private Contents contents;

        

        private void OnEnable()
        {
            scriptable = base.target as ContentsData;
        }


        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField("Contents Name", GUILayout.Width(90));
                    scriptable.ContentsName = EditorGUILayout.TextField(scriptable.ContentsName);
                }
                EditorGUILayout.EndHorizontal();

                if (scriptable.ContentsName == "스트레스란?1_2")
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            scriptable.isRock = GUILayout.Toggle(scriptable.isRock, scriptable.isRock ? "⊂[]" : "_[]", EditorStyles.miniButton, GUILayout.Width(30));
                            EditorGUI.BeginDisabledGroup(scriptable.isRock);
                            {

                                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                                {
                                    scriptable.isFirst = EditorGUILayout.Toggle("1회차", scriptable.isFirst);
                                    //scriptable.isFirst = GUILayout.Toggle(scriptable.isFirst, "1회차", EditorStyles.miniButton, GUILayout.Width(50));
                                }
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                                {
                                    scriptable.isSecond = EditorGUILayout.Toggle("2회차", scriptable.isSecond);
                                    //scriptable.isSecond = GUILayout.Toggle(scriptable.isSecond, "2회차", EditorStyles.miniButton, GUILayout.Width(50));
                                }
                                EditorGUILayout.EndHorizontal();


                            }
                            EditorGUI.EndDisabledGroup();
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.Space();


                //EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                //{
                //    EditorGUILayout.LabelField("Contents Count", GUILayout.Width(90));
                //    count = EditorGUILayout.IntField(scriptable.contentsList.Length);

                //    Debug.Log(count + " : count1");
                //}
                //EditorGUILayout.EndHorizontal();

                // -------------------------------------------------------------------------------------------------------------
                EditorGUILayout.Space();

                //if (scriptable.contentsList.Length != count)
                //{
                //    scriptable.contentsList = CopyArray<Contents>(count, scriptable.contentsList);
                //}

                scriptable.ContentsList = ListController(scriptable.ContentsList, true);                

                for (int cnt = 0; cnt < scriptable.ContentsList.Count; cnt++)
                {
                    Contents contents = scriptable.ContentsList[cnt];
                    if (contents == null) break;

                    EditorGUILayout.BeginHorizontal();
                    {
                        scriptable.ContentsList = ListController(scriptable.ContentsList, cnt, false);
                        EditorGUILayout.LabelField("ContentData" + (cnt + 1), GUILayout.Width(100));
                        //scriptable.contentsList[cnt] = (Contents)EditorGUILayout.ObjectField(scriptable.contentsList[cnt], typeof(Contents));
                        //datas[cnt].isInteractable = GUILayout.Toggle(datas[cnt].isInteractable, "Interactable", EditorStyles.miniButton, GUILayout.Width(80));
                    }
                    EditorGUILayout.EndHorizontal();
                    if (contents == null) break;

                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    {
                        EditorGUILayout.BeginVertical();
                        {
                            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                            {
                                contents.contentType = (PlayState)EditorGUILayout.EnumPopup("콘텐츠타입", contents.contentType);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.EndVertical();

                        switch (scriptable.ContentsList[cnt].contentType)
                        {
                            case PlayState.ButtonGroup:

                                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                                {
                                    scriptable.ContentsList[cnt].buttonData = Buttons(scriptable.ContentsList[cnt].buttonData);
                                    scriptable.ContentsList[cnt].nextContentsList = NextContents(scriptable.ContentsList[cnt].nextContentsList);

                                }
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                                {
                                    contents.xPos = EditorGUILayout.FloatField("Button X Position", contents.xPos);
                                }
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                                {
                                    contents.yPos = EditorGUILayout.FloatField("Button Y Position", contents.yPos);
                                }
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                                {
                                    contents.zPos = EditorGUILayout.FloatField("Button Y Position", contents.zPos);
                                }
                                EditorGUILayout.EndHorizontal();

                                break;
                            case PlayState.Animation:
                                EditorGUILayout.BeginVertical();
                                {
                                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                                    {
                                        EditorGUILayout.LabelField("AnimationName", GUILayout.Width(100));
                                        contents.animationName = EditorGUILayout.TextField(contents.animationName);
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }
                                EditorGUILayout.EndVertical();
                                break;

                            case PlayState.FadeDelayTime:
                                EditorGUILayout.BeginVertical();
                                {
                                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                                    {
                                        EditorGUILayout.LabelField("delayTime", GUILayout.Width(100));
                                        contents.delayTime = EditorGUILayout.FloatField(contents.delayTime);
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }
                                EditorGUILayout.EndVertical();
                                break;
                            case PlayState.VideoLoadAndPlay:
                                EditorGUILayout.BeginVertical();
                                {
                                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                                    {
                                        EditorGUILayout.LabelField("MovieName", GUILayout.Width(100));
                                        contents.movieName = EditorGUILayout.TextField(contents.movieName);
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }
                                EditorGUILayout.EndVertical();
                                break;

                            case PlayState.WaitVideoTime:
                                EditorGUILayout.BeginVertical();
                                {
                                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                                    {
                                        EditorGUILayout.LabelField("WaitVideoTime", GUILayout.Width(100));
                                        contents.waitVideoTime = EditorGUILayout.FloatField(contents.waitVideoTime);
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }
                                EditorGUILayout.EndVertical();
                                break;

                            case PlayState.Guide:
                                EditorGUILayout.BeginVertical();
                                {
                                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                                    {
                                        //EditorGUILayout.LabelField("안내 문구", GUILayout.Width(100));
                                        //contents.guideComment = EditorGUILayout.TextField(contents.guideComment);
                                        //contents.contentType = (PlayState)EditorGUILayout.EnumPopup("콘텐츠타입", contents.contentType);
                                        contents.emotionVideoOption = (EmotionVideoOption)EditorGUILayout.EnumPopup("시작 메뉴", contents.emotionVideoOption);
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }
                                EditorGUILayout.EndVertical();
                                break;

                            //case PlayState.Step1_1:
                            //    EditorGUILayout.BeginVertical();
                            //    {
                            //        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                            //        {
                            //            EditorGUILayout.LabelField("AnimationName", GUILayout.Width(100));
                            //            contents.animationName = EditorGUILayout.TextField(contents.animationName);
                            //        }
                            //        EditorGUILayout.EndHorizontal();
                            //    }
                            //    EditorGUILayout.EndVertical();
                            //    break;
                            //case PlayState.Step1_2:
                            //    EditorGUILayout.BeginVertical();
                            //    {
                            //        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                            //        {
                            //            EditorGUILayout.LabelField("AnimationName", GUILayout.Width(100));
                            //            contents.animationName = EditorGUILayout.TextField(contents.animationName);
                            //        }
                            //        EditorGUILayout.EndHorizontal();
                            //    }
                            //    EditorGUILayout.EndVertical();
                            //    break;
                            //case PlayState.Step2_1:
                            //    EditorGUILayout.BeginVertical();
                            //    {
                            //        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                            //        {
                            //            EditorGUILayout.LabelField("AnimationName", GUILayout.Width(100));
                            //            contents.animationName = EditorGUILayout.TextField(contents.animationName);
                            //        }
                            //        EditorGUILayout.EndHorizontal();
                            //    }
                            //    EditorGUILayout.EndVertical();
                            //    break;
                            //case PlayState.Step2_2:
                            //    EditorGUILayout.BeginVertical();
                            //    {
                            //        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                            //        {
                            //            EditorGUILayout.LabelField("AnimationName", GUILayout.Width(100));
                            //            contents.animationName = EditorGUILayout.TextField(contents.animationName);
                            //        }
                            //        EditorGUILayout.EndHorizontal();
                            //    }
                            //    EditorGUILayout.EndVertical();
                            //    break;
                            //case PlayState.Step3_1:
                            //    EditorGUILayout.BeginVertical();
                            //    {
                            //        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                            //        {
                            //            EditorGUILayout.LabelField("AnimationName", GUILayout.Width(100));
                            //            contents.animationName = EditorGUILayout.TextField(contents.animationName);
                            //        }
                            //        EditorGUILayout.EndHorizontal();
                            //    }
                            //    EditorGUILayout.EndVertical();
                            //    break;
                            //case PlayState.Step3_2:
                            //    EditorGUILayout.BeginVertical();
                            //    {
                            //        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                            //        {
                            //            EditorGUILayout.LabelField("AnimationName", GUILayout.Width(100));
                            //            contents.animationName = EditorGUILayout.TextField(contents.animationName);
                            //        }
                            //        EditorGUILayout.EndHorizontal();
                            //    }
                            //    EditorGUILayout.EndVertical();
                            //    break;
                            //case PlayState.Step4_1:
                            //    EditorGUILayout.BeginVertical();
                            //    {
                            //        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                            //        {
                            //            EditorGUILayout.LabelField("AnimationName", GUILayout.Width(100));
                            //            contents.animationName = EditorGUILayout.TextField(contents.animationName);
                            //        }
                            //        EditorGUILayout.EndHorizontal();
                            //    }
                            //    EditorGUILayout.EndVertical();
                            //    break;
                            //case PlayState.Step4_2:
                            //    EditorGUILayout.BeginVertical();
                            //    {
                            //        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                            //        {
                            //            EditorGUILayout.LabelField("AnimationName", GUILayout.Width(100));
                            //            contents.animationName = EditorGUILayout.TextField(contents.animationName);
                            //        }
                            //        EditorGUILayout.EndHorizontal();
                            //    }
                            //    EditorGUILayout.EndVertical();
                            //    break;
                            //case PlayState.Step5_1:
                            //    EditorGUILayout.BeginVertical();
                            //    {
                            //        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                            //        {
                            //            EditorGUILayout.LabelField("AnimationName", GUILayout.Width(100));
                            //            contents.animationName = EditorGUILayout.TextField(contents.animationName);
                            //        }
                            //        EditorGUILayout.EndHorizontal();
                            //    }
                            //    EditorGUILayout.EndVertical();
                            //    break;
                            //case PlayState.Step5_2:
                            //    EditorGUILayout.BeginVertical();
                            //    {
                            //        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                            //        {
                            //            EditorGUILayout.LabelField("AnimationName", GUILayout.Width(100));
                            //            contents.animationName = EditorGUILayout.TextField(contents.animationName);
                            //        }
                            //        EditorGUILayout.EndHorizontal();
                            //    }
                            //    EditorGUILayout.EndVertical();
                            //    break;

                            case PlayState.Record:
                                break;
                            case PlayState.FadeIn:
                                break;
                            case PlayState.FadeOut:
                                break;
                            case PlayState.VideoPause:
                                break;
                        }
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space(15);
                }
            }
            EditorGUILayout.EndVertical();

            //여기까지 검사해서 필드에 변화가 있으면
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(targets, "Changed Update Mode");
                //변경이 있을 시 적용된다. 이 코드가 없으면 인스펙터 창에서 변화는 있지만 적용은 되지 않는다.
                EditorUtility.SetDirty(scriptable);
            }
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
        #region
//        EditorGUILayout.BeginVertical();
//                                {
//                                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
//                                    {

//                                        EditorGUILayout.BeginHorizontal();
//                                        {
//                                            scriptable.buttonList = ListController(scriptable.buttonList);
//        EditorGUILayout.LabelField("buttonData", GUILayout.Width(100));
                                            
//                                        }
//    EditorGUILayout.EndHorizontal();


//                                        if (scriptable.buttonList == null) break;
//                                        for (int i = 0; i<scriptable.buttonList.Count; i++)
//                                        {                                           
//                                            EditorGUILayout.BeginHorizontal();
//                                            {
//                                                scriptable.buttonList = ListController(scriptable.buttonList, i, false);
//    //scriptable.contentsList[cnt] = (Contents)EditorGUILayout.ObjectField(scriptable.contentsList[cnt], typeof(Contents));
//    //datas[cnt].isInteractable = GUILayout.Toggle(datas[cnt].isInteractable, "Interactable", EditorStyles.miniButton, GUILayout.Width(80));
//}
//EditorGUILayout.EndHorizontal();
//                                            if (scriptable.buttonList == null) break;
//                                            //contents.buttonData = (IS_ButtonData)EditorGUILayout.ObjectField(contents.buttonData, typeof(IS_ButtonData), true);
//                                        }


//                                        //scriptable.ButtonCount = EditorGUILayout.IntField("갯수", scriptable.ButtonCount);
//                                        //for (int i = 0; i< scriptable.ButtonCount; i++)
//                                        //{
//                                        //    contents.buttonData = (IS_ButtonData)EditorGUILayout.ObjectField(contents.buttonData, typeof(IS_ButtonData), true);
//                                        //}                                        
//                                    }
//                                    EditorGUILayout.EndHorizontal();
//                                }
//                                EditorGUILayout.EndVertical();
        #endregion

        /// <summary>
        /// 배열을 카피하여 줍니다.
        /// datas보다 count가 많으면 남는 곳은 기본 형태로 만들어줍니다.
        /// datas보다 count가 적으면 datas중 오버되는 배열을 삭제 합니다.
        /// </summary>
        /// <typeparam name="T">배열 형식을 사용합니다.</typeparam>
        /// <param name="makeCount">지정할 갯수</param>
        /// <param name="originList">원본 데이터</param>
        /// <returns>복사 된 배열</returns>
        private T[] CopyArray<T>(int makeCount, T[] originList)
        {
            T[] newlist = new T[makeCount];
            for (int cnt = 0; cnt < makeCount; cnt++)
            {
                if (originList != null)
                {
                    if (cnt < originList.Length)
                        newlist[cnt] = originList[cnt];
                    else
                    {
                        if (originList.Length != 0)
                            newlist[cnt] = originList[originList.Length - 1];
                        else
                            newlist[cnt] = default(T);
                    }
                }
                else
                    newlist[cnt] = default(T);
            }
            return newlist;
        }


        private List<T> ListController<T>(List<T> list, bool isRight = true)
        {
            if (list == null)
                list = new List<T>();

            if (isRight)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space();
            }
            else
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(120));
            {
                EditorGUILayout.LabelField($"Total[{list.Count}]", GUILayout.MaxWidth(60));


                if (GUILayout.Button("R", GUILayout.Width(20)))
                {
                    if (list.Count != 0)
                    {
                        if (EditorUtility.DisplayDialog("경고", "초기화 할거임?\n복구 못해", "응", "아니"))
                            list = new List<T>();
                    }
                    else
                        EditorUtility.DisplayDialog("이런", "초기화 할게 없져", "응");
                }
                if (GUILayout.Button("+", GUILayout.Width(20)))
                {
                    list.Add(default);
                }
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    if (EditorUtility.DisplayDialog("경고", "갯수 줄일거임?\n줄이면 값 넣은거 사라져", "응", "아니"))
                    {
                        if (list.Count != 0)
                            list.RemoveAt(list.Count - 1);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            return list;
        }
        private List<T> ListController<T>(List<T> list, int num, bool isRight = true)
        {
            if (list == null)
                list = new List<T>();
            if (list.Count == 0)
                list.Add(default);

            if (isRight)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space();
            }
            else
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(60));
            {
                if (GUILayout.Button("D", GUILayout.Width(20)))
                {
                    if (EditorUtility.DisplayDialog("경고", "삭제 할거임?\n복구 못해", "응", "아니"))
                        list.RemoveAt(num);
                }

                EditorGUI.BeginDisabledGroup(!(0 < num));
                {
                    if (GUILayout.Button("△", GUILayout.Width(20)))
                    {
                        list.Insert(num - 1, list[num]);
                        list.RemoveAt(num + 1);
                    }
                }
                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(!(num < list.Count - 1));
                {
                    if (GUILayout.Button("▽", GUILayout.Width(20)))
                    {
                        list.Insert(num + 2, list[num]);
                        list.RemoveAt(num);
                    }
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndHorizontal();

            return list;
        }

        private List<IS_ButtonData> Buttons(List<IS_ButtonData> buttonDatas)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                //if (buttonDatas == null)
                //    buttonDatas = new SceneButton[0];

                int count = 0;//배열의 갯수
                //EditorGUILayout.BeginHorizontal();
                //{
                //    EditorGUILayout.LabelField("Buttons", EditorStyles.boldLabel, GUILayout.Width(100));
                //    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                //    {
                //        count = EditorGUILayout.IntField(buttonDatas.Count);
                //    }
                //    EditorGUILayout.EndHorizontal();
                //}
                //EditorGUILayout.EndHorizontal();

                //if (buttonDatas.Count != count)//배열의 갯수에 맞춰서 새로 만들어 줍니다.
                //{
                //    buttonDatas = CopyArray<IS_ButtonData>(count, buttonDatas);

                //}
                buttonDatas = ListController(buttonDatas);

                for (int cnt = 0; cnt < buttonDatas.Count; cnt++)
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Button Data", GUILayout.Width(70));
                            buttonDatas[cnt] = (IS_ButtonData)EditorGUILayout.ObjectField(buttonDatas[cnt], typeof(IS_ButtonData));
                            //buttonDatas[cnt].isInteractable = GUILayout.Toggle(buttonDatas[cnt].isInteractable, "Interactable", EditorStyles.miniButton, GUILayout.Width(80));
                        }
                        EditorGUILayout.EndHorizontal();

                        //buttonDatas[cnt].nextContentsList = NextScenesUI_Sub(false, buttonDatas[cnt].nextContentsList, "Next Scene");

                        //EditorGUILayout.BeginHorizontal();
                        //{
                        //    EditorGUILayout.LabelField("Position", GUILayout.Width(100));
                        //    buttonDatas[cnt].buttonPos = EditorGUILayout.Vector2Field("", buttonDatas[cnt].buttonPos);
                        //}
                        //EditorGUILayout.EndHorizontal();

                        //buttonDatas[cnt].nextScene = NextScenesUI_Sub(false, buttonDatas[cnt].nextScene, "Next Scene");
                    }
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndVertical();

            return buttonDatas;
        }


        private ContentsData NextScenesUI_Sub(bool isCustom, ContentsData value, string label)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(100));
                value = (ContentsData)EditorGUILayout.ObjectField(value, typeof(ContentsData));
            }
            EditorGUILayout.EndHorizontal();

            return value;
        }

        private List<ContentsData> NextContents(List<ContentsData> contentsDatas)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                contentsDatas = ListController(contentsDatas);

                for (int cnt = 0; cnt < contentsDatas.Count; cnt++)
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Contents Datas", GUILayout.Width(80));
                            contentsDatas[cnt] = (ContentsData)EditorGUILayout.ObjectField(contentsDatas[cnt], typeof(ContentsData));
                            //buttonDatas[cnt].isInteractable = GUILayout.Toggle(buttonDatas[cnt].isInteractable, "Interactable", EditorStyles.miniButton, GUILayout.Width(80));
                        }
                        EditorGUILayout.EndHorizontal();

                    }
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndVertical();

            return contentsDatas;
        }

    }

   

    #region
    //EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
    //{
    //    scriptable.contentType = (ContentType)EditorGUILayout.EnumPopup("콘텐츠", scriptable.contentType);
    //}
    //EditorGUILayout.EndHorizontal();

    //EditorGUILayout.BeginHorizontal(GUI.skin.box);
    //{
    //    switch (contentType = scriptable.contentType)
    //    {
    //        case ContentType.animation:
    //            scriptable.AnimationName = EditorGUILayout.TextField("AnimName", scriptable.AnimationName);
    //            break;
    //        case ContentType.buttonGroup:
    //            EditorGUILayout.BeginVertical();
    //            {
    //                EditorGUILayout.LabelField("buttons", EditorStyles.whiteLabel, GUILayout.Width(50));
    //                EditorGUILayout.BeginHorizontal();
    //                {
    //                    count = EditorGUILayout.IntField(count, GUILayout.Width(50));
    //                }
    //                EditorGUILayout.EndHorizontal();
    //            }
    //            EditorGUILayout.EndVertical();

    //            EditorGUILayout.BeginVertical();
    //            {
    //                for (int i = 0; i < count; i++)
    //                {
    //                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
    //                    {
    //                        EditorGUILayout.BeginHorizontal();
    //                        {
    //                            EditorGUILayout.LabelField("Button Data", GUILayout.Width(75));
    //                            scriptable.Button = (IS_ButtonData)EditorGUILayout.ObjectField(scriptable.Button, typeof(IS_ButtonData));
    //                        }
    //                        EditorGUILayout.EndHorizontal();
    //                    }
    //                    EditorGUILayout.EndHorizontal();
    //                }
    //            }
    //            EditorGUILayout.EndVertical();
    //            break;
    //        case ContentType.video:
    //            scriptable.MovieName = EditorGUILayout.TextField("VideoName", scriptable.MovieName);
    //            break;
    //        case ContentType.record:
    //            //굵은 선을 긋습니다.
    //            EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb, GUILayout.ExpandWidth(true), GUILayout.Height(1));
    //            EditorGUILayout.EndHorizontal();
    //            //굵은 선을 긋습니다.
    //            break;
    //        case ContentType.None:
    //            //굵은 선을 긋습니다.
    //            EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb, GUILayout.ExpandWidth(true), GUILayout.Height(1));
    //            EditorGUILayout.EndHorizontal();
    //            //굵은 선을 긋습니다.
    //            break;
    //    }
    //}
    //EditorGUILayout.EndHorizontal();
    #endregion

    // -------------------------------------------------------------------------------------------------------------

    #region
    //EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
    //{
    //    scriptable.contentType2 = (ContentType)EditorGUILayout.EnumPopup("콘텐츠 2", scriptable.contentType2);
    //}
    //EditorGUILayout.EndHorizontal();

    //EditorGUILayout.BeginHorizontal(GUI.skin.box);
    //{
    //    switch (contentType2 = scriptable.contentType2)
    //    {
    //        case ContentType.animation:
    //            scriptable.AnimationName = EditorGUILayout.TextField("AnimName", scriptable.AnimationName);
    //            break;
    //        case ContentType.buttonGroup:
    //            EditorGUILayout.BeginVertical();
    //            {
    //                EditorGUILayout.LabelField("buttons", EditorStyles.whiteLabel, GUILayout.Width(50));
    //                EditorGUILayout.BeginHorizontal();
    //                {
    //                    count = EditorGUILayout.IntField(count, GUILayout.Width(50));
    //                }
    //                EditorGUILayout.EndHorizontal();
    //            }
    //            EditorGUILayout.EndVertical();

    //            EditorGUILayout.BeginVertical();
    //            {
    //                for (int i = 0; i < count; i++)
    //                {
    //                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
    //                    {
    //                        EditorGUILayout.BeginHorizontal();
    //                        {
    //                            EditorGUILayout.LabelField("Button Data", GUILayout.Width(75));
    //                            scriptable.Button = (IS_ButtonData)EditorGUILayout.ObjectField(scriptable.Button, typeof(IS_ButtonData));
    //                        }
    //                        EditorGUILayout.EndHorizontal();
    //                    }
    //                    EditorGUILayout.EndHorizontal();
    //                }
    //            }
    //            EditorGUILayout.EndVertical();
    //            break;
    //        case ContentType.video:
    //            scriptable.MovieName = EditorGUILayout.TextField("VideoName", scriptable.MovieName);
    //            break;
    //        case ContentType.record:
    //            //굵은 선을 긋습니다.
    //            EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb, GUILayout.ExpandWidth(true), GUILayout.Height(1));
    //            EditorGUILayout.EndHorizontal();
    //            //굵은 선을 긋습니다.
    //            break;
    //        case ContentType.None:
    //            //굵은 선을 긋습니다.
    //            EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb, GUILayout.ExpandWidth(true), GUILayout.Height(1));
    //            EditorGUILayout.EndHorizontal();
    //            //굵은 선을 긋습니다.
    //            break;
    //    }
    //}
    //EditorGUILayout.EndHorizontal();

    //// -------------------------------------------------------------------------------------------------------------
    //EditorGUILayout.Space();

    //EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
    //{
    //    scriptable.contentType3 = (ContentType)EditorGUILayout.EnumPopup("콘텐츠 3", scriptable.contentType3);
    //}
    //EditorGUILayout.EndHorizontal();

    //EditorGUILayout.BeginHorizontal(GUI.skin.box);
    //{
    //    switch (contentType3 = scriptable.contentType3)
    //    {
    //        case ContentType.animation:
    //            scriptable.AnimationName = EditorGUILayout.TextField("AnimName", scriptable.AnimationName);
    //            break;
    //        case ContentType.buttonGroup:
    //            EditorGUILayout.BeginVertical();
    //            {
    //                EditorGUILayout.LabelField("buttons", EditorStyles.whiteLabel, GUILayout.Width(50));
    //                EditorGUILayout.BeginHorizontal();
    //                {
    //                    count = EditorGUILayout.IntField(count, GUILayout.Width(50));
    //                }
    //                EditorGUILayout.EndHorizontal();
    //            }
    //            EditorGUILayout.EndVertical();

    //            EditorGUILayout.BeginVertical();
    //            {
    //                for (int i = 0; i < count; i++)
    //                {
    //                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
    //                    {
    //                        EditorGUILayout.BeginHorizontal();
    //                        {
    //                            EditorGUILayout.LabelField("Button Data", GUILayout.Width(75));
    //                            scriptable.Button = (IS_ButtonData)EditorGUILayout.ObjectField(scriptable.Button, typeof(IS_ButtonData));
    //                        }
    //                        EditorGUILayout.EndHorizontal();
    //                    }
    //                    EditorGUILayout.EndHorizontal();
    //                }
    //            }
    //            EditorGUILayout.EndVertical();
    //            break;
    //        case ContentType.video:
    //            scriptable.MovieName = EditorGUILayout.TextField("VideoName", scriptable.MovieName);
    //            break;
    //        case ContentType.record:
    //            //굵은 선을 긋습니다.
    //            EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb, GUILayout.ExpandWidth(true), GUILayout.Height(1));
    //            EditorGUILayout.EndHorizontal();
    //            //굵은 선을 긋습니다.
    //            break;
    //        case ContentType.None:
    //            //굵은 선을 긋습니다.
    //            EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb, GUILayout.ExpandWidth(true), GUILayout.Height(1));
    //            EditorGUILayout.EndHorizontal();
    //            //굵은 선을 긋습니다.
    //            break;
    //    }
    //}
    //EditorGUILayout.EndHorizontal();

    //// -------------------------------------------------------------------------------------------------------------
    //EditorGUILayout.Space();

    //EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
    //{
    //    scriptable.contentType4 = (ContentType)EditorGUILayout.EnumPopup("콘텐츠 4", scriptable.contentType4);
    //}
    //EditorGUILayout.EndHorizontal();

    //EditorGUILayout.BeginHorizontal(GUI.skin.box);
    //{
    //    switch (contentType4 = scriptable.contentType4)
    //    {
    //        case ContentType.animation:
    //            scriptable.AnimationName = EditorGUILayout.TextField("AnimName", scriptable.AnimationName);
    //            break;
    //        case ContentType.buttonGroup:
    //            EditorGUILayout.BeginVertical();
    //            {
    //                EditorGUILayout.LabelField("buttons", EditorStyles.whiteLabel, GUILayout.Width(50));
    //                EditorGUILayout.BeginHorizontal();
    //                {
    //                    count = EditorGUILayout.IntField(count, GUILayout.Width(50));
    //                }
    //                EditorGUILayout.EndHorizontal();
    //            }
    //            EditorGUILayout.EndVertical();

    //            EditorGUILayout.BeginVertical();
    //            {
    //                for (int i = 0; i < count; i++)
    //                {
    //                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
    //                    {
    //                        EditorGUILayout.BeginHorizontal();
    //                        {
    //                            EditorGUILayout.LabelField("Button Data", GUILayout.Width(75));
    //                            scriptable.Button = (IS_ButtonData)EditorGUILayout.ObjectField(scriptable.Button, typeof(IS_ButtonData));
    //                        }
    //                        EditorGUILayout.EndHorizontal();
    //                    }
    //                    EditorGUILayout.EndHorizontal();
    //                }
    //            }
    //            EditorGUILayout.EndVertical();
    //            break;
    //        case ContentType.video:
    //            scriptable.MovieName = EditorGUILayout.TextField("VideoName", scriptable.MovieName);
    //            break;
    //        case ContentType.record:
    //            //굵은 선을 긋습니다.
    //            EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb, GUILayout.ExpandWidth(true), GUILayout.Height(1));
    //            EditorGUILayout.EndHorizontal();
    //            //굵은 선을 긋습니다.
    //            break;
    //        case ContentType.None:
    //            //굵은 선을 긋습니다.
    //            EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb, GUILayout.ExpandWidth(true), GUILayout.Height(1));
    //            EditorGUILayout.EndHorizontal();
    //            //굵은 선을 긋습니다.
    //            break;
    //    }
    //}
    //EditorGUILayout.EndHorizontal();

    //// -------------------------------------------------------------------------------------------------------------
    //EditorGUILayout.Space();

    //EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
    //{
    //    scriptable.contentType5 = (ContentType)EditorGUILayout.EnumPopup("콘텐츠 5", scriptable.contentType5);
    //}
    //EditorGUILayout.EndHorizontal();

    //EditorGUILayout.BeginHorizontal(GUI.skin.box);
    //{
    //    switch (contentType5 = scriptable.contentType5)
    //    {
    //        case ContentType.animation:
    //            scriptable.AnimationName = EditorGUILayout.TextField("AnimName", scriptable.AnimationName);
    //            break;
    //        case ContentType.buttonGroup:
    //            EditorGUILayout.BeginVertical();
    //            {
    //                EditorGUILayout.LabelField("buttons", EditorStyles.whiteLabel, GUILayout.Width(50));
    //                EditorGUILayout.BeginHorizontal();
    //                {
    //                    count = EditorGUILayout.IntField(count, GUILayout.Width(50));
    //                }
    //                EditorGUILayout.EndHorizontal();
    //            }
    //            EditorGUILayout.EndVertical();

    //            EditorGUILayout.BeginVertical();
    //            {
    //                for (int i = 0; i < count; i++)
    //                {
    //                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
    //                    {
    //                        EditorGUILayout.BeginHorizontal();
    //                        {
    //                            EditorGUILayout.LabelField("Button Data", GUILayout.Width(75));
    //                            scriptable.Button = (IS_ButtonData)EditorGUILayout.ObjectField(scriptable.Button, typeof(IS_ButtonData));
    //                        }
    //                        EditorGUILayout.EndHorizontal();
    //                    }
    //                    EditorGUILayout.EndHorizontal();
    //                }
    //            }
    //            EditorGUILayout.EndVertical();
    //            break;
    //        case ContentType.video:
    //            scriptable.MovieName = EditorGUILayout.TextField("VideoName", scriptable.MovieName);
    //            break;
    //        case ContentType.record:
    //            //굵은 선을 긋습니다.
    //            EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb, GUILayout.ExpandWidth(true), GUILayout.Height(1));
    //            EditorGUILayout.EndHorizontal();
    //            //굵은 선을 긋습니다.
    //            break;
    //        case ContentType.None:
    //            //굵은 선을 긋습니다.
    //            EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb, GUILayout.ExpandWidth(true), GUILayout.Height(1));
    //            EditorGUILayout.EndHorizontal();
    //            //굵은 선을 긋습니다.
    //            break;
    //    }
    //}
    //EditorGUILayout.EndHorizontal();

    //// -------------------------------------------------------------------------------------------------------------
    //EditorGUILayout.Space();
    #endregion
}