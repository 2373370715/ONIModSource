using UnityEngine;

public class SimpleTransformAnimation : MonoBehaviour
{
	[SerializeField]
	private Vector3 rotationSpeed;

	[SerializeField]
	private Vector3 translateSpeed;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(rotationSpeed * Time.unscaledDeltaTime);
		base.transform.Translate(translateSpeed * Time.unscaledDeltaTime);
	}
}
