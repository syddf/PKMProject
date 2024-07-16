using Naninovel;
using Naninovel.Commands;

[InitializeAtRuntime]
public class ChangeNextRevealSpeedService : IStatefulService<GameStateMap>
{
    [System.Serializable]
    class GameState { public float Modifier; }

    public float NextRevealSpeedMofifier { get; set; }

    public UniTask InitializeServiceAsync() => UniTask.CompletedTask;

    public void ResetService() => NextRevealSpeedMofifier = 1;

    public void DestroyService() { }

    public void SaveServiceState(GameStateMap stateMap)
    {
        var state = new GameState() { Modifier = NextRevealSpeedMofifier };
        stateMap.SetState(state);
    }

    public UniTask LoadServiceStateAsync(GameStateMap stateMap)
    {
        NextRevealSpeedMofifier = stateMap.GetState<GameState>()?.Modifier ?? 1;
        return UniTask.CompletedTask;
    }
}

[CommandAlias("s")]
public class ChangeNextRevealSpeedCommand : Command
{
    [ParameterAlias(NamelessParameterAlias), RequiredParameter]
    public DecimalParameter SpeedModifier;

    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        Engine.GetService<ChangeNextRevealSpeedService>().NextRevealSpeedMofifier = SpeedModifier;
        return UniTask.CompletedTask;
    }
}

[CommandAlias("print")]
public class MyCustomPrintCommand : PrintText
{
    protected override float AssignedRevealSpeed => base.AssignedRevealSpeed *
        Engine.GetService<ChangeNextRevealSpeedService>().NextRevealSpeedMofifier;

    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        await base.ExecuteAsync(asyncToken);
        Engine.GetService<ChangeNextRevealSpeedService>().NextRevealSpeedMofifier = 1f;
    }
}