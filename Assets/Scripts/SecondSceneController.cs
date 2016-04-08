using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SecondSceneController : MonoBehaviour
{
	private LineRenderer lineRenderer;
	private float counter;
	private float dist;

	public Transform origin;
	public Transform destination;
	public Transform destination2;
	public float lineDrawSpeed = 6f;

	public Material startNodeMaterial;
	public Material defaultNodeMaterial;

	public GameObject nodePrefab;
	private GameObject n;
	public List<Node> nodes;

	public GameObject linePrefab;
	private GameObject l;

	public GameObject button;
	private Node startNode;
	private ParticleSystem startNodeParticle;

	private Color32 defaultStartLineColor = new Color32 (187, 240, 36, 255);
	private Color32 defaultEndLineColor = new Color32 (11, 248, 71, 255);
	// Use this for initialization
	void Start ()
	{
		nodes = new List<Node> ();

		n = (GameObject)Instantiate (nodePrefab);
		n.transform.localPosition = new Vector3 (0, 24, 0);

		GameObject startGameObject = GameObject.Find ("StartNodeParticleSystem");
		startNodeParticle = startGameObject.GetComponent<ParticleSystem> ();
		startNodeParticle.transform.localPosition = n.transform.localPosition;
		ParticleSystem.EmissionModule em = startNodeParticle.emission;
		em.enabled = true;
		startNodeParticle.startSize = 3.0f;
		startNodeParticle.Play ();

//		Vector3[] positions;
//		positions [0] = new Vector3 (-24, 10, 0);
//		positions [1] = new Vector3 (0, 10, 0);
//		positions [2] = new Vector3 (24, 10, 0);
//		positions [3] = new Vector3 (-8.5f, 0, 0);
//		positions [4] = new Vector3 (8.5f, 0, 0);
//		positions [5] = new Vector3 (38.5f, 0, 0);
//		positions [6] = new Vector3 (8.5f, -10, 0);
//
//
//		for (int i = 0; i <= 6; i++) {
//			n = (GameObject)Instantiate (nodePrefab);
//			n.transform.localPosition = positions[i];
//			Node node =new Node ();
//			node = new Node ();
//			node.NodeValue = 2;
//			node.objReference = n;
//			nodes.Add (node);
//		}
		Node node1;
		node1 = new Node ();
		node1.NodeValue = 1;
		node1.objReference = n;
		nodes.Add (node1);
		node1.objReference.GetComponent<MeshRenderer> ().material = startNodeMaterial;
		startNode = node1;

		n = (GameObject)Instantiate (nodePrefab);
		n.transform.localPosition = new Vector3 (-24, 10, 0);
		Node node2;
		node2 = new Node ();
		node2.NodeValue = 2;
		node2.objReference = n;
		nodes.Add (node2);

		n = (GameObject)Instantiate (nodePrefab);
		n.transform.localPosition = new Vector3 (0, 10, 0);
		Node node3;
		node3 = new Node ();
		node3.NodeValue = 3;
		node3.objReference = n;
		nodes.Add (node3);
//
		n = (GameObject)Instantiate (nodePrefab);
		n.transform.localPosition = new Vector3 (24, 10, 0);
		Node node4;
		node4 = new Node ();
		node4.NodeValue = 4;
		node4.objReference = n;
		nodes.Add (node4);

		n = (GameObject)Instantiate (nodePrefab);
		n.transform.localPosition = new Vector3 (-8.5f, 0, 0);
		Node node5;
		node5 = new Node ();
		node5.NodeValue = 5;
		node5.objReference = n;
		nodes.Add (node5);

		n = (GameObject)Instantiate (nodePrefab);
		n.transform.localPosition = new Vector3 (8.5f, 0, 0);
		Node node6;
		node6 = new Node ();
		node6.NodeValue = 6;
		node6.objReference = n;
		nodes.Add (node6);

		n = (GameObject)Instantiate (nodePrefab);
		n.transform.localPosition = new Vector3 (38.5f, 0, 0);
		Node node7;
		node7 = new Node ();
		node7.NodeValue = 7;
		node7.objReference = n;
		nodes.Add (node7);

		n = (GameObject)Instantiate (nodePrefab);
		n.transform.localPosition = new Vector3 (8.5f, -10, 0);
		Node node8;
		node8 = new Node ();
		node8.NodeValue = 8;
		node8.objReference = n;
		nodes.Add (node8);


		node1.Neighbours.Add (node2);
		node1.Neighbours.Add (node3);
		node1.Neighbours.Add (node4);
		node2.Neighbours.Add (node1);
		node2.Neighbours.Add (node5);
		node3.Neighbours.Add (node1);
		node3.Neighbours.Add (node5);
		node4.Neighbours.Add (node1);
		node4.Neighbours.Add (node6);
		node4.Neighbours.Add (node7);
		node5.Neighbours.Add (node2);
		node5.Neighbours.Add (node3);
		node5.Neighbours.Add (node8);
		node6.Neighbours.Add (node4);
		node6.Neighbours.Add (node8);
		node7.Neighbours.Add (node4);
		node7.Neighbours.Add (node8);
		node8.Neighbours.Add (node5);
		node8.Neighbours.Add (node6);
		node8.Neighbours.Add (node7);

//		dist = Vector3.Distance (nodes[0].objReference.transform.position, nodes[1].objReference.transform.position);
		lineRenderer = GetComponent<LineRenderer> ();
//		lineRenderer.SetPosition (0, nodes[0].objReference.transform.position);
//		lineRenderer.SetWidth (.45f, .45f);
//		lineRenderer.SetVertexCount(20);
		int counter = 1;
		foreach (Node noda in nodes) {
			noda.objReference.name = counter.ToString ();
			counter++;
		}

		drawEdges ();
		startNode = node1;
	}

	private bool spanFound;
	// Update is called once per frame
	void FixedUpdate ()
	{
		
		if (Input.GetMouseButtonDown (0)) {
			
			var hit = castObject ();
			Debug.Log (hit.collider.tag.ToString ());
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

	GameObject line;
	private List<Node> visited;

	private void drawEdges ()
	{
		visited = new List<Node> ();
		//line = linePrefab.GetComponent<LineRenderer> ();
		for (int i = 0; i < nodes.Count; i++) {
			Node parent = nodes [i];
			List<Node> neighbours = nodes [i].Neighbours;
			for (int j = 0; j < neighbours.Count; j++) {
				Node neighbour = neighbours [j];
				if (!visited.Contains (neighbour)) {
					dist = Vector3.Distance (parent.objReference.transform.position, neighbour.objReference.transform.position);
					float lineDistance = Mathf.Lerp (0, dist, dist);
					Vector3 segmentVector = lineDistance * Vector3.Normalize (neighbour.objReference.transform.position - parent.objReference.transform.position) + parent.objReference.transform.position;
					l = GameObject.Instantiate (linePrefab) as GameObject;
					//  l = linePrefab.GetComponent<LineRenderer> ();
					l.GetComponent<LineRenderer> ().SetWidth (.45f, .45f);
					l.GetComponent<LineRenderer> ().SetPosition (0, parent.objReference.transform.position);

					l.GetComponent<LineRenderer> ().SetPosition (1, segmentVector);
					l.GetComponent<LineRenderer> ().name = parent.NodeValue + ":" + neighbour.NodeValue;

				}
			
			}
			visited.Add (parent);
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

	void DrawSpanTreeEdges ()
	{
		Color startColor = new Color (0.043137255f, 0.20392157f, 0.97254902f, 1f);
		Color endColor = new Color (0.88235294f, 0.14117647f, 0.82745098f, 1f);
		foreach (Node node in nodes) {
			node.objReference.GetComponent<MeshRenderer> ().material = startNodeMaterial;
			foreach (Node edge in node.Edges) {
				GameObject line = GameObject.Find (Mathf.Min (node.NodeValue, edge.NodeValue).ToString () + ":" + Mathf.Max (node.NodeValue, edge.NodeValue).ToString ());
				//	Debug.Log ("the line is:" + line.name);
				lineRenderer = line.GetComponent<LineRenderer> ();
				lineRenderer.SetColors (startColor, endColor);
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
}
