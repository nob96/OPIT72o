using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using OPIT72o.Model.DataObject;

namespace OPIT72o.Model
{
    class Parser
    {
        public Hand Hand { get; set; }
        public string PfadLokaleHaende { get; set; }
        public string LogfilePath { get; private set; }
        public string LastHand { get; private set; }

        public Parser(string PfadLokaleHaende, string Nickname)
        {
            this.PfadLokaleHaende = PfadLokaleHaende;
            this.LogfilePath = this.FindLogfile().FullName;
            while (!this.IsFileReady(this.LogfilePath))
            {
                Thread.Sleep(150);
            }
            this.Hand = new Hand();
            this.Hand.Nickname = Nickname;

            this.LastHand = this.FindLastPokerhand();
            this.AddPokerKingID();
            this.AddHolecards();
            this.AddFlop();
            this.AddTurn();
            this.AddRiver();
            this.AddZusammenfassung();
        }

        private bool IsFileReady(string filename)
        {
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    return inputStream.Length > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private FileInfo FindLogfile()
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(this.PfadLokaleHaende);
                FileInfo file = dirInfo.GetFiles()
                     .OrderByDescending(f => f.LastWriteTime)
                     .First();

                return file;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }



        private string FindLastPokerhand()
        {
            string content = File.ReadAllText(this.LogfilePath);
            int pos = content.LastIndexOf("Hand #");

            return content.Substring(pos);
        }

        private void AddHolecards()
        {
            string delimiter = this.HasFlop() ? "*** FLOP ***" : "*** SUMMARY ***";
            string holeCardAbschnitt = this.FindAbschnitt("*** HOLE CARDS ***", delimiter);

            string cardValues = this.FindAbschnitt("[", "]", holeCardAbschnitt);
            List<Card> cardList = this.ParseCardValueStringToObjectList(cardValues);

            this.Hand.Holecard1 = cardList[0];
            this.Hand.Holecard2 = cardList[1];
        }

        private void AddPokerKingID()
        {
            string delimiter = "-";
            string pokerKingID = "#" + this.FindAbschnittFirstIndexOfDelimiter("#", delimiter, this.FindLastPokerhand());
            this.Hand.PokerKingID = pokerKingID;
        }

        private void AddFlop()
        {
            try
            {
                string latestFlopAbschnitt;
                if (this.LastHand.Contains("*** TURN ***"))
                {
                    latestFlopAbschnitt = this.FindAbschnitt("*** FLOP ***", "*** TURN ***");

                }
                else
                {
                    latestFlopAbschnitt = this.FindAbschnitt("*** FLOP ***", "*** SHOWDOWN ***");

                }

                string cardValues = this.FindAbschnitt("[", "]", latestFlopAbschnitt);
                List<Card> cardList = this.ParseCardValueStringToObjectList(cardValues);

                this.Hand.Flop1 = cardList[0];
                this.Hand.Flop2 = cardList[1];
                this.Hand.Flop3 = cardList[2];
            }
            catch (ArgumentOutOfRangeException e)
            {
                this.Hand.Flop1 = new Card();
                this.Hand.Flop1.FullName = "Nicht gespielt";
                this.Hand.Flop2 = new Card();
                this.Hand.Flop2.FullName = "Nicht gespielt";
                this.Hand.Flop3 = new Card();
                this.Hand.Flop3.FullName = "Nicht gespielt";
            }
        }

        private void AddTurn()
        {
            try
            {
                string latestTurnAbschnitt;
                if (this.LastHand.Contains("*** RIVER ***"))
                {
                    latestTurnAbschnitt = this.FindAbschnitt("*** TURN ***", "*** RIVER ***");

                }
                else
                {
                    latestTurnAbschnitt = this.FindAbschnitt("*** TURN ***", "*** SHOWDOWN ***");

                }

                string cardValues = this.FindAbschnitt("[", "]", latestTurnAbschnitt);
                List<Card> cardList = this.ParseCardValueStringToObjectList(cardValues);

                this.Hand.Turn = cardList[0];
            }
            catch (ArgumentOutOfRangeException e)
            {
                this.Hand.Turn = new Card();
                this.Hand.Turn.FullName = "Nicht gespielt";
            }
        }

        private void AddRiver()

        {
            try
            {

                string latestRiverAbschnitt = this.FindAbschnitt("*** RIVER ***", "*** SHOW DOWN ***");
                string cardValues = this.FindAbschnitt("[", "]", latestRiverAbschnitt);
                List<Card> cardList = this.ParseCardValueStringToObjectList(cardValues);

                this.Hand.River = cardList[0];
            }
            catch (ArgumentOutOfRangeException e)
            {
                this.Hand.River = new Card();
                this.Hand.River.FullName = "Nicht gespielt";
            }
        }

        private void AddZusammenfassung()
        {
            string latestSummary = this.FindAbschnitt("*** SUMMARY ***", "");
            this.Hand.Pot = this.FindPot(latestSummary);
            this.Hand.Rake = this.FindRake(latestSummary);
            this.Hand.ErgebnisDetail = this.FindErgebnisDetail(latestSummary);
            this.Hand.Ergebnis = this.ParseErgebnis(this.Hand.ErgebnisDetail);
        }

        private List<DataObject.Card> ParseCardValueStringToObjectList(string cardValueString)
        {
            List<Card> cardList = new List<Card>();
            String[] cardValues = cardValueString.Split(' ');

            foreach (String cardValue in cardValues)
            {
                char[] cardDetail = cardValue.ToCharArray();
                Card card = new Card();
                card.Value = cardDetail[0];
                card.Color = cardDetail[1];
                card.FullName = cardValue;
                cardList.Add(card);
            }

            return cardList;
        }


        private string FindAbschnitt(string delimiterStart, string delimiterStop)
        {
            int startPos = this.LastHand.IndexOf(delimiterStart);
            int stopPos = this.LastHand.LastIndexOf(delimiterStop);
            int lenght = stopPos - startPos;

            return this.LastHand.Substring(startPos, lenght);
        }

        private string FindAbschnitt(string delimiterStart, string delimiterStop, string content)
        {
            int startPos = content.LastIndexOf(delimiterStart) + 1;
            int stopPos = content.LastIndexOf(delimiterStop);
            int lenght = stopPos - startPos;

            return content.Substring(startPos, lenght);
        }

        private string FindAbschnittFirstIndexOfDelimiter(string delimiterStart, string delimiterStop, string content)
        {
            int startPos = content.IndexOf(delimiterStart) + 1;
            int stopPos = content.IndexOf(delimiterStop);
            int lenght = stopPos - startPos;
            if (lenght > 0)
            {
                return content.Substring(startPos, lenght);
            }

            return "empty";
        }

        private string FindErgebnisDetail(string latestSummary)
        {
            string ergebnisLine = null;
            String[] lines = latestSummary.Split('\n');

            foreach (String line in lines)
            {
                if (line.Contains(this.Hand.Nickname))
                {
                    ergebnisLine = line + "EOL";
                }
            }

            ergebnisLine = this.FindAbschnittFirstIndexOfDelimiter(":", "EOL", ergebnisLine);

            return ergebnisLine.Trim();
        }

        private string ParseErgebnis(string ergebnisLine)
        {
            if (ergebnisLine.Contains("mucked"))
            {
                return "Lose";
            }
            else if (ergebnisLine.Contains("won"))
            {
                return "Win";
            }
            else
            {
                return "Fold";
            }
        }

        private decimal FindPot(string latestSummary)
        {
            string potLine = null;
            String[] lines = latestSummary.Split('\n');

            foreach (String line in lines)
            {
                if (line.Contains("Total pot"))
                {
                    if (line.Contains("Rake"))
                    {
                        potLine = this.FindAbschnittFirstIndexOfDelimiter("Total pot", "|", line);
                    }
                    else
                    {
                        potLine = line + " EOL";
                        potLine = this.FindAbschnittFirstIndexOfDelimiter("Total pot", " EOL", potLine);
                    }
                }
            }



            if (potLine.Contains("Total pot"))
            {
                string abschnittToRemove = this.FindAbschnitt("Total", "", potLine);
                potLine = potLine.Replace(abschnittToRemove, "");
            }

            Regex regex = new Regex("[^0-9. -]");
            potLine = regex.Replace(potLine, "");

            return Convert.ToDecimal(potLine);
        }

        private decimal FindRake(string latestSummary)
        {
            //Kein rake
            if (latestSummary.Contains("Rake") == false)
            {
                return 0;
            }

            string rakeLine = null;

            //Find Rakeline and add EOL
            String[] lines = latestSummary.Split('\n');
            foreach (String line in lines)
            {

                if (line.Contains("Rake "))
                {

                    //PokerKing JP Fee 
                    if (line.Contains("JP"))
                    {
                        rakeLine = this.FindAbschnittFirstIndexOfDelimiter("|", "JP", line);
                    }
                    else
                    {
                        rakeLine = line + " EOL";
                        rakeLine = this.FindAbschnittFirstIndexOfDelimiter("|", " EOL", rakeLine);
                    }


                }
            }

            Regex regex = new Regex("[^0-9. -]");
            rakeLine = regex.Replace(rakeLine, "");

            return Convert.ToDecimal(rakeLine);
        }

        private bool HasFlop()
        {
            return this.LastHand.Contains("*** FLOP ***");
        }
    }
}
