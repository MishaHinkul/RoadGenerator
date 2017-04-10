using strange.extensions.mediation.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.context.api;

public class BaseView : View
{
	[Inject(ContextKeys.CONTEXT_DISPATCHER)]
	public IEventDispatcher dispatcher { get; set;}

}