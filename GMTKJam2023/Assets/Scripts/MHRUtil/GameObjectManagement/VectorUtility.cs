using UnityEngine;

namespace MHR.GameObjectManagement
{
    public abstract class VectorUtility<T>
    {
        public abstract void SetX(ref T vector, float x);
        public abstract void SetY(ref T vector, float y);
        public abstract void SetXY(ref T vector, float x, float y);
    }

    public class Vector2Utility : VectorUtility<Vector2>
    {
        public override void SetX(ref Vector2 vector, float x)
        {
            vector = new Vector2(x, vector.y);
        }

        public override void SetY(ref Vector2 vector, float y)
        {
            vector = new Vector2(vector.x, y);
        }

        public override void SetXY(ref Vector2 vector, float x, float y)
        {
            SetX(ref vector, x);
            SetY(ref vector, y);
        }
    }

    public class Vector3Utility : VectorUtility<Vector3>
    {
        public override void SetX(ref Vector3 vector, float x)
        {
            vector = new Vector3(x, vector.y, vector.z);
        }

        public override void SetY(ref Vector3 vector, float y)
        {
            vector = new Vector3(vector.x, y, vector.z);
        }

        public void SetZ(ref Vector3 vector, float z)
        {
            vector = new Vector3(vector.x, vector.y, z);
        }

        public override void SetXY(ref Vector3 vector, float x, float y)
        {
            SetX(ref vector, x);
            SetY(ref vector, y);
        }

        public void SetXZ(ref Vector3 vector, float x, float z)
        {
            SetX(ref vector, x);
            SetZ(ref vector, z);
        }

        public void SetYZ(ref Vector3 vector, float y, float z)
        {
            SetY(ref vector, y);
            SetZ(ref vector, z);
        }

        public void SetXYZ(ref Vector3 vector, float x, float y, float z)
        {
            SetX(ref vector, x);
            SetY(ref vector, y);
            SetZ(ref vector, z);
        }
    }
}