using AscendedZ.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    /// <summary>
    /// A collection of dialog scenarios to be shown during cutscenes.
    /// </summary>
    public class DialogScenes
    {
        /// <summary>
        /// This is the opening cutscene of the game.
        /// </summary>
        public static string[] Opening = new string[] 
        {
            "Once upon a buce, there was a labyrinth known\nas The Endless Dungeon.",
            "Legend has it that contestants from\naround the world came together to\nconquer it.",
            "Decades passed by, and no one seemed\nup to the challenge.",
            "Until, one day, it happened...\nThe Endless Dungeon, right before\neveryone's eyes...",
            "...Completely disappeared. Someone had\nmanaged to conquer it once and for all.",
            "The opportunity to obtain the ultimate\npower of Ascended Enlightenment was\ngone... but not for long.",
            "Hundreds of years later, a new dungeon\nspawned in its place.",
            "It's said that this one is tougher than\nthe last, but the reward is\ngreater than ever before.",
            "You are a villager who has decided to\ntake on the challenge.",
            "Will you fail like all the others who've\ncome before you, or will you do the\nimpossible?",
            "Do you have what it takes to Ascend?"
        };

        public static Dictionary<string, string[]> DCBossDialog = new Dictionary<string, string[]>()
        {
            {
                EnemyNames.Ocura, new string[]
                {
                    "Bah har har har har.",
                    "In my flummers and wiggums as the\nruler of this bucetal sector...",
                    "Yet have I to experience a contestant\nwho has managed to make it thus far.",
                    "Strong as you are, and flippital as\nyou might be...",
                    "You will not reach the top of the\nLabrybuce.",
                    "For I, Ocura, will stand opposed\nto all those who wish to ASCEND!!!",
                }
            },
            {
                EnemyNames.Emush, new string[]
                {
                    "Your tolerance for suffering is admirable,\nyoung Ascended.",
                    "But, I can see it in your eyes.\nYou know not the meaning of this\nquest.",
                    "Thou ought to turn back for the\ntruth that layeth deepest in these\ncaverns is not for the unbuced.",
                    "Of course, being a contestant\nyou thirst for power regardless.",
                    "Shouldst thou not hearst my warnings,\nI will satiate thine need for\nanguish.",
                    "I am Emush. Have at thee when ready.",
                }
            },
            {
                EnemyNames.Logvat, new string[]
                {
                    "NOW THIS IS A SIGHT TO SEE!",
                    "MOST PEOPLE GIVE UP BEFORE\nREACHING THIS FLOOR AND LOSING\nBY MY HANDS!",
                    "OUT OF RESPECT FOR YOUR PERSERVERANCE\nI WILL GIVE YOU A BIT OF A HISTORY LESSON:",
                    "WHEN THE LAST ASCENDED CONTESTANT\nCONQUERED THE DUNGEON...",
                    "THE WORLD WAS SHAKEN TO ITS CORE.\nNO SUCH ACCOMPLISHMENT WAS\nEVER THOUGHT POSSIBLE.",
                    "QUESTIONS WERE RAISED: WAS THE\nPREVIOUS WINNER TRULY DESERVING OF\nTHEIR TITLE?",
                    "AS I RECALL, HE WAS FROM\nYOUR WORLD.",
                    "WAS HE THE LIMIT OF YOUR POTENTIAL?",
                    "THIS IS WHAT I INTEND TO FIND OUT\nTODAY.",
                    "ASCENDED CONTESTANT, YOU MIGHT NOT\nHAVE REALIZED...",
                    "BUT THE POWER YOU SEEK WILL\nNOT BE GIVEN TO YOU THROUGH\nTHE ENDLESS DUNGEON.",
                    "THOSE WISHING TO PUT A STOP\nTO THE ENDLESS CYCLE OF ASCENSION\nWERE MISGUIDED!",
                    "IT IS MERELY A PLACEBO TO FILTER\nTHE FOOLS WHO WANT ALL THE STRENGTH\nWE OFFER WITHOUT THE SACRIFICE.",
                    "ONLY BY CONQUERING THE\nLABRYBUCE AND DEFEATING THE\nELDERS...",
                    "...WILL YOU TRULY BE ABLE TO\nPUT AN END TO THE ASCENDED\nGAME!",
                    "COME FORTH, ASCENDED AND PROVE YOURSELF\nWORTHY OF ACCOMPLISHING THIS FEAT!",
                }
            },
            {
                EnemyNames.Pleromyr, new string[]
                {
                    "Ah... a seeker, a climber,\na soul drawn by the endless\nspiral...",
                    "Two hundred steps in the darkened abyss,\nyet what do you seek, truly?",
                    "The top is near, or so you believe...\nbut what is 'top,' what is 'bottom,'\nwhen all is but a circle within circles?",
                    "You ascend, and yet, descend...",
                    "You understand, and yet, are lost...",
                    "Is it knowledge you crave,\nor ignorance you flee?",
                    "The Labrybuce... a maze of mind and\nmatter, a riddle with no answer\nyet countless solutions...",
                    "Would you open the door, only\nto find a challenge you cannot\novercome?",
                    "What will you do when you can\nno longer subsist off the blessings\nof the Labrybuce?",
                    "I look forward to your answer!",
                    "Let us dance in this spiral... and see\nif you can untangle these threads\nof fate, or be woven into them!",
                }
            },
            {
                EnemyNames.Kodek, new string[]
                {
                    "...So the rumors were true.",
                    "An Ascended contestant has\nsomehow managed to come this\nfar up the Labrybuce.",
                    "Of course, this must not mean\nmuch to you so I will get\nto the point.",
                    "I am Kodek. I am the esteemed guard\nof the Bucilium Chamber.",
                    "While I cannot deny your tenacity\nfor reaching my domain, I can\nassure you one thing is clear.",
                    "The Labrybuce is far too forgiving\nfor someone like yourself to have\nmade it here!",
                    "Through a series of flukes, you have\nfailed your way to the top.",
                    "It is my duty to put an end\nto the cycle of delusions convincing\nyou that you will be successful on your quest!",
                    "Come, Ascended! Let me show you\nthe extent to which you've fallen\nbehind!",
                    "Your fraudulent journey ends here!!!",
                }
            },
            {
                EnemyNames.Ancient_Nodys, new string[]
                {
                    "One who walks the path of blasphemy, turn\nback now.",
                    "I am but a fragment of one of the\nbeings who control this game.",
                    "I am an extension of their will.",
                    "One who seeks forbidden strength,\nreturn to your dimension at once.",
                    "If you disregard this warning, then I will\ncarry out your sentence as a being wishing\nto thwart the Elder's game."
                }
            },
            {
                EnemyNames.Sentinal, new string[]
                {
                    "O bearer of blasphemy, turn thy steps\nback, lest you court inevitable ruin.",
                    "I am but the vestige of one who wields\ncommand over this very game.",
                    "An emissary of their boundless will, given form.",
                    "Should you seek power unbidden, return\nat once to your lesser plane.",
                    "Ignore this decree, and I shall mete out\nyour doom, as one who defies the Elders' design."
                }
            },
            {
                EnemyNames.Iminth, new string[]
                {
                                        "Thou who treadst the path of heresy,\nturn thee back, for thou\nencroachest upon the sacred.",
                    "I am but a shadow of the Ones who\ndoth govern this dominion.",
                    "An avatar of their eternal will, made manifest.",
                    "Shouldst thou seek power forbidden,\nreturn thee hence to thine own realm.",
                    "But if thou wilt not heed this warning,\nthen by mine hand shall thy doom be wrought,\nfor none may defy the Elders' will and endure."

                 }
            },
            {
                EnemyNames.Pakorag, new string[]
                {
                    "Treading the path of heresy, are ya?\nTurn back now, while you still can.",
                    "I’m just a shadow of the ones\nwho pull the strings in this game.",
                    "A puppet of their twisted desires, given flesh.",
                    "You think you can handle forbidden power?\nRun back to your little world.",
                    "But if you’re stupid enough to stay, I’ll be\nthe one to tear you apart for defying the Elders’ play."
                }
            },
            {
                EnemyNames.Floor_Architect, new string[]
                {
                    "Greetings, young Ascended.",
                    "There is no point in attempting to\ndisuade you from your conquest\nat this point, so I will keep it brief:",
                    "I am the nameless architect who\ndesigned the floors of the Labrybuce.",
                    "It is a role that I was grateful\nto receive after my untimely death at\nthe hands of the game.",
                    "Now my soul endless travels across\ndimensions per the Elders' whims, creating\nnew obstacles for future Ascendeds.",
                    "Throughout each universe, I've yet\nto see someone formally challenge\nthe Elders such as you.",
                    "While I wish I could support such\nan endeavor, I'm sadly bound by the\nshackles this game has placed on me.",
                    "I have been linked to a key needed\nto find the last Elder who presides over\nthe rest.",
                    "Meaning, I must face you in combat\nlest I be cast into an endless prison\ndevised by the Elders as punishment.",
                    "Let it be known that I wish you\nno hard feelings.",
                    "For this is the price you must pay\nif you wish to truly Ascend."
                }
            }
        };
    }
}
