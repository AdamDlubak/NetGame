using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsUI : MonoBehaviour
{
    public GameObject prefab;
    public PlayerStatisticsStruct localPlayerStatistics;

    public struct HighscoreRecord {
        public GameObject node;
        public Text rank, name, points;

        public HighscoreRecord(GameObject node)
        {
            this.node = node;
            this.rank = node.transform.Find("Rank").GetComponent<Text>();
            this.name = node.transform.Find("Name").GetComponent<Text>();
            this.points = node.transform.Find("Points").GetComponent<Text>();
        }
    }

    private List<HighscoreRecord> _records;
    private Color _highlightColor = new Color(0, 1, 1, 1);
    private Color _defaultColor = new Color(1, 1, 1, 1);
    private Text _pointsLabel, _upperTitle, _lowerTitle;
    private Transform _highscore;

    string GetRank(int rank)
    {
        if (rank == 0) return "1st";
        else if (rank == 1) return "2nd";
        else if (rank == 2) return "3rd";
        else return string.Format("{1}th", rank);
    }

    void Start()
    {
        _pointsLabel = transform.Find("Container/PointsLabel").GetComponent<Text>();
        _upperTitle = transform.Find("Container/UpperTitle").GetComponent<Text>();
        _lowerTitle = transform.Find("Container/LowerTitle").GetComponent<Text>();
        _highscore = transform.Find("Container/HighScore");
        SetupStatisticsGUI();
    }

    void Update ()
    {
        var highscore = new List<PlayerStatistics>(FindObjectsOfType<PlayerStatistics>());
        highscore.Sort((x, y) => {
            if (x.statistics.points > y.statistics.points) { return -1; }
            else if (x.statistics.points < y.statistics.points) { return 1; }
            return 0;
        });

        for (int i = 0; i < _records.Count; i++)
        {
            _records[i].rank.text = GetRank(i);
            _records[i].name.text = highscore[i].nick;
            _records[i].points.text = highscore[i].statistics.points.ToString("0.00");

            var color = (highscore[i].isLocalPlayer ? _highlightColor : _defaultColor);
            _records[i].rank.color = _records[i].name.color = _records[i].points.color = color;
        }
    }

    void SetupStatisticsGUI()
    {
        var players = new List<PlayerStatistics>(FindObjectsOfType<PlayerStatistics>());

        var points = Mathf.RoundToInt(localPlayerStatistics.points);
        var header = StatisticsStore.GetHeader(points);

        _pointsLabel.text = points.ToString();
        _upperTitle.text = header.upperTitle;
        _lowerTitle.text = header.lowerTitle;

        _records = new List<HighscoreRecord>();
        for (int i = 0; i < players.Count; i++)
        {
            var instance = Instantiate<GameObject>(prefab, prefab.transform.position, prefab.transform.rotation);
            instance.transform.SetParent(_highscore.transform, false);
            instance.transform.Translate(new Vector3(0, -30 * i, 0));
            instance.transform.name = "Rank" + (i + 1);
            _records.Add(new HighscoreRecord(instance));
        }
    }

#region PublicInterface

    public void DestroyHighscoreList()
    {
        foreach (var item in _records)
        {
            Destroy(item.node);
        }
        _records.Clear();
    }

# endregion
}
