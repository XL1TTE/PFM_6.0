using Domain.Components;

namespace Persistence.DB{

    public sealed class ev_TextTest1 : IDbRecord{
        public ev_TextTest1()
        {
            // -------------------------------------------------- ID and TAG
            With<ID>(new ID{Value = "ev_TextTest1"}); 
            With<MapEvTextTag>(new MapEvTextTag { });


            // -------------------------------------------------- Required components
            With<MapEvStageRequirComponent>(new MapEvStageRequirComponent 
            { 
                acceptable_stages = new System.Collections.Generic.List<STAGES> {
                    STAGES.VILLAGE
                }
            });
            With<MapEvCollumnRequirComponent>(new MapEvCollumnRequirComponent
            {
                count_start_from_zero = false,
                count_offset = 1,
                count_offset_percentile = 0.2f,
            });


            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/Spr_Bodypart_Head_Test_1"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "I FUCKING HATE YOU I FUCKING HATE YOU I FUCKING HATE YOU I FUCKING HATE YOU I FUCKING HATE YOU I FUCKING HATE YOU I FUCKING HATE YOU I FUCKING HATE YOU"
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = {

                    { "SCHEISSE!",
                        "Map/MapTextEventResultScripts/Scr_MapEvResult_Test1" },

                    {"NO CHOICE!",
                        "Map/MapTextEventResultScripts/Scr_MapEvResult_Test1" },

                    {"Люби себя ^_^",
                        "Map/MapTextEventResultScripts/Scr_MapEvResult_Test1" }
                }
            });
        }
    }
    public sealed class ev_TextTest2 : IDbRecord
    {
        public ev_TextTest2()
        {
            With<ID>(new ID { Value = "ev_TextTest2" });
            With<MapEvCollumnRequirComponent>(new MapEvCollumnRequirComponent
            {
                count_start_from_zero = true,
                count_offset = 2,
                count_offset_percentile = 0.4f
            });
        }
    }
}
