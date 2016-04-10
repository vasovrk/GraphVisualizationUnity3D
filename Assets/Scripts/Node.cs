using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node {

	private int nodeValue;

	private List<Node> neighbours;

	private List<Node> edges;

	private Node parent;

	private int nodeRow;

	private int nodeColumn;

	public GameObject objReference;

	public float animationOffset; 


	public Node(){

		animationOffset = Random.Range (0.0f, 1.0f);
		neighbours = new List<Node> ();
		edges = new List<Node> ();
	}

	public int NodeValue {
		get { return nodeValue; }

		set { this.nodeValue = value; }

	}

	public List<Node> Neighbours {
		get { return neighbours; }

		set { this.neighbours = value; }

	}

	public List<Node> Edges {
		get { return edges; }

		set { this.edges = value; }

	}

	public  Node Parent{
		get { return parent; }

		set { this.parent = value; }

	}

	public int NodeRow {
		get{ return nodeRow; }

		set{ nodeRow = value; }
	}

	public int NodeColumn{
		get{return  nodeColumn;}

		set {nodeColumn = value;}
	}

	public GameObject ObjReference {
		get{ return objReference; }

		set{ objReference = value; }
	}
		
}
