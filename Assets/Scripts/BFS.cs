using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BFS  {

	private Dictionary<int, List<int>> adjMap;
	private List<int> visitedList;

	public BFS(Dictionary<int,List<int>> adjMap) {
		this.adjMap = adjMap;
//		List<int> neighbours;
//		neighbours = new List<int> ();
//		neighbours.Add (1);
//		neighbours.Add (2);
//		adjMap.Add (0, neighbours);
//
//		neighbours = new List<int> ();
//		neighbours.Add (3);
//		neighbours.Add (4);
//		adjMap.Add (1, neighbours);
//
//		neighbours = new List<int> ();
//		neighbours.Add (5);
//		neighbours.Add (6);
//		adjMap.Add (2, neighbours);
//
//		neighbours = new List<int> ();
//		adjMap.Add (3, neighbours);
//
//		neighbours = new List<int> ();
//		adjMap.Add (4, neighbours);
//
//		neighbours = new List<int> ();
//		adjMap.Add (5, neighbours);
//
//		neighbours = new List<int> ();
//		adjMap.Add (6, neighbours);

		this.visitedList = new List<int>();
	}

	public void findBFS(int rootNode) {
		Debug.Log ("starting now");
		foreach (int key in adjMap.Keys) {
			Debug.Log (key);
			List<int> neighbours = new List<int> ();
			adjMap.TryGetValue (key, out neighbours);
			findShortest (neighbours);
//			foreach (int neighbour in neighbours) {
//				Debug.Log ("neighbours :" + neighbour);
//			}
		}
		//Debug.Log (adjMap);
		Queue<int> q = new Queue<int>();
		q.Enqueue (rootNode);

		visitedList.Add(rootNode);

		while (!(q.Count == 0)) {
			int n = q.Dequeue();
			Debug.Log ("new bfs path:" + n);
			List<int> nodeAdjlist = new List<int>();
			adjMap.TryGetValue(n, out nodeAdjlist);
			Debug.Log ("adjList size iiiiissss:" + nodeAdjlist.Count);

			foreach (int x in nodeAdjlist) {
				if (!isVisited(x, visitedList)) {
					q.Enqueue(x);
					visitedList.Add(x);
				}
			}
		}

		foreach (int i in q) {
			Debug.Log ("Bfs");
			Debug.Log (i);
		}

		//    for(int i : visitedList){
		//        System.out.print(i+":");
		//    }
	}

	private bool isVisited(int k, List<int> visitedList) {
		bool res = false;
		for (int i = 0; i < visitedList.Count; i++) {
			if (visitedList[i] == k) {
				res = true;
			}
		}
		return res;
	}

	private int findShortest(List<int> neighbours){
		int shortest = neighbours[0];
		foreach (int neighbour in neighbours) {
			if (neighbour < shortest) {
				shortest = neighbour;
			}
		
		}
		return shortest;
	}
}

