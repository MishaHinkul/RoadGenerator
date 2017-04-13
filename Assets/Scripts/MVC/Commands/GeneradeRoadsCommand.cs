using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradeRoadsCommand : BaseCommand
{
    [Inject]
    public RoadNetworkModel networkModel { get; private set; }

    public override void Execute()
    {
        networkModel.Scale = 100;
        networkModel.Init();


        CenterTemplateModel templateModel = new CenterTemplateModel(Vector2.zero, 120);
        dispatcher.Dispatch(EventGlobal.E_SetTemplate, templateModel);
    }
}
