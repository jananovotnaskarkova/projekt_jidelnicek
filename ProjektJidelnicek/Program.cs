namespace ProjektJidelnicek;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Vita vas program na planovani jidelnicku!");
        Jidlo.NactiJidlaZeSouboru();
        Jidlo.PridejJidlo();
        
        Jidlo.VypisJidla();
        Surovina.VypisSuroviny();

        while (true)
        {

        }
    }
}