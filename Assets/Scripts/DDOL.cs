using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DDOL : MonoBehaviour
{
    private static bool created = false;


    void Awake()
    {
		if (!created)
		{
			DontDestroyOnLoad(gameObject);
			created = true;
		}
    }
}
