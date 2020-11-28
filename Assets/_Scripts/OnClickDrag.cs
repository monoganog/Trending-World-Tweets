using UnityEngine;

/// <summary>
/// Drag a rigidbody with the mouse using a spring joint.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class OnClickDrag : MonoBehaviour
{
    public float force = 300;
    public float damping = 10;
    public float dragHeight = 1;
    public GameObject feedbackInstance;
    public GameObject feedback;

    Transform jointTrans;
    public float dragDepth;
    private Vector3 offset;

    void OnMouseDown()
    {
        HandleInputBegin(Input.mousePosition);
    }

    void OnMouseUp()
    {
        HandleInputEnd(Input.mousePosition);
        Destroy(feedbackInstance, 0);
    }

    void OnMouseDrag()
    {
        HandleInput(Input.mousePosition);
    }

    public void HandleInputBegin(Vector3 screenPosition)
    {
        var ray = Camera.main.ScreenPointToRay(screenPosition);
        //Debug.Log(screenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Interactive"))
            {
                //dragDepth = CameraPlane.CameraToPointDepth(Camera.main, hit.point);
                jointTrans = AttachJoint(hit.rigidbody, hit.point);

                feedbackInstance = Instantiate(feedback, hit.point, transform.rotation) as GameObject;


                feedbackInstance.transform.parent = hit.transform;

            }
        }
    }

    public void HandleInput(Vector3 screenPosition)
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(transform.position, hit.point);
            offset = hit.point + new Vector3(0, dragHeight, 0);
        }





        if (jointTrans == null)
            return;
        
            var worldPos = Camera.main.ScreenPointToRay(screenPosition);
        jointTrans.position = offset;
    }

    public void HandleInputEnd(Vector3 screenPosition)
    {
        if (jointTrans.gameObject != null)
        {
            Destroy(jointTrans.gameObject);
        }
    }

    Transform AttachJoint(Rigidbody rb, Vector3 attachmentPosition)
    {
        GameObject go = new GameObject("Attachment Point");
        go.hideFlags = HideFlags.HideInHierarchy;
        go.transform.position = attachmentPosition;

        var newRb = go.AddComponent<Rigidbody>();
        newRb.isKinematic = true;

        var joint = go.AddComponent<ConfigurableJoint>();
        joint.connectedBody = rb;
        joint.configuredInWorldSpace = true;
        joint.xDrive = NewJointDrive(force, damping);
        joint.yDrive = NewJointDrive(force, damping);
        joint.zDrive = NewJointDrive(force, damping);
        joint.slerpDrive = NewJointDrive(force, damping);
        joint.rotationDriveMode = RotationDriveMode.Slerp;

        return go.transform;
    }

    private JointDrive NewJointDrive(float force, float damping)
    {
        JointDrive drive = new JointDrive();
        //drive.mode = JointDriveMode.Position;
        drive.positionSpring = force;
        drive.positionDamper = damping;
        drive.maximumForce = Mathf.Infinity;
        return drive;
    }
}