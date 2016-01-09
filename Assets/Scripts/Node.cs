using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node {
	private int value; 

	private List<int> neighbours;

	private float weight;

	private int nodeRow;

	private int nodeColumn;


	public Node(){
		neighbours = new List<int> ();
	
	}

	public List<int> Neighbours {
		get { return neighbours; }

		set { neighbours = value; }
	}


	public int Value {
		get { return value; }

		set { this.value = value; }

	}


	public float Weight {
		get { return weight; }

		set { weight = value; }
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
