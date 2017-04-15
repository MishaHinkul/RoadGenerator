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

        injectionBinder.Bind<RoadNetworkModel>().ToSingleton();
        injectionBinder.Bind<GraphModel>().ToSingleton();
    }

    // Commands and Bindings
    protected override void mapBindings()
    {
        base.mapBindings();

        //Mediator
        mediationBinder.Bind<UILoaderView>().To<UILoaderMediator>();
        mediationBinder.Bind<DebugView>().To<DebugMediator>();

        commandBinder.Bind(ContextEvent.START).To<AppStartCommand>()
            .To<LoadLevelStartGameCommand>()
            .Pooled().InSequence().Once();

        commandBinder.Bind(EventGlobal.E_SplitSegmentForLevel).To<SplitSegmentForLevelCommand>();
        commandBinder.Bind(EventGlobal.E_SplitSegment).To<SplitSegmentCommand>();
        commandBinder.Bind(EventGlobal.E_SetTemplate).To<XCenterTemplateCommand>();

        commandBinder.Bind(EventGlobal.E_GeneradeRoads).To<GeneradeRoadsCommand>()
                                                       .To<SplitBaseConfigurationCommand>()
                                                       .To<ShowIntersectionCommand>()
                                                       .To<ShowRoadSegmentsCommands>()
                                                       .To<LoadGraphCommand>()
                                                       .To<DebugBuilPuthCommand>()
                                                       .To<ShowDrawLineRoadsCommand>().Pooled();
    }
}