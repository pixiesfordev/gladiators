using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"Cinemachine.dll",
		"DOTween.dll",
		"LitJson.dll",
		"Loxodon.Framework.dll",
		"Realm.dll",
		"SerializableDictionary.dll",
		"System.Core.dll",
		"System.dll",
		"UniRx.dll",
		"UniTask.dll",
		"Unity.Addressables.dll",
		"Unity.RenderPipelines.Core.Runtime.dll",
		"Unity.ResourceManager.dll",
		"Unity.VisualScripting.Core.dll",
		"UnityEngine.CoreModule.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<DBPlayer.<SetInMatchgameID>d__45>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Battle.BattleManager.<CheckGameState>d__22>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Battle.BattleManager.<CreateTerrainAndChar>d__27>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Battle.BattleManager.<Init>d__21>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Main.AllocatedRoom.<SetRoom>d__51>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Main.AllocatedRoom.<SetRoom_TestvVer>d__52>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Main.GamePlayer.<GetMatchGame>d__16,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Main.StartSceneUI.<InitPlayerData>d__27>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Socket.GameConnector.<ConnToMatchmaker>d__27>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Socket.GameConnector.<JoinMatchgame>d__16>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Socket.GameConnector.<OnLoginToMatchmakerError>d__29>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Socket.GameConnector.<SendRestfulAPI>d__7,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Scoz.Func.AddressablesLoader.<GetResourceByFullPath_Async>d__8<object>,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Scoz.Func.Poster.<Post>d__0,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Scoz.Func.UniTaskManager.<OneTimesTask>d__11>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Scoz.Func.UniTaskManager.<RepeatTask>d__9>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Service.Realms.RealmManager.<AnonymousSignup>d__30>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Service.Realms.RealmManager.<CallAtlasFunc_InitPlayerData>d__29,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Service.Realms.RealmManager.<GetProvider>d__33,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Service.Realms.RealmManager.<GetServerTime>d__35>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Service.Realms.RealmManager.<GetValidAccessToken>d__32,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Service.Realms.RealmManager.<OnSignin>d__34>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Service.Realms.RealmManager.<Query_GetDoc>d__38,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Service.Realms.RealmManager.<Signout>d__37>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<DBPlayer.<SetInMatchgameID>d__45>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Battle.BattleManager.<CheckGameState>d__22>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Battle.BattleManager.<CreateTerrainAndChar>d__27>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Battle.BattleManager.<Init>d__21>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Main.AllocatedRoom.<SetRoom>d__51>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Main.AllocatedRoom.<SetRoom_TestvVer>d__52>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Main.GamePlayer.<GetMatchGame>d__16,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Main.StartSceneUI.<InitPlayerData>d__27>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Socket.GameConnector.<ConnToMatchmaker>d__27>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Socket.GameConnector.<JoinMatchgame>d__16>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Socket.GameConnector.<OnLoginToMatchmakerError>d__29>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Socket.GameConnector.<SendRestfulAPI>d__7,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Scoz.Func.AddressablesLoader.<GetResourceByFullPath_Async>d__8<object>,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Scoz.Func.Poster.<Post>d__0,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Scoz.Func.UniTaskManager.<OneTimesTask>d__11>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Scoz.Func.UniTaskManager.<RepeatTask>d__9>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Service.Realms.RealmManager.<AnonymousSignup>d__30>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Service.Realms.RealmManager.<CallAtlasFunc_InitPlayerData>d__29,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Service.Realms.RealmManager.<GetProvider>d__33,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Service.Realms.RealmManager.<GetServerTime>d__35>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Service.Realms.RealmManager.<GetValidAccessToken>d__32,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Service.Realms.RealmManager.<OnSignin>d__34>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Service.Realms.RealmManager.<Query_GetDoc>d__38,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Service.Realms.RealmManager.<Signout>d__37>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<BattleGladiatorInfo.<HPChange>d__48>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<BattleGladiatorInfo.<HPGrayChange>d__49>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<Character.<>c__DisplayClass48_0.<<Knockback>b__0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<Gladiators.Battle.DivineSelectUI.<DoCloseAni>d__30>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<Gladiators.Battle.DivineSelectUI.<PlayCountDownCandleTime>d__20>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<Gladiators.Main.LobbySceneUI.<>c__DisplayClass17_0.<<RealmLoginCheck>b__0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<Gladiators.Main.StartSceneUI.<<AuthChek>b__21_0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<Gladiators.Main.StartSceneUI.<>c__DisplayClass26_0.<<OnSignupClick>b__0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<Gladiators.Socket.GameConnector.<<ConnToMatchgame>b__14_0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<Gladiators.Socket.GameConnector.<>c__DisplayClass15_0.<<ConnectToMatchgameTestVer>b__0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<Gladiators.Socket.GladiatorsSocket.<<OnMatchgameUDPDisconnect>b__17_0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<Service.Realms.RealmManager.<>c__DisplayClass28_0.<<CallAtlasFuncNoneAsync>b__0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<BattleGladiatorInfo.<HPChange>d__48>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<BattleGladiatorInfo.<HPGrayChange>d__49>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<Character.<>c__DisplayClass48_0.<<Knockback>b__0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<Gladiators.Battle.DivineSelectUI.<DoCloseAni>d__30>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<Gladiators.Battle.DivineSelectUI.<PlayCountDownCandleTime>d__20>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<Gladiators.Main.LobbySceneUI.<>c__DisplayClass17_0.<<RealmLoginCheck>b__0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<Gladiators.Main.StartSceneUI.<<AuthChek>b__21_0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<Gladiators.Main.StartSceneUI.<>c__DisplayClass26_0.<<OnSignupClick>b__0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<Gladiators.Socket.GameConnector.<<ConnToMatchgame>b__14_0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<Gladiators.Socket.GameConnector.<>c__DisplayClass15_0.<<ConnectToMatchgameTestVer>b__0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<Gladiators.Socket.GladiatorsSocket.<<OnMatchgameUDPDisconnect>b__17_0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<Service.Realms.RealmManager.<>c__DisplayClass28_0.<<CallAtlasFuncNoneAsync>b__0>d>
	// Cysharp.Threading.Tasks.CompilerServices.IStateMachineRunnerPromise<object>
	// Cysharp.Threading.Tasks.ITaskPoolNode<object>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.IUniTaskSource<object>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<object>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<object>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<object>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask<object>
	// Cysharp.Threading.Tasks.UniTaskCompletionSource<object>
	// Cysharp.Threading.Tasks.UniTaskCompletionSourceCore<Cysharp.Threading.Tasks.AsyncUnit>
	// Cysharp.Threading.Tasks.UniTaskCompletionSourceCore<object>
	// Cysharp.Threading.Tasks.UniTaskExtensions.<>c__0<object>
	// DelegateList<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<long>>
	// DelegateList<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>>
	// DelegateList<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// DelegateList<float>
	// Loxodon.Framework.Observables.ObservableProperty<byte>
	// Loxodon.Framework.Observables.ObservablePropertyBase<byte>
	// Realms.CollectionExtensions.<>c__19<int>
	// Realms.CollectionExtensions.<>c__19<object>
	// Realms.IRealmCollection<int>
	// Realms.IRealmCollection<object>
	// Realms.MarshaledVector.<ToEnumerable>d__7<Realms.NotifiableObjectHandleBase.CollectionChangeSet.Move>
	// Realms.MarshaledVector.<ToEnumerable>d__7<System.IntPtr>
	// Realms.MarshaledVector.Enumerator<Realms.NotifiableObjectHandleBase.CollectionChangeSet.Move>
	// Realms.MarshaledVector.Enumerator<System.IntPtr>
	// Realms.MarshaledVector<Realms.NotifiableObjectHandleBase.CollectionChangeSet.Move>
	// Realms.MarshaledVector<System.IntPtr>
	// Realms.Native.Arena.Buffer<Realms.NotifiableObjectHandleBase.CollectionChangeSet.Move>
	// Realms.Native.Arena.Buffer<System.IntPtr>
	// Realms.NotificationCallbackDelegate<int>
	// Realms.NotificationCallbackDelegate<object>
	// Realms.NotificationCallbacks.<>c<int>
	// Realms.NotificationCallbacks.<>c<object>
	// Realms.NotificationCallbacks.<>c__DisplayClass3_0<int>
	// Realms.NotificationCallbacks.<>c__DisplayClass3_0<object>
	// Realms.NotificationCallbacks<int>
	// Realms.NotificationCallbacks<object>
	// Realms.RealmCollectionBase.<>c<int>
	// Realms.RealmCollectionBase.<>c<object>
	// Realms.RealmCollectionBase.<>c__DisplayClass43_0<int>
	// Realms.RealmCollectionBase.<>c__DisplayClass43_0<object>
	// Realms.RealmCollectionBase.<>c__DisplayClass48_0<int>
	// Realms.RealmCollectionBase.<>c__DisplayClass48_0<object>
	// Realms.RealmCollectionBase.Enumerator<int>
	// Realms.RealmCollectionBase.Enumerator<object>
	// Realms.RealmCollectionBase<int>
	// Realms.RealmCollectionBase<object>
	// Realms.RealmList<int>
	// Realms.RealmList<object>
	// Realms.RealmResults<int>
	// Realms.RealmResults<object>
	// SerializableDictionary<int,Scoz.Func.BloomSetting>
	// SerializableDictionary<int,object>
	// SerializableDictionary<object,object>
	// SerializableDictionaryBase<int,Scoz.Func.BloomSetting,Scoz.Func.BloomSetting>
	// SerializableDictionaryBase<int,object,object>
	// SerializableDictionaryBase<object,object,object>
	// System.Action<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Action<System.ValueTuple<object,object>>
	// System.Action<UniRx.Unit>
	// System.Action<UniWebViewMessage>
	// System.Action<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle,object>
	// System.Action<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<long>>
	// System.Action<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>>
	// System.Action<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Action<UnityEngine.UIVertex>
	// System.Action<UnityEngine.Vector3>
	// System.Action<byte>
	// System.Action<float>
	// System.Action<int>
	// System.Action<object,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle,int>
	// System.Action<object,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Action<object,object>
	// System.Action<object>
	// System.Action<ushort>
	// System.Buffers.ArrayPool<int>
	// System.Buffers.TlsOverPerCoreLockedStacksArrayPool.LockedStack<int>
	// System.Buffers.TlsOverPerCoreLockedStacksArrayPool.PerCoreLockedStacks<int>
	// System.Buffers.TlsOverPerCoreLockedStacksArrayPool<int>
	// System.ByReference<int>
	// System.ByReference<ushort>
	// System.Collections.Concurrent.ConcurrentQueue.<Enumerate>d__28<object>
	// System.Collections.Concurrent.ConcurrentQueue.Segment<object>
	// System.Collections.Concurrent.ConcurrentQueue<object>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ArraySortHelper<System.ValueTuple<object,object>>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.UIVertex>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Vector3>
	// System.Collections.Generic.ArraySortHelper<byte>
	// System.Collections.Generic.ArraySortHelper<float>
	// System.Collections.Generic.ArraySortHelper<int>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.ArraySortHelper<ushort>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.Comparer<System.Numerics.BigInteger>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.Comparer<System.ValueTuple<object,object>>
	// System.Collections.Generic.Comparer<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.Comparer<UnityEngine.UIVertex>
	// System.Collections.Generic.Comparer<UnityEngine.Vector3>
	// System.Collections.Generic.Comparer<byte>
	// System.Collections.Generic.Comparer<float>
	// System.Collections.Generic.Comparer<int>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Comparer<ushort>
	// System.Collections.Generic.Dictionary.Enumerator<int,Scoz.Func.BloomSetting>
	// System.Collections.Generic.Dictionary.Enumerator<int,System.ValueTuple<object,byte,object>>
	// System.Collections.Generic.Dictionary.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,System.DateTimeOffset>
	// System.Collections.Generic.Dictionary.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.Enumerator<object,long>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,Scoz.Func.BloomSetting>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,System.ValueTuple<object,byte,object>>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,System.DateTimeOffset>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,long>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<int,Scoz.Func.BloomSetting>
	// System.Collections.Generic.Dictionary.KeyCollection<int,System.ValueTuple<object,byte,object>>
	// System.Collections.Generic.Dictionary.KeyCollection<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,System.DateTimeOffset>
	// System.Collections.Generic.Dictionary.KeyCollection<object,byte>
	// System.Collections.Generic.Dictionary.KeyCollection<object,float>
	// System.Collections.Generic.Dictionary.KeyCollection<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection<object,long>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,Scoz.Func.BloomSetting>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,System.ValueTuple<object,byte,object>>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,System.DateTimeOffset>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,long>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,Scoz.Func.BloomSetting>
	// System.Collections.Generic.Dictionary.ValueCollection<int,System.ValueTuple<object,byte,object>>
	// System.Collections.Generic.Dictionary.ValueCollection<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,System.DateTimeOffset>
	// System.Collections.Generic.Dictionary.ValueCollection<object,byte>
	// System.Collections.Generic.Dictionary.ValueCollection<object,float>
	// System.Collections.Generic.Dictionary.ValueCollection<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection<object,long>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary<int,Scoz.Func.BloomSetting>
	// System.Collections.Generic.Dictionary<int,System.ValueTuple<object,byte,object>>
	// System.Collections.Generic.Dictionary<int,int>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<object,System.DateTimeOffset>
	// System.Collections.Generic.Dictionary<object,byte>
	// System.Collections.Generic.Dictionary<object,float>
	// System.Collections.Generic.Dictionary<object,int>
	// System.Collections.Generic.Dictionary<object,long>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.EqualityComparer<Scoz.Func.BloomSetting>
	// System.Collections.Generic.EqualityComparer<System.DateTimeOffset>
	// System.Collections.Generic.EqualityComparer<System.Numerics.BigInteger>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<object,byte,object>>
	// System.Collections.Generic.EqualityComparer<UnityEngine.Color>
	// System.Collections.Generic.EqualityComparer<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.EqualityComparer<byte>
	// System.Collections.Generic.EqualityComparer<float>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<long>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.HashSet.Enumerator<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.HashSet.Enumerator<int>
	// System.Collections.Generic.HashSet.Enumerator<object>
	// System.Collections.Generic.HashSet<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.HashSet<int>
	// System.Collections.Generic.HashSet<object>
	// System.Collections.Generic.HashSetEqualityComparer<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.HashSetEqualityComparer<int>
	// System.Collections.Generic.HashSetEqualityComparer<object>
	// System.Collections.Generic.ICollection<Realms.Schema.Property>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,Scoz.Func.BloomSetting>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,System.ValueTuple<object,byte,object>>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,Realms.Schema.Property>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,System.DateTimeOffset>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,long>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<System.ValueTuple<object,object>>
	// System.Collections.Generic.ICollection<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.ICollection<UnityEngine.UIVertex>
	// System.Collections.Generic.ICollection<UnityEngine.Vector3>
	// System.Collections.Generic.ICollection<byte>
	// System.Collections.Generic.ICollection<float>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.ICollection<ushort>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IComparer<System.ValueTuple<object,object>>
	// System.Collections.Generic.IComparer<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.IComparer<UnityEngine.UIVertex>
	// System.Collections.Generic.IComparer<UnityEngine.Vector3>
	// System.Collections.Generic.IComparer<byte>
	// System.Collections.Generic.IComparer<float>
	// System.Collections.Generic.IComparer<int>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IComparer<ushort>
	// System.Collections.Generic.IDictionary<object,LitJson.ArrayMetadata>
	// System.Collections.Generic.IDictionary<object,LitJson.ObjectMetadata>
	// System.Collections.Generic.IDictionary<object,LitJson.PropertyMetadata>
	// System.Collections.Generic.IDictionary<object,Realms.Schema.Property>
	// System.Collections.Generic.IDictionary<object,object>
	// System.Collections.Generic.IEnumerable<Realms.ChangeSet.Move>
	// System.Collections.Generic.IEnumerable<Realms.NotifiableObjectHandleBase.CollectionChangeSet.Move>
	// System.Collections.Generic.IEnumerable<Realms.Schema.Property>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.UIntPtr,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,Scoz.Func.BloomSetting>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,System.ValueTuple<object,byte,object>>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,Realms.Schema.Property>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,System.DateTimeOffset>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,long>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<System.IntPtr>
	// System.Collections.Generic.IEnumerable<System.ValueTuple<object,object>>
	// System.Collections.Generic.IEnumerable<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.IEnumerable<UnityEngine.UIVertex>
	// System.Collections.Generic.IEnumerable<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerable<byte>
	// System.Collections.Generic.IEnumerable<float>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerable<ushort>
	// System.Collections.Generic.IEnumerator<Realms.ChangeSet.Move>
	// System.Collections.Generic.IEnumerator<Realms.NotifiableObjectHandleBase.CollectionChangeSet.Move>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.UIntPtr,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,Scoz.Func.BloomSetting>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,System.ValueTuple<object,byte,object>>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,Realms.Schema.Property>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,System.DateTimeOffset>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,long>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<System.IntPtr>
	// System.Collections.Generic.IEnumerator<System.ValueTuple<object,object>>
	// System.Collections.Generic.IEnumerator<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.IEnumerator<UnityEngine.UIVertex>
	// System.Collections.Generic.IEnumerator<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerator<byte>
	// System.Collections.Generic.IEnumerator<float>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEnumerator<ushort>
	// System.Collections.Generic.IEqualityComparer<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IList<System.ValueTuple<object,object>>
	// System.Collections.Generic.IList<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.IList<UnityEngine.UIVertex>
	// System.Collections.Generic.IList<UnityEngine.Vector3>
	// System.Collections.Generic.IList<byte>
	// System.Collections.Generic.IList<float>
	// System.Collections.Generic.IList<int>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.IList<ushort>
	// System.Collections.Generic.IReadOnlyCollection<Realms.NotifiableObjectHandleBase.CollectionChangeSet.Move>
	// System.Collections.Generic.IReadOnlyCollection<System.IntPtr>
	// System.Collections.Generic.IReadOnlyCollection<object>
	// System.Collections.Generic.IReadOnlyDictionary<object,System.IntPtr>
	// System.Collections.Generic.IReadOnlyList<int>
	// System.Collections.Generic.IReadOnlyList<object>
	// System.Collections.Generic.KeyValuePair<System.UIntPtr,object>
	// System.Collections.Generic.KeyValuePair<int,Scoz.Func.BloomSetting>
	// System.Collections.Generic.KeyValuePair<int,System.ValueTuple<object,byte,object>>
	// System.Collections.Generic.KeyValuePair<int,int>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<object,Realms.Schema.Property>
	// System.Collections.Generic.KeyValuePair<object,System.DateTimeOffset>
	// System.Collections.Generic.KeyValuePair<object,byte>
	// System.Collections.Generic.KeyValuePair<object,float>
	// System.Collections.Generic.KeyValuePair<object,int>
	// System.Collections.Generic.KeyValuePair<object,long>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.LinkedList.Enumerator<object>
	// System.Collections.Generic.LinkedList<object>
	// System.Collections.Generic.LinkedListNode<object>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.List.Enumerator<System.ValueTuple<object,object>>
	// System.Collections.Generic.List.Enumerator<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.List.Enumerator<UnityEngine.UIVertex>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Vector3>
	// System.Collections.Generic.List.Enumerator<byte>
	// System.Collections.Generic.List.Enumerator<float>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List.Enumerator<ushort>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.List<System.ValueTuple<object,object>>
	// System.Collections.Generic.List<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.List<UnityEngine.UIVertex>
	// System.Collections.Generic.List<UnityEngine.Vector3>
	// System.Collections.Generic.List<byte>
	// System.Collections.Generic.List<float>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.List<ushort>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ObjectComparer<System.Numerics.BigInteger>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<object,object>>
	// System.Collections.Generic.ObjectComparer<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.ObjectComparer<UnityEngine.UIVertex>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Vector3>
	// System.Collections.Generic.ObjectComparer<byte>
	// System.Collections.Generic.ObjectComparer<float>
	// System.Collections.Generic.ObjectComparer<int>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectComparer<ushort>
	// System.Collections.Generic.ObjectEqualityComparer<Scoz.Func.BloomSetting>
	// System.Collections.Generic.ObjectEqualityComparer<System.DateTimeOffset>
	// System.Collections.Generic.ObjectEqualityComparer<System.Numerics.BigInteger>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<object,byte,object>>
	// System.Collections.Generic.ObjectEqualityComparer<UnityEngine.Color>
	// System.Collections.Generic.ObjectEqualityComparer<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.ObjectEqualityComparer<byte>
	// System.Collections.Generic.ObjectEqualityComparer<float>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<long>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.Queue.Enumerator<object>
	// System.Collections.Generic.Queue<object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_0<object,object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_1<object,object>
	// System.Collections.Generic.SortedDictionary.Enumerator<object,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass5_0<object,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass6_0<object,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection<object,object>
	// System.Collections.Generic.SortedDictionary.KeyValuePairComparer<object,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass5_0<object,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass6_0<object,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection<object,object>
	// System.Collections.Generic.SortedDictionary<object,object>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass52_0<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass53_0<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.SortedSet.Enumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.SortedSet.Node<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.SortedSet.TreeSubSet.<>c__DisplayClass9_0<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.SortedSet.TreeSubSet<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.SortedSet<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.Stack.Enumerator<object>
	// System.Collections.Generic.Stack<object>
	// System.Collections.Generic.TreeSet<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.TreeWalkPredicate<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ValueListBuilder<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.ValueTuple<object,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.UIVertex>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Vector3>
	// System.Collections.ObjectModel.ReadOnlyCollection<byte>
	// System.Collections.ObjectModel.ReadOnlyCollection<float>
	// System.Collections.ObjectModel.ReadOnlyCollection<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<ushort>
	// System.Collections.ObjectModel.ReadOnlyDictionary.DictionaryEnumerator<object,Realms.Schema.Property>
	// System.Collections.ObjectModel.ReadOnlyDictionary.KeyCollection<object,Realms.Schema.Property>
	// System.Collections.ObjectModel.ReadOnlyDictionary.ValueCollection<object,Realms.Schema.Property>
	// System.Collections.ObjectModel.ReadOnlyDictionary<object,Realms.Schema.Property>
	// System.Comparison<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Comparison<System.ValueTuple<object,object>>
	// System.Comparison<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Comparison<UnityEngine.UIVertex>
	// System.Comparison<UnityEngine.Vector3>
	// System.Comparison<byte>
	// System.Comparison<float>
	// System.Comparison<int>
	// System.Comparison<object>
	// System.Comparison<ushort>
	// System.Converter<object,float>
	// System.Converter<object,int>
	// System.Converter<object,object>
	// System.Dynamic.Utils.CacheDict.Entry<object,object>
	// System.Dynamic.Utils.CacheDict<object,object>
	// System.Func<Cysharp.Threading.Tasks.UniTaskVoid>
	// System.Func<Realms.ChangeSet.Move,int>
	// System.Func<Realms.NotifiableObjectHandleBase.CollectionChangeSet.Move,Realms.ChangeSet.Move>
	// System.Func<System.Collections.Generic.KeyValuePair<int,object>,int>
	// System.Func<System.Collections.Generic.KeyValuePair<int,object>,object>
	// System.Func<System.Collections.Generic.KeyValuePair<object,int>,System.Collections.Generic.KeyValuePair<object,int>>
	// System.Func<System.Collections.Generic.KeyValuePair<object,int>,object>
	// System.Func<System.Collections.Generic.KeyValuePair<object,object>,object>
	// System.Func<System.IntPtr,int>
	// System.Func<System.Threading.Tasks.VoidTaskResult>
	// System.Func<System.ValueTuple<object,byte,object>,object>
	// System.Func<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>>
	// System.Func<byte,byte>
	// System.Func<byte,ushort>
	// System.Func<byte>
	// System.Func<float,byte>
	// System.Func<int,int>
	// System.Func<int,object>
	// System.Func<int>
	// System.Func<long>
	// System.Func<object,System.Threading.Tasks.VoidTaskResult>
	// System.Func<object,byte>
	// System.Func<object,float>
	// System.Func<object,int>
	// System.Func<object,long>
	// System.Func<object,object,byte,object,object>
	// System.Func<object,object,object>
	// System.Func<object,object>
	// System.Func<object>
	// System.Func<ushort,byte>
	// System.IObservable<UniRx.Unit>
	// System.IObservable<object>
	// System.IObserver<UniRx.Unit>
	// System.IObserver<object>
	// System.Lazy<object>
	// System.Linq.Buffer<object>
	// System.Linq.Enumerable.<CastIterator>d__99<int>
	// System.Linq.Enumerable.<CastIterator>d__99<object>
	// System.Linq.Enumerable.<ConcatIterator>d__59<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.Enumerable.<OfTypeIterator>d__97<object>
	// System.Linq.Enumerable.Iterator<byte>
	// System.Linq.Enumerable.Iterator<float>
	// System.Linq.Enumerable.Iterator<object>
	// System.Linq.Enumerable.Iterator<ushort>
	// System.Linq.Enumerable.WhereEnumerableIterator<float>
	// System.Linq.Enumerable.WhereEnumerableIterator<ushort>
	// System.Linq.Enumerable.WhereSelectArrayIterator<byte,ushort>
	// System.Linq.Enumerable.WhereSelectArrayIterator<object,float>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<byte,ushort>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<object,float>
	// System.Linq.Enumerable.WhereSelectListIterator<byte,ushort>
	// System.Linq.Enumerable.WhereSelectListIterator<object,float>
	// System.Linq.EnumerableSorter<object,float>
	// System.Linq.EnumerableSorter<object>
	// System.Linq.GroupedEnumerable<System.Collections.Generic.KeyValuePair<object,int>,object,System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.IGrouping<object,System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.IdentityFunction.<>c<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.IdentityFunction<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.Lookup.<GetEnumerator>d__12<object,System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.Lookup.Grouping.<GetEnumerator>d__7<object,System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.Lookup.Grouping<object,System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.Lookup<object,System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.OrderedEnumerable.<GetEnumerator>d__1<object>
	// System.Linq.OrderedEnumerable<object,float>
	// System.Linq.OrderedEnumerable<object>
	// System.Nullable<Realms.NotifiableObjectHandleBase.CollectionChangeSet>
	// System.Nullable<Realms.RealmValue.HandlesToCleanup>
	// System.Nullable<Realms.Schema.Property>
	// System.Nullable<System.DateTimeOffset>
	// System.Nullable<byte>
	// System.Nullable<double>
	// System.Nullable<float>
	// System.Nullable<int>
	// System.Nullable<long>
	// System.Predicate<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Predicate<System.ValueTuple<object,object>>
	// System.Predicate<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Predicate<UnityEngine.UIVertex>
	// System.Predicate<UnityEngine.Vector3>
	// System.Predicate<byte>
	// System.Predicate<float>
	// System.Predicate<int>
	// System.Predicate<object>
	// System.Predicate<ushort>
	// System.ReadOnlySpan<int>
	// System.ReadOnlySpan<ushort>
	// System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>
	// System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<System.Threading.Tasks.VoidTaskResult>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<long>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<System.Threading.Tasks.VoidTaskResult>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<long>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<object>
	// System.Runtime.CompilerServices.ReadOnlyCollectionBuilder.Enumerator<object>
	// System.Runtime.CompilerServices.ReadOnlyCollectionBuilder<object>
	// System.Runtime.CompilerServices.TaskAwaiter<System.Threading.Tasks.VoidTaskResult>
	// System.Runtime.CompilerServices.TaskAwaiter<long>
	// System.Runtime.CompilerServices.TaskAwaiter<object>
	// System.Runtime.CompilerServices.TrueReadOnlyCollection<object>
	// System.Span<int>
	// System.Span<ushort>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<System.Threading.Tasks.VoidTaskResult>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<long>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<object>
	// System.Threading.Tasks.Task<System.Threading.Tasks.VoidTaskResult>
	// System.Threading.Tasks.Task<long>
	// System.Threading.Tasks.Task<object>
	// System.Threading.Tasks.TaskCompletionSource<long>
	// System.Threading.Tasks.TaskCompletionSource<object>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<System.Threading.Tasks.VoidTaskResult>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<long>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<object>
	// System.Threading.Tasks.TaskFactory<System.Threading.Tasks.VoidTaskResult>
	// System.Threading.Tasks.TaskFactory<long>
	// System.Threading.Tasks.TaskFactory<object>
	// System.Tuple<object,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Tuple<object,int>
	// System.ValueTuple<System.Numerics.BigInteger,System.Numerics.BigInteger>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,object>>
	// System.ValueTuple<byte,object>
	// System.ValueTuple<object,byte,object>
	// System.ValueTuple<object,object>
	// UniRx.InternalUtil.ImmutableList<object>
	// UniRx.InternalUtil.ListObserver<UniRx.Unit>
	// UniRx.InternalUtil.ListObserver<object>
	// UniRx.Observer.Subscribe<UniRx.Unit>
	// UniRx.Observer.Subscribe<object>
	// UniRx.Observer.Subscribe_<UniRx.Unit>
	// UniRx.Observer.Subscribe_<object>
	// UniRx.Subject.Subscription<UniRx.Unit>
	// UniRx.Subject.Subscription<object>
	// UniRx.Subject<UniRx.Unit>
	// UniRx.Subject<object>
	// Unity.Collections.NativeArray.Enumerator<ushort>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<ushort>
	// Unity.Collections.NativeArray.ReadOnly<ushort>
	// Unity.Collections.NativeArray<ushort>
	// UnityEngine.AddressableAssets.AddressablesImpl.<>c__DisplayClass79_0<object>
	// UnityEngine.AddressableAssets.AddressablesImpl.<>c__DisplayClass88_0<object>
	// UnityEngine.AddressableAssets.AddressablesImpl.<>c__DisplayClass91_0<object>
	// UnityEngine.Events.InvokableCall<byte>
	// UnityEngine.Events.InvokableCall<object>
	// UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene,int>
	// UnityEngine.Events.UnityAction<byte>
	// UnityEngine.Events.UnityAction<object>
	// UnityEngine.Events.UnityEvent<byte>
	// UnityEngine.Events.UnityEvent<object>
	// UnityEngine.Rendering.VolumeParameter<UnityEngine.Color>
	// UnityEngine.Rendering.VolumeParameter<float>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationBase.<>c__DisplayClass60_0<long>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationBase.<>c__DisplayClass60_0<object>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationBase.<>c__DisplayClass61_0<long>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationBase.<>c__DisplayClass61_0<object>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationBase<long>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationBase<object>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<long>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>
	// UnityEngine.ResourceManagement.ChainOperationTypelessDepedency<object>
	// UnityEngine.ResourceManagement.ResourceManager.<>c__DisplayClass95_0<object>
	// UnityEngine.ResourceManagement.ResourceManager.CompletedOperation<object>
	// UnityEngine.ResourceManagement.Util.GlobalLinkedListNodeCache<object>
	// UnityEngine.ResourceManagement.Util.LinkedListNodeCache<object>
	// }}

	public void RefMethods()
	{
		// object Cinemachine.CinemachineVirtualCamera.GetCinemachineComponent<object>()
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Battle.BattleManager.<CreateTerrainAndChar>d__27>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Battle.BattleManager.<CreateTerrainAndChar>d__27&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Battle.BattleManager.<Init>d__21>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Battle.BattleManager.<Init>d__21&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Main.AllocatedRoom.<SetRoom>d__51>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Main.AllocatedRoom.<SetRoom>d__51&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Main.AllocatedRoom.<SetRoom_TestvVer>d__52>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Main.AllocatedRoom.<SetRoom_TestvVer>d__52&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Scoz.Func.UniTaskManager.<OneTimesTask>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Scoz.Func.UniTaskManager.<OneTimesTask>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Scoz.Func.UniTaskManager.<RepeatTask>d__9>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Scoz.Func.UniTaskManager.<RepeatTask>d__9&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Service.Realms.RealmManager.<AnonymousSignup>d__30>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Service.Realms.RealmManager.<AnonymousSignup>d__30&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Service.Realms.RealmManager.<OnSignin>d__34>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Service.Realms.RealmManager.<OnSignin>d__34&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Gladiators.Main.StartSceneUI.<InitPlayerData>d__27>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Gladiators.Main.StartSceneUI.<InitPlayerData>d__27&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Gladiators.Socket.GameConnector.<ConnToMatchmaker>d__27>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Gladiators.Socket.GameConnector.<ConnToMatchmaker>d__27&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Gladiators.Socket.GameConnector.<JoinMatchgame>d__16>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Gladiators.Socket.GameConnector.<JoinMatchgame>d__16&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Gladiators.Socket.GameConnector.<OnLoginToMatchmakerError>d__29>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Gladiators.Socket.GameConnector.<OnLoginToMatchmakerError>d__29&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,Gladiators.Battle.BattleManager.<CheckGameState>d__22>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,Gladiators.Battle.BattleManager.<CheckGameState>d__22&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,DBPlayer.<SetInMatchgameID>d__45>(System.Runtime.CompilerServices.TaskAwaiter&,DBPlayer.<SetInMatchgameID>d__45&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Service.Realms.RealmManager.<OnSignin>d__34>(System.Runtime.CompilerServices.TaskAwaiter&,Service.Realms.RealmManager.<OnSignin>d__34&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Service.Realms.RealmManager.<Signout>d__37>(System.Runtime.CompilerServices.TaskAwaiter&,Service.Realms.RealmManager.<Signout>d__37&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Service.Realms.RealmManager.<AnonymousSignup>d__30>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Service.Realms.RealmManager.<AnonymousSignup>d__30&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Service.Realms.RealmManager.<GetServerTime>d__35>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Service.Realms.RealmManager.<GetServerTime>d__35&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Scoz.Func.AddressablesLoader.<GetResourceByFullPath_Async>d__8<object>>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Scoz.Func.AddressablesLoader.<GetResourceByFullPath_Async>d__8<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Service.Realms.RealmManager.<CallAtlasFunc_InitPlayerData>d__29>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Service.Realms.RealmManager.<CallAtlasFunc_InitPlayerData>d__29&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Gladiators.Main.GamePlayer.<GetMatchGame>d__16>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Gladiators.Main.GamePlayer.<GetMatchGame>d__16&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Gladiators.Socket.GameConnector.<SendRestfulAPI>d__7>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Gladiators.Socket.GameConnector.<SendRestfulAPI>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Service.Realms.RealmManager.<CallAtlasFunc_InitPlayerData>d__29>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Service.Realms.RealmManager.<CallAtlasFunc_InitPlayerData>d__29&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UnityAsyncExtensions.UnityWebRequestAsyncOperationAwaiter,Scoz.Func.Poster.<Post>d__0>(Cysharp.Threading.Tasks.UnityAsyncExtensions.UnityWebRequestAsyncOperationAwaiter&,Scoz.Func.Poster.<Post>d__0&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Service.Realms.RealmManager.<GetProvider>d__33>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Service.Realms.RealmManager.<GetProvider>d__33&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Service.Realms.RealmManager.<GetValidAccessToken>d__32>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Service.Realms.RealmManager.<GetValidAccessToken>d__32&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Service.Realms.RealmManager.<Query_GetDoc>d__38>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Service.Realms.RealmManager.<Query_GetDoc>d__38&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<DBPlayer.<SetInMatchgameID>d__45>(DBPlayer.<SetInMatchgameID>d__45&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Gladiators.Battle.BattleManager.<CheckGameState>d__22>(Gladiators.Battle.BattleManager.<CheckGameState>d__22&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Gladiators.Battle.BattleManager.<CreateTerrainAndChar>d__27>(Gladiators.Battle.BattleManager.<CreateTerrainAndChar>d__27&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Gladiators.Battle.BattleManager.<Init>d__21>(Gladiators.Battle.BattleManager.<Init>d__21&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Gladiators.Main.AllocatedRoom.<SetRoom>d__51>(Gladiators.Main.AllocatedRoom.<SetRoom>d__51&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Gladiators.Main.AllocatedRoom.<SetRoom_TestvVer>d__52>(Gladiators.Main.AllocatedRoom.<SetRoom_TestvVer>d__52&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Gladiators.Main.StartSceneUI.<InitPlayerData>d__27>(Gladiators.Main.StartSceneUI.<InitPlayerData>d__27&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Gladiators.Socket.GameConnector.<ConnToMatchmaker>d__27>(Gladiators.Socket.GameConnector.<ConnToMatchmaker>d__27&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Gladiators.Socket.GameConnector.<JoinMatchgame>d__16>(Gladiators.Socket.GameConnector.<JoinMatchgame>d__16&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Gladiators.Socket.GameConnector.<OnLoginToMatchmakerError>d__29>(Gladiators.Socket.GameConnector.<OnLoginToMatchmakerError>d__29&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Scoz.Func.UniTaskManager.<OneTimesTask>d__11>(Scoz.Func.UniTaskManager.<OneTimesTask>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Scoz.Func.UniTaskManager.<RepeatTask>d__9>(Scoz.Func.UniTaskManager.<RepeatTask>d__9&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Service.Realms.RealmManager.<AnonymousSignup>d__30>(Service.Realms.RealmManager.<AnonymousSignup>d__30&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Service.Realms.RealmManager.<GetServerTime>d__35>(Service.Realms.RealmManager.<GetServerTime>d__35&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Service.Realms.RealmManager.<OnSignin>d__34>(Service.Realms.RealmManager.<OnSignin>d__34&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Service.Realms.RealmManager.<Signout>d__37>(Service.Realms.RealmManager.<Signout>d__37&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Gladiators.Main.GamePlayer.<GetMatchGame>d__16>(Gladiators.Main.GamePlayer.<GetMatchGame>d__16&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Gladiators.Socket.GameConnector.<SendRestfulAPI>d__7>(Gladiators.Socket.GameConnector.<SendRestfulAPI>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Scoz.Func.AddressablesLoader.<GetResourceByFullPath_Async>d__8<object>>(Scoz.Func.AddressablesLoader.<GetResourceByFullPath_Async>d__8<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Scoz.Func.Poster.<Post>d__0>(Scoz.Func.Poster.<Post>d__0&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Service.Realms.RealmManager.<CallAtlasFunc_InitPlayerData>d__29>(Service.Realms.RealmManager.<CallAtlasFunc_InitPlayerData>d__29&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Service.Realms.RealmManager.<GetProvider>d__33>(Service.Realms.RealmManager.<GetProvider>d__33&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Service.Realms.RealmManager.<GetValidAccessToken>d__32>(Service.Realms.RealmManager.<GetValidAccessToken>d__32&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Service.Realms.RealmManager.<Query_GetDoc>d__38>(Service.Realms.RealmManager.<Query_GetDoc>d__38&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,BattleGladiatorInfo.<HPChange>d__48>(Cysharp.Threading.Tasks.UniTask.Awaiter&,BattleGladiatorInfo.<HPChange>d__48&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,BattleGladiatorInfo.<HPGrayChange>d__49>(Cysharp.Threading.Tasks.UniTask.Awaiter&,BattleGladiatorInfo.<HPGrayChange>d__49&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Battle.DivineSelectUI.<DoCloseAni>d__30>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Battle.DivineSelectUI.<DoCloseAni>d__30&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Battle.DivineSelectUI.<PlayCountDownCandleTime>d__20>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Battle.DivineSelectUI.<PlayCountDownCandleTime>d__20&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Main.LobbySceneUI.<>c__DisplayClass17_0.<<RealmLoginCheck>b__0>d>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Main.LobbySceneUI.<>c__DisplayClass17_0.<<RealmLoginCheck>b__0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Main.StartSceneUI.<<AuthChek>b__21_0>d>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Main.StartSceneUI.<<AuthChek>b__21_0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Main.StartSceneUI.<>c__DisplayClass26_0.<<OnSignupClick>b__0>d>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Main.StartSceneUI.<>c__DisplayClass26_0.<<OnSignupClick>b__0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Socket.GameConnector.<>c__DisplayClass15_0.<<ConnectToMatchgameTestVer>b__0>d>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Socket.GameConnector.<>c__DisplayClass15_0.<<ConnectToMatchgameTestVer>b__0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Gladiators.Socket.GameConnector.<<ConnToMatchgame>b__14_0>d>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Gladiators.Socket.GameConnector.<<ConnToMatchgame>b__14_0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Gladiators.Socket.GladiatorsSocket.<<OnMatchgameUDPDisconnect>b__17_0>d>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Gladiators.Socket.GladiatorsSocket.<<OnMatchgameUDPDisconnect>b__17_0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,Character.<>c__DisplayClass48_0.<<Knockback>b__0>d>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,Character.<>c__DisplayClass48_0.<<Knockback>b__0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,Gladiators.Battle.DivineSelectUI.<PlayCountDownCandleTime>d__20>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,Gladiators.Battle.DivineSelectUI.<PlayCountDownCandleTime>d__20&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Service.Realms.RealmManager.<>c__DisplayClass28_0.<<CallAtlasFuncNoneAsync>b__0>d>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Service.Realms.RealmManager.<>c__DisplayClass28_0.<<CallAtlasFuncNoneAsync>b__0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleGladiatorInfo.<<Update>b__46_0>d>(BattleGladiatorInfo.<<Update>b__46_0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleGladiatorInfo.<>c__DisplayClass48_0.<<HPChange>b__0>d>(BattleGladiatorInfo.<>c__DisplayClass48_0.<<HPChange>b__0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleGladiatorInfo.<HPChange>d__48>(BattleGladiatorInfo.<HPChange>d__48&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleGladiatorInfo.<HPGrayChange>d__49>(BattleGladiatorInfo.<HPGrayChange>d__49&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Character.<>c__DisplayClass48_0.<<Knockback>b__0>d>(Character.<>c__DisplayClass48_0.<<Knockback>b__0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Gladiators.Battle.DivineSelectUI.<<CountDownCandleTime>b__17_0>d>(Gladiators.Battle.DivineSelectUI.<<CountDownCandleTime>b__17_0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Gladiators.Battle.DivineSelectUI.<>c__DisplayClass29_0.<<CloseUI>b__0>d>(Gladiators.Battle.DivineSelectUI.<>c__DisplayClass29_0.<<CloseUI>b__0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Gladiators.Battle.DivineSelectUI.<DoCloseAni>d__30>(Gladiators.Battle.DivineSelectUI.<DoCloseAni>d__30&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Gladiators.Battle.DivineSelectUI.<PlayCountDownCandleTime>d__20>(Gladiators.Battle.DivineSelectUI.<PlayCountDownCandleTime>d__20&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Gladiators.Main.LobbySceneUI.<>c__DisplayClass17_0.<<RealmLoginCheck>b__0>d>(Gladiators.Main.LobbySceneUI.<>c__DisplayClass17_0.<<RealmLoginCheck>b__0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Gladiators.Main.StartSceneUI.<<AuthChek>b__21_0>d>(Gladiators.Main.StartSceneUI.<<AuthChek>b__21_0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Gladiators.Main.StartSceneUI.<>c__DisplayClass26_0.<<OnSignupClick>b__0>d>(Gladiators.Main.StartSceneUI.<>c__DisplayClass26_0.<<OnSignupClick>b__0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Gladiators.Socket.GameConnector.<<ConnToMatchgame>b__14_0>d>(Gladiators.Socket.GameConnector.<<ConnToMatchgame>b__14_0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Gladiators.Socket.GameConnector.<>c__DisplayClass15_0.<<ConnectToMatchgameTestVer>b__0>d>(Gladiators.Socket.GameConnector.<>c__DisplayClass15_0.<<ConnectToMatchgameTestVer>b__0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Gladiators.Socket.GladiatorsSocket.<<OnMatchgameUDPDisconnect>b__17_0>d>(Gladiators.Socket.GladiatorsSocket.<<OnMatchgameUDPDisconnect>b__17_0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Service.Realms.RealmManager.<>c__DisplayClass28_0.<<CallAtlasFuncNoneAsync>b__0>d>(Service.Realms.RealmManager.<>c__DisplayClass28_0.<<CallAtlasFuncNoneAsync>b__0>d&)
		// Cysharp.Threading.Tasks.UniTask.Awaiter Cysharp.Threading.Tasks.EnumeratorAsyncExtensions.GetAwaiter<object>(object)
		// Cysharp.Threading.Tasks.UniTask<object> Cysharp.Threading.Tasks.UniTaskExtensions.AsUniTask<object>(System.Threading.Tasks.Task<object>,bool)
		// object DG.Tweening.TweenExtensions.Pause<object>(object)
		// object DG.Tweening.TweenSettingsExtensions.OnComplete<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.SetAutoKill<object>(object,bool)
		// object DG.Tweening.TweenSettingsExtensions.SetDelay<object>(object,float)
		// object DG.Tweening.TweenSettingsExtensions.SetEase<object>(object,DG.Tweening.Ease)
		// object LitJson.JsonMapper.ToObject<object>(string)
		// Realms.IRealmCollection<object> Realms.CollectionExtensions.AsRealmCollection<object>(System.Linq.IQueryable<object>)
		// System.Void Realms.CollectionExtensions.PopulateCollection<int>(System.Collections.Generic.ICollection<int>,System.Collections.Generic.ICollection<int>,bool,bool)
		// System.Void Realms.CollectionExtensions.PopulateCollection<object>(System.Collections.Generic.ICollection<object>,System.Collections.Generic.ICollection<object>,bool,bool)
		// System.Void Realms.CollectionExtensions.PopulateCollectionCore<int>(System.Collections.Generic.ICollection<int>,System.Collections.Generic.ICollection<int>,bool,bool,System.Func<int,object>)
		// System.Void Realms.CollectionExtensions.PopulateCollectionCore<object>(System.Collections.Generic.ICollection<object>,System.Collections.Generic.ICollection<object>,bool,bool,System.Func<object,object>)
		// System.IDisposable Realms.CollectionExtensions.SubscribeForNotifications<object>(System.Linq.IQueryable<object>,Realms.NotificationCallbackDelegate<object>)
		// object Realms.Helpers.Argument.EnsureType<object>(object,string,string)
		// System.Collections.Generic.IList<int> Realms.ManagedAccessor.GetListValue<int>(string)
		// System.Collections.Generic.IList<object> Realms.ManagedAccessor.GetListValue<object>(string)
		// Realms.RealmList<int> Realms.ObjectHandle.GetList<int>(Realms.Realm,string,Realms.Metadata,string)
		// Realms.RealmList<object> Realms.ObjectHandle.GetList<object>(Realms.Realm,string,Realms.Metadata,string)
		// object Realms.Realm.Add<object>(object,bool)
		// System.Void Realms.Realm.AddInternal<object>(object,System.Type,bool)
		// System.Linq.IQueryable<object> Realms.Realm.All<object>()
		// object Realms.Realm.Find<object>(string)
		// object Realms.Realm.FindCore<object>(Realms.RealmValue)
		// object Realms.RealmValue.AsRealmObject<object>()
		// Realms.Sync.Subscription Realms.Sync.SubscriptionSet.Add<object>(System.Linq.IQueryable<object>,Realms.Sync.SubscriptionOptions)
		// System.Threading.Tasks.Task<object> Realms.Sync.User.FunctionsClient.CallAsync<object>(string,object[])
		// System.Threading.Tasks.Task<object> Realms.Sync.User.FunctionsClient.CallSerializedAsync<object>(string,string,string)
		// object System.Activator.CreateInstance<object>()
		// float[] System.Array.ConvertAll<object,float>(object[],System.Converter<object,float>)
		// int[] System.Array.ConvertAll<object,int>(object[],System.Converter<object,int>)
		// object[] System.Array.ConvertAll<object,object>(object[],System.Converter<object,object>)
		// object[] System.Array.Empty<object>()
		// bool System.Array.Exists<object>(object[],System.Predicate<object>)
		// int System.Array.FindIndex<object>(object[],System.Predicate<object>)
		// int System.Array.FindIndex<object>(object[],int,int,System.Predicate<object>)
		// int System.Array.IndexOf<object>(object[],object)
		// int System.Array.IndexOfImpl<object>(object[],object,int,int)
		// System.Void System.Array.Resize<object>(object[]&,int)
		// System.Collections.Generic.List<object> System.Collections.Generic.List<object>.ConvertAll<object>(System.Converter<object,object>)
		// System.Collections.ObjectModel.ReadOnlyCollection<object> System.Dynamic.Utils.CollectionExtensions.ToReadOnly<object>(System.Collections.Generic.IEnumerable<object>)
		// bool System.Enum.TryParse<int>(string,bool,int&)
		// bool System.Enum.TryParse<int>(string,int&)
		// bool System.Enum.TryParse<object>(string,bool,object&)
		// bool System.Enum.TryParse<object>(string,object&)
		// bool System.Linq.Enumerable.All<byte>(System.Collections.Generic.IEnumerable<byte>,System.Func<byte,bool>)
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.Cast<int>(System.Collections.IEnumerable)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Cast<object>(System.Collections.IEnumerable)
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.CastIterator<int>(System.Collections.IEnumerable)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.CastIterator<object>(System.Collections.IEnumerable)
		// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>> System.Linq.Enumerable.Concat<System.Collections.Generic.KeyValuePair<object,int>>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>,System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>)
		// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>> System.Linq.Enumerable.ConcatIterator<System.Collections.Generic.KeyValuePair<object,int>>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>,System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>)
		// System.Collections.Generic.KeyValuePair<object,int> System.Linq.Enumerable.First<System.Collections.Generic.KeyValuePair<object,int>>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle System.Linq.Enumerable.First<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>(System.Collections.Generic.IEnumerable<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>)
		// object System.Linq.Enumerable.First<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<System.Linq.IGrouping<object,System.Collections.Generic.KeyValuePair<object,int>>> System.Linq.Enumerable.GroupBy<System.Collections.Generic.KeyValuePair<object,int>,object>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>,System.Func<System.Collections.Generic.KeyValuePair<object,int>,object>)
		// System.Collections.Generic.KeyValuePair<object,int> System.Linq.Enumerable.Last<System.Collections.Generic.KeyValuePair<object,int>>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>)
		// object System.Linq.Enumerable.Last<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.OfType<object>(System.Collections.IEnumerable)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.OfTypeIterator<object>(System.Collections.IEnumerable)
		// System.Linq.IOrderedEnumerable<object> System.Linq.Enumerable.OrderBy<object,float>(System.Collections.Generic.IEnumerable<object>,System.Func<object,float>)
		// System.Collections.Generic.IEnumerable<float> System.Linq.Enumerable.Select<object,float>(System.Collections.Generic.IEnumerable<object>,System.Func<object,float>)
		// System.Collections.Generic.IEnumerable<ushort> System.Linq.Enumerable.Select<byte,ushort>(System.Collections.Generic.IEnumerable<byte>,System.Func<byte,ushort>)
		// object[] System.Linq.Enumerable.ToArray<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.Dictionary<int,object> System.Linq.Enumerable.ToDictionary<System.Collections.Generic.KeyValuePair<int,object>,int,object>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>,System.Func<System.Collections.Generic.KeyValuePair<int,object>,int>,System.Func<System.Collections.Generic.KeyValuePair<int,object>,object>)
		// System.Collections.Generic.Dictionary<int,object> System.Linq.Enumerable.ToDictionary<System.Collections.Generic.KeyValuePair<int,object>,int,object>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>,System.Func<System.Collections.Generic.KeyValuePair<int,object>,int>,System.Func<System.Collections.Generic.KeyValuePair<int,object>,object>,System.Collections.Generic.IEqualityComparer<int>)
		// System.Collections.Generic.Dictionary<object,int> System.Linq.Enumerable.ToDictionary<object,object,int>(System.Collections.Generic.IEnumerable<object>,System.Func<object,object>,System.Func<object,int>)
		// System.Collections.Generic.Dictionary<object,int> System.Linq.Enumerable.ToDictionary<object,object,int>(System.Collections.Generic.IEnumerable<object>,System.Func<object,object>,System.Func<object,int>,System.Collections.Generic.IEqualityComparer<object>)
		// System.Collections.Generic.Dictionary<object,object> System.Linq.Enumerable.ToDictionary<System.Collections.Generic.KeyValuePair<object,object>,object,object>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>,System.Func<System.Collections.Generic.KeyValuePair<object,object>,object>,System.Func<System.Collections.Generic.KeyValuePair<object,object>,object>)
		// System.Collections.Generic.Dictionary<object,object> System.Linq.Enumerable.ToDictionary<System.Collections.Generic.KeyValuePair<object,object>,object,object>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>,System.Func<System.Collections.Generic.KeyValuePair<object,object>,object>,System.Func<System.Collections.Generic.KeyValuePair<object,object>,object>,System.Collections.Generic.IEqualityComparer<object>)
		// System.Collections.Generic.HashSet<int> System.Linq.Enumerable.ToHashSet<int>(System.Collections.Generic.IEnumerable<int>)
		// System.Collections.Generic.HashSet<int> System.Linq.Enumerable.ToHashSet<int>(System.Collections.Generic.IEnumerable<int>,System.Collections.Generic.IEqualityComparer<int>)
		// System.Collections.Generic.HashSet<object> System.Linq.Enumerable.ToHashSet<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.HashSet<object> System.Linq.Enumerable.ToHashSet<object>(System.Collections.Generic.IEnumerable<object>,System.Collections.Generic.IEqualityComparer<object>)
		// System.Collections.Generic.List<float> System.Linq.Enumerable.ToList<float>(System.Collections.Generic.IEnumerable<float>)
		// System.Collections.Generic.List<int> System.Linq.Enumerable.ToList<int>(System.Collections.Generic.IEnumerable<int>)
		// System.Collections.Generic.List<object> System.Linq.Enumerable.ToList<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<float> System.Linq.Enumerable.Iterator<object>.Select<float>(System.Func<object,float>)
		// System.Collections.Generic.IEnumerable<ushort> System.Linq.Enumerable.Iterator<byte>.Select<ushort>(System.Func<byte,ushort>)
		// System.Linq.Expressions.Expression<object> System.Linq.Expressions.Expression.Lambda<object>(System.Linq.Expressions.Expression,System.Linq.Expressions.ParameterExpression[])
		// System.Linq.Expressions.Expression<object> System.Linq.Expressions.Expression.Lambda<object>(System.Linq.Expressions.Expression,bool,System.Collections.Generic.IEnumerable<System.Linq.Expressions.ParameterExpression>)
		// System.Linq.Expressions.Expression<object> System.Linq.Expressions.Expression.Lambda<object>(System.Linq.Expressions.Expression,string,bool,System.Collections.Generic.IEnumerable<System.Linq.Expressions.ParameterExpression>)
		// System.Linq.IQueryable<object> System.Linq.IQueryProvider.CreateQuery<object>(System.Linq.Expressions.Expression)
		// System.Linq.IQueryable<object> System.Linq.Queryable.Where<object>(System.Linq.IQueryable<object>,System.Linq.Expressions.Expression<System.Func<object,bool>>)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Service.Realms.RealmManager.<SetupConfig>d__36>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Service.Realms.RealmManager.<SetupConfig>d__36&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Service.Realms.RealmManager.<SetupConfig>d__36>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Service.Realms.RealmManager.<SetupConfig>d__36&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Service.Realms.RealmManager.<CallAtlasFunc>d__27>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Service.Realms.RealmManager.<CallAtlasFunc>d__27&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Service.Realms.RealmManager.<SetupConfig>d__36>(Service.Realms.RealmManager.<SetupConfig>d__36&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<Realms.Sync.User.FunctionsClient.<CallSerializedAsync>d__4<object>>(Realms.Sync.User.FunctionsClient.<CallSerializedAsync>d__4<object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<Service.Realms.RealmManager.<CallAtlasFunc>d__27>(Service.Realms.RealmManager.<CallAtlasFunc>d__27&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Socket.TcpClient.<Thread_Connect>d__37>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Socket.TcpClient.<Thread_Connect>d__37&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Service.Realms.RealmManager.<EmailPWSignup>d__31>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Service.Realms.RealmManager.<EmailPWSignup>d__31&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Service.Realms.RealmManager.<EmailPWSignup>d__31>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Service.Realms.RealmManager.<EmailPWSignup>d__31&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Gladiators.Socket.TcpClient.<Thread_Connect>d__37>(Gladiators.Socket.TcpClient.<Thread_Connect>d__37&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Service.Realms.RealmManager.<EmailPWSignup>d__31>(Service.Realms.RealmManager.<EmailPWSignup>d__31&)
		// object& System.Runtime.CompilerServices.Unsafe.As<object,object>(object&)
		// System.Void* System.Runtime.CompilerServices.Unsafe.AsPointer<object>(object&)
		// System.IDisposable UniRx.ObservableExtensions.Subscribe<UniRx.Unit>(System.IObservable<UniRx.Unit>,System.Action<UniRx.Unit>,System.Action<System.Exception>)
		// System.IDisposable UniRx.ObservableExtensions.Subscribe<object>(System.IObservable<object>,System.Action<object>,System.Action<System.Exception>)
		// System.IObserver<UniRx.Unit> UniRx.Observer.CreateSubscribeObserver<UniRx.Unit>(System.Action<UniRx.Unit>,System.Action<System.Exception>,System.Action)
		// System.IObserver<object> UniRx.Observer.CreateSubscribeObserver<object>(System.Action<object>,System.Action<System.Exception>,System.Action)
		// object Unity.VisualScripting.ComponentHolderProtocol.AddComponent<object>(UnityEngine.Object)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object> UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<object>(object)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<System.Collections.Generic.IList<object>> UnityEngine.AddressableAssets.Addressables.LoadAssetsAsync<object>(object,System.Action<object>)
		// System.Void UnityEngine.AddressableAssets.Addressables.Release<object>(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>)
		// System.Void UnityEngine.AddressableAssets.AddressablesImpl.<AutoReleaseHandleOnCompletion>b__116_0<object>(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>)
		// System.Void UnityEngine.AddressableAssets.AddressablesImpl.AutoReleaseHandleOnCompletion<object>(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object> UnityEngine.AddressableAssets.AddressablesImpl.LoadAssetAsync<object>(object)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object> UnityEngine.AddressableAssets.AddressablesImpl.LoadAssetWithChain<object>(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle,object)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<System.Collections.Generic.IList<object>> UnityEngine.AddressableAssets.AddressablesImpl.LoadAssetsAsync<object>(System.Collections.Generic.IList<UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation>,System.Action<object>,bool)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<System.Collections.Generic.IList<object>> UnityEngine.AddressableAssets.AddressablesImpl.LoadAssetsAsync<object>(object,System.Action<object>,bool)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<System.Collections.Generic.IList<object>> UnityEngine.AddressableAssets.AddressablesImpl.LoadAssetsWithChain<object>(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle,System.Collections.Generic.IList<UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation>,System.Action<object>,bool)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<System.Collections.Generic.IList<object>> UnityEngine.AddressableAssets.AddressablesImpl.LoadAssetsWithChain<object>(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle,object,System.Action<object>,bool)
		// System.Void UnityEngine.AddressableAssets.AddressablesImpl.Release<object>(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object> UnityEngine.AddressableAssets.AddressablesImpl.TrackHandle<object>(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>)
		// object UnityEngine.Component.GetComponent<object>()
		// object UnityEngine.Component.GetComponentInParent<object>()
		// object[] UnityEngine.Component.GetComponents<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>(bool)
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// object[] UnityEngine.GameObject.GetComponents<object>()
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>(bool)
		// object UnityEngine.Object.Instantiate<object>(object)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Vector3,UnityEngine.Quaternion)
		// bool UnityEngine.Rendering.VolumeProfile.TryGet<object>(System.Type,object&)
		// bool UnityEngine.Rendering.VolumeProfile.TryGet<object>(object&)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object> UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle.Convert<object>()
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object> UnityEngine.ResourceManagement.ResourceManager.CreateChainOperation<object>(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle,System.Func<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>>)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object> UnityEngine.ResourceManagement.ResourceManager.CreateChainOperation<object>(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle,System.Func<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>>,bool)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object> UnityEngine.ResourceManagement.ResourceManager.CreateCompletedOperation<object>(object,string)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object> UnityEngine.ResourceManagement.ResourceManager.CreateCompletedOperationInternal<object>(object,bool,System.Exception,bool)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object> UnityEngine.ResourceManagement.ResourceManager.CreateCompletedOperationWithException<object>(object,System.Exception)
		// object UnityEngine.ResourceManagement.ResourceManager.CreateOperation<object>(System.Type,int,UnityEngine.ResourceManagement.Util.IOperationCacheKey,System.Action<UnityEngine.ResourceManagement.AsyncOperations.IAsyncOperation>)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object> UnityEngine.ResourceManagement.ResourceManager.ProvideResource<object>(UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<System.Collections.Generic.IList<object>> UnityEngine.ResourceManagement.ResourceManager.ProvideResources<object>(System.Collections.Generic.IList<UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation>,bool,System.Action<object>)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object> UnityEngine.ResourceManagement.ResourceManager.StartOperation<object>(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationBase<object>,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle)
		// object UnityEngine.Resources.GetBuiltinResource<object>(string)
		// object UnityEngine.Resources.Load<object>(string)
	}
}