////////////////////////////////////////////////////////////////////////////
// bl_SocialCounterDisplay
//
//
//                    Lovatto Studio 2016
////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;

public class bl_SocialCounterDisplay : MonoBehaviour
{
    [TextArea(2,7)]
    public string DisplayFormat = "Prefix: {0}";
    public Social m_Social = Social.Facebook;
    [SerializeField]private string UrlToOpen;

    [Header("References")]
    [SerializeField]private Text DisplayText;

    private bl_SocialCounter CounterManager;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        if(CounterManager == null)
        {
            CounterManager = FindObjectOfType<bl_SocialCounter>();
        }

        if (CounterManager)
        {
            CounterManager.RegisterDisplay(this);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="count"></param>
    public void SendCount(int count)
    {
        DisplayText.text = string.Format(DisplayFormat,AbreviateValue(count));
    }

    /// <summary>
    /// 
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public string AbreviateValue(int num)
    {
        if (num >= 100000000)
            return (num / 1000000).ToString("#,0M");

        if (num >= 10000000)
            return (num / 1000000).ToString("0.#") + "M";

        if (num >= 100000)
            return (num / 1000).ToString("#,0K");

        if (num >= 10000)
            return (num / 1000).ToString("0.#") + "K";

        return num.ToString("#,0");
    }

    public void Open()
    {
        if (string.IsNullOrEmpty(UrlToOpen))
            return;

        Application.OpenURL(UrlToOpen);
    }


    [System.Serializable]
    public enum Social
    {
        Facebook,
        Youtube,
        GooglePlus,
        Twitter,
        Instagram,
        Github,
    }
}