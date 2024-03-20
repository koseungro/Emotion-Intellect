/// 작성자: 조효련
/// 작성일: 2018-11-01
/// 수정일: 2020-01-23
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력
/// (2020-01-23)
/// 1. 쓰지않는 기능 삭제
/// 2. Scenes폴더의 Scene파일 생성 및 자동 새로고침 기능 추가
/// 3. Obj목록 및 생성 버튼 추가


using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using UnityToolbarExtender;
using UnityToolbarExtender.Examples;
using System;
using UnityEngine.SceneManagement;
using System.IO;

namespace FNI
{
    /// <summary>
    /// (참고)
    /// GUIStyle 종류
    /// Font , Box , Button , Toggle , Label , Text Field , Text Area , Window , 
    /// Vertical Slider   , Vertical Slider Thumb   , Vertical Scrollbar   , Vertical Scrollbar Thumb  , Vertical Scrollbar Up Button   , Vertical Scrollbar Down Button
    /// Horizontal Slider , Horizontal Slider Thumb ,Horizontal Scrollbar , Horizontal Scrollbar Thumb , Horizontal Scrollbar Left Button , Horizontal Scrollbar Right Button
    /// Scroll View
    /// Custom Styles
    /// Settings
    /// </summary>

    static class ToolbarStyles
    {
        public static readonly GUIStyle commandButtonStyle;
        
        static ToolbarStyles()
        {
            commandButtonStyle = new GUIStyle("Button")
            {
                fontSize = 13,
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Normal,
                imagePosition = ImagePosition.ImageAbove,
            };
        }
    }

    [InitializeOnLoad]
    public class FNIEditorStartup
    {    
        static FNIEditorStartup()
        {
            Debug.Log("StartUp 실행");

            ToolbarExtender.LeftToolbarGUI.Add(OnLeftToolbarGUI);
            ToolbarExtender.RightToolbarGUI.Add(OnRightToolbarGUI);

            // 신 리스트를 생성
            CreateSceneList();

            #region 사용하지 않음
            // 에디터가 시작할 떄 실행할 일을 구현
            // 프로젝트 설정 수정이라든지...
            // 일반 프로젝트에선 사용하지 않습니다.
            //EditorSceneManager.sceneOpened += OnEditorSceneManagerSceneOpened;

            //EditorSceneManager.newSceneCreated += OnEditorSceneCreated;
            #endregion
        }

        #region 씬 목록 생성 및 이동

        private static int selectedScene = 0;
        private static int[] sceneList;
        private static string[] sceneNameList;
        private static string[] scenePathList;
        public static bool m_enabled;

        /// <summary>
        /// FNI/Scenes 폴더안의 Scenes 파일을 읽어와서 리스트를 생성해줍니다.
        /// </summary>
        public static void CreateSceneList()
        {
            // 씬들의 정보를 저장해줄 List
            List<int> FNIsceneIndexList = new List<int>();
            List<string> FNIsceneNameList = new List<string>();
            List<string> FNIscenePathList = new List<string>();

            DirectoryInfo dirInfo;

            dirInfo = new DirectoryInfo(Path.Combine(Application.dataPath, "FNI"));

            int count = 0;

            // 해당 경로에 있는 씬 파일들을 전부 읽어와서 List에 각각 인덱스, 이름, 경로를 추가해줍니다.
            foreach (DirectoryInfo directory in dirInfo.GetDirectories())
            {
                foreach (FileInfo file2 in directory.GetFiles("*.unity"))
                {
                    string name = file2.Name;
                    string path = string.Format("Assets/FNI/{0}/{1}", directory.Name, name);

                    FNIsceneIndexList.Add(count);
                    FNIsceneNameList.Add(name);
                    FNIscenePathList.Add(path);

                    count += 1;
                }
            }

            sceneList = FNIsceneIndexList.ToArray();
            sceneNameList = FNIsceneNameList.ToArray();
            scenePathList = FNIscenePathList.ToArray();
        }

        // 재생버튼이 존재하는 툴바에 버튼 추가하기
        static void OnLeftToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            #region 임시 보관
            //새로고침을 안해도 자동으로 적용되도록 수정
            //if (GUILayout.Button(new GUIContent(" Scenes폴더 새로고침 ", "Scenes폴더 내부 파일에 변경이 있을 때 새로고침 합니다."), ToolbarStyles.commandButtonStyle, GUILayout.Height(22)))
            //{
            //    CreateSceneList();
            //}
            #endregion

            if (sceneList != null)
            {
                // Assets\FNI\Scenes폴더의 신 가져오기   
                selectedScene = EditorGUILayout.IntPopup(selectedScene, sceneNameList, sceneList, ToolbarStyles.commandButtonStyle, GUILayout.Height(22), GUILayout.Width(175));

                // 씬이 추가되면 이 함수가 실행되면서 자동으로 갱신됩니다.
                CreateSceneList();

                // 씬 이동 버튼 생성
                if (GUILayout.Button(new GUIContent(" Scene 이동 ", "선택된 신으로 이동하기"), ToolbarStyles.commandButtonStyle, GUILayout.Height(22)))
                {
                    SceneHelper.StartScene(scenePathList[selectedScene]);
                }
            }
            GUILayout.Space(100);

            #region 사용하지 않음
            //해당기능 현재 불필요
            //if (GUILayout.Button(new GUIContent(" Main ", "Main Scene을 열어줍니다."), ToolbarStyles.commandButtonStyle, GUILayout.Height(22)))
            //{
            //    //SceneHelper.StartScene("Assets/FNI/Scenes/Main.unity");
            //}

            //현재 불필요
            //if (GUILayout.Button(new GUIContent("AnimationPreset" ,"애니메이션 프리셋 신 시작하기") ,ToolbarStyles.commandButtonStyle ,GUILayout.Height(22)))
            //{
            //    SceneHelper.StartScene("Assets/AnimationPreset.unity");
            //}

            //속도 문제로 제거
            //CreateSceneList();

            //씬이 너무 많아서 나눠서 보이도록 드롭다운 메뉴 추가             
            //ShowContentDropdown();
            #endregion
        }

        #endregion

        #region 오브젝트 생성 및 CreateEmpty
        private static int selectedObj = 0;
        // ObjNameList와 숫자를 맞춰주시면 됩니다.
        private static int[] ObjList = new int[] { 0, 1, 2, 3, 4, 5 };
        private static string[] ObjNameList = new string[] { "Cube", "Sphere", "Capsule", "Cylinder", "Plane", "Quad" };
        private static string ObjPath = "GameObject/3D Object";

        // 재생버튼이 존재하는 툴바에 버튼 추가하기
        static void OnRightToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            if (ObjList != null)
            {
                // CreateEmpty 생성 버튼
                if (GUILayout.Button(new GUIContent(" Create Empty "), ToolbarStyles.commandButtonStyle, GUILayout.Height(22)))
                {
                    EditorApplication.ExecuteMenuItem("GameObject/Create Empty");
                }


                // 여백
                GUILayout.Space(20);

                // 오브젝트 리스트 버튼 생성 방식 두 가지
                #region 오브젝트 리스트 생성 방식
                selectedObj = EditorGUILayout.IntPopup(selectedObj, ObjNameList, ObjList, ToolbarStyles.commandButtonStyle, GUILayout.Height(22), GUILayout.Width(100));

                if (GUILayout.Button(new GUIContent(" 생성 ", "선택된 오브젝트 생성"), ToolbarStyles.commandButtonStyle, GUILayout.Height(22)))
                {
                    EditorApplication.ExecuteMenuItem(string.Format("GameObject/3D Object/{0}", ObjNameList[selectedObj]));
                }
                #endregion


                #region 오브젝트 개별 버튼 생성 방식
                //if (GUILayout.Button(new GUIContent(" Cube "), ToolbarStyles.commandButtonStyle, GUILayout.Height(22)))
                //{
                //    //SceneHelper.StartScene(scenePathList[selectedScene]);
                //    EditorApplication.ExecuteMenuItem(ObjPath+"/Cube");
                //    Debug.Log("Cube 생성");
                //}
                //if (GUILayout.Button(new GUIContent(" Sphere "), ToolbarStyles.commandButtonStyle, GUILayout.Height(22)))
                //{
                //    //SceneHelper.StartScene(scenePathList[selectedScene]);
                //    EditorApplication.ExecuteMenuItem(ObjPath+"/Sphere");
                //    Debug.Log("Sphere 생성");
                //}
                //if (GUILayout.Button(new GUIContent(" Capsule "), ToolbarStyles.commandButtonStyle, GUILayout.Height(22)))
                //{
                //    //SceneHelper.StartScene(scenePathList[selectedScene]);
                //    EditorApplication.ExecuteMenuItem(ObjPath+"/Capsule");
                //    Debug.Log("Capsule 생성");
                //}
                //if (GUILayout.Button(new GUIContent(" Cylinder "), ToolbarStyles.commandButtonStyle, GUILayout.Height(22)))
                //{
                //    //SceneHelper.StartScene(scenePathList[selectedScene]);
                //    EditorApplication.ExecuteMenuItem(ObjPath + "/Cylinder");
                //    Debug.Log("Cylinder 생성");
                //}
                //if (GUILayout.Button(new GUIContent(" Plane "), ToolbarStyles.commandButtonStyle, GUILayout.Height(22)))
                //{
                //    //SceneHelper.StartScene(scenePathList[selectedScene]);
                //    EditorApplication.ExecuteMenuItem(ObjPath + "/Plane");
                //    Debug.Log("Plane 생성");
                //}
                //if (GUILayout.Button(new GUIContent(" Quad "), ToolbarStyles.commandButtonStyle, GUILayout.Height(22)))
                //{
                //    //SceneHelper.StartScene(scenePathList[selectedScene]);
                //    EditorApplication.ExecuteMenuItem(ObjPath + "/Quad");
                //    Debug.Log("Quad 생성");
                //}
                #endregion

            }
            GUILayout.Space(200);
        }
        #endregion

        #region 사용하지 않는 코드 임시보관


        private static void OnEditorSceneCreated(Scene scene, NewSceneSetup setup, NewSceneMode mode)
        {
            CreateSceneList();
        }

        /// <summary>
        /// 현재는 사용하지 않습니다.
        /// </summary>
        //static void OnRightToolbarGUI()
        //{
        //    GUILayout.Space(20);

        //    GUI.changed = false;

        //    GUI.color = Color.white;


        //    if (GUI.changed)
        //    {
        //        //if (Global.Setting != null)
        //        //    Global.Setting.AnimationSpeed.Current = AnimationSpeeds[selectedAnimationSpeed-1];

        //        //Debug.Log("Speed : " + Global.Setting.AnimationSpeed.Current + " : " + selectedAnimationSpeed );
        //    }

        //    //if (GUILayout.Button(new GUIContent(" 1X ", "애니 속도"), ToolbarStyles.commandButtonStyle, GUILayout.Width(50), GUILayout.Height(22)))
        //    //{
        //    //    if (Global.Setting != null)
        //    //        Global.Setting.AnimationSpeed.Current = 1;
        //    //}

        //    //if (GUILayout.Button(new GUIContent(" 10X ", "애니 속도"), ToolbarStyles.commandButtonStyle, GUILayout.Width(50), GUILayout.Height(22)))
        //    //{
        //    //    if (Global.Setting != null)
        //    //        Global.Setting.AnimationSpeed.Current = 10;
        //    //}

        //}

        /// <summary>
        /// Main신 외에는 현재 열려진 신과 Main신이 자동으로 BuildSettings에 추가되도록 하는 스크립트.
        /// 콘텐츠 신 테스트에 필요함.
        /// 현재는 사용하지 않습니다.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        static void OnEditorSceneManagerSceneOpened(UnityEngine.SceneManagement.Scene scene, UnityEditor.SceneManagement.OpenSceneMode mode)
        {
            List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();

            // Main, Test 이란 이름이 포함되지 않은 신 이거나 AnimationPreset이 아닌 경우 
            // 즉 콘텐츠 신인 경우
            if (scene.name.Contains("Main") == false &&
                scene.name.Contains("Test") == false &&
                scene.name.Contains("AnimationPreset") == false)
            {
                Debug.LogFormat("{0} 신을 열었습니다.", scene.name);

                // 신 추가하기
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(EditorSceneManager.GetActiveScene().path, true));
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene("Assets/Main.unity", true));

                EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();


                // 라이트 체크하기
                foreach (Light light in Resources.FindObjectsOfTypeAll(typeof(Light)) as Light[])
                {
                    if (light.hideFlags == HideFlags.NotEditable || light.hideFlags == HideFlags.HideAndDontSave)
                        continue;
                    if (!EditorUtility.IsPersistent(light.transform.root.gameObject))
                        continue;

                    if (light.type == LightType.Directional)
                    {
                        //EditorMainCamera comp = light.GetComponent<EditorMainCamera>();
                        //if (comp)
                        //{
                        //    light.gameObject.SetActive(true);
                        //    GameObject.Destroy(comp);

                        //    light.gameObject.GetOrAddCompoment<EditorDirectionalLight>();

                        //    Debug.LogFormat("{0} 오브젝트에 EditorMainCamera이 존재하여 삭제해였습니다.", light.name);
                        //}

                        //if (light.gameObject.tag != EditorDirectionalLight.Tag)
                        //{
                        //    Debug.LogWarning("Directional 라이트가 존재합니다. 삭제하시기 바랍니다.");
                        //    light.gameObject.SetActive(false);
                        //}
                    }
                }


                // 카메라 체크하기
                Camera[] allCameras = Camera.allCameras;
                foreach (Camera camera in allCameras)
                {
                    if (camera.gameObject.scene.name != "Main")
                    {
                        //AudioListener comp = camera.GetComponent<AudioListener>();
                        //if (comp)
                        //{
                        //    GameObject.DestroyImmediate(comp);
                        //}

                        //if (camera.gameObject.tag == "MainCamera")
                        //{
                        //    //Debug.LogWarning("메인 카메라가 존재합니다. 카메라가 필요할 경우 FNI/Global/Prefabs/Contents Camera 프리팹을 추가하시기 바랍니다.");
                        //    //camera.gameObject.SetActive(false);
                        //}
                    }
                }

                //Debug.LogFormat("{0} 카메라 및 라이트 오브젝트 확인을 완료했습니다.", scene.name);
                // EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

                //메뉴의 기능을 직접 실행
                EditorApplication.ExecuteMenuItem("File/Save");

                // EditorApplication.ExecuteMenuItem("File/Save Project");
            }
        }

        #endregion
    }
}
