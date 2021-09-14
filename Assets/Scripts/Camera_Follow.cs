using UnityEngine;

///<summary> 
/// Behavioyr of the camera
///</summary>
public class Camera_Follow : MonoBehaviour
{
    #region public variables
    [Header ("References")]
    [SerializeField]
    private Transform target;  
    
    [Header ("Customizable Features")]
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private bool RotateAroundPlayer = true;
    [SerializeField]
    private float RotationSpeed = 5.0f;
    [SerializeField]
    private float smoothSpeed = 0.125f;
    #endregion

    private void LateUpdate()
    {
        float angH = Input.GetAxis("RightH");
        float angV = Input.GetAxis("RightV");
    

        if (RotateAroundPlayer)
        {
            Quaternion cameraAngle = Quaternion.AngleAxis(Input.GetAxis("RightH") * RotationSpeed, Vector3.up);
            offset = cameraAngle * offset;
        }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        transform.LookAt(target);
    }
}
