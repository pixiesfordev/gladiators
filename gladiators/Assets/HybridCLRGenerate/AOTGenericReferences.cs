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
		"Newtonsoft.Json.dll",
		"SerializableDictionary.dll",
		"System.Core.dll",
		"System.dll",
		"UniRx.dll",
		"UniTask.dll",
		"Unity.Addressables.dll",
		"Unity.ResourceManager.dll",
		"UnityEngine.CoreModule.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<BattleSkillButton.<DoExposureGradient>d__69>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<BattleSkillButton.<DoSaturationGradient>d__70>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Character.<MoveClientToPos>d__44>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Battle.BattleManager.<CheckGameState>d__23>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Battle.BattleManager.<Init>d__22>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Battle.BattleManager.<InitBattleModelController>d__32>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Battle.BattleManager.<changeFovAsync>d__29>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Main.APIManager.<GameState>d__11,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Main.APIManager.<GetServerTime>d__5,System.DateTimeOffset>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Main.APIManager.<Signin>d__9,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Main.APIManager.<Signup>d__7,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Main.APIManager.<query>d__3,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Main.Projector.<move>d__4>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Main.StartSceneUI.<Signin>d__22>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Main.StartSceneUI.<onSignin>d__30>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Gladiators.Main.StartSceneUI.<signup>d__29>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Scoz.Func.AddressablesLoader.<GetResourceByFullPath_Async>d__4<object>,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Scoz.Func.Poster.<Get>d__1,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Scoz.Func.Poster.<Post>d__0,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Scoz.Func.UniTaskManager.<OneTimesTask>d__11>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<Scoz.Func.UniTaskManager.<RepeatTask>d__9>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<TestShader.<FirstWaitTask>d__38>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<TestShader.<SecondWaitTask>d__39>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<BattleSkillButton.<DoExposureGradient>d__69>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<BattleSkillButton.<DoSaturationGradient>d__70>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Character.<MoveClientToPos>d__44>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Battle.BattleManager.<CheckGameState>d__23>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Battle.BattleManager.<Init>d__22>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Battle.BattleManager.<InitBattleModelController>d__32>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Battle.BattleManager.<changeFovAsync>d__29>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Main.APIManager.<GameState>d__11,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Main.APIManager.<GetServerTime>d__5,System.DateTimeOffset>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Main.APIManager.<Signin>d__9,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Main.APIManager.<Signup>d__7,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Main.APIManager.<query>d__3,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Main.Projector.<move>d__4>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Main.StartSceneUI.<Signin>d__22>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Main.StartSceneUI.<onSignin>d__30>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Gladiators.Main.StartSceneUI.<signup>d__29>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Scoz.Func.AddressablesLoader.<GetResourceByFullPath_Async>d__4<object>,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Scoz.Func.Poster.<Get>d__1,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Scoz.Func.Poster.<Post>d__0,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Scoz.Func.UniTaskManager.<OneTimesTask>d__11>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<Scoz.Func.UniTaskManager.<RepeatTask>d__9>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<TestShader.<FirstWaitTask>d__38>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<TestShader.<SecondWaitTask>d__39>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<System.DateTimeOffset>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<BattleBufferIcon.<Shine>d__9>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<BattleController.<StateUpdateLoop>d__24>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<BattleGladiatorInfo.<HPChange>d__61>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<BattleGladiatorInfo.<HPGrayChange>d__62>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<BattleSceneUI.<ReleasedSkillLock>d__45>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<BattleSkillButton.<CastSkill>d__67>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<BattleSkillButton.<SetAvailableMaterial>d__58>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<BattleStaminaObj.<DoBrigtenMask>d__48>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<BattleStaminaObj.<DoCastLattices>d__40>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<BattleStaminaObj.<DoSkillVigorValFadeIn>d__46>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<BattleStaminaObj.<DoSkillVigorValFadeOut>d__44>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<Character.<>c__DisplayClass48_0.<<knockback>b__0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<Gladiators.Battle.DivineSelectUI.<DoCloseAni>d__54>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<Gladiators.Battle.DivineSelectUI.<PlayCountDownCandleTime>d__43>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<Gladiators.Battle.DivineSkill.<SelectSkill>d__12>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<Gladiators.Main.AllocatedRoom.<PingLoop>d__82>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<Gladiators.Socket.GameConnector.<>c__DisplayClass13_0.<<ConnectToMatchgameTestVer>b__0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<MaterialSwitcher.<<SetMetallicValue>b__5_0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<MaterialSwitcher.<<Start>b__3_0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<NextBattleSkillButton.<PlayChangeSkill>d__17>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<TestShader.<TryLerp>d__36>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<TestShader.<TryWaitTask>d__37>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<BattleBufferIcon.<Shine>d__9>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<BattleController.<StateUpdateLoop>d__24>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<BattleGladiatorInfo.<HPChange>d__61>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<BattleGladiatorInfo.<HPGrayChange>d__62>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<BattleSceneUI.<ReleasedSkillLock>d__45>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<BattleSkillButton.<CastSkill>d__67>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<BattleSkillButton.<SetAvailableMaterial>d__58>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<BattleStaminaObj.<DoBrigtenMask>d__48>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<BattleStaminaObj.<DoCastLattices>d__40>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<BattleStaminaObj.<DoSkillVigorValFadeIn>d__46>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<BattleStaminaObj.<DoSkillVigorValFadeOut>d__44>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<Character.<>c__DisplayClass48_0.<<knockback>b__0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<Gladiators.Battle.DivineSelectUI.<DoCloseAni>d__54>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<Gladiators.Battle.DivineSelectUI.<PlayCountDownCandleTime>d__43>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<Gladiators.Battle.DivineSkill.<SelectSkill>d__12>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<Gladiators.Main.AllocatedRoom.<PingLoop>d__82>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<Gladiators.Socket.GameConnector.<>c__DisplayClass13_0.<<ConnectToMatchgameTestVer>b__0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<MaterialSwitcher.<<SetMetallicValue>b__5_0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<MaterialSwitcher.<<Start>b__3_0>d>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<NextBattleSkillButton.<PlayChangeSkill>d__17>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<TestShader.<TryLerp>d__36>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<TestShader.<TryWaitTask>d__37>
	// Cysharp.Threading.Tasks.CompilerServices.IStateMachineRunnerPromise<System.DateTimeOffset>
	// Cysharp.Threading.Tasks.CompilerServices.IStateMachineRunnerPromise<object>
	// Cysharp.Threading.Tasks.ITaskPoolNode<object>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.DateTimeOffset>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.DateTimeOffset>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>>>
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
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.DateTimeOffset>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.DateTimeOffset>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>>>
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
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.DateTimeOffset>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.DateTimeOffset>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>>>
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
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.DateTimeOffset>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.DateTimeOffset>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>>>
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
	// Cysharp.Threading.Tasks.UniTask<System.DateTimeOffset>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.DateTimeOffset>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>>>>
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
	// Cysharp.Threading.Tasks.UniTaskCompletionSourceCore<Cysharp.Threading.Tasks.AsyncUnit>
	// Cysharp.Threading.Tasks.UniTaskCompletionSourceCore<System.DateTimeOffset>
	// Cysharp.Threading.Tasks.UniTaskCompletionSourceCore<object>
	// DelegateList<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>>
	// DelegateList<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<long>>
	// DelegateList<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>>
	// DelegateList<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// DelegateList<float>
	// Loxodon.Framework.Observables.ObservableProperty<byte>
	// Loxodon.Framework.Observables.ObservablePropertyBase<byte>
	// SerializableDictionary<int,object>
	// SerializableDictionary<object,object>
	// SerializableDictionaryBase<int,object,object>
	// SerializableDictionaryBase<object,object,object>
	// System.Action<System.Nullable<UnityEngine.SceneManagement.Scene>,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Action<UniRx.Unit>
	// System.Action<UniWebViewMessage>
	// System.Action<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle,object>
	// System.Action<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>>
	// System.Action<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<long>>
	// System.Action<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>>
	// System.Action<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Action<UnityEngine.UIVertex>
	// System.Action<UnityEngine.Vector3>
	// System.Action<byte>
	// System.Action<float>
	// System.Action<int>
	// System.Action<long>
	// System.Action<object,System.Nullable<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>>
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
	// System.Collections.Generic.ArraySortHelper<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.UIVertex>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Vector3>
	// System.Collections.Generic.ArraySortHelper<byte>
	// System.Collections.Generic.ArraySortHelper<float>
	// System.Collections.Generic.ArraySortHelper<int>
	// System.Collections.Generic.ArraySortHelper<long>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.ArraySortHelper<ushort>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.Comparer<System.DateTimeOffset>
	// System.Collections.Generic.Comparer<System.Numerics.BigInteger>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.DateTimeOffset>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.Comparer<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.Comparer<UnityEngine.UIVertex>
	// System.Collections.Generic.Comparer<UnityEngine.Vector3>
	// System.Collections.Generic.Comparer<byte>
	// System.Collections.Generic.Comparer<float>
	// System.Collections.Generic.Comparer<int>
	// System.Collections.Generic.Comparer<long>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Comparer<ushort>
	// System.Collections.Generic.Dictionary.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<long,long>
	// System.Collections.Generic.Dictionary.Enumerator<object,System.DateTimeOffset>
	// System.Collections.Generic.Dictionary.Enumerator<object,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.Dictionary.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,long>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,System.DateTimeOffset>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<long,long>
	// System.Collections.Generic.Dictionary.KeyCollection<object,System.DateTimeOffset>
	// System.Collections.Generic.Dictionary.KeyCollection<object,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.Dictionary.KeyCollection<object,byte>
	// System.Collections.Generic.Dictionary.KeyCollection<object,float>
	// System.Collections.Generic.Dictionary.KeyCollection<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,long>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,System.DateTimeOffset>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<long,long>
	// System.Collections.Generic.Dictionary.ValueCollection<object,System.DateTimeOffset>
	// System.Collections.Generic.Dictionary.ValueCollection<object,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.Dictionary.ValueCollection<object,byte>
	// System.Collections.Generic.Dictionary.ValueCollection<object,float>
	// System.Collections.Generic.Dictionary.ValueCollection<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary<int,int>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<long,long>
	// System.Collections.Generic.Dictionary<object,System.DateTimeOffset>
	// System.Collections.Generic.Dictionary<object,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.Dictionary<object,byte>
	// System.Collections.Generic.Dictionary<object,float>
	// System.Collections.Generic.Dictionary<object,int>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.EqualityComparer<System.DateTimeOffset>
	// System.Collections.Generic.EqualityComparer<System.Numerics.BigInteger>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.DateTimeOffset>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,object>>
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
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,System.DateTimeOffset>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.ICollection<UnityEngine.UIVertex>
	// System.Collections.Generic.ICollection<UnityEngine.Vector3>
	// System.Collections.Generic.ICollection<byte>
	// System.Collections.Generic.ICollection<float>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<long>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.ICollection<ushort>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IComparer<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.IComparer<UnityEngine.UIVertex>
	// System.Collections.Generic.IComparer<UnityEngine.Vector3>
	// System.Collections.Generic.IComparer<byte>
	// System.Collections.Generic.IComparer<float>
	// System.Collections.Generic.IComparer<int>
	// System.Collections.Generic.IComparer<long>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IComparer<ushort>
	// System.Collections.Generic.IDictionary<object,LitJson.ArrayMetadata>
	// System.Collections.Generic.IDictionary<object,LitJson.ObjectMetadata>
	// System.Collections.Generic.IDictionary<object,LitJson.PropertyMetadata>
	// System.Collections.Generic.IDictionary<object,object>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.UIntPtr,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,System.DateTimeOffset>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.IEnumerable<UnityEngine.UIVertex>
	// System.Collections.Generic.IEnumerable<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerable<byte>
	// System.Collections.Generic.IEnumerable<float>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<long>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerable<ushort>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.UIntPtr,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,System.DateTimeOffset>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.IEnumerator<UnityEngine.UIVertex>
	// System.Collections.Generic.IEnumerator<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerator<byte>
	// System.Collections.Generic.IEnumerator<float>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<long>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEnumerator<ushort>
	// System.Collections.Generic.IEqualityComparer<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<long>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IList<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.IList<UnityEngine.UIVertex>
	// System.Collections.Generic.IList<UnityEngine.Vector3>
	// System.Collections.Generic.IList<byte>
	// System.Collections.Generic.IList<float>
	// System.Collections.Generic.IList<int>
	// System.Collections.Generic.IList<long>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.IList<ushort>
	// System.Collections.Generic.KeyValuePair<System.UIntPtr,object>
	// System.Collections.Generic.KeyValuePair<int,int>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<long,long>
	// System.Collections.Generic.KeyValuePair<long,object>
	// System.Collections.Generic.KeyValuePair<object,System.DateTimeOffset>
	// System.Collections.Generic.KeyValuePair<object,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.KeyValuePair<object,byte>
	// System.Collections.Generic.KeyValuePair<object,float>
	// System.Collections.Generic.KeyValuePair<object,int>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.LinkedList.Enumerator<object>
	// System.Collections.Generic.LinkedList<object>
	// System.Collections.Generic.LinkedListNode<object>
	// System.Collections.Generic.List.Enumerator<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.List.Enumerator<UnityEngine.UIVertex>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Vector3>
	// System.Collections.Generic.List.Enumerator<byte>
	// System.Collections.Generic.List.Enumerator<float>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<long>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List.Enumerator<ushort>
	// System.Collections.Generic.List<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.List<UnityEngine.UIVertex>
	// System.Collections.Generic.List<UnityEngine.Vector3>
	// System.Collections.Generic.List<byte>
	// System.Collections.Generic.List<float>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<long>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.List<ushort>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ObjectComparer<System.DateTimeOffset>
	// System.Collections.Generic.ObjectComparer<System.Numerics.BigInteger>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.DateTimeOffset>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.ObjectComparer<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.ObjectComparer<UnityEngine.UIVertex>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Vector3>
	// System.Collections.Generic.ObjectComparer<byte>
	// System.Collections.Generic.ObjectComparer<float>
	// System.Collections.Generic.ObjectComparer<int>
	// System.Collections.Generic.ObjectComparer<long>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectComparer<ushort>
	// System.Collections.Generic.ObjectEqualityComparer<System.DateTimeOffset>
	// System.Collections.Generic.ObjectEqualityComparer<System.Numerics.BigInteger>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.DateTimeOffset>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.ObjectEqualityComparer<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.ObjectEqualityComparer<byte>
	// System.Collections.Generic.ObjectEqualityComparer<float>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<long>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.Queue.Enumerator<double>
	// System.Collections.Generic.Queue<double>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_0<long,object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_0<object,object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_1<long,object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_1<object,object>
	// System.Collections.Generic.SortedDictionary.Enumerator<long,object>
	// System.Collections.Generic.SortedDictionary.Enumerator<object,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass5_0<long,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass5_0<object,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass6_0<long,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass6_0<object,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.Enumerator<long,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection<long,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection<object,object>
	// System.Collections.Generic.SortedDictionary.KeyValuePairComparer<long,object>
	// System.Collections.Generic.SortedDictionary.KeyValuePairComparer<object,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass5_0<long,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass5_0<object,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass6_0<long,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass6_0<object,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.Enumerator<long,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection<long,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection<object,object>
	// System.Collections.Generic.SortedDictionary<long,object>
	// System.Collections.Generic.SortedDictionary<object,object>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass52_0<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass52_0<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass53_0<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass53_0<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.SortedSet.Enumerator<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet.Enumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.SortedSet.Node<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet.Node<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.SortedSet<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.Stack.Enumerator<object>
	// System.Collections.Generic.Stack<object>
	// System.Collections.Generic.TreeSet<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.TreeSet<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.TreeWalkPredicate<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.TreeWalkPredicate<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ValueListBuilder<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.UIVertex>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Vector3>
	// System.Collections.ObjectModel.ReadOnlyCollection<byte>
	// System.Collections.ObjectModel.ReadOnlyCollection<float>
	// System.Collections.ObjectModel.ReadOnlyCollection<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<long>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<ushort>
	// System.Comparison<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Comparison<UnityEngine.UIVertex>
	// System.Comparison<UnityEngine.Vector3>
	// System.Comparison<byte>
	// System.Comparison<float>
	// System.Comparison<int>
	// System.Comparison<long>
	// System.Comparison<object>
	// System.Comparison<ushort>
	// System.Converter<object,float>
	// System.Converter<object,int>
	// System.Converter<object,object>
	// System.Func<Cysharp.Threading.Tasks.UniTaskVoid>
	// System.Func<System.Collections.Generic.KeyValuePair<int,object>,int>
	// System.Func<System.Collections.Generic.KeyValuePair<int,object>,object>
	// System.Func<System.Collections.Generic.KeyValuePair<object,int>,System.Collections.Generic.KeyValuePair<object,int>>
	// System.Func<System.Collections.Generic.KeyValuePair<object,int>,object>
	// System.Func<System.Collections.Generic.KeyValuePair<object,object>,object>
	// System.Func<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>>
	// System.Func<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>
	// System.Func<byte,byte>
	// System.Func<byte,ushort>
	// System.Func<byte>
	// System.Func<float,byte>
	// System.Func<int,byte>
	// System.Func<int>
	// System.Func<long>
	// System.Func<object,UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>
	// System.Func<object,byte>
	// System.Func<object,float>
	// System.Func<object,int>
	// System.Func<object,long>
	// System.Func<object,object,object>
	// System.Func<object,object>
	// System.Func<object>
	// System.Func<ushort,byte>
	// System.IObservable<UniRx.Unit>
	// System.IObservable<object>
	// System.IObserver<UniRx.Unit>
	// System.IObserver<object>
	// System.Linq.Buffer<object>
	// System.Linq.Enumerable.<CastIterator>d__99<int>
	// System.Linq.Enumerable.<CastIterator>d__99<object>
	// System.Linq.Enumerable.<ConcatIterator>d__59<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.Enumerable.<OfTypeIterator>d__97<object>
	// System.Linq.Enumerable.Iterator<byte>
	// System.Linq.Enumerable.Iterator<float>
	// System.Linq.Enumerable.Iterator<int>
	// System.Linq.Enumerable.Iterator<object>
	// System.Linq.Enumerable.Iterator<ushort>
	// System.Linq.Enumerable.WhereArrayIterator<int>
	// System.Linq.Enumerable.WhereEnumerableIterator<float>
	// System.Linq.Enumerable.WhereEnumerableIterator<int>
	// System.Linq.Enumerable.WhereEnumerableIterator<ushort>
	// System.Linq.Enumerable.WhereListIterator<int>
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
	// System.Nullable<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Nullable<UnityEngine.SceneManagement.Scene>
	// System.Nullable<float>
	// System.Nullable<long>
	// System.Predicate<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Predicate<UnityEngine.UIVertex>
	// System.Predicate<UnityEngine.Vector3>
	// System.Predicate<byte>
	// System.Predicate<float>
	// System.Predicate<int>
	// System.Predicate<long>
	// System.Predicate<object>
	// System.Predicate<ushort>
	// System.ReadOnlySpan<int>
	// System.ReadOnlySpan<ushort>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<long>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<long>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<object>
	// System.Runtime.CompilerServices.TaskAwaiter<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>
	// System.Runtime.CompilerServices.TaskAwaiter<long>
	// System.Runtime.CompilerServices.TaskAwaiter<object>
	// System.Span<int>
	// System.Span<ushort>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<long>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<object>
	// System.Threading.Tasks.Task<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>
	// System.Threading.Tasks.Task<long>
	// System.Threading.Tasks.Task<object>
	// System.Threading.Tasks.TaskCompletionSource<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>
	// System.Threading.Tasks.TaskCompletionSource<long>
	// System.Threading.Tasks.TaskCompletionSource<object>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<long>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<object>
	// System.Threading.Tasks.TaskFactory<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>
	// System.Threading.Tasks.TaskFactory<long>
	// System.Threading.Tasks.TaskFactory<object>
	// System.Tuple<UnityEngine.Vector3,UnityEngine.Vector3>
	// System.Tuple<object,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Tuple<object,int>
	// System.ValueTuple<System.Numerics.BigInteger,System.Numerics.BigInteger>
	// System.ValueTuple<byte,System.DateTimeOffset>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.DateTimeOffset>>>>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,object>>
	// System.ValueTuple<byte,int>
	// System.ValueTuple<byte,object>
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
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationBase.<>c__DisplayClass60_0<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationBase.<>c__DisplayClass60_0<long>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationBase.<>c__DisplayClass60_0<object>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationBase.<>c__DisplayClass61_0<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationBase.<>c__DisplayClass61_0<long>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationBase.<>c__DisplayClass61_0<object>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationBase<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationBase<long>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationBase<object>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle.<>c<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle.<>c<long>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle.<>c<object>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<long>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>
	// UnityEngine.ResourceManagement.ChainOperationTypelessDepedency<object>
	// UnityEngine.ResourceManagement.ResourceManager.<>c__DisplayClass101_0<object>
	// UnityEngine.ResourceManagement.ResourceManager.CompletedOperation<object>
	// UnityEngine.ResourceManagement.Util.GlobalLinkedListNodeCache<object>
	// UnityEngine.ResourceManagement.Util.LinkedListNodeCache<object>
	// }}

	public void RefMethods()
	{
		// object Cinemachine.CinemachineVirtualCamera.GetCinemachineComponent<object>()
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Battle.BattleManager.<Init>d__22>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Battle.BattleManager.<Init>d__22&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Battle.BattleManager.<InitBattleModelController>d__32>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Battle.BattleManager.<InitBattleModelController>d__32&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Main.StartSceneUI.<Signin>d__22>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Main.StartSceneUI.<Signin>d__22&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Main.StartSceneUI.<signup>d__29>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Main.StartSceneUI.<signup>d__29&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Scoz.Func.UniTaskManager.<OneTimesTask>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Scoz.Func.UniTaskManager.<OneTimesTask>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Scoz.Func.UniTaskManager.<RepeatTask>d__9>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Scoz.Func.UniTaskManager.<RepeatTask>d__9&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,TestShader.<FirstWaitTask>d__38>(Cysharp.Threading.Tasks.UniTask.Awaiter&,TestShader.<FirstWaitTask>d__38&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,TestShader.<SecondWaitTask>d__39>(Cysharp.Threading.Tasks.UniTask.Awaiter&,TestShader.<SecondWaitTask>d__39&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<System.DateTimeOffset>,Gladiators.Main.StartSceneUI.<Signin>d__22>(Cysharp.Threading.Tasks.UniTask.Awaiter<System.DateTimeOffset>&,Gladiators.Main.StartSceneUI.<Signin>d__22&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Gladiators.Main.StartSceneUI.<Signin>d__22>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Gladiators.Main.StartSceneUI.<Signin>d__22&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Gladiators.Main.StartSceneUI.<onSignin>d__30>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Gladiators.Main.StartSceneUI.<onSignin>d__30&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Gladiators.Main.StartSceneUI.<signup>d__29>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Gladiators.Main.StartSceneUI.<signup>d__29&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,BattleSkillButton.<DoExposureGradient>d__69>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,BattleSkillButton.<DoExposureGradient>d__69&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,BattleSkillButton.<DoSaturationGradient>d__70>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,BattleSkillButton.<DoSaturationGradient>d__70&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,Gladiators.Battle.BattleManager.<CheckGameState>d__23>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,Gladiators.Battle.BattleManager.<CheckGameState>d__23&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,Gladiators.Battle.BattleManager.<changeFovAsync>d__29>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,Gladiators.Battle.BattleManager.<changeFovAsync>d__29&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,Gladiators.Main.Projector.<move>d__4>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,Gladiators.Main.Projector.<move>d__4&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Character.<MoveClientToPos>d__44>(System.Runtime.CompilerServices.TaskAwaiter&,Character.<MoveClientToPos>d__44&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<System.DateTimeOffset>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Gladiators.Main.APIManager.<GetServerTime>d__5>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Gladiators.Main.APIManager.<GetServerTime>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Scoz.Func.AddressablesLoader.<GetResourceByFullPath_Async>d__4<object>>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Scoz.Func.AddressablesLoader.<GetResourceByFullPath_Async>d__4<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Gladiators.Main.APIManager.<GameState>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Gladiators.Main.APIManager.<GameState>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Gladiators.Main.APIManager.<Signin>d__9>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Gladiators.Main.APIManager.<Signin>d__9&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Gladiators.Main.APIManager.<Signup>d__7>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Gladiators.Main.APIManager.<Signup>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Gladiators.Main.APIManager.<query>d__3>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Gladiators.Main.APIManager.<query>d__3&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UnityAsyncExtensions.UnityWebRequestAsyncOperationAwaiter,Scoz.Func.Poster.<Get>d__1>(Cysharp.Threading.Tasks.UnityAsyncExtensions.UnityWebRequestAsyncOperationAwaiter&,Scoz.Func.Poster.<Get>d__1&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UnityAsyncExtensions.UnityWebRequestAsyncOperationAwaiter,Scoz.Func.Poster.<Post>d__0>(Cysharp.Threading.Tasks.UnityAsyncExtensions.UnityWebRequestAsyncOperationAwaiter&,Scoz.Func.Poster.<Post>d__0&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<BattleSkillButton.<DoExposureGradient>d__69>(BattleSkillButton.<DoExposureGradient>d__69&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<BattleSkillButton.<DoSaturationGradient>d__70>(BattleSkillButton.<DoSaturationGradient>d__70&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Character.<MoveClientToPos>d__44>(Character.<MoveClientToPos>d__44&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Gladiators.Battle.BattleManager.<CheckGameState>d__23>(Gladiators.Battle.BattleManager.<CheckGameState>d__23&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Gladiators.Battle.BattleManager.<Init>d__22>(Gladiators.Battle.BattleManager.<Init>d__22&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Gladiators.Battle.BattleManager.<InitBattleModelController>d__32>(Gladiators.Battle.BattleManager.<InitBattleModelController>d__32&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Gladiators.Battle.BattleManager.<changeFovAsync>d__29>(Gladiators.Battle.BattleManager.<changeFovAsync>d__29&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Gladiators.Main.AllocatedRoom.<SetRoom_TestvVer>d__68>(Gladiators.Main.AllocatedRoom.<SetRoom_TestvVer>d__68&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Gladiators.Main.Projector.<move>d__4>(Gladiators.Main.Projector.<move>d__4&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Gladiators.Main.StartSceneUI.<Signin>d__22>(Gladiators.Main.StartSceneUI.<Signin>d__22&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Gladiators.Main.StartSceneUI.<onSignin>d__30>(Gladiators.Main.StartSceneUI.<onSignin>d__30&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Gladiators.Main.StartSceneUI.<signup>d__29>(Gladiators.Main.StartSceneUI.<signup>d__29&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Scoz.Func.UniTaskManager.<OneTimesTask>d__11>(Scoz.Func.UniTaskManager.<OneTimesTask>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Scoz.Func.UniTaskManager.<RepeatTask>d__9>(Scoz.Func.UniTaskManager.<RepeatTask>d__9&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<TestShader.<FirstWaitTask>d__38>(TestShader.<FirstWaitTask>d__38&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<TestShader.<SecondWaitTask>d__39>(TestShader.<SecondWaitTask>d__39&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<System.DateTimeOffset>.Start<Gladiators.Main.APIManager.<GetServerTime>d__5>(Gladiators.Main.APIManager.<GetServerTime>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Gladiators.Main.APIManager.<GameState>d__11>(Gladiators.Main.APIManager.<GameState>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Gladiators.Main.APIManager.<Signin>d__9>(Gladiators.Main.APIManager.<Signin>d__9&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Gladiators.Main.APIManager.<Signup>d__7>(Gladiators.Main.APIManager.<Signup>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Gladiators.Main.APIManager.<query>d__3>(Gladiators.Main.APIManager.<query>d__3&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Scoz.Func.AddressablesLoader.<GetResourceByFullPath_Async>d__4<object>>(Scoz.Func.AddressablesLoader.<GetResourceByFullPath_Async>d__4<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Scoz.Func.Poster.<Get>d__1>(Scoz.Func.Poster.<Get>d__1&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Scoz.Func.Poster.<Post>d__0>(Scoz.Func.Poster.<Post>d__0&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,BattleBufferIcon.<Shine>d__9>(Cysharp.Threading.Tasks.UniTask.Awaiter&,BattleBufferIcon.<Shine>d__9&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,BattleGladiatorInfo.<HPChange>d__61>(Cysharp.Threading.Tasks.UniTask.Awaiter&,BattleGladiatorInfo.<HPChange>d__61&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,BattleGladiatorInfo.<HPGrayChange>d__62>(Cysharp.Threading.Tasks.UniTask.Awaiter&,BattleGladiatorInfo.<HPGrayChange>d__62&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,BattleSceneUI.<ReleasedSkillLock>d__45>(Cysharp.Threading.Tasks.UniTask.Awaiter&,BattleSceneUI.<ReleasedSkillLock>d__45&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,BattleSkillButton.<CastSkill>d__67>(Cysharp.Threading.Tasks.UniTask.Awaiter&,BattleSkillButton.<CastSkill>d__67&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,BattleSkillButton.<SetAvailableMaterial>d__58>(Cysharp.Threading.Tasks.UniTask.Awaiter&,BattleSkillButton.<SetAvailableMaterial>d__58&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,BattleStaminaObj.<DoCastLattices>d__40>(Cysharp.Threading.Tasks.UniTask.Awaiter&,BattleStaminaObj.<DoCastLattices>d__40&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Battle.DivineSelectUI.<DoCloseAni>d__54>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Battle.DivineSelectUI.<DoCloseAni>d__54&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Battle.DivineSelectUI.<PlayCountDownCandleTime>d__43>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Battle.DivineSelectUI.<PlayCountDownCandleTime>d__43&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Battle.DivineSkill.<SelectSkill>d__12>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Battle.DivineSkill.<SelectSkill>d__12&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Main.AllocatedRoom.<PingLoop>d__82>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Main.AllocatedRoom.<PingLoop>d__82&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Socket.GameConnector.<>c__DisplayClass13_0.<<ConnectToMatchgameTestVer>b__0>d>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Socket.GameConnector.<>c__DisplayClass13_0.<<ConnectToMatchgameTestVer>b__0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,MaterialSwitcher.<<SetMetallicValue>b__5_0>d>(Cysharp.Threading.Tasks.UniTask.Awaiter&,MaterialSwitcher.<<SetMetallicValue>b__5_0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,MaterialSwitcher.<<Start>b__3_0>d>(Cysharp.Threading.Tasks.UniTask.Awaiter&,MaterialSwitcher.<<Start>b__3_0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,NextBattleSkillButton.<PlayChangeSkill>d__17>(Cysharp.Threading.Tasks.UniTask.Awaiter&,NextBattleSkillButton.<PlayChangeSkill>d__17&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,TestShader.<TryLerp>d__36>(Cysharp.Threading.Tasks.UniTask.Awaiter&,TestShader.<TryLerp>d__36&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,TestShader.<TryWaitTask>d__37>(Cysharp.Threading.Tasks.UniTask.Awaiter&,TestShader.<TryWaitTask>d__37&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,BattleController.<StateUpdateLoop>d__24>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,BattleController.<StateUpdateLoop>d__24&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,BattleStaminaObj.<DoBrigtenMask>d__48>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,BattleStaminaObj.<DoBrigtenMask>d__48&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,BattleStaminaObj.<DoCastLattices>d__40>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,BattleStaminaObj.<DoCastLattices>d__40&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,BattleStaminaObj.<DoSkillVigorValFadeIn>d__46>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,BattleStaminaObj.<DoSkillVigorValFadeIn>d__46&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,BattleStaminaObj.<DoSkillVigorValFadeOut>d__44>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,BattleStaminaObj.<DoSkillVigorValFadeOut>d__44&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,Character.<>c__DisplayClass48_0.<<knockback>b__0>d>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,Character.<>c__DisplayClass48_0.<<knockback>b__0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,Gladiators.Battle.DivineSelectUI.<PlayCountDownCandleTime>d__43>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,Gladiators.Battle.DivineSelectUI.<PlayCountDownCandleTime>d__43&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleBufferIcon.<Shine>d__9>(BattleBufferIcon.<Shine>d__9&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleController.<StateUpdateLoop>d__24>(BattleController.<StateUpdateLoop>d__24&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleGladiatorInfo.<<PerformHPChange>b__60_0>d>(BattleGladiatorInfo.<<PerformHPChange>b__60_0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleGladiatorInfo.<>c__DisplayClass61_0.<<HPChange>b__0>d>(BattleGladiatorInfo.<>c__DisplayClass61_0.<<HPChange>b__0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleGladiatorInfo.<HPChange>d__61>(BattleGladiatorInfo.<HPChange>d__61&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleGladiatorInfo.<HPGrayChange>d__62>(BattleGladiatorInfo.<HPGrayChange>d__62&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleSceneUI.<ReleasedSkillLock>d__45>(BattleSceneUI.<ReleasedSkillLock>d__45&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleSkillButton.<CastSkill>d__67>(BattleSkillButton.<CastSkill>d__67&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleSkillButton.<SetAvailableMaterial>d__58>(BattleSkillButton.<SetAvailableMaterial>d__58&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleStaminaObj.<DoBrigtenMask>d__48>(BattleStaminaObj.<DoBrigtenMask>d__48&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleStaminaObj.<DoCastLattices>d__40>(BattleStaminaObj.<DoCastLattices>d__40&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleStaminaObj.<DoSkillVigorValFadeIn>d__46>(BattleStaminaObj.<DoSkillVigorValFadeIn>d__46&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleStaminaObj.<DoSkillVigorValFadeOut>d__44>(BattleStaminaObj.<DoSkillVigorValFadeOut>d__44&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Character.<>c__DisplayClass48_0.<<knockback>b__0>d>(Character.<>c__DisplayClass48_0.<<knockback>b__0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Gladiators.Battle.DivineSelectUI.<>c__DisplayClass53_0.<<CloseUI>b__0>d>(Gladiators.Battle.DivineSelectUI.<>c__DisplayClass53_0.<<CloseUI>b__0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Gladiators.Battle.DivineSelectUI.<DoCloseAni>d__54>(Gladiators.Battle.DivineSelectUI.<DoCloseAni>d__54&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Gladiators.Battle.DivineSelectUI.<PlayCountDownCandleTime>d__43>(Gladiators.Battle.DivineSelectUI.<PlayCountDownCandleTime>d__43&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Gladiators.Battle.DivineSkill.<SelectSkill>d__12>(Gladiators.Battle.DivineSkill.<SelectSkill>d__12&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Gladiators.Main.AllocatedRoom.<PingLoop>d__82>(Gladiators.Main.AllocatedRoom.<PingLoop>d__82&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Gladiators.Socket.GameConnector.<<ConnToMatchgame>b__12_0>d>(Gladiators.Socket.GameConnector.<<ConnToMatchgame>b__12_0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Gladiators.Socket.GameConnector.<>c__DisplayClass13_0.<<ConnectToMatchgameTestVer>b__0>d>(Gladiators.Socket.GameConnector.<>c__DisplayClass13_0.<<ConnectToMatchgameTestVer>b__0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<MaterialSwitcher.<<SetMetallicValue>b__5_0>d>(MaterialSwitcher.<<SetMetallicValue>b__5_0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<MaterialSwitcher.<<Start>b__3_0>d>(MaterialSwitcher.<<Start>b__3_0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<NextBattleSkillButton.<PlayChangeSkill>d__17>(NextBattleSkillButton.<PlayChangeSkill>d__17&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<TestShader.<<Update>b__35_0>d>(TestShader.<<Update>b__35_0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<TestShader.<<Update>b__35_1>d>(TestShader.<<Update>b__35_1>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<TestShader.<TryLerp>d__36>(TestShader.<TryLerp>d__36&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<TestShader.<TryWaitTask>d__37>(TestShader.<TryWaitTask>d__37&)
		// Cysharp.Threading.Tasks.UniTask.Awaiter Cysharp.Threading.Tasks.EnumeratorAsyncExtensions.GetAwaiter<object>(object)
		// object DG.Tweening.TweenExtensions.Pause<object>(object)
		// object DG.Tweening.TweenSettingsExtensions.OnComplete<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.SetAutoKill<object>(object,bool)
		// object DG.Tweening.TweenSettingsExtensions.SetDelay<object>(object,float)
		// object DG.Tweening.TweenSettingsExtensions.SetEase<object>(object,DG.Tweening.Ease)
		// object LitJson.JsonMapper.ToObject<object>(string)
		// object Newtonsoft.Json.JsonConvert.DeserializeObject<object>(string)
		// object Newtonsoft.Json.JsonConvert.DeserializeObject<object>(string,Newtonsoft.Json.JsonSerializerSettings)
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
		// bool System.Linq.Enumerable.Contains<int>(System.Collections.Generic.IEnumerable<int>,int)
		// bool System.Linq.Enumerable.Contains<int>(System.Collections.Generic.IEnumerable<int>,int,System.Collections.Generic.IEqualityComparer<int>)
		// System.Collections.Generic.KeyValuePair<object,int> System.Linq.Enumerable.First<System.Collections.Generic.KeyValuePair<object,int>>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>)
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
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.Where<int>(System.Collections.Generic.IEnumerable<int>,System.Func<int,bool>)
		// System.Collections.Generic.IEnumerable<float> System.Linq.Enumerable.Iterator<object>.Select<float>(System.Func<object,float>)
		// System.Collections.Generic.IEnumerable<ushort> System.Linq.Enumerable.Iterator<byte>.Select<ushort>(System.Func<byte,ushort>)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Gladiators.Socket.TcpClient.<Thread_Connect>d__37>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Gladiators.Socket.TcpClient.<Thread_Connect>d__37&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Gladiators.Socket.TcpClient.<Thread_Connect>d__37>(Gladiators.Socket.TcpClient.<Thread_Connect>d__37&)
		// object& System.Runtime.CompilerServices.Unsafe.As<object,object>(object&)
		// System.Void* System.Runtime.CompilerServices.Unsafe.AsPointer<object>(object&)
		// System.IDisposable UniRx.ObservableExtensions.Subscribe<UniRx.Unit>(System.IObservable<UniRx.Unit>,System.Action<UniRx.Unit>,System.Action<System.Exception>)
		// System.IDisposable UniRx.ObservableExtensions.Subscribe<object>(System.IObservable<object>,System.Action<object>,System.Action<System.Exception>)
		// System.IObserver<UniRx.Unit> UniRx.Observer.CreateSubscribeObserver<UniRx.Unit>(System.Action<UniRx.Unit>,System.Action<System.Exception>,System.Action)
		// System.IObserver<object> UniRx.Observer.CreateSubscribeObserver<object>(System.Action<object>,System.Action<System.Exception>,System.Action)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object> UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<object>(object)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<System.Collections.Generic.IList<object>> UnityEngine.AddressableAssets.Addressables.LoadAssetsAsync<object>(object,System.Action<object>)
		// System.Void UnityEngine.AddressableAssets.Addressables.Release<System.Nullable<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>>(System.Nullable<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>)
		// System.Void UnityEngine.AddressableAssets.Addressables.Release<object>(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object> UnityEngine.AddressableAssets.AddressablesImpl.LoadAssetAsync<object>(object)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object> UnityEngine.AddressableAssets.AddressablesImpl.LoadAssetWithChain<object>(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle,object)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<System.Collections.Generic.IList<object>> UnityEngine.AddressableAssets.AddressablesImpl.LoadAssetsAsync<object>(System.Collections.Generic.IList<UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation>,System.Action<object>,bool)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<System.Collections.Generic.IList<object>> UnityEngine.AddressableAssets.AddressablesImpl.LoadAssetsAsync<object>(object,System.Action<object>,bool)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<System.Collections.Generic.IList<object>> UnityEngine.AddressableAssets.AddressablesImpl.LoadAssetsWithChain<object>(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle,System.Collections.Generic.IList<UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation>,System.Action<object>,bool)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<System.Collections.Generic.IList<object>> UnityEngine.AddressableAssets.AddressablesImpl.LoadAssetsWithChain<object>(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle,object,System.Action<object>,bool)
		// System.Void UnityEngine.AddressableAssets.AddressablesImpl.Release<System.Nullable<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>>(System.Nullable<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>)
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