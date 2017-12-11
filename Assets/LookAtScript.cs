using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtScript : MonoBehaviour {
    public Transform head;
	// Use this for initialization
	void Start () {
		head = GameObject.Find("FirstPersonCharacter").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        head = GameObject.Find("Camera (eye)").GetComponent<Transform>();
            transform.LookAt(transform.position + head.transform.rotation * Vector3.forward, head.transform.rotation * Vector3.up);
	}
}
