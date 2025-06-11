namespace ProjektJidelnicek
{
    public class Recept
    {
        public string Nazev { get; }
        public int Kategorie { get; }
        public bool MaPrilohu { get; }
        public List<Surovina> SeznamSurovin { get; }

        public Recept(string nazev, int kategorie, bool maPrilohu, List<Surovina> seznam)
        {
            Nazev = nazev;
            Kategorie = kategorie;
            MaPrilohu = maPrilohu;
            SeznamSurovin = seznam;
        }

        // Seznam vsech receptu
        public static List<Recept> vsechno = [];

        // Slovnik obsahujici kategorie receptu
        private static Dictionary<string, int> slovnikRecept = new Dictionary<string, int>
        {
            { "recept s masem", 1},
            { "recept bez masa", 2},
            { "recept na sladke jidlo", 3},
            { "priloha", 4},
            { "recept na peceni", 5 }
        };

        // Kategorie receptu
        public static Kategorie kategorieRecept = new Kategorie(slovnikRecept);

        // Slovnik obsahujici kategorie priloh
        private static Dictionary<string, int> slovnikPriloha = new Dictionary<string, int>
        {
            { "ma prilohu", 1},
            { "nema prilohu", 2},
        };

        // Kategorie priloh
        private static Kategorie kategoriePriloha = new Kategorie(slovnikPriloha);

        // Slovnik obsahujici moznosti vyhledavani
        private static Dictionary<string, int> slovnikVyhledavani = new Dictionary<string, int>
        {
            { "vyhledat podle nazvu", 1 },
            { "vyhledat podle surovin", 2 },
            { "vyhledat podle kategorie", 3},
        };

        // Kategorie vyhledavani
        private static Kategorie kategorieVyhledavani = new Kategorie(slovnikVyhledavani);

        // Cesta k souboru se seznamem receptu
        private static string soubor = @"C:\C_Sharp\czechitas_jaro_25\projekt_jidelnicek\ProjektJidelnicek\seznam";

        public static string oddelovac = "------------------------------------------------------";

        public static void NactiReceptyZeSouboru()
        // Metoda nacte recepty ulozene v souboru
        {
            string[] obsahSouboru = File.ReadAllLines(soubor);
            foreach (string radek in obsahSouboru)
            {
                string[] rozdelenyRadek = radek.Split('|');
                List<Surovina> seznamSurovin = Surovina.PrevedRetezecNaSuroviny(rozdelenyRadek[3]);

                Recept novyRecept = new Recept(rozdelenyRadek[0],
                                               int.Parse(rozdelenyRadek[1]),
                                               bool.Parse(rozdelenyRadek[2]),
                                               seznamSurovin
                                               );
                vsechno.Add(novyRecept);
            }
        }

        public static (bool JePlatny, string nazevReceptu, List<string> seznamSurovin) ZkontrolujVstup(string vstup)
        //Metoda zkontroluje vstup
        {
            bool jePlatny = false;
            string[] rozdelenyVstup = vstup.Split(',');
            List<string> seznamSurovin = [];

            if (rozdelenyVstup.Length >= 2)
            {
                foreach (string s in rozdelenyVstup.TakeLast(rozdelenyVstup.Length - 1).ToList())
                {
                    seznamSurovin = [.. seznamSurovin, s];
                }
                jePlatny = true;
            }
            return (jePlatny, rozdelenyVstup[0], seznamSurovin);
        }

        public static void PridejRecept()
        // Metoda se zepta na nazev receptu a suroviny
        // Dale zjisti do jake kategorie recept patri, jestli ma prilohu a roztridi suroviny do kategorii
        {
            Console.WriteLine("Zadejte novy recept ve formatu: 'nazev,surovina1,surovina2,surovina3,surovina4,surovina5'");
            string vstup = Console.ReadLine();
            bool maPrilohu;

            (bool JePlatny, string nazevReceptu, List<string> suroviny) = ZkontrolujVstup(vstup);
            if (!JePlatny)
            {
                Console.WriteLine("Recept nema vhodny format");
                return;
            }

            if (ZjistiJestliJeReceptVSeznamu(nazevReceptu))
            {
                Console.WriteLine("Recept s timto nazvem uz je v seznamu");
                return;
            }

            Console.WriteLine($"Do jake skupiny receptu patri '{nazevReceptu}'?");
            kategorieRecept.VypisKategorie();
            int cisloKategorie = kategorieRecept.NactiCisloKategorie();

            if ((cisloKategorie == 4) ^ (cisloKategorie == 5))
            {
                maPrilohu = false;
            }
            else
            {
                Console.WriteLine($"Ma recept '{nazevReceptu}' prilohu?");
                kategoriePriloha.VypisKategorie();
                int cisloPrilohy = kategoriePriloha.NactiCisloKategorie();
                maPrilohu = cisloPrilohy.Equals(1);
            }

            List<Surovina> seznamSurovin = Surovina.RoztridSuroviny(suroviny);

            Recept novyRecept = new(nazevReceptu, cisloKategorie, maPrilohu, seznamSurovin);
            vsechno.Add(novyRecept);
            File.AppendAllLines(soubor, [PrevedReceptNaRetezec(novyRecept)]);
            Console.WriteLine($"Recept '{nazevReceptu}' byl pridan");
        }

        public static void VypisRecepty(List<Recept> seznam)
        // Metoda vypise nazvy receptu na zadanem seznamu
        {
            Console.WriteLine(oddelovac);
            if (seznam.Count == 0)
            {
                Console.WriteLine("Recepty odpovidajici zadanym kriteriim nenalezeny");
            }
            foreach (string nazev in seznam.Select(x => x.Nazev))
            {
                Console.WriteLine(nazev);
            }
            Console.WriteLine(oddelovac);
        }

        public static string PrevedReceptNaRetezec(Recept recept)
        // Metoda prevede recept na retezec
        {
            var surovinyPole = recept.SeznamSurovin.Select(x => String.Join('-', x.Nazev, x.Kategorie));
            var surovinyRetezec = String.Join(',', surovinyPole);
            return String.Join('|', recept.Nazev, recept.Kategorie, recept.MaPrilohu, surovinyRetezec);
        }

        public static bool ZjistiJestliJeReceptVSeznamu(string nazevReceptu)
        // Metoda zjisti, jestli uz je recept ulozeny v seznamu
        {
            return vsechno.Any(x => x.Nazev == nazevReceptu);
        }

        public static Recept NajdiRecept(string nazevReceptu)
        // Metoda vrati recept s danym nazvem
        {
            return vsechno.Where(x => x.Nazev == nazevReceptu).Select(x => x).ToList()[0];
        }

        public static Recept NajdiReceptDleNazvu(string nazevReceptu)
        // Metoda najde recept v seznamu podle nazvu
        {
            return vsechno.Where(x => x.Nazev == nazevReceptu).Select(x => x).ToList()[0];
        }

        public static void VyhledejRecept()
        // Metoda vyhleda recept podle nazvu, surovin, nebo kategorie
        {
            Console.WriteLine("Podle ceho chcete recepty vyhledat?");
            kategorieVyhledavani.VypisKategorie();
            int vyhledavani = kategorieVyhledavani.NactiCisloKategorie();
            string vstup;

            switch (vyhledavani)
            {
                case 1:
                    Console.WriteLine("Zadejte cast nazvu receptu pro vyhledavani:");
                    vstup = Console.ReadLine();
                    VypisRecepty(vsechno.Where(x => x.Nazev.Contains(vstup)).Select(x => x).ToList());
                    break;
                case 2:
                    Console.WriteLine("Zadejte surovinu pro vyhledavani:");
                    vstup = Console.ReadLine();
                    string surovinyDleVstupu = String.Join(", ", Surovina.vsechno.Where(x => x.Nazev.Contains(vstup)).Select(x => x.Nazev).ToList());
                    if (surovinyDleVstupu.Length != 0)
                    {
                        Console.WriteLine($"Recepty obsahujici suroviny: {surovinyDleVstupu}");
                    }
                    VypisRecepty(vsechno.Where(x => x.SeznamSurovin.Any(x => x.Nazev.Contains(vstup))).Select(x => x).ToList());
                    break;
                case 3:
                    kategorieRecept.VypisKategorie();
                    int cislo = kategorieRecept.NactiCisloKategorie();
                    VypisRecepty(vsechno.Where(x => x.Kategorie == cislo).Select(x => x).ToList());
                    break;
            }
        }

        public static void SmazRecept()
        // Metoda smaze recept ze seznamu receptu
        // Po smazani receptu aktualizuje soubor s recepty i seznam surovin
        {
            Console.WriteLine("Zadejte nazev receptu, ktery chcete smazat. Muzete vybirat z techto moznosti:");
            Recept.VypisRecepty(Recept.vsechno);
            string vstup = Console.ReadLine();
            Recept receptKeSmazani;

            bool JeVSeznamu = ZjistiJestliJeReceptVSeznamu(vstup);
            if (!JeVSeznamu)
            {
                Console.WriteLine("Neznamy recept, neni mozne ho smazat");
                return;
            }

            receptKeSmazani = NajdiReceptDleNazvu(vstup);
            vsechno.Remove(receptKeSmazani);

            // soubor s recepty se vymaze
            File.WriteAllText(soubor, string.Empty);
            // seznam surovin se vymaze
            Surovina.vsechno = [];

            // recepty na seznamu se znovu ulozi do souboru a znovu se vytvori seznam surovin
            foreach (Recept recept in vsechno)
            {
                PrevedReceptNaRetezec(recept);
                File.AppendAllText(soubor, PrevedReceptNaRetezec(recept) + Environment.NewLine);

                foreach (Surovina surovina in recept.SeznamSurovin)
                {
                    if (!Surovina.ZjistiJestliJeSurovinaVSeznamu(surovina.Nazev))
                    {
                        Surovina.vsechno.Add(surovina);
                    }
                }
            }

            Console.WriteLine($"Recept '{receptKeSmazani.Nazev}' byl smazan");
        }

        public static void VypisSurovinyUReceptu()
        // Metoda vypise suroviny u receptu
        {
            Console.WriteLine("Zadejte nazev receptu, u ktereho chcete vypsat suroviny:");
            string vstup = Console.ReadLine();
            Recept recept;

            if (!ZjistiJestliJeReceptVSeznamu(vstup))
            {
                Console.WriteLine("Neznamy recept, neni mozne suroviny vypsat");
                return;
            }

            recept = NajdiReceptDleNazvu(vstup);
            Surovina.VypisSuroviny(recept.SeznamSurovin);
        }
    }
}