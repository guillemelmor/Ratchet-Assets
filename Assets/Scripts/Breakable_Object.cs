using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary> 
/// It manages the affected objects by the explosion which can be breakable
///</summary>
public class Breakable_Object : MonoBehaviour
{
    #region public variables
    [Header ("References")]
    [SerializeField]
    private GameObject original_object;                     // ** object composed by only one piece
    [SerializeField]
    private GameObject fractured_object;                    // ** object composed by several pieces
    [SerializeField]
    private GameObject[] guitons_objects;                   // ** loot objects
    #endregion

    #region private variables
    private List<GameObject> fractured_pieces = new List<GameObject> ();
    private bool isAlredyAffected = false;
    private GameObject affectedPiece;
    #endregion

    // forced to have tag
    public void Awake()
    {
        foreach (Transform piece in gameObject.transform)
            piece.gameObject.tag = "BreakableObject";            
    }

    public void setAffected(bool value) => isAlredyAffected = value;
    public bool isAffected() => isAlredyAffected;

    ///<summary> 
    /// It swaps the current gameobject which is the original for the fractured one
    /// in case that the bool is true
    ///</summary>
    public void Affected() 
    {
            original_object.SetActive(false);
            var position = original_object.transform.position;
            var rotation = original_object.transform.rotation;
            fractured_object.transform.position = position;
            fractured_object.transform.localRotation = rotation;
            fractured_object.SetActive(true);       
    }

    ///<summary> 
    /// It apply the explosion force to the different pieces of the gameobject
    ///</summary>
    public void AffectedExplosionForce (float _strength, Vector3 _explosionPosition, float _radius) 
    {
        // fractured objetc
        if (fractured_object.activeSelf) 
        {
            foreach (Transform piece in fractured_object.transform) 
            {
                fractured_pieces.Add(piece.gameObject);
                Rigidbody _rigidbody = piece.GetComponent<Rigidbody>();
                if (_rigidbody != null)
                {
                    _rigidbody.constraints = RigidbodyConstraints.None;
                    _rigidbody.AddExplosionForce(_strength, _explosionPosition, _radius);
                }
                affectedPiece = piece.gameObject;               
            }
        } 
        // sigle objects
        else 
        {
            Rigidbody _rigidbody = original_object.GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.None;
            _rigidbody.AddExplosionForce(_strength, _explosionPosition, _radius);
        }

        Invoke("guitons", 0.5f);

        // hides some pieces of the geometry
        foreach (Transform piece in fractured_object.transform)
        {
            piece.gameObject.GetComponent<MeshRenderer>().enabled = random();
            if(!piece.gameObject.GetComponent<MeshRenderer>().enabled)
                StartCoroutine(vanish(piece.gameObject));
        }
    }

    ///<summary> 
    /// It hides the geometry one second later
    ///</summary>
    IEnumerator vanish (GameObject go)
    {
        yield return new WaitForSeconds(1);
        go.SetActive(false);
    }

    ///<summary> 
    /// Sets the loot position
    ///</summary>
    private void guitons()
    {
        int counter = 0;
        foreach (GameObject go in guitons_objects)
        {
            counter = Random.Range(0, fractured_pieces.Count);
            var position = fractured_pieces[counter].transform.position;
            var rotation = fractured_pieces[counter].transform.rotation;
            go.transform.position = position;
            go.transform.localRotation = rotation;
            go.SetActive(true);
        }            
    }

    ///<summary> 
    /// Returns a random bool
    ///</summary>
    public bool random () => (Random.Range(0, 80) <= 2);
}
