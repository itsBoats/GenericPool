public class Example : MonoBehaviour {

	// Make sure to set the pool prefab in the inspector
	public Pool<Transform> lemons = new Pool<Transform>();
	public Pool<Enemy> enemies = new Pool<Enemy>();

	public void Awake() {
		// Priming the pool instantiates deactivated copies of the prefab
		// It's not required, but recommended if you know you are going to need a large amount of them
		enemies.Prime(10);
	}

	public void SpawnEnemies(int count) {
		for (int i = 0; i < count; i++) {
			// Spawn enemy randomly on the screen 
			Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width+1), Random.Range(0, Screen.height+1), 0));

			// ScreenToWorldPoint z value is always the camera z value (-10 by default for 2D)
			spawnPosition.z = 0;

			// Get an enemy from our pool
			Enemy e = enemies.GetObject(spawnPosition);

			// Make sure to reset necessary variables after you get them from the pool
			e.health = 100;
		}
	}

	public void KillEnemy(Enemy enemy) {
		// Make sure to use gameObject.SetActive(false) instead of Destroy(gameObject) for pooled objects;
		enemy.gameObject.SetActive(false);

		// Spawn a lemon where our enemy died
		lemons.GetObject(enemy.transform.position);
	}

	public void CleanupLevel() {
		// Don't need the pool anymore or changing levels?
		enemies.Clear();
		lemons.Clear();


		// HideAll() just deactivates everything in the pool (gameObject.SetActive(false))
		// If you want to keep the pool even after changing levels, Mark the pool AND pooled objects with DontDestroyOnLoad() and use HideAll() INSTEAD OF Clear()
		enemies.HideAll();
		lemons.HideAll();
	}
}
