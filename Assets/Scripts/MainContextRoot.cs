using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;
using strange.extensions.context.api;
using strange.examples.strangerocks;

public class MainContextRoot : MVCSContext
{
    public MainContextRoot(MonoBehaviour contextView) : base(contextView, ContextStartupFlags.MANUAL_MAPPING)
    {
    }

    // CoreComponents
    protected override void addCoreComponents()
    {
        base.addCoreComponents();
        injectionBinder.Bind<ICoroutineExecutor>().To<CoroutineExecutor>().ToSingleton();

        //Model
        injectionBinder.Bind<RoadNetworkModel>().ToSingleton();
        injectionBinder.Bind<GraphModel>().ToSingleton();
        injectionBinder.Bind<TreeEntryModel>().ToSingleton();
        injectionBinder.Bind<PopulationsModel>().ToSingleton();
        injectionBinder.Bind<SettingsModel>().ToSingleton();
    }

    // Commands and Bindings
    protected override void mapBindings()
    {
        base.mapBindings();

        //Mediator
        mediationBinder.Bind<UILoaderView>().To<UILoaderMediator>();
        mediationBinder.Bind<DebugView>().To<DebugMediator>();
        mediationBinder.Bind<AgentView>().To<AgentMediator>();
        mediationBinder.Bind<AgentBehaviurView>().To<AgentBehaviourMediator>();
        mediationBinder.Bind<SeekView>().To<SeekMediator>();
        mediationBinder.Bind<PathFollowerView>().To<FollowPathMediator>();
        mediationBinder.Bind<CarView>().To<CarMediator>();

        commandBinder.Bind(ContextEvent.START).To<AppStartCommand>()
            .To<LoadLevelStartGameCommand>()
            .Pooled().InSequence().Once();

        //Roads Network

        //От базовой фигуры зависит вид всей дорожной сети
        commandBinder.Bind(EventGlobal.E_SetTemplate).To<XCenterTemplateCommand>();

        commandBinder.Bind(EventGlobal.E_SplitSegmentForLevel).To<SplitSegmentForLevelCommand>();
        commandBinder.Bind(EventGlobal.E_SplitSegment).To<SplitSegmentCommand>();
        commandBinder.Bind(EventGlobal.E_GeneradeRoads).To<GeneradeRoadsCommand>()
                                                       .To<SplitBaseConfigurationCommand>()
                                                       .To<ShowIntersectionCommand>()
                                                       .To<ShowRoadSegmentsCommands>()
                                                       .To<LoadGraphCommand>()
                                                       .To<DisableNavigationColliderCommand>()
                                                       .To<InitTreeEntryCommand>()
                                                       .To<GeneradeGasStationCommand>()
                                                       .To<SpawnCarsCommand>().Pooled();

        //Car
        commandBinder.Bind(EventGlobal.E_CarLogics).To<CarLogicCommand>();
        commandBinder.Bind(EventGlobal.E_LeaveСity).To<LeaveСityCommand>();

        //Debug
        commandBinder.Bind(EventGlobal.E_Dubug_ShowPath).To<DebugBuilPuthCommand>();

        //Other
        commandBinder.Bind(EventGlobal.E_WaitTime).To<WaitTimeCommand>();
    }
}