using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	

	private Dictionary<int,List<Node>> adjMap;
	private List<Node> adjList;

	private List<Node> nodes = new List<Node> ();
	private static readonly int gridWidth = 3;
	private static readonly int gridHeight = 5;
	private int[,] gridArray = new int[gridHeight, gridWidth];
	private Node startNode;
	private Node endNode;

	public Dictionary<NodeState,Color> materialMapper;

	void Start ()
	{
		materialMapper = new Dictionary<NodeState,Color> ();

		materialMapper.Add(NodeState.START_NODE,new Color(1f, 0f, 0f));
		materialMapper.Add(NodeState.END_NODE,new Color(0f, 0f, 1f));
		materialMapper.Add(NodeState.DEFAULT,new Color(0f, 1f, 0f));


		int counter = 0;
		for (int i = 0; i < gridHeight; i++) {
			for (int j = 0; j < gridWidth; j++) {
				GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
				cube.name = counter.ToString();
				cube.transform.localPosition = new Vector3 (j, 0, i);

				cube.GetComponent<MeshRenderer> ().material.color = new Color (0f, 1f, 0f);

				gridArray [i, j] = counter;

				//Maybe Node should inherrit the GameObject class in order to have extra utilities.
				Node node = new Node ();
				node.NodeRow = i;
				node.NodeColumn = j;
				node.NodeValue = counter;
				node.objReference = cube;

				nodes.Add (node);

				counter++;
			}
		}
		adjMap = new Dictionary<int,List<Node>> ();

		//TODO: startNode and endNode to be initialized on user interaction
		foreach(Node node in nodes) {
			this.findAdj (node);
			adjMap.Add (node.NodeValue,node.Neighbours);
		}
			
			
//		bfs = new BFS (adjMap);
//		bfs.findBFS (0);


	}

	private void findAdjacency(Node node){
		
		for (int row = -1; row <= 1; row++) {
			for (int column = -1; column <= 1; column++) {
				
				if (node.NodeRow + row < 0 || node.NodeRow + row > gridHeight - 1) {
					continue;
				}

				if (node.NodeColumn + column < 0 || node.NodeColumn + column > gridWidth - 1) {
					continue;
				}

				if ((node.NodeRow + row == node.NodeRow) && (node.NodeColumn + column == node.NodeColumn)) {
					continue;
				}

				int gridNode = gridArray [node.NodeRow + row, node.NodeColumn + column];
				Node newNeighbour = findNodeInNodeList(gridNode);
				node.Neighbours.Add (newNeighbour);
			}
		}
		Debug.Log (node.NodeValue);
		Debug.Log ("Neighbours!!!");
		foreach (Node neighbour in node.Neighbours) {
			Debug.Log (neighbour.NodeValue);
		}
		Debug.Log ("------------------------------------");
	}
		

	public void findAdj(Node node){
		int row = node.NodeRow;
		int column = node.NodeColumn;
		int gridNode;
		Node newNeighbour;

		if (!(row - 1 < 0)) {
			gridNode = gridArray [row - 1, column];
			newNeighbour = findNodeInNodeList (gridNode);
			node.Neighbours.Add (newNeighbour);
		}
		if(!(row + 1> gridHeight-1)){
			
			gridNode = gridArray [row + 1, column];
			newNeighbour = findNodeInNodeList(gridNode);
			node.Neighbours.Add (newNeighbour);
				
		}

		if (!(column + 1 > gridWidth - 1 )) {
			gridNode = gridArray [row, column + 1];
			newNeighbour = findNodeInNodeList (gridNode);
			node.Neighbours.Add (newNeighbour);
		}

		if(!(column - 1 < 0)){
			gridNode = gridArray [row, column - 1];
			newNeighbour = findNodeInNodeList(gridNode);
			node.Neighbours.Add (newNeighbour);
		}


	

		Debug.Log (node.NodeValue);
		Debug.Log ("Neighbours!!!");
		foreach (Node neighbour in node.Neighbours) {
			Debug.Log (neighbour.NodeValue);
		}
		Debug.Log ("------------------------------------");
	}

	private Node findNodeInNodeList(int nodeValue){
		foreach (Node n in nodes) {
			if (n.NodeValue == nodeValue) {
				return n;
			}
		}
		return null;
	}


	void FixedUpdate () {
//		float moveHorizontal = Input.GetAxis ("Horizontal");
//		float moveVertical = Input.GetAxis ("Vertical");
//
//		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
//
//			
//
//		rb.AddForce (movement * speed);
		HandleMouseEvents();
		HandleKeyboardEvents ();
	}

	private void FindPath() {
			BFS2 bfs = new BFS2 ();
			LinkedList<Node> path = new LinkedList<Node> ();
			path = bfs.findPath (startNode, endNode);
			path.AddLast (startNode);
			foreach (Node node in path) {
				Debug.Log (node.NodeValue);
			}
	}

	private void HandleKeyboardEvents() {
		
		if (Input.GetKeyDown (KeyCode.F)) {
			FindPath ();
		}
	}

	private void HandleMouseEvents() {
			Color startNodeColor;
		    Color endNodeColor;
		Color defaultNodeColor;

			materialMapper.TryGetValue(NodeState.START_NODE,out startNodeColor);
		    materialMapper.TryGetValue (NodeState.END_NODE, out endNodeColor);
		materialMapper.TryGetValue(NodeState.DEFAULT,out defaultNodeColor);



		    if(Input.GetMouseButtonDown(0)){
		//	SceneManager.LoadScene ("demScene");
//			Scene scene = SceneManager.GetSceneByName ("demScene");
//			SceneManager.SetActiveScene (scene);
		
			foreach (Node n in nodes) {
				n.objReference.GetComponent<MeshRenderer> ().material.color = defaultNodeColor;
			}
					
		
			if(this.startNode != null){
				this.startNode.objReference.GetComponent<MeshRenderer> ().material.color = defaultNodeColor;

			}

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			// Casts the ray and get the first game object hit
			Physics.Raycast(ray, out hit);

			Debug.Log("Start Node: " + hit.transform );
			this.startNode = findNodeInNodeList(int.Parse(hit.collider.name));

			
			this.startNode.objReference.GetComponent<MeshRenderer> ().material.color = startNodeColor;
				
		//	this.startNode.objReference.GetComponent<MeshRenderer> ().material.color = new Color (1f, 0f, 0f);


		}

		if (Input.GetMouseButtonDown (1)) {

			if(this.endNode != null){
				this.endNode.objReference.GetComponent<MeshRenderer> ().material.color = defaultNodeColor;

			}

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			// Casts the ray and get the first game object hit
			Physics.Raycast(ray, out hit);

			Debug.Log("End Node: " + hit.transform );
			this.endNode = findNodeInNodeList(int.Parse(hit.collider.name));
			this.endNode.objReference.GetComponent<MeshRenderer> ().material.color = endNodeColor;
		}
	}


		
}



