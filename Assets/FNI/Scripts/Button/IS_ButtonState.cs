using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using Panic2;
using TMPro;

namespace Panic2
{
    public class IS_ButtonState : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        /// <summary>
        /// 버튼의 이미지입니다.
        /// </summary>
        public Image MyImage
        {
            get
            {
                if (m_Image == null && Parent)
                    m_Image = Parent.GetComponent<Image>();
                if (m_Image == null)
                    m_Image = GetComponent<Image>();

                return m_Image;
            }
            set
            {
                m_Image = value;
            }
        }
        /// <summary>
        /// 버튼의 이미지 입니다.
        /// </summary>
        public virtual Image MyIcon
        {
            get
            {
                if (m_Icon == null)
                {
                    Transform find = Parent.Find("Icon");
                    if (find)
                        m_Icon = find.GetComponent<Image>();
                }
                return m_Icon;
            }
            set
            {
                m_Icon = value;
            }
        }
        /// <summary>
        /// 버튼의 부모입니다.
        /// </summary>
        public Transform Parent
        {
            get
            {
                if (target != null)
                    m_parent = target;
                if (m_parent == null)
                    m_parent = transform.Find("Parent");
                if (m_parent == null)
                    m_parent = transform;
                return m_parent;
            }
        }
        /// <summary>
        /// 버튼의 글자입니다.
        /// </summary>
        public TextMeshProUGUI MyText
        {
            get
            {
                Transform find;
                if (m_Text == null && Parent)
                {
                    find = Parent.Find("Text");
                    if (find)
                        m_Text = find.GetComponent<TextMeshProUGUI>();
                }

                if (m_Text == null)
                {
                    find = transform.Find("Text");
                    if (find)
                        m_Text = find.GetComponent<TextMeshProUGUI>();
                }
                return m_Text;
            }
            set
            {
                m_Text = value;
            }
        }
        /// <summary>
        /// 버튼의 RectTransform입니다.
        /// </summary>
        public virtual RectTransform MyRect
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
            set
            {
                m_Rect = value;
            }
        }

        /// <summary>
        /// 버튼의 상호작용입니다.
        /// </summary>
        public virtual bool Interactable
        {
            set
            {
                m_Interactable = value;

                if (data.GetDisableImage || data.GetDisableImageColor != Color.white)//이 이미지가 없다면 따로 형태가 바뀌지는 않습니다.
                {
                    if (value == false)
                        SetDisable();
                    else
                    {
                        switch (m_state)
                        {
                            case ButtonFlag.Enable:
                                if (data.useImageColor)
                                    MyImage.color = data.GetDefaultImageColor;
                                if (data.useTextColor)
                                    MyText.color = data.GetDefaultTextColor;
                                SetDefault();
                                break;
                            case ButtonFlag.Hover:
                                if (data.useImageColor)
                                    MyImage.color = data.GetHoverImageColor;
                                if (data.useTextColor)
                                    MyText.color = data.GetHoverTextColor;
                                SetHover();
                                break;
                            case ButtonFlag.Pressed:
                                if (data.useImageColor)
                                    MyImage.color = data.GetPressImageColor;
                                if (data.useTextColor)
                                    MyText.color = data.GetPressTextColor;
                                SetPress();
                                break;
                        }
                    }
                }
            }
            get
            {
                return m_Interactable;
            }
        }
        public bool IsActive { get { return gameObject.activeSelf; } }
        [Header("변경 이미지")]
        public IS_ButtonData data;
        //[Space]//버튼 인식이 되지 않으면 활성화 할 것
        private Button dummyButton;
        [Space]
        public Transform target;
        /// <summary>
        /// 현재 버튼의 상태입니다.
        /// </summary>
        [Space]
        protected ButtonFlag m_state = ButtonFlag.Enable;
        /// <summary>
        /// 상호작용 가능 상태 값 저장용입니다.
        /// </summary>
        [SerializeField]
        protected bool m_Interactable = true;
        /// <summary>
        /// 클릭 상태 값 저장용입니다.
        /// </summary>
        protected bool m_isClick = false;
        /// <summary>
        /// 포인터 온 상태 값 저장용입니다.
        /// </summary>
        protected bool m_isOn = false;
        [Space]
        /// <summary>
        /// 크기가 변경될 부모입니다.
        /// </summary>
        protected Transform m_parent;
        /// <summary>
        /// 버튼 RectTransform 저장용입니다.
        /// </summary>
        protected RectTransform m_Rect;
        /// <summary>
        /// 버튼 Image 저장용 입니다.
        /// </summary>
        protected Image m_Image;
        /// <summary>
        /// 버튼 Icon 저장용 입니다.
        /// </summary>
        protected Image m_Icon;
        /// <summary>
        /// 버튼 Text 저장용입니다.
        /// </summary>
        protected TextMeshProUGUI m_Text;
        [Space]//이벤트 입니다.
        protected UnityAction action;

        [Space]
        public bool useSound = true;
        /// <summary>
        /// 포인터가 버튼에 들어왔을 때 음
        /// </summary>
        public AudioClip enterSound;
        /// <summary>
        /// 클릭 음
        /// </summary>
        public AudioClip clickSound;
        /// <summary>
        /// 클릭음을 출력할 믹서 그룹
        /// </summary>
        public AudioMixerGroup effectAudioMixerGroup;
        /// <summary>
        /// 나의 오디오 소스
        /// </summary>
        private AudioSource myAudioSource;

        protected virtual void Awake()
        {
            //버튼 인식이 되지 않으면 활성화 할 것
            if (GetComponent<Selectable>() == null)
            {
                dummyButton = GetComponent<Button>();
                if (!dummyButton)
                    dummyButton = gameObject.AddComponent<Button>();
                dummyButton.transition = Selectable.Transition.None;
            }
            myAudioSource = gameObject.AddComponent<AudioSource>();
            myAudioSource.playOnAwake = false;
            myAudioSource.outputAudioMixerGroup = effectAudioMixerGroup;

            if (data != null)
                Init(data);
        }

        protected virtual void Start()
        {
            m_state = ButtonFlag.Enable;
        }

        private void OnDisable()
        {
            SetDefault();
        }

        /// <summary>
        /// 버튼을 활성화 상태를 변경 할 수 있습니다.
        /// </summary>
        /// <param name="isActive">활성화 상태</param>
        public virtual void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);

            if (isActive)
                SetDefault();
        }

        /// <summary>
        /// 버튼을 초기화 합니다.
        /// </summary>
        /// <param name="data"></param>
        public void Init(IS_ButtonData data, bool isInteractable = true)
        {
            this.data = data;

            if (MyText)
            {
                if (data.IsChangeText)//이미 지정되어 있는 이름으로 사용할지 아니면 데이터에서 가져 올지 입니다.
                    MyText.text = data.buttonName;
                if (data.IsChangeFontSize)//0이면 본래 폰트 크기를 사용합니다.
                    MyText.fontSize = data.FontSize;// 글자의 크기를 가져옵니다.

                MyText.color = data.GetDefaultTextColor;
            }

            Interactable = isInteractable;
            SetDefault();

            if (data.IsFlexibility && MyText)//버튼의 길이를 변경할지 입니다.
            {
                MyImage.SetNativeSize();//SetDefault()로 변경된 이미지를 월래 사이즈로 변경합니다.

                MyImage.type = Image.Type.Sliced;

                float hight = MyImage.sprite ? MyImage.sprite.texture.height : MyRect.sizeDelta.y;//높이는 이미지의 높이로 고정
                float width = MyText.preferredWidth + data.FontMargin;//넓이는 글자의 길이에 마진을 더한 값

                if (width < hight)//위 과정을 거쳐도 가로 폭이 작으면 높이와 값을 맞춘다.
                    width = hight;

                MyRect.sizeDelta = new Vector2(width, hight);
            }
            else
            {
                if (MyImage.sprite && data.useImageSize)
                {
                    MyImage.type = Image.Type.Simple;
                    //MyImage.SetNativeSize();
                    MyRect.sizeDelta = new Vector2(MyImage.sprite.texture.width, MyImage.sprite.texture.height);
                }
                else if (data.size != new Vector2())
                {
                    MyImage.type = Image.Type.Sliced;
                    MyRect.sizeDelta = data.size;
                }
            }

            MyImage.SetAllDirty();

            if (MyIcon)
            {
                MyIcon.enabled = true;
                //MyIcon.SetNativeSize();//SetDefault()로 변경된 이미지를 월래 사이즈로 변경합니다.
                MyIcon.color = data.GetDefaultIconColor;
                MyIcon.SetAllDirty();
            }
        }
        /// <summary>
        /// 포인터가 들어오면 호출됩니다.
        /// </summary>
        /// <param name="ped"></param>
        public virtual void OnPointerEnter(PointerEventData ped)
        {
            m_state = ButtonFlag.Hover;
            m_isOn = true;

            SetHover();

            if (enterSound && useSound)
            {
                myAudioSource.clip = enterSound;
                myAudioSource.Play();
            }
        }
        /// <summary>
        /// 포인터가 나가면 호출됩니다.
        /// </summary>
        /// <param name="ped"></param>
        public virtual void OnPointerExit(PointerEventData ped)
        {
            Debug.Log("포인터 나감");
            m_isOn = false;
            OnExit();
        }
        /// <summary>
        /// 포인터 다운이 발생하면 호출됩니다.
        /// </summary>
        /// <param name="ped"></param>
        public virtual void OnPointerDown(PointerEventData ped)
        {
            m_state = ButtonFlag.Pressed;
            m_isClick = true;

            SetPress();

            if (clickSound && useSound)
            {
                myAudioSource.clip = clickSound;
                myAudioSource.Play();
            }
        }
        /// 포인터 업이 발생하면 호출됩니다.
        /// <summary>
        /// </summary>
        /// <param name="ped"></param>
        public virtual void OnPointerUp(PointerEventData ped)
        {
            m_state = m_isOn == true ? ButtonFlag.Hover : ButtonFlag.Enable;
            m_isClick = false;

            if (m_isOn == true)
            {
                SetHover();
            }
            else
                SetDefault();
        }

        /// <summary>
        /// OnPointerExit가 호출되면 호출합니다.
        /// </summary>
        public virtual void OnExit()
        {
            m_state = ButtonFlag.Enable;
            m_isClick = false;
            SetDefault();
        }
        /// <summary>
        /// 지정된 이벤트를 추가합니다.
        /// </summary>
        /// <param name="action">추가할 이벤트</param>
        public virtual void AddListener(UnityAction action)
        {
            this.action += action;
        }
        /// <summary>
        /// 지정된 이벤트를 제거합니다.
        /// </summary>
        /// <param name="action">제거할 이벤트</param>
        public virtual void RemoveListener(UnityAction action)
        {
            this.action -= action;
        }

        /// <summary>
        /// 모든 이벤트를 제거합니다.
        /// </summary>
        public virtual void RemoveAllListeners()
        {
            action = null;
        }

        public void SetDefault()
        {
            if (Interactable)
            {
                if (data.useImage)
                    MyImage.sprite = data.GetDefaultImage;
                if (MyIcon && data.GetDefaultIcon)
                {
                    MyIcon.sprite = data.GetDefaultIcon;
                }
            }
            else
                SetDisable();
        }
        public void SetHover()
        {
            if (data.useImage)
                MyImage.sprite = data.GetHoverImage;
            if (MyIcon && data.GetHoverIcon)
            {
                MyIcon.sprite = data.GetHoverIcon;
            }
        }
        public void SetPress()
        {
            if (data.useImage)
                MyImage.sprite = data.GetPressImage;
            if (MyIcon && data.GetPressIcon)
            {
                MyIcon.sprite = data.GetPressIcon;
            }
        }
        public void SetDisable()
        {
            if (data.useImage && data.GetDisableImage)
                MyImage.sprite = data.GetDisableImage;
            if (MyIcon && data.GetDisableIcon)
            {
                MyIcon.sprite = data.GetDisableIcon;
            }
            if (data.useImageColor)
                MyImage.color = data.GetDisableImageColor;
            if (data.useTextColor)
                MyText.color = data.GetDisableTextColor;
        }
    }
    /// <summary>
    /// 버튼의 상태입니다.
    /// </summary>
    public enum ButtonFlag
    {
        /// <summary>
        /// 활성화 상태
        /// </summary>
        Enable = 0x0,
        /// <summary>
        /// 포인터 오버 상태
        /// </summary>
        Hover = 0x1,
        /// <summary>
        /// 눌렀을 때
        /// </summary>
        Pressed = 0x2,
        /// <summary>
        /// 비활성화 상태
        /// </summary>
        Disable = 0x4
    }
}

