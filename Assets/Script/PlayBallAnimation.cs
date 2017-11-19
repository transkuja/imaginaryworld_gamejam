using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBallAnimation : MonoBehaviour {
    Animator anim;
    [SerializeField]
    float timer;
    float timeToReach;
    [SerializeField]
    float minTime = 10.0f;
    [SerializeField]
    float maxTime = 30.0f;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        timeToReach = Random.Range(minTime, maxTime);
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if(timer >= timeToReach)
        {
            anim.SetTrigger("play");
            timer = 0.0f;
            timeToReach = Random.Range(minTime, maxTime);
        }
	}
}
