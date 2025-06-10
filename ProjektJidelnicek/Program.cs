namespace ProjektJidelnicek;

class Program
{
    static void Main(string[] args)
    {
        Recept.NactiReceptyZeSouboru();
        Console.WriteLine("Vita vas program na planovani jidelnicku!");

        while (true)
        {
            kategorieAkce.VypisKategorie();
            int cisloAkce = kategorieAkce.NactiCisloKategorie();
            switch (cisloAkce)
            {
                case 1:
                    Recept.PridejRecept();
                    break;
                case 2:
                    Recept.SmazRecept();
                    break;
                case 3:
                    // Jidlo.VyhledejJidlo();
                    Console.WriteLine("Vyhledani jidla bude pridano");
                    break;
                case 4:
                    // Jidlo.VypisJidla(); mozna staci vyhledavani, i tam se muze vyhledavat dle kategorii
                    Console.WriteLine("Vypsani jidel bude pridano");
                    break;
                case 5:
                    Jidelnicek.PridejJidlo();
                    break;
                case 6:
                    // Jidelnicek.SmazJidlo();
                    Console.WriteLine("Smazani jidla z jidelnicku bude pridano");
                    break;
                case 7:
                    Jidelnicek.VypisInfo();
                    break;
                case 8:
                    return;
            }
        }
    }

    // Slovnik obsahujici akce
    public static Dictionary<string, int> slovnikAkce = new Dictionary<string, int>
        {
            { "pridat jidlo", 1 },
            { "smazat jidlo", 2 },
            { "vyhledat jidlo", 3 },
            { "vypsat jidla v kategorii", 4 },
            { "pridat jidlo do jidelnicku", 5 },
            { "odebrat jidlo z jidelnicku", 6 },
            { "vypsat info k jidelnicku", 7 },
            { "ukoncit program", 8 }
        };
    private static Kategorie kategorieAkce = new Kategorie(slovnikAkce);
}