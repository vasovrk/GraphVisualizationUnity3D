using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BFS2 {
	

	public BFS2(){
	}

	public LinkedList<Node2> findPath(Node2 startNode, Node2 endNode){
		 LinkedList<Node2> visitedList = new LinkedList<Node2>();

		 LinkedList<Node2> bfsList = new LinkedList<Node2>();

		bfsList.AddLast (startNode);
		startNode.Parent = null;

		while (!(bfsList.Count == 0)) {
			Node2 node = bfsList.First.Value;
			bfsList.Remove(node);

			if (node.NodeValue == endNode.NodeValue) {
				return constructPath (node);
			}

			else{
				visitedList.AddLast (node);

				foreach(Node2 neighbouNode in node.Neighbours){
					if (!(visitedList.Contains (neighbouNode)) && !(bfsList.Contains (neighbouNode))) {
						neighbouNode.Parent = node;
						bfsList.AddLast (neighbouNode);
					}
				}
			}
		}
		return null;
	}

	private LinkedList<Node2> constructPath(Node2 node){
		LinkedList<Node2> path = new LinkedList<Node2> ();
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
