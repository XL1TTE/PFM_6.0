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
                bg_sprite_path = "Map/MapTextEventBGs/-0"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "The Fire-Ring Dance" +
                "\r\nThe embers glow, the shadows leap," +
                "\r\nFrom hollow, burning villages so deep." +
                "\r\nA rat, a goat, a raven's call," +
                "\r\nThey dance in rings around the pall." +
                "\r\nThey beckon with a twisted grace," +
                "\r\nA sly and knowing smile on each face." +
                "\r\n\"\"Oh, weary traveler, lost and cold," +
                "\r\nCome join our tale that never shall be told.\"\"\""
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {

                    { "Join the dance",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 10 },
                            res_text = "THIS IS SOME RESULT" }  },

                    { "Watch",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.TAKE_GOLD,
                            request = new TakeGoldRequest() { amount = 999999999 },
                            res_text = "THIS IS SOME RESULT" }},

                    { "Run away",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 10 },
                            res_text = "THIS IS SOME RESULT" } }

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
                bg_sprite_path = "Map/MapTextEventBGs/-0"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "The Black Curse" +
                "\nThe black cat ran, an omen fell," +
                "\nA woven curse, a ancient spell." +
                "\nBeware the path it chose to cross," +
                "\nBeware its shadow, and your loss."
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ADD OPTIONS",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ADD RESULT" }  }
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
                bg_sprite_path = "Map/MapTextEventBGs/-0"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "The Flock of Crows" +
                "\nOn darkened bought, a raven's cry," +
                "\nBeneath a waning moon's cold eye." +
                "\nTheir calls weave spells in ancient tongue," +
                "\nOf stories old, yet ever young."
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ADD OPTIONS",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ADD RESULT" }  }
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
                bg_sprite_path = "Map/MapTextEventBGs/-0"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "The Wheat Field" +
                "\nThe wheat parts where no wind blows," +
                "\nA heavy breath, a whispered sigh." +
                "\nA shadow where no moon can lie," +
                "\nThe ancient stalk in terror grows." +
                "\nI dare not ask, and no one knows."
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ADD OPTIONS",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ADD RESULT" }  }
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
                bg_sprite_path = "Map/MapTextEventBGs/-0"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "The Goat's Hoof" +
                "\nThis water, dark and cold and deep," +
                "\nHas secrets that the stones will keep." +
                "\nA price is owed, a vow to bind," +
                "\nTo quench the thirst of humankind."
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ADD OPTIONS",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ADD RESULT" }  }
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
                bg_sprite_path = "Map/MapTextEventBGs/-0"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "The Wishing Well" +
                "\nA well of ancient, mossy stone," +
                "\nI cast my coin and make a plea." +
                "\nA whisper from the depths, a moan," +
                "\n\"The price is paid,\" it answered me." +
                "\n\"Your wish is granted, flesh and bone," +
                "\nBut what you gain, was never free.\""
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ADD OPTIONS",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ADD RESULT" }  }
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
                bg_sprite_path = "Map/MapTextEventBGs/-0"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "The Stone at the Crossroads" +
                "\nWhere three ways meet, a marker stands," +
                "\nIt speaks in sighs, not with hands." +
                "\nThe left road steals what you hold dear," +
                "\nAnd drowns your past in silent fear." +
                "\nThe straight road saps your life, your health," +
                "\nBut grants you knowledge, arcane wealth." +
                "\nTo right, a clink of coins untold," +
                "\nA heavy purse, a heart of cold."
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ADD OPTIONS",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ADD RESULT" }  }
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
                bg_sprite_path = "Map/MapTextEventBGs/-0"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "The Burning Hope" +
                "\nA crossroads stands in pale moonlight," +
                "\nA burning house, a piercing fright." +
                "\nThe flames whisper low, a tempting plea," +
                "\n\"All that they owned, for you, is free.\"" +
                "\nBut shadows writhe in smoky air," +
                "\nWith hollow voices of despair."
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ADD OPTIONS",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ADD RESULT" }  }
                }
            });
        }
    }

    public sealed class ev_TextGatheringStorm : IDbRecord
    {
        public ev_TextGatheringStorm()
        {
            ID("ev_TextGatheringStorm");
            //// -------------------------------------------------- ID and TAG
            With<MapEvTextTag>(new MapEvTextTag { });
            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/-0"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "The Gathering Storm" +
                "\nThe path ahead was blocked by torch and steel," +
                "\nBy faces twisted in a righteous rage." +
                "\nA shadow fell, and smoke began to kneel," +
                "\nAs I began to read the burning page." +
                "\nTheir angry cries transformed into a chant," +
                "\nTo feed the ancient hunger that they plant."
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ADD OPTIONS",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ADD RESULT" }  }
                }
            });
        }
    }

    public sealed class ev_TextSmellOfDeath : IDbRecord
    {
        public ev_TextSmellOfDeath()
        {
            ID("ev_TextSmellOfDeath");
            //// -------------------------------------------------- ID and TAG
            With<MapEvTextTag>(new MapEvTextTag { });
            // -------------------------------------------------- Main information
            With<MapEvTextBGComponent>(new MapEvTextBGComponent
            {
                bg_sprite_path = "Map/MapTextEventBGs/-0"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "The Smell of Death" +
                "\nThe stones are worn, the names are faint to see," +
                "\nA whispered truth from ages long ago." +
                "\nThey do not speak of death, but prophecy," +
                "\nOf seasons turning, fast and also slow." +
                "\nI trace the letters with a careful hand," +
                "\nTo feel the future sleeping in the land."
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ADD OPTIONS",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ADD RESULT" }  }
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
                bg_sprite_path = "Map/MapTextEventBGs/-0"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "The Horse" +
                "\nIt stands so still, the smoke itself holds breath," +
                "\nIts gaze a needle, steady, dark, and deep." +
                "\nIt knows the hour of your life and death," +
                "\nA secret it is neither yours to keep."
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ADD OPTIONS",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ADD RESULT" }  }
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
                bg_sprite_path = "Map/MapTextEventBGs/-0"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "The Cart of Provisions" +
                "\nThe cart is full, yet casts no shadow there," +
                "\nWhere embers dance upon the sooty air." +
                "\nIt offers more than simple mortal bread," +
                "\nTo fill the living, or to wake the dead."
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ADD OPTIONS",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ADD RESULT" }  }
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
                bg_sprite_path = "Map/MapTextEventBGs/-0"
            });
            With<MapEvTextMessageComponent>(new MapEvTextMessageComponent
            {
                string_message = "The Blushing Apple Tree" +
                "\nAmidst the crackling, hungry glow," +
                "\nUnscorched, a tree begins to grow." +
                "\nIts fruit, a promise, ripe and red," +
                "\nTo taste is to forget the dead."
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "ADD OPTIONS",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "ADD RESULT" }  }
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
