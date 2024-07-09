using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            instance = (T)FindObjectOfType(typeof(T));
            if (instance == null)
            {
                GameObject gameObj = new GameObject();
                gameObj.name = typeof(T).Name;
                instance = gameObj.AddComponent<T>();
                DontDestroyOnLoad(gameObj);
            }
            
            return instance;
        }
    }

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
