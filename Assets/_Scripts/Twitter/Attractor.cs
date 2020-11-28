using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Attractor : MonoBehaviour {

    Vector3 sizeIncreaseVector = new Vector3();
    private float sizeIncrease = 0.3f;
    const float G = 667.4f;

    private float animationTime = 1f;

    public float grav;
    public static List<Attractor> Attractors;

    public Rigidbody rb;

    private bool oneTime = true;

    public Vector3 finalSize;

    //private void OnMouseDown()
    //{
    //    Application.OpenURL("https://www.google.com.au/maps/@-20.2705963,148.719751,15.7z");
    //}

    private void OnMouseOver()
    {
        //Debug.Log("mouse ovcer");
    }

    private void OnMouseEnter()
    {
        
        Debug.Log("mouse enter "+name);
        this.transform.DOScale(this.GetComponent<Attractor>().finalSize + sizeIncreaseVector, animationTime).SetEase(Ease.OutCubic);
    }

    private void OnMouseExit()
    {
        //DOTween.KillAll();
        this.transform.DOScale(this.GetComponent<Attractor>().finalSize - sizeIncreaseVector, animationTime).SetEase(Ease.OutCubic);
        Debug.Log("mouse exit " + name);
    }

    private void Start()
    {
        sizeIncreaseVector = new Vector3(sizeIncrease, sizeIncrease, sizeIncrease);
        //this.rb.mass = this.transform.localScale.sqrMagnitude * grav;
        oneTime = false;
        //Debug.Log(this.transform.localScale + " is my scale" + this.rb.mass + " is my mass");

    }

    public void UpdateMass()
    {
        this.rb.mass = finalSize.sqrMagnitude * grav;
    }

    void FixedUpdate ()
	{
		foreach (Attractor attractor in Attractors)
		{
			if (attractor != this)
				Attract(attractor);
            Debug.DrawLine(transform.position, attractor.transform.position);
		}
        if (oneTime)
        {


        }
    }

	void OnEnable ()
	{
		if (Attractors == null)
			Attractors = new List<Attractor>();

		Attractors.Add(this);
    }

    

    void OnDisable ()
	{
		Attractors.Remove(this);
	}

	void Attract (Attractor objToAttract)
	{
		Rigidbody rbToAttract = objToAttract.rb;

		Vector3 direction = rb.position - rbToAttract.position;
		float distance = direction.magnitude;

		if (distance == 0f)
			return;

		float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
		Vector3 force = direction.normalized * forceMagnitude;

		rbToAttract.AddForce(force);
    }
}
