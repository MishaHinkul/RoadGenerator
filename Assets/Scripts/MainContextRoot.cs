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
    }

    // Commands and Bindings
    protected override void mapBindings()
    {
        base.mapBindings();

        //Mediator
        mediationBinder.Bind<UILoaderView>().To<UILoaderMediator>();

        commandBinder.Bind(ContextEvent.START).To<AppStartCommand>()
            .To<LoadLevelStartGameCommand>()
            .Pooled().InSequence().Once();

    }
}