namespace ProjektJidelnicek;

class Program
{
    static void Main(string[] args)
    {
        Recept.NactiReceptyZeSouboru();
        Console.WriteLine("Vita vas program na planovani jidelnicku!");

        while (true)
        {
            Console.WriteLine("******************************************************");
            kategorieAkce.VypisKategorie();
            int cisloAkce = kategorieAkce.NactiCisloKategorie();
            switch (cisloAkce)
            {
                case 1:
                    Surovina.VypisSurovinyDleKategorie();
                    break;
                case 2:
                    Recept.VypisSurovinyUReceptu();
                    break;
                case 3:
                    Recept.VyhledejRecept();
                    break;
                case 4:
                    Recept.PridejRecept();
                    break;
                case 5:
                    Recept.SmazRecept();
                    break;
                case 6:
                    Jidlo.PridejJidlo();
                    break;
                case 7:
                    Jidlo.SmazJidlo();
                    break;
                case 8:
                    Jidlo.VypisInfo();
                    break;
                case 9:
                    return;
            }
        }
    }

    // Slovnik obsahujici akce
    private static Dictionary<string, int> slovnikAkce = new Dictionary<string, int>
        {
            { "vypsat suroviny dle kategorie", 1 },
            { "vypsat suroviny u receptu", 2 },
            { "vyhledat recept", 3 },
            { "pridat recept", 4 },
            { "smazat recept", 5 },
            { "pridat jidlo do jidelnicku", 6 },
            { "smazat jidlo z jidelnicku", 7 },
            { "vypsat info k jidelnicku", 8 },
            { "ukoncit program", 9 }
        };
    private static Kategorie kategorieAkce = new Kategorie(slovnikAkce);
}