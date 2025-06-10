using System.Security.Cryptography.X509Certificates;

namespace ProjektJidelnicek
{
    public class Recept
    {
        public string Nazev;
        public int Kategorie;
        public bool MaPrilohu;
        public List<Surovina> SeznamSurovin = [];

        public Recept(string nazev, int kategorie, bool maPrilohu, List<Surovina> seznam)
        {
            Nazev = nazev;
            Kategorie = kategorie;
            MaPrilohu = maPrilohu;
            SeznamSurovin = seznam;
        }

        // Seznam vsech jidel
        public static List<Recept> vsechno = [];

        // Slovnik obsahujici kategorie jidel
        private static Dictionary<string, int> slovnikRecept = new Dictionary<string, int>
        {
            { "jidlo s masem", 1},
            { "jidlo bez masa", 2},
            { "sladke jidlo", 3},
            { "priloha", 4},
        };

        // Kategorie jidel
        private static Kategorie kategorieRecept = new Kategorie(slovnikRecept);

        // Slovnik obsahujici kategorie priloh
        private static Dictionary<string, int> slovnikPriloha = new Dictionary<string, int>
        {
            { "ma prilohu", 1},
            { "nema prilohu", 2},
        };

        // Kategorie priloh
        private static Kategorie kategoriePriloha = new Kategorie(slovnikPriloha);

        // Cesta k souboru se seznamem jidel
        private static string soubor = @"C:\C_Sharp\czechitas_jaro_25\projekt_jidelnicek\ProjektJidelnicek\seznam";

        public static void NactiJidlaZeSouboru()
        // Metoda nacte jidla ulozena v souboru
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

        public static (bool JePlatny, string nazevJidla, List<string> seznamSurovin) ZkontrolujVstup(string vstup)
        //Metoda kontroluje vstup
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
        // Metoda se zepta na nazev jidla a suroviny, zjisti do jake kategorie recept patri, jestli ma prilohu a roztridi suroviny do kategorii
        {
            Console.WriteLine("Zadejte nove jidlo ve formatu: 'nazev,surovina1,surovina2,surovina3,surovina4,surovina5'");
            string vstup = Console.ReadLine();
            bool maPrilohu;

            (bool JePlatny, string nazevJidla, List<string> suroviny) = ZkontrolujVstup(vstup);
            if (!JePlatny)
            {
                Console.WriteLine("Jidlo nema vhodny format");
                return;
            }

            Console.WriteLine($"Do jake skupiny jidel patri '{nazevJidla}'?");
            kategorieRecept.VypisKategorie();
            int cisloKategorie = kategorieRecept.NactiCisloKategorie();

            if (cisloKategorie == 4)
            {
                maPrilohu = false;
            }
            else
            {
                Console.WriteLine($"Ma jidlo '{nazevJidla}' prilohu?");
                kategoriePriloha.VypisKategorie();
                int cisloPrilohy = kategoriePriloha.NactiCisloKategorie();
                maPrilohu = cisloPrilohy.Equals(1);
            }

            List<Surovina> seznamSurovin = Surovina.RoztridSuroviny(suroviny);

            Recept novyRecept = new(nazevJidla, cisloKategorie, maPrilohu, seznamSurovin);
            vsechno.Add(novyRecept);
            UlozReceptDoSouboru(novyRecept);
        }

        public static void VypisJidla(List<Recept> seznam)
        // Metoda vypise nazvy jidel na seznamu
        {
            foreach (string nazev in seznam.Select(x => x.Nazev))
            {
                Console.WriteLine(nazev);
            }
        }

        public static void UlozReceptDoSouboru(Recept recept)
        // Metoda ulozi recept do souboru
        {
            var surovinyPole = recept.SeznamSurovin.Select(x => String.Join('-', x.Nazev, x.Kategorie));
            var surovinyRetezec = String.Join(',', surovinyPole);
            File.AppendAllLines(soubor, [String.Join('|', recept.Nazev, recept.Kategorie, recept.MaPrilohu, surovinyRetezec)]);
        }

        public static bool ZjistiJestliJeReceptVSeznamu(string nazevReceptu)
        // Metoda zjisti, jestli uz je recept ulozene v seznamu
        {
            return vsechno.Any(x => x.Nazev == nazevReceptu);
        }

        public static Recept NajdiJidlo(string nazevReceptu)
        // Metoda vrati jidlo s danym nazvem
        {
            return vsechno.Where(x => x.Nazev == nazevReceptu).Select(x => x).ToList()[0];
        }

        public static Recept NajdiReceptDleNazvu(string nazevJidla)
        // Metoda najde recept v seznamu podle nazvu
        {
            return vsechno.Where(x => x.Nazev == nazevJidla).Select(x => x).ToList()[0];
        }

        public static void SmazRecept()
        {
            Console.WriteLine("Zadejte nazev jidla, ktere chcete smazat");
            string vstup = Console.ReadLine();
            Recept receptKeSmazani;

            bool JeVSeznamu = ZjistiJestliJeReceptVSeznamu(vstup);
            if (!JeVSeznamu)
            {
                Console.WriteLine("Nezname jidlo, neni mozne ho smazat");
                return;
            }

            receptKeSmazani = NajdiReceptDleNazvu(vstup);
            vsechno.Remove(receptKeSmazani);
            File.WriteAllLines(soubor, [string.Empty]);
            Surovina.vsechno = [];

            // aktualizovany seznam jidel se znovu ulozi do souboru
            foreach(Recept recept in vsechno)
            {
                UlozReceptDoSouboru(recept);
                foreach(Surovina surovina in recept.SeznamSurovin)
                {
                    if (!Surovina.ZjistiJestliJeSurovinaVSeznamu(surovina.Nazev))
                    {
                        Surovina.vsechno.Add(surovina);
                    }
                }
            }
        }
    }
}