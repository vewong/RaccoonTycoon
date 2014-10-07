using UnityEngine;
using System.Collections;

public class BounceAnimation : MonoBehaviour 
{
    public Sprite mySprite;
    public Transform position;

    float jumpTime;
    public float bounceTrigger;

	// Use this for initialization
	void Start () 
    {
        jumpTime = 30f;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}
}
