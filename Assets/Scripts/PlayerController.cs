using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	

	private Dictionary<int,List<Node>> adjMap;
	private List<Node> adjList;

	private List<Node> nodes = new List<Node> ();
	private static readonly int gridWidth = 10;
	private static readonly int gridHeight = 10;
	private int gridSize = gridWidth * gridHeight;
	private int[,] gridArray = new int[gridHeight, gridWidth];
	private Node startNode;
	private Node endNode;

	public Material nodeDefaultMaterial;
	public Material startNodeMaterial;
	public Material endNodeMaterial;
	public Material pathMaterial;
	public Material visitedMaterial;
	public Material neighbourMaterial;

	public GameObject endParticleSystem;
	public GameObject startParticleSystem;

	public BFS2 bfs;

	public GameObject demoPrefab;
	public GameObject d;
	public Dictionary<NodeState, Color> materialMapper;
	void Start ()
	{
			
		bfs = new BFS2 (visitedMaterial,neighbourMaterial);

		int counter = 0;
	
		float offset = 0.5f;

		float halfWidth = gridWidth / 2.0f;
		float halfHeight = gridHeight / 2.0f;

		for (int i = 0; i < gridHeight; i++) {
			
			for (int j = 0; j < gridWidth; j++) {
				
				GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
				sphere.name = counter.ToString();
				sphere.transform.localPosition = new Vector3 ((i - halfHeight) + offset , 0, (j - halfWidth) + offset);
				
				sphere.GetComponent<MeshRenderer> ().material = nodeDefaultMaterial;

				gridArray [i, j] = counter;

				//Maybe Node should inherrit the GameObject class in order to have extra utilities.
				Node node = new Node ();
				node.NodeRow = i;
				node.NodeColumn = j;
				node.NodeValue = counter;
				node.objReference = sphere;

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


	

//		Debug.Log (node.NodeValue);
//		Debug.Log ("Neighbours!!!");
//		foreach (Node neighbour in node.Neighbours) {
//			Debug.Log (neighbour.NodeValue);
//		}
//		Debug.Log ("------------------------------------");
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
		bfs.UpdateTime (Time.time);

		float mSec = Time.time;
		foreach (Node node in nodes) {
			float animationOffset = node.animationOffset;
			float y = ((float)Math.Sin(mSec) * 0.05f) 
				+ ((float)Math.Cos((mSec * 1000f * animationOffset * 3.0) / 2000.0f) * 0.06f);

			Vector3 pos = node.objReference.transform.localPosition;
			pos.y = y;

			node.objReference.transform.localPosition = pos;


		}

		HandleMouseEvents();
		HandleKeyboardEvents ();
	}
	bool pathFound = false;
	ParticleSystem PathParticle;
	private void FindPath() {
		  //  BFS2 bfs = new BFS2 (visitedMaterial,neighbourMaterial);
			LinkedList<Node> path = new LinkedList<Node> ();
			
		    path = bfs.findPath (startNode, endNode);
			path.AddLast (startNode);

		    pathFound = true;

		foreach (Node node in path) {
			//d = Instantiate(demoPrefab):
			if (node.NodeValue == startNode.NodeValue){
				node.objReference.GetComponent<MeshRenderer> ().material = pathMaterial;
			continue;
		}
			if (node.NodeValue == endNode.NodeValue) {
				node.objReference.GetComponent<MeshRenderer> ().material = pathMaterial;
				continue;
			
			}
				d = (GameObject)Instantiate (demoPrefab);
				d.transform.localPosition = node.objReference.transform.localPosition;
				PathParticle = d.GetComponent<ParticleSystem> ();
				ParticleSystem.EmissionModule em = PathParticle.emission;
				em.enabled = true;
				PathParticle.startSize = 0.7f;
				node.objReference.GetComponent<MeshRenderer> ().material = pathMaterial;


			}



	}

	private void HandleKeyboardEvents() {
		
		if (Input.GetKeyDown (KeyCode.F) && (pathFound == false)) {
			
			FindPath ();
		}

		if (Input.GetKeyDown(KeyCode.E)) {
			Debug.Log ("SKATOULES");
			Application.Quit ();

		}
	}
	GameObject startNodePrefab;
	GameObject endNodePrefab;

	private void HandleMouseEvents() {
			

	
		    if(Input.GetMouseButtonDown(0)){
		//	SceneManager.LoadScene ("demScene");
//			Scene scene = SceneManager.GetSceneByName ("demScene");
//			SceneManager.SetActiveScene (scene);
			pathFound = false;
			foreach (Node n in nodes) {

				if (n == startNode) {
					continue;
				}
				if (n == endNode) {
					n.objReference.GetComponent<MeshRenderer> ().material = endNodeMaterial;
					continue;
				}

				n.objReference.GetComponent<MeshRenderer> ().material = nodeDefaultMaterial;
			
			}
//		

		


			GameObject[] allPrefabs;
			//killEmAll = GameObject.FindGameObjectsWithTag("demoPrefab");
			if (GameObject.FindGameObjectsWithTag ("demoPrefab") != null) {
				allPrefabs = GameObject.FindGameObjectsWithTag("demoPrefab");
				for (int i = 0; i < allPrefabs.Length; i++) {

					Destroy (allPrefabs[i]);

				}
			}

			if(this.startNode != null){
				this.startNode.objReference.GetComponent<MeshRenderer> ().material = nodeDefaultMaterial;
				this.startNode = null;

			}
		


//			if (this.endNode != null) {
//				endNode = null;
//				GameObject endGameObject = GameObject.Find ("EndNodeParticleSystem");
//
//				ParticleSystem ENParticle = endGameObject.GetComponent<ParticleSystem> ();
//				ENParticle.Stop ();
//			}

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			// Casts the ray and get the first game object hit
			Physics.Raycast(ray, out hit);

			Debug.Log("Start Node: " + hit.transform );
		
			this.startNode = findNodeInNodeList (int.Parse (hit.collider.name));
		
		
				

				GameObject startGameObject = GameObject.Find ("StartNodeParticleSystem");
				ParticleSystem SNParticle = startGameObject.GetComponent<ParticleSystem> ();

				ParticleSystem.EmissionModule emf = SNParticle.emission;
				emf.enabled = true;
				SNParticle.startSize = 0.7f;

				SNParticle.transform.localPosition = this.startNode.objReference.transform.localPosition;
			
				this.startNode.nodeParticleSystem = SNParticle;

				this.startNode.objReference.GetComponent<MeshRenderer> ().material = startNodeMaterial;


		}

		if (Input.GetMouseButtonDown (1)) {
			pathFound = false;
//			foreach (Node n in nodes) {
//				
//				if ((n.objReference.GetComponent<MeshRenderer> ().material != endNodeMaterial) ||
//				    (n.objReference.GetComponent<MeshRenderer> ().material != startNodeMaterial)) {
//					n.objReference.GetComponent<MeshRenderer> ().material = nodeDefaultMaterial;
//				} 
//
//			}

			foreach (Node n in nodes) {

				if (n == startNode) {
					n.objReference.GetComponent<MeshRenderer> ().material = startNodeMaterial;
					continue;
				}
				if (n == endNode) {
					continue;
				}

				n.objReference.GetComponent<MeshRenderer> ().material = nodeDefaultMaterial;
			}


			GameObject[] allPrefabs;
			//killEmAll = GameObject.FindGameObjectsWithTag("demoPrefab");
			if (GameObject.FindGameObjectsWithTag ("demoPrefab") != null) {
				allPrefabs = GameObject.FindGameObjectsWithTag("demoPrefab");
				for (int i = 0; i < allPrefabs.Length; i++) {

					Destroy (allPrefabs[i]);

				}
			}


			if(this.endNode != null){
				this.endNode.objReference.GetComponent<MeshRenderer> ().material = nodeDefaultMaterial;
				this.endNode = null;
			}
				
			foreach (Node n in nodes) {

				if (n.objReference.GetComponent<MeshRenderer>().material == startNodeMaterial) {
					n.objReference.GetComponent<MeshRenderer> ().material = nodeDefaultMaterial;
				}
//				else {
//					n.objReference.GetComponent<MeshRenderer> ().material = nodeDefaultMaterial;
//				}


			}

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			// Casts the ray and get the first game object hit
			Physics.Raycast(ray, out hit);

			Debug.Log("End Node: " + hit.transform );
			this.endNode = findNodeInNodeList(int.Parse(hit.collider.name));


			GameObject endGameObject = GameObject.Find ("EndNodeParticleSystem");
		
			ParticleSystem ENParticle = endGameObject.GetComponent<ParticleSystem> ();
			ENParticle.Play ();
			ParticleSystem.EmissionModule em2 = ENParticle.emission;
			em2.enabled = true;
			ENParticle.startSize = 0.7f;

			ENParticle.transform.localPosition = this.endNode.objReference.transform.localPosition;


			this.endNode.nodeParticleSystem = ENParticle;


			this.endNode.objReference.GetComponent<MeshRenderer> ().material = endNodeMaterial;

		}

	}


		
}



