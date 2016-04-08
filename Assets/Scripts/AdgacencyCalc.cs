using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdjacencyCalc
{
	private int[,] gridArray;
	private int gridHeight;
	private int gridWidth;
	private List<Node> nodes;
	private Dictionary<int,List<Node>> adjMap;

	public AdjacencyCalc ()
	{
	}

	public AdjacencyCalc (int[,] gridArray, int gridHeight, int gridWidth, List<Node> nodes)
	{
		this.gridArray = gridArray;
		this.gridHeight = gridHeight;
		this.gridWidth = gridWidth;
		this.nodes = nodes;
		this.adjMap = new Dictionary<int,List<Node>> ();
	}

	public Dictionary<int, List<int>> findAdjacency ()
	{

		return null;
	}

	public void adjacencyMap ()
	{
		foreach (Node node in nodes) {
			adjMap = new Dictionary<int,List<Node>> ();
			findAdj (node);
			adjMap.Add (node.NodeValue, node.Neighbours);
		}
	}


	public void findAdj (Node node)
	{
		int row = node.NodeRow;
		int column = node.NodeColumn;
		int gridNode;
		Node newNeighbour;

		if (!(row - 1 < 0)) {
			gridNode = gridArray [row - 1, column];
			newNeighbour = findNodeInNodeList (gridNode);
			node.Neighbours.Add (newNeighbour);
		}
		if (!(row + 1 > gridHeight - 1)) {

			gridNode = gridArray [row + 1, column];
			newNeighbour = findNodeInNodeList (gridNode);
			node.Neighbours.Add (newNeighbour);

		}

		if (!(column + 1 > gridWidth - 1)) {
			gridNode = gridArray [row, column + 1];
			newNeighbour = findNodeInNodeList (gridNode);
			node.Neighbours.Add (newNeighbour);
		}

		if (!(column - 1 < 0)) {
			gridNode = gridArray [row, column - 1];
			newNeighbour = findNodeInNodeList (gridNode);
			node.Neighbours.Add (newNeighbour);
		}

	}

	private Node findNodeInNodeList (int nodeValue)
	{
		foreach (Node n in nodes) {
			if (n.NodeValue == nodeValue) {
				return n;
			}
		}
		return null;
	}
}
