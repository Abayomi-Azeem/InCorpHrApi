using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Domain.ValueObjects
{
    public static class AllPersonalityQuestion
    {
        public static List<PersonalityQuestion> PersonalityQuestions()
        {
            
            var personalityQuestion1 = new PersonalityQuestion
            {
                QuestionId = 1,
                QuestionValue = "It's a Friday evening, It's been a long week and you need to unwind" +
                                    "Your friends propose a spontaneous gathering. " +
                                    "Do you feel the thrill of organizing a game night, bringing everyone together for laughter and camaraderie," +
                                    " or do you sense the call of solitude, opting for a cozy night with a book or movie," +
                                    " recharging in your own haven",
                Options = new List<PersonalityTestOptions>
                {
                    new PersonalityTestOptions()
                    {
                        Id = 1,
                        Value = "E",

                    },
                    new PersonalityTestOptions()
                    {
                        Id = 2,
                        Value = "I"
                    }
                }

            };
            var personalityQuestion2 = new PersonalityQuestion
            {
                QuestionId = 2,
                QuestionValue = "The next Monday after the relaxing weekend. A new project unfolds at work, demanding innovation and you've been asked to spear head it." +
                                    "Do you immerse yourself in the details, breaking down tasks, " +
                                    "and creating a step-by-step plan for a smooth execution," +
                                    "or do you feel the spark of creativity, exploring possibilities, brainstorming " +
                                    "ideas, and pushing the boundaries of traditional approaches?",
                Options = new List<PersonalityTestOptions>
                {
                    new PersonalityTestOptions()
                    {
                        Id = 1,
                        Value = "S",

                    },
                    new PersonalityTestOptions()
                    {
                        Id = 2,
                        Value = "N"
                    }
                }

            };
            var personalityQuestion3 = new PersonalityQuestion
            {
                QuestionId = 3,
                QuestionValue = "A close friend confides in you, seeking guidance. Do you find comfort in logical analysis, " +
                                    "offering practical solutions and constructive feedback" +
                                    "or do you embrace empathy, providing a listening ear, offering emotional support, " +
                                    "and focusing on the impact on their feelings?",
                Options = new List<PersonalityTestOptions>
                {
                    new PersonalityTestOptions()
                    {
                        Id = 1,
                        Value = "T",

                    },
                    new PersonalityTestOptions()
                    {
                        Id = 2,
                        Value = "F"
                    }
                }

            };
            var personalityQuestion4 = new PersonalityQuestion
            {
                QuestionId = 4,
                QuestionValue = "Upon completion of the project, the team is going on a team bonding exercise." +
                                 "As you pack for the upcoming journey, do you methodically create a detailed checklist," +
                                 "organize items systematically, and ensure everything is in its place before departure," +
                                 "or do you toss essentials into a bag at the last minute, savoring the excitement of " +
                                 "spontaneity and adaptability?",
                Options = new List<PersonalityTestOptions>
                {
                    new PersonalityTestOptions()
                    {
                        Id = 1,
                        Value = "J",

                    },
                    new PersonalityTestOptions()
                    {
                        Id = 2,
                        Value = "P"
                    }
                }

            };

            var personalityTestQuestions = new List<PersonalityQuestion>() { personalityQuestion1, personalityQuestion2, personalityQuestion3, personalityQuestion4 };
            return personalityTestQuestions;
        }
    }
}
