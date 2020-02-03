using UnityEngine;

public abstract class Singleton<T> where T : class
{ 
    protected static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = System.Activator.CreateInstance(typeof(T)) as T;
            }
            return _instance;
        }
    }
}



/*
스레드 세이프버전 
출처: http://lonpeach.com/2017/02/04/unity3d-singleton-pattern-example/

using System;
using System.Reflection;
public class Singleton<T> where T : class
{
    private static object _syncobj = new object();
    private static volatile T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                CreateInstance();
            } return _instance;
        }
    }

    private static void CreateInstance()
    {
        lock (_syncobj)
        {
            if (_instance == null)
            {
                Type t = typeof(T); // Ensure there are no public constructors... 
                ConstructorInfo[] ctors = t.GetConstructors();
                if (ctors.Length > 0)
                {
                    throw new InvalidOperationException(String.Format("{0} has at least one accesible ctor making it impossible to enforce singleton behaviour", t.Name));
                } // Create an instance via the private constructor 
                _instance = (T)Activator.CreateInstance(t, true);
            }
        }
    }
}
*/
