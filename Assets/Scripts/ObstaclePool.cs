using UnityEngine;
using System;
using System.Collections.Generic;

public class ObstaclePool : MonoBehaviour {

	public GameObject[] obstaclePrefabs;

	void Awake() {
		// Cache the obstacle names for fast lookup
		var obstaclesComponents = Array.ConvertAll<GameObject,Obstacle> (obstaclePrefabs, prefab =>{
			var obs = prefab.GetComponent<Obstacle> ();
			if( obs==null ) { Debug.LogErrorFormat( "Obstacle prefab %s doesn't contains an Obstacle Component.", prefab.name ); }
			return obs;
		});
		obstaclesComponents = Array.FindAll (obstaclesComponents, oc => oc != null);
		obstacleNames = Array.ConvertAll<Obstacle,string> (obstaclesComponents, oc => oc.obstacleName);
	}

	public Obstacle GetObstacle( string name ) {
		if (obstacleMap.ContainsKey (name)) {
			// We have a stack of obstacles with this name.
			var obstacleList = obstacleMap [name];

			if (obstacleList.Count != 0) {
				// We have a spare on the stack, pop it off.
				return obstacleList.Pop();
			}
		}

		// Nothing on the stack or no stack, create a new one.
		return CreateObstacle (name);
	}

	public void ReturnObstacle( Obstacle obstacle )
	{
		if (!obstacleMap.ContainsKey (obstacle.obstacleName)) {
			// Create a new stack for the first obstacle with this name
			obstacleMap.Add ( obstacle.obstacleName, new Stack<Obstacle>() );
		}

		// Store the obstacle for later
		obstacleMap [name].Push( obstacle );
	}

	private Obstacle CreateObstacle( string name ) {
		int index = Array.FindIndex (obstacleNames, on => on == name);
		if (index == -1) {
			Debug.LogError( "No obstacle prefab found with name " + name );
			return null;
		}
		return Instantiate (obstaclePrefabs [index]).GetComponent<Obstacle>();
	}

	private string[] obstacleNames;
	private Dictionary<string, Stack<Obstacle>> obstacleMap = new Dictionary<string, Stack<Obstacle>>();

}
