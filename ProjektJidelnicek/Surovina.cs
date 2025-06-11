namespace ProjektJidelnicek
{
    public class Surovina
    {
        public string Nazev { get; }
        public int Kategorie { get; }

        public Surovina(string nazev, int kategorie)
        {
            Nazev = nazev;
            Kategorie = kategorie;
        }

        // Seznam vsech jidel
        public static List<Surovina> vsechno = [];

        // Slovnik obsahujici vsechny kategorie surovin
        private static Dictionary<string, int> slovnikSurovina = new Dictionary<string, int>
        {
            { "maso", 1 },
            { "mleko a vejce", 2 },
            { "obiloviny", 3 },
            { "zelenina", 4 },
            { "ovoce", 5 },
            { "lusteniny", 6 },
            { "koreni", 7 },
            { "suche plody", 8 },
            { "ostatni", 9 },
            { "vyrobky", 10 },
        };
        public static Kategorie kategorieSurovina = new Kategorie(slovnikSurovina);

        public static string oddelovac = "------------------------------------------------------";

        public static bool ZjistiJestliJeSurovinaVSeznamu(string surovina)
        // Metoda zjisti, jestli uz je surovina ulozena v seznamu
        {
            return vsechno.Any(x => x.Nazev == surovina);
        }

        public static int ZjistiCisloKategorie(string surovina)
        // Metoda zjisti do jake kategorie surovina patrid
        {
            return vsechno.Where(x => x.Nazev == surovina).Select(x => x.Kategorie).ToList()[0];
        }

        public static void VypisSurovinyDleKategorie()
        // Metoda vypise suroviny dle kategorie
        {
            Console.WriteLine("Zadejte cislo kategorie:");
            kategorieSurovina.VypisKategorie();
            int cislo = kategorieSurovina.NactiCisloKategorie();
            VypisSuroviny(vsechno.Where(x => x.Kategorie == cislo).Select(x => x).ToList());
        }

        public static void VypisSuroviny(List<Surovina> seznam)
        // Metoda vypise nazvy surovin na seznamu
        {
            Console.WriteLine(oddelovac);
            if (seznam.Count == 0)
            {
                Console.WriteLine("Suroviny odpovidajici zadanym kriteriim nenalezeny");
            }
            foreach (string nazev in seznam.Select(x => x.Nazev))
                {
                    Console.WriteLine(nazev);
                }
            Console.WriteLine(oddelovac);
        }

        public static List<Surovina> PrevedRetezecNaSuroviny(string retezec)
        {
            string[] suroviny = retezec.Split(',');
            List<Surovina> seznamSurovin = [];
            foreach (string surovina in suroviny)
            {
                string[] rozdelenaSurovina = surovina.Split('-');
                Surovina novaSurovina = new(rozdelenaSurovina[0], int.Parse(rozdelenaSurovina[1]));
                seznamSurovin.Add(novaSurovina);
                if (!ZjistiJestliJeSurovinaVSeznamu(rozdelenaSurovina[0]))
                {
                    vsechno.Add(novaSurovina);
                }
            }
            return seznamSurovin;
        }

        public static List<Surovina> RoztridSuroviny(List<string> suroviny)
        {
            List<Surovina> seznamSurovin = [];
            foreach (string surovina in suroviny)
            {
                Surovina novaSurovina;
                if (!ZjistiJestliJeSurovinaVSeznamu(surovina))
                {
                    Console.WriteLine($"Do jake skupiny surovin patri '{surovina}'?");
                    kategorieSurovina.VypisKategorie();
                    int cisloSuroviny = kategorieSurovina.NactiCisloKategorie();
                    novaSurovina = new(surovina, cisloSuroviny);
                    vsechno.Add(novaSurovina);
                }
                else
                {
                    novaSurovina = new(surovina, ZjistiCisloKategorie(surovina));
                }
                seznamSurovin.Add(novaSurovina);
            }
            return seznamSurovin;
        }
    }
}