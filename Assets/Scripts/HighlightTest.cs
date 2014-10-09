using UnityEngine;
using System.Collections;

public class HighlightTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnMouseEnter()
    {
        Debug.Log("Hello mouse!");
    }

    void OnMouseExit()
    {
        Debug.Log("Goodbye mouse!");
    }
}
