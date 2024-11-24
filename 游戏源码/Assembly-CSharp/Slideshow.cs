using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002008 RID: 8200
[AddComponentMenu("KMonoBehaviour/scripts/Slideshow")]
public class Slideshow : KMonoBehaviour
{
	// Token: 0x0600AE52 RID: 44626 RVA: 0x00418AB8 File Offset: 0x00416CB8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.timeUntilNextSlide = this.timePerSlide;
		if (this.transparentIfEmpty && this.sprites != null && this.sprites.Length == 0)
		{
			this.imageTarget.color = Color.clear;
		}
		if (this.isExpandable)
		{
			this.button = base.GetComponent<KButton>();
			this.button.onClick += delegate()
			{
				if (this.onBeforePlay != null)
				{
					this.onBeforePlay();
				}
				SlideshowUpdateType slideshowUpdateType = this.updateType;
				if (slideshowUpdateType == SlideshowUpdateType.preloadedSprites)
				{
					VideoScreen.Instance.PlaySlideShow(this.sprites);
					return;
				}
				if (slideshowUpdateType != SlideshowUpdateType.loadOnDemand)
				{
					return;
				}
				VideoScreen.Instance.PlaySlideShow(this.files);
			};
		}
		if (this.nextButton != null)
		{
			this.nextButton.onClick += delegate()
			{
				this.nextSlide();
			};
		}
		if (this.prevButton != null)
		{
			this.prevButton.onClick += delegate()
			{
				this.prevSlide();
			};
		}
		if (this.pauseButton != null)
		{
			this.pauseButton.onClick += delegate()
			{
				this.SetPaused(!this.paused);
			};
		}
		if (this.closeButton != null)
		{
			this.closeButton.onClick += delegate()
			{
				VideoScreen.Instance.Stop();
				if (this.onEndingPlay != null)
				{
					this.onEndingPlay();
				}
			};
		}
	}

	// Token: 0x0600AE53 RID: 44627 RVA: 0x00418BC0 File Offset: 0x00416DC0
	public void SetPaused(bool state)
	{
		this.paused = state;
		if (this.pauseIcon != null)
		{
			this.pauseIcon.gameObject.SetActive(!this.paused);
		}
		if (this.unpauseIcon != null)
		{
			this.unpauseIcon.gameObject.SetActive(this.paused);
		}
		if (this.prevButton != null)
		{
			this.prevButton.gameObject.SetActive(this.paused);
		}
		if (this.nextButton != null)
		{
			this.nextButton.gameObject.SetActive(this.paused);
		}
	}

	// Token: 0x0600AE54 RID: 44628 RVA: 0x00418C68 File Offset: 0x00416E68
	private void resetSlide(bool enable)
	{
		this.timeUntilNextSlide = this.timePerSlide;
		this.currentSlide = 0;
		if (enable)
		{
			this.imageTarget.color = Color.white;
			return;
		}
		if (this.transparentIfEmpty)
		{
			this.imageTarget.color = Color.clear;
		}
	}

	// Token: 0x0600AE55 RID: 44629 RVA: 0x00418CB4 File Offset: 0x00416EB4
	private Sprite loadSlide(string file)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		Texture2D texture2D = new Texture2D(512, 768);
		texture2D.filterMode = FilterMode.Point;
		texture2D.LoadImage(File.ReadAllBytes(file));
		return Sprite.Create(texture2D, new Rect(Vector2.zero, new Vector2((float)texture2D.width, (float)texture2D.height)), new Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect);
	}

	// Token: 0x0600AE56 RID: 44630 RVA: 0x00418D24 File Offset: 0x00416F24
	public void SetFiles(string[] files, int loadFrame = -1)
	{
		if (files == null)
		{
			return;
		}
		this.files = files;
		bool flag = files.Length != 0 && files[0] != null;
		this.resetSlide(flag);
		if (flag)
		{
			int num = (loadFrame != -1) ? loadFrame : (files.Length - 1);
			string file = files[num];
			Sprite slide = this.loadSlide(file);
			this.setSlide(slide);
			this.currentSlideImage = slide;
		}
	}

	// Token: 0x0600AE57 RID: 44631 RVA: 0x00418D7C File Offset: 0x00416F7C
	public void updateSize(Sprite sprite)
	{
		Vector2 fittedSize = this.GetFittedSize(sprite, 960f, 960f);
		base.GetComponent<RectTransform>().sizeDelta = fittedSize;
	}

	// Token: 0x0600AE58 RID: 44632 RVA: 0x00111705 File Offset: 0x0010F905
	public void SetSprites(Sprite[] sprites)
	{
		if (sprites == null)
		{
			return;
		}
		this.sprites = sprites;
		this.resetSlide(sprites.Length != 0 && sprites[0] != null);
		if (sprites.Length != 0 && sprites[0] != null)
		{
			this.setSlide(sprites[0]);
		}
	}

	// Token: 0x0600AE59 RID: 44633 RVA: 0x00418DA8 File Offset: 0x00416FA8
	public Vector2 GetFittedSize(Sprite sprite, float maxWidth, float maxHeight)
	{
		if (sprite == null || sprite.texture == null)
		{
			return Vector2.zero;
		}
		int width = sprite.texture.width;
		int height = sprite.texture.height;
		float num = maxWidth / (float)width;
		float num2 = maxHeight / (float)height;
		if (num < num2)
		{
			return new Vector2((float)width * num, (float)height * num);
		}
		return new Vector2((float)width * num2, (float)height * num2);
	}

	// Token: 0x0600AE5A RID: 44634 RVA: 0x00111740 File Offset: 0x0010F940
	public void setSlide(Sprite slide)
	{
		if (slide == null)
		{
			return;
		}
		this.imageTarget.texture = slide.texture;
		this.updateSize(slide);
	}

	// Token: 0x0600AE5B RID: 44635 RVA: 0x00111764 File Offset: 0x0010F964
	public void nextSlide()
	{
		this.setSlideIndex(this.currentSlide + 1);
	}

	// Token: 0x0600AE5C RID: 44636 RVA: 0x00111774 File Offset: 0x0010F974
	public void prevSlide()
	{
		this.setSlideIndex(this.currentSlide - 1);
	}

	// Token: 0x0600AE5D RID: 44637 RVA: 0x00418E14 File Offset: 0x00417014
	private void setSlideIndex(int slideIndex)
	{
		this.timeUntilNextSlide = this.timePerSlide;
		SlideshowUpdateType slideshowUpdateType = this.updateType;
		if (slideshowUpdateType != SlideshowUpdateType.preloadedSprites)
		{
			if (slideshowUpdateType != SlideshowUpdateType.loadOnDemand)
			{
				return;
			}
			if (slideIndex < 0)
			{
				slideIndex = this.files.Length + slideIndex;
			}
			this.currentSlide = slideIndex % this.files.Length;
			if (this.currentSlide == this.files.Length - 1)
			{
				this.timeUntilNextSlide *= this.timeFactorForLastSlide;
			}
			if (this.playInThumbnail)
			{
				if (this.currentSlideImage != null)
				{
					UnityEngine.Object.Destroy(this.currentSlideImage.texture);
					UnityEngine.Object.Destroy(this.currentSlideImage);
					GC.Collect();
				}
				this.currentSlideImage = this.loadSlide(this.files[this.currentSlide]);
				this.setSlide(this.currentSlideImage);
			}
		}
		else
		{
			if (slideIndex < 0)
			{
				slideIndex = this.sprites.Length + slideIndex;
			}
			this.currentSlide = slideIndex % this.sprites.Length;
			if (this.currentSlide == this.sprites.Length - 1)
			{
				this.timeUntilNextSlide *= this.timeFactorForLastSlide;
			}
			if (this.playInThumbnail)
			{
				this.setSlide(this.sprites[this.currentSlide]);
				return;
			}
		}
	}

	// Token: 0x0600AE5E RID: 44638 RVA: 0x00418F40 File Offset: 0x00417140
	private void Update()
	{
		if (this.updateType == SlideshowUpdateType.preloadedSprites && (this.sprites == null || this.sprites.Length == 0))
		{
			return;
		}
		if (this.updateType == SlideshowUpdateType.loadOnDemand && (this.files == null || this.files.Length == 0))
		{
			return;
		}
		if (this.paused)
		{
			return;
		}
		this.timeUntilNextSlide -= Time.unscaledDeltaTime;
		if (this.timeUntilNextSlide <= 0f)
		{
			this.nextSlide();
		}
	}

	// Token: 0x04008913 RID: 35091
	public RawImage imageTarget;

	// Token: 0x04008914 RID: 35092
	private string[] files;

	// Token: 0x04008915 RID: 35093
	private Sprite currentSlideImage;

	// Token: 0x04008916 RID: 35094
	private Sprite[] sprites;

	// Token: 0x04008917 RID: 35095
	public float timePerSlide = 1f;

	// Token: 0x04008918 RID: 35096
	public float timeFactorForLastSlide = 3f;

	// Token: 0x04008919 RID: 35097
	private int currentSlide;

	// Token: 0x0400891A RID: 35098
	private float timeUntilNextSlide;

	// Token: 0x0400891B RID: 35099
	private bool paused;

	// Token: 0x0400891C RID: 35100
	public bool playInThumbnail;

	// Token: 0x0400891D RID: 35101
	public SlideshowUpdateType updateType;

	// Token: 0x0400891E RID: 35102
	[SerializeField]
	private bool isExpandable;

	// Token: 0x0400891F RID: 35103
	[SerializeField]
	private KButton button;

	// Token: 0x04008920 RID: 35104
	[SerializeField]
	private bool transparentIfEmpty = true;

	// Token: 0x04008921 RID: 35105
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04008922 RID: 35106
	[SerializeField]
	private KButton prevButton;

	// Token: 0x04008923 RID: 35107
	[SerializeField]
	private KButton nextButton;

	// Token: 0x04008924 RID: 35108
	[SerializeField]
	private KButton pauseButton;

	// Token: 0x04008925 RID: 35109
	[SerializeField]
	private Image pauseIcon;

	// Token: 0x04008926 RID: 35110
	[SerializeField]
	private Image unpauseIcon;

	// Token: 0x04008927 RID: 35111
	public Slideshow.onBeforeAndEndPlayDelegate onBeforePlay;

	// Token: 0x04008928 RID: 35112
	public Slideshow.onBeforeAndEndPlayDelegate onEndingPlay;

	// Token: 0x02002009 RID: 8201
	// (Invoke) Token: 0x0600AE66 RID: 44646
	public delegate void onBeforeAndEndPlayDelegate();
}
