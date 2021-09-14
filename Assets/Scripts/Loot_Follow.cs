using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary> 
/// Behaviour of the loot
///</summary>
public class Loot_Follow : MonoBehaviour
{
    #region public variables
    [Header ("Customizable Features")]
    [SerializeField]
    private float min_modifier = 7;
    [SerializeField]
    private float max_modifier = 11;
    #endregion

    #region private variables
    private Transform target;
    private Vector3 _velocity = Vector3.zero;
    private bool _isFollowing = false;
    #endregion

    private void Awake()
    {
        target = GameObject.Find("player").transform.GetChild(0).transform;
    }

    public void StartFollowing()
    {
        transform.rotation = Random.rotation;
        _isFollowing = true;
    }

    void Update()
    {
        if (_isFollowing)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref _velocity, Time.deltaTime * Random.Range(min_modifier, max_modifier));
            transform.Rotate(Random.Range(0, 0.25f), Random.Range(0, 0.25f), Random.Range(0, 0.25f), Space.World);
        }
            
    }
}
