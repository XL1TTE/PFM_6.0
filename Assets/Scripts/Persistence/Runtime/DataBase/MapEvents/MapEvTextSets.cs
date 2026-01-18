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
                    { "This is a default option 1",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                                new GiveGoldRequest() { amount = 10 } },

                                {CHOICE_SCRIPT_TYPE.TAKE_GOLD,
                                new TakeGoldRequest() { amount = 1 } },

                                {CHOICE_SCRIPT_TYPE.GIVE_PARTS,
                                new GivePartsRequest() {
                                    parts_id_and_amount = new Dictionary<string, int>()
                                    {
                                        {"bp_pig-head",99},
                                        {"bp_cow-arm",99}
                                    }
                                }},

                                {CHOICE_SCRIPT_TYPE.GIVE_HP,
                                new GiveHPRequest() {
                                    use_percent = true,
                                    amount_percent = 0.2f,
                                    monster_count = 3
                                }},

                                {CHOICE_SCRIPT_TYPE.TAKE_HP,
                                new TakeHPRequest() {
                                    use_percent = false,
                                    amount_flat = 1,
                                    monster_count = 3
                                }}
                            },
                            res_text = "This is a default RESULT" 
                        }
                    },
                    { "This is a default option 2!!!!",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.SWAP_PARTS,
                                new SwapPartsRequest() { 
                                    parts_type_with_id = new Dictionary<BODYPART_SPECIFIED_TYPE, string>()
                                    {
                                        {BODYPART_SPECIFIED_TYPE.HEAD, "bp_pig-head" }
                                    }
                                }},

                                {CHOICE_SCRIPT_TYPE.SWAP_PARTS_BETWEEN,
                                new SwapPartsBetweenMonstersRequest() {
                                    parts_types = new List<BODYPART_SPECIFIED_TYPE>()
                                    {
                                        BODYPART_SPECIFIED_TYPE.RARM,BODYPART_SPECIFIED_TYPE.LARM
                                    }
                                }}
                            },
                            res_text = "This is a default RESULT 2!!!!"
                        }
                    }

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
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.SWAP_PARTS,
                                new SwapPartsRequest() {
                                    parts_type_with_id = new Dictionary<BODYPART_SPECIFIED_TYPE, string>()
                                    {
                                        {BODYPART_SPECIFIED_TYPE.HEAD, "bp_goat-head" },
                                        {BODYPART_SPECIFIED_TYPE.RLEG, "bp_rat-leg" },
                                        {BODYPART_SPECIFIED_TYPE.LLEG, "bp_raven-leg" },
                                    }
                                }}
                            },
                            res_text = "ev_TextFireRing_result1"
                        }
                    },
                    { "ev_TextFireRing_choice2",
                        new MapChoiceWrapper() {
                            request_type_data = null,
                            res_text = "ev_TextFireRing_result2"
                        }
                    },
                    { "ev_TextFireRing_choice3",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.TAKE_HP,
                                new TakeHPRequest() {
                                    use_percent = true,
                                    amount_percent = 0.1f,
                                    monster_count = 3
                                }}
                            },
                            res_text = "ev_TextFireRing_result3"
                        }
                    }
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
                            request_type_data = null,
                            res_text = "ev_TextBlackCurse_result1"
                        }
                    },
                    { "ev_TextBlackCurse_choice2",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.TAKE_HP,
                                new TakeHPRequest() {
                                    use_percent = true,
                                    amount_percent = 0.1f,
                                    monster_count = 3
                                }}
                            },
                            res_text = "ev_TextBlackCurse_result2"
                        }
                    },
                    { "ev_TextBlackCurse_choice3",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.TAKE_HP,
                                new TakeHPRequest() {
                                    use_percent = true,
                                    amount_percent = 0.1f,
                                    monster_count = 1
                                }},

                                {CHOICE_SCRIPT_TYPE.GIVE_HP,
                                new GiveHPRequest() {
                                    use_percent = true,
                                    amount_percent = 0.3f,
                                    monster_count = 3
                                }}
                            },
                            res_text = "ev_TextBlackCurse_result3"
                        }
                    }
                }
            });
        }
    }

    public sealed class ev_TextFlockOfCrows : IDbRecord
    {
        public ev_TextFlockOfCrows()
        {
            ID("ev_TextFlockOfCrows");
            //// -------------------------------------------------- ID and TAG
            With<MapEvTextTag>(new MapEvTextTag { });
            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/TextEvents_5"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "ev_TextFlockOfCrows_message"
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {

                    { "ev_TextFlockOfCrows_choice1",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.SWAP_PARTS,
                                new SwapPartsRequest() {
                                    parts_type_with_id = new Dictionary<BODYPART_SPECIFIED_TYPE, string>()
                                    {
                                        {BODYPART_SPECIFIED_TYPE.TORSO, "bp_raven-torso" },
                                        {BODYPART_SPECIFIED_TYPE.RLEG, "bp_raven-leg" },
                                        {BODYPART_SPECIFIED_TYPE.LLEG, "bp_raven-leg" },
                                        {BODYPART_SPECIFIED_TYPE.RARM, "bp_raven-arm" },
                                        {BODYPART_SPECIFIED_TYPE.LARM, "bp_raven-arm" }
                                    }
                                }}
                            },
                            res_text = "ev_TextFlockOfCrows_result1"
                        }
                    },
                    { "ev_TextFlockOfCrows_choice2",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.GIVE_PARTS,
                                new GivePartsRequest() {
                                    parts_id_and_amount = new Dictionary<string, int>()
                                    {
                                        {"bp_raven-torso",1},
                                        {"bp_raven-leg",2},
                                        {"bp_raven-arm",2},
                                    }
                                }}
                            },
                            res_text = "ev_TextFlockOfCrows_result2"
                        }
                    },
                    { "ev_TextFlockOfCrows_choice3",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.TAKE_HP,
                                new TakeHPRequest() {
                                    use_percent = false,
                                    amount_flat = 1,
                                    monster_count = 3
                                }}
                            },
                            res_text = "ev_TextFlockOfCrows_result3"
                        }
                    }
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
            With<MapEvUnavailableTag>(new MapEvUnavailableTag { });
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
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.SWAP_PARTS,
                                new SwapPartsRequest() {
                                    parts_type_with_id = new Dictionary<BODYPART_SPECIFIED_TYPE, string>()
                                    {
                                        {BODYPART_SPECIFIED_TYPE.TORSO, "bp_raven-torso" },
                                        {BODYPART_SPECIFIED_TYPE.RLEG, "bp_raven-leg" },
                                        {BODYPART_SPECIFIED_TYPE.LLEG, "bp_raven-leg" },
                                        {BODYPART_SPECIFIED_TYPE.RARM, "bp_raven-arm" },
                                        {BODYPART_SPECIFIED_TYPE.LARM, "bp_raven-arm" }
                                    }
                                }}
                            },
                            res_text = "ev_TextWheatField_result1"
                        }
                    },
                    { "ev_TextWheatField_choice2",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.GIVE_PARTS,
                                new GivePartsRequest() {
                                    parts_id_and_amount = new Dictionary<string, int>()
                                    {
                                        {"bp_raven-torso",1},
                                        {"bp_raven-leg",2},
                                        {"bp_raven-arm",2},
                                    }
                                }}
                            },
                            res_text = "ev_TextWheatField_result2"
                        }
                    }
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
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.SWAP_PARTS,
                                new SwapPartsRequest() {
                                    parts_type_with_id = new Dictionary<BODYPART_SPECIFIED_TYPE, string>()
                                    {
                                        {BODYPART_SPECIFIED_TYPE.HEAD, "bp_goat-head" }
                                    }
                                }}
                            },
                            res_text = "ev_TextGoatsHoof_result1"
                        }
                    },
                    { "ev_TextGoatsHoof_choice2",
                        new MapChoiceWrapper() {
                            request_type_data = null,
                            res_text = "ev_TextGoatsHoof_result2"
                        }
                    },
                    { "ev_TextGoatsHoof_choice3",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.SWAP_PARTS_BETWEEN,
                                new SwapPartsBetweenMonstersRequest() {
                                    parts_types = new List<BODYPART_SPECIFIED_TYPE>()
                                    {
                                        BODYPART_SPECIFIED_TYPE.HEAD
                                    }
                                }}
                            },
                            res_text = "ev_TextGoatsHoof_result3"
                        }
                    }
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
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                                new GiveGoldRequest() { amount = 20 } },

                            },
                            res_text = "ev_TextWishingWell_result1"
                        }
                    },
                    { "ev_TextWishingWell_choice2",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.GIVE_PARTS,
                                new GivePartsRequest() {
                                    parts_id_and_amount = new Dictionary<string, int>()
                                    {
                                        {"bp_pig-torso",1},
                                        {"bp_ladybug-arm",1},
                                        {"bp_cow-arm",1},
                                        {"bp_raccoon-leg",1},
                                        {"bp_cat-head",1},
                                        {"bp_bear-arm",1}
                                    }
                                }}
                            },
                            res_text = "ev_TextWishingWell_result2"
                        }
                    },
                    { "ev_TextWishingWell_choice3",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.GIVE_HP,
                                new GiveHPRequest() {
                                    use_percent = true,
                                    amount_percent = 0.5f,
                                    monster_count = 3
                                }}
                            },
                            res_text = "ev_TextWishingWell_result3"
                        }
                    }
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
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.TAKE_GOLD,
                                new TakeGoldRequest() { amount = 10 } },

                            },
                            res_text = "ev_TextStoneAtCrossroads_result1"
                        }
                    },
                    { "ev_TextStoneAtCrossroads_choice2",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.TAKE_HP,
                                new TakeHPRequest() {
                                    use_percent = false,
                                    amount_flat = 25,
                                    monster_count = 1
                                }},

                                {CHOICE_SCRIPT_TYPE.GIVE_HP,
                                new GiveHPRequest() {
                                    use_percent = false,
                                    amount_flat = 15,
                                    monster_count = 3
                                }}
                            },
                            res_text = "ev_TextStoneAtCrossroads_result2"
                        }
                    },
                    { "ev_TextStoneAtCrossroads_choice3",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                                new GiveGoldRequest() { amount = 10 } },

                            },
                            res_text = "ev_TextStoneAtCrossroads_result3"
                        }
                    }
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
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.TAKE_HP,
                                new TakeHPRequest() {
                                    use_percent = true,
                                    amount_percent = 0.5f,
                                    monster_count = 1
                                }},

                                {CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                                new GiveGoldRequest() { amount = 12 } }
                            },
                            res_text = "ev_TextBurningHope_result1"
                        }
                    },
                    { "ev_TextBurningHope_choice2",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                                new GiveGoldRequest() { amount = 6 } },


                                {CHOICE_SCRIPT_TYPE.GIVE_PARTS,
                                new GivePartsRequest() {
                                    parts_id_and_amount = new Dictionary<string, int>()
                                    {
                                        {"bp_pig-head",1},
                                        {"bp_pig-torso",2},
                                        {"bp_pig-leg",3},
                                        {"bp_cow-arm",1},
                                        {"bp_sheep-arm",2},
                                        {"bp_rooster-torso",1}
                                    }
                                }}
                            },
                            res_text = "ev_TextBurningHope_result2"
                        }
                    }
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
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.SWAP_PARTS_BETWEEN,
                                new SwapPartsBetweenMonstersRequest() {
                                    parts_types = new List<BODYPART_SPECIFIED_TYPE>()
                                    {
                                        BODYPART_SPECIFIED_TYPE.TORSO,
                                        BODYPART_SPECIFIED_TYPE.LARM,
                                        BODYPART_SPECIFIED_TYPE.LLEG
                                    }
                                }},
                            },
                            res_text = "ev_TextCollapsedChurch_result1"
                        }
                    },
                    { "ev_TextCollapsedChurch_choice2",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.GIVE_HP,
                                new GiveHPRequest() {
                                    use_percent = true,
                                    amount_percent = 0.33f,
                                    monster_count = 3
                                }},
                            },
                            res_text = "ev_TextCollapsedChurch_result2"
                        }
                    },
                    { "ev_TextCollapsedChurch_choice3",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                                new GiveGoldRequest() { amount = 12 } },


                                {CHOICE_SCRIPT_TYPE.TAKE_HP,
                                new TakeHPRequest() {
                                    use_percent = true,
                                    amount_percent = 0.33f,
                                    monster_count = 1
                                }}
                            },
                            res_text = "ev_TextCollapsedChurch_result3"
                        }
                    }
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
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.GIVE_HP,
                                new GiveHPRequest() {
                                    use_percent = true,
                                    amount_percent = 1f,
                                    monster_count = 3
                                }}
                            },
                            res_text = "ev_TextGiantCow_result1"
                        }
                    },
                    { "ev_TextGiantCow_choice2",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.SWAP_PARTS,
                                new SwapPartsRequest() {
                                    parts_type_with_id = new Dictionary<BODYPART_SPECIFIED_TYPE, string>()
                                    {
                                        {BODYPART_SPECIFIED_TYPE.LARM, "bp_cow-arm" }
                                    }
                                }},
                                {CHOICE_SCRIPT_TYPE.TAKE_HP,
                                new TakeHPRequest() {
                                    use_percent = false,
                                    amount_flat = 5,
                                    monster_count = 3
                                }}
                            },
                            res_text = "ev_TextGiantCow_result2"
                        }
                    },
                    { "ev_TextGiantCow_choice3",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                                new GiveGoldRequest() { amount = 7 } },

                                {CHOICE_SCRIPT_TYPE.GIVE_PARTS,
                                new GivePartsRequest() {
                                    parts_id_and_amount = new Dictionary<string, int>()
                                    {
                                        {"bp_horse-leg",1},
                                        {"bp_goose-head",1},
                                        {"bp_bee-arm",1},
                                        {"bp_bear-leg",1},
                                        {"bp_dove-arm",1}
                                    }
                                }}
                            },
                            res_text = "ev_TextGiantCow_result3"
                        }
                    }
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
                            request_type_data = null,
                            res_text = "ev_TextHorse_result1"
                        }
                    },
                    { "ev_TextHorse_choice2",
                        new MapChoiceWrapper() {
                            request_type_data = null,
                            res_text = "ev_TextHorse_result2"
                        }
                    },
                    { "ev_TextHorse_choice3",
                        new MapChoiceWrapper() {
                            request_type_data = null,
                            res_text = "ev_TextHorse_result3"
                        }
                    },
                    { "ev_TextHorse_choice4",
                        new MapChoiceWrapper() {
                            request_type_data = null,
                            res_text = "ev_TextHorse_result4"
                        }
                    }
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
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.GIVE_PARTS,
                                new GivePartsRequest() {
                                    parts_id_and_amount = new Dictionary<string, int>()
                                    {
                                        {"bp_cockroach-arm",1},
                                        {"bp_cat-arm",1},
                                        {"bp_dog-torso",1},
                                        {"bp_goat-leg",2},
                                        {"bp_raccoon-torso",1},
                                        {"bp_raccoon-leg",2},
                                        {"bp_raccoon-arm",2}
                                    }
                                }}
                            },
                            res_text = "ev_TextCartOfProvisions_result1"
                        }
                    },
                    { "ev_TextCartOfProvisions_choice2",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                                new GiveGoldRequest() { amount = 13 } },

                                {CHOICE_SCRIPT_TYPE.GIVE_PARTS,
                                new GivePartsRequest() {
                                    parts_id_and_amount = new Dictionary<string, int>()
                                    {
                                        {"bp_rat-arm",1},
                                        {"bp_rat-leg",1},
                                        {"bp_sheep-head",1}
                                    }
                                }}
                            },
                            res_text = "ev_TextCartOfProvisions_result2"
                        }
                    }
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
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.GIVE_HP,
                                new GiveHPRequest() {
                                    use_percent = true,
                                    amount_percent = 1f,
                                    monster_count = 1
                                }},
                            },
                            res_text = "ev_TextBlushingAppleTree_result1"
                        }
                    },
                    { "ev_TextBlushingAppleTree_choice2",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.TAKE_HP,
                                new TakeHPRequest() {
                                    use_percent = true,
                                    amount_percent = 0.15f,
                                    monster_count = 3
                                }}
                            },
                            res_text = "ev_TextBlushingAppleTree_result2"
                        }
                    },
                    { "ev_TextBlushingAppleTree_choice3",
                        new MapChoiceWrapper() {
                            request_type_data = new Dictionary<CHOICE_SCRIPT_TYPE, Scellecs.Morpeh.IRequestData>()
                            {
                                {CHOICE_SCRIPT_TYPE.SWAP_PARTS_BETWEEN,
                                new SwapPartsBetweenMonstersRequest() {
                                    parts_types = new List<BODYPART_SPECIFIED_TYPE>()
                                    {
                                        BODYPART_SPECIFIED_TYPE.LLEG,
                                        BODYPART_SPECIFIED_TYPE.HEAD,
                                        BODYPART_SPECIFIED_TYPE.RARM,
                                        BODYPART_SPECIFIED_TYPE.TORSO
                                    }
                                }}
                            },
                            res_text = "ev_TextBlushingAppleTree_result3"
                        }
                    }
                }
            });
        }
    }

    /*
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
    */



}
