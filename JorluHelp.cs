using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class JorluHelp 
{
    /******************************
                VECTORS
    *******************************/
    public static Vector2 to2(this Vector3 vector) 
    {
        return vector;
    }

    public static Vector3 setX(this Vector3 vector, float x) 
    {
        return new Vector3(x, vector.y, vector.z);
    }

    public static Vector3 setY(this Vector3 vector, float y) 
    {
        return new Vector3(vector.x, y, vector.z);
    }

    public static Vector3 setZ(this Vector3 vector, float z) 
    {
        return new Vector3(vector.x, vector.y, z);
    }

    public static Vector3 addX(this Vector3 vector, float plusX) 
    {
        return new Vector3(vector.x + plusX, vector.y, vector.z);
    }

    public static Vector3 addY(this Vector3 vector, float plusY) 
    {
        return new Vector3(vector.x, vector.y + plusY, vector.z);
    }

    public static Vector3 addZ(this Vector3 vector, float plusZ) 
    {
        return new Vector3(vector.x, vector.y, vector.z + plusZ);
    }

    public static Vector3 timesX(this Vector3 vector, float timesX) 
    {
        return new Vector3(vector.x * timesX, vector.y, vector.z);
    }

    public static Vector3 timesY(this Vector3 vector, float timesY) 
    {
        return new Vector3(vector.x, vector.y * timesY, vector.z);
    }

    public static Vector3 timesZ(this Vector3 vector, float timesZ) 
    {
        return new Vector3(vector.x, vector.y, vector.z * timesZ);
    }

    /******************************
                COLORS
    *******************************/
    public static Color setAlpha(this Color color, float alpha) 
    {
        color.a = alpha;
        return color;
    }

    /******************************
                ANIMATIONS
    *******************************/
    public static Animation PlayNormal(this Animation anim, float speed) 
    {
       anim[anim.clip.name].speed = speed;
       anim[anim.clip.name].time = 0;
       anim.Play();

       return anim;
    }

    public static Animation PlayFromFrame(this Animation anim, float frame) 
    {
       anim[anim.clip.name].time = frame;
       anim.Play();

       return anim;
    }

    public static Animation PlayFromFrame(this Animation anim, float frame, float speed) 
    {
       anim[anim.clip.name].speed = speed;
       anim[anim.clip.name].time = frame;
       anim.Play();

       return anim;
    }

    public static Animation PlayBackwards(this Animation anim, float speed) 
    {
       anim[anim.clip.name].speed = -speed;
       anim[anim.clip.name].time = anim[anim.clip.name].length;
       anim.Play();

       return anim;
    }

    public static float FrameTime(this Animation anim)
    {
        return anim[anim.clip.name].time;
    }
    public static Animation SetSpeed(this Animation anim, float speed)
    {
        anim[anim.clip.name].speed = speed;

        return anim;
    }

    public static Animation SetTime(this Animation anim, float time)
    {
        anim[anim.clip.name].time = time;

        return anim;
    }

    public static T[] Shuffle<T>(this T[] originalArray)
    {
        for (int i = 0; i < originalArray.Length; i++ )
        {
            T tmp = originalArray[i];
            int j = UnityEngine.Random.Range(i, originalArray.Length);
            originalArray[i] = originalArray[j];
            originalArray[j] = tmp;
        }
        return originalArray;
    }

    /*************************
    CUTE STUFF
    **************************/
    public static void InputFeedback(this Transform trans, Vector3 originalSize, float increment, float time)
    {
        RoutineRunner.instance.StartCoroutine (InputFeedbackCoroutine(trans, originalSize, increment, time));
    }

    public static IEnumerator InputFeedbackCoroutine(Transform trans, Vector3 originalSize, float increment, float time)
    {
        float elapsedTime = 0;
        while(elapsedTime < time)
        {
             trans.localScale = Vector3.Lerp(trans.localScale, originalSize*increment, (elapsedTime/time));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(.001f);

        elapsedTime = 0;

        while(elapsedTime < time)
        {
            trans.localScale = Vector3.Lerp(trans.localScale, originalSize, (elapsedTime/time));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

     /************************
    AUDIOSOURCE STUFF
    ************************/
    public static void FadeTo (this AudioSource audioSource, float targetVolume)
     {
         FadeTo (audioSource, targetVolume, null);
     }
 
     public static void FadeTo (this AudioSource audioSource, float targetVolume, Action onComplete)
     {
         FadeTo (audioSource, targetVolume, 0.3f, onComplete);
     }
 
     public static void FadeTo (this AudioSource audioSource, float targetVolume, float duration, Action onComplete)
     {
         if (coroutines.ContainsKey (audioSource)) 
         {
             //RoutineRunner.instance.StopCoroutine (coroutines [audioSource]);
         }
 
         var coroutine = FadeToCoroutine (audioSource, targetVolume, duration, onComplete);
         RoutineRunner.instance.StartCoroutine (coroutines [audioSource] = coroutine);
     }
     
     public static IEnumerator FadeToCoroutine (AudioSource audioSource, float targetVolume, float duration, Action onComplete)
    {
        float elapsed = 0;
        float startVolume = audioSource.volume;

        while ((elapsed += Time.deltaTime) < duration) 
        {
            audioSource.volume = Mathf.Lerp (startVolume, targetVolume, elapsed / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
        coroutines.Remove (audioSource);

        if (onComplete != null)
            onComplete ();
    }
 
    static Dictionary<AudioSource, IEnumerator> coroutines = new Dictionary<AudioSource, IEnumerator> ();
}