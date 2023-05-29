using System.Collections.Generic;
using UnityEngine;

public class UniqueDontDestroyOnLoad : MonoBehaviour
{
    private static Dictionary<string, UniqueDontDestroyOnLoad> instances = new Dictionary<string, UniqueDontDestroyOnLoad>();

    void Awake()
    {
        if (instances.ContainsKey(gameObject.name) && instances[gameObject.name] == null)
        {
            instances.Remove(gameObject.name);
        }
        if (!instances.ContainsKey(gameObject.name))
        {
            instances.Add(gameObject.name, this);
            DontDestroyOnLoad(gameObject);
        }
        else if (instances[gameObject.name] != this)
        {
            Destroy(gameObject);
        }
    }
}
