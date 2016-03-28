using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class BFS
{
	public Material visitedMaterial;
	public Material neighbourMaterial;



	public BFS (Material visitedMaterial, Material neighbourMaterial)
	{
		this.visitedMaterial = visitedMaterial;
		this.neighbourMaterial = neighbourMaterial;
		//this.time = time;
	}

	public void UpdateTime (float time)
	{
		//Debug.Log (time);


	}

		
	public BFS ()
	{
	}

	public LinkedList<Node> findPath (Node startNode, Node endNode,bool spanTree)
	{
		
		LinkedList<Node> visitedList = new LinkedList<Node> ();

		Queue<Node> bfsList = new Queue<Node> ();
		//LinkedList<Node> bfsList = new LinkedList<Node>();
		//bfsList.AddLast (startNode);
		bfsList.Enqueue (startNode);
		startNode.Parent = null;

		while (!(bfsList.Count == 0)) {
//			    Node node = bfsList.First.Value;
//				bfsList.Remove (node);
			Node node = bfsList.Dequeue ();
			Debug.Log (node.NodeValue);
				

//				Debug.Log ("The time is:" + Time.time);
			if ((node.NodeValue == endNode.NodeValue) && (spanTree==false)) {
				return constructPath (node);
			
			} else {
				visitedList.AddLast (node);
				if (!spanTree) {
					node.objReference.GetComponent<MeshRenderer> ().material = visitedMaterial;
				}
				foreach (Node neighbouNode in node.Neighbours) {
					if (!(visitedList.Contains (neighbouNode)) && !(bfsList.Contains (neighbouNode))) {
						//neighbouNode.objReference.GetComponent<MeshRenderer> ().material = visitedMaterial;
						neighbouNode.Parent = node;
						node.Edges.Add (neighbouNode);
						//bfsList.AddLast (neighbouNode);
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

//	public void findSpanTree (Node startNode)
//	{
//
//		LinkedList<Node> visitedList = new LinkedList<Node> ();
//
//		Queue<Node> bfsList = new Queue<Node> ();
//		bfsList.Enqueue (startNode);
//		startNode.Parent = null;
//
//		while (!(bfsList.Count == 0)) {
//			Node node = bfsList.Dequeue ();
//			Debug.Log (node.NodeValue);
//		
//			visitedList.AddLast (node);
//
//			foreach (Node neighbouNode in node.Neighbours) {
//				if (!(visitedList.Contains (neighbouNode)) && !(bfsList.Contains (neighbouNode))) {
//
//					neighbouNode.Parent = node;
//					node.Edges.Add (neighbouNode);
//					bfsList.Enqueue (neighbouNode);
//				}
//			}
//
//
//		}
//
//
//	}

	private LinkedList<Node> constructPath (Node node)
	{
		LinkedList<Node> path = new LinkedList<Node> ();

		while (node != null) {

			//node.objReference.GetComponent<MeshRenderer> ().material.color = new Color (1f, 1f, 0f);

			path.AddLast (node);
			node = node.Parent;
		}

		return path;
	}

}
