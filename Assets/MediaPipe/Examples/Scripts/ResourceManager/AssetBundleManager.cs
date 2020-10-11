using Mediapipe;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public sealed class AssetBundleManager : ResourceManager {
  private static readonly Lazy<AssetBundleManager> lazy = new Lazy<AssetBundleManager>(() => new AssetBundleManager());
  public static AssetBundleManager Instance { get { return lazy.Value; } }
  private readonly static string CacheRootPath = Path.Combine(Application.persistentDataPath, "Cache");
  private readonly static string ModelCacheRootPath = Path.Combine(CacheRootPath, "MediaPipe", "Models");

  private AssetBundleManager() : base() {
    if (!Directory.Exists(ModelCacheRootPath)) {
      Directory.CreateDirectory(ModelCacheRootPath);
    }
  }

  public async Task LoadAllAssetsAsync() {
    Debug.Log("Starting to load assets");

    var bundleLoadReq = await AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, "AssetBundles", "mediapipe", "models"));

    if (bundleLoadReq.assetBundle == null) {
      Debug.LogError("Failed to load the AssetBundle");
      return;
    }

    var assetLoadReq = await bundleLoadReq.assetBundle.LoadAllAssetsAsync<TextAsset>();
    if (assetLoadReq.allAssets == null) {
      Debug.LogError("Failed to load assets");
      return;
    }

    var loadTasks = assetLoadReq.allAssets.Select((asset) => WriteCacheFileAsync((TextAsset)asset));
    await Task.WhenAll(loadTasks);
    Debug.Log("Loaded all assets");
  }

  protected override string CacheFileFromAsset(string assetPath) {
    var assetName = Path.GetFileNameWithoutExtension(assetPath);
    var cachePath = CachePathFor(assetName);

    if (File.Exists(cachePath)) {
      return cachePath;
    }

    return null;
  }

  protected override bool ReadFile(string path, IntPtr dst) {
    var cachePath = CacheFileFromAsset(path);

    if (!File.Exists(cachePath)) {
      return false;
    }

    var asset = File.ReadAllBytes(cachePath);
    ResourceUtil.CopyBytes(dst, asset);
    return true;
  }

  private string CachePathFor(string assetName) {
    return Path.Combine(ModelCacheRootPath, assetName);
  }

  private async Task WriteCacheFileAsync(TextAsset asset) {
    var path = CachePathFor(asset.name);
    var bytes = asset.bytes;
    Debug.Log($"Saving {asset.name} to {path} (length={bytes.Length})");

    FileStream sourceStream = null;

    try {
      sourceStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
      await sourceStream.WriteAsync(bytes, 0, bytes.Length);
    } finally {
      if (sourceStream != null) {
        sourceStream.Close();
      }
    }
  }
}
