namespace ProjektJidelnicek
{
    public class Kategorie
    {
        public Dictionary<string, int> Slovnik {get; }
        public Kategorie(Dictionary<string, int> slovnik)
        {
            Slovnik = slovnik;
        }

        /// <summary>
        /// Metoda pro nacteni cisla kategorie.
        /// </summary>
        /// <returns>
        /// cislo kategorie
        /// </returns>
        public int NactiCisloKategorie()
        {
            bool platnyVstup = false;
            int cisloKategorie = 0;

            while (!platnyVstup)
            {
                if (int.TryParse(Console.ReadLine(), out cisloKategorie))
                {
                    if ((cisloKategorie > 0) && (cisloKategorie <= Slovnik.Count))
                    {
                        platnyVstup = true;
                    }
                    else
                    {
                        Console.WriteLine("Neplatne cislo kategorie");
                    }
                }
                else
                {
                    Console.WriteLine("Neplatne cislo kategorie");
                }
            }
            return cisloKategorie;
        }

        /// <summary>
        /// Metoda vypise kategorie ve forme menu (cislo: nazev kategorie).
        /// </summary>
        public void VypisKategorie()
        {
            Console.WriteLine("Napiste cislo pro jednu z nasledujich moznosti:");
            string vystup = "";
            foreach (var s in Slovnik)
            {
                vystup = string.Join(", ", Slovnik.Select(s => s.Value + ": " + s.Key));
            }
            Console.WriteLine(vystup);
        }
    }
}