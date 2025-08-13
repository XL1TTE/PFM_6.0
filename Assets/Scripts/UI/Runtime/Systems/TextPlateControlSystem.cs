using Core.Utilities.Extentions;
using Scellecs.Morpeh;
using UI.Components;
using UI.Components.Tags;
using UI.Requests;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class TextPlateControlSystem : ISystem 
{
    public World World { get; set;}
    
    private Filter _textPlates;
    
    private Stash<TextMeshProRefComponent> stash_tmpRef;
    private Stash<PlateWithTextTag> stash_plateTags;
    
    private Request<TextPlateChangeTextRequest> _request;

    public void OnAwake() 
    {
        _textPlates = World.Filter.With<PlateWithTextTag>().Build();

        stash_tmpRef = World.GetStash<TextMeshProRefComponent>();
        stash_plateTags = World.GetStash<PlateWithTextTag>();

        _request = World.GetRequest<TextPlateChangeTextRequest>();
    }

    public void OnUpdate(float deltaTime) 
    {
        foreach(var req in _request.Consume()){
            var plate = GetPlateFromOrigin(req.origin);
            
            if(plate.IsExist()){
                stash_tmpRef.Get(plate).TMP.text = req.message;
            }
        }
    }

    public void Dispose()
    {

    }
    
    private Entity GetPlateFromOrigin(PlateWithTextTag.Origin origin){
        foreach(var e in _textPlates){
            if(stash_plateTags.Get(e).origin == origin){return e;}
        }
        return default;
    }
}
