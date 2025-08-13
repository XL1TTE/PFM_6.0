using Core.Components;
using Scellecs.Morpeh;
using UI.Components;
using UI.Requests;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class ButtonClickObserveSystem : ISystem 
{
    public World World { get; set;}
    
    private Filter _buttonsUnderCursor;
    
    private Request<ButtonClickedRequest> req_btnClicked;

    public void OnAwake() 
    {
        _buttonsUnderCursor = World.Filter
            .With<ButtonTag>()
            .With<UnderCursorComponent>()
            .Build();

        req_btnClicked = World.GetRequest<ButtonClickedRequest>();
    }

    public void OnUpdate(float deltaTime) 
    {
        if(_buttonsUnderCursor.IsEmpty()){return;}
        
        if(Input.GetMouseButtonDown(0)){
            req_btnClicked.Publish(new ButtonClickedRequest{
               ClickedButton = _buttonsUnderCursor.First() 
            });
        }
    }

    public void Dispose()
    {

    }
}
