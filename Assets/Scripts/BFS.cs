using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class BFS {
	public Material visitedMaterial;
	public Material neighbourMaterial;



	public BFS(Material visitedMaterial, Material neighbourMaterial){
		this.visitedMaterial = visitedMaterial;
		this.neighbourMaterial = neighbourMaterial;
		//this.time = time;
	}

	public void UpdateTime(float time){
		//Debug.Log (time);


	}

		

	public LinkedList<Node> findPath(Node startNode, Node endNode){
		
		 LinkedList<Node> visitedList = new LinkedList<Node>();

		 Queue<Node> bfsList = new Queue<Node>();
		//LinkedList<Node> bfsList = new LinkedList<Node>();
		//bfsList.AddLast (startNode);
		bfsList.Enqueue(startNode);
		startNode.Parent = null;

		while (!(bfsList.Count == 0)) {
//			    Node node = bfsList.First.Value;
//				bfsList.Remove (node);
			    Node node = bfsList.Dequeue();
			    Debug.Log (node.NodeValue);
				

//				Debug.Log ("The time is:" + Time.time);
				if (node.NodeValue == endNode.NodeValue) {
				return constructPath (node);
			
				} else {
					visitedList.AddLast (node);
				node.objReference.GetComponent<MeshRenderer> ().material = visitedMaterial;
					foreach (Node neighbouNode in node.Neighbours) {
						if (!(visitedList.Contains (neighbouNode)) && !(bfsList.Contains (neighbouNode))) {
						//neighbouNode.objReference.GetComponent<MeshRenderer> ().material = visitedMaterial;
							neighbouNode.Parent = node;
					//	bfsList.AddLast (neighbouNode);
						bfsList.Enqueue (neighbouNode);
						}
					}
				}

			}
			

		return null;
	}


	private LinkedList<Node> constructPath(Node node){
		LinkedList<Node> path = new LinkedList<Node> ();

		while (node != null) {

			//node.objReference.GetComponent<MeshRenderer> ().material.color = new Color (1f, 1f, 0f);

			path.AddLast(node);
			node = node.Parent;
		}

		return path;
	}

}
