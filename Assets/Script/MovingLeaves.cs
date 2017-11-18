using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLeaves : MonoBehaviour {
    Vector3 startPos;
    Vector3 startSize;
    float rand;
	// Use this for initialization
	void Start () {
        startPos = transform.position;
        startSize = transform.localScale;
        rand = Random.Range(0.5f, 2.0f);
	}
    float timer = 0;
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime*2.0f;
        Vector3 newPos = startPos;
        Vector3 newScale = startSize;
        newPos.x += Mathf.Sin(timer) * rand * 0.001f;
        newPos.y += Mathf.Sin(timer) * rand * 0.001f;
        newScale.x += Mathf.Sin(timer) * 0.001f;
        newScale.y += Mathf.Sin(timer) * 0.001f;
        transform.position = newPos;
        if(timer > 2*Mathf.PI)
        {
            timer -= 2 * Mathf.PI;
        }
    }
}
