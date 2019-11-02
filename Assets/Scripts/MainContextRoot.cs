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
    injectionBinder.Bind<EntryModel>().ToSingleton();
    injectionBinder.Bind<PopulationsModel>().ToSingleton();
    injectionBinder.Bind<SettingsModel>().ToSingleton();
    injectionBinder.Bind<CameraSettings>().ToSingleton();
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
    mediationBinder.Bind<CameraView>().To<CameraMediator>();
    mediationBinder.Bind<MainMenuView>().To<MainMenuMediator>();

    commandBinder.Bind(ContextEvent.START)
                 .To<AppStartCommand>()
                 .To<ResourceLoadMainMenuCommand>()
                 .Pooled().InSequence().Once();

    //Camera
    commandBinder.Bind(EventGlobal.E_CameraMove).To<MoveCameraCommand>().To<UpdateSettingsCameraCommand>().InSequence().Pooled();
    commandBinder.Bind(EventGlobal.E_CameraScale).To<ScaleCameraCommand>().To<UpdateSettingsCameraCommand>().InSequence().Pooled();

    //Roads Network

    commandBinder.Bind(EventGlobal.E_ShowIntersection).To<ShowIntersectionCommand>();
    commandBinder.Bind(EventGlobal.E_ShowSegment).To<ShowRoadSegmentCommand>();
    commandBinder.Bind(EventGlobal.E_SetTemplate).To<XCenterTemplateCommand>();  //От базовой фигуры зависит вид всей дорожной сети

    commandBinder.Bind(EventGlobal.E_SplitSegmentForLevel).To<SplitSegmentForLevelCommand>();
    commandBinder.Bind(EventGlobal.E_SplitSegment).To<SplitSegmentCommand>();

    commandBinder.Bind(EventGlobal.E_GeneradeRoads).To<LoadLevelStartGameCommand>()
                                                   .To<RoadSettingsCommand>()
                                                   .To<SetCameraSettingsCommand>()
                                                   .To<UpdateSettingsCameraCommand>()
                                                   .To<XCenterTemplateCommand>()
                                                   //.To<SplitBaseConfigurationCommand>()
                                                   //.To<ShowIntersectionCommand>()
                                                   //.To<ShowRoadSegmentsCommands>()
                                                   //.To<LoadGraphCommand>()
                                                   //.To<DisableNavigationColliderCommand>()
                                                   //.To<InitTreeEntryCommand>()
                                                   //.To<GeneradeGasStationCommand>()
                                                   /*.To<SpawnCarsCommand>()*/.InSequence().Pooled();

    //Car
    commandBinder.Bind(EventGlobal.E_CarLogics).To<CarLogicCommand>();
    commandBinder.Bind(EventGlobal.E_LeaveСity).To<LeaveСityCommand>();

    //Other
    commandBinder.Bind(EventGlobal.E_WaitTime).To<WaitTimeCommand>();
  }
}