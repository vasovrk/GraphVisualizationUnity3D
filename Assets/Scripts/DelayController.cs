using UnityEngine;
using System.Collections;

public class DelayController : MonoBehaviour {
	public bool delay = false;
	public BFS bfs ;
	// Use this for initialization
	void Start () {
		bfs = new BFS ();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (Time.time);
		bfs.Update(Time.time);
		//if (delay) {
//			StartCoroutine (TestCoroutine ());
//			delay = false;
//			Debug.Log ("Finoshed");

	//	}
	}

	public bool Delay{
		set { delay = value; }
		get { return delay; }
	}
	public void StartDelay(){
		StartCoroutine (TestCoroutine ());
	//	new WaitForSeconds(5.0f);
		Debug.Log ("skatoules");
	}
	IEnumerator TestCoroutine(){
		Debug.Log ("about to yield return WaitForSeconds(1)");
		Debug.Log (Time.time);
		yield return new WaitForSeconds(5.0f);
		Debug.Log ("fuck");

	}
}
