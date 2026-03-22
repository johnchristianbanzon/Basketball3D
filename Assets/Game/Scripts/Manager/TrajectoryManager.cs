using UnityEngine;

public class TrajectoryManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private LineRenderer line;
    private Transform _startPoint;
    [SerializeField] private int resolution = 30;
    [SerializeField] private float timeStep = 0.1f;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private Transform _headPosition;

    [SerializeField] private float shootAngle = 45f;

    public Vector3 GetShootDirection()
    {
        Vector3 flatForward = _headPosition.forward;
        flatForward.y = 0;
        flatForward.Normalize();

        return Quaternion.AngleAxis(-shootAngle, _headPosition.right) * flatForward;
    }

    public void SetThrowPowerForce(float throwForce)
    {
        this.throwForce = throwForce;
    }

    public void DrawTrajectory(Transform startPoint)
    {
        _startPoint = startPoint;
        Vector3 startPos = startPoint.position;
        Vector3 startVel = GetShootDirection() * (throwForce);

        line.positionCount = resolution;

        for (int i = 0; i < resolution; i++)
        {
            float t = i * timeStep;

            Vector3 point = startPos
                + startVel * t
                + 0.5f * Physics.gravity * t * t;

            if (i > 0)
            {
                Vector3 prevPoint = line.GetPosition(i - 1);

                if (Physics.Linecast(prevPoint, point, out RaycastHit hit))
                {
                    line.positionCount = i + 1;
                    line.SetPosition(i, hit.point);
                    break;
                }
            }

            line.SetPosition(i, point);
        }
    }

    public void ClearTrajectory()
    {
        line.positionCount = 0;
    }
}
