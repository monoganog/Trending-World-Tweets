using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class MenuButton : MonoBehaviour
{
    public Text locationText;

    public string locationName;
    public float size;
    public int WOEID;

    private float multiplyer = 1.3f;
    private Transform countryTextTransform;
    private Text countryText;
    private bool oneTime = true;

    void Start()
    {
        this.GetComponent<Attractor>().finalSize = new Vector3(size, size, size) * multiplyer;
        this.GetComponent<Attractor>().UpdateMass();

        countryTextTransform = this.transform.Find("Canvas/Text");
        countryText = countryTextTransform.GetComponent<Text>();
        countryText.text = locationName;
    }

    void Update()
    {
        if (oneTime)
        {
            //this.transform.DOScale(size * multiplyer, 1f);
            oneTime = false;
        }
    }

    public void LoadData()
    {

        locationText = GameObject.FindWithTag("locationText").GetComponent<Text>();
        locationText.text = locationName;
        API.Instance.loadCountries(WOEID);
    }
}
