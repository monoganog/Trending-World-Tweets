using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweet : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        float r = this.gameObject.GetComponent<Renderer>().material.color.r;
        float g = this.gameObject.GetComponent<Renderer>().material.color.g;
        float b = this.gameObject.GetComponent<Renderer>().material.color.b;


        Debug.Log("magnitude of this gameObjects Final size = "+this.GetComponentInParent<Attractor>().finalSize.magnitude);

        //float a = Random.Range(0.5f, 1f);

        float a = this.GetComponentInParent<Attractor>().finalSize.magnitude;
        float aRemapped;
        aRemapped = Remap(a, 1.5f, 3, 0.3f, 0.9f);

        this.gameObject.GetComponent<Renderer>().material.color = new Color(r,g,b,aRemapped);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }
}
