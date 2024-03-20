/// 작성자: 백인성 
/// 작성일: 2018-05-01 
/// 수정일: 2018-08-23
/// 저작권: Copyright(C) FNI Co., LTD. 
/// 수정이력 
/// (2018-07-25) 백인성 
///     1. 함수 구조 및 네이밍 규칙 적용
/// (2018-08-23) 백인성 
///		1. 주석 추가
/// (2018-09-11) 백인성 
///		1. 새로운 버튼 데이터에 맞게 작동되도록 수정
///		2. IS_ButtonScale이름 IS_Button로 변경

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Panic2;

namespace Panic2
{

    /// <summary>
    /// IS_ButtonData를 온전히 사용하는 버튼입니다.
    /// </summary>
    public class IS_Button : IS_ButtonState
    {
        #region private
        /// <summary>
        /// 
        /// </summary>
        private Transform m_cover;
        private IEnumerator m_ButtonScale_Routine;
        #endregion

        #region Public Property
        /// <summary>
        /// 버튼 상호작용
        /// </summary>
        public override bool Interactable
        {
            set
            {
                base.Interactable = value;

                if (MyCover != null)
                    MyCover.gameObject.SetActive(!value);
            }
            get
            {
                return base.Interactable;
            }
        }
        public Transform MyCover
        {
            get
            {
                if (m_cover == null)//일단 찾아본다.
                {
                    Transform find = null;
                    if (Parent.Find("Cover"))
                        find = Parent.Find("Cover");
                    else if (Parent.Find("Disable"))//Cover를 찾아보고 없으면 Disable을 찾는다.
                        find = Parent.Find("Disable");
                    if (find != null)
                        m_cover = find;
                }
                return m_cover;
            }
        }
        /// <summary>
        /// 버튼의 렉트 입니다.
        /// </summary>
        public override RectTransform MyRect
        {
            get
            {
                if (Parent && m_Rect == null)
                    m_Rect = Parent.GetComponent<RectTransform>();
                return m_Rect;
            }
            set
            {
                m_Rect = value;
            }
        }
        #endregion

        #region Unity Base Func
        protected override void Start()
        {
            base.Start();

            MyImage.color = data.GetDefaultImageColor;
            if (MyCover)
                MyCover.gameObject.SetActive(false);
        }
        private void OnEnable()
        {
            SetDefault();
        }
        private void OnDisable()
        {
            OnExit();
        }
        #endregion

        #region Public Func
        public override void OnPointerEnter(PointerEventData ped)
        {
            if (Interactable)
            {
                base.OnPointerEnter(ped);
                SetButtonEffect(ButtonFlag.Hover);

                if (CheckType(data.useEvent, UseButtonEventType.Enter) && action != null)
                    action.Invoke();
            }
        }

        public override void OnPointerExit(PointerEventData ped)
        {
            m_isOn = false;

            if (m_isClick == false)
            {
                if (CheckType(data.useEvent, UseButtonEventType.Exit) && action != null)
                    action.Invoke();
            }

            if (Interactable)
            {
                SetButtonEffect(ButtonFlag.Enable);
                if (m_isClick == false)
                    OnExit();
                else
                    SetDefault();
            }
            else
                SetDisable();
        }

        public override void OnPointerDown(PointerEventData ped)
        {
            if (Interactable)
            {
                base.OnPointerDown(ped);
                SetButtonEffect(ButtonFlag.Pressed);

                //조건이 맞으면 이벤트를 호출합니다.
                if (CheckType(data.useEvent, UseButtonEventType.Down) && m_isOn == true && action != null)
                    action();
            }
            else
                OnExit();
        }

        public override void OnPointerUp(PointerEventData ped)
        {
            if (Interactable)
            {
                //Interactable = false;
                base.OnPointerUp(ped);
                SetButtonEffect(m_isOn ? ButtonFlag.Hover : ButtonFlag.Enable, true);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            MyImage.color = data.GetDefaultImageColor;
            if (MyText)
                MyText.color = data.GetDefaultTextColor;
        }
        public override void SetActive(bool isActive)
        {
            base.SetActive(isActive);

            Parent.localScale = data.GetDefaultScale;
            MyImage.SetAllDirty();
        }
        #endregion

        #region Private Func
        /// <summary>
        /// 버튼 이펙트를 시작합니다.
        /// </summary>
        /// <param name="state">이펙트를 실행할 현재 상태입니다.</param>
        /// <param name="isClick">클릭인지 확인합니다. 클릭이면 이벤트를 호출합니다.</param>
        protected void SetButtonEffect(ButtonFlag state, bool isClick = false)
        {
            if (m_ButtonScale_Routine != null)
                StopCoroutine(m_ButtonScale_Routine);

            if (data.UseTransition)
            {
                m_ButtonScale_Routine = ButtonScale_Routine(state, isClick);

                if (gameObject.activeSelf)
                    StartCoroutine(m_ButtonScale_Routine);
            }
            else
            {
                m_ButtonScale_Routine = null;
                //조건이 맞으면 이벤트를 호출합니다.
                if (CheckType(data.useEvent, UseButtonEventType.Up) && isClick && m_isOn == true && action != null)
                    action.Invoke();
            }
        }
        /// <summary>
        /// 이펙트 동작입니다.
        /// </summary>
        /// <param name="state">이펙트를 실행할 현재 상태입니다.</param>
        /// <param name="isClick">클릭인지 확인합니다. 클릭이면 이벤트를 호출합니다.</param>
        /// <returns></returns>
        private IEnumerator ButtonScale_Routine(ButtonFlag state, bool isClick)
        {
            //시작 및 종료 위치 지정
            Vector3 startSize = Parent.localScale;
            Vector3 endSize = data.GetDefaultScale;

            Color startColorI = new Color(), startColorIc = new Color(), startColorT = new Color();
            Color endColorI = new Color(), endColorIc = new Color(), endColorT = new Color();

            startColorI = MyImage.color;
            endColorI = data.GetDefaultImageColor;

            if (MyIcon)
            {
                startColorIc = MyIcon.color;
                endColorIc = data.GetDefaultIconColor;
            }
            if (MyText)
            {
                startColorT = MyText ? MyText.color : new Color();
                endColorT = data.GetDefaultTextColor;
            }

            switch (state)//종료 위치가 아래 상황일때 값을 변경합니다.
            {
                case ButtonFlag.Hover:
                    endSize = data.GetHoverScale;//크기는 동일하게 커져야 합니다.
                    endColorI = data.GetHoverImageColor;
                    endColorIc = data.GetHoverIconColor;
                    endColorT = data.GetHoverTextColor;
                    break;
                case ButtonFlag.Pressed:
                    endSize = data.GetPressScale;
                    endColorI = data.GetPressImageColor;
                    endColorIc = data.GetPressIconColor;
                    endColorT = data.GetPressTextColor;
                    break;
            }

            //크기를 변경합니다.
            float curT = 0;
            while (curT < data.transitionTime)
            {
                curT += Time.deltaTime;

                float percent = curT / data.transitionTime;

                Parent.localScale = Vector3.Lerp(startSize, endSize, percent);
                if (MyImage)
                    MyImage.color = Color.Lerp(startColorI, endColorI, percent);
                if (MyIcon)
                    MyIcon.color = Color.Lerp(startColorIc, endColorIc, percent);
                if (MyText)
                    MyText.color = Color.Lerp(startColorT, endColorT, percent);

                yield return null;
            }

            //종료시 크기를 고정해줍니다.
            Parent.localScale = endSize;

            //조건이 맞으면 이벤트를 호출합니다.
            if (CheckType(data.useEvent, UseButtonEventType.Up) && m_isOn && isClick && action != null)
            {
                if (MyImage)
                    MyImage.color = data.GetDefaultImageColor;
                if (MyIcon)
                    MyIcon.color = data.GetDefaultIconColor;
                if (MyText)
                    MyText.color = data.GetDefaultTextColor;

                action.Invoke();
            }
            else
            {
                if (MyImage)
                    MyImage.color = endColorI;
                if (MyIcon)
                    MyIcon.color = endColorIc;
                if (MyText)
                    MyText.color = endColorT;
            }
        }
        private bool CheckType(UseButtonEventType dataType, UseButtonEventType check)
        {
            return (dataType & check) == check;
        }
        #endregion
    }
}

