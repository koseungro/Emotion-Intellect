using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ResultData
{
    public examResult examResult;
}

[Serializable]
public class examResult
{
    public int user;
    public string service;
    public string code;
    public string tested;
    public string json;
    public string seq;
}

public static class Global {

    public static string startTime1 = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm");
    public static string startTime = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm");
    public static List<string> selectData = new List<string>();
    public static List<string> recPath = new List<string>();

    public static string NowToString()
    {
        return System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm");
    }
}