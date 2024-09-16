using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	private Rigidbody rigidbody;
	public float Force = 100;

	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		if (Input.GetKey(KeyCode.A)) {
			rigidbody.AddForce (Force*Time.deltaTime,0,0);
		}
		if (Input.GetKey(KeyCode.D)) {
			rigidbody.AddForce (-Force*Time.deltaTime,0,0);
		}
		if (Input.GetKey(KeyCode.W)) {
			rigidbody.AddForce (0,0,-Force*Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.S)) {
			rigidbody.AddForce (0,0,Force*Time.deltaTime);
		}
	}
}
