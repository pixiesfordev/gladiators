
using Cysharp.Threading.Tasks;
using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

namespace Scoz.Func {
    public sealed class AddressablesLoader {
        public static void GetAssetRef<T>(AssetReference _ref, Action<T> _cb) {
            if (_ref == null || !_ref.RuntimeKeyIsValid()) {
                WriteLog.LogError("GetAssetRef 傳入 null AssetReference");
                _cb?.Invoke(default(T));
                return;
            }

            Addressables.LoadAssetAsync<T>(_ref).Completed += handle => {

                switch (handle.Status) {
                    case AsyncOperationStatus.Succeeded:
                        _cb?.Invoke(handle.Result);
                        break;
                    default:
                        // WriteLog.LogError("讀取資源失敗:" + _path);
                        break;
                }
            };
        }
        public static void GetAudioClipByRef(AssetReference _ref, Action<AudioClip, AsyncOperationHandle> _cb) {
            if (_ref == null || !_ref.RuntimeKeyIsValid()) {
                WriteLog.LogError("GetAudioClipByRef 傳入不合法的 AssetReference");
                return;
            }
            Addressables.LoadAssetAsync<AudioClip>(_ref).Completed += handle => {
                _cb?.Invoke(handle.Result, handle);
            };
        }
        public static void GetPrefabByRef(AssetReference _ref, Action<GameObject, AsyncOperationHandle> _cb, Action _notExistCB = null) {
            if (_ref == null) {
                WriteLog.LogError("GetPrefabByRef 傳入不合法的 AssetReference");
                return;
            }
            Addressables.LoadResourceLocationsAsync(_ref).Completed += check => {
                if (check.Status == AsyncOperationStatus.Succeeded) {
                    if (check.Result.Count > 0) {
                        Addressables.LoadAssetAsync<GameObject>(_ref).Completed += handle => {
                            switch (handle.Status) {
                                case AsyncOperationStatus.Succeeded:
                                    _cb?.Invoke(handle.Result, handle);
                                    break;
                                default:
                                    WriteLog.LogErrorFormat("載入失敗: " + _ref);
                                    _notExistCB?.Invoke();
                                    break;
                            }
                        };
                    } else {
                        WriteLog.LogErrorFormat("找不到Prefab: " + _ref);
                        _notExistCB?.Invoke();
                    }
                } else {
                    WriteLog.LogErrorFormat("找不到Prefab:" + _ref);
                    _notExistCB?.Invoke();
                }
            };
        }

        public static void GetResourceByFullPath<T>(string _fullPath, Action<T, AsyncOperationHandle> _cb) {
            Addressables.LoadAssetAsync<T>(_fullPath).Completed += handle => {
                if (string.IsNullOrEmpty(_fullPath)) {
                    WriteLog.LogError("GetResourceByFullPath 傳入Path為空");
                    return;
                }
                switch (handle.Status) {
                    case AsyncOperationStatus.Succeeded:
                        _cb?.Invoke(handle.Result, handle);
                        break;
                    default:
                        //WriteLog.LogError("讀取資源失敗:" + _path);
                        break;
                }
                //Addressables.Release(handle);
            };
        }
        public static async UniTask<Tuple<T, AsyncOperationHandle>> GetResourceByFullPath_Async<T>(string _fullPath) {
            if (string.IsNullOrEmpty(_fullPath)) {
                WriteLog.LogError("GetResourceByFullPath_Async 傳入Path為空");
                return new Tuple<T, AsyncOperationHandle>(default(T), default(AsyncOperationHandle));
            }
            var handle = Addressables.LoadAssetAsync<T>(_fullPath);
            await handle.ToUniTask();

            switch (handle.Status) {
                case AsyncOperationStatus.Succeeded:
                    return new Tuple<T, AsyncOperationHandle>(handle.Result, handle);
                default:
                    // WriteLog.LogError("讀取資源失敗:" + _path);
                    return new Tuple<T, AsyncOperationHandle>(default(T), handle);
            }
        }


        public static void GetSpriteAtlas(string _path, Action<SpriteAtlas> _cb) {
            if (string.IsNullOrEmpty(_path)) {
                WriteLog.LogError("GetSpriteAtlas 傳入Path為空");
                _cb?.Invoke(null);
                return;
            }
            _path = string.Format("Assets/AddressableAssets/Atlas/{0}.spriteatlasv2", _path);

            Addressables.LoadAssetAsync<SpriteAtlas>(_path).Completed += handle => {
                switch (handle.Status) {
                    case AsyncOperationStatus.Succeeded:
                        _cb?.Invoke(handle.Result);
                        //WriteLog.Log("讀取Atlas成功:"+_path);
                        break;
                    default:
                        // WriteLog.LogError("讀取資源失敗:" + _path);
                        break;
                }
                //Addressables.Release(handle);
            };
        }
        public static void GetSprite(string _path, Action<Sprite, AsyncOperationHandle> _cb) {
            if (string.IsNullOrEmpty(_path)) {
                WriteLog.LogError("GetSprite 傳入Path為空");
                return;
            }

            _path = string.Format("Assets/AddressableAssets/Images/{0}.png", _path);
            Addressables.LoadAssetAsync<Sprite>(_path).Completed += handle => {
                switch (handle.Status) {
                    case AsyncOperationStatus.Succeeded:
                        _cb?.Invoke(handle.Result, handle);
                        break;
                    default:
                        // WriteLog.LogError("讀取資源失敗:" + _path);
                        break;
                }
            };
        }
        public static void GetMultipleSprites(string _path, Action<Sprite[], AsyncOperationHandle> _cb) {
            if (string.IsNullOrEmpty(_path)) {
                WriteLog.LogError("GetMultipleSprites 傳入Path為空");
                return;
            }

            _path = string.Format("Assets/AddressableAssets/Images/{0}.png", _path);
            Addressables.LoadAssetAsync<Sprite[]>(_path).Completed += handle => {
                switch (handle.Status) {
                    case AsyncOperationStatus.Succeeded:
                        _cb?.Invoke(handle.Result, handle);
                        break;
                    default:
                        // WriteLog.LogError("讀取資源失敗:" + _path);
                        break;
                }
            };
        }
        public static void GetParticle(string _path, Action<GameObject, AsyncOperationHandle> _cb) {
            if (string.IsNullOrEmpty(_path)) {
                WriteLog.LogError("GetParticle 傳入Path為空");
                return;
            }

            _path = string.Format("Assets/AddressableAssets/Particles/{0}.prefab", _path);

            Addressables.LoadAssetAsync<GameObject>(_path).Completed += handle => {
                switch (handle.Status) {
                    case AsyncOperationStatus.Succeeded:
                        _cb?.Invoke(handle.Result, handle);
                        break;
                    default:
                        // WriteLog.LogError("讀取資源失敗:" + _path);
                        break;
                }
            };
        }
        public static void GetTexture(string _path, Action<Texture, AsyncOperationHandle> _cb, Action _notExistCB = null) {
            if (string.IsNullOrEmpty(_path)) {
                WriteLog.LogError("GetTexture 傳入Path為空");
                return;
            }
            _path = string.Format("Assets/AddressableAssets/Textures/{0}.png", _path);

            Addressables.LoadResourceLocationsAsync(_path).Completed += check => {
                if (check.Status == AsyncOperationStatus.Succeeded) {
                    if (check.Result.Count > 0) {
                        Addressables.LoadAssetAsync<Texture>(_path).Completed += handle => {
                            switch (handle.Status) {
                                case AsyncOperationStatus.Succeeded:
                                    _cb?.Invoke(handle.Result, handle);
                                    break;
                            }
                        };
                    } else {
                        WriteLog.LogErrorFormat("找不到Texture: " + _path);
                        _notExistCB?.Invoke();
                    }
                } else {
                    WriteLog.LogErrorFormat("找不到Texture:" + _path);
                    _notExistCB?.Invoke();
                }
            };

        }
        public static void GetTextureWithIndex(string _path, int _index, Action<Texture, AsyncOperationHandle, int> _cb, Action _notExistCB = null) {
            if (string.IsNullOrEmpty(_path)) {
                WriteLog.LogError("GetTextureWithIndex 傳入Path為空");
                return;
            }

            _path = string.Format("Assets/AddressableAssets/Textures/{0}.png", _path);



            Addressables.LoadResourceLocationsAsync(_path).Completed += check => {
                if (check.Status == AsyncOperationStatus.Succeeded) {
                    if (check.Result.Count > 0) {
                        Addressables.LoadAssetAsync<Texture>(_path).Completed += handle => {
                            switch (handle.Status) {
                                case AsyncOperationStatus.Succeeded:
                                    _cb?.Invoke(handle.Result, handle, _index);
                                    break;
                            }
                        };
                    } else {
                        WriteLog.LogErrorFormat("找不到Texture: " + _path);
                        _notExistCB?.Invoke();
                    }
                } else {
                    WriteLog.LogErrorFormat("找不到Texture:" + _path);
                    _notExistCB?.Invoke();
                }
            };

        }


        public static void GetPrefab(string _path, Action<GameObject, AsyncOperationHandle> _cb, Action _notExistCB = null) {
            if (string.IsNullOrEmpty(_path)) {
                WriteLog.LogError("GetPrefab 傳入Path為空");
                return;
            }
            _path = string.Format("Assets/AddressableAssets/Prefabs/{0}.prefab", _path);

            Addressables.LoadResourceLocationsAsync(_path).Completed += check => {
                if (check.Status == AsyncOperationStatus.Succeeded) {
                    if (check.Result.Count > 0) {
                        Addressables.LoadAssetAsync<GameObject>(_path).Completed += handle => {
                            switch (handle.Status) {
                                case AsyncOperationStatus.Succeeded:
                                    _cb?.Invoke(handle.Result, handle);
                                    break;
                            }
                        };
                    } else {
                        WriteLog.LogErrorFormat("找不到Prefab: " + _path);
                        _notExistCB?.Invoke();
                    }
                } else {
                    WriteLog.LogErrorFormat("找不到Prefab:" + _path);
                    _notExistCB?.Invoke();
                }
            };
        }

        public static void GetAudio(MyAudioType _type, string _name, Action<AudioClip> _cb) {
            if (string.IsNullOrEmpty(_name)) {
                WriteLog.LogError("GetAudio 傳入Path為空");
                _cb?.Invoke(null);
                return;
            }
            string fileExtension = "mp3";//統一用mp3

            string path = string.Format("Assets/AddressableAssets/Audios/{0}/{1}.{2}", _type.ToString(), _name, fileExtension);
            Addressables.LoadAssetAsync<AudioClip>(path).Completed += handle => {
                switch (handle.Status) {
                    case AsyncOperationStatus.Succeeded:
                        _cb?.Invoke(handle.Result);
                        break;
                    default:
                        // WriteLog.LogError("讀取資源失敗:" + _path);
                        break;
                }
                //Addressables.Release(handle);
            };
        }
        public enum ControllerFileExtention {
            controller,
            overrideController
        }
        public static void GetController(string _path, ControllerFileExtention _fileExtension, Action<RuntimeAnimatorController> _cb) {
            if (string.IsNullOrEmpty(_path)) {
                WriteLog.LogError("GetController 傳入Path為空");
                return;
            }
            _path = string.Format("Assets/AddressableAssets/Animations/{0}.{1}", _path, _fileExtension);
            Addressables.LoadAssetAsync<RuntimeAnimatorController>(_path).Completed += handle => {
                switch (handle.Status) {
                    case AsyncOperationStatus.Succeeded:
                        _cb?.Invoke(handle.Result);
                        break;
                    default:
                        // WriteLog.LogError("讀取資源失敗:" + _path);
                        break;
                }
                //Addressables.Release(handle);
            };

        }
        public static void GetAdditiveScene(string _path, Action<Scene?, AsyncOperationHandle> _cb) {
            if (string.IsNullOrEmpty(_path)) {
                WriteLog.LogError("LoadAdditiveScene 傳入Path為空");
                return;
            }
            _path = string.Format("Assets/AddressableAssets/Scenes/{0}.unity", _path);
            Addressables.LoadSceneAsync(_path, LoadSceneMode.Additive).Completed += handle => {
                switch (handle.Status) {
                    case AsyncOperationStatus.Succeeded:
                        _cb?.Invoke(handle.Result.Scene, handle);
                        break;
                    default:
                        WriteLog.LogError("讀取場景失敗:" + _path);
                        _cb?.Invoke(null, handle);
                        break;
                }
            };
        }
        public static void GetVolumeProflie(string _path, Action<VolumeProfile> _cb) {
            if (string.IsNullOrEmpty(_path)) {
                WriteLog.LogError("GetVolumeProflie 傳入Path為空");
                _cb?.Invoke(null);
                return;
            }
            _path = string.Format("Assets/AddressableAssets/Settings/VolumeProfile/{0}.asset", _path);
            Addressables.LoadAssetAsync<VolumeProfile>(_path).Completed += handle => {
                switch (handle.Status) {
                    case AsyncOperationStatus.Succeeded:
                        _cb?.Invoke(handle.Result);
                        handle.Release();
                        break;
                    default:
                        WriteLog.LogError("讀取VolumeProfile失敗:" + _path);
                        _cb?.Invoke(null);
                        break;
                }
            };
        }



    }
}
