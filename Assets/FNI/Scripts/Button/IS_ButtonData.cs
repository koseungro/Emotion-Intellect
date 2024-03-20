using UnityEngine;
using UnityEngine.UI;
using FNI;
/// <summary>
/// 씬 안의 버튼 정보
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "New Button Data", menuName = "FNI/Button Data")]
public class IS_ButtonData : ScriptableObject
{
    public Color GetDefaultTextColor { get { return useTextColor ? Default.TextColor : Base.TextColor; } }
    public Color GetHoverTextColor { get { return useTextColor ? Hover.TextColor : Base.TextColor; } }
    public Color GetPressTextColor { get { return useTextColor ? Press.TextColor : Base.TextColor; } }
    public Color GetDisableTextColor { get { return useTextColor ? Disable.TextColor : Base.TextColor; } }

    public Color GetDefaultImageColor { get { return useImageColor ? Default.ImageColor : Base.ImageColor; } }
    public Color GetHoverImageColor { get { return useImageColor ? Hover.ImageColor : Base.ImageColor; } }
    public Color GetPressImageColor { get { return useImageColor ? Press.ImageColor : Base.ImageColor; } }
    public Color GetDisableImageColor { get { return useImageColor ? Disable.ImageColor : Base.ImageColor; } }

    public Color GetDefaultIconColor { get { return useIconColor ? Default.IconColor : Base.IconColor; } }
    public Color GetHoverIconColor { get { return useIconColor ? Hover.IconColor : Base.IconColor; } }
    public Color GetPressIconColor { get { return useIconColor ? Press.IconColor : Base.IconColor; } }
    public Color GetDisableIconColor { get { return useIconColor ? Disable.IconColor : Base.IconColor; } }

    public Sprite GetDefaultIcon { get { return useIcon ? Default.Icon : Base.Icon; } }
    public Sprite GetHoverIcon { get { return useIcon ? Hover.Icon : Base.Icon; } }
    public Sprite GetPressIcon { get { return useIcon ? Press.Icon : Base.Icon; } }
    public Sprite GetDisableIcon { get { return useIcon ? Disable.Icon : Base.Icon; } }

    public Sprite GetDefaultImage { get { return useImage ? Default.Image : Base.Image; } }
    public Sprite GetHoverImage { get { return useImage ? Hover.Image : Base.Image; } }
    public Sprite GetPressImage { get { return useImage ? Press.Image : Base.Image; } }
    public Sprite GetDisableImage { get { return useImage ? Disable.Image : Base.Image; } }

    public Vector3 GetDefaultScale { get { return useScale ? Default.Scale : Base.Scale; } }
    public Vector3 GetHoverScale { get { return useScale ? Hover.Scale : Base.Scale; } }
    public Vector3 GetPressScale { get { return useScale ? Press.Scale : Base.Scale; } }
    public Vector3 GetDisableScale { get { return useScale ? Disable.Scale : Base.Scale; } }

    public bool UseTransition { get { return useTextColor || useImageColor || useIconColor || useScale; } }

    private static Color DisableColor = new Color(1, 1, 1, 0.5f);

    /// <summary>
    /// 버튼의 이름
    /// </summary>
    public string buttonName;
    /// <summary>
    /// 버튼의 글자를 바꿔 줄 것인지
    /// </summary>
    public bool IsChangeText = true;
    /// <summary>
    /// 버튼의 글자 수에 맞춰 버튼 사이즈를 바꿀지
    /// </summary>
    public bool IsFlexibility = true;
    /// <summary>
    /// 버튼의 글자를 바꿔 줄 것인지
    /// </summary>
    public bool IsChangeFontSize = true;
    /// <summary>
    /// 버튼 폰트의 크기
    /// </summary>
    public int FontSize = 36;
    /// <summary>
    /// 버튼의 이미지와 폰트의 좌우 마진, 좌우 합계값으로 해야 함
    /// </summary>
    public int FontMargin = 120;
    /// <summary>
    /// 버튼의 크기입니다. IsFlexibility이 False일 때만 사용합니다.
    /// </summary>
    public Vector2 size = new Vector2();
    /// <summary>
    /// true일 때 이미지의 크기로 버튼의 크기를 정합니다.
    /// </summary>
    public UseButtonEventType useEvent;
    /// <summary>
    /// true일 때 이미지의 크기로 버튼의 크기를 정합니다.
    /// </summary>
    public bool useImageSize;
    /// <summary>
    /// True일 때 옵션의 TextColor를 사용합니다.
    /// </summary>
    public bool useTextColor;
    /// <summary>
    /// True일 때 옵션의 ImageColor를 사용합니다.
    /// </summary>
    public bool useImageColor;
    /// <summary>
    /// True일 때 옵션의 Image를 사용합니다.
    /// </summary>
    public bool useImage;
    /// <summary>
    /// True일 때 옵션의 IconColor를 사용합니다.
    /// </summary>
    public bool useIconColor;
    /// <summary>
    /// True일 때 옵션의 Icon를 사용합니다.
    /// </summary>
    public bool useIcon;
    /// <summary>
    /// True일 때 옵션의 Scale를 사용합니다.
    /// </summary>
    public bool useScale;
    /// <summary>
    /// True일 때 오브젝트용으로 세팅합니다.
    /// </summary>
    public bool isObject;
    /// <summary>
    /// 상태가 변환되는 시간
    /// </summary>
    public float transitionTime = 0.1f;

    /// <summary>
    /// 옵션 비활성화시 사용되는 옵션
    /// </summary>
    public TransitionSet Base = new TransitionSet(TransitionSet.Type.Base, Color.white, Color.white, Color.white, null, null, Vector3.one);
    /// <summary>
    /// 버튼의 기본
    /// </summary>
    public TransitionSet Default = new TransitionSet(TransitionSet.Type.Default, Color.white, Color.white, Color.white, null, null, Vector3.one);
    /// <summary>
    /// 버튼에 올렸을 때
    /// </summary>
    public TransitionSet Hover = new TransitionSet(TransitionSet.Type.Hover, Color.white, Color.white, Color.white, null, null, Vector3.one * 1.1f);
    /// <summary>
    /// 버튼이 눌렸을 때
    /// </summary>
    public TransitionSet Press = new TransitionSet(TransitionSet.Type.Press, Color.white, Color.white, Color.white, null, null, Vector3.one * 0.9f);
    /// <summary>
    /// 버튼이 비활성화 되었을 때
    /// </summary>
    public TransitionSet Disable = new TransitionSet(TransitionSet.Type.Disable, DisableColor, DisableColor, DisableColor, null, null, Vector3.one);


}

/// <summary>
/// 버튼의 변경 동작입니다.
/// </summary>
[System.Serializable]
public struct TransitionSet
{
    public enum Type
    {
        Base,
        Default,
        Hover,
        Press,
        Disable
    }

    public Type type;

    public Color TextColor;
    public Color ImageColor;
    public Color IconColor;
    public Sprite Image;
    public Sprite Icon;
    public Vector3 Scale;

    public TransitionSet(Type type, Color TextColor, Color ImageColor, Color IconColor, Sprite Icon, Sprite Image, Vector3 Scale)
    {
        this.type = type;
        this.TextColor = TextColor;
        this.IconColor = IconColor;
        this.ImageColor = ImageColor;
        this.Icon = Icon;
        this.Image = Image;
        this.Scale = Scale;
    }
}
public enum UseButtonEventType
{
    None = 0x0,
    Enter = 0x1,
    Exit = 0x2,
    Down = 0x4,
    Up = 0x8,
    Drag = 0x10
}