using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class River : MonoBehaviour {

	public int flowSpeed;
	public int spawnRate;
	public Bounds riverBounds;
	public ObstaclePool obstaclePool;

	private List<Obstacle> aliveObstacles = new List<Obstacle>();

	void Start()
	{
		StartCoroutine (SpawnObstacles ());
		StartCoroutine (MoveObstacles ());
	}

	void Update()
	{
		MoveObstacles ();
		CullObstacles ();
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube ( transform.TransformPoint(riverBounds.center), riverBounds.size);
	}

	// Create new obs
	private IEnumerator SpawnObstacles() {
		while (true) {
			yield return new WaitForSeconds( spawnRate );

			// Spawn a new obstacle
			var obstacle = obstaclePool.GetObstacle( "Test" );
			obstacle.transform.position = transform.TransformPoint( riverBounds.center );

			// Put it at the back (+z) of the river bounding box
			obstacle.transform.Translate( 0, 0,  riverBounds.extents.z );
		}
	}

	private void CullObstacles() {
	}

	private IEnumerator MoveObstacles() {
		while (true) {
			foreach (var obstacle in aliveObstacles) {
				// Move towards -z
				obstacle.transform.Translate (0, 0, -flowSpeed * Time.deltaTime);
			}
			yield return null;
			
		}
	}
}
