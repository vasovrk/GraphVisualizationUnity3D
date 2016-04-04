using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridAdjacencyCalc
{
	private int[,] gridArray;
	private int gridHeight;
	private int gridWidth;
	private List<Node> nodes;
	private Dictionary<int,List<Node>> adjMap;

	public GridAdjacencyCalc ()
	{
	}

	public GridAdjacencyCalc (int[,] gridArray, int gridHeight, int gridWidth, List<Node> nodes)
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

//	private void findAdjacency (Node node)
//	{
//
//
//
//		for (int row = -1; row <= 1; row++) {
//			for (int column = -1; column <= 1; column++) {
//
//				if (node.NodeRow + row < 0 || node.NodeRow + row > gridHeight - 1) {
//					continue;
//				}
//
//				if (node.NodeColumn + column < 0 || node.NodeColumn + column > gridWidth - 1) {
//					continue;
//				}
//
//				if ((node.NodeRow + row == node.NodeRow) && (node.NodeColumn + column == node.NodeColumn)) {
//					continue;
//				}
//
//				int gridNode = gridArray [node.NodeRow + row, node.NodeColumn + column];
//				Node newNeighbour = findNodeInNodeList (gridNode);
//				node.Neighbours.Add (newNeighbour);
//			}
//		}
//		Debug.Log (node.NodeValue);
//		Debug.Log ("Neighbours!!!");
//		foreach (Node neighbour in node.Neighbours) {
//			Debug.Log (neighbour.NodeValue);
//		}
//		Debug.Log ("------------------------------------");
//	}


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
