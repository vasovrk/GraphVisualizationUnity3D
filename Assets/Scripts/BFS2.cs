using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BFS2 {
	

	public BFS2(){
	}

	public LinkedList<Node> findPath(Node startNode, Node endNode){
		 LinkedList<Node> visitedList = new LinkedList<Node>();

		 LinkedList<Node> bfsList = new LinkedList<Node>();

		bfsList.AddLast (startNode);
		startNode.Parent = null;

		while (!(bfsList.Count == 0)) {
			Node node = bfsList.First.Value;
			bfsList.Remove(node);

			if (node.NodeValue == endNode.NodeValue) {
				return constructPath (node);
			}

			else{
				visitedList.AddLast (node);

				foreach(Node neighbouNode in node.Neighbours){
					if (!(visitedList.Contains (neighbouNode)) && !(bfsList.Contains (neighbouNode))) {
						neighbouNode.Parent = node;
						bfsList.AddLast (neighbouNode);
					}
				}
			}
		}
		return null;
	}

	private LinkedList<Node> constructPath(Node node){
		LinkedList<Node> path = new LinkedList<Node> ();

		while (node != null) {

			node.objReference.GetComponent<MeshRenderer> ().material.color = new Color (1f, 1f, 0f);

			path.AddLast(node);
			node = node.Parent;
		}

		return path;
	}
}
