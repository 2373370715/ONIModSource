using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02001E42 RID: 7746
public class MotdDataFetchRequest : IDisposable
{
	// Token: 0x0600A240 RID: 41536 RVA: 0x00109476 File Offset: 0x00107676
	public void Dispose()
	{
		this.onCompleteFn = null;
	}

	// Token: 0x0600A241 RID: 41537 RVA: 0x0010947F File Offset: 0x0010767F
	public void Fetch(string url)
	{
		MotdDataFetchRequest.FetchWebMotdJson(url, delegate(MotdData webMotd)
		{
			this.data = webMotd;
			if (webMotd == null)
			{
				global::Debug.LogWarning("MOTD Error: failed to get web motd json");
				this.<Fetch>g__CompleteWith|4_1(null);
				return;
			}
			MotdDataFetchRequest.FetchWebMotdImagesFor(webMotd, delegate(bool isOk)
			{
				using (List<MotdData_Box>.Enumerator enumerator = webMotd.boxesLive.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.resolvedImage.IsNullOrDestroyed())
						{
							isOk = false;
						}
					}
				}
				if (!isOk)
				{
					global::Debug.LogWarning("MOTD Error: couldn't fetch all web motd images");
					this.<Fetch>g__CompleteWith|4_1(null);
					return;
				}
				MotdDataFetchRequest.WriteCachedMotdImages(webMotd);
				this.<Fetch>g__CompleteWith|4_1(webMotd);
			});
		});
	}

	// Token: 0x0600A242 RID: 41538 RVA: 0x00109493 File Offset: 0x00107693
	public void OnComplete(Action<MotdData> callbackFn)
	{
		if (this.isComplete)
		{
			callbackFn(this.data);
			return;
		}
		this.onCompleteFn = (Action<MotdData>)Delegate.Combine(this.onCompleteFn, callbackFn);
	}

	// Token: 0x0600A243 RID: 41539 RVA: 0x003DC824 File Offset: 0x003DAA24
	public static void FetchWebMotdJson(string url, Action<MotdData> onCompleteFn)
	{
		UnityWebRequest webRequest = UnityWebRequest.Get(url);
		webRequest.timeout = 3;
		webRequest.SetRequestHeader("Content-Type", "application/json");
		webRequest.SendWebRequest().completed += delegate(AsyncOperation operation)
		{
			if (string.IsNullOrEmpty(webRequest.error))
			{
				onCompleteFn(MotdData.Parse(webRequest.downloadHandler.text));
			}
			else
			{
				global::Debug.LogWarning("MOTD Error: failed to fetch web motd. " + webRequest.error);
				onCompleteFn(null);
			}
			webRequest.Dispose();
		};
	}

	// Token: 0x0600A244 RID: 41540 RVA: 0x003DC888 File Offset: 0x003DAA88
	public static void FetchWebMotdImagesFor(MotdData motdData, Action<bool> onCompleteFn)
	{
		using (List<MotdData_Box>.Enumerator enumerator = motdData.boxesLive.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.image == null)
				{
					onCompleteFn(false);
					return;
				}
			}
		}
		int imagesToFetchCount = motdData.boxesLive.Count;
		if (imagesToFetchCount == 0)
		{
			onCompleteFn(false);
			return;
		}
		int imagesValidCount = 0;
		using (List<MotdData_Box>.Enumerator enumerator = motdData.boxesLive.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MotdData_Box box = enumerator.Current;
				MotdDataFetchRequest.FetchWebMotdImage(box.image, delegate(Texture2D resolvedImage, bool isFromDisk)
				{
					imagesToFetchCount--;
					box.resolvedImage = resolvedImage;
					box.resolvedImageIsFromDisk = isFromDisk;
					if (box.resolvedImage != null)
					{
						imagesValidCount++;
					}
					if (imagesToFetchCount == 0)
					{
						onCompleteFn(imagesValidCount == motdData.boxesLive.Count);
					}
				});
			}
		}
	}

	// Token: 0x0600A245 RID: 41541 RVA: 0x003DC9A4 File Offset: 0x003DABA4
	public static void FetchWebMotdImage(string url, Action<Texture2D, bool> onCompleteFn)
	{
		Texture2D texture2D = MotdDataFetchRequest.ReadCachedMotdImage(url);
		if (texture2D != null)
		{
			onCompleteFn(texture2D, true);
			return;
		}
		UnityWebRequest webRequest = UnityWebRequest.Get(url);
		webRequest.timeout = 3;
		webRequest.SendWebRequest().completed += delegate(AsyncOperation operation)
		{
			if (string.IsNullOrEmpty(webRequest.error))
			{
				onCompleteFn(MotdDataFetchRequest.ParseImage(webRequest.downloadHandler.data), false);
			}
			else
			{
				global::Debug.LogWarning("MOTD Error: failed to fetch web image at " + url + ". " + webRequest.error);
				onCompleteFn(null, false);
			}
			webRequest.Dispose();
		};
	}

	// Token: 0x0600A246 RID: 41542 RVA: 0x001094C1 File Offset: 0x001076C1
	public static string GetCachePath()
	{
		return Path.Combine(Util.CacheFolder(), "motd");
	}

	// Token: 0x0600A247 RID: 41543 RVA: 0x001094D2 File Offset: 0x001076D2
	public static string GetCachedFilePath(string filePath)
	{
		return Path.Combine(Util.CacheFolder(), "motd", Path.GetFileName(filePath));
	}

	// Token: 0x0600A248 RID: 41544 RVA: 0x003DCA24 File Offset: 0x003DAC24
	public static void WriteCachedMotdImages(MotdData data)
	{
		if (data == null)
		{
			return;
		}
		try
		{
			if (!Directory.Exists(MotdDataFetchRequest.GetCachePath()))
			{
				Directory.CreateDirectory(MotdDataFetchRequest.GetCachePath());
			}
		}
		catch (Exception arg)
		{
			global::Debug.LogWarning(string.Format("MOTD Error: Failed to create image cache directory --- {0}", arg));
		}
		try
		{
			if (Directory.Exists(MotdDataFetchRequest.GetCachePath()))
			{
				using (List<MotdData_Box>.Enumerator enumerator = data.boxesLive.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MotdData_Box motdData_Box = enumerator.Current;
						if (motdData_Box.image != null && motdData_Box.resolvedImage != null && !motdData_Box.resolvedImageIsFromDisk)
						{
							File.WriteAllBytes(MotdDataFetchRequest.GetCachedFilePath(motdData_Box.image), motdData_Box.resolvedImage.EncodeToPNG());
						}
					}
					goto IL_B0;
				}
			}
			global::Debug.LogWarning("MOTD Error: Failed to write cached motd images, couldn't find a valid cache directory");
			IL_B0:;
		}
		catch (Exception arg2)
		{
			global::Debug.LogWarning(string.Format("MOTD Error: Failed to write cached motd images --- {0}", arg2));
		}
		try
		{
			if (Directory.Exists(MotdDataFetchRequest.GetCachePath()))
			{
				List<string> list = new List<string>(16);
				foreach (MotdData_Box motdData_Box2 in data.boxesLive)
				{
					if (motdData_Box2.image != null)
					{
						list.Add(MotdDataFetchRequest.GetCachedFilePath(motdData_Box2.image));
					}
				}
				foreach (string text in Directory.GetFiles(MotdDataFetchRequest.GetCachePath()))
				{
					if (!list.Contains(MotdDataFetchRequest.GetCachedFilePath(text)))
					{
						File.Delete(text);
					}
				}
			}
			else
			{
				global::Debug.LogWarning("MOTD Error: Failed to clean cached motd images, couldn't find a valid cache directory");
			}
		}
		catch (Exception arg3)
		{
			global::Debug.LogWarning(string.Format("MOTD Error: Failed to clean cached motd images --- {0}", arg3));
		}
	}

	// Token: 0x0600A249 RID: 41545 RVA: 0x003DCBFC File Offset: 0x003DADFC
	public static Texture2D ReadCachedMotdImage(string url)
	{
		string fileName = Path.GetFileName(url);
		string cachedFilePath = MotdDataFetchRequest.GetCachedFilePath(fileName);
		if (!File.Exists(cachedFilePath))
		{
			return null;
		}
		Texture2D result;
		try
		{
			result = MotdDataFetchRequest.ParseImage(File.ReadAllBytes(cachedFilePath));
		}
		catch (Exception arg)
		{
			global::Debug.LogWarning(string.Format("MOTD Error: Can't load cached motd image \"{0}\" --- {1}", fileName, arg));
			result = null;
		}
		return result;
	}

	// Token: 0x0600A24A RID: 41546 RVA: 0x003DCC58 File Offset: 0x003DAE58
	public static string GetLocaleCode()
	{
		Localization.Locale locale = Localization.GetLocale();
		if (locale != null)
		{
			Localization.Language lang = locale.Lang;
			if (lang == Localization.Language.Chinese || lang - Localization.Language.Korean <= 1)
			{
				return locale.Code;
			}
		}
		return null;
	}

	// Token: 0x0600A24B RID: 41547 RVA: 0x003DCC88 File Offset: 0x003DAE88
	public static Texture2D ParseImage(byte[] buffer)
	{
		if (MotdDataFetchRequest.<ParseImage>g__IsPng|14_0(buffer) || MotdDataFetchRequest.<ParseImage>g__IsJpg|14_1(buffer))
		{
			Texture2D texture2D = new Texture2D(0, 0);
			texture2D.LoadImage(buffer);
			return texture2D;
		}
		if (MotdDataFetchRequest.<ParseImage>g__IsKleiTex|14_2(buffer))
		{
			global::Debug.LogWarning("MOTD Error: Couldn't load image - KTEX isn't supported yet.");
			return null;
		}
		global::Debug.LogWarning("MOTD Error: Couldn't load image - Unsupported image file format.");
		return null;
	}

	// Token: 0x0600A24C RID: 41548 RVA: 0x003DCCD4 File Offset: 0x003DAED4
	public static void GetUrlParams(out string platformCode, out string languageCode)
	{
		platformCode = "default";
		if ((((Localization.GetLocale() == null) ? null : new Localization.Language?(Localization.GetLocale().Lang)) ?? Localization.Language.Japanese) == Localization.Language.Chinese)
		{
			languageCode = "schinese";
			return;
		}
		languageCode = "en";
	}

	// Token: 0x0600A24D RID: 41549 RVA: 0x003DCD2C File Offset: 0x003DAF2C
	public static string BuildUrl()
	{
		string str;
		string str2;
		MotdDataFetchRequest.GetUrlParams(out str, out str2);
		return "https://motd.klei.com/motd.json/?game=oni&platform=" + str + "&lang=" + str2;
	}

	// Token: 0x0600A250 RID: 41552 RVA: 0x001094E9 File Offset: 0x001076E9
	[CompilerGenerated]
	private void <Fetch>g__CompleteWith|4_1(MotdData data)
	{
		if (this.isComplete)
		{
			return;
		}
		this.isComplete = true;
		this.data = data;
		if (this.onCompleteFn != null)
		{
			this.onCompleteFn(data);
		}
	}

	// Token: 0x0600A251 RID: 41553 RVA: 0x00109516 File Offset: 0x00107716
	[CompilerGenerated]
	internal static bool <ParseImage>g__IsPng|14_0(byte[] buffer)
	{
		return buffer[0] == 137 && buffer[1] == 80 && buffer[2] == 78 && buffer[3] == 71 && buffer[4] == 13 && buffer[5] == 10 && buffer[6] == 26 && buffer[7] == 10;
	}

	// Token: 0x0600A252 RID: 41554 RVA: 0x00109555 File Offset: 0x00107755
	[CompilerGenerated]
	internal static bool <ParseImage>g__IsJpg|14_1(byte[] buffer)
	{
		return buffer[0] == byte.MaxValue && buffer[1] == 216 && buffer[6] == 74 && buffer[7] == 70 && buffer[8] == 73 && buffer[9] == 70;
	}

	// Token: 0x0600A253 RID: 41555 RVA: 0x0010958A File Offset: 0x0010778A
	[CompilerGenerated]
	internal static bool <ParseImage>g__IsKleiTex|14_2(byte[] buffer)
	{
		return buffer[0] == 75 && buffer[1] == 84 && buffer[2] == 69 && buffer[3] == 88;
	}

	// Token: 0x04007E9F RID: 32415
	private MotdData data;

	// Token: 0x04007EA0 RID: 32416
	private bool isComplete;

	// Token: 0x04007EA1 RID: 32417
	private Action<MotdData> onCompleteFn;
}
