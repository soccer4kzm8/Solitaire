using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustPosition : MonoBehaviour
{
    [SerializeField] private Transform _followedObj = null;
    void Start()
    {
        gameObject.transform.position = new Vector3(_followedObj.position.x,  gameObject.transform.position.y, _followedObj.position.z);
    }
}
