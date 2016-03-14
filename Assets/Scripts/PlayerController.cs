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
	private static readonly int gridHeight = 3;
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

	public BFS bfs;
	public DFS dfs;

	public GameObject demoPrefab;
	public GameObject d;
	public Dictionary<NodeState, Color> materialMapper;

	private GridAdjacencyCalc nodeAdjacency;
	void Start ()
	{
			
		bfs = new BFS (visitedMaterial,neighbourMaterial);
		dfs = new DFS (visitedMaterial, neighbourMaterial);

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

		nodeAdjacency = new GridAdjacencyCalc (gridArray,gridHeight,gridWidth,nodes);
		nodeAdjacency.adjacencyMap ();
		//TODO: startNode and endNode to be initialized on user interaction


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
	private void FindPath(String algorithm) {
		  
			LinkedList<Node> path = new LinkedList<Node> ();
		if (algorithm.Equals ("BFS")) {
			path = bfs.findPath (startNode, endNode);
		}
		if (algorithm.Equals ("DFS")) {
			path = dfs.findDFS (startNode,endNode);
		}
		//	path.AddLast (startNode);

		    pathFound = true;

		foreach (Node node in path) {
			
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
		
		if (Input.GetKeyDown (KeyCode.B) && (pathFound == false)) {
			
			FindPath ("BFS");
		}
		if (Input.GetKeyDown (KeyCode.D) && (pathFound == false)) {

			FindPath ("DFS");
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

			DestroyFlamePrefabs ();

			if(this.startNode != null){
				this.startNode.objReference.GetComponent<MeshRenderer> ().material = nodeDefaultMaterial;
				this.startNode = null;

			}
		

			var hit = castObject ();

			Debug.Log("Start Node: " + hit.transform );
		
			this.startNode = findNodeInNodeList (int.Parse (hit.collider.name));
		
		
				

				GameObject startGameObject = GameObject.Find ("StartNodeParticleSystem");
				ParticleSystem StartNodeParticle = startGameObject.GetComponent<ParticleSystem> ();

				ParticleSystem.EmissionModule StartNodeEmissionModule = StartNodeParticle.emission;
				StartNodeEmissionModule.enabled = true;
				StartNodeParticle.startSize = 0.7f;

				StartNodeParticle.transform.localPosition = this.startNode.objReference.transform.localPosition;
			
				this.startNode.nodeParticleSystem = StartNodeParticle;

				this.startNode.objReference.GetComponent<MeshRenderer> ().material = startNodeMaterial;


		}

		if (Input.GetMouseButtonDown (1)) {
			pathFound = false;

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


			DestroyFlamePrefabs ();


			if(this.endNode != null){
				this.endNode.objReference.GetComponent<MeshRenderer> ().material = nodeDefaultMaterial;
				this.endNode = null;
			}
				
			foreach (Node n in nodes) {

				if (n.objReference.GetComponent<MeshRenderer>().material == startNodeMaterial) {
					n.objReference.GetComponent<MeshRenderer> ().material = nodeDefaultMaterial;
				}



			}

			var hit = castObject ();

			Debug.Log("End Node: " + hit.transform );
			this.endNode = findNodeInNodeList(int.Parse(hit.collider.name));


			GameObject endGameObject = GameObject.Find ("EndNodeParticleSystem");
		
			ParticleSystem EndNodeParticleSystem = endGameObject.GetComponent<ParticleSystem> ();
			EndNodeParticleSystem.Play ();
			ParticleSystem.EmissionModule EndNodeEmissionModule = EndNodeParticleSystem.emission;
			EndNodeEmissionModule.enabled = true;
			EndNodeParticleSystem.startSize = 0.7f;

			EndNodeParticleSystem.transform.localPosition = this.endNode.objReference.transform.localPosition;


			this.endNode.nodeParticleSystem = EndNodeParticleSystem;


			this.endNode.objReference.GetComponent<MeshRenderer> ().material = endNodeMaterial;

		}

	}

	static void DestroyFlamePrefabs ()
	{
		GameObject[] allPrefabs;
		if (GameObject.FindGameObjectsWithTag ("demoPrefab") != null) {
			allPrefabs = GameObject.FindGameObjectsWithTag ("demoPrefab");
			for (int i = 0; i < allPrefabs.Length; i++) {
				Destroy (allPrefabs [i]);
			}
		}
	}

		
	static RaycastHit castObject ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		// Casts the ray and get the first game object hit
		Physics.Raycast (ray, out hit);
		return hit;
	}
}



