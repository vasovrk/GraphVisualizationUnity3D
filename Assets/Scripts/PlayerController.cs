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
	private int[,] gridArray = new int[gridHeight, gridWidth];
	private Node startNode;
	private Node endNode;

	public Material nodeDefaultMaterial;
	public Material startNodeMaterial;
	public Material endNodeMaterial;
	public Material pathMaterial;
	public Material visitedMaterial;
	public Material neighbourMaterial;


	public GameObject demoPrefab;

	public Dictionary<NodeState, Color> materialMapper;
	void Start ()
	{
			


//			Vector3 v = new Vector3 (1.5f, -1.5f);
//			startNodePrefab.transform.localPosition = this.startNode.objReference.transform.localPosition;
//		//We find the particle system in the scene.
//		GameObject smokeParticle = GameObject.Find ("SmokeParticleSystem");
//		ParticleSystem psys = smokeParticle.GetComponent<ParticleSystem> ();
//
//		//We change the particle system's emission radius based on the grid size.
//		ParticleSystem.ShapeModule module = psys.shape;
//		int max_size = Math.Max (gridWidth, gridHeight);
//		module.radius = max_size / 2.0f;
//
//		//We change the particle system's emission rate based on the grid size.
//		ParticleSystem.EmissionModule emissionModule = psys.emission;
//		emissionModule.rate = new ParticleSystem.MinMaxCurve(emissionModule.rate.constantMax * max_size);

//		psys.startSpeed = 0.3f;
//		psys.startSize = (gridHeight + gridWidth) / 2;



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


				//deformSphere (sphere);
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
		float mSec = Time.time;
		foreach (Node node in nodes) {
			float animationOffset = node.animationOffset;
			float y = ((float)Math.Sin(mSec) * 0.05f) 
				+ ((float)Math.Cos((mSec * 1000f * animationOffset * 3.0) / 2000.0f) * 0.06f);

			Vector3 pos = node.objReference.transform.localPosition;
			pos.y = y;

			node.objReference.transform.localPosition = pos;

//			GameObject flame = GameObject.Find ("demoFlame");
//		    ParticleSystem flamePsys = flame.GetComponent<ParticleSystem> ();
//			flamePsys.emission.enabled = false;
//			Vector3 pos2 = flamePsys.transform.localPosition;
//			pos2.y = y;
//			flamePsys.transform.localPosition = pos2;


		}

		HandleMouseEvents();
		HandleKeyboardEvents ();
	}
	bool pathFound = false;
	private void FindPath() {
		    BFS2 bfs = new BFS2 (visitedMaterial,neighbourMaterial);
			LinkedList<Node> path = new LinkedList<Node> ();
			
		    path = bfs.findPath (startNode, endNode);
			path.AddLast (startNode);

		    pathFound = true;

			foreach (Node node in path) {
			demoPrefab = (GameObject)Instantiate (demoPrefab);
		
			demoPrefab.transform.localPosition = node.objReference.transform.localPosition;
			ParticleSystem PathParticle = demoPrefab.GetComponent<ParticleSystem> ();
			ParticleSystem.EmissionModule em = PathParticle.emission;
			em.enabled = true;
			PathParticle.startSize = 0.7f;
			node.objReference.GetComponent<MeshRenderer> ().material = pathMaterial;
				Debug.Log (node.NodeValue);
			}



	}

	private void HandleKeyboardEvents() {
		
		if (Input.GetKeyDown (KeyCode.F) && (pathFound == false)) {
			
			FindPath ();
		}
	}
	GameObject startNodePrefab;
	GameObject endNodePrefab;

	private void HandleMouseEvents() {
			

	
		    if(Input.GetMouseButtonDown(0)){
		//	SceneManager.LoadScene ("demScene");
//			Scene scene = SceneManager.GetSceneByName ("demScene");
//			SceneManager.SetActiveScene (scene);
		
//			foreach (Node n in nodes) {
//				if ((n.objReference.GetComponent<MeshRenderer> ().material.color != endNodeColor) && 
//					(n.objReference.GetComponent<MeshRenderer> ().material.color != startNodeColor)) {
//					n.objReference.GetComponent<MeshRenderer> ().material.color = defaultNodeColor;
//				}
//			}
		
			
			if(this.startNode != null){
				this.startNode.objReference.GetComponent<MeshRenderer> ().material = nodeDefaultMaterial;
				this.startNode = null;

			}

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			// Casts the ray and get the first game object hit
			Physics.Raycast(ray, out hit);

			Debug.Log("Start Node: " + hit.transform );
	
			this.startNode = findNodeInNodeList(int.Parse(hit.collider.name));
		
			GameObject startGameObject = GameObject.Find ("StartNodeParticleSystem");
			ParticleSystem SNParticle = startGameObject.GetComponent<ParticleSystem> ();

			ParticleSystem.EmissionModule em = SNParticle.emission;
			em.enabled = true;
			SNParticle.startSize = 0.7f;

			SNParticle.transform.localPosition = this.startNode.objReference.transform.localPosition;
//			GameObject flame = GameObject.Find ("demoFlame");
//					    ParticleSystem flamePsys = flame.GetComponent<ParticleSystem> ();
//						flamePsys.emission.enabled = false;
//						Vector3 pos2 = flamePsys.transform.localPosition;
		//	startNodePrefab = (GameObject)Instantiate (demoPrefab);
		//	Vector3 v = new Vector3 (1.5f, -1.5f);
		//	startNodePrefab.transform.localPosition = this.startNode.objReference.transform.localPosition;
		//	startNodeMaterial.SetColor ("_EmissionColor", new Color(0.9044118f,0.1795523f,0.7960993f));
			this.startNode.objReference.GetComponent<MeshRenderer> ().material = startNodeMaterial;
		//	this.startNode.objReference.GetComponent<MeshRenderer> ().material.color = new Color (1f, 0f, 0f);


		}

		if (Input.GetMouseButtonDown (1)) {

//			foreach (Node n in nodes) {
//				if ((n.objReference.GetComponent<MeshRenderer> ().material != endNodeMaterial) && 
//					(n.objReference.GetComponent<MeshRenderer> ().material != startNodeMaterial)) {
//					n.objReference.GetComponent<MeshRenderer> ().material = nodeDefaultMaterial;
//
//				}
//			}
			if(this.endNode != null){
				this.endNode.objReference.GetComponent<MeshRenderer> ().material = nodeDefaultMaterial;
				this.endNode = null;
			}


			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			// Casts the ray and get the first game object hit
			Physics.Raycast(ray, out hit);

			Debug.Log("End Node: " + hit.transform );
			this.endNode = findNodeInNodeList(int.Parse(hit.collider.name));
			this.endNode.objReference.GetComponent<MeshRenderer> ().material = endNodeMaterial;

			GameObject endGameObject = GameObject.Find ("EndNodeParticleSystem");
			ParticleSystem ENParticle = endGameObject.GetComponent<ParticleSystem> ();

			ParticleSystem.EmissionModule em2 = ENParticle.emission;
			em2.enabled = true;
			ENParticle.startSize = 0.7f;

			ENParticle.transform.localPosition = this.endNode.objReference.transform.localPosition;
		}
	}


		
}



