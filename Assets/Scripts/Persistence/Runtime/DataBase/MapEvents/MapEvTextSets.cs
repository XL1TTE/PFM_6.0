using Domain.Components;
using Domain.MapEvents.Requests;
using Scellecs.Morpeh;
using System.Collections.Generic;

namespace Persistence.DB
{

    public sealed class ev_FAILSAFE : IDbRecord
    {
        public ev_FAILSAFE()
        {
            ID("ev_FAILSAFE");

            // -------------------------------------------------- ID and TAG
            With<ID>(new ID { m_Value = "ev_FAILSAFE" });
            With<MapEvTextTag>(new MapEvTextTag { });

            // -------------------------------------------------- Required components



            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/Spr_Bodypart_Head_Test_1"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "IF YOU SEE THIS, THEN SOMETHING WENT WRONG"
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, IRequestData>
                {

                    { "�, ���� :(",
                        new GiveGoldRequest() { amount = 10 } }

                }
            });
        }
    }

    public sealed class ev_TextDefault : IDbRecord
    {
        public ev_TextDefault()
        {
            ID("ev_TextDefault");

            // -------------------------------------------------- ID and TAG
            With<ID>(new ID { m_Value = "ev_TextDefault" });
            With<MapEvTextTag>(new MapEvTextTag { });

            // -------------------------------------------------- Required components



            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/Spr_Bodypart_Head_Test_1"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "This is a default message"
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, IRequestData>
                {

                    { "This is a default option",
                        new GiveGoldRequest() { amount = 10 } }

                }
            });
        }
    }
    public sealed class ev_TextTest1 : IDbRecord
    {
        public ev_TextTest1()
        {

            ID("ev_TextTest1");

            // -------------------------------------------------- ID and TAG
            With<ID>(new ID { m_Value = "ev_TextTest1" });
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

            //With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent /// BUG HERE
            //{
            //    choices = {
            //
            //        { "SCHEISSE!",
            //            "Map/MapTextEventResultScripts/Scr_MapEvResult_Test1" },
            //        
            //        {"NO CHOICE!",
            //            "Map/MapTextEventResultScripts/Scr_MapEvResult_Test1" },
            //        
            //        {"���� ���� ^_^",
            //            "Map/MapTextEventResultScripts/Scr_MapEvResult_Test1" },
            //    }
            //});


            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                //choices = new Dictionary<string, ScriptClass>
                choices = new Dictionary<string, IRequestData>
                {
                    //{ "SCHEISSE!",
                    //    GiveItems{ id1,id2,id3 } },

                    { "SCHEISSE!",
                        new TakeGoldRequest() { amount = 10 }  },

                    {"NO CHOICE!",
                        new TakeGoldRequest() { amount = 100 }  },

                    {"���� ���� ^_^",
                        new GiveGoldRequest() { amount = 999 }  }
                }
            });

        }
    }
    public sealed class ev_TextTest2 : IDbRecord
    {
        public ev_TextTest2()
        {
            ID("ev_TextTest2");

            // -------------------------------------------------- ID and TAG
            With<ID>(new ID { m_Value = "ev_TextTest2" });
            With<MapEvTextTag>(new MapEvTextTag { });

            // -------------------------------------------------- Required components
            With<MapEvCollumnRequirComponent>(new MapEvCollumnRequirComponent
            {
                count_start_from_zero = true,
                count_offset = 2,
                count_offset_percentile = 0.4f
            });



            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/Spr_Bodypart_Head_Test_1"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "Lets kill people together"
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, IRequestData>
                {

                    { "Yeah, sure",
                        new GiveGoldRequest() { amount = 10 }  },

                    { "What? No",
                        new TakeGoldRequest() { amount = 999999999 } },

                    { "�� ����",
                        new GiveGoldRequest() { amount = 10 }  }

                }
            });
        }
    }

}
