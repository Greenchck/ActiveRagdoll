using UnityEngine;

public class ActiveRagdollController : MonoBehaviour
{
    public Transform animatedTarget;

    public Rigidbody physicalHips;
    public Transform animatedHips;

    public float balanceSpring = 5000f;
    public float balanceDamper = 500f;
    public float jointSpring = 1000f;
    public float jointDamper = 100f;

    private class Limb
    {
        public ConfigurableJoint joint;
        public Transform targetTransform;
        public Quaternion startLocalRotation;
    }

    private Limb[] limbs;

    void Start()
    {
        if (animatedTarget == null)
        {
            Debug.LogError("Animated Target is not assigned");
            return;
        }

        ConfigurableJoint[] joints = GetComponentsInChildren<ConfigurableJoint>();
        limbs = new Limb[joints.Length];

        for (int i = 0; i < joints.Length; i++)
        {
            limbs[i] = new Limb();
            limbs[i].joint = joints[i];
            limbs[i].startLocalRotation = joints[i].transform.localRotation;
            limbs[i].targetTransform = FindChildByName(animatedTarget, joints[i].gameObject.name);

            if (limbs[i].targetTransform == null)
            {
                Debug.LogWarning($"Bone named '{joints[i].gameObject.name}' not found in target skeleton");
            }

            JointDrive drive = new JointDrive();
            drive.positionSpring = jointSpring;
            drive.positionDamper = jointDamper;
            drive.maximumForce = Mathf.Infinity;
            limbs[i].joint.slerpDrive = drive;
        }
    }

    void FixedUpdate()
    {
        if (limbs == null) return;

        foreach (var limb in limbs)
        {
            if (limb.targetTransform != null)
                SetTargetRotationLocal(limb.joint, limb.targetTransform.localRotation, limb.startLocalRotation);
        }

        if (physicalHips != null && animatedHips != null)
        {
            Vector3 targetPosition = animatedHips.position;
            Vector3 currentPosition = physicalHips.position;
            
            Vector3 positionError = targetPosition - currentPosition;
            Vector3 desiredVelocity = positionError * 20f;
            Vector3 velocityChange = desiredVelocity - physicalHips.linearVelocity;
            
            physicalHips.AddForce(velocityChange, ForceMode.VelocityChange);

            Quaternion targetRot = animatedHips.rotation;
            Quaternion currentRot = physicalHips.rotation;
            Quaternion deltaRot = targetRot * Quaternion.Inverse(currentRot);
            
            deltaRot.ToAngleAxis(out float angle, out Vector3 axis);
            if (angle > 180f) angle -= 360f;
            
            Vector3 desiredAngularVelocity = (axis * (angle * Mathf.Deg2Rad)) * 20f;
            Vector3 angularVelocityChange = desiredAngularVelocity - physicalHips.angularVelocity;
            
            physicalHips.AddTorque(angularVelocityChange, ForceMode.VelocityChange);
        }
    }

    private Transform FindChildByName(Transform parent, string name)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.name == name)
                return child;
        }
        return null;
    }

    private void SetTargetRotationLocal(ConfigurableJoint joint, Quaternion targetLocalRotation, Quaternion startLocalRotation)
    {
        if (joint.configuredInWorldSpace) return;

        Vector3 right = joint.axis;
        Vector3 forward = Vector3.Cross(joint.axis, joint.secondaryAxis).normalized;
        Vector3 up = Vector3.Cross(forward, right).normalized;
        Quaternion worldToJointSpace = Quaternion.LookRotation(forward, up);

        Quaternion resultRotation = Quaternion.Inverse(worldToJointSpace);
        resultRotation *= Quaternion.Inverse(targetLocalRotation) * startLocalRotation;
        resultRotation *= worldToJointSpace;

        joint.targetRotation = resultRotation;
    }
}
