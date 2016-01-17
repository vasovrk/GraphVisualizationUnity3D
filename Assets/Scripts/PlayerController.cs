using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	
	public BFS bfs;

	private Dictionary<int,List<Node2>> adjMap;
	private List<Node2> adjList;

	private List<Node2> nodes = new List<Node2> ();
	private int gridSize = 3;
	private int[,] gridArray = new int[3,3];
	private Node2 startNode;
	private Node2 endnode;

	void Start ()
	{

		int counter = 0;
		for (int i = 0; i < gridSize; i++) {
			for (int j = 0; j < gridSize; j++) {
				GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
				float rand = UnityEngine.Random.Range (1, 2.5f);
				cube.transform.localScale = new Vector3 (1, rand, 1);
				cube.transform.localPosition = new Vector3 (i, 0 , j);


				gridArray [i, j] = counter;

				Node2 node = new Node2 ();
				node.NodeRow = i;
				node.NodeColumn = j;
				node.NodeValue = counter;

				nodes.Add (node);

				counter++;

			}
		}
		adjMap = new Dictionary<int,List<Node2>> ();


		foreach(Node2 node in nodes) {
			this.findAdjacency (node);
			adjMap.Add (node.NodeValue,node.Neighbours);
			if (node.NodeValue == 4) {
				startNode = node;
			}
			if (node.NodeValue == 4) {
				endnode = node;
			}
		}
			
		BFS2 bfs = new BFS2 ();
		LinkedList<Node2> path = new LinkedList<Node2> ();
		path = bfs.findPath (startNode, endnode);
		foreach (Node2 node in path) {
			Debug.Log (node.NodeValue);
		}
			
//		bfs = new BFS (adjMap);
//		bfs.findBFS (0);


	}
	private void findAdjacency(Node2 node){
		
		for (int row = -1; row <= 1; row++) {
			for (int column = -1; column <= 1; column++) {
				
				if (node.NodeRow + row < 0 || node.NodeRow + row > gridSize-1) {
					continue;
				}

				if (node.NodeColumn + column < 0 || node.NodeColumn + column > gridSize-1) {
					continue;
				}
					

				if ((node.NodeRow + row == node.NodeRow) && (node.NodeColumn + column == node.NodeColumn)) {
					continue;
				}
				int gridNode = gridArray [node.NodeRow + row, node.NodeColumn + column];
				Node2 newNeighbour = findNodeInNodeList(gridNode);
				node.Neighbours.Add (newNeighbour);
			}
		}
	}
		
	private Node2 findNodeInNodeList(int nodeValue){
		foreach (Node2 n in nodes) {
			if (n.NodeValue == nodeValue) {
				return n;
			} else {
				continue;
			}
		}
		return null;
	}


	void FixedUpdate ()
	{
//		float moveHorizontal = Input.GetAxis ("Horizontal");
//		float moveVertical = Input.GetAxis ("Vertical");
//
//		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
//
//			
//
//		rb.AddForce (movement * speed);
	}
		
}



