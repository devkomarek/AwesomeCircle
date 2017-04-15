using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Assets.Scripts.ScriptableObject.Database
{
    public class ScriptableObjectDatabase<T> : UnityEngine.ScriptableObject where T : class
    {
        [SerializeField] private List<T> _database = new List<T>();

        public int Count
        {
            get { return _database.Count; }
        }

        public void Add(T quality)
        {
            _database.Add(quality);
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }

        public void Insert(int index, T item)
        {
            _database.Insert(index, item);
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }

        public void Remove(T item)
        {
            _database.Remove(item);
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }

        public void Remove(int index)
        {
            _database.RemoveAt(index);
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }

        public T Get(int index)
        {
            return _database.ElementAt(index);
        }

        public void Replace(int index, T item)
        {
            _database[index] = item;
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }

        public void Clear()
        {
            _database.Clear();
        }

            #if UNITY_EDITOR
        public static U GetDatabase<U>(string dbPath, string dbName) where U : UnityEngine.ScriptableObject
        {
            var dbFullPath = @"Assets/" + dbPath + "/" + dbName;

            var db = AssetDatabase.LoadAssetAtPath(dbFullPath, typeof (U)) as U;
            if (db == null)
            {
                if (!AssetDatabase.IsValidFolder("Assets/" + dbPath))
                    AssetDatabase.CreateFolder("Assets", dbPath);

                db = CreateInstance<U>();
                AssetDatabase.CreateAsset(db, dbFullPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            return db;
        }
            #endif
    }
}