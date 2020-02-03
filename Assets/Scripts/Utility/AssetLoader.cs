using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetLoader
{
	static private Dictionary<string, GameObject> _cache = new Dictionary<string, GameObject>();

	static public void Load(string subfolder)
	{
		object[] objects = Resources.LoadAll(subfolder);
		
		for (int i = 0; i < objects.Length ; i++ )
		{
			GameObject obj = (GameObject)(objects[i]);
			_cache[obj.name] = obj;
		}
	}

	static public GameObject Get(string key)
	{
		return _cache[key];
	}

	static public GameObject Instance(string key, Vector2 pos, Transform parent)
	{
		GameObject obj = (GameObject)(GameObject.Instantiate(_cache[key], pos, Quaternion.identity));
		obj.transform.parent = parent;
		return obj;
	}

	static public void Remove(params string[] arg)
	{
		for (int i = 0; i < arg.Length ; i++ ){
			string key = arg[i];
			_cache.Remove(key);
		}
	}

	static public void ClearCache()
	{
		_cache.Clear();
		Resources.UnloadUnusedAssets();
	}

}
