using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/Slideshow")]
public class Slideshow : KMonoBehaviour {
    public delegate void onBeforeAndEndPlayDelegate();

    [SerializeField]
    private KButton button;

    [SerializeField]
    private KButton closeButton;

    private int      currentSlide;
    private Sprite   currentSlideImage;
    private string[] files;
    public  RawImage imageTarget;

    [SerializeField]
    private bool isExpandable;

    [SerializeField]
    private KButton nextButton;

    public onBeforeAndEndPlayDelegate onBeforePlay;
    public onBeforeAndEndPlayDelegate onEndingPlay;

    [SerializeField]
    private KButton pauseButton;

    private bool paused;

    [SerializeField]
    private Image pauseIcon;

    public bool playInThumbnail;

    [SerializeField]
    private KButton prevButton;

    private Sprite[] sprites;
    public  float    timeFactorForLastSlide = 3f;
    public  float    timePerSlide           = 1f;
    private float    timeUntilNextSlide;

    [SerializeField]
    private readonly bool transparentIfEmpty = true;

    [SerializeField]
    private Image unpauseIcon;

    public SlideshowUpdateType updateType;

    protected override void OnSpawn() {
        base.OnSpawn();
        timeUntilNextSlide = timePerSlide;
        if (transparentIfEmpty && sprites != null && sprites.Length == 0) imageTarget.color = Color.clear;
        if (isExpandable) {
            button = GetComponent<KButton>();
            button.onClick += delegate {
                                  if (onBeforePlay != null) onBeforePlay();
                                  var slideshowUpdateType = updateType;
                                  if (slideshowUpdateType == SlideshowUpdateType.preloadedSprites) {
                                      VideoScreen.Instance.PlaySlideShow(sprites);
                                      return;
                                  }

                                  if (slideshowUpdateType != SlideshowUpdateType.loadOnDemand) return;

                                  VideoScreen.Instance.PlaySlideShow(files);
                              };
        }

        if (nextButton != null) nextButton.onClick += delegate { nextSlide(); };

        if (prevButton != null) prevButton.onClick += delegate { prevSlide(); };

        if (pauseButton != null) pauseButton.onClick += delegate { SetPaused(!paused); };

        if (closeButton != null)
            closeButton.onClick += delegate {
                                       VideoScreen.Instance.Stop();
                                       if (onEndingPlay != null) onEndingPlay();
                                   };
    }

    public void SetPaused(bool state) {
        paused = state;
        if (pauseIcon   != null) pauseIcon.gameObject.SetActive(!paused);
        if (unpauseIcon != null) unpauseIcon.gameObject.SetActive(paused);
        if (prevButton  != null) prevButton.gameObject.SetActive(paused);
        if (nextButton  != null) nextButton.gameObject.SetActive(paused);
    }

    private void resetSlide(bool enable) {
        timeUntilNextSlide = timePerSlide;
        currentSlide       = 0;
        if (enable) {
            imageTarget.color = Color.white;
            return;
        }

        if (transparentIfEmpty) imageTarget.color = Color.clear;
    }

    private Sprite loadSlide(string file) {
        var realtimeSinceStartup = Time.realtimeSinceStartup;
        var texture2D            = new Texture2D(512, 768);
        texture2D.filterMode = FilterMode.Point;
        texture2D.LoadImage(File.ReadAllBytes(file));
        return Sprite.Create(texture2D,
                             new Rect(Vector2.zero, new Vector2(texture2D.width, texture2D.height)),
                             new Vector2(0.5f, 0.5f),
                             100f,
                             0U,
                             SpriteMeshType.FullRect);
    }

    public void SetFiles(string[] files, int loadFrame = -1) {
        if (files == null) return;

        this.files = files;
        var flag = files.Length != 0 && files[0] != null;
        resetSlide(flag);
        if (flag) {
            var num   = loadFrame != -1 ? loadFrame : files.Length - 1;
            var file  = files[num];
            var slide = loadSlide(file);
            setSlide(slide);
            currentSlideImage = slide;
        }
    }

    public void updateSize(Sprite sprite) {
        var fittedSize = GetFittedSize(sprite, 960f, 960f);
        GetComponent<RectTransform>().sizeDelta = fittedSize;
    }

    public void SetSprites(Sprite[] sprites) {
        if (sprites == null) return;

        this.sprites = sprites;
        resetSlide(sprites.Length != 0 && sprites[0] != null);
        if (sprites.Length != 0 && sprites[0] != null) setSlide(sprites[0]);
    }

    public Vector2 GetFittedSize(Sprite sprite, float maxWidth, float maxHeight) {
        if (sprite == null || sprite.texture == null) return Vector2.zero;

        var width  = sprite.texture.width;
        var height = sprite.texture.height;
        var num    = maxWidth  / width;
        var num2   = maxHeight / height;
        if (num < num2) return new Vector2(width * num, height * num);

        return new Vector2(width * num2, height * num2);
    }

    public void setSlide(Sprite slide) {
        if (slide == null) return;

        imageTarget.texture = slide.texture;
        updateSize(slide);
    }

    public void nextSlide() { setSlideIndex(currentSlide + 1); }
    public void prevSlide() { setSlideIndex(currentSlide - 1); }

    private void setSlideIndex(int slideIndex) {
        timeUntilNextSlide = timePerSlide;
        var slideshowUpdateType = updateType;
        if (slideshowUpdateType != SlideshowUpdateType.preloadedSprites) {
            if (slideshowUpdateType != SlideshowUpdateType.loadOnDemand) return;

            if (slideIndex < 0) slideIndex = files.Length + slideIndex;
            currentSlide = slideIndex % files.Length;
            if (currentSlide == files.Length - 1) timeUntilNextSlide *= timeFactorForLastSlide;
            if (playInThumbnail) {
                if (currentSlideImage != null) {
                    Destroy(currentSlideImage.texture);
                    Destroy(currentSlideImage);
                    GC.Collect();
                }

                currentSlideImage = loadSlide(files[currentSlide]);
                setSlide(currentSlideImage);
            }
        } else {
            if (slideIndex < 0) slideIndex = sprites.Length + slideIndex;
            currentSlide = slideIndex % sprites.Length;
            if (currentSlide == sprites.Length - 1) timeUntilNextSlide *= timeFactorForLastSlide;
            if (playInThumbnail) setSlide(sprites[currentSlide]);
        }
    }

    private void Update() {
        if (updateType == SlideshowUpdateType.preloadedSprites && (sprites == null || sprites.Length == 0)) return;

        if (updateType == SlideshowUpdateType.loadOnDemand && (files == null || files.Length == 0)) return;

        if (paused) return;

        timeUntilNextSlide -= Time.unscaledDeltaTime;
        if (timeUntilNextSlide <= 0f) nextSlide();
    }
}