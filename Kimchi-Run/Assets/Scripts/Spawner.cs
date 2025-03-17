using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("References")]
    public GameObject[] gameObjects;    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Spawn() {
        GameObject randomObject = gameObjects[Random.Range(0, gameObjects.Length)];
        Instantiate(randomObject, transform.position, Quaternion.identity);
    }
}
