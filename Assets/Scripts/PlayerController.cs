using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	
	public BFS bfs;

	private Dictionary<int,List<int>> adjMap;
	private List<int> adjList;

	private List<Node> nodes = new List<Node> ();
	private int gridSize = 3;
	private int[,] gridArray = new int[3,3];

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

				Node node = new Node ();
				node.NodeRow = i;
				node.NodeColumn = j;
				node.Value = counter;

				nodes.Add (node);

				counter++;

			}
		}
		adjMap = new Dictionary<int,List<int>> ();


		foreach(Node node in nodes) {
			this.findAdjacency (node);
			adjMap.Add (node.Value,node.Neighbours);
		}
			

			
		bfs = new BFS (adjMap);
		bfs.findBFS (0);


	}
	private void findAdjacency(Node node){
		
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

				node.Neighbours.Add (gridArray [node.NodeRow + row, node.NodeColumn + column]);
			}
		}
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



