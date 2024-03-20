using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using FNIUnityEngine.hjchae;

//public static class GetUserInfo {

//    public static string userIndex;
//    public static string userEmail;
//    public static string serviceSeq;
//    public static string code;
//    public static string userSeqSigned;
//    public static string severPath;
//    public static string examResult;
//    public static string soundPath;

//    public static void GetIniInfo()
//    {
//#if UNITY_EDITOR
//        fniINIFile resultFile = new fniINIFile(string.Format("{0}/../../curSetup.ini", Application.dataPath));
//#else
//        fniINIFile resultFile = new fniINIFile(string.Format("{0}/../../../curSetup.ini", Application.dataPath));
//#endif

//        resultFile.Read();

//        userIndex = resultFile.GetValue("UserInfo", "userSeq", "");
//        userSeqSigned = resultFile.GetValue("UserInfo", "userSeqSigned", "");
//        userEmail = resultFile.GetValue("UserInfo", "userEmail", "");
//        serviceSeq = resultFile.GetValue("UserInfo", "serviceSeq", "");
//        code = resultFile.GetValue("UserInfo", "code", "");
//        severPath = resultFile.GetValue("UserInfo", "severPath", "");
//        examResult = resultFile.GetValue("UserInfo", "examResult", "");
//        soundPath = resultFile.GetValue("UserInfo", "soundPath", "");

//        Debug.Log(severPath + " / " + soundPath + " / " + examResult);
//    }
//}

public static class GetUserInfo
{

    /// <summary>
    /// 회원여부::
    /// 회원==0, 손님==1
    /// </summary>
    public static int isLoginByGuest;
    /// <summary>
    /// 유저 아이디
    /// </summary>
    public static string id;
    /// <summary>
    /// 유저 성별
    /// </summary>
    public static string gender;
    /// <summary>
    /// 유저 나이
    /// </summary>
    public static int age;
    /// <summary>
    /// 감정선택카드 1
    /// </summary>
    public static string Ecard1;
    /// <summary>
    /// 감정선택카드 2
    /// </summary>
    public static string Ecard2;
    /// <summary>
    /// 감정선택카드 3
    /// </summary>
    public static string Ecard3;
    /// <summary>
    /// 훈련 구분코드
    /// </summary>
    public static string contentcode;

    /// <summary>
    /// 사용자 세션넘버
    /// </summary>
    public static int sessNum;

    /// <summary>
    /// 스트레스사건들 몇번째 보는건지
    /// </summary>
    public static int nth;

    /// <summary>
    /// 유니티 몇번 째 실행인지 1, 2
    /// </summary>
    public static int unityNum;

    /// <summary>
    /// 검사 시작 시간
    /// </summary>
    public static string start_time;
    /// <summary>
    /// 검사 종료 시간
    /// </summary>
    public static string finish_time;
    /// <summary>
    /// 선택한 카테고리
    /// </summary>
    public static string category1;
    /// <summary>
    /// 훈련 JSON string
    /// </summary>
    public static string result;

    public static string recName;
    //public static string Index;
    //public static string Email;
    //public static string serviceSeq;
    //public static string code;
    //public static string SeqSigned;
    //public static string severPath;
    //public static string examResult;
    //public static string soundPath;

    public static void GetIniInfo()
    {
        Debug.Log(string.Format("{0}", Application.dataPath));
#if UNITY_EDITOR
        fniINIFile resultFile = new fniINIFile(string.Format("{0}/../../curSetup.ini", Application.dataPath));
#else
        fniINIFile resultFile = new fniINIFile(string.Format("{0}/../../../curSetup.ini", Application.dataPath));
#endif
        resultFile.Read();
        //resultFile.GetValue("UserInfo", "SiteSeq", "11");
        //Index = resultFile.GetValue("UserInfo", "Seq", "");

        //SeqSigned = resultFile.GetValue("UserInfo", "SeqSigned", "");

        //siteSeq = resultFile.GetValue("UserInfo", "siteSeq", 0);
        isLoginByGuest = resultFile.GetValue("UserInfo", "isLoginByGuest", 0);
        id             = resultFile.GetValue("UserInfo", "id", "");
        gender         = resultFile.GetValue("UserInfo", "gender", "");
        age            = resultFile.GetValue("UserInfo", "age", 0);
        contentcode    = resultFile.GetValue("UserInfo", "contentcode", "");
        Ecard1         = resultFile.GetValue("UserInfo", "Ecard1", "");
        Ecard2         = resultFile.GetValue("UserInfo", "Ecard2", "");
        Ecard3         = resultFile.GetValue("UserInfo", "Ecard3", "");
        sessNum        = resultFile.GetValue("UserInfo", "sessNum", 0);
        nth            = resultFile.GetValue("UserInfo", "nth", 0);
        unityNum       = resultFile.GetValue("UserInfo", "unityNum", 0);
        start_time     = resultFile.GetValue("UserInfo", "start_time", "");
        finish_time    = resultFile.GetValue("UserInfo", "finish_time", "");
        //service = resultFile.GetValue("UserInfo", "service", "");
        //platform = resultFile.GetValue("UserInfo", "platform", "");
        //traininglevel = resultFile.GetValue("UserInfo", "traininglevel", "");

        //preCheck = resultFile.GetValue("UserInfo", "preCheck", "");


        //Email = resultFile.GetValue("UserInfo", "Email", "");
        //serviceSeq = resultFile.GetValue("UserInfo", "serviceSeq", "");
        //code = resultFile.GetValue("UserInfo", "code", "");
        //severPath = resultFile.GetValue("UserInfo", "severPath", "");
        //examResult = resultFile.GetValue("UserInfo", "examResult", "");
        //soundPath = resultFile.GetValue("UserInfo", "soundPath", "");

        //Debug.Log(Index + " / " + Email + " / " + code);
    }

    public static string DataToString()
    {
        return string.Format(
            "isLoginByGuest : {1}{16}," +
            "id : {2}{16}," +
            "gender : {3}{16}," +
            "age : {4}{16}," +
            "Ecard1 : {8}{16}," +
            "Ecard2 : {9}{16}," +
            "Ecard3 : {10}{16}," +
            "contentcode : {11}{16}," +
            "start_time : {13}{16}," +
            "finish_time : {14}{16}," +
            "result : {15}{16}",
            isLoginByGuest,
            id,
            gender,
            age,
            Ecard1,
            Ecard2,
            Ecard3,
            contentcode,
            start_time,
            finish_time,
            result,
            "\n"
            );
    }
}

public class UserInfoJsonObject
{
    /// <summary>
    /// 기관고유번호
    /// </summary>
    //public int siteSeq;
    /// <summary>
    /// 회원여부::
    /// 회원==0, 손님==1
    /// </summary>
    public int isLoginByGuest;
    /// <summary>
    /// 유저 아이디
    /// </summary>
    public string id;
    /// <summary>
    /// 유저 성별
    /// </summary>
    public string gender;
    /// <summary>
    /// 유저 나이
    /// </summary>
    public int age;
    /// <summary>
    /// 프로그램 유형::
    /// 시니엔케어, 마인즈케어, 디톡스, ETC
    /// </summary>
    //public string service;
    /// <summary>
    /// 선택된 플랫폼::
    /// Web, Oculus, Odyssey, GearVR, PicoVR, ETC
    /// </summary>
   // public string platform;
    /// <summary>
    /// 난이도
    /// </summary>
    //public string traininglevel;

    public string ecard1;
    public string ecard2;
    public string ecard3;
    /// <summary>
    /// 훈련 구분코드
    /// </summary>
    public string contentcode;
    /// <summary>
    /// VR 진행전 측정값
    /// </summary>
    //public string preCheck;
    /// <summary>
    /// 검사 시작 시간
    /// </summary>
    public string start_time;
    /// <summary>
    /// 검사 종료 시간
    /// </summary>
    public string finish_time;
    /// <summary>
    /// 세부 콘텐츠, 심리교육 콘텐츠
    /// </summary>
    public string category1;
    /// <summary>
    /// User세션번호
    /// </summary>
    public int sessNum;
    /// <summary>
    /// Unity 몇번 실행했는지
    /// </summary>
    public int unityNum;
    /// <summary>
    /// 녹음 파일 이름
    /// </summary>
    public string recName;
    /// <summary>
    /// 훈련 JSON string
    /// </summary>    
    //public RawData[] result;


    public UserInfoJsonObject(
                            int _isLoginByGuest,
                            string _id,
                            string _gender,
                            int _age,
                            string _Ecard1,
                            string _Ecard2,
                            string _Ecard3,
                            string _contentcode,
                            string _category1,
                            int _sessNum,
                            string _recName,
                            string _start_time,
                            string _finish_time)
    {
        isLoginByGuest = _isLoginByGuest;
        id = _id;
        gender = _gender;
        age = _age;
        ecard1 = _Ecard1;
        ecard2 = _Ecard2;
        ecard3 = _Ecard3;
        contentcode = _contentcode;
        category1 = _category1;
        sessNum = _sessNum;
        recName = _recName;
        start_time = _start_time;
        finish_time = _finish_time;
    }
}


public static class ServerPath
{
    public static string serverPath;
    public static string examResultPath;
    public static string soundPath;
    public static string sendDataPath;

    public static void GetIniInfo()
    {
        Debug.Log(string.Format("{0}", Application.dataPath));
#if UNITY_EDITOR
        fniINIFile resultFile = new fniINIFile(string.Format("{0}/../../curSetup.ini", Application.dataPath));
#else
        fniINIFile resultFile = new fniINIFile(string.Format("{0}/../../../curSetup.ini", Application.dataPath));
#endif
        resultFile.Read();

        //Email = resultFile.GetValue("UserInfo", "Email", "");
        //serviceSeq = resultFile.GetValue("UserInfo", "serviceSeq", "");
        //code = resultFile.GetValue("UserInfo", "code", "");

        //serverPath = resultFile.GetValue("Path", "serverPath", "");
        //examResultPath = resultFile.GetValue("Path", "examResult", "");
        //soundPath = resultFile.GetValue("Path", "soundPath", "");
        //sendDataPath = resultFile.GetValue("Path", "sendDataPath", "");

        //Debug.Log(Index + " / " + Email + " / " + code);

        #region 2020.01.17 백인성
        //serverPath = resultFile.GetValue("UserInfo", "serverPath", "");
        //examResultPath = resultFile.GetValue("UserInfo", "examResult", "");
        //soundPath = resultFile.GetValue("UserInfo", "soundPath", "");
        //sendDataPath = resultFile.GetValue("UserInfo", "sendDataPath", "");

        //SectionName을 "UserInfo" -> "Path"로 변경
        serverPath = resultFile.GetValue("UserInfo", "serverPath", "");
        examResultPath = resultFile.GetValue("UserInfo", "examResult", "");
        //경로를 불러오지 못하면 스트리밍 에셋 폴더로 경로를 지정하도록 수정함
        soundPath = resultFile.GetValue("UserInfo", "soundPath", Path.Combine(Application.streamingAssetsPath, "soundPath"));
        sendDataPath = resultFile.GetValue("UserInfo", "sendDataPath", "");
        #endregion

    }

    public static string DataToString()
    {
        return string.Format
        (
            "severPath : {0}{3}" +
            "examResultPath : {1}{3}" +
            "soundPath : {2}{3}",
            serverPath,
            examResultPath,
            soundPath,
            "\n"
        );
    }
}
