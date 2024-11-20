using System;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/UICurvePath")]
public class UICurvePath : KMonoBehaviour
{
	protected override void OnSpawn()
	{
		this.Init();
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		this.OnResize();
		this.startDelay = (float)UnityEngine.Random.Range(0, 8);
	}

	private void OnResize()
	{
		this.A = this.startPoint.position;
		this.B = this.controlPointStart.position;
		this.C = this.controlPointEnd.position;
		this.D = this.endPoint.position;
	}

	protected override void OnCleanUp()
	{
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Remove(instance.OnResize, new System.Action(this.OnResize));
		base.OnCleanUp();
	}

	private void Update()
	{
		this.startDelay -= Time.unscaledDeltaTime;
		this.sprite.gameObject.SetActive(this.startDelay < 0f);
		if (this.startDelay > 0f)
		{
			return;
		}
		this.tick += Time.unscaledDeltaTime * this.moveSpeed;
		this.sprite.transform.position = this.DeCasteljausAlgorithm(this.tick);
		this.sprite.SetAlpha(Mathf.Min(this.sprite.color.a + this.tick / 2f, 1f));
		if (this.animateScale)
		{
			float num = Mathf.Min(this.sprite.transform.localScale.x + Time.unscaledDeltaTime * this.moveSpeed, 1f);
			this.sprite.transform.localScale = new Vector3(num, num, 1f);
		}
		if (this.loop && this.tick > 1f)
		{
			this.Init();
		}
	}

	private void Init()
	{
		this.sprite.transform.position = this.startPoint.position;
		this.tick = 0f;
		if (this.animateScale)
		{
			this.sprite.transform.localScale = this.initialScale;
		}
		this.sprite.SetAlpha(this.initialAlpha);
	}

	private void OnDrawGizmos()
	{
		if (!Application.isPlaying)
		{
			this.A = this.startPoint.position;
			this.B = this.controlPointStart.position;
			this.C = this.controlPointEnd.position;
			this.D = this.endPoint.position;
		}
		Gizmos.color = Color.white;
		Vector3 a = this.A;
		float num = 0.02f;
		int num2 = Mathf.FloorToInt(1f / num);
		for (int i = 1; i <= num2; i++)
		{
			float t = (float)i * num;
			this.DeCasteljausAlgorithm(t);
		}
		Gizmos.color = Color.green;
	}

	private Vector3 DeCasteljausAlgorithm(float t)
	{
		float d = 1f - t;
		Vector3 a = d * this.A + t * this.B;
		Vector3 a2 = d * this.B + t * this.C;
		Vector3 a3 = d * this.C + t * this.D;
		Vector3 a4 = d * a + t * a2;
		Vector3 a5 = d * a2 + t * a3;
		return d * a4 + t * a5;
	}

	public Transform startPoint;

	public Transform endPoint;

	public Transform controlPointStart;

	public Transform controlPointEnd;

	public Image sprite;

	public bool loop = true;

	public bool animateScale;

	public Vector3 initialScale;

	private float startDelay;

	public float initialAlpha = 0.5f;

	public float moveSpeed = 0.1f;

	private float tick;

	private Vector3 A;

	private Vector3 B;

	private Vector3 C;

	private Vector3 D;
}
