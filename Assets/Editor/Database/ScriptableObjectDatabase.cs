using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Database
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
            EditorUtility.SetDirty(this);
        }

        public void Insert(int index, T item)
        {
            _database.Insert(index, item);
            EditorUtility.SetDirty(this);
        }

        public void Remove(T item)
        {
            _database.Remove(item);
            EditorUtility.SetDirty(this);
        }

        public void Remove(int index)
        {
            _database.RemoveAt(index);
            EditorUtility.SetDirty(this);
        }

        public T Get(int index)
        {
            return _database.ElementAt(index);
        }

        public void Replace(int index, T item)
        {
            _database[index] = item;
            EditorUtility.SetDirty(this);
        }

        public void Clear()
        {
            _database.Clear();
        }

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
    }
}