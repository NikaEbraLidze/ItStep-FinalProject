using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace HangmanGame.DataAccess
{
    public class XmlDataService
    {
        private const string FileName = "../../../hangman_scores.xml";
        private readonly SemaphoreSlim _fileLock = new SemaphoreSlim(1, 1);
        private static readonly XName RootName = "PlayerScores";
        private static readonly XName ScoreName = "PlayerScore";

        private XDocument LoadOrCreate()
        {
            if (!File.Exists(FileName))
            {
                var root = new XElement(RootName);
                var newDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root);

                newDoc.Save(FileName);
                return newDoc;
            }

            return XDocument.Load(FileName);
        }

        private void Save(XDocument doc)
        {
            var temp = FileName + ".tmp";
            doc.Save(temp);
            File.Copy(temp, FileName, overwrite: true);
            File.Delete(temp);
        }

        public async Task SaveScoresAsync(List<PlayerScore> scores)
        {
            await _fileLock.WaitAsync();

            try
            {
                var doc = new XDocument(
                    new XElement(RootName,
                        scores.Select(FromDto)
                    )
                );

                Save(doc);
            }
            finally { _fileLock.Release(); }
        }


        public async Task<List<PlayerScore>> LoadScoresAsync()
        {
            await _fileLock.WaitAsync();

            try
            {
                var doc = LoadOrCreate();

                return doc.Root?
                    .Elements(ScoreName)
                    .Select(ToDto)
                    .Where(s => s.Name != null)
                    .ToList()
                    ?? new List<PlayerScore>();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading scores: {ex.Message}");
                return new List<PlayerScore>();
            }
            finally
            {
                _fileLock.Release();
            }
        }

        private static PlayerScore ToDto(XElement el)
        {
            return new PlayerScore
            {
                Name = el.Element("Name")?.Value,
                HighScore = int.TryParse(el.Element("HighScore")?.Value, out int score) ? score : 0
            };
        }

        private static XElement FromDto(PlayerScore s)
        {
            return new XElement(ScoreName,
                new XElement("Name", s.Name),
                new XElement("HighScore", s.HighScore)
            );
        }
    }
}