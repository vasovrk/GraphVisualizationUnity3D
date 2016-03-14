using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DFS {
	public Material visitedMaterial;
	public Material neighbourMaterial;

	public DFS(){
	}

	public DFS(Material visitedMaterial, Material neighbourMaterial){
		this.visitedMaterial = visitedMaterial;
		this.neighbourMaterial = neighbourMaterial;
	}


	public LinkedList<Node> findDFS(Node startNode, Node endNode){
		LinkedList<Node> visitedList = new LinkedList<Node> ();
//
		Stack<Node> dfsStack = new Stack<Node> ();

		dfsStack.Push(startNode);
		startNode.Parent = null;


		while (!(dfsStack.Count == 0)) {
			Node node = dfsStack.Pop ();

			Debug.Log (node.NodeValue);
			if (node.NodeValue == endNode.NodeValue) {
				return constructPath (node);

			} else {
				visitedList.AddLast (node);
				node.objReference.GetComponent<MeshRenderer> ().material = visitedMaterial;
				foreach (Node neighbouNode in node.Neighbours) {
					if (!(visitedList.Contains (neighbouNode)) && !(dfsStack.Contains (neighbouNode))) {
						//neighbouNode.objReference.GetComponent<MeshRenderer> ().material = visitedMaterial;
						neighbouNode.Parent = node;
						dfsStack.Push (neighbouNode);
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
//while (stack.Count != 0)
//{
//	Node tempNode = stack.Pop();
//	Console.WriteLine("Node number: " + tempNode.Index);
//	var negibours = tempNode.neighbors;
//	foreach (var item in negibours)
//	{
//		stack.Push(item);
//	}
//}
//}