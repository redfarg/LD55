public class ScoreTranslator
{
    public static (string, string) TranslateSummonOptions(int ritualCount)
    {
        string demon = "";
        string failure = "";
        switch (ritualCount)
        {
            case 2:
                demon = "LORD";
                failure = "POTATO";
                break;
            case 1:
                demon = "FIEND";
                failure = "FRIEND";
                break;
            case 0:
                demon = "IMP";
                failure = "SHRIMP";
                break;
            default:
                break;
        }
        return (demon, failure);
    }

    public static string TranslateAdjectiveOptions(float totalPercentage)
    {
        var adjective = totalPercentage switch
        {
            <= 55f => "pathetic",
            <= 60f => "anemic",
            <= 65f => "frail",
            <= 70f => "feisty",
            <= 75f => "mighty",
            <= 80f => "grand",
            <= 85f => "apocalyptic",
            > 85f => "ungodly",
            _ => ""
        };

        return adjective;
    }

    public static string TranslateSecondSentenceOptions(int ritualCount)
    {
        var secondSentence = ritualCount switch
        {
            0 => "How awkward...",
            1 => "What the hell!?",
            2 => "Embarrassing.",
            _ => "Are you serious?"
        };

        return secondSentence;
    }
}