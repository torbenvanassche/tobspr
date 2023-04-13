﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace Utilities
{
    public class Singleton<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour
    {
        private static T _instance;

        private static object _lock = new object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<T>();
                            singleton.name = "(singleton) " + typeof(T);

                            if (Application.isPlaying)
                            {
                                DontDestroyOnLoad(singleton);
                            }
                        }
                    }

                    return _instance;
                }
            }
        }
    }
}