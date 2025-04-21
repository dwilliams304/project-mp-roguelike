using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    protected static T _instance;

    public static bool HasInstance => _instance != null;
    public static T TryGetInstance() => HasInstance ? _instance : null;

    public static T Instance
    {
        get {
            if(_instance == null){
                _instance = FindAnyObjectByType<T>();
                if(_instance == null){
                    var obj = new GameObject(typeof(T).Name + "Auto-Generated");
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake(){
        Init();
    }

    protected virtual void Init(){
        if(!Application.isPlaying) return;

        if(transform.parent != null) transform.SetParent(null);
        
        if(_instance == this){
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else{
            if(_instance != this){
                Destroy(gameObject);
            }
        }
    }
}