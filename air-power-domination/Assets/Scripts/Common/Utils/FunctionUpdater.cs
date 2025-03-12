using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RDP.Common.Utils {
	public class FunctionUpdater {
		private static List<FunctionUpdater> _updaterList; // Holds a reference to all active updaters

		private static GameObject
			_initGameObject; // Global game object used for initializing class, is destroyed on scene change

		private bool _active;
		private readonly string _functionName;

		private readonly GameObject _gameObject;
		private readonly Func<bool> _updateFunc; // Destroy Updater if return true;

		public FunctionUpdater(GameObject gameObject, Func<bool> updateFunc, string functionName, bool active) {
			_gameObject = gameObject;
			_updateFunc = updateFunc;
			_functionName = functionName;
			_active = active;
		}

		private static void InitIfNeeded() {
			if (_initGameObject == null) {
				_initGameObject = new GameObject("FunctionUpdater_Global");
				_updaterList = new List<FunctionUpdater>();
			}
		}

		public static FunctionUpdater Create(Action updateFunc) {
			return Create(() => {
				updateFunc();
				return false;
			}, "", true, false);
		}

		public static FunctionUpdater Create(Func<bool> updateFunc) {
			return Create(updateFunc, "", true, false);
		}

		public static FunctionUpdater Create(Func<bool> updateFunc, string functionName) {
			return Create(updateFunc, functionName, true, false);
		}

		public static FunctionUpdater Create(Func<bool> updateFunc, string functionName, bool active) {
			return Create(updateFunc, functionName, active, false);
		}

		public static FunctionUpdater Create(Func<bool> updateFunc, string functionName, bool active,
		                                     bool stopAllWithSameName) {
			InitIfNeeded();

			if (stopAllWithSameName) StopAllUpdatersWithName(functionName);

			GameObject gameObject = new GameObject("FunctionUpdater Object " + functionName, typeof(MonoBehaviourHook));
			FunctionUpdater functionUpdater = new FunctionUpdater(gameObject, updateFunc, functionName, active);
			gameObject.GetComponent<MonoBehaviourHook>().OnUpdate = functionUpdater.Update;

			_updaterList.Add(functionUpdater);
			return functionUpdater;
		}

		private static void RemoveUpdater(FunctionUpdater funcUpdater) {
			InitIfNeeded();
			_updaterList.Remove(funcUpdater);
		}

		public static void DestroyUpdater(FunctionUpdater funcUpdater) {
			InitIfNeeded();
			if (funcUpdater != null) funcUpdater.DestroySelf();
		}

		public static void StopUpdaterWithName(string functionName) {
			InitIfNeeded();
			for (int i = 0; i < _updaterList.Count; i++)
				if (_updaterList[i]._functionName == functionName) {
					_updaterList[i].DestroySelf();
					return;
				}
		}

		public static void StopAllUpdatersWithName(string functionName) {
			InitIfNeeded();
			for (int i = 0; i < _updaterList.Count; i++)
				if (_updaterList[i]._functionName == functionName) {
					_updaterList[i].DestroySelf();
					i--;
				}
		}

		public void Pause() {
			_active = false;
		}

		public void Resume() {
			_active = true;
		}

		private void Update() {
			if (!_active) return;
			if (_updateFunc()) DestroySelf();
		}

		public void DestroySelf() {
			RemoveUpdater(this);
			if (_gameObject != null) Object.Destroy(_gameObject);
		}

		// Class to hook Actions into MonoBehaviour
		private class MonoBehaviourHook : MonoBehaviour {
			public Action OnUpdate;

			private void Update() {
				if (OnUpdate != null) OnUpdate();
			}
		}
	}
}