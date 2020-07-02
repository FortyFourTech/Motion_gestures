using UnityEngine;

public static class ExtensionMethods
{
    public static Transform CreateChild(this Component mb, string name)
    {
        var go = new GameObject(name);
        var tr = go.transform;
        tr.SetParent(mb.transform);
        tr.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        return tr;
    }
}
