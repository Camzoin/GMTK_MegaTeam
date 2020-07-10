using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float magnitude;
    public float roughness;
    public float maxAngle;
    public float decay;

    Vector3 originalPosition;
    Quaternion originalRotation;
    [Range(0, 1)] float trauma;
    float timer;
    float seed;

    public static CameraShake instance;

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
        if (instance == null)
        {
            instance = this;
        }

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        seed = Random.value;
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
        Trauma += shakeValue;
    }
}
