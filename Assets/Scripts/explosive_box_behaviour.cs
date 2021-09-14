using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class explosive_box_behaviour : MonoBehaviour
{
    #region public variables
    [Header("References")]
    [SerializeField]
    private Material explosion_material;
    [SerializeField]
    private MeshRenderer box_mesh_renderer;

    [Header("Customizable Features")]
    [SerializeField]
    private float timer = 5f;
    [SerializeField]
    private float radius = 3f;
    [SerializeField]
    private float strength = 10f;    
    #endregion

    #region private variables
    private Breakable_Object breakable_object;
    private bool exploded = false;
    private float angle = 0f;
    private Rigidbody self_rb;
    private bool isExploded = false;
    #endregion

    private void Awake()
    {
        gameObject.tag = "ExplosiveObject";
        self_rb = gameObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<player_input>(out player_input _input) && !exploded)
        {
            StartCoroutine(explosion());
        }

        if(collision.gameObject.TryGetComponent<item_weapon>(out item_weapon _weapon) && !exploded)
        {
            Explode();
        }
    }

    IEnumerator explosion()
    {
        Material [] materials = box_mesh_renderer.materials;
        materials[1] = explosion_material;
        box_mesh_renderer.materials = materials;
        while (timer > 0)
        {
            angle += 0.1f;
            if (angle > 2 * Mathf.PI)
                angle = 0;
            materials[1].SetFloat("Rotation_Angle", angle);
            box_mesh_renderer.materials = materials;
            yield return new WaitForSeconds(0.025f);
            timer -= 0.025f;
        }
        Explode();
    }

    public bool hasExploded() => isExploded;

    private void Explode()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        explosionEffect();

        StopAllCoroutines();

        isExploded = true;

        self_rb.constraints = RigidbodyConstraints.None;

        Vector3 _explosionPosition = transform.position;

        Collider[] obstacles_affected = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider hit in obstacles_affected)
        {
            if (hit.tag.Equals("BreakableObject"))
            {
                breakable_object = hit.transform.parent.GetComponent<Breakable_Object>();
                if (breakable_object != null)
                {
                    if (!breakable_object.isAffected())
                    {
                        breakable_object.setAffected(true);
                        breakable_object.Affected();
                        breakable_object.AffectedExplosionForce(strength, _explosionPosition, radius);
                    }
                    
                }
            }
            if (hit.tag.Equals("ExplosiveObject"))
                if (gameObject != hit.gameObject)
                    if(!hit.transform.gameObject.GetComponent<explosive_box_behaviour>().hasExploded())
                        hit.transform.gameObject.GetComponent<explosive_box_behaviour>().Explode();
        }

        

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        exploded = true;
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    private void OnDrawGizmos()
    {
        Gradient gradient = new Gradient();
        Handles.color = new Color (0.6f, 0f, 0f, 0.25f);
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;
        Handles.SphereHandleCap(0, transform.position, transform.rotation, radius, EventType.Repaint);
    }

    private void explosionEffect()
    {
        GameObject explosion = gameObject.transform.GetChild(0).gameObject;
        foreach (Transform vfx_part in explosion.transform)
        {
            vfx_part.gameObject.GetComponent<ParticleSystem>().Play();
        }

        Invoke("hide", 0.75f);
    }

    private void hide() => gameObject.SetActive(false);
}
