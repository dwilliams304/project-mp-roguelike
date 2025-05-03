using UnityEngine;

public static class GameObjectExtensions
{
    public static GameObject SetLayersRecursive(this GameObject _go, int _layer, LayerMask layerToIgnore = default, bool ignoreLayer = false)
    {
        _go.layer = _layer;
        foreach (Transform child in _go.transform)
        {
            if(child.gameObject.layer == layerToIgnore && ignoreLayer) continue;
            
            child.gameObject.layer = _layer;
            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if (_HasChildren != null)
                SetLayersRecursive(child.gameObject, _layer);
          
        }
        return _go;
    }
}