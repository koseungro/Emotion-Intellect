using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.IO.Pipes;
using System.Security.Principal;
using System.Text;
using UnityEngine.UI;
using Unity.Collections.LowLevel.Unsafe;
using FNI;

//public class Answer
//{

//    public string startTime;
//    public string endTime;
//    public List<string> recordData = new List<string>();
//    public List<string> selectData = new List<string>();

//    public Answer()
//    {
//        startTime = Global.startTime1;
//        endTime = Global.NowToString();
//    }

//    public IEnumerator SendAnswerData()
//    {
//        endTime = Global.NowToString();

//        recordData = Global.recPath;
//        selectData = Global.selectData;

//        Debug.Log("here");
//        string trData = JsonUtility.ToJson(this, prettyPrint: true);

//        Debug.Log(trData);
//        DirectoryInfo di = new DirectoryInfo(Application.streamingAssetsPath + "\\ResultData");
//        if (di.Exists == false)
//        {
//            di.Create();
//        }

//        FileInfo file = new FileInfo(di + "\\" + GetUserInfo.userEmail + "-" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt");
//        if (!file.Exists)
//        {
//            FileStream fs = file.Create();
//            TextWriter tw = new StreamWriter(fs);
//            tw.Write(trData);
//            tw.Close();
//            fs.Close();
//        }
//        Debug.Log("severPath : " + GetUserInfo.severPath); 

//        if (GetUserInfo.severPath == null || GetUserInfo.severPath == "")
//        {
//            Debug.Log("No Sever!!");
//            yield return null;
//        }
//        else
//        {
//            int userIndex = int.Parse(GetUserInfo.userIndex);

//            WWWForm form1 = new WWWForm();
//            form1.AddField("signedSeq", GetUserInfo.userSeqSigned);
//            form1.AddField("serviceSeq", GetUserInfo.serviceSeq);
//            form1.AddField("code", GetUserInfo.code);
//            form1.AddField("tested", Global.startTime1);
//            form1.AddField("json", "{\"Result\":\"검사 종료\"}");

//            WWW www1 = new WWW(GetUserInfo.examResult, form1.data);

//            yield return www1;

//            Debug.Log(www1.text);

//            var obj = JsonUtility.FromJson<ResultData>(www1.text);

//            WWWForm form = new WWWForm();

//            form.AddField("UserIndex", userIndex);
//            form.AddField("UserEmail", GetUserInfo.userEmail);
//            form.AddField("trData", trData);
//            form.AddField("seq", obj.examResult.seq);

//            WWW www = new WWW(GetUserInfo.severPath + PlayerPrefs.GetString("folderName"), form);

//            yield return www;

//            // check for errors 
//            if (www.error == null)
//            {
//                Debug.Log("WWW Ok!: " + www.text);
//                NamedPipeClientStream();
//            }
//            else
//            {
//                Debug.Log("WWW Error: " + www.error);
//            }
//        }
//        yield return null;
//    }

//    public void NamedPipeClientStream()
//    {
//#if !UNITY_EDITOR
//        NamedPipeClientStream clientStream = new NamedPipeClientStream("SeniorPipe");
//        //clientStream.Connect();
//        clientStream.Connect();
//        if (clientStream.IsConnected)
//        {
//            //Debug.Log("connected");
//            //Debug.Log(str.Length);

//            clientStream.WaitForPipeDrain();
//            clientStream.WriteByte(0xFE);

//            clientStream.Flush();
//            clientStream.Close();
//        }
//#endif
//    }

//}

[Serializable]
public class RawData
{
    public string startTime = "";
    public string endTime = "";
    public List<string> recordData = new List<string>();
}

public class Answer
{
    public string startTime;
    public string endTime;
    public List<string> recordData = new List<string>();
    public string recName;

    public Answer()
    {
        startTime = Global.startTime1;
        endTime = Global.NowToString();
        //category1 = "test";
    }

    public IEnumerator SendAnswerData()
    {
        Debug.Log("SendAnswerData");

        //------1. 종료 파일 기록 
        //NamedPipeClientStream();
        if (MainManager.Instance.isStart == true)
        {            
            NamedPipeClientStream();
            //--------2. 데이타 전달 
            // 2019.07.23 
            // 수정된 Json 포맷을 맞추기 위한 Info Load
            //GetUserInfo.GetIniInfo();

            // 저장하기 위한 경로 Info Load
            // Local Server, Kiosk Server        
            //ServerPath.GetIniInfo();

            endTime = Global.NowToString();

            recordData = Global.recPath;
            //selectData = Global.selectData;



            Debug.Log("here");
            string trData = JsonUtility.ToJson(this, prettyPrint: true);

            Debug.Log(trData);
            DirectoryInfo di = new DirectoryInfo(Application.streamingAssetsPath + "\\ResultData");
            if (di.Exists == false)
            {
                di.Create();
            }

            FileInfo file = new FileInfo(di + "\\" + GetUserInfo.id + "-" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt");
            if (!file.Exists)
            {
                FileStream fs = file.Create();
                TextWriter tw = new StreamWriter(fs);
                tw.Write(trData);
                tw.Close();
                fs.Close();
            }
            //Debug.Log("severPath : " + GetUserInfo.severPath);

            if (ServerPath.sendDataPath == null || ServerPath.sendDataPath == "")
            {
                Debug.Log("No Sever!!");
                //yield return null;
            }
            else
            {
                //int userIndex = int.Parse(GetUserInfo.userIndex);

                WWWForm form1 = new WWWForm();

                //form1.AddField("signedSeq", GetUserInfo.userSeqSigned);
                //form1.AddField("serviceSeq", GetUserInfo.serviceSeq);
                //form1.AddField("code", GetUserInfo.code);
                //form1.AddField("tested", Global.startTime1);
                //form1.AddField("json", "{\"Result\":\"검사 종료\"}");

                // ========================================================================================
                // 2019.07.23 
                // Json Format  수정
                // Kiosk 쪽으로 날리는 WWW

                RawData rawData = new RawData();
                rawData.startTime = startTime;
                rawData.endTime = endTime;
                rawData.recordData = recordData;

                //rawData.selectData = selectData;

                UserInfoJsonObject userInfoJsonString = new UserInfoJsonObject
                (
                    GetUserInfo.isLoginByGuest,
                    GetUserInfo.id,
                    GetUserInfo.gender,
                    GetUserInfo.age,
                    GetUserInfo.Ecard1,
                    GetUserInfo.Ecard2,
                    GetUserInfo.Ecard3,
                    GetUserInfo.contentcode,
                    GetUserInfo.category1,
                    GetUserInfo.sessNum,
                    GetUserInfo.recName,
                    startTime,
                    endTime
                );


                string UserInfoDataJsonString = JsonUtility.ToJson(userInfoJsonString, prettyPrint: true);

                form1.AddField("json", UserInfoDataJsonString);
                Debug.Log("UserInfoDataJsonString : " + UserInfoDataJsonString);

                Debug.Log("json : " + trData);
                Debug.Log("severPath : " + ServerPath.sendDataPath);





                WWW www1 = new WWW(ServerPath.sendDataPath, form1.data);
                yield return www1;


                if (www1.error == null)
                {
                    Debug.Log("WWW Ok!: " + www1.text);
                    //NamedPipeClientStream();

                    if (www1.text == "OK")
                    {
                        MainManager.Instance.isEnd = true;
                        //Application.Quit();
                    }
                    MainManager.Instance.isEnd = true;
                    //Application.Quit();
                    //if (www1.text != "OK")
                    //{
                    //    string filePath = Application.persistentDataPath;
                    //    File.WriteAllText(filePath, UserInfoDataJsonString);
                    //    Debug.Log("로컬 저장");
                    //    Application.Quit();
                    //}
                }
                else
                {
                    Debug.Log("WWW Error: " + www1.error);
                    int cnt = 0;
                    while (cnt < 1)
                    {
                        yield return www1;
                        cnt++;
                    }
                    // 한번 더 전송
                    Debug.Log("한번 더2");
                    if (www1.error == null)
                    {
                        if (www1.text == "OK")
                        {
                            MainManager.Instance.isEnd = true;
                            //Application.Quit();
                        }
                    }
                    else
                    {
                        string path = Application.dataPath + "/playerData.json";
                        File.WriteAllText(path, UserInfoDataJsonString);
                        Debug.Log("로컬 저장");
                        MainManager.Instance.isEnd = true;
                        //Application.Quit();
                    }
                }
            }
        }
        else
        {
            NamedPipeClientStream();
            yield return new WaitForSeconds(1f);
            MainManager.Instance.isEnd = true;
            //Application.Quit();
        }
        
        yield return null;
    }

    public void NamedPipeClientStream()
    {        
#if !UNITY_EDITOR
        NamedPipeClientStream clientStream = new NamedPipeClientStream("SeniorPipe");
        //clientStream.Connect();
        clientStream.Connect();
        if (clientStream.IsConnected)
        {
            //Debug.Log("connected");
            //Debug.Log(str.Length);

            clientStream.WaitForPipeDrain();
            clientStream.WriteByte(0xFE);

            clientStream.Flush();
            clientStream.Close();
        }
#endif
    }

}
