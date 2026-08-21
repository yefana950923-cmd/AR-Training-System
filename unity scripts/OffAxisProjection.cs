using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OffAxisProjection : MonoBehaviour
{
    public Transform pa;   // 左下
    public Transform pb;   // 右下
    public Transform pc;   // 左上
    public Transform eye;  // 观察者位置

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (pa == null || pb == null || pc == null || eye == null) return;
        transform.position = eye.position;
        
        Vector3 va = pa.position - eye.position;
        Vector3 vb = pb.position - eye.position;
        Vector3 vc = pc.position - eye.position;

        Vector3 vr = (pb.position - pa.position).normalized;   // right
        Vector3 vu = (pc.position - pa.position).normalized;   // up
        Vector3 vn = Vector3.Cross(vr, vu).normalized;         // normal

        // 保证法线朝向观察者
        if (Vector3.Dot(vn, eye.position - pa.position) < 0f)
        {
            vn = -vn;
        }

        float n = cam.nearClipPlane;
        float f = cam.farClipPlane;

        float d = -Vector3.Dot(va, vn);
        if (d <= 0.01f) return;

        float l = Vector3.Dot(vr, va) * n / d;
        float r = Vector3.Dot(vr, vb) * n / d;
        float b = Vector3.Dot(vu, va) * n / d;
        float t = Vector3.Dot(vu, vc) * n / d;

        cam.projectionMatrix = PerspectiveOffCenter(l, r, b, t, n, f);


    }

    Matrix4x4 PerspectiveOffCenter(float l, float r, float b, float t, float n, float f)
    {
        Matrix4x4 m = new Matrix4x4();

        m[0, 0] = 2f * n / (r - l);
        m[0, 1] = 0f;
        m[0, 2] = (r + l) / (r - l);
        m[0, 3] = 0f;

        m[1, 0] = 0f;
        m[1, 1] = 2f * n / (t - b);
        m[1, 2] = (t + b) / (t - b);
        m[1, 3] = 0f;

        m[2, 0] = 0f;
        m[2, 1] = 0f;
        m[2, 2] = -(f + n) / (f - n);
        m[2, 3] = -(2f * f * n) / (f - n);

        m[3, 0] = 0f;
        m[3, 1] = 0f;
        m[3, 2] = -1f;
        m[3, 3] = 0f;

        return m;
        
    }
}
