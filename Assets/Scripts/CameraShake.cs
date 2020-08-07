using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public static CameraShake current;
	public float magnitude;
	IEnumerator currentShakeCoroutine;
	Vector3 originalPos;
	private void Awake()
	{
		if (current == null)
		{
			current = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
			return;
		}
	}

	public void StartShake()
	{
		if (currentShakeCoroutine != null)
		{
			StopCoroutine(currentShakeCoroutine);
		}

		currentShakeCoroutine = Shake();
		StartCoroutine(currentShakeCoroutine);
	}

	public void StopShake()
    {
		if (currentShakeCoroutine != null)
        {
			StopCoroutine(currentShakeCoroutine);
        }
		transform.localPosition = originalPos;

	}

	IEnumerator Shake()
	{
		Vector3 originalPos = transform.localPosition;

		while (true)
        {
			float x = Random.Range(-1f, 1f) * magnitude;
			float y = Random.Range(-1f, 1f) * magnitude;
			transform.localPosition = new Vector3(x, y, originalPos.z);

			yield return null;
		}
	}
}
