namespace ProjektJidelnicek
{
    public class Jidlo
    {
        public string Nazev;
        public int Kategorie;
        public bool MaPrilohu;
        public List<Surovina> SeznamSurovin = [];

        public Jidlo(string nazev, int kategorie, bool maPrilohu, List<Surovina> seznam)
        {
            Nazev = nazev;
            Kategorie = kategorie;
            MaPrilohu = maPrilohu;
            SeznamSurovin = seznam;
        }

        // Seznam vsech jidel
        private static List<Jidlo> vsechno = [];

        // Slovnik obsahujici kategorie jidel
        private static Dictionary<string, int> slovnikJidlo = new Dictionary<string, int>
        {
            { "jidlo s masem", 1},
            { "jidlo bez masa", 2},
            { "sladke jidlo", 3},
            { "priloha", 4},
        };
        private static Kategorie kategorieJidlo = new Kategorie(slovnikJidlo);
        private static Dictionary<string, int> slovnikPriloha = new Dictionary<string, int>
        {
            { "ma prilohu", 1},
            { "nema prilohu", 2},
        };
        private static Kategorie kategoriePriloha = new Kategorie(slovnikPriloha);

        // Cesta k seznamu jidel
        private static string soubor = @"C:\C_Sharp\czechitas_jaro_25\projekt_jidelnicek\ProjektJidelnicek\seznam";

        public static void NactiJidlaZeSouboru()
        // Metoda nacte jidla ulozena v souboru
        {
            string[] obsahSouboru = File.ReadAllLines(soubor);
            foreach (string radek in obsahSouboru)
            {
                string[] rozdelenyRadek = radek.Split('|');
                string[] suroviny = rozdelenyRadek[3].Split(',');

                List<Surovina> seznamSurovin = [];
                foreach (string surovina in suroviny)
                {
                    string[] rozdelenaSurovina = surovina.Split('-');
                    Surovina novaSurovina = new(rozdelenaSurovina[0], int.Parse(rozdelenaSurovina[1]));
                    seznamSurovin.Add(novaSurovina);
                    if (!Surovina.ZjistiJestliJeSurovinaVSeznamu(rozdelenaSurovina[0]))
                    {
                        Surovina.vsechno.Add(novaSurovina);
                    }
                }

                Jidlo noveJidlo = new Jidlo(rozdelenyRadek[0],
                                            int.Parse(rozdelenyRadek[1]),
                                            bool.Parse(rozdelenyRadek[2]),
                                            seznamSurovin
                                            );
                vsechno.Add(noveJidlo);
            }
        }

        public static (bool JePlatny, string nazevJidla, List<string> seznamSurovin) ZkontrolujVstup(string vstup)
        //Metoda kontroluje vstup
        {
            bool jePlatny = false;
            string[] rozdelenyVstup = vstup.Split(',');
            List<string> seznamSurovin = [];

            if (rozdelenyVstup.Count() >= 2)
            {
                foreach (string s in rozdelenyVstup.TakeLast(rozdelenyVstup.Length - 1).ToList())
                {
                    seznamSurovin = [.. seznamSurovin, s];
                }
                jePlatny = true;
            }
            return (jePlatny, rozdelenyVstup[0], seznamSurovin);
        }

        public static void PridejJidlo()
        // Metoda se zepta na nazev jidla a suroviny, zjisti do jake kategorie jidlo patri, jestli ma prilohu a roztridi suroviny do kategorii
        {
            Console.WriteLine("Zadejte nove jidlo ve formatu: 'nazev,surovina1,surovina2,surovina3,surovina4,surovina5'");
            string vstup = Console.ReadLine();

            (bool JePlatny, string nazevJidla, List<string> suroviny) = ZkontrolujVstup(vstup);
            if (!JePlatny)
            {
                Console.WriteLine("Jidlo nema vhodny format");
                return;
            }

            Console.WriteLine($"Do jake skupiny jidel patri '{nazevJidla}'?");
            kategorieJidlo.VypisKategorie();
            int cisloKategorie = kategorieJidlo.NactiCisloKategorie();

            Console.WriteLine($"Ma jidlo '{nazevJidla}' prilohu?");
            kategoriePriloha.VypisKategorie();
            int cisloPrilohy = kategoriePriloha.NactiCisloKategorie();
            bool maPrilohu = cisloPrilohy.Equals(1);

            List<Surovina> seznamSurovin = [];
            foreach (string surovina in suroviny)
            {
                Surovina novaSurovina;
                if (!Surovina.ZjistiJestliJeSurovinaVSeznamu(surovina))
                {
                    Console.WriteLine($"Do jake skupiny surovin patri '{surovina}'?");
                    Surovina.kategorieSurovina.VypisKategorie();
                    int cisloSuroviny = Surovina.kategorieSurovina.NactiCisloKategorie();
                    novaSurovina = new(surovina, cisloSuroviny);
                    Surovina.vsechno.Add(novaSurovina);
                }
                else
                {
                    novaSurovina = new(surovina, Surovina.ZjistiCisloKategorie(surovina));
                }
                seznamSurovin.Add(novaSurovina);
            }
            Jidlo noveJidlo = new(nazevJidla, cisloKategorie, maPrilohu, seznamSurovin);
            vsechno.Add(noveJidlo);
            UlozJidloDoSouboru(noveJidlo);
        }

        public static void VypisJidla()
        // Metoda vypise nazvy jidel na seznamu
        {
            foreach (string nazev in vsechno.Select(x => x.Nazev))
            {
                Console.WriteLine(nazev);
            }
        }

        public static void UlozJidloDoSouboru(Jidlo jidlo)
        // Metoda ulozi jidlo do souboru
        {
            var surovinyPole = jidlo.SeznamSurovin.Select(x => String.Join('-', x.Nazev, x.Kategorie));
            var surovinyRetezec = String.Join(',', surovinyPole);
            File.AppendAllLines(soubor, [String.Join('|', jidlo.Nazev, jidlo.Kategorie, jidlo.MaPrilohu, surovinyRetezec)]);
        }

        
    }
}