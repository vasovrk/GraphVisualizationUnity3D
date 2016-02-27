using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public float xRotation;
	public float yRotation;
	public float zRotation;

	public float speed = 0.5f;


	void Start () {
		transform.localEulerAngles = new Vector3 (xRotation, yRotation, zRotation);
	}
	

	void Update () {
//		xRotation += Input.GetAxis("x") * speed;
//		yRotation += Input.GetAxis ("y") * speed;
//		zRotation += Input.GetAxis( * speed;
	}
}
