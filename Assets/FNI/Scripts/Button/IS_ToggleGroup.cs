/// 작성자: 백인성
/// 작성일: 2018-10-04
/// 수정일: 2018-10-04
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력
/// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Panic2;

namespace Panic2
{
    /// <summary>
    /// 버튼 토글의 상태를 관리 합니다.
    /// </summary>
    public class IS_ToggleGroup : MonoBehaviour
    {
        [Header("Select Option")]
        public bool isAloneSelect;//한개의 토글만 선택되도록합니다.
        [Header("Multi Option")]
        public bool isAlwaysOn;//선택이후 최소 한개가 선택되어 있도록 합니다.
        public bool selectAtDisable;//멀티 선택 전용, 선택시 비활성화 되도록 합니다.
        [HideInInspector]
        public List<IS_ButtonToggle> toggles = new List<IS_ButtonToggle>();//선택한 토글의 목록입니다.

        public bool IsAlwaysOn { get { return isAlwaysOn && toggles.Count == 1; } }//상태를 불러옵니다. 하위 토글이 사용합니다.

        /// <summary>
        /// 토글버튼의 이벤트를 받아 토글의 속성을 변경합니다.
        /// </summary>
        /// <param name="target"></param>
        public void SelectToggle(IS_ButtonToggle target)
        {
            if (isAloneSelect)//1개 선택옵션 사용시
            {
                if (toggles.Count == 0)
                    toggles.Add(target);
                else
                {
                    if (target.IsToggle)
                    {
                        if (target != toggles[0])
                            toggles[0].IsToggle = false;
                        toggles[0].OnExit();
                        toggles[0] = target;
                    }
                    else
                    {
                        toggles = new List<IS_ButtonToggle>();
                    }
                }
            }
            else//다중 선택 가능시 옵션입니다.
            {
                if (target.IsToggle)
                {
                    if (toggles.Find(x => x == target) == null)
                    {
                        toggles.Add(target);
                        if (selectAtDisable)
                        {
                            target.Interactable = false;
                        }
                    }
                }
                else
                {
                    if (toggles.Find(x => x == target) != null)
                        toggles.Remove(target);
                }
            }
        }

        /// <summary>
        /// 모든 토글을 비선택 상태로 만들어 줍니다.
        /// </summary>
        public void AllOff()
        {
            for (int cnt = 0; cnt < toggles.Count; cnt++)
            {
                toggles[cnt].Interactable = true;
                Debug.Log("false");
                toggles[cnt].IsToggle = false;
                toggles[cnt].OnExit();
            }
            toggles = new List<IS_ButtonToggle>();
        }


        /// <summary>
        /// 토글이 선택 되어있는 토글리스트를 반환한다. 
        /// </summary>
        /// <returns></returns>
        public List<IS_ButtonToggle> ActiveToggles()
        {
            List<IS_ButtonToggle> activeToggles = new List<IS_ButtonToggle>();

            foreach (IS_ButtonToggle toggle in toggles)
            {
                if (toggle.IsToggle)
                    activeToggles.Add(toggle);
            }

            Debug.Log(activeToggles.Count);
            return activeToggles;
        }
    }

}

