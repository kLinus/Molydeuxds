using System;
using System.Collections;
using System.Collections.Generic;


public class MadLib
{
	
	public class LibPart
	{
		public string[] words;
		public string key;
		
		public LibPart(string newKey, string[] newWords)
		{
			words = newWords;
			key = newKey;
		}
	}

	private List<LibPart> lp_libParts;
	
	private string[] sa_paragraphs = new string[] {
		@"Hah, sicker than your average Poppa
Twist cabbage off instinct niggas don't think shit stink
pink gators, my Detroit NOUNs
Timbs for my hooligans in Brooklyn
Dead right, if they head right, Biggie there every night
NICK been ADJ since days of Underroos
Never lose, never choose to, bruise crews who
do something to us, talk go through us
Girls walk to us, wanna do us, VERB us
Who us? Yeah, Poppa and Puff (ehehehe)
Close like Starsky and Hutch, stick the clutch
Dare I squeeze three at your cherry M-3
(Take that, take that, take that, ha ha!)
Bang every MC easily, busily
Recently niggas fronting ain't saying nothing (nope)
So I just VERB my piece, (c'mon) keep my piece
Cubans with the Jesus piece (thank you God), with my peeps
Packing, asking who want it, you got it nigga flaunt it
That Brooklyn NOUN, we're on it"

	};

	
	// Use this for initialization
	public MadLib()
	{
		lp_libParts = new List<LibPart>();
		
		lp_libParts.Add(new LibPart("NOUN", new string[] {
			"roller skate",
			"banana",
			"grave"
		}));
		
		lp_libParts.Add(new LibPart("VERB", new string[] {
			"staple",
			"punch",
			"slide"
		}));
		
		lp_libParts.Add(new LibPart("ADJ", new string[] {
			"cold",
			"happy",
			"yellow"
		}));
		
	}
	
	public String GetParagraph(string name)
	{
		return GetParagraph(name, name, name);
	}
	
	public String GetParagraph(string name, string nickname)
	{
		return GetParagraph(name, nickname, name);
	}
	
	public String GetParagraph(string name, string nickname, string formalname)
	{
		Random rand = new Random();
		
		lp_libParts.Add(new LibPart("NAME", new string[] { name }));
		lp_libParts.Add(new LibPart("NICK", new string[] { nickname }));
		lp_libParts.Add(new LibPart("FORMAL", new string[] { formalname }));
		
		String paragraph = sa_paragraphs[rand.Next(sa_paragraphs.Length)];
		
		foreach(LibPart lp in lp_libParts)
		{
			int keylength = lp.key.Length;
			int i = 0;
			while((i = paragraph.IndexOf(lp.key, i)) != -1)
			{
				string randomWord = lp.words[rand.Next(lp.words.Length)];
				//find if last character was a period, and capitalize the first letter of the added word
				int j = i;
				do{
					j--;
					if(paragraph[j] == char.Parse(".") || paragraph[j] == char.Parse("\n"))
						randomWord = char.ToUpper(randomWord[0]) + randomWord.Substring(1);
				}
				while(char.IsWhiteSpace(paragraph[j]));
				
				paragraph = paragraph.Remove(i,keylength).Insert(i,randomWord);
				i += keylength;
			}
		}
		
		return paragraph;
	}
}

