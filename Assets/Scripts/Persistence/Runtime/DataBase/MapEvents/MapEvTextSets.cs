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
                string_message = "The Fire-Ring Dance" +
                "\r\nThe embers glow, the shadows leap," +
                "\r\nFrom hollow, burning villages so deep." +
                "\r\nA rat, a goat, a raven's call," +
                "\r\nThey dance in rings around the pall." +
                "\r\nThey beckon with a twisted grace," +
                "\r\nA sly and knowing smile on each face." +
                "\r\n\"Oh, weary traveler, lost and cold," +
                "\r\nCome join our tale that never shall be told.\"\""
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {

                    { "Join the dance",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 10 },
                            res_text = "You step into the circle and are swept into the whirling dance, feeling the ancient, wild rhythm claim your limbs and muddle your thoughts." }  },

                    { "Watch",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.TAKE_GOLD,
                            request = new TakeGoldRequest() { amount = 999999999 },
                            res_text = "You remain a hidden observer, watching the hypnotic and disturbing patterns of the ritual until the dancers and embers dissolve into the mist." }},

                    { "Run away",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 10 },
                            res_text = "You turn and run from the ominous circle, the sound of their mocking laughter echoing in your ears long after the firelight has died down." } }

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
                    { "Spit over your left shoulder and knock on wood three times",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You spit over your shoulder and knock on wood, and the oppressive feeling lifts as the omen passes you by without effect" }  },
                    { "Ignore superstitions and continue on your way",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You ignore the superstition and continue on, immediately feeling a cold, clinging shadow latch onto your soul" }  },
                     { "Tear off a piece of your monster and give it to the cat",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You offer a piece of your monster to the cat, which accepts it and bestows upon you a fleeting but potent blessing" }  }
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
                    { "Croak back",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You croak back, and the crows fall silent for a moment before answering in a chorus that feels like a cryptic, approving welcome" }  },
                    { "Throw something at them",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You throw something at them and the flock explodes in a furious storm of wings and sharp beaks, forcing them to flee wounded." }  },
                    { "Brush it off and go around",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You brush it off and walk away, but the feeling of their watchful, all-knowing eyes stays with you for a long time." }  }
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
                    { "Find out what's lurking in the field",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "As you push through the stalks, you continually encounter the torn carcasses of small animals. The further you go, the more of them there are. In the middle of the field, you see the shapeless silhouette of a creature, which suddenly disappears into the darkness." }  },
                    { "Set the field on fire",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You set the field ablaze, and a terrible, piercing shriek rises from the flames as the unseen thing within is consumed" }  }
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
                    { "One monster drinks from a puddle",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "Your monster drinks and becomes filled with a strange, cold energy, its shape gradually changing, becoming more goat-like." }  },
                    { "Spit in a puddle",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You spit into a puddle and the water instantly becomes dirty and stagnant, its hidden power now destroyed and inert." }  },
                    { "Look into a puddle",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "Looking into the puddle, you see yourself covered in blood. You feel guilty." }  }
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
                    { "Make a wish for money",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You wish for money and your pockets grow heavy with cold, unfamiliar coins that feel somehow tainted" }  },
                    { "Make a wish for pieces of meat",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You wish for pieces of meat and receive body parts" }  },
                    { "Make a wish for a blessing",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You wish for a blessing and your monsters become stronger" }  }
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
                    { "Go left",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You go left and feel a profound loss, as cherished memories of your past begin to fade into a silent fog" }  },
                     { "Go straight",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You go straight and a wave of weakness washes over you, but your mind is suddenly flooded with cryptic, arcane symbols" }  },
                      { "Go right",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You go right and find a heavy purse of gold, but a deep chill settles in your chest, numbing your capacity for joy." }  }
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
                    { "Jump into the fire and help",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You jump into a fire and manage to save a man. He thanks you for saving him and gives you money." }  },
                    { "Wait until the fire dies down and search the dead",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You wait for the fire to die down and, sifting through the ashes, find only blackened bones." }  }
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
                string_message = "The Collapsed Church" +
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
                    { "Ring the bell",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You ring the bell, and its pure, clear sound dispels hostile shadows." }  },
                    { "Pray for your sins",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You pray for your sins, and a heavy weight lifts from your soul, but you feel a keen, observant presence now watching you." }  },
                    { "Rob",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You rob a ruined church, but you feel the weight of that money" }  }
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
                string_message = "The Giant of Cow" +
                "\nTThe earth has claimed its gentle child," +
                "\nWhose breath was soft, whose nature mild." +
                "\nNo fault of yours, in dew and hay," +
                "\nTo meet the ending of the day."
            });
            With<MapEvTextChoicesComponent>(new MapEvTextChoicesComponent
            {
                choices = new Dictionary<string, MapChoiceWrapper>
                {
                    { "Bow down",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You bow in respect, and a sense of the creature's gentle spirit passes through you, leaving a feeling of tranquil acceptance." }  },
                     { "Eat",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You consume a piece of the remains, and a surge of raw, earthy strength fortifies your body, making you feel deeply rooted and resilient." }  },
                      { "Research",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You examine the body closely and discover strange, symbiotic fungi growing upon it, suggesting a nature that was far from mundane.р" }  }
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
                    { "Watch into the eyes",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You look into its eyes." }  },
                    { "Watch into the eyes.",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You look into its eyes." }  },
                    { "Watch into the eyes..",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You look into its eyes." }  },
                    { "Watch into the eyes...",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You look into its eyes." }  }
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
                    { "Search",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You search the cart and find loaves of dense, dark bread that seem to pulse with a faint, sustaining energy.\r\n\r\n" }  },
                     { "Kick",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You kick the cart, and it collapses into a pile of dust and splinters, releasing a mournful wail that fades on the wind." }  },
                     { "Turn upside down",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You turn the cart upside down, and from it falls a single, perfectly preserved but utterly inedible stone fruit." }  }
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
                    { "Take one apple",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "You take a single apple and your monster eats it, seeming content and momentarily at peace" }  },
                     { "Take all the apples",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "In your greed, you strip the tree bare, and the apples instantly rot in your hands, filling the air with a sickly-sweet stench of decay" }  },
                     { "Touch the tree",
                        new MapChoiceWrapper() {
                            type = CHOICE_SCRIPT_TYPE.GIVE_GOLD,
                            request = new GiveGoldRequest() { amount = 67 },
                            res_text = "Your fingers brush the bark, and a wave of profound, sorrowful memories from the forgotten dead floods your mind" }  }
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
