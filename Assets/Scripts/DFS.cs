using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DFS
{
	public Material visitedMaterial;
	public Material neighbourMaterial;
	private LinkedList<Node> visited;

	public DFS ()
	{
		visited = new LinkedList<Node> ();
	}

	public DFS (Material visitedMaterial, Material neighbourMaterial)
	{
		this.visitedMaterial = visitedMaterial;
		this.neighbourMaterial = neighbourMaterial;
		visited = new LinkedList<Node> ();
	}


	public LinkedList<Node> findDFS (Node startNode, Node endNode, bool spanTree)
	{
		
		if (visited != null) {
			visited = new LinkedList<Node> ();
		}
		Stack<Node> dfsStack = new Stack<Node> ();
		visited.AddLast (startNode);
		dfsStack.Push (startNode);
		startNode.Parent = null;


		while (!(dfsStack.Count == 0)) {

			Node node = dfsStack.Peek ();
			Node neighbour = getAdjUnvisitedVertex (node);

			if ((node.NodeValue == endNode.NodeValue) && spanTree == false) {
				return constructPath (node);

			} else {

				if (neighbour == null) {
					dfsStack.Pop ();
				} else {
					visited.AddLast (neighbour);
					neighbour.Parent = node;
					node.Edges.Add (neighbour);
					neighbour.objReference.GetComponent<MeshRenderer> ().material = visitedMaterial;
					dfsStack.Push (neighbour);
				}
			}
		

		}
		foreach (Node node in visited) {
			Debug.Log (node.NodeValue);
		}
		return null;
	}


	private LinkedList<Node> constructPath (Node node)
	{
		LinkedList<Node> path = new LinkedList<Node> ();

		while (node != null) {

			path.AddLast (node);
			node = node.Parent;
		}

		return path;
	}

	public Node getAdjUnvisitedVertex (Node node)
	{
		foreach (Node neighbour in node.Neighbours) {
			if (!visited.Contains (neighbour)) {
				neighbour.Parent = node;
				return neighbour;
			}
		}
		return null;
	}
}
