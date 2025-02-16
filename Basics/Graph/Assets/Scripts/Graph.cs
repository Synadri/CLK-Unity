using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefab;
    [SerializeField, Range(10, 1000)]
	int resolution = 10; // Default value = 10

    // On activation
    void Awake(){
        int cube_length = 2 ;
        float scale = (cube_length / (float) resolution);
        Vector3 position = Vector3.zero;
        for(int i = 0; i < resolution; i++){
            // Creates an instance of the point prefab
            Transform point = Instantiate(pointPrefab); // Returns a transform since prefab is one
            position.x = (float) (i*cube_length)/(resolution) + scale/2 - 1f; // Pos value into -1to1 range then shift by half size to right
            position.y = position.x * position.x * position.x; // y = x^3
            point.localPosition = position; 
            point.localScale = Vector3.one * scale;
            // Assign the new object as child of the graph (transform of the scene)
            point.SetParent(transform, false); // false => doesn't inherit parent's values
            
        }
    }

    void Update(){

    }
}
