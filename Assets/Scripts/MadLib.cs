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
		@"Hah, we're on it"

	};

	private string[] sa_childhood = new string[] {
		@"When I was young I spent my days at *pluralplace* even though it made my parents feel *feelbad*. Oh well, it always made me feel *feelgood*. Other Molydeuxds picked on me because I had a *shape* *bodypart* that really stuck out. I had a pet *animal* who was a total jerk and would always bite my *bodypart*. I would spend long nights with *pluralthing* and pick my nose a lot, as all good Molydeuxds do. I wanted to be a *occupation* when I grew up, but I learned I would have to *verb* a *singularthing* at a *singularplace* if I wanted to do that, so I ended up doing *hobby* instead." 
	};

	private string[] sa_teen = new string[] {
		@"In my teens I ran away from my *appearance* home and worked in a *size* *singularplace*. The pay was awful, but the work made me feel *feelgood*. Sometimes *shape* Molydeuxds would stop by and *verb* things for me. My favorite was a *size* *occupation* who would stop by and talk about how *appearance* bears are. Bears make me *feelbad*. Shortly after, I left to pursue my dream of *hobby* for a *appearance* living but I was mugged by a *shape* *occupation* and had to go back home. My *shape* parents were *feelgood* when I returned, but I felt *feelbad* for giving up on my *size* dreams. And they said I looked *shape* because they were still *feelbad* jerks."
	};
	
	private string[] sa_youngAdult = new string[] {
		@"As a young *size* adult I was really into *hobby*, until I took an arrow to the *bodypart* and I had to stop. In the end I felt *feelgood* because it meant I didn't have to work anymore. Work makes me feel *feelbad*. Until one day I met a *shape* Molydeuxd who was *appearance* and I fell in love.  Unfortunately, they would only date *occupation* Molyduexds with  *size* *bodypart* so I ended up feeling *feelbad*. I bought a *size* pet *animal* instead. It used to *verb* *pluralthing* when I was feeling *feelbad* to cheer me up. It was really good to me until it died in a freak accident involving a  *singularthing*, several *pluralthing*, a shady *occupation*, and a *shape* *animal* in the back of a *size* *singularplace*. I cried a lot until my friend gave me a new *appearance* *singularthing* and I forgot about my old *feelgood* pet. "
	};

	private string[] sa_adult = new string[] {
		@"I had a *feelbad* breakdown when I was an adult. It was triggered by a *appearance* *animal* that I saw and made me question the *size* universe we live in. I was under the *feelgood* influence of *pluralthing* which made me *verb* *pluralthing* to really feel *feelgood*. I went to a few *pluralplace* for rehab and even though I felt *feelbad* while their, it was for the best. Except when I tripped on a *shape* *singularthing* and lost the use of my *feelgood* *bodypart*. After I got back to feeling *feelgood* I decided to try and become a *appearance* *occupation* but it was too hard so I decided to just *verb* *pluralthing* instead. Friends noticed that I had become *size* and *shape* so I decided it was time to stop eating *pluralthing*. "
	};

	private string[] sa_elderly = new string[] {
		@"I was one of those *shape* *feelbad* elderly people. I spent way too much time doing *hobby* and yelling at *shape* children to make them feel *feelbad*. Other Molydeuxds took note of my situtaion and bought me a *size* pet *animal* with a *shape* *bodypart* which was weird, but it was a *feelgood* creature. All good things end however, and it was stolen by a *appearance* *occupation* while I was *hobby* at the local *singularplace*. I found a *size* tumor on my *bodypart* and was worried I had contracted cancer from *singularthing*  exposure, but it was just a *size* *feelbad* bug. I ended up getting married to a *size* *animal* but our *appearance* love wasn't recognized by the *feelbad* Molyduexd government and was annulled. As my health deteriorated I started collecting *size* *pluralthing* to feel *feelgood*, but I only felt *feelbad*. "
	};
	
	// Use this for initialization
	public MadLib()
	{
		lp_libParts = new List<LibPart>();
		
		lp_libParts.Add(new LibPart("*singularthing*", new string[] {
			"roller skate",
			"banana",
			"grave",
			"glass",
			"animal",
			"apple",
			"weapon",
			"bell",
			"yacht",
			"amulet",
			"firework",
			"fork",
			"fairy",
			"gun",
			"ice",
			"incubus",
			"succubus",
			"joker",
			"chain",
			"pen",
			"soda pop",
			"teddy bear",
			"body pillow",
			"notebook",
			"lawnmower",
			"computer",
			"backpack"
		}));
		
		lp_libParts.Add(new LibPart("*pluralthing*", new string[] {
			"roller skates",
			"bananas",
			"graves",
			"glasses",
			"animals",
			"apples",
			"weapons",
			"bells",
			"yachts",
			"amulets",
			"fireworks",
			"forks",
			"fairies",
			"guns",
			"ice",
			"incubusses",
			"succubusses",
			"jokers",
			"chains",
			"pens",
			"soda pops",
			"teddy bears",
			"body pillows",
			"notebooks",
			"lawnmowers",
			"computers",
			"backpacks"
		}));
		
		lp_libParts.Add(new LibPart("*singularplace*", new string[] {
			"skating rink",
			"graveyard",
			"warehouse",
			"mansion",
			"lawn",
			"library",
			"dock",
			"jail",
			"bar",
			"club",
			"laboratory",
			"dungeon",
			"madhouse",
			"museum",
			"tavern",
			"bazaar",
			"strip club",
			"hospital",
			"office",
			"church",
			"junkyard",
			"boat",
			"tower",
			"swamp",
			"forest",
			"mountain",
			"post office",
			"tundra"
		}));
		
		lp_libParts.Add(new LibPart("*pluralplace*", new string[] {
			"skating rink",
			"graveyard",
			"warehouses",
			"mansions",
			"lawns",
			"libraries",
			"docks",
			"jails",
			"bars",
			"clubs",
			"laboratories",
			"dungeons",
			"madhouses",
			"museums",
			"taverns",
			"bazaars",
			"strip clubs",
			"hospitals",
			"offices",
			"churches",
			"junkyards",
			"boats",
			"towers",
			"swamps",
			"forests",
			"mountains",
			"post offices",
			"tundra"
		}));
		
		lp_libParts.Add(new LibPart("*animal*", new string[] {
			"dragon",
			"dog",
			"cat",
			"stegasaurus",
			"liger",
			"velociraptor",
			"cow",
			"frog",
			"komodo dragon",
			"dung beetle",
			"platypus",
			"duck",
			"gnome",
			"displacer beast",
			"manticore",
			"pikachu",
			"snorlax",
			"mudcrab",
			"facehugger"
		}));
		
		lp_libParts.Add(new LibPart("*bodypart*", new string[] {
			"arm",
			"leg",
			"tooth",
			"nose",
			"head",
			"hand",
			"finger",
			"toe",
			"foot",
			"thigh",
			"chest",
			"abdomen",
			"knee",
			"ear",
			"earlobe",
			"eye",
			"eyebrow"
		}));
		
		lp_libParts.Add(new LibPart("*occupation*", new string[] {
			"Game Designer",
			"Software Engineer",
			"One-Eyed Bandit",
			"Sith Lord",
			"Hermit",
			"Game Journalist",
			"Starfleet Commander",
			"Jedi",
			"Pirate",
			"Thief",
			"Entertainer",
			"Con Artist",
			"Train Conductor",
			"Farmer",
			"Gladiator",
			"Circus Clown",
			"Janitor",
			"Food Tester",
			"Sandwich Artist",
			"Camp Instructor",
			"Window Washer",
			"Desperate Housewife",
			"Golfer",
			"Personal Assistant",
			"Comedian",
			"Kpop-Singer",
			"Mad Scientist",
			"Geisha",
			"Nanny",
			"Nun",
			"Optometrist",
			"Vagrant"
		}));
		
		lp_libParts.Add(new LibPart("*hobby*", new string[] {
			"knitting",
			"drawing",
			"playing with legos",
			"playing board games",
			"boating",
			"metal detecting",
			"blogging",
			"origami",
			"puppetry",
			"camping",
			"bowling",
			"cake decorating",
			"chess",
			"buying used games",
			"rafting",
			"canoeing",
			"rock collecting",
			"picking boogers",
			"coin collecting",
			"reading to the elderly",
			"shark wrestling",
			"sewing",
			"texting",
			"fencing",
			"gardening",
			"treasure hunting",
			"ghost hunting",
			"studying rocket science",
			"building jet engines",
			"yoga",
			"boot tossing"
		}));
		
		lp_libParts.Add(new LibPart("*verb*", new string[] {
			"abide",
			"gather",
			"jog",
			"jump",
			"obey",
			"throw",
			"bake",
			"rinse",
			"visit",
			"name",
			"question",
			"bake",
			"behave",
			"carry",
			"chase",
			"destroy",
			"disect",
			"design",
			"grope",
			"examine",
			"entertain",
			"inspire",
			"gratify",
			"purchase"
		}));
		
		lp_libParts.Add(new LibPart("*feelgood*", new string[] {
			"agreeable",
			"brave",
			"calm",
			"delightful",
			"eager",
			"faithful",
			"gentle",
			"happy",
			"jolly",
			"kind",
			"lively",
			"nice",
			"obedient",
			"proud",
			"relieved",
			"silly",
			"thankful",
			"victorious",
			"witty",
			"zealous"
		}));
		
		lp_libParts.Add(new LibPart("*feelbad*", new string[] {
			"angry",
			"bewildered",
			"clumsy",
			"defeated",
			"embarrassed",
			"fierce",
			"grumpy",
			"helpless",
			"itchy",
			"jealous",
			"lazy",
			"mysterious",
			"nervous",
			"obnoxious",
			"panicky",
			"repulsive",
			"scary",
			"thoughtless",
			"uptight",
			"worried"
		}));
		
		lp_libParts.Add(new LibPart("*shape*", new string[] {
			"broad",
			"chubby",
			"crooked",
			"curved",
			"deep",
			"flat",
			"high",
			"hollow",
			"low",
			"narrow",
			"round",
			"shallow",
			"skinny",
			"square",
			"steep",
			"straight",
			"wide"
		}));
		
		lp_libParts.Add(new LibPart("*size*", new string[] {
			"big",
			"colossal",
			"fat",
			"gigantic",
			"great",
			"huge",
			"immense",
			"large",
			"little",
			"mammoth",
			"massive",
			"miniature",
			"petite",
			"puny",
			"scrawny",
			"short",
			"small",
			"tall",
			"teeny",
			"tiny"
		}));
		
		lp_libParts.Add(new LibPart("*appearance*", new string[] {
			"adorable",
			"beautiful",
			"clean",
			"drab",
			"elegant",
			"fancy",
			"glamorous",
			"handsome",
			"long",
			"magnificent",
			"old-fashioned",
			"plain",
			"quaint",
			"sparkling",
			"ugliest",
			"unsightly",
			"wide-eyed",
			"hideous",
			"monstrous",
			"strange",
			"bizzare"
		}));
		
	}
	
	public String GetParagraph(int type, string name)
	{
		//UnityEngine.Debug.Log(name);
		return GetParagraph(type, name, name, name);
	}
	
	public String GetParagraph(int type, string name, string nickname)
	{
		return GetParagraph(type, name, nickname, name);
	}
	
	public String GetParagraph(int type, string name, string nickname, string formalname)
	{
		Random rand = new Random((int) DateTime.Now.Ticks);
		
		lp_libParts.Add(new LibPart("NAME", new string[] { name }));
		lp_libParts.Add(new LibPart("NICK", new string[] { nickname }));
		lp_libParts.Add(new LibPart("FORMAL", new string[] { formalname }));
		
		String paragraph;
		
		//UnityEngine.Debug.Log("Type: " + type);
		
		switch(type)
		{
		case 0:
			paragraph = sa_childhood[rand.Next(sa_childhood.Length)];
			break;
		case 1:
			paragraph = sa_teen[rand.Next(sa_teen.Length)];
			break;
		case 2:
			paragraph = sa_youngAdult[rand.Next(sa_youngAdult.Length)];
			break;
		case 3:
			paragraph = sa_adult[rand.Next(sa_adult.Length)];
			break;
		case 4:
			paragraph = sa_elderly[rand.Next(sa_elderly.Length)];
			break;
		default:
			paragraph = sa_paragraphs[rand.Next(sa_paragraphs.Length)];
			break;
			
		}
		
		 
		
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
				//i += keylength - 1;
				//UnityEngine.Debug.Log("stringLength: " + paragraph.Length + ", i:" + i + ", key: " + lp.key + ", keylength: " + keylength);
			}
		}
		
		return paragraph;
	}
}

