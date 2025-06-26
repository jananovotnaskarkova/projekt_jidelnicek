namespace ProjektJidelnicek
{
    public class Surovina
    {
        public string Nazev { get; private set; }
        public int Kategorie { get; private set; }

        public Surovina(string nazev, int kategorie)
        {
            Nazev = nazev;
            Kategorie = kategorie;
        }

        // Seznam vsech surovin
        public static List<Surovina> vsechno = [];

        // Slovnik obsahujici kategorie surovin
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

        // Kategorie surovin
        public static Kategorie kategorieSurovina = new Kategorie(slovnikSurovina);

        public static string oddelovac = "------------------------------------------------------";

        /// <summary>
        /// Metoda zjisti, jestli uz je surovina ulozena v seznamu.
        /// </summary>
        /// <param name="surovina"></param>
        /// <returns>
        /// true nebo false pokud je resp. neni surovina v seznamu
        /// </returns>
        public static bool ZjistiJestliJeSurovinaVSeznamu(string surovina)
        {
            return vsechno.Any(x => x.Nazev == surovina);
        }

        /// <summary>
        /// Metoda zjisti, do jake kategorie surovina patri.
        /// </summary>
        /// <param name="surovina"></param>
        /// <returns>
        /// cislo kategorie dane suroviny
        /// </returns>
        public static int ZjistiCisloKategorie(string surovina)
        {
            return vsechno.Where(x => x.Nazev == surovina).Select(x => x.Kategorie).First();
        }

        /// <summary>
        /// Metoda vypise suroviny dle kategorie.
        /// </summary>
        public static void VypisSurovinyDleKategorie()
        {
            kategorieSurovina.VypisKategorie();
            int cislo = kategorieSurovina.NactiCisloKategorie();
            VypisSuroviny(vsechno.Where(x => x.Kategorie == cislo).ToList());
        }

        /// <summary>
        /// Metoda vypise nazvy surovin na zadanem seznamu.
        /// </summary>
        /// <param name="seznam"></param>
        public static void VypisSuroviny(List<Surovina> seznam)
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

        /// <summary>
        /// Metoda prevede retezec obsahujici udaje o surovinach na seznam surovin.
        /// Ulozi surovinu na seznam vsech surovin, pokud tam jeste surovina neni.
        /// </summary>
        /// <param name="retezec"></param>
        /// <returns>
        /// seznam surovin
        /// </returns>
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

        /// <summary>
        /// Metoda roztridi suroviny u zadaneho receptu.
        /// Pokud uz je surovina na seznamu vsech surovin, tak najde cislo kategorie.
        /// Pokud surovina jeste neni na seznamu vsech surovin, tak se uzivatele zepta, do jake kategorie patri.
        /// </summary>
        /// <param name="suroviny"></param>
        /// <returns>
        /// seznam surovin
        /// </returns>
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