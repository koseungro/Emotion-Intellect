/// 작성자: 백인성 
/// 작성일: 2018-08-23
/// 수정일: 2018-08-23
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력 
/// (2018-07-25) 백인성 
///		1. 함수 구조 및 네이밍 규칙 적용
/// (2018-10-08) 백인성
///		1. 토글 기능 수정

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Panic2;

namespace Panic2
{
    /// <summary>
    /// 토글의 기본형입니다.
    /// </summary>
    public class IS_ButtonToggle : IS_Button
    {
        public IS_ToggleGroup toggleGroup;
        public UnityEvent OnValueChanged = new UnityEvent();

        public bool IsToggle { get { return MyCover.gameObject.activeSelf; } set { MyCover.gameObject.SetActive(value); } }//현재 토글이 선택상태인지 상태를 반환합니다.
        public bool IsOn { get { return m_isOn; } }

        public override void OnPointerUp(PointerEventData ped)
        {
            base.OnPointerUp(ped);

            if (Interactable)
            {
                if (IsOn)
                {
                    if (IsToggle == true)
                    {
                        if (toggleGroup)
                        {
                            if (toggleGroup.IsAlwaysOn == false)
                            {
                                IsToggle = false;
                                if (toggleGroup)
                                    toggleGroup.SelectToggle(this);
                                OnValueChanged.Invoke();
                            }
                        }
                        else
                        {
                            IsToggle = false;
                            if (toggleGroup)
                                toggleGroup.SelectToggle(this);
                            OnValueChanged.Invoke();
                        }
                    }
                    else
                    {
                        IsToggle = true;
                        if (toggleGroup)
                            toggleGroup.SelectToggle(this);
                        OnValueChanged.Invoke();
                    }
                }
                else
                {
                    if (IsToggle)
                    {
                        IsToggle = false;
                        if (toggleGroup.isAlwaysOn)
                        {
                            foreach (IS_ButtonToggle toggle in toggleGroup.toggles)
                            {
                                if (toggle.IsOn)
                                {
                                    if (toggleGroup)
                                        toggleGroup.SelectToggle(this);
                                    OnValueChanged.Invoke();
                                    return;
                                }
                            }
                        }
                        IsToggle = true;
                    }
                }
            }
        }

        public void Click(bool force = false)
        {
            if (Interactable)
            {
                if (IsOn || force)
                {
                    if (IsToggle == true)
                    {
                        if (toggleGroup)
                        {
                            if (toggleGroup.IsAlwaysOn == false)
                            {
                                IsToggle = false;
                                if (toggleGroup)
                                    toggleGroup.SelectToggle(this);
                                OnValueChanged.Invoke();
                            }
                        }
                        else
                        {
                            IsToggle = false;
                            if (toggleGroup)
                                toggleGroup.SelectToggle(this);
                            OnValueChanged.Invoke();
                        }
                    }
                    else
                    {
                        IsToggle = true;
                        if (toggleGroup)
                            toggleGroup.SelectToggle(this);
                        OnValueChanged.Invoke();
                    }
                }
                else
                {
                    if (IsToggle)
                    {
                        IsToggle = false;
                        if (toggleGroup.isAlwaysOn)
                        {
                            foreach (IS_ButtonToggle toggle in toggleGroup.toggles)
                            {
                                if (toggle.IsOn)
                                {
                                    if (toggleGroup)
                                        toggleGroup.SelectToggle(this);
                                    OnValueChanged.Invoke();
                                    return;
                                }
                            }
                        }
                        IsToggle = true;
                    }
                }
            }

        }
    }
}
