using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public float xRotation;
	public float yRotation;
	public float zRotation;


	void Start () {
		transform.localEulerAngles = new Vector3 (xRotation, yRotation, zRotation);
	}
	

	void Update () {
	
	}
}
