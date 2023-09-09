using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class AudioSourcePool
{
    protected List<AudioSource> _pool;

    /// <summary>
    /// 设置对象池大小
    /// </summary>
    /// <param name="poolSize"></param>
    public virtual void FillPool(int poolSize)
    {
        if(_pool == null)
        {
            _pool = new List<AudioSource>(poolSize);
        }
        
        if(poolSize <= 0 || poolSize <= _pool.Count)
            return;
        
        foreach(AudioSource source in _pool)
        {
            UnityEngine.Object.Destroy(source.gameObject);
        }

        for(int i = 0; i < poolSize; i++)
        {
            GameObject tempAudioSourcePool = new GameObject("AudioSourcePool_" + i);
            tempAudioSourcePool.transform.parent = ObjectPoolManager._audioSourceEmpty.transform;
            AudioSource tempSource = tempAudioSourcePool.AddComponent<AudioSource>();
            tempAudioSourcePool.SetActive(false);
            _pool.Add(tempSource);
        }

    }

    /// <summary>
    /// 从对象池中获取AudioSource
    /// </summary>
    /// <param name="poolCanExpand"></param>
    /// <returns></returns>
    public virtual AudioSource GetAvailableAudioSource(bool poolCanExpand)
    {
        foreach (AudioSource source in _pool)
        {
            if (!source.gameObject.activeInHierarchy)
            {
                source.gameObject.SetActive(true);
                return source;
            }
        }
        if (poolCanExpand)
        {
            GameObject tempAudioSourcePool = new GameObject("AudioSourcePool_" + _pool.Count);
            tempAudioSourcePool.transform.parent = ObjectPoolManager._audioSourceEmpty.transform;
            AudioSource tempSource = tempAudioSourcePool.AddComponent<AudioSource>();
            tempAudioSourcePool.SetActive(true);
            _pool.Add(tempSource);
            return tempSource;
        }
        return null;

    }

    /// <summary>
    /// 自动关闭AudioSource
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="source"></param>
    /// <param name="clip"></param>
    /// <returns></returns>
    public virtual IEnumerator AutoDisableAudioSource(float duration, AudioSource source, AudioClip clip)
    {
        yield return CoroutineMethods.WaitFor(duration);
        if (source.clip != clip)
        {
            yield break;
        }
        source.gameObject.SetActive(false);
    }

    /// <summary>
    /// 关闭某个AudioSource
    /// </summary>
    /// <param name="sourceToStop"></param>
    /// <returns></returns>
    public virtual bool FreeSound(AudioSource sourceToStop)
    {
        foreach (AudioSource source in _pool)
        {
            if (source == sourceToStop)
            {
                source.Stop();
                source.gameObject.SetActive(false);
                return true;
            }
        }
        return false;
    }
}
