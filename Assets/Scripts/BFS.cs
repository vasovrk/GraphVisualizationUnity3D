using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class BFS
{
	public DelayController delayController;

	public Material visitedMaterial;

	private float t;

	public BFS (Material visitedMaterial)
	{
		this.visitedMaterial = visitedMaterial;
		delayController = GameObject.Find("GameObject").GetComponent<DelayController>();

	}
		
		
	public BFS ()
	{
		
	}

	public LinkedList<Node> findPath (Node startNode, Node endNode,bool spanTree)
	{
		
		LinkedList<Node> visitedList = new LinkedList<Node> ();

		Queue<Node> bfsList = new Queue<Node> ();

		bfsList.Enqueue (startNode);
		startNode.Parent = null;

		while (!(bfsList.Count == 0)) {
			Node node = bfsList.Dequeue ();
			Debug.Log (node.NodeValue);

			if ((node.NodeValue == endNode.NodeValue) && (spanTree==false)) {
				return constructPath (node);
			
			} else {
				visitedList.AddLast (node);
				delayController.StartCoroutine (TestCoroutine ());
				if (!spanTree) {
				//	startDelayTime = t;
					//delay = true;
				//	delayController.StartDelay();
					node.objReference.GetComponent<MeshRenderer> ().material = visitedMaterial;
				}
				foreach (Node neighbouNode in node.Neighbours) {
					if (!(visitedList.Contains (neighbouNode)) && !(bfsList.Contains (neighbouNode))) {
						neighbouNode.Parent = node;
						node.Edges.Add (neighbouNode);
						bfsList.Enqueue (neighbouNode);
					}
				}

			}
				

		}
			
		foreach (Node n in visitedList) {
			Debug.Log (n.NodeValue + ":");
		}
		return visitedList;
	}
	private bool delay;
	private LinkedList<Node> constructPath (Node node)
	{
		LinkedList<Node> path = new LinkedList<Node> ();

		while (node != null) {

			path.AddLast (node);
			node = node.Parent;
		}

		return path;
	}
//	private float startDelayTime;
	public void Update(float time){
		this.t = Time.time;
//		if (delay) {
//			while (time < startDelayTime+3.0f) {
//				Debug.Log ("skata");
//			}
//			delay = false;
//		}

	}

	IEnumerator TestCoroutine(){
		Debug.Log ("about to yield return WaitForSeconds(1)");
		Debug.Log (Time.time);
		yield return new WaitForSeconds(5.0f);
		Debug.Log ("fuck");

	}
}
