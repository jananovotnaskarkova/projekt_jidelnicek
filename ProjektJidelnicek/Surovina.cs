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
            { "mleko", 2 },
            { "obiloviny", 3 },
            { "zelenina", 4 },
            { "ovoce", 5 },
            { "lusteniny", 6 },
            { "koreni", 7 },
            { "ostatni", 8 }
        };
        public static Kategorie kategorieSurovina = new Kategorie(slovnikSurovina);

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

        public static void VypisSuroviny()
        // Metoda vypise suroviny na seznamu
        {
            foreach (string nazev in vsechno.Select(x => x.Nazev))
            {
                Console.WriteLine(nazev);
            }
        }
    }
}