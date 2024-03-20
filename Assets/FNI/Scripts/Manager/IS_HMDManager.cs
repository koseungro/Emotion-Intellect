/// 작성자: 백인성
/// 작성일: 2018-12-07
/// 수정일: 2018-12-07
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력
/// 

using System;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;

/// <summary>
/// HMD의 착용 상태를 점검하고 이벤트를 호출 합니다.
/// </summary>
public class IS_HMDManager : MonoBehaviour
{
    #region Singleton
    private static IS_HMDManager _instance;
    public static IS_HMDManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<IS_HMDManager>();
                if (_instance == null)
                    Debug.LogError("IS_HMDManager를 찾을 수 없습니다. ");
            }
            return _instance;
        }
    }
	#endregion

	public static UnityAction hmdPauseAction;
	public static UnityAction hmdPlayAction;

	public static bool IsPause;

	#region Unity base method
	private void Start ()
	{
		if (OVRManager.instance.isUserPresent)//사용자가 HMD를 착용하고 있는 중이면 True를 반환합니다.
			HMDMounted();
		else
			HMDUnMounted();

		OVRManager.HMDMounted += HMDMounted;
		OVRManager.HMDUnmounted += HMDUnMounted;
	}

	public void OnApplicationQuit()
	{
		OVRManager.HMDMounted -= HMDMounted;
		OVRManager.HMDUnmounted -= HMDUnMounted;
	}
	#endregion

	private void HMDMounted()
	{
		IsPause = false;

		if (hmdPlayAction != null)
			hmdPlayAction();

        Debug.Log("HMD 착용");
	}
	private void HMDUnMounted()
	{
		IsPause = true;

		if (hmdPauseAction != null)
			hmdPauseAction();

        Debug.Log("HMD 미착용");
	}
}
