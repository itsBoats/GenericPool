// This is a generic pooling system that can store any type of component, including monobehaviours
// It can't store GameObjects but all GameObjects have a Transform or RectTransform so you can then use transform.gameObject
// Requires Unity 2021.1 or later:
	// The serializer can now serialize fields of generic types directly;
	// it is no longer necessary to derive a concrete subclass from a generic type in order to serialize it
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool<T> where T : Component {

	public T prefab;
	private List<T> pool = new List<T>();

	public T GetObject(Vector3 pos) {
		for (int i = 0; i < pool.Count; i++) {
			if (!pool[i].gameObject.activeInHierarchy) {
				pool[i].transform.position = pos;
				pool[i].gameObject.SetActive(true);
				return pool[i];
			}
		}

		T obj = GameObject.Instantiate(prefab, pos, Quaternion.identity) as T;
		pool.Add(obj);

		return obj;
	}

	public void HideAll() {
		for (int i = 0; i < pool.Count; i++) {
			pool[i].gameObject.SetActive(false);
		}
	}

	public void Clear() {
		for (int i = 0; i < pool.Count; i++) {
			GameObject.Destroy(pool[i].gameObject);
		}

		pool.Clear();
	}

	public void Prime(int count) {
		for (int i = 0; i < count; i++) {
			T obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as T;
			obj.gameObject.SetActive(false);
			pool.Add(obj);
		}
	}
}
