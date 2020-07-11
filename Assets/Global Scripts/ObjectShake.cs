using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectShake : MonoBehaviour
{
    public Vector3 origin;
    public bool shakeBasedOnOrigin;
    public float magnitude;
    public float roughness;
    public float maxAngle;
    public float decay;

    Vector3 originalPosition;
    Quaternion originalRotation;
    [Range(0, 1)] float trauma;
    float timer;
    float seed;

    public delegate void EventHandler();

    public event EventHandler BeforeShake;
    public event EventHandler OnShake;
    public float Trauma
    {
        get
        {
            return trauma;
        }
        set
        {
            trauma = Mathf.Clamp01(value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;

        seed = Random.value;

        if (shakeBasedOnOrigin)
        {
            OnShake += () =>
            {
                float distance = Vector3.Distance(transform.position.normalized, origin.normalized);
                magnitude *= 1 - distance;
                roughness *= 1 - distance;
                maxAngle *= 1 - distance;
                decay *= 1 - distance;
            };
        }

        WordManager.instance.OnCorrectInput += () => Shake(0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Trauma > 0)
        {
            timer += Time.deltaTime * Mathf.Pow(trauma, 0.2f) * roughness;
            Vector3 offset = GetNoiseVec() * magnitude * Trauma;
            transform.localPosition = originalPosition + offset;
            transform.localRotation = originalRotation * Quaternion.Euler(GetNoiseVec() * maxAngle * trauma);

            Trauma -= Time.deltaTime * decay * (trauma + 0.2f);
        }
        else
        {
            timer = 0;
            seed = Random.value;

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, originalPosition, Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, originalRotation, Time.deltaTime);
        }
    }

    public float GetNoise(float seed)
    {
        return (Mathf.PerlinNoise(seed, timer) - 0.5f) * 2;
    }

    public Vector3 GetNoiseVec()
    {
        return new Vector3(
            GetNoise(seed + 1),
            GetNoise(seed + 2),
            GetNoise(seed + 3)
            );
    }

    public void Shake(float shakeValue)
    {
        BeforeShake?.Invoke();
        OnShake?.Invoke();
        Trauma += shakeValue;
    }
}
