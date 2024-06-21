using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour {

    [SerializeField] protected T generatedSample;

    protected virtual T Generate() {
        return Instantiate(generatedSample);
    }
}