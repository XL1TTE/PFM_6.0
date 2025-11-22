using Domain.Components;
using Domain.Map.Mono;
using Domain.MapEvents.Requests;
using System.Collections.Generic;

namespace Persistence.DB
{

    //public sealed class ev_FAILSAFE : IDbRecord
    //{
    //    public ev_FAILSAFE()
    //    {
    //        ID("ev_FAILSAFE");

    //        // -------------------------------------------------- ID and TAG
    //       // With<ID>(new ID { m_Value = "ev_FAILSAFE" });
    //        With<MapEvTextTag>(new MapEvTextTag { });

    //        // -------------------------------------------------- Required components



    //        // -------------------------------------------------- Main information
    //        With<MapEvTextBGComponent>(new MapEvTextBGComponent
    //        {
    //            bg_sprite_path = "Map/MapTextEventBGs/Spr_Bodypart_Head_Test_1"
    //        });
    //        With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
    //        {
    //            string_message = "IF YOU SEE THIS, THEN SOMETHING WENT WRONG"
    //        });
    //        With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
    //        {
    //            choices = new Dictionary<string, MapChoiceWrapper>
    //            {

    //                { "А, блин :(",
    //                    new MapChoiceWrapper() {
    //                        type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
    //                        request = new GiveGoldRequest() { amount = 10 } }  }

    //            }
    //        });
    //    }
    //}

    public sealed class ev_TextDefault : IDbRecord
    {
        public ev_TextDefault()
        {
            ID("ev_TextDefault");

            // -------------------------------------------------- ID and TAG
            //With<ID>(new ID { m_Value = "ev_TextDefault" });
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
                choices = new Dictionary<string, MapChoiceWrapper>
                {

                    { "This is a default option",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 10 },
                            res_text = "This is a default RESULT" } }

                }
            });
        }
    }
    // public sealed class ev_TextTest1 : IDbRecord
    // {
    //     public ev_TextTest1()
    //     {

    //         ID("ev_TextTest1");

    //         // -------------------------------------------------- ID and TAG
    //         // With<ID>(new ID { m_Value = "ev_TextTest1" });
    //         With<MapEvTextTag>(new MapEvTextTag { });


    //         // -------------------------------------------------- Required components
    //         With<MapEvStageRequirComponent>(new MapEvStageRequirComponent
    //         {
    //             acceptable_stages = new System.Collections.Generic.List<STAGES> {
    //                 STAGES.VILLAGE
    //             }
    //         });
    //         With<MapEvCollumnRequirComponent>(new MapEvCollumnRequirComponent
    //         {
    //             count_start_from_zero = false,
    //             count_offset = 1,
    //             count_offset_percentile = 0.2f,
    //         });


    //         // -------------------------------------------------- Main information
    //         With<MapEvTextBGComponent>(new MapEvTextBGComponent
    //         {
    //             bg_sprite_path = "Map/MapTextEventBGs/Spr_Bodypart_Head_Test_1"
    //         });
    //         With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
    //         {
    //             string_message = ""
    //         });

    //         //With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent /// BUG HERE
    //         //{
    //         //    choices = {
    //         //
    //         //        { "SCHEISSE!",
    //         //            "Map/MapTextEventResultScripts/Scr_MapEvResult_Test1" },
    //         //        
    //         //        {"NO CHOICE!",
    //         //            "Map/MapTextEventResultScripts/Scr_MapEvResult_Test1" },
    //         //        
    //         //        {"���� ���� ^_^",
    //         //            "Map/MapTextEventResultScripts/Scr_MapEvResult_Test1" },
    //         //    }
    //         //});


    //         With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
    //         {
    //             //choices = new Dictionary<string, ScriptClass>
    //             choices = new Dictionary<string, MapChoiceWrapper>
    //             {
    //                 //{ "SCHEISSE!",
    //                 //    GiveItems{ id1,id2,id3 } },

    //                 { "SCHEISSE!",
    //                     new MapChoiceWrapper() {
    //                         type = CHOICE_SCRIPT_TYPE.TAKE_GOLD,
    //                         request = new TakeGoldRequest() { amount = 10 } }   },

    //                 {"NO CHOICE!",
    //                     new MapChoiceWrapper() {
    //                         type = CHOICE_SCRIPT_TYPE.TAKE_GOLD,
    //                         request = new TakeGoldRequest() { amount = 100 } } },
    //             }
    //         });

    //     }
    // }
    public sealed class ev_TextTest2 : IDbRecord
    {
        public ev_TextTest2()
        {
            ID("ev_TextTest2");

            // -------------------------------------------------- ID and TAG
            //With<ID>(new ID { m_Value = "ev_TextTest2" });
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
                bg_sprite_path = "Map/MapTextEventBGs/-0"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "The Fire-Ring Dance\r\nThe embers glow, the shadows leap,\r\nFrom hollow, burning villages so deep.\r\nA rat, a goat, a raven's call,\r\nThey dance in rings around the pall.\r\nThey beckon with a twisted grace,\r\nA sly and knowing smile on each face.\r\n\"\"Oh, weary traveler, lost and cold,\r\nCome join our tale that never shall be told.\"\"\""
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {

                    { "Join the dance",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 10 },
                            res_text = "ЭТО РЕЗУЛЬТАТ" }  },

                    { "Watch",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.TAKE_GOLD,
                            request = new TakeGoldRequest() { amount = 999999999 },
                            res_text = "ЭТО РЕЗУЛЬТАТ" }},

                    { "Run away",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 10 },
                            res_text = "ЭТО РЕЗУЛЬТАТ" } }

                }
            });
        }
    }
    public sealed class ev_TextTest3 : IDbRecord
    {
        public ev_TextTest3()
        {
            ID("ev_TextTest3");

            //// -------------------------------------------------- ID and TAG
            //With<ID>(new ID { m_Value = "ev_TextTest3" });
            With<MapEvTextTag>(new MapEvTextTag { });



            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/-0"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "The Shadowed Wanderer\nAt the crossroads stands a figure cloaked in twilight,\nhis eyes holding the wisdom of forgotten ages.\n\"\"I offer you three paths,\"\" he whispers, his voice like rustling leaves.\n\"\"One leads to fortune, one to knowledge, one to freedom.\nBut choose wisely - every gift has its price.\"\""
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {

                    { "Take the golden coin",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ЭТО РЕЗУЛЬТАТ" }  },

                    { "Accept the ancient scroll",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.TAKE_GOLD,
                            request = new TakeGoldRequest() { amount = 69 },
                            res_text = "ЭТО РЕЗУЛЬТАТ" }},
                    {"Ask for safe passage",
                    new MapChoiceWrapper() {
                        type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                        request = new GiveGoldRequest() { amount = 10 },
                            res_text = "ЭТО РЕЗУЛЬТАТ" } }}
            });
        }
    }

}
