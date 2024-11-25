using UnityEngine;

public class KCircleCollider2D : KCollider2D {
    [SerializeField]
    private float _radius;

    public float radius {
        get => _radius;
        set {
            _radius = value;
            MarkDirty();
        }
    }

    public override Bounds bounds =>
        new Bounds(transform.GetPosition() + new Vector3(offset.x, offset.y, 0f),
                   new Vector3(_radius * 2f, _radius * 2f, 0f));

    public override Extents GetExtents() {
        var vector  = transform.GetPosition() + new Vector3(offset.x, offset.y, 0f);
        var vector2 = new Vector2(vector.x - radius, vector.y - radius);
        var vector3 = new Vector2(vector.x + radius, vector.y + radius);
        var width   = (int)vector3.x - (int)vector2.x + 1;
        var height  = (int)vector3.y - (int)vector2.y + 1;
        return new Extents((int)(vector.x - _radius), (int)(vector.y - _radius), width, height);
    }

    public override bool Intersects(Vector2 pos) {
        var position = transform.GetPosition();
        var b        = new Vector2(position.x, position.y) + offset;
        return (pos                                        - b).sqrMagnitude <= _radius * _radius;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(bounds.center, radius);
    }
}