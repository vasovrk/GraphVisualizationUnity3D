using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node2 {
	private int nodeValue;

	private List<Node2> neighbours;

	private Node2 parent;

	private int nodeRow;

	private int nodeColumn;

	public Node2(){

		neighbours = new List<Node2> ();
	}

	public int NodeValue {
		get { return nodeValue; }

		set { this.nodeValue = value; }

	}

	public List<Node2> Neighbours {
		get { return neighbours; }

		set { this.neighbours = value; }

	}

	public  Node2 Parent{
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

}
