using FNI;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class BackGroundChanger : MonoBehaviour
{
    #region Singleton
    private static BackGroundChanger _instance;
    public static BackGroundChanger Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BackGroundChanger>();
                if (_instance == null)
                    Debug.LogError("BackGroundChanger�� ã�� �� �����ϴ�. ");
            }
            return _instance;
        }
    }
    #endregion

   

    public GameObject[] Places;
    public GameObject mainCamare;

    public void OnOffObj(string value)
    {
        for (int cnt = 0; cnt < Places.Length; cnt++)
        {
            if (Places[cnt] != null)
            {
                if (Places[cnt].name.Contains(value))
                {
                    Places[cnt].SetActive(true);
                }
                else
                {
                    Places[cnt].SetActive(false);
                }
            }
        }
    }

    [Header("---------------Stage ����---------------")]
    public Material stageSkybox;
    public Light stageSunSource;
    public AmbientMode stageEnvironmentMode;
    public DefaultReflectionMode stageDefaultReflectionMode;
    [ColorUsageAttribute(true, true)]
    public Color stageSkyColor;
    [ColorUsageAttribute(true, true)]
    public Color stageEquatorColor;
    [ColorUsageAttribute(true, true)]
    public Color stageGroundColor;
    public PostProcessLayer stagepostProcessLayer;
    public float stageEnvironmentIntencity;
    

    public void StageSettingRender()
    {
        mainCamare.transform.localPosition = new Vector3(0f, 0.15f, -0.05f);
        OnOffObj("Stage");
        stagepostProcessLayer.enabled = true;
        enviroSkyRendering.enabled = false;
        enviroPostProcessing.enabled = false;
        crepuscular.enabled = false;
        RenderSettings.skybox = stageSkybox;
        RenderSettings.sun = stageSunSource;
        RenderSettings.ambientMode = stageEnvironmentMode;
        RenderSettings.defaultReflectionMode = stageDefaultReflectionMode;
        RenderSettings.ambientIntensity = stageEnvironmentIntencity;
        RenderSettings.ambientSkyColor = stageSkyColor;
        RenderSettings.ambientEquatorColor = stageEquatorColor;
        RenderSettings.ambientGroundColor = stageGroundColor;
        Debug.Log("StageSettingRender");
    }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("---------------Sea ����---------------")]
    // �ٴ� ��ī�̹ڽ��� �˾Ƽ� ��
    public Material seaSkybox;
    public Light seaSsunSource;
    public AmbientMode seaEnvironmentMode;
    public DefaultReflectionMode seaDefaultReflectionMode;
    [ColorUsageAttribute(true, true)]
    public Color seaSkyColor;
    public float seaEnvironmentIntencity;
    public EnviroSkyRendering enviroSkyRendering;
    public EnviroPostProcessing enviroPostProcessing;
    public Crepuscular crepuscular;

    public void SeaSettingRender()
    {
        mainCamare.transform.localPosition = new Vector3(1.75f, -10f, 0f);
        OnOffObj("Sea");
        RenderSettings.skybox = seaSkybox;
        //EnviroSky.instance.SetupSkybox();
        crepuscular.enabled = false;
        stagepostProcessLayer.enabled = true;
        enviroSkyRendering.enabled = false;
        enviroPostProcessing.enabled = false;
        RenderSettings.sun = seaSsunSource;
        RenderSettings.ambientMode = seaEnvironmentMode;
        RenderSettings.ambientIntensity = seaEnvironmentIntencity;
        RenderSettings.defaultReflectionMode = seaDefaultReflectionMode;
        RenderSettings.ambientSkyColor = seaSkyColor;

        Debug.Log("SeaSettingRender");
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("---------------Sky ����---------------")]
    public Material skySkybox;
    public Light skySunSource;
    public AmbientMode skyEnvironmentMode;
    public DefaultReflectionMode skyDefaultReflectionMode;
    public Cubemap skyCubemap;
    public float skyEnvironmentIntencity;
    public FogVolumeRenderer left;
    public FogVolumeRenderer right;
    //public FogVolumeRenderer center;

    public void SkySettingRender()
    {
        mainCamare.transform.localPosition = new Vector3(0f, 105f, 0f);
        //stagepostProcessLayer.enabled = false;
        enviroSkyRendering.enabled = false;
        enviroPostProcessing.enabled = false;
        crepuscular.enabled = false;
        RenderSettings.skybox = skySkybox;
        RenderSettings.sun = skySunSource;
        RenderSettings.ambientMode = skyEnvironmentMode;
        RenderSettings.ambientIntensity = skyEnvironmentIntencity;
        RenderSettings.defaultReflectionMode = skyDefaultReflectionMode;
        RenderSettings.customReflection = skyCubemap;
        OnOffObj("Sky");
        Places[2].transform.position = new Vector3(0f, -20f, 0f);
        Debug.Log("SkySettingRender");
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("---------------Space ����---------------")]
    public Material spaceSkybox;
    public Light spaceSunSource;
    public AmbientMode spaceEnvironmentMode;
    public DefaultReflectionMode spaceDefaultReflectionMode;
    public float spaceEnvironmentIntencity;

    public void SpaceSettingRender()
    {
        mainCamare.transform.localPosition = new Vector3(0f, 0.15f, -0.05f);
        OnOffObj("Space");
        stagepostProcessLayer.enabled = false;
        enviroSkyRendering.enabled = false;
        enviroPostProcessing.enabled = false;
        crepuscular.enabled = false;
        RenderSettings.skybox = spaceSkybox;
        RenderSettings.sun = null;
        RenderSettings.ambientMode = spaceEnvironmentMode;
        RenderSettings.defaultReflectionMode = spaceDefaultReflectionMode;
        RenderSettings.ambientIntensity = spaceEnvironmentIntencity;
        Debug.Log("SpaceSettingRender");
    }


    [Header("---------------default ����---------------")]
    public Material defaultSkybox;
    public Light defaultSunSource;
    public AmbientMode defaultEnvironmentMode;
    public DefaultReflectionMode defaulttReflectionMode;
    public float defaultEnvironmentIntencity;
    public Texture defaultBG;
    public GameObject uiBG;

    public void DefaultSettingRender(bool isRecord = false)
    {
        mainCamare.transform.localPosition = new Vector3(0f, 0.15f, -0.05f);
        OnOffObj("Video");
        stagepostProcessLayer.enabled = false;
        enviroSkyRendering.enabled = false;
        enviroPostProcessing.enabled = false;
        crepuscular.enabled = false;
        RenderSettings.skybox = defaultSkybox;
        RenderSettings.sun = null;
        RenderSettings.ambientMode = defaultEnvironmentMode;
        RenderSettings.defaultReflectionMode = defaulttReflectionMode;
        RenderSettings.ambientIntensity = defaultEnvironmentIntencity;
        uiBG.SetActive(true);
        uiBG.GetComponent<MeshRenderer>().material.mainTexture = defaultBG;
        uiBG.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        Debug.Log("DefaultSettingRender");
    }


    [Header("---------------Self Practice ����---------------")]
    public Material practiceSkybox;
    public Light practiceSunSource;
    public AmbientMode practiceEnvironmentMode;
    public DefaultReflectionMode practiceReflectionMode;
    public float practiceEnvironmentIntencity;

    public void SelfPracticeSettingRender()
    {
        //OnOffObj("Video");
        mainCamare.transform.localPosition = new Vector3(0f, 0.15f, -0.05f);
        stagepostProcessLayer.enabled = false;
        enviroSkyRendering.enabled = false;
        enviroPostProcessing.enabled = false;
        crepuscular.enabled = false;
        RenderSettings.skybox = practiceSkybox;
        RenderSettings.sun = null;
        RenderSettings.ambientMode = practiceEnvironmentMode;
        RenderSettings.defaultReflectionMode = practiceReflectionMode;
        RenderSettings.ambientIntensity = practiceEnvironmentIntencity;
        Debug.Log("DefaultSettingRender");
    }


    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        OnOffObj("Stage");
    //        StageSettingRender();
    //    }
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        OnOffObj("Sea");
    //        SeaSettingRender();
    //        //SceneManager.LoadScene("SeaScene", LoadSceneMode.Additive);
    //    }
    //    if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        OnOffObj("Sky");
    //        SkySettingRender();
    //    }
    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        OnOffObj("Space");
    //        SpaceSettingRender();
    //    }
    //}

    void Start()
    {
        if (Places[2] != null)
        {
            Places[2].SetActive(false);
        }
        //RenderSettings.skybox = skybox;
        //RenderSettings.sun = sunSource;
        //RenderSettings.ambientMode = ;
        //RenderSettings.ambientIntensity = environmentIntencity;
        //RenderSettings.ambientSkyColor = stageGroundColor;
        //RenderSettings.ambientEquatorColor = stageGroundColor;
        //RenderSettings.ambientGroundColor = stageGroundColor;
        //RenderSettings.fog = fog;
        //RenderSettings.fogColor = fogColor;
        //RenderSettings.defaultReflectionMode = ReflectionMode;
        //RenderSettings.customReflection = ReflectionCubemap;
    }



}



// ���� �ڵ�
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Rendering;

//public class SkyboxAutoChanger : MonoBehaviour
//{
//    public Material skybox;
//    public Light sunSource;
//    public AmbientMode environmentMode;
//    public float environmentIntencity;
//    public Color environmentColor;
//    public bool fog;
//    public Color fogColor;
//    public DefaultReflectionMode ReflectionMode;
//    public Cubemap ReflectionCubemap;
//    void Start()
//    {
//        RenderSettings.skybox = skybox;
//        RenderSettings.sun = sunSource;
//        RenderSettings.ambientMode = environmentMode;
//        RenderSettings.ambientIntensity = environmentIntencity;
//        RenderSettings.ambientSkyColor = environmentColor;
//        RenderSettings.fog = fog;
//        RenderSettings.fogColor = fogColor;
//        RenderSettings.defaultReflectionMode = ReflectionMode;
//        RenderSettings.customReflection = ReflectionCubemap;
//    }
//}