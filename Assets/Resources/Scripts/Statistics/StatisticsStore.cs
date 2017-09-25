using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatisticsStore
{
    public struct Header
    {
        public int requiredPoints;
        public string upperTitle, lowerTitle;

        public Header(int requiredPoints, string upperTitle, string lowerTitle)
        {
            this.requiredPoints = requiredPoints;
            this.upperTitle = upperTitle;
            this.lowerTitle = lowerTitle;
        }
    }

    public static List<Header> headers = new List<Header>
    {
        new Header(120, "Chuck Norris", "is proud of you!"),
        new Header(60, "Not bad", "That was really a quite nice ride"),
        new Header(30, "This stunt was calculated", "But Bro... I'm bad at math"),
        new Header(15, "[Knock knock] Who's there?", "Definitely not you, because you're dead"),
        new Header(0, "Nah...", "that was really rapid"),
    };

    public static Header GetHeader(int points)
    {
        foreach(var header in headers)
        {
            if (points > header.requiredPoints)
                return header;
        }
        return new Header(0, "You DIED!", "");
    }
}
