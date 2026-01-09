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
            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });
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
    public sealed class ev_TextFireRing : IDbRecord
    {
        public ev_TextFireRing()
        {
            ID("ev_TextFireRing");

            // -------------------------------------------------- ID and TAG
            //With<ID>(new ID { m_Value = "ev_TextFireRing" });
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
                bg_sprite_path = "Map/MapTextEventBGs/TextEvents_0"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "ev_TextFireRing_message"
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {

                    { "ev_TextFireRing_choice1",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 10 },
                            res_text = "ev_TextFireRing_result1" } },

                    { "ev_TextFireRing_choice2",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.TAKE_GOLD,
                            request = new TakeGoldRequest() { amount = 999999999 },
                            res_text = "ev_TextFireRing_result2" } },

                    { "ev_TextFireRing_choice3",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 10 },
                            res_text = "ev_TextFireRing_result3" } }

                }
            });
        }
    }

    public sealed class ev_TextBlackCurse : IDbRecord
    {
        public ev_TextBlackCurse()
        {
            ID("ev_TextBlackCurse");
            //// -------------------------------------------------- ID and TAG
            With<MapEvTextTag>(new MapEvTextTag { });
            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/TextEvents_7"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "ev_TextBlackCurse_message"
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ev_TextBlackCurse_choice1",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextBlackCurse_result1" }  },
                    { "ev_TextBlackCurse_choice2",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextBlackCurse_result2" }  },
                     { "ev_TextBlackCurse_choice3",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextBlackCurse_result3" }  }
                }
            });
        }
    }

    public sealed class ev_TextBlockOfCrows : IDbRecord
    {
        public ev_TextBlockOfCrows()
        {
            ID("ev_TextBlockOfCrows");
            //// -------------------------------------------------- ID and TAG
            With<MapEvTextTag>(new MapEvTextTag { });
            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/TextEvents_5"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "ev_TextBlockOfCrows_message"
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ev_TextBlockOfCrows_choice1",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextBlockOfCrows_result1" }  },
                    { "ev_TextBlockOfCrows_choice2",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextBlockOfCrows_result2" }  },
                    { "ev_TextBlockOfCrows_choice3",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextBlockOfCrows_result3" }  }
                }
            });
        }
    }

    public sealed class ev_TextWheatField : IDbRecord
    {
        public ev_TextWheatField()
        {
            ID("ev_TextWheatField");
            //// -------------------------------------------------- ID and TAG
            With<MapEvTextTag>(new MapEvTextTag { });
            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/TextEvents_3"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "ev_TextWheatField_message"
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ev_TextWheatField_choice1",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextWheatField_result1" }  },
                    { "ev_TextWheatField_choice2",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextWheatField_result2" }  }
                }
            });
        }
    }

    public sealed class ev_TextGoatsHoof : IDbRecord
    {
        public ev_TextGoatsHoof()
        {
            ID("ev_TextGoatsHoof");
            //// -------------------------------------------------- ID and TAG
            With<MapEvTextTag>(new MapEvTextTag { });
            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/TextEvents_4"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "ev_TextGoatsHoof_message"
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ev_TextGoatsHoof_choice1",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextGoatsHoof_result1" }  },
                    { "ev_TextGoatsHoof_choice2",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextGoatsHoof_result2" }  },
                    { "ev_TextGoatsHoof_choice3",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextGoatsHoof_result3" }  }
                }
            });
        }
    }

    public sealed class ev_TextWishingWell : IDbRecord
    {
        public ev_TextWishingWell()
        {
            ID("ev_TextWishingWell");
            //// -------------------------------------------------- ID and TAG
            With<MapEvTextTag>(new MapEvTextTag { });
            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/TextEvents_9"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "ev_TextWishingWell_message"
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ev_TextWishingWell_choice1",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextWishingWell_result1" }  },
                    { "ev_TextWishingWell_choice2",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextWishingWell_result2" }  },
                    { "ev_TextWishingWell_choice3",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextWishingWell_result3" }  }
                }
            });
        }
    }

    public sealed class ev_TextStoneAtCrossroads : IDbRecord
    {
        public ev_TextStoneAtCrossroads()
        {
            ID("ev_TextStoneAtCrossroads");
            //// -------------------------------------------------- ID and TAG
            With<MapEvTextTag>(new MapEvTextTag { });
            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/TextEvents_8"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "ev_TextStoneAtCrossroads_message"
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ev_TextStoneAtCrossroads_choice1",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextStoneAtCrossroads_result1" }  },
                     { "ev_TextStoneAtCrossroads_choice2",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextStoneAtCrossroads_result2" }  },
                      { "ev_TextStoneAtCrossroads_choice3",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextStoneAtCrossroads_result3" }  }
                }
            });
        }
    }

    public sealed class ev_TextBurningHope : IDbRecord
    {
        public ev_TextBurningHope()
        {
            ID("ev_TextBurningHope");
            //// -------------------------------------------------- ID and TAG
            With<MapEvTextTag>(new MapEvTextTag { });
            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/TextEvents_10"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "ev_TextBurningHope_message"
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ev_TextBurningHope_choice1",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextBurningHope_result1" }  },
                    { "ev_TextBurningHope_choice2",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextBurningHope_result2" }  }
                }
            });
        }
    }

    public sealed class ev_TextCollapsedChurch : IDbRecord
    {
        public ev_TextCollapsedChurch()
        {
            ID("ev_TextCollapsedChurch");
            //// -------------------------------------------------- ID and TAG
            With<MapEvTextTag>(new MapEvTextTag { });
            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/TextEvents_11"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "ev_TextCollapsedChurch_message"
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ev_TextCollapsedChurch_choice1",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextCollapsedChurch_result1" }  },
                    { "ev_TextCollapsedChurch_choice2",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextCollapsedChurch_result2" }  },
                    { "ev_TextCollapsedChurch_choice3",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextCollapsedChurch_result3" }  }
                }
            });
        }
    }

    public sealed class ev_TextGiantCow : IDbRecord
    {
        public ev_TextGiantCow()
        {
            ID("ev_TextGiantCow");
            //// -------------------------------------------------- ID and TAG
            With<MapEvTextTag>(new MapEvTextTag { });
            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/TextEvents_12"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "ev_TextGiantCow_message"
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ev_TextGiantCow_choice1",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextGiantCow_result1" }  },
                     { "ev_TextGiantCow_choice2",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextGiantCow_result2" }  },
                      { "ev_TextGiantCow_choice3",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextGiantCow_result3" }  }
                }
            });
        }
    }

    public sealed class ev_TextHorse : IDbRecord
    {
        public ev_TextHorse()
        {
            ID("ev_TextHorse");
            //// -------------------------------------------------- ID and TAG
            With<MapEvTextTag>(new MapEvTextTag { });
            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/TextEvents_2"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "ev_TextHorse_message"
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ev_TextHorse_choice1",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextHorse_result1" }  },
                    { "ev_TextHorse_choice2",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextHorse_result2" }  },
                    { "ev_TextHorse_choice3",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextHorse_result3" }  },
                    { "ev_TextHorse_choice4",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextHorse_result4" }  }
                }
            });
        }
    }

    public sealed class ev_TextCartOfProvisions : IDbRecord
    {
        public ev_TextCartOfProvisions()
        {
            ID("ev_TextCartOfProvisions");
            //// -------------------------------------------------- ID and TAG
            With<MapEvTextTag>(new MapEvTextTag { });
            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/TextEvents_6"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "ev_TextCartOfProvisions_message"
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ev_TextCartOfProvisions_choice1",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextCartOfProvisions_result1" }  },
                     { "ev_TextCartOfProvisions_choice2",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextCartOfProvisions_result2" }  },
                     { "ev_TextCartOfProvisions_choice3",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextCartOfProvisions_result3" }  }
                }
            });
        }
    }

    public sealed class ev_TextBlushingAppleTree : IDbRecord
    {
        public ev_TextBlushingAppleTree()
        {
            ID("ev_TextBlushingAppleTree");
            //// -------------------------------------------------- ID and TAG
            With<MapEvTextTag>(new MapEvTextTag { });
            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/TextEvents_1"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "ev_TextBlushingAppleTree_message"
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ev_TextBlushingAppleTree_choice1",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextBlushingAppleTree_result1" }  },
                     { "ev_TextBlushingAppleTree_choice2",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextBlushingAppleTree_result2" }  },
                     { "ev_TextBlushingAppleTree_choice3",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ev_TextBlushingAppleTree_result3" }  }
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
            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });
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
                            res_text = "THIS IS SOME RESULT" }  },

                    { "Accept the ancient scroll",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.TAKE_GOLD,
                            request = new TakeGoldRequest() { amount = 69 },
                            res_text = "THIS IS SOME RESULT" }},
                    {"Ask for safe passage",
                    new MapChoiceWrapper() {
                        type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                        request = new GiveGoldRequest() { amount = 10 },
                            res_text = "THIS IS SOME RESULT" } }}
            });
        }
    }

}
