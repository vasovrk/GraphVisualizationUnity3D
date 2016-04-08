using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainSceneController : MonoBehaviour
{
	

	private Dictionary<int,List<Node>> adjMap;
	private List<Node> adjList;

	private List<Node> nodes = new List<Node> ();
	private static readonly int gridWidth = 8;
	private static readonly int gridHeight = 8;
	private int gridSize = gridWidth * gridHeight;
	private int[,] gridArray = new int[gridHeight, gridWidth];
	private Node startNode;
	private Node endNode;

	public Material nodeDefaultMaterial;
	public Material startNodeMaterial;
	public Material endNodeMaterial;
	public Material pathMaterial;
	public Material visitedMaterial;

	public GameObject endParticleSystem;
	public GameObject startParticleSystem;

	public BFS bfs;
	public DFS dfs;

	public GameObject flamePrefab;
	private GameObject flame;

	public Dictionary<NodeState, Color> materialMapper;

	private AdjacencyCalc nodeAdjacency;


	private GameObject startGameObject;
	ParticleSystem StartNodeParticle;
	ParticleSystem.EmissionModule StartNodeEmissionModule;

	private GameObject endGameObject;
	private ParticleSystem EndNodeParticleSystem;
	private ParticleSystem.EmissionModule EndNodeEmissionModule;

	void Start ()
	{
			
		bfs = new BFS (visitedMaterial);
		dfs = new DFS (visitedMaterial);

		startGameObject = GameObject.Find ("StartNodeParticleSystem");
		StartNodeParticle = startGameObject.GetComponent<ParticleSystem> ();

		StartNodeEmissionModule = StartNodeParticle.emission;

		endGameObject = GameObject.Find ("EndNodeParticleSystem");
		EndNodeParticleSystem = endGameObject.GetComponent<ParticleSystem> ();

		EndNodeEmissionModule = EndNodeParticleSystem.emission;



		int counter = 0;
	
		float offset = 0.5f;

		float halfWidth = gridWidth / 2.0f;
		float halfHeight = gridHeight / 2.0f;


		for (int i = 0; i < gridHeight; i++) {
			for (int j = 0; j < gridWidth; j++) {
				GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
				sphere.name = counter.ToString ();
				sphere.transform.localPosition = new Vector3 ((i - halfHeight) + offset, 0, (j - halfWidth) + offset);
				
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

		nodeAdjacency = new AdjacencyCalc (gridArray, gridHeight, gridWidth, nodes);
		nodeAdjacency.adjacencyMap ();

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


	void FixedUpdate ()
	{
		float mSec = Time.time;
		foreach (Node node in nodes) {
			float animationOffset = node.animationOffset;
			float y = ((float)Math.Sin (mSec) * 0.05f)
			          + ((float)Math.Cos ((mSec * 1000f * animationOffset * 3.0) / 2000.0f) * 0.06f);

			Vector3 pos = node.objReference.transform.localPosition;
			pos.y = y;

			node.objReference.transform.localPosition = pos;


		}

		HandleMouseEvents ();
		//HandleKeyboardEvents ();
	}

	bool pathFound = false;
	ParticleSystem PathParticle;

	private void FindPath (String algorithm)
	{
		  
		LinkedList<Node> path = new LinkedList<Node> ();
		if (algorithm.Equals ("BFS")) {
			path = bfs.findPath (startNode, endNode, false);
		}
		if (algorithm.Equals ("DFS")) {
			path = dfs.findDFS (startNode, endNode, false);

		}
			
		pathFound = true;

		foreach (Node node in path) {
			
			if (node.NodeValue == startNode.NodeValue) {
				node.objReference.GetComponent<MeshRenderer> ().material = pathMaterial;
				continue;
			}
			if (node.NodeValue == endNode.NodeValue) {
				node.objReference.GetComponent<MeshRenderer> ().material = pathMaterial;
				continue;
			
			}
			flame = (GameObject)Instantiate (flamePrefab);
			flame.transform.localPosition = node.objReference.transform.localPosition;
			PathParticle = flame.GetComponent<ParticleSystem> ();
			ParticleSystem.EmissionModule em = PathParticle.emission;
			em.enabled = true;
			PathParticle.startSize = 0.7f;
			node.objReference.GetComponent<MeshRenderer> ().material = pathMaterial;


		}



	}
	public void OnBFSbtnPressed(){
		if (pathFound == false) {
			FindPath ("BFS");
		}
	}

	public void OnDFSbtnPressed(){
		if (pathFound == false) {
			FindPath ("DFS");
		}
	}

	public void OnQuitBtnPressed(){
		Debug.Log ("Checking That Application will quit");
		Application.Quit ();
	}

	public void OnChangeSceneBtnPressed(){
		SceneManager.LoadScene ("SecondScene");	
	}
		


	private void HandleMouseEvents ()
	{
			

	
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit = castObject ();
			if (hit.collider == null || hit.collider.name.Equals("BFS") || hit.collider.name.Equals("DFS") || 
				hit.collider.name.Equals("ChangeScreen") || hit.collider.name.Equals("Quit") ){

				return;
			}
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

			if (this.startNode != null) {
				this.startNode.objReference.GetComponent<MeshRenderer> ().material = nodeDefaultMaterial;
				StartNodeEmissionModule.enabled = false;
				StartNodeParticle.startSize = 0;
				this.startNode = null;
			
			}
		


			Debug.Log ("Start Node: " + hit.transform);
		
			this.startNode = findNodeInNodeList (int.Parse (hit.collider.name));
		
		
				
				
			
			StartNodeEmissionModule.enabled = true;
			StartNodeParticle.startSize = 0.7f;

			StartNodeParticle.transform.localPosition = this.startNode.objReference.transform.localPosition;
			
			this.startNode.nodeParticleSystem = StartNodeParticle;

			this.startNode.objReference.GetComponent<MeshRenderer> ().material = startNodeMaterial;

		} 

		if (Input.GetMouseButtonDown (1)) {
			var hit = castObject ();
			if (hit.collider == null || hit.collider.name.Equals("BFS") || hit.collider.name.Equals("DFS") || 
				hit.collider.name.Equals("ChangeScreen") || hit.collider.name.Equals("Quit") ){

				return;
			}
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


			if (this.endNode != null) {
				this.endNode.objReference.GetComponent<MeshRenderer> ().material = nodeDefaultMaterial;
				EndNodeEmissionModule.enabled = false;
				EndNodeParticleSystem.startSize = 0;

				this.endNode = null;
			}
				

			Debug.Log ("End Node: " + hit.transform);
			this.endNode = findNodeInNodeList (int.Parse (hit.collider.name));

					
			EndNodeParticleSystem.Play ();
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
		if (GameObject.FindGameObjectsWithTag ("flamePrefab") != null) {
			allPrefabs = GameObject.FindGameObjectsWithTag ("flamePrefab");
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



