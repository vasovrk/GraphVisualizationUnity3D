using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SecondSceneController : MonoBehaviour
{
	private LineRenderer lineRenderer;
	private float counter;
	private float distance;

	public Material startNodeMaterial;
	public Material defaultNodeMaterial;

	public GameObject nodePrefab;
	private GameObject n;
	public List<Node> nodes;

	public GameObject linePrefab;
	private GameObject line;

	private Node startNode;
	private ParticleSystem startNodeParticle;

	// Color -> number divided with 255, Color32 -> number between 0 and 255
	private Color32 defaultStartLineColor = new Color32 (187, 240, 36, 255);
	private Color32 defaultEndLineColor = new Color32 (11, 248, 71, 255);

	private GameObject instructions;
	private GameObject instructionsToggle;
	// Use this for initialization
	void Start ()
	{

		instructions = GameObject.FindGameObjectWithTag ("instructionsText");
		instructionsToggle = GameObject.FindGameObjectWithTag ("instructionsToggle");

		instructions.SetActive (false);
		nodes = new List<Node> ();


		Vector3[] positions = new Vector3[8];
		positions [0] = new Vector3 (0, 24, 0);
		positions [1] = new Vector3 (-24, 10, 0);
		positions [2] = new Vector3 (0, 10, 0);
		positions [3] = new Vector3 (24, 10, 0);
		positions [4] = new Vector3 (-8.5f, 0, 0);
		positions [5] = new Vector3 (8.5f, 0, 0);
		positions [6] = new Vector3 (38.5f, 0, 0);
		positions [7] = new Vector3 (8.5f, -10, 0);

		for (int i = 0; i < 8; i++) {
			n = (GameObject)Instantiate (nodePrefab);
			n.transform.localPosition = positions [i];
			Node node = new Node ();
			node.NodeValue = i + 1;
			node.objReference = n;
			nodes.Add (node);
		}

	
		nodes [0].Neighbours.Add (nodes [1]);
		nodes [0].Neighbours.Add (nodes [2]);
		nodes [0].Neighbours.Add (nodes [3]);


		nodes [1].Neighbours.Add (nodes [0]);
		nodes [1].Neighbours.Add (nodes [4]);

		nodes [2].Neighbours.Add (nodes [0]);
		nodes [2].Neighbours.Add (nodes [4]);

		nodes [3].Neighbours.Add (nodes [0]);
		nodes [3].Neighbours.Add (nodes [5]);
		nodes [3].Neighbours.Add (nodes [6]);

		nodes [4].Neighbours.Add (nodes [1]);
		nodes [4].Neighbours.Add (nodes [2]);
		nodes [4].Neighbours.Add (nodes [7]);

		nodes [5].Neighbours.Add (nodes [3]);
		nodes [5].Neighbours.Add (nodes [7]);

		nodes [6].Neighbours.Add (nodes [3]);
		nodes [6].Neighbours.Add (nodes [7]);

		nodes [7].Neighbours.Add (nodes [4]);
		nodes [7].Neighbours.Add (nodes [5]);
		nodes [7].Neighbours.Add (nodes [6]);

		lineRenderer = GetComponent<LineRenderer> ();

		int counter = 1;
		foreach (Node node in nodes) {
			node.objReference.name = counter.ToString ();
			counter++;
		}
		startNode = nodes[0];

		GameObject startGameObject = GameObject.Find ("StartNodeParticleSystem");
		startNodeParticle = startGameObject.GetComponent<ParticleSystem> ();
		startNodeParticle.transform.localPosition = nodes[0].objReference.transform.localPosition;
		ParticleSystem.EmissionModule em = startNodeParticle.emission;
		em.enabled = true;
		startNodeParticle.startSize = 3.0f;
		startNodeParticle.Play ();
		nodes[0].objReference.GetComponent<MeshRenderer> ().material = startNodeMaterial;

		drawEdges ();

	}

	private bool spanFound;
	// Update is called once per frame
	void FixedUpdate ()
	{
		
		if (Input.GetMouseButtonDown (0)) {
			
			var hit = castObject ();
			//Debug.Log (hit.collider.tag.ToString ());
			if (hit.collider == null || hit.collider.name.Equals ("BfsSpanTreeButton") || hit.collider.name.Equals ("DfsSpanTreeButton") ||
			    hit.collider.name.Equals ("ChangeScreen") || hit.collider.name.Equals ("Quit")) {

				return;
			}

			spanFound = false;
			GameObject[] allLines;
			if (GameObject.FindGameObjectsWithTag ("linePrefab") != null) {
				allLines = GameObject.FindGameObjectsWithTag ("linePrefab");
				for (int i = 0; i < allLines.Length; i++) {
					allLines [i].GetComponent<LineRenderer> ().SetColors (defaultStartLineColor, defaultEndLineColor);
				}
			}

			foreach (Node node in nodes) {
				node.objReference.GetComponent<MeshRenderer> ().material = defaultNodeMaterial;
				if (node.Edges.Count != 0) {
					node.Edges.Clear ();
				}
			}

			if (startNode != null) {
				startNode.objReference.GetComponent<MeshRenderer> ().material = defaultNodeMaterial;
				startNode = null;

			}


			startNode = findNodeInNodeList (int.Parse (hit.collider.name));
			startNodeParticle.transform.localPosition = startNode.objReference.transform.localPosition;
			startNode.objReference.GetComponent<MeshRenderer> ().material = startNodeMaterial;
		}
			
	}


	private List<Node> visited;

	private void drawEdges ()
	{
		visited = new List<Node> ();

		for (int i = 0; i < nodes.Count; i++) {
			Node currentNode = nodes [i];
			List<Node> neighbours = nodes [i].Neighbours;
			for (int j = 0; j < neighbours.Count; j++) {
				Node neighbour = neighbours [j];
				//If the neighbour exists in the visited list it means that the line has already been drawn
				if (!visited.Contains (neighbour)) {
					
					line = GameObject.Instantiate (linePrefab) as GameObject;

					line.GetComponent<LineRenderer> ().SetWidth (.45f, .45f);
					line.GetComponent<LineRenderer> ().SetPosition (0, currentNode.objReference.transform.position);

					line.GetComponent<LineRenderer> ().SetPosition (1, neighbour.objReference.transform.position);
					line.GetComponent<LineRenderer> ().name = currentNode.NodeValue + ":" + neighbour.NodeValue;

				}
			
			}
			visited.Add (currentNode);
		}
	}



	public void DrawSpannTreeWithBFS ()
	{
		
		if (startNode != null && spanFound == false) {
			BFS bfs = new BFS ();

			bfs.findPath (startNode, nodes [7], true);
			DrawSpanTreeEdges ();
			spanFound = true;
		}


	}

	public void DrawSpannTreeWithDFS ()
	{

		if (startNode != null && spanFound == false) {
			DFS dfs = new DFS ();

			dfs.findDFS (startNode, nodes [7], true);
			DrawSpanTreeEdges ();
			spanFound = true;
		}


	}

	private void DrawSpanTreeEdges ()
	{
		Color startColor = new Color (0.043137255f, 0.20392157f, 0.97254902f, 1f);
		Color endColor = new Color (0.88235294f, 0.14117647f, 0.82745098f, 1f);
		foreach (Node node in nodes) {
			node.objReference.GetComponent<MeshRenderer> ().material = startNodeMaterial;
			foreach (Node edge in node.Edges) {
				GameObject line = GameObject.Find (Mathf.Min (node.NodeValue, edge.NodeValue).ToString () + ":" + Mathf.Max (node.NodeValue, edge.NodeValue).ToString ());
				lineRenderer = line.GetComponent<LineRenderer> ();
				lineRenderer.SetColors (startColor, endColor);
			}
		}
	}

	private static RaycastHit castObject ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		// Casts the ray and get the first game object hit
		Physics.Raycast (ray, out hit);
		return hit;
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

	public void OnQuitBtnPressed ()
	{
		Debug.Log ("skatoules");
		Application.Quit ();
	}

	public void OnChangeSceneBtnPressed ()
	{
		SceneManager.LoadScene ("MainScene");	
	}

	public void instructionsVisible ()
	{

		if (instructionsToggle.GetComponent<Toggle> ().isOn) {
			instructions.SetActive (true);
		} else if (!instructionsToggle.GetComponent<Toggle> ().isOn) {
			instructions.SetActive (false);
		}
		Debug.Log ("something");
	}

}
