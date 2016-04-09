using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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


	private AdjacencyCalc nodeAdjacency;


	private GameObject startGameObject;
	ParticleSystem StartNodeParticle;
	ParticleSystem.EmissionModule StartNodeEmissionModule;

	private GameObject endGameObject;
	private ParticleSystem EndNodeParticleSystem;
	private ParticleSystem.EmissionModule EndNodeEmissionModule;

	private GameObject instructions;
	private GameObject instructionsToggle;
	void Start ()
	{
		instructions = GameObject.FindGameObjectWithTag("instructionsText");
		instructionsToggle = GameObject.FindGameObjectWithTag("instructionsToggle");

		instructions.SetActive (false);
		//We pass the visited material in BDF,DFS classes so that the material of the node changes when it's visited by the algorithm
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
				sphere.transform.localPosition = new Vector3 ((j - halfWidth) + offset, 0, (i - halfHeight) + offset);
				
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
			//The animationOffset variable that is stored in Node class, 
			//is a random number that makes every node to move randomly up and down
			//in the y axis, otherwise all the nodes would be equally moving at the same time.
			float animationOffset = node.animationOffset;
			float y = ((float)Math.Sin (mSec) * 0.05f)
			          + ((float)Math.Cos ((mSec * 1000f * animationOffset * 3.0) / 2000.0f) * 0.06f);

			Vector3 pos = node.objReference.transform.localPosition;
			pos.y = y;

			node.objReference.transform.localPosition = pos;


		}

		HandleMouseEvents ();
	}

	private bool pathFound = false;
	private ParticleSystem PathParticle;

	private void FindPath (String algorithm)
	{
		//The path with the selected algorithm is being found - a list of Node objects
		LinkedList<Node> path = new LinkedList<Node> ();
		if (algorithm.Equals ("BFS")) {
			path = bfs.findPath (startNode, endNode, false);
		}
		if (algorithm.Equals ("DFS")) {
			path = dfs.findDFS (startNode, endNode, false);

		}
			
		pathFound = true;

		foreach (Node node in path) {
			//If the current node is a startNode or an endNode, we don't want to change appearance
			if (node.NodeValue == startNode.NodeValue) {
				node.objReference.GetComponent<MeshRenderer> ().material = pathMaterial;
				continue;
			}
			if (node.NodeValue == endNode.NodeValue) {
				node.objReference.GetComponent<MeshRenderer> ().material = pathMaterial;
				continue;
			
			}
			//That way we create a flame prefab with the appropriate particle, material and position
			flame = (GameObject)Instantiate (flamePrefab);
			flame.transform.localPosition = node.objReference.transform.localPosition;
			PathParticle = flame.GetComponent<ParticleSystem> ();
			ParticleSystem.EmissionModule em = PathParticle.emission;
			em.enabled = true;
			PathParticle.startSize = 0.7f;
			node.objReference.GetComponent<MeshRenderer> ().material = pathMaterial;


		}



	}

	public void OnBFSbtnPressed ()
	{
		if (pathFound == false) {
			FindPath ("BFS");
		}
	}

	public void OnDFSbtnPressed ()
	{
		if (pathFound == false) {
			FindPath ("DFS");
		}
	}

	public void OnQuitBtnPressed ()
	{
		Debug.Log ("Checking That Application will quit");
		Application.Quit ();
	}

	public void OnChangeSceneBtnPressed ()
	{
		SceneManager.LoadScene ("SecondScene");	
	}



	private void HandleMouseEvents ()
	{
			
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit = castObject ();
			//If the collider is null or it's name is equal with some other object except for a sphere in the grid, then we don't want to set the startNode variable to be equal with it.
			if (hit.collider == null || hit.collider.name.Equals ("BFS") || hit.collider.name.Equals ("DFS") ||
			    hit.collider.name.Equals ("ChangeScreen") || hit.collider.name.Equals ("Quit")) {

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
			//The flame prefabs are being destroyed. The grid cleans up.
			DestroyFlamePrefabs ();

			//If startNode is previously selected, we "reset" it by stoping the startNodeParticle emission, setting the mode's material to default and finally setting the startNode variable to be equal with null.
			if (this.startNode != null) {
				this.startNode.objReference.GetComponent<MeshRenderer> ().material = nodeDefaultMaterial;
				StartNodeEmissionModule.enabled = false;
				StartNodeParticle.startSize = 0;
				this.startNode = null;
			
			}
		
			Debug.Log ("Start Node: " + hit.transform);

			//The new startNode is found in the list of total nodes and we make the apropriate changes for the particle system and material
			this.startNode = findNodeInNodeList (int.Parse (hit.collider.name));
		
		
			StartNodeEmissionModule.enabled = true;
			StartNodeParticle.startSize = 0.7f;

			StartNodeParticle.transform.localPosition = this.startNode.objReference.transform.localPosition;
			
			this.startNode.nodeParticleSystem = StartNodeParticle;

			this.startNode.objReference.GetComponent<MeshRenderer> ().material = startNodeMaterial;

		} 

		//Same thing for the endNode selection
		if (Input.GetMouseButtonDown (1)) {
			var hit = castObject ();
			if (hit.collider == null || hit.collider.name.Equals ("BFS") || hit.collider.name.Equals ("DFS") ||
			    hit.collider.name.Equals ("ChangeScreen") || hit.collider.name.Equals ("Quit")) {

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

	//Casts a ray against all colliders in the scene and returns detailed information on what was hit.	
	static RaycastHit castObject ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		// Casts the ray and get the first game object hit
		Physics.Raycast (ray, out hit);
		return hit;
	}

	public void instructionsVisible(){

		if (instructionsToggle.GetComponent<Toggle> ().isOn) {
			instructions.SetActive (true);
		} else if(!instructionsToggle.GetComponent<Toggle> ().isOn){
			instructions.SetActive (false);
		}
		Debug.Log ("something");
	}


}



