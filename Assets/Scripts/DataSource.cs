using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;


namespace DataCenter
{
	public class DataSource
	{
		const string FILENAME = "./Assets/Scripts/wordList_combined.csv";
		private List<SongInfo> _songs = new List<SongInfo> ();


		public void Load (string filename = FILENAME)
		{
			_songs.Clear ();
			StreamReader file = new StreamReader (filename);
			string line = String.Empty;
			uint row = 0;

			while ((line = file.ReadLine ()) != null) {
				if (row != 0) {
					// parse each row
					_songs.Add (new SongInfo (line));
				}
				row += 1;
			}
			file.Close ();
		}

		public SongInfo GetSongByIndex (uint index)
		{
			if (index > _songs.Count || index == 0) { // index begins with 1
				return null;
			} else {
				return _songs [(int)index - 1];
			}
		}

		public List<SongInfo> GetAllSongs ()
		{
			return _songs;
		}
	}

	public class SongInfo
	{
		public uint SongIndex;
		public string SongName;
		public string Artist;
		public uint Year;
		public uint Rank;

		public string[] WordList;

		public SongInfo (string csvRow)
		{
			// Word List,Artist,Song Name,Rank,Song Index,Year
			string[] fields = csvRow.Split (',');

			// assume CSV doesn't change by itself
			WordList = fields [0].Replace ("\"", "").Split (' ');
			Artist = fields [1];
			SongName = fields [2];
			Rank = uint.Parse (fields [3]);
			SongIndex = uint.Parse (fields [4]);
			Year = uint.Parse (fields [5]);
		}

		public override string ToString ()
		{
			return "Song [" + SongIndex + "] by " + Artist + " - rank " + Rank + " of year " +
				Year + " with top words " + String.Join (" ", WordList);  
		}
	}

	public class AnnualWordList
	{
		const string FILENAME = "./Assets/Scripts/annual_top_words_sen.csv";
		public Dictionary<string, float> sentimentTable = new Dictionary<string,float> ();
		string[] STOPWORDS = { };
		// {"wan", "wit", "cause"};
		const uint TOP_COUNT = 5;
		// _topWordsAndYears

		private Dictionary<uint, List<TopWord>> _annualWords = new Dictionary<uint, List<TopWord>> ();
		private List<TopWord> _topWords = new List<TopWord> ();
		private Dictionary<string, List<uint>> _topWordsAndYears = new Dictionary<string, List<uint>> ();

		public void Load (string filename = FILENAME)
		{
			_topWords.Clear ();
			_annualWords.Clear ();
			StreamReader file = new StreamReader (filename);
			string line = String.Empty;
			Dictionary<string, uint> topWords = new Dictionary<string, uint> ();

			while ((line = file.ReadLine ()) != null) {
				string[] fields = line.Split (',');
				if (fields.Length == 0 || fields.Length < 4) {
					continue;
				}

				// update annual top words list
				uint year = uint.Parse (fields [0]);
				if (!_annualWords.ContainsKey (year)) {
					_annualWords [year] = new List<TopWord> ();
				}
				string word = fields [1].Trim ();

				if (word.Length == 0 || Array.IndexOf (STOPWORDS, word) != -1) {   // skip if it's in the stop words
					continue;
				}

				uint count = uint.Parse (fields [2]);
				float polarity = float.Parse (fields [3]);
				_annualWords[year].Add (new TopWord (word, count, polarity));
				if (!sentimentTable.ContainsKey (word)) {
					//UnityEngine.Debug.Log (word + " - " + polarity);
					sentimentTable.Add (word, polarity);
				}


				// update total top words list
				if (!topWords.ContainsKey (word)) {
					topWords [word] = count;
				} else {
					topWords [word] += count;
				}

				// assume annualWords are sorted by count
				if (_annualWords [year].Count <= TOP_COUNT) {
					// update words and years list
					if (!_topWordsAndYears.ContainsKey (word)) {
						_topWordsAndYears [word] = new List<uint> ();
					}
						
					_topWordsAndYears [word].Add (year);
				}
			}

			// sort the dictionary and save to the list
			foreach (KeyValuePair<string, uint> item in topWords) {
				_topWords.Add (new TopWord (item.Key, item.Value, 0)); //polarity is set to 0 because we don't use this value in _topWords
			}


			_topWords.Sort ();

			file.Close ();
		}

		public List<TopWord> GetTopWordsByYear (uint year, int top)
		{
			if (year < 1965 || year > 2015 || !_annualWords.ContainsKey (year)) { // index begins with 1
				return null;
			} else {   
				// return the whole list
				if (top > _annualWords [year].Count || top <= 0) {
					return _annualWords [year];
				} else {   // return part of the list
					List<TopWord> ret = new List<TopWord> ();

					for (int i = 0; i <= top; i++) {
						ret.Add (_annualWords [year] [i]);
					}

					return ret;
				}
			}
		}

		public List<TopWord> GetTopWords (int top)
		{
			// return the whole list
			if (top > _topWords.Count || top <= 0) {
				return _topWords;
			} else {   // return part of the list
				List<TopWord> ret = new List<TopWord> ();

				for (int i = 0; i <= top; i++) {
					ret.Add (_topWords [i]);
				}

				return ret;
			}
		}

		public uint[] GetYearsByTopWord (string topWord)
		{
			if (!_topWordsAndYears.ContainsKey (topWord)) {
				return null;
			} else {
				return _topWordsAndYears [topWord].ToArray ();
			}
		}




	}

	public class TopWord : IComparable <TopWord>
	{
		public string Word;
		public uint Count;
		public float Polarity;

		public TopWord (string word, uint count, float polarity)
		{
			Word = word;
			Count = count;
			Polarity = polarity;
		}


		public int CompareTo (TopWord other)
		{
			if (other == null) {
				return 1;
			} else {   // descending order
				return -Count.CompareTo (other.Count);
			}
		}
	}
}



