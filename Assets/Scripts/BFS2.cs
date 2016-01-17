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
		path.AddLast (node);
		node = node.Parent;
		while (node != null) {
			path.AddLast(node);
			if (node != null) {
				node = node.Parent;
			}
		}
		return path;
	}
}
