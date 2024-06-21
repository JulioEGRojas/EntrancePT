using System.Collections;
using UnityEngine;

public class LaserSpawner : Spawner<Laser> {

    [Header("Spawn control")]
    /// <summary>
    /// Area (x,y) where the lasers can spawn
    /// </summary>
    [SerializeField] private Bounds spawnBounds;

    /// <summary>
    /// Height at which the laser is spawned
    /// </summary>
    [SerializeField] private float laserHeight;

    /// <summary>
    /// Time between the generation of the lasers
    /// </summary>
    [SerializeField] private float generationCooldown = 3f;
    
    /// <summary>
    /// Number of rays to generate
    /// </summary>
    [SerializeField] private int raysToGenerate = 3;

    /// <summary>
    /// Grid used as reference to aim the lasers correctly
    /// </summary>
    [Header("Gridlike Generation")]
    [SerializeField] private Grid grid;

    /// <summary>
    /// Reference to the player. To aim some lasers at them.
    /// </summary>
    [Header("Player")]
    [SerializeField] private Transform target;

    private void Start() {
        // Coroutines of laser and tracked laser generation are started
        StartCoroutine(GenerationCoroutine());
        StartCoroutine(TrackedLaserGeneration());
    }

    private IEnumerator GenerationCoroutine() {
        while (true) {
            // We wait before generating, so player has some time to react.
            yield return new WaitForSeconds(generationCooldown);
            for (int i = 0; i < raysToGenerate; i++) {
                // Create a laser and move it up at the requested distance.
                Laser newLaser = Generate();
                newLaser.transform.position = MathExtensions.RandomPointInBounds(spawnBounds);
                newLaser.transform.Translate(Vector3.up * laserHeight);
                // When aiming the laser, move it to the closest grid point, like in the video.
                Vector3 positionInGrid = MathExtensions.RandomPointInBounds(spawnBounds);
                positionInGrid = grid.GetCellCenterWorld(grid.WorldToCell(positionInGrid));
                newLaser.SetTarget(positionInGrid);
                newLaser.gameObject.SetActive(true);
            }
        }
    }
    
    /// <summary>
    /// Generates lasers that aim to the palyer each 5 seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator TrackedLaserGeneration() {
        // Same as generating a laser, we only point it at the player. Needs refactor, but time is ticking.
        while (true) {
            yield return new WaitForSeconds(5f);
            Laser newLaser = Generate();
            newLaser.transform.position = MathExtensions.RandomPointInBounds(spawnBounds);
            newLaser.transform.Translate(Vector3.up * laserHeight);
            newLaser.SetTarget(target.position);
            newLaser.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Sets the number of rays to generate on each generation cycle. Used to increase rays generated when time changes.
    /// </summary>
    /// <param name="raysToGenerate"></param>
    public void SetRaysToGenerate(int raysToGenerate) {
        this.raysToGenerate = raysToGenerate;
    }
}
