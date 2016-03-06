using UnityEngine;
using System.Collections;

public class demoScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public void callWaiLigaki(float delay){
		StartCoroutine (WaitLigaki (delay));
	}

	IEnumerator WaitLigaki(float delay){
		yield return new WaitForSeconds (delay);
	}
}
